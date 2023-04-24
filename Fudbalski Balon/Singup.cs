using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fudbalski_Balon
{
    public partial class Singup : Form
    {
        public Singup()
        {
            InitializeComponent();
            button2.FlatStyle = FlatStyle.Flat;
            button2.FlatAppearance.BorderSize = 0;
        }

        private void Singup_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Login frm = new Login();
            frm.Show();
            this.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool emailValid = false, passValid = true;
            if (textBox3.Text.Split('@').Length == 2) { if (textBox3.Text.Split('@')[0] != "" && textBox3.Text.Split('@')[1] != "" && textBox3.Text.Split('@')[1].Contains('.')) { emailValid = true; } }
            if (textBox4.Text.Length < 8 || textBox4.Text.Length > 14) passValid = false;
            if(textBox1.Text.Length>2 && textBox1.Text.Length > 2 && emailValid && passValid)
            {
                SqlCommand komanda = new SqlCommand();
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Baza"].ConnectionString);
                komanda.CommandType = CommandType.StoredProcedure;
                komanda.CommandText = "dbo.DodajKorisnika";
                komanda.Connection = con;
                komanda.Parameters.Add(new SqlParameter("@ime", SqlDbType.NVarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, textBox1.Text));
                komanda.Parameters.Add(new SqlParameter("@prezime", SqlDbType.NVarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, textBox2.Text));
                komanda.Parameters.Add(new SqlParameter("@email", SqlDbType.VarChar, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, textBox3.Text));
                komanda.Parameters.Add(new SqlParameter("@lozinka", SqlDbType.VarChar, 14, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, textBox4.Text));
                komanda.Parameters.Add(new SqlParameter("@retVal", SqlDbType.Int)).Direction = ParameterDirection.ReturnValue;
                try
                {
                    con.Open();
                    komanda.ExecuteNonQuery();
                    con.Close();
                    Korisnik.email = textBox3.Text;
                    Pocetna frm = new Pocetna();
                    frm.Show();
                    this.Visible = false;
                }
                catch 
                {
                    errorProvider5.SetError(button1, "Doslo je do greske!");
                }              
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length < 3) errorProvider1.SetError(textBox1, "Uneto ime je prekratko!"); else errorProvider1.Clear();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Length < 3) errorProvider2.SetError(textBox2, "Uneto prezime je prekratko!"); else errorProvider2.Clear();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text.Split('@').Length == 2)
            {
                if (textBox3.Text.Split('@')[0] != "" && textBox3.Text.Split('@')[1] != "" && textBox3.Text.Split('@')[1].Contains('.'))
                {
                    errorProvider3.Clear();
                }
                else errorProvider3.SetError(textBox3, "Morate Uneti validnu e-mail adresu!");
            }
            else
            {
                errorProvider3.SetError(textBox3, "Morate Uneti validnu e-mail adresu!");
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox4.Text.Length < 7 || textBox4.Text.Length > 14) errorProvider4.SetError(textBox4, "Lozinka mora imate izmedju 8 i 14 karaktera!");
            else errorProvider4.Clear();
        }
    }
}
