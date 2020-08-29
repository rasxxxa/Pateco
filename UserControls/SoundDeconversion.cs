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
using System.Media;

namespace WindowsFormsApplication2.UserControls
{
    public partial class SoundDeconversion : UserControl
    {
        public  enum TypeOfSoundDeconversion { PictureInSound = 1, TextInSound };
        private byte[] wav = null;
        SoundPlayer pl;
        List<byte> sound;
        string path = null;
        public SoundDeconversion()
        {
            sound = new List<byte>();
            pl = new SoundPlayer();

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string file = openFileDialog1.FileName;
                try
                {
                    path = file;
                    wav = File.ReadAllBytes(file);

                }
                catch
                {
                    MessageBox.Show("Грешка при учитавању");
                }

            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (wav != null)
            {
               if (HelperClass.ReturnTypeOFSoundDeconversion(wav) == (int)TypeOfSoundDeconversion.PictureInSound)
                {
                    Image s = HelperClass.SoundToImage(wav);
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
                                s.Save(fs,
                                   System.Drawing.Imaging.ImageFormat.Bmp);
                                break;

                        }

                        fs.Close();
                    }
                }
               else
                {
                    string text = HelperClass.SoundToText(wav);
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.ShowDialog();

                    // If the file name is not an empty string open it for saving.
                    if (saveFileDialog1.FileName != "")
                    {
                        string s = saveFileDialog1.FileName;
                        s += ".txt";
                        File.WriteAllText(s, text);
                        saveFileDialog1.Dispose();

                    }
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (path == null)
            {
                return;
            }
            pl.SoundLocation = path;
            pl.Play();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (path == null)
            {
                return;
            }
            pl.Stop();
        }
    }
}
