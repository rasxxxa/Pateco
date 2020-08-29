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
    public partial class Konvertovanje : UserControl
    {
        // Promenljive
        private Image mainImage = null;
        bool ub1 = false, ub2 = false;
        private List<byte> mainImageBytes;
        private List<List<byte>> smallImageBytes;
        private List<int> qualityBytes;
        private List<byte> tempPicture;
        private List<Tuple<int, int>> resolutions;
        private Tuple<int, int> tempResolution;
        //Konstruktor
        public Konvertovanje()
        {
            mainImageBytes = new List<byte>();
            smallImageBytes = new List<List<byte>>();
            qualityBytes = new List<int>();
            resolutions = new List<Tuple<int, int>>();
            tempResolution = null;
            InitializeComponent();
        }


        #region Algoritmi za skrivanje slike

        #endregion

        #region Eventi za Controle iz Forme

        private void button1_Click(object sender, EventArgs e)
        {

            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string file = openFileDialog1.FileName;
                try
                {

                    Image image = Bitmap.FromFile(file);
                    Bitmap bmp = new Bitmap(image);
                    mainImageBytes = new List<byte>();
                    for (int i = 0; i < image.Width; i++)
                    {
                        for (int j = 0; j < image.Height; j++)
                        {
                            byte r = bmp.GetPixel(i, j).R;
                            byte g = bmp.GetPixel(i, j).G;
                            byte b = bmp.GetPixel(i, j).B;
                            mainImageBytes.Add(r);
                            mainImageBytes.Add(g);
                            mainImageBytes.Add(b);
                        }
                    }
                    pictureBox1.Image = image;
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    mainImage = image;
                    Check();
                }
                catch
                {
                    MessageBox.Show("Грешка при учитавању");
                }

            }
        }

        private int GetCompressionLevel()
        {
            if (radioButton1.Checked == true)
            {
                EditDropMenu();
                return 2;
            }
            if (radioButton2.Checked == true)
            {
                EditDropMenu();
                return 4;
            }
            EditDropMenu();
            return 6;

        }

        private int GetQuality()
        {
            if (radioButton4.Checked == true)
            {
                EditDropMenu();
                return 2;
            }
            if (radioButton5.Checked == true)
            {
                EditDropMenu();
                return 4;
            }
            EditDropMenu();
            return 6;

        }

        private void EditDropMenu()
        {
            comboBox1.Items.Clear();
            for (int i = 0; i < smallImageBytes.Count; i++)
            {
                comboBox1.Items.Add((i + 1));
            }
        }

        private void Check()
        {
            if (mainImageBytes == null)
            {
                return;
            }
            if (smallImageBytes.Count == 0)
            {
                double value = (mainImage.Width * mainImage.Height * 3) * 8 - (mainImage.Width * mainImage.Height * 3) * (8 - GetCompressionLevel());
                label5.Text = (value / 8 / 1024.0 / 1024.0).ToString("0.##") + " мегабајта";
                return;
            }
            else if (smallImageBytes.Count != 0)
            {
                long sumOfLengt = 0;
                for (int i = 0; i < smallImageBytes.Count; i++)
                {
                    sumOfLengt += smallImageBytes[i].Count * 8 - smallImageBytes[i].Count * (qualityBytes[i]);

                }


                long possibleBytes = (mainImage.Width * mainImage.Height * 3);
                long finalPossibleBytes = (possibleBytes * 8) - (possibleBytes * (8 - GetCompressionLevel()));
                if (finalPossibleBytes > sumOfLengt)
                {
                    label5.Text = ((finalPossibleBytes - sumOfLengt) / 8 / 1024.0 / 1024.0).ToString("0.##") + " мегабајта";
                }
                else
                {
                    label5.Text = "Преоптерећено!";
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {


            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string file = openFileDialog1.FileName;
                try
                {

                    Image s = Bitmap.FromFile(file);
                    Bitmap bmp = new Bitmap(s);
                    tempPicture = new List<byte>();
                    tempResolution = new Tuple<int, int>(s.Width, s.Height);
                    for (int i = 0; i < bmp.Width; i++)
                    {
                        for (int j = 0; j < bmp.Height; j++)
                        {
                            byte r = bmp.GetPixel(i, j).R;
                            byte g = bmp.GetPixel(i, j).G;
                            byte b = bmp.GetPixel(i, j).B;
                            tempPicture.Add(r);
                            tempPicture.Add(g);
                            tempPicture.Add(b);
                        }
                    }


                }
                catch
                {
                    MessageBox.Show("Грешка при учитавању");
                }

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!label5.Text.Equals("Преоптерећено!") && mainImageBytes!= null && mainImageBytes.Count > 0 && smallImageBytes != null && smallImageBytes.Count > 0)
            {

                    Image image2 = HelperClass.MultiPictureInsert(mainImageBytes, mainImage.Width, mainImage.Height, smallImageBytes, qualityBytes,resolutions, GetCompressionLevel());
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
            else
            {
                MessageBox.Show("Унесите све параметре");
            }

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Check();
        }

        private void button5_Click(object sender, EventArgs e)
        {

            if ((tempPicture == null || tempPicture.Count == 0) && comboBox1.SelectedIndex >= 0)
            {
                if (radioButton4.Checked == false && radioButton5.Checked == false && radioButton6.Checked == false)
                {
                    MessageBox.Show("Молимо подесите квалитет слике која се компресује");
                    return;
                }
                radioButton4.Checked = false;
                radioButton5.Checked = false;
                radioButton6.Checked = false;
                qualityBytes[comboBox1.SelectedIndex] = GetQuality();
                comboBox1.SelectedIndex = -1;
                EditDropMenu();
                Check();
                return;
            }

            if (tempPicture == null || tempPicture.Count == 0)
            {
                MessageBox.Show("Молимо унесите слику");
                return;
            }
            if (radioButton4.Checked == false && radioButton5.Checked == false && radioButton6.Checked == false)
            {
                MessageBox.Show("Молимо подесите квалитет слике која се компресује");
                return;
            }
            resolutions.Add((tempResolution));
            qualityBytes.Add(GetQuality());
            smallImageBytes.Add(new List<byte>(tempPicture));
            tempPicture = new List<byte>();
            tempResolution = null;
            radioButton4.Checked = false;
            radioButton5.Checked = false;
            radioButton6.Checked = false;
            comboBox1.SelectedIndex = -1;
            EditDropMenu();
            Check();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {

                int i = Convert.ToInt32(comboBox1.SelectedItem) - 1;
                smallImageBytes.RemoveAt(i);
                qualityBytes.RemoveAt(i);
                resolutions.RemoveAt(i);
            }
            catch
            {

            }
            EditDropMenu();
            Check();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (qualityBytes[comboBox1.SelectedIndex]<=0)
            {
                return;
            }
            if (qualityBytes[comboBox1.SelectedIndex] == 2)
            {
                radioButton4.Checked = true;
            }
            if (qualityBytes[comboBox1.SelectedIndex] == 4)
            {
                radioButton5.Checked = true;
            }
            if (qualityBytes[comboBox1.SelectedIndex] == 6)
            {
                radioButton6.Checked = true;
            }
        }

        private void izvrniKlik(object sender, EventArgs e)
        {
          //  if (pictureBox1.Image != null && pictureBox2.Image != null && ub1 && ub2)
         //   {
          //      Image temp = pictureBox1.Image;
         //       pictureBox1.Image = pictureBox2.Image;
         //       pictureBox2.Image = temp;
         //   }
        }

        #endregion
    }
}
