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
using WindowsFormsApplication2.UserControls;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {

        // Promenljive
       public static UserControl prethodnastranica = null;
       public static   UserControl[] paneli;
       public static   int trenutnastranica = 0;

        // Konstuktor forme
        public Form1()
        {
            paneli = new UserControl[9];
            InitializeComponent();
            paneli[0] = new Konvertovanje();
            paneli[1] = new Dekonverzija();
            paneli[2] = new Tekstkompresija();
            paneli[3] = new TextInSound();
            paneli[4] = new SoundDeconversion();
            paneli[5] = new PictureInSound();
            paneli[6] = new SoundInPicture();
            paneli[7] = new MultiSoundInsert();
            paneli[8] = new ResolutionCutter();
            for (int i = 0; i < paneli.Length; i++)
            {
                paneli[i].Width = panel4.Width;
                paneli[i].Height = panel4.Height;
                paneli[i].Parent = panel4;
            }

            this.KeyPreview = true;
        }


        #region Eventi za Controle na formi

        private void button1_Click(object sender, EventArgs e)
        {
            paneli[0].BringToFront();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            paneli[1].BringToFront();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            paneli[2].BringToFront();

        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == (char)27 && trenutnastranica==0)
            {

                this.Close();
            }

        }

        private void klikNazad(object sender, EventArgs e)
        {
            if (prethodnastranica==null || trenutnastranica==1)
            {
                for (int i = 0; i < paneli.Length; i++)
                {
                    paneli[i].SendToBack();
                }

            }else
            {
                for (int i = 0; i < paneli.Length; i++)
                {
                    paneli[i].SendToBack();
                }
                trenutnastranica--;
                prethodnastranica.BringToFront();
            }
        }

        private void klikExit(object sender, EventArgs e)
        {
            this.Close();
        }

        private void skrivanje4(object sender, EventArgs e)
        {
            paneli[3].BringToFront();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://drive.google.com/open?id=1hYxp4nXjU1rhlL5ckCy_BlFiLHMaXST2");
        }


        #endregion

        private void button5_Click(object sender, EventArgs e)
        {
            paneli[3].BringToFront();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            paneli[9].BringToFront();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            paneli[5].BringToFront();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            paneli[11].BringToFront();
        }

        private void button9_Click(object sender, EventArgs e)
        {
           paneli[6].BringToFront();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            paneli[8].BringToFront();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal || this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Minimized;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            paneli[7].BringToFront();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            paneli[4].BringToFront();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {

        }

        private void button3_Click_2(object sender, EventArgs e)
        {
            paneli[8].BringToFront();
        }
    }
}
