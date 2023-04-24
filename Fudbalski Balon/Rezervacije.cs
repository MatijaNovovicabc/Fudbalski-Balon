using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace Fudbalski_Balon
{
    public partial class Rezervacije : Form
    {
        public Rezervacije()
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
            dateTimePicker1.MinDate = DateTime.Today;
            dateTimePicker1.MaxDate = DateTime.Today.AddDays(7);
            DataTable baloni = new DataTable();
            baloni = Konekcija.Unos("select * from balon");
            for (int i = 0; i < baloni.Rows.Count; i++) comboBox1.Items.Add(baloni.Rows[i][1]);
        }

        private void Rezervacije_FormClosed(object sender, FormClosedEventArgs e)
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable zauzetiTermini, radnoVreme, tabela = new DataTable();
            string dan = "";
            string[] datum = dateTimePicker1.Value.Date.ToString().Split(' ')[0].Split('/');
            int balonID =Convert.ToInt32(Konekcija.Unos("select id from balon where naziv='" + comboBox1.Text + "'").Rows[0][0]);
            if (dateTimePicker1.Value.DayOfWeek.ToString() == "Monday") dan = "Ponedeljak";
            if (dateTimePicker1.Value.DayOfWeek.ToString() == "Tuesday") dan = "Utorak";
            if (dateTimePicker1.Value.DayOfWeek.ToString() == "Wednesday") dan = "Sreda";
            if (dateTimePicker1.Value.DayOfWeek.ToString() == "Thursday") dan = "Cetvrtak";
            if (dateTimePicker1.Value.DayOfWeek.ToString() == "Friday") dan = "Petak";
            if (dateTimePicker1.Value.DayOfWeek.ToString() == "Saturday") dan = "Subota";
            if (dateTimePicker1.Value.DayOfWeek.ToString() == "Sunday") dan = "Nedelja";
            dataGridView1.DataSource = null;
            radnoVreme = Konekcija.Unos("select pocetak,kraj from balon_radnoVreme where dan='" + dan + "' and balon_id=" + balonID);
            zauzetiTermini = Konekcija.Unos("select pocetak from balon_rezervacija where balon_id=" + balonID + " and datum='" + datum[2] + "-" + datum[0] + "-" + datum[1] + "'");
            tabela.Columns.Add("Od");
            tabela.Columns.Add("Do");
            for (int i = Convert.ToInt32(radnoVreme.Rows[0][0]); i < Convert.ToInt32(radnoVreme.Rows[0][1]); i++)
            {
                string[] row = new string[] { i.ToString(), (i + 1).ToString() };
                tabela.Rows.Add(row);
            }
            dataGridView1.DataSource = tabela;
            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells[0].Style.BackColor = Color.White;
                dataGridView1.Rows[i].Cells[1].Style.BackColor = Color.White;
            }
            for (int i = 0; i < zauzetiTermini.Rows.Count; i++)
            {
                int red = Convert.ToInt32(zauzetiTermini.Rows[i][0]) - Convert.ToInt32(radnoVreme.Rows[0][0]);
                dataGridView1.Rows[red].Cells[0].Style.BackColor = Color.Red;
                dataGridView1.Rows[red].Cells[1].Style.BackColor = Color.Red;
            }
            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Style.BackColor != Color.Red)
                {
                    dataGridView1.Rows[i].Cells[0].Style.BackColor = Color.White;
                    dataGridView1.Rows[i].Cells[1].Style.BackColor = Color.White;
                }
            }
            string[] baloni = Korisnik.baloni.TrimEnd(';').Split(';');
            string[] termini = Korisnik.termini.TrimEnd(';').Split(';');
            string[] datumi = Korisnik.datumi.TrimEnd(';').Split(';');
            for (int i = 0; i < baloni.Length; i++)
            {
                if (baloni[i] == balonID.ToString() && datumi[i] == (datum[2] + "-" + datum[0] + "-" + datum[1]))
                {
                    for (int j = 0; j < dataGridView1.RowCount; j++)
                    {
                        if (termini[i].Split(',').Contains(dataGridView1.Rows[j].Cells[0].Value))
                        {
                            dataGridView1.Rows[j].Cells[0].Style.BackColor = Color.Cyan;
                            dataGridView1.Rows[j].Cells[1].Style.BackColor = Color.Cyan;
                        }
                    }
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Style.BackColor == Color.White)
            {
                dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Style.BackColor = Color.Cyan;
                dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[1].Style.BackColor = Color.Cyan;
            } else
            if (dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Style.BackColor == Color.Cyan)
            {
                dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Style.BackColor = Color.White;
                dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[1].Style.BackColor = Color.White;
            }
            dataGridView1.Refresh();
        }

        private void Rezervacije_Shown(object sender, EventArgs e)
        {           
            if (Korisnik.balonID != 0) comboBox1.SelectedIndex = comboBox1.Items.IndexOf(Konekcija.Unos("select naziv from balon where id=" + Korisnik.balonID).Rows[0][0]); else { comboBox1.SelectedIndex = 0; }
            Korisnik.balonID = 0;            
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            e.PaintBackground(e.ClipBounds, false);
            e.PaintContent(e.ClipBounds);
            e.Handled = true;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            comboBox1_SelectedIndexChanged(sender,e);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int balonID = Convert.ToInt32(Konekcija.Unos("select id from balon where naziv='" + comboBox1.Text + "'").Rows[0][0]);
            string[] baloni = Korisnik.baloni.TrimEnd(';').Split(';');
            string[] termini = Korisnik.termini.TrimEnd(';').Split(';');
            string[] datumi = Korisnik.datumi.TrimEnd(';').Split(';');
            bool check = false;
            string[] datum = dateTimePicker1.Value.Date.ToString().Split(' ')[0].Split('/');
            for (int i = 0; i < baloni.Length; i++)
            {
                if (baloni[i] == balonID.ToString() && datumi[i] == (datum[2] + "-" + datum[0] + "-" + datum[1])) check = true;
            }
            if (check)
            {
                Korisnik.baloni = "";
                Korisnik.termini = "";
                Korisnik.datumi = "";
                for (int i = 0; i < baloni.Length; i++)
                {
                    if (!(baloni[i] == balonID.ToString() && datumi[i] == (datum[2] + "-" + datum[0] + "-" + datum[1])))
                    {
                        Korisnik.baloni += baloni[i] + ';';
                        Korisnik.termini += termini[i] + ';';
                        Korisnik.datumi += datumi[i] + ';';
                    }
                    else
                    {
                        int count = 0;
                        for (int j = 0; j < dataGridView1.RowCount; j++)
                        {
                            if (dataGridView1.Rows[j].Cells[0].Style.BackColor == Color.Cyan)
                            {
                                count++;
                                Korisnik.termini += dataGridView1.Rows[j].Cells[0].Value.ToString() + ',';
                            }
                        }
                        if (count != 0)
                        {
                            Korisnik.termini = Korisnik.termini.TrimEnd(',') +';';
                            Korisnik.baloni += balonID.ToString() + ';';
                            Korisnik.datumi += datum[2] + "-" + datum[0] + "-" + datum[1] +';';
                        }
                        else
                        {

                        }                       
                    }
                }
            }
            else
            {
                int count = 0;
                for (int j = 0; j < dataGridView1.RowCount; j++)
                {
                    if (dataGridView1.Rows[j].Cells[0].Style.BackColor == Color.Cyan)
                    {
                        count++;
                        Korisnik.termini += dataGridView1.Rows[j].Cells[0].Value.ToString() + ',';
                    }
                }
                if (count != 0)
                {
                    Korisnik.termini = Korisnik.termini.TrimEnd(',') +';';
                    Korisnik.baloni += balonID.ToString() + ';';
                    Korisnik.datumi += datum[2] + "-" + datum[0] + "-" + datum[1] + ';';
                }
                else
                {

                }
            }
            Korisnik.termini = Korisnik.termini.TrimEnd(';') + ';';
            Korisnik.baloni = Korisnik.baloni.TrimEnd(';') + ';';
            Korisnik.datumi = Korisnik.datumi.TrimEnd(';') + ';';
        }
    }
}
