using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fudbalski_Balon
{
    public partial class Placanje : Form
    {
        public Placanje()
        {
            InitializeComponent();
            button1.FlatStyle = FlatStyle.Flat;
            button1.FlatAppearance.BorderSize = 0;
            button2.FlatStyle = FlatStyle.Flat;
            button2.FlatAppearance.BorderSize = 0;
            button3.FlatStyle = FlatStyle.Flat;
            button3.FlatAppearance.BorderSize = 0;
            button4.FlatStyle = FlatStyle.Flat;
            button4.FlatAppearance.BorderSize = 0;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.White;
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black;
            dataGridView1.DefaultCellStyle.BackColor = Color.White;
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
            string[] baloni = Korisnik.baloni.TrimEnd(';').Split(';');
            string[] termini = Korisnik.termini.TrimEnd(';').Split(';');
            string[] datumi = Korisnik.datumi.TrimEnd(';').Split(';');
            dataGridView1.DataSource = null;
            DataTable tabela = new DataTable();
            tabela.Columns.Add("Naziv balona");
            tabela.Columns.Add("Termin");
            tabela.Columns.Add("Datum");
            for (int i = 0; i < baloni.Length; i++)
            {
                string nazivBalona = Konekcija.Unos("select naziv from balon where id=" + baloni[i]).Rows[0][0].ToString();
                for (int j = 0; j < termini[i].Split(',').Length; j++)
                {
                    string[] row = new string[] { nazivBalona, termini[i].Split(',')[j] + " - " + (Convert.ToInt32(termini[i].Split(',')[j]) + 1), datumi[i] };
                    tabela.Rows.Add(row);
                }
            }
            dataGridView1.DataSource = tabela;
            DateTime datum = new DateTime();
            int suma = 0;
            for (int i = 0; i < baloni.Length; i++)
            {
                datum = new DateTime(Convert.ToInt32(datumi[i].Split('-')[0]), Convert.ToInt32(datumi[i].Split('-')[1]), Convert.ToInt32(datumi[i].Split('-')[2]));
                string dan = "";
                if (datum.DayOfWeek.ToString() == "Monday") dan = "Ponedeljak";
                if (datum.DayOfWeek.ToString() == "Tuesday") dan = "Utorak";
                if (datum.DayOfWeek.ToString() == "Wednesday") dan = "Sreda";
                if (datum.DayOfWeek.ToString() == "Thursday") dan = "Cetvrtak";
                if (datum.DayOfWeek.ToString() == "Friday") dan = "Petak";
                if (datum.DayOfWeek.ToString() == "Saturday") dan = "Subota";
                if (datum.DayOfWeek.ToString() == "Sunday") dan = "Nedelja";
                int cena = Convert.ToInt32(Konekcija.Unos("select cenaTermina from balon_radnoVreme where dan='" + dan + "' and balon_id=" + baloni[i]).Rows[0][0]);
                suma += cena * termini[i].Split(',').Length;
            }
            label6.Text = "Ukupna suma: "+ suma +" RSD";
        }

        private void Placanje_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Pocetna frm = new Pocetna();
            frm.Show();
            this.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ONama frm = new ONama();
            frm.Show();
            this.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Rezervacije frm = new Rezervacije();
            frm.Show();
            this.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Nalog frm = new Nalog();
            frm.Show();
            this.Visible = false;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentCell.RowIndex != dataGridView1.RowCount - 1)
            {
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    dataGridView1.Rows[i].Cells[0].Style.BackColor = Color.White;
                    dataGridView1.Rows[i].Cells[1].Style.BackColor = Color.White;
                    dataGridView1.Rows[i].Cells[2].Style.BackColor = Color.White;
                }
                if (dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Style.BackColor == Color.White)
                {
                    dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Style.BackColor = Color.Aqua;
                    dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[1].Style.BackColor = Color.Aqua;
                    dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[2].Style.BackColor = Color.Aqua;
                }
                else
                {
                    dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Style.BackColor = Color.White;
                    dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[1].Style.BackColor = Color.White;
                    dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[2].Style.BackColor = Color.White;
                }
            }
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            e.PaintBackground(e.ClipBounds, false);
            e.PaintContent(e.ClipBounds);
            e.Handled = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            bool ima = false;
            for(int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Style.BackColor==Color.Aqua)
                {
                    if (dataGridView1.RowCount == 2)
                    {
                        Korisnik.baloni = "";
                        Korisnik.termini = "";
                        Korisnik.datumi = "";
                        Pocetna frm = new Pocetna();
                        frm.Show();
                        this.Visible = false;
                    }
                    else
                    {
                        ima = true;
                        string balonID = Konekcija.Unos("select id from balon where naziv='" + dataGridView1.Rows[i].Cells[0].Value + "'").Rows[0][0].ToString();
                        string termin = dataGridView1.Rows[i].Cells[1].Value.ToString().Split('-')[0].TrimEnd();
                        string datum = dataGridView1.Rows[i].Cells[2].Value.ToString();
                        string[] baloni = Korisnik.baloni.TrimEnd(';').Split(';');
                        string[] termini = Korisnik.termini.TrimEnd(';').Split(';');
                        string[] datumi = Korisnik.datumi.TrimEnd(';').Split(';');
                        for (int j = 0; j < baloni.Length; j++)
                        {
                            if (baloni[j] == balonID && datumi[j] == datum)
                            {
                                termini[j] = termini[j].Replace(termin + ',', "");
                                termini[j] = termini[j].Replace(',' + termin, "");
                                if (termini[j] == "")
                                {
                                    Korisnik.baloni = "";
                                    Korisnik.termini = "";
                                    Korisnik.datumi = "";
                                    for (int m = 0; m < baloni.Length; m++)
                                    {
                                        if (m != j)
                                        {
                                            Korisnik.baloni += baloni[m] + ';';
                                            Korisnik.termini = termini[m] + ';';
                                            Korisnik.datumi = datumi[m] + ';';
                                        }
                                    }
                                }
                                else
                                {
                                    Korisnik.baloni = "";
                                    Korisnik.termini = "";
                                    Korisnik.datumi = "";
                                    for (int m = 0; m < baloni.Length; m++)
                                    {
                                        Korisnik.baloni += baloni[m] + ';';
                                        Korisnik.termini = termini[m] + ';';
                                        Korisnik.datumi = datumi[m] + ';';
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (!ima)
            {
                errorProvider1.SetError(button5,"Niste odabrali nista!");
            }
            else
            {
                Placanje frm = new Placanje();
                frm.Show();
                this.Visible = false;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if(textBox1.Text.Trim()==""|| textBox2.Text.Trim() == "" || textBox3.Text.Trim() == "" || textBox4.Text.Trim() == "")
            {
                errorProvider2.SetError(button6, "Uneti podaci nisu kompletni!");
            }
            else
            {
                if(textBox1.Text.Trim().Length<3 || textBox2.Text.Trim().Length!=21 || textBox3.Text.Trim().Length!=5 || textBox4.Text.Trim().Length<3 || textBox4.Text.Trim().Length > 4)
                {
                    errorProvider2.SetError(button6,"Uneti podaci nisu validni!");
                }
                else
                {
                    DateTime datum = new DateTime();
                    string[] baloni = Korisnik.baloni.TrimEnd(';').Split(';');
                    string[] termini = Korisnik.termini.TrimEnd(';').Split(';');
                    string[] datumi = Korisnik.datumi.TrimEnd(';').Split(';');
                    int suma = 0;
                    for(int i = 0; i < baloni.Length; i++)
                    {
                        datum = new DateTime(Convert.ToInt32(datumi[i].Split('-')[0]), Convert.ToInt32(datumi[i].Split('-')[1]), Convert.ToInt32(datumi[i].Split('-')[2]));
                        string dan = "";
                        if (datum.DayOfWeek.ToString() == "Monday") dan = "Ponedeljak";
                        if (datum.DayOfWeek.ToString() == "Tuesday") dan = "Utorak";
                        if (datum.DayOfWeek.ToString() == "Wednesday") dan = "Sreda";
                        if (datum.DayOfWeek.ToString() == "Thursday") dan = "Cetvrtak";
                        if (datum.DayOfWeek.ToString() == "Friday") dan = "Petak";
                        if (datum.DayOfWeek.ToString() == "Saturday") dan = "Subota";
                        if (datum.DayOfWeek.ToString() == "Sunday") dan = "Nedelja";
                        int cena = Convert.ToInt32(Konekcija.Unos("select cenaTermina from balon_radnoVreme where dan='"+dan+"' and balon_id=" + baloni[i]).Rows[0][0]);
                        suma += cena * termini[i].Split(',').Length;
                    }
                    SqlCommand komanda = new SqlCommand();
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Baza"].ConnectionString);
                    komanda.CommandType = CommandType.StoredProcedure;
                    komanda.CommandText = "dbo.NapraviRacun";
                    komanda.Connection = con;
                    komanda.Parameters.Add(new SqlParameter("@korisnik_id", SqlDbType.Int, 32, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, Konekcija.Unos("select id from korisnik where email='"+Korisnik.email+"'").Rows[0][0]));
                    komanda.Parameters.Add(new SqlParameter("@suma", SqlDbType.Int, 32, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, suma));
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
                        MessageBox.Show("Doslo je do greske.");
                    }
                    else
                    {
                        for(int i = 0; i < baloni.Length; i++)
                        {
                            for(int j = 0; j < termini[i].Split(',').Length; j++)
                            {

                                string[] delovi = datumi[i].Split('-');
                                if (delovi[1].Length == 1) datumi[i].Replace("-" + delovi[1] + "-", "-0" + delovi[1]+"-");
                                if (delovi[2].Length == 1) datumi[i] = datumi[i].Split(',')[0] + '-' + datumi[i].Split(',')[0] + "-0" + delovi[2];
                                komanda = new SqlCommand();
                                komanda.CommandType = CommandType.StoredProcedure;
                                komanda.CommandText = "dbo.DodajURacun";
                                komanda.Connection = con;
                                komanda.Parameters.Add(new SqlParameter("@racun_id", SqlDbType.Int, 32, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, rez));
                                komanda.Parameters.Add(new SqlParameter("@balon_id", SqlDbType.Int, 32, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, baloni[i]));
                                komanda.Parameters.Add(new SqlParameter("@datumTermina", SqlDbType.Date, 32, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, datumi[i]));
                                komanda.Parameters.Add(new SqlParameter("@termin", SqlDbType.Int, 32, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, termini[i].Split(',')[j]));
                                try
                                {
                                    con.Open();
                                    komanda.ExecuteNonQuery();
                                    con.Close();
                                }
                                catch 
                                {
                                    MessageBox.Show("Doslo je do greske.");
                                }
                            }
                        }
                        Korisnik.datumi = "";
                        Korisnik.baloni = "";
                        Korisnik.termini = "";
                        Pocetna frm = new Pocetna();
                        frm.Show();
                        this.Visible = false;
                    }                   
                }
            }
        }
    }
}
