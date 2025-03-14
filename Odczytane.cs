using MailKit.Net.Imap;
using MailKit.Search;
using MailKit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.IO;
using Org.BouncyCastle.Crypto;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using System.Reflection.Emit;

namespace JtK_Poczta
{
    public partial class Odczytane : Form
    {
        public string email;
        public string haslo;
        public string mailServer;
        public string imap;

        public Odczytane()
        {
            InitializeComponent();
            dgvOdczytane.RowTemplate.Height = 40;
            dgvOdczytane.AllowUserToAddRows = false;
            SetFormResolution();
        }

        private void SetFormResolution()
        {
            Screen primaryScreen = Screen.PrimaryScreen;
            int screenWidth = primaryScreen.Bounds.Width;

            int newFormWidth;
            int newFormHeight;

            if (screenWidth <= 1360)
            {
                newFormWidth = 1000;
                newFormHeight = 550;

                this.Width = newFormWidth;
                this.Height = newFormHeight;

                groupBox1.Width = 170;
                gBNapisz.Width = 170;
                btnNapisz.Width = 140;
                btnNowe.Width = 140;
                btnOdebrane.Width = 140;
                btnWyslane.Width = 140;
                btnKosz.Width = 140;

                Font comicSansFont = new Font("Comic Sans MS", 12, FontStyle.Regular);
                btnNapisz.Font = comicSansFont;
                btnNapisz.TextAlign = ContentAlignment.MiddleRight;
                btnNowe.Font = comicSansFont;
                btnNowe.TextAlign = ContentAlignment.MiddleRight;
                btnOdebrane.Font = comicSansFont;
                btnOdebrane.TextAlign = ContentAlignment.MiddleRight;
                btnWyslane.Font = comicSansFont;
                btnWyslane.TextAlign = ContentAlignment.MiddleRight;
                btnKosz.Font = comicSansFont;
                btnKosz.TextAlign = ContentAlignment.MiddleRight;

                //prawa
                gBox.Location = new Point(200, 12);
                gBox.Width = 750;
                gBox.Height = 475;
                label5.Location = new Point(150, 30);
                txtSzukaj.Location = new Point(240, 30);
                txtSzukaj.Width = 350;
                panel5.Location = new Point(240, 72);
                panel5.Width = 350;
                dgvOdczytane.Width = 725;
                dgvOdczytane.Height = 360;
                Nazwa.Width = 425;
                Data.Width = 150;
            }
            this.StartPosition = FormStartPosition.WindowsDefaultLocation;
        }

        private void dgvOdczytane_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string[] lines = File.ReadAllLines("Data\\daneUzytkownika.txt");

                if (lines.Length >= 3)
                {
                    // Przypisz pierwszą i drugą linię do zmiennych
                    email = lines[0];
                    haslo = lines[1];
                    mailServer = lines[2];
                    if (mailServer == "Gmail")
                    {
                        imap = "imap.gmail.com";
                    }
                    else if (mailServer == "WP")
                    {
                        imap = "imap.wp.pl";
                    }
                    else if (mailServer == "Interia")
                    {
                        imap = "poczta.interia.pl";
                    }
                    else if (mailServer == "Onet")
                    {
                        imap = "imap.poczta.onet.pl";
                    }

                    int port = 993; // Domyślny port IMAP
                    bool useSsl = true;

                    using (var client = new ImapClient())
                    {
                        client.ServerCertificateValidationCallback = (s, c, h, certError) => true; // Ignorowanie weryfikacji certyfikatu SSL/TLS

                        client.Connect(imap, port, useSsl);

                        client.Authenticate(email, haslo);

                        var inbox = client.Inbox;
                        inbox.Open(FolderAccess.ReadWrite);

                        string position = dgvOdczytane.Rows[e.RowIndex].Cells[0].Value.ToString();
                        var search = inbox.Search(SearchQuery.SubjectContains(position));

                        if (search.Count > 0)
                        {
                            var message = inbox.GetMessage(search[0]);
                            string to = message.From.ToString();
                            string from = message.To.ToString();
                            string subject = message.Subject;
                            string body = message.TextBody;

                            Wiadomosc wiadomoscWindow = new Wiadomosc(from, to, subject, body);
                            wiadomoscWindow.Show();
                            inbox.Close();
                            this.Hide();
                        }
                        else
                        {
                            // Obsługa sytuacji, gdy nie znaleziono pasujących wiadomości
                            MessageBox.Show("Błąd. Uruchom ponownie aplikację.\n:(", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void btnNapisz_Click(object sender, EventArgs e)
        {
            Wysylanie ft = new Wysylanie();
            ft.Location = this.Location;
            ft.StartPosition = FormStartPosition.Manual;
            ft.FormClosing += delegate { this.Show(); };
            ft.Show();
            this.Hide();
        }

        private void btnNowe_Click(object sender, EventArgs e)
        {
            Glowna ft = new Glowna();
            ft.Location = this.Location;
            ft.StartPosition = FormStartPosition.Manual;
            ft.FormClosing += delegate { this.Show(); };
            ft.Show();
            this.Hide();
        }

        private void btnWyslane_Click(object sender, EventArgs e)
        {
            Wyslane ft = new Wyslane();
            ft.Location = this.Location;
            ft.StartPosition = FormStartPosition.Manual;
            ft.FormClosing += delegate { this.Show(); };
            ft.Show();
            this.Hide();
        }

        private void btnKosz_Click(object sender, EventArgs e)
        {
            Kosz ft = new Kosz();
            ft.Location = this.Location;
            ft.StartPosition = FormStartPosition.Manual;
            ft.FormClosing += delegate { this.Show(); };
            ft.Show();
            this.Hide();
        }

        private void Odczytane_Load(object sender, EventArgs e)
        {
            string imap = "";
            try
            {
                // Odczytaj wszystkie linie z pliku tekst.txt
                string[] lines = File.ReadAllLines("Data\\daneUzytkownika.txt");

                // Sprawdź, czy plik zawiera co najmniej dwie linie
                if (lines.Length >= 3)
                {
                    // Przypisz pierwszą i drugą linię do zmiennych
                    email = lines[0];
                    haslo = lines[1];
                    mailServer = lines[2];
                    if (mailServer == "Gmail")
                    {
                        imap = "imap.gmail.com";
                    }
                    else if (mailServer == "WP")
                    {
                        imap = "imap.wp.pl";
                    }
                    else if (mailServer == "Interia")
                    {
                        imap = "poczta.interia.pl";
                    }
                    else if (mailServer == "Onet")
                    {
                        imap = "imap.poczta.onet.pl";
                    }

                    DataGridViewButtonColumn deleteButtonColumn = new DataGridViewButtonColumn();
                    deleteButtonColumn.HeaderText = "Usuń";
                    deleteButtonColumn.Name = "deleteButtonColumn";
                    deleteButtonColumn.Text = "Usuń";
                    deleteButtonColumn.UseColumnTextForButtonValue = true;
                    dgvOdczytane.Columns.Add(deleteButtonColumn);

                    int port = 993; // Domyślny port IMAP
                    bool useSsl = true;

                    using (var client = new ImapClient())
                    {
                        client.ServerCertificateValidationCallback = (s, c, h, certError) => true; // Ignorowanie weryfikacji certyfikatu SSL/TLS

                        client.Connect(imap, port, useSsl);

                        client.Authenticate(email, haslo);

                        var inbox = client.Inbox;
                        inbox.Open(FolderAccess.ReadWrite);

                        for (int i = 0; i < inbox.Count; i++)
                        {
                            var uniqueId = inbox.Search(SearchQuery.All)[i];
                            var summary = inbox.Fetch(new[] { uniqueId }, MessageSummaryItems.UniqueId | MessageSummaryItems.Full | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags).FirstOrDefault();

                            if (summary != null && summary.Flags.Value.HasFlag(MessageFlags.Seen))
                            {
                                inbox.AddFlags(uniqueId, MessageFlags.Seen, true);

                                var message = inbox.GetMessage(uniqueId);

                                dgvOdczytane.Rows.Add(message.Subject, message.Date.DateTime.ToString());
                            }
                        }
                        client.Disconnect(true);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Wystąpił błąd: " + ex.Message);
            }
        }

        private void btnZamknij_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void dgvOdczytane_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dgvOdczytane.Columns[e.ColumnIndex].Name == "deleteButtonColumn" && e.RowIndex >= 0)
            {
                // Odczytaj wszystkie linie z pliku tekst.txt
                string[] lines = File.ReadAllLines("Data\\daneUzytkownika.txt");

                // Sprawdź, czy plik zawiera co najmniej dwie linie
                if (lines.Length >= 3)
                {
                    // Przypisz pierwszą i drugą linię do zmiennych
                    email = lines[0];
                    haslo = lines[1];
                    mailServer = lines[2];
                    if (mailServer == "Gmail")
                    {
                        imap = "imap.gmail.com";
                    }
                    else if (mailServer == "WP")
                    {
                        imap = "imap.wp.pl";
                    }
                    else if (mailServer == "Interia")
                    {
                        imap = "poczta.interia.pl";
                    }
                    else if (mailServer == "Onet")
                    {
                        imap = "imap.poczta.onet.pl";
                    }

                    int port = 993; // Domyślny port IMAP
                    bool useSsl = true;

                    using (var client = new ImapClient())
                    {
                        client.ServerCertificateValidationCallback = (s, c, h, certError) => true; // Ignorowanie weryfikacji certyfikatu SSL/TLS

                        client.Connect(imap, port, useSsl);

                        client.Authenticate(email, haslo);

                        var inbox = client.Inbox;
                        inbox.Open(FolderAccess.ReadWrite);

                        string subjectToDelete = dgvOdczytane.Rows[e.RowIndex].Cells[0].Value.ToString();

                        var searchResults = inbox.Search(SearchQuery.SubjectContains(subjectToDelete));

                        // Zapisanie zmian w folderze
                        inbox.Expunge();

                        // Oznaczenie wiadomości jako do usunięcia (jeśli nie zostało to już zrobione)
                        inbox.AddFlags(searchResults, MessageFlags.Deleted, true);
                        inbox.Close();

                        // Przeniesienie wiadomości do folderu "Kosz"
                        var trash = client.GetFolder(SpecialFolder.Trash);
                        trash.Open(FolderAccess.ReadWrite);

                        // Skopiowanie wiadomości do folderu "Kosz"
                        inbox.Open(FolderAccess.ReadWrite);
                        inbox.MoveTo(searchResults, trash);

                        // Zamykanie folderów
                        inbox.Close();
                    }
                }
                else
                {
                    Console.WriteLine("Plik tekst.txt nie zawiera co najmniej dwóch linii.");
                }

                // Usunięcie wiersza na podstawie indeksu wiersza
               dgvOdczytane.Rows.RemoveAt(e.RowIndex);
            }
        }

        private void txtSzukaj_TextChanged(object sender, EventArgs e)
        {
            string szukanaFraza = txtSzukaj.Text.ToLower();

            dgvOdczytane.ClearSelection();

            if (string.IsNullOrWhiteSpace(szukanaFraza))
            {

                for (int i = 0; i < dgvOdczytane.Rows.Count; i++)
                {
                    dgvOdczytane.Rows[i].Visible = true;
                }
            }
            else
            {

                for (int i = 0; i < dgvOdczytane.Rows.Count; i++)
                {
                    bool pasuje = false;

                    foreach (DataGridViewCell cell in dgvOdczytane.Rows[i].Cells)
                    {
                        if (cell.Value != null && cell.Value.ToString().ToLower().Contains(szukanaFraza))
                        {
                            pasuje = true;
                            break;
                        }
                    }

                    dgvOdczytane.Rows[i].Visible = pasuje;
                }
            }
        }

        private void Odczytane_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
