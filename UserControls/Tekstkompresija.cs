using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Tekstkompresija : UserControl
    {
        // Promenljive 
        private bool sel1 = false;
        private Image image;
        private List<byte> imageBytes;


        // Konstruktor
        public Tekstkompresija()
        {
            imageBytes = new List<byte>();
            image = null;
            InitializeComponent();
        }

        #region Algoritmi za skrivanje teksta

        // Pretvaranje celog broja u niz 0 i 1
        public int[] binarni(int br)
        {
            int[] niz = new int[8];
            int i = 7;
            while (br != 0)
            {
                niz[i] = (br % 2);
                br /= 2;
                i--;
            }
            for (int j = i; j >= 0; j--)
            {
                niz[j] = 0;
            }


            return niz;
        }

        // Pretvaranje niza 0 i 1 u ceo broj
        public int preobrazi(int[] niz)
        {
            int suma = 0;
            for (int i = 0; i < niz.Length; i++)
            {
                if (niz[i] != 0)
                {
                    suma += (int)Math.Pow(2, 7 - i);
                }

            }


            return suma;





        }

        // Spajanje 2 niza u jedan uzimajuci 4 glavna bita iz niz1 i 4 glavna bita iz niz2
        public int[] spoji(int[] niz1, int[] niz2)
        {
            int[] novi = new int[8];
            for (int i = 0; i < 4; i++)
            {
                novi[i] = niz1[i];
            }
            for (int i = 4; i < 8; i++)
            {
                novi[i] = niz2[i - 4];
            }



            return novi;



        }

        #endregion

        #region Eventi za Controle iz Forme

        private void Check()
        {
            if (image == null)
            {
                return;
            }
            var size = image.Width * image.Height * 3;
            var allBytes = size * 8;

            if (textBox1.Text != null && textBox1.Text.Length > 0)
            {
                var usedSpace = textBox1.Text.Length * 8;
                var usableBytes = image.Width * image.Height * 3 * getQuality();
                var freeSpace = allBytes - usableBytes - usedSpace;
                var possibleCharacters = (int)Math.Round((decimal)freeSpace / 8);
                label4.Text = "Слободно још: " + possibleCharacters.ToString() + " карактера.";
            }
            else
            {
                var usableBytes = image.Width * image.Height * 3 * getQuality();
                var freeSpace = allBytes - usableBytes;
                var possibleCharacters = (int)Math.Round((decimal)freeSpace / 8);
                label4.Text = possibleCharacters.ToString();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {

            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string file = openFileDialog1.FileName;
                try
                {

                    Image s = Bitmap.FromFile(file);
                    image = s;
                    imageBytes = new List<byte>();
                    Bitmap bmp = new Bitmap(s);
                    for (int i = 0; i < image.Width; i++)
                    {
                        for (int j = 0; j < image.Height; j++)
                        {
                            byte r = bmp.GetPixel(i, j).R;
                            byte g = bmp.GetPixel(i, j).G;
                            byte b = bmp.GetPixel(i, j).B;
                            imageBytes.Add(r);
                            imageBytes.Add(g);
                            imageBytes.Add(b);
                        }
                    }
                    Check();
                    pictureBox1.Image = s;
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

                }
                catch
                {
                    MessageBox.Show("Грешка при учитавању");
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0 && image!=null)
            {
                Image image2 = HelperClass.TextToPicture(imageBytes,image.Width,image.Height,textBox1.Text.ToString(),getQuality());
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "Bitmap Image|*.bmp";
                saveFileDialog1.Title = "Save an Image File";
                saveFileDialog1.ShowDialog();

                // If the file name is not an empty string open it for saving.  
                if (saveFileDialog1.FileName != "")
                {
                    // Saves the Image via a FileStream created by the OpenFile method.  
                    System.IO.FileStream fs =
                       (System.IO.FileStream)saveFileDialog1.OpenFile();
                    // Saves the Image in the appropriate ImageFormat based upon the  
                    // File type selected in the dialog box.  
                    // NOTE that the FilterIndex property is one-based.  
                    switch (saveFileDialog1.FilterIndex)
                    {


                        case 1:
                            image2.Save(fs,
                               System.Drawing.Imaging.ImageFormat.Bmp);
                            break;
                    }

                    fs.Close();
                }
            }
        }

        #endregion

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private int getQuality()
        {
            if (radioButton1.Checked)
            {
                return 2;
            }
            if (radioButton2.Checked)
            {
                return 4;
            }
            return 6;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Check();
        }
    }
}
