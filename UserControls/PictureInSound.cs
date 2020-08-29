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
    public partial class PictureInSound : UserControl
    {
        byte[] wav = null;
        Image s = null;
        public PictureInSound()
        {
            InitializeComponent();
        }

        private void EditCompressionInfo(int compressionLevel, int quality)
        {
            var qualityFactor = 1;
            if (quality != 0)
            {
                qualityFactor = 2;
            }
            var spaceForOnePixel = 3 * (8 / compressionLevel) / qualityFactor;
            if (wav != null)
            {
               
                var length = wav.Length;
                double fileInLength = length / spaceForOnePixel / 1024.0 / 1024.0;
                if (s != null)
                {
                    if (s.Width * s.Height * 3 > (length/spaceForOnePixel))
                    {
                        tooBigPicture();
                        return;
                    }
                }
                label2.Text = fileInLength.ToString() + " мегабајта";
             }
        }

        public void tooBigPicture()
        {
            label2.Text = "Слика је превелика";
        }

        private bool isOk()
        {
            var length = wav.Length;
            var compressionLevel = getCompression();
            var qualityFactor = getQuality();
            qualityFactor = (qualityFactor == 0) ? 1 : 2;
            var spaceForOnePixel = 3 * (8 / compressionLevel) / qualityFactor;
            double fileInLength = length / spaceForOnePixel / 1024.0 / 1024.0;
            if (s != null)
            {
                if (s.Width * s.Height * 3 > (length / spaceForOnePixel))
                {
                    return false;
                }else
                {
                    return true;
                }
            }
            return false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string file = openFileDialog1.FileName;
                try
                {

                    EditCompressionInfo(getCompression(),getQuality());
                    s = Bitmap.FromFile(file);
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
            DialogResult result = openFileDialog2.ShowDialog();
            if (result == DialogResult.OK)
            {
                string file = openFileDialog2.FileName;
                try
                {

                    wav = File.ReadAllBytes(file);
                    EditCompressionInfo(getCompression(), getQuality());
                }
                catch
                {
                    MessageBox.Show("Грешка при учитавању");
                }

            }
        }

        public int getCompression()
        {
            if (radioButton1.Checked == true)
            {
                return 1;
            }
            else if (radioButton2.Checked == true)
            {
                return 2;
            }
            else
            {
                return 4;
            }
        }

        public int getQuality()
        {
            if (radioButton6.Checked == true)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (wav != null && s != null && isOk())
            {

                Bitmap b = new Bitmap(s);
                HelperClass.ImageToSound(b, wav, getCompression(), getQuality());
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "Sound WAV|*.wav";
                saveFileDialog1.Title = "Save an Sound File";
                saveFileDialog1.ShowDialog();

                // If the file name is not an empty string open it for saving.  
                if (saveFileDialog1.FileName != "")
                {
                    // Saves the Image via a FileStream created by the OpenFile method.  
                    // Saves the Image in the appropriate ImageFormat based upon the  
                    // File type selected in the dialog box.  
                    // NOTE that the FilterIndex property is one-based.  
                    switch (saveFileDialog1.FilterIndex)
                    {
                        case 1:
                            string s = saveFileDialog1.FileName;
                            saveFileDialog1.Dispose();
                            File.WriteAllBytes(s, wav);
                            break;
                    }

                }
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            EditCompressionInfo(getCompression(), getQuality());
        }
    }
}
