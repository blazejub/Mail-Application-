using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Security.Policy;
using System.IO;
using MailKit;
using MimeKit;
using MailKit.Net.Imap;
using System.Runtime.Remoting.Messaging;

namespace JtK_Poczta
{
    public partial class Wysylanie : Form
    {
        public Wysylanie()
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
                panel1.Width = 710;
                panel1.Height = 375;

                label5.Location = new Point(15, 10);
                txtOd.Width = 600;
                txtOd.Location = new Point(65, 10);

                label7.Location = new Point(15, 60);
                txtDo.Width = 600;
                txtDo.Location = new Point(65, 60);

                label8.Location = new Point(15, 110);
                txtTemat.Width = 600;
                txtTemat.Location = new Point(100, 110);

                txtWiadomosc.Location = new Point(15, 160);
                txtWiadomosc.Width = 675;
                txtWiadomosc.Height = 200;

                btnWyslij.Location = new Point(15, 425);

            }
            this.StartPosition = FormStartPosition.WindowsDefaultLocation;
        }

        private void Wysylanie_Load(object sender, EventArgs e)
        {
            string[] lines = File.ReadAllLines("Data\\daneUzytkownika.txt");

            // Sprawdź, czy plik zawiera co najmniej dwie linie
            if (lines.Length >= 2)
            {
                // Przypisz pierwszą i drugą linię do zmiennych
                string email = lines[0];
            
                txtOd.Text = email;
            }
        }

        private void btnZamknij_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnWyslij_Click(object sender, EventArgs e)
        {
            
            string doAdres = txtDo.Text;
            string temat = txtTemat.Text;
            string wiadomosc = txtWiadomosc.Text;
            string email ="";
            string haslo ="";
            string mailServer = "";
            string imap = "";

            string[] lines = File.ReadAllLines("Data\\daneUzytkownika.txt");


            if (lines.Length >= 3)
            {

                email = lines[0];
                haslo = lines[1];
                mailServer = lines[2];
                if (mailServer == "Gmail")
                {
                    imap = "smtp.gmail.com";
                }
                else if (mailServer == "WP")
                {
                    imap = "smtp.wp.pl";
                }
                else if (mailServer == "Interia")
                {
                    imap = "poczta.interia.pl";
                }
                else if (mailServer == "Onet")
                {
                    imap = "smtp.poczta.onet.pl";
                }
            }

            try
            {
                using (SmtpClient client = new SmtpClient(imap))
                {
                    //Ten port SMTP będzie odpowiedni dla wp.pl

                    client.Port = 587; // Port SMTP
                    client.Credentials = new NetworkCredential(email, haslo);
                    client.EnableSsl = true;

                    // Tworzenie wiadomości e-mail
                    MailMessage message = new MailMessage(email, doAdres, temat, wiadomosc);

                    // Wysłanie wiadomości
                    client.Send(message);

                    MessageBox.Show("Wiadomość została wysłana!");

                    txtDo.Text = "";
                    txtTemat.Text = "";
                    txtWiadomosc.Text = "";
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Wystąpił błąd podczas wysyłania wiadomości: " + ex.Message);
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

        private void Wysylanie_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
