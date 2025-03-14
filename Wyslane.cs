using MailKit.Net.Imap;
using MailKit.Search;
using MailKit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Org.BouncyCastle.Crypto;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using System.Reflection.Emit;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Security.Policy;

namespace JtK_Poczta
{
    public partial class Wyslane : Form
    {
        string email;
        string haslo;
        string mailServer;
        string imap;

        public Wyslane()
        {
            InitializeComponent();
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
                dgvWyslane.Width = 725;
                dgvWyslane.Height = 360;
                Nazwa.Width = 525;
                Data.Width = 150;
            }
            this.StartPosition = FormStartPosition.WindowsDefaultLocation;
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

        private void btnOdebrane_Click(object sender, EventArgs e)
        {
            Odczytane ft = new Odczytane();
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

        private void btnZamknij_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Wyslane_Load(object sender, EventArgs e)
        {
            dgvWyslane.RowTemplate.Height = 40;
            dgvWyslane.AllowUserToAddRows = false;

            string[] lines = File.ReadAllLines("Data\\daneUzytkownika.txt");

            email = "";
            haslo = "";
            mailServer = "";
            imap = "";

            // Sprawdź, czy plik zawiera co najmniej trzy linie
            if (lines.Length >= 3)
            {
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

                try
                {
                    // Sprawdź, czy plik zawiera co najmniej dwie linie
                    if (lines.Length >= 2)
                    {
                        // Przypisz pierwszą i drugą linię do zmiennych
                        email = lines[0];
                        haslo = lines[1];

                        int port = 993; // Domyślny port IMAP
                        bool useSsl = true;

                        using (var client = new ImapClient())
                        {
                            client.ServerCertificateValidationCallback = (s, c, h, certError) => true; // Ignorowanie weryfikacji certyfikatu SSL/TLS

                            client.Connect(imap, port, useSsl);

                            client.Authenticate(email, haslo);

                            var sentFolder = client.GetFolder(SpecialFolder.Sent);
                            sentFolder.Open(FolderAccess.ReadOnly);

                            // Pobranie wiadomości z folderu "Kosz"
                            for (int i = 0; i < sentFolder.Count; i++)
                            {
                                var uniqueId = sentFolder.Search(SearchQuery.All)[i];
                                var message = sentFolder.GetMessage(uniqueId);

                                dgvWyslane.Rows.Add(message.Subject, message.Date.DateTime.ToString());
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Plik tekst.txt nie zawiera co najmniej dwóch linii.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Wystąpił błąd: " + ex.Message);
                }
            }
        }

        private void dgvWyslane_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
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

                        //var inbox = client.Inbox;
                        var sentFolder = client.GetFolder(SpecialFolder.Sent);
                        sentFolder.Open(FolderAccess.ReadWrite);
                        

                        string position = dgvWyslane.Rows[e.RowIndex].Cells[0].Value.ToString();
                        var search = sentFolder.Search(SearchQuery.SubjectContains(position));

                        if (search.Count > 0)
                        {
                            var message = sentFolder.GetMessage(search[0]);
                            string to = message.From.ToString();
                            string from = message.To.ToString();
                            string subject = message.Subject;
                            string body = message.TextBody;

                            Wiadomosc wiadomoscWindow = new Wiadomosc(from, to, subject, body);
                            wiadomoscWindow.Show();
                            sentFolder.Close();
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

        private void txtSzukaj_TextChanged(object sender, EventArgs e)
        {
            string szukanaFraza = txtSzukaj.Text.ToLower(); 

            dgvWyslane.ClearSelection();

            if (string.IsNullOrWhiteSpace(szukanaFraza))
            {
                
                for (int i = 0; i < dgvWyslane.Rows.Count; i++)
                {
                    dgvWyslane.Rows[i].Visible = true;
                }
            }
            else
            {
               
                for (int i = 0; i < dgvWyslane.Rows.Count; i++)
                {
                    bool pasuje = false;

                    foreach (DataGridViewCell cell in dgvWyslane.Rows[i].Cells)
                    {
                        if (cell.Value != null && cell.Value.ToString().ToLower().Contains(szukanaFraza))
                        {
                            pasuje = true;
                            break;
                        }
                    }

                    dgvWyslane.Rows[i].Visible = pasuje;
                }
            }
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

        private void Wyslane_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
