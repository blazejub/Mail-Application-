using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using Org.BouncyCastle.Crypto;
using System;
using System.Drawing;
using System.IO;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace JtK_Poczta
{
    public partial class Wiadomosc : Form
    {
        private int realMessageId;
        public string email;
        public string haslo;
        public string mailServer;
        public string imap;

        public Wiadomosc(string from, string to, string subject, string body)
        {
            InitializeComponent();
            string[] lines = File.ReadAllLines("Data\\daneUzytkownika.txt");

            txtOd.Text = to;
            txtDo.Text = from;
            txtTemat.Text = subject;
            txtWiadomosc.Text = body;

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

                    var search = inbox.Search(SearchQuery.SubjectContains(txtTemat.Text));
                    inbox.AddFlags(search, MessageFlags.Seen, true);
                }
            }
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
                panel1.Width = 710;
                panel1.Height = 425;

                label5.Location = new Point(15, 10);
                txtOd.Width = 600;
                txtOd.Location = new Point(65, 10);

                label7.Location = new Point(15, 60);
                txtDo.Width = 600;
                txtDo.Location = new Point(65, 60);

                label8.Location = new Point(15, 110);
                txtTemat.Width = 600;
                txtTemat.Location = new Point(65, 110);

                txtWiadomosc.Location = new Point(15, 160);
                txtWiadomosc.Width = 675;
                txtWiadomosc.Height = 250;

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

        private void Wiadomosc_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
