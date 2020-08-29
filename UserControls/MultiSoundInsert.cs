using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication2.UserControls
{
    public partial class MultiSoundInsert : UserControl
    {
        private List<byte> imageByte = null;
        private List<byte> qualityBytes;
        private Image image = null;
        private List<byte[]> sounds;
        private byte[] tempSounds;

        public MultiSoundInsert()
        {
            sounds = new List<byte[]>();
            qualityBytes = new List<byte>();
            tempSounds = null;
            InitializeComponent();

        }

        private int getQuality()
        {
            if (radioButton4.Checked == true)
            {
                EditDropMenu();
                return 0;
            }
            if (radioButton5.Checked == true)
            {
                EditDropMenu();
                return 2;
            }
            EditDropMenu();
            return 4;
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

        private void CheckState()
        {
            if (image == null)
            {
                return;
            }
            if (sounds.Count == 0)
            {
                double value = (image.Width * image.Height * 3) * 8 - (image.Width * image.Height * 3)*(8 - GetCompressionLevel()) ;
                label2.Text = (value/8/1024.0/1024.0).ToString("0.##") + " мегабајта";
                return;
            }else if (sounds.Count != 0)
            {
                long sumOfLengt = 0;
                for (int i = 0; i<sounds.Count;i++)
                {
                    sumOfLengt += sounds[i].Length*8 - sounds[i].Length*(getQuality());

                }


                long possibleBytes = (image.Width * image.Height * 3);
                long finalPossibleBytes = (possibleBytes * 8) - (possibleBytes * (8-GetCompressionLevel()));
                if (finalPossibleBytes > sumOfLengt)
                {
                    label2.Text = ((finalPossibleBytes - sumOfLengt)/8 / 1024.0 / 1024.0).ToString("0.##") + " мегабајта";
                }else
                {
                    label2.Text = "Преоптерећено!";
                }
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


                    image = Bitmap.FromFile(file);
                    Bitmap bmp = new Bitmap(image);
                    pictureBox1.Image = image;
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    imageByte = new List<byte>();
                    for (int i = 0; i < image.Width; i++)
                    {
                        for (int j = 0; j < image.Height; j++)
                        {
                            byte r = bmp.GetPixel(i, j).R;
                            byte g = bmp.GetPixel(i, j).G;
                            byte b = bmp.GetPixel(i, j).B;
                            imageByte.Add(r);
                            imageByte.Add(g);
                            imageByte.Add(b);
                        }
                    }

                   CheckState();

                }
                catch
                {
                    MessageBox.Show("Грешка при учитавању");
                }

            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            CheckState();
        }

        private void EditDropMenu()
        {
            comboBox1.Items.Clear();
            for (int i = 0; i < sounds.Count; i++)
            {
                comboBox1.Items.Add((i + 1));
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
                    tempSounds =  File.ReadAllBytes(file);
                }
                catch
                {
                    MessageBox.Show("Грешка при учитавању");
                }

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {

                int i = Convert.ToInt32(comboBox1.SelectedItem) - 1;
                sounds.RemoveAt(i);
                qualityBytes.RemoveAt(i);
            }
            catch
            {

            }
            EditDropMenu();
            CheckState();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (tempSounds==null)
                {
                    MessageBox.Show("Унесите звук");
                    return;
                }
                if (radioButton4.Checked == false && radioButton5.Checked == false && radioButton6.Checked == false)
                {
                    MessageBox.Show("Подесите квалитет звука");
                    return;
                }
                if (comboBox1.SelectedIndex != -1)
                {
                    int index = comboBox1.SelectedIndex;
                    qualityBytes[index] = (byte)getQuality();
                }else
                {
                 //   int i = Convert.ToInt32(comboBox1.SelectedItem) - 1;

                    qualityBytes.Add((byte)getQuality());
                    sounds.Add(tempSounds);
                    tempSounds = null;
                }
                EditDropMenu();
                CheckState();
                radioButton4.Checked = false;
                radioButton5.Checked = false;
                radioButton6.Checked = false;
            }
            catch
            {

            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (imageByte.Count != 0 && sounds.Count != 0 && isOk())
            {
                Image image2 = HelperClass.MultiSoundInImage(imageByte, image.Width, image.Height,sounds ,qualityBytes, GetCompressionLevel() );
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

        private bool isOk()
        {
            if (image == null)
            {
                return false;
            }
            if (sounds.Count == 0)
            {
                return false;
            }
            long sumOfLengt = 0;
            for (int i = 0; i < sounds.Count; i++)
            {
                sumOfLengt += sounds[i].Length * 8 - sounds[i].Length * (getQuality());

            }


            long possibleBytes = (image.Width * image.Height * 3);
            long finalPossibleBytes = (possibleBytes * 8) - (possibleBytes * (8 - GetCompressionLevel()));
            if (finalPossibleBytes > sumOfLengt)
            {
                return true;
            }
            return false;

        }
    }
}
