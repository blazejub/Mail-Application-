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
using MailKit.Net.Imap;
using System.IO;
using Org.BouncyCastle.Crypto;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using System.Reflection.Emit;

namespace JtK_Poczta
{
    public partial class Kosz : Form
    {
        public Kosz()
        {
            InitializeComponent();
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
                dgvKosz.Width = 725;
                dgvKosz.Height = 360;
                Nazwa.Width = 525;
                Data.Width = 150;
            }
            this.StartPosition = FormStartPosition.WindowsDefaultLocation;
        }

        private void Kosz_Load(object sender, EventArgs e)
        {
            dgvKosz.RowTemplate.Height = 40;
            dgvKosz.AllowUserToAddRows = false;

            string[] lines = File.ReadAllLines("Data\\daneUzytkownika.txt");

            string email = "";
            string haslo = "";
            string mailServer = "";
            string imap = "";

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

                            var trash = client.GetFolder(SpecialFolder.Trash);
                            trash.Open(FolderAccess.ReadOnly);

                            // Pobranie wiadomości z folderu "Kosz"
                            for (int i = 0; i < trash.Count; i++)
                            {
                                var uniqueId = trash.Search(SearchQuery.All)[i];
                                var message = trash.GetMessage(uniqueId);

                                dgvKosz.Rows.Add(message.Subject, message.Date.DateTime.ToString());
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
            SetFormResolution();
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

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string szukanaFraza = txtSzukaj.Text.ToLower();

            dgvKosz.ClearSelection();

            if (string.IsNullOrWhiteSpace(szukanaFraza))
            {

                for (int i = 0; i < dgvKosz.Rows.Count; i++)
                {
                    dgvKosz.Rows[i].Visible = true;
                }
            }
            else
            {

                for (int i = 0; i < dgvKosz.Rows.Count; i++)
                {
                    bool pasuje = false;

                    foreach (DataGridViewCell cell in dgvKosz.Rows[i].Cells)
                    {
                        if (cell.Value != null && cell.Value.ToString().ToLower().Contains(szukanaFraza))
                        {
                            pasuje = true;
                            break;
                        }
                    }

                    dgvKosz.Rows[i].Visible = pasuje;
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

        private void btnOdebrane_Click(object sender, EventArgs e)
        {
            Odczytane ft = new Odczytane();
            ft.Location = this.Location;
            ft.StartPosition = FormStartPosition.Manual;
            ft.FormClosing += delegate { this.Show(); };
            ft.Show();
            this.Hide();
        }

        private void Kosz_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
