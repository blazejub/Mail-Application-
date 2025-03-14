using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JtK_Poczta
{
    public partial class Login : Form
    {
        public string nowyEmail = "";
        public string noweHaslo = "";
        public string nowyImap = "";
        public Login()
        {
            InitializeComponent();
        }

        private void btnZamknij_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnZaloguj_Click(object sender, EventArgs e)
        {

             nowyEmail = txtLogin.Text;
             noweHaslo = txtHaslo.Text;
            nowyImap = cmbImap.Text;
            try
            {
                // Zapisz nowe wartości do pliku tekst.txt
                using (StreamWriter sw = new StreamWriter("Data\\daneUzytkownika.txt"))
                {
                    sw.WriteLine(nowyEmail);
                    sw.WriteLine(noweHaslo);
                    sw.WriteLine(nowyImap);
                }

                Console.WriteLine("Dane zostały zaktualizowane w pliku tekst.txt.");
                Glowna ft = new Glowna();
                ft.Location = this.Location;
                ft.StartPosition = FormStartPosition.Manual;
                ft.FormClosing += delegate { this.Show(); };
                ft.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Wystąpił błąd: " + ex.Message);
                MessageBox.Show("Nieprawidłowy login lub hasło", "Błąd logowania", MessageBoxButtons.OK);
               
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}