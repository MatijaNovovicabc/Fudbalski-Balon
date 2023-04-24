using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fudbalski_Balon
{
    public partial class Pocetna : Form
    {
        public Pocetna()
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
            pictureBox1.Load("https://www.sportskicentarole.rs/media/balon/balon-za-fudbal-01.jpg");
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.Load("https://cdn.navidiku.rs/firme/proizvodgalerija2/galerija85094/iznajmljivanje-balona-za-mali-fudbal-pancevo-e314a9.jpg");
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.Load("https://www.soccerteam.rs/wp-content/uploads/2017/05/IMG_8790.jpg");
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void Pocetna_FormClosed(object sender, FormClosedEventArgs e)
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Korisnik.balonID = 1;
            Rezervacije frm = new Rezervacije();
            frm.Show();
            this.Visible = false;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Korisnik.balonID = 2;
            Rezervacije frm = new Rezervacije();
            frm.Show();
            this.Visible = false;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Korisnik.balonID = 3;
            Rezervacije frm = new Rezervacije();
            frm.Show();
            this.Visible = false;
        }
    }
}
