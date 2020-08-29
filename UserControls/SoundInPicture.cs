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
    public partial class SoundInPicture : UserControl
    {
        private byte[] wav = null;
        private Image image = null;
        private List<byte> imageByte;
        public SoundInPicture()
        {
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            CheckState();
        }

        private int GetCompressionLevel()
        {
            if (radioButton1.Checked == true)
            {
                return 2;
            }
            if (radioButton2.Checked == true)
            {
                return 4;
            }

            return 6;
        }

        private void CheckState()
        {
            if (image == null)
            {
                return;
            }

            int width = image.Width;
            int height = image.Height;
            int compression = GetCompressionLevel();
            int PictureSize = width * height * 3;
            if (wav == null)
            {
                double value = (PictureSize - 44) * (8.0 / (8.0 - getQuality())) / (8 - compression)  / 1024.0 / 1024.0;
                label2.Text = value.ToString("#.##") + " мегабајта";
                return;
            } else if (wav != null && (PictureSize * (8.0 / (8.0 - getQuality())) / (8 - compression) < wav.Length))
            {
                label2.Text = "Звук је превелик!";
            }
            else
            {
                label2.Text = "У реду је!";
            }


        }

        private int getQuality()
        {
            if (radioButton4.Checked == true)
            {
                return 0;
            }
            if (radioButton5.Checked == true)
            {
                return 2;
            }
            return 4;
        }

        private void button2_Click(object sender, EventArgs e)
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
                    for(int i = 0;i<image.Width;i++)
                    {
                        for (int j = 0;j<image.Height;j++)
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

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string file = openFileDialog1.FileName;
                try
                {
                    wav = File.ReadAllBytes(file);

                }
                catch
                {
                    MessageBox.Show("Грешка при учитавању");
                }

            }
            CheckState();
        }

        public bool isOk()
        {
            if (wav == null || image == null)
            {
                return false;

            }
            if (wav != null && ((image.Width * image.Height * 3) * (8.0 / (8.0 - getQuality())) / (8 - GetCompressionLevel()) < wav.Length))
            {
                return false;
            }
            return true;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (wav != null && image!=null && isOk())
            {
                Image image2 = HelperClass.SoundInImage(wav, image.Width, image.Height, GetCompressionLevel(), getQuality(), imageByte);
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
    }
}
