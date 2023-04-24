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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Fudbalski_Balon
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            button2.FlatStyle = FlatStyle.Flat;
            button2.FlatAppearance.BorderSize = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool emailValid=false, passValid=true;
            if (textBox1.Text.Split('@').Length == 2) if (textBox1.Text.Split('@')[0] != "" && textBox1.Text.Split('@')[1] != "" && textBox1.Text.Split('@')[1].Contains('.')) emailValid = true;
            if (textBox2.Text.Length < 8 || textBox2.Text.Length > 14) passValid = false;
            if(passValid && emailValid){
                SqlCommand komanda = new SqlCommand();
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Baza"].ConnectionString);
                komanda.CommandType = CommandType.StoredProcedure;
                komanda.CommandText = "dbo.ProveraKorisnika";
                komanda.Connection = con;
                komanda.Parameters.Add(new SqlParameter("@email", SqlDbType.VarChar, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, textBox1.Text));
                komanda.Parameters.Add(new SqlParameter("@lozinka", SqlDbType.VarChar, 14, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, textBox2.Text));
                komanda.Parameters.Add(new SqlParameter("@retVal", SqlDbType.Int)).Direction = ParameterDirection.ReturnValue;
                int rez = 2;
                try
                {
                    con.Open();
                    komanda.ExecuteNonQuery();
                    con.Close();
                    rez = (int)komanda.Parameters["@retVal"].Value;
                }
                catch { }
                if (rez == 2)
                {
                    errorProvider3.SetError(button1, "Doslo je do greske!");

                }
                if (rez == 0)
                {
                    errorProvider3.SetError(button1, "Uneti podaci nisu dobri!");

                }
                if (rez == 1)
                {
                    Korisnik.email = textBox1.Text;
                    Pocetna frm = new Pocetna();
                    frm.Show();
                    this.Visible = false;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Singup frm = new Singup();
            frm.Show();
            this.Visible = false;
        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Split('@').Length == 2)
            {
                if (textBox1.Text.Split('@')[0] != "" && textBox1.Text.Split('@')[1] != "" && textBox1.Text.Split('@')[1].Contains('.'))
                {
                    errorProvider1.Clear();
                }
                else errorProvider1.SetError(textBox1, "Morate Uneti validnu e-mail adresu!");
            }
            else
            {
                errorProvider1.SetError(textBox1, "Morate Uneti validnu e-mail adresu!");
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Length < 7 || textBox2.Text.Length > 14) errorProvider2.SetError(textBox2, "Lozinka mora imate izmedju 8 i 14 karaktera!");
            else errorProvider2.Clear();
        }
    }
}
