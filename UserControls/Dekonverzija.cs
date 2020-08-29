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

namespace WindowsFormsApplication2
{
    public partial class Dekonverzija : UserControl
    {
        public enum TypeOfDeconversion{ PictureFromPicture = 1, TextFromPicture, MultiSoundInPicture, SoundInPicture};
        // Promenljive
        bool izabrana = false;
        private List<byte> imageBytes;

        // Konstruktor
        public Dekonverzija()
        {
            imageBytes = new List<byte>();
            InitializeComponent();
        }


        #region Algoritmi potrebni za manipulaciju bitovima








        #endregion

        #region Eventi na controle iz forme
        // Odabir slike
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string file = openFileDialog1.FileName;
                try
                {

                    Image s = Bitmap.FromFile(file);
                    pictureBox1.Image = s;
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    Bitmap bmp = new Bitmap(s);
                    for (int i = 0; i < bmp.Width; i++)
                    {
                        for (int j = 0; j < bmp.Height; j++)
                        {
                            byte r = bmp.GetPixel(i, j).R;
                            byte g = bmp.GetPixel(i, j).G;
                            byte b = bmp.GetPixel(i, j).B;
                            imageBytes.Add(r);
                            imageBytes.Add(g);
                            imageBytes.Add(b);
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Грешка при учитавању");
                }

            }
        }

        // Pokretanje dekonverzije
        private void button2_Click(object sender, EventArgs e)
        {
           if (imageBytes != null && imageBytes.Count > 0)
           {
                if (imageBytes[0]==(int)TypeOfDeconversion.PictureFromPicture)
                {
                    List<Image> images = HelperClass.MultiPictureDeconversion(imageBytes);
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.Filter = "Bitmap Image|*.bmp";
                    saveFileDialog1.Title = "Save an Image File";
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
                                {
                                    string s = saveFileDialog1.FileName;
                                    string[] paths = s.Split('\\');
                                    string[] savePaths = new string[images.Count];
                                    for (int i = 0; i < savePaths.Length; i++)
                                    {
                                        for (int j = 0; j < paths.Length; j++)
                                        {
                                            if (j != paths.Length - 1)
                                            {
                                                savePaths[i] += paths[j] + "\\";
                                            }
                                            else
                                            {
                                                savePaths[i] += i + paths[j];
                                            }

                                        }
                                        Bitmap bmp = new Bitmap(images[i]);
                                        bmp.Save(savePaths[i], System.Drawing.Imaging.ImageFormat.Bmp);


                                    }

                                    saveFileDialog1.Dispose();

                                    break;
                                }

                        }

                    }
                }else if (imageBytes[0] == (int)TypeOfDeconversion.TextFromPicture)
                {
                    string text = HelperClass.ImageWithText(imageBytes);
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
                }else if (imageBytes[0] == (int)TypeOfDeconversion.MultiSoundInPicture)
                {
                    List<byte[]> sounds = HelperClass.MultiSoundDeconversion(imageBytes);
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.Filter = "Wav Sound|*.wav";
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
                                {
                                    string s = saveFileDialog1.FileName;
                                    string[] paths = s.Split('\\');
                                    string[] savePaths = new string[sounds.Count];
                                    for (int i = 0; i < savePaths.Length; i++)
                                    {
                                        for (int j = 0; j < paths.Length; j++)
                                        {
                                            if (j != paths.Length - 1)
                                            {
                                                savePaths[i] += paths[j] + "\\";
                                            }
                                            else
                                            {
                                                savePaths[i] += i + paths[j];
                                            }

                                        }
                                        File.WriteAllBytes(savePaths[i], sounds[i]);
                                    }

                                    saveFileDialog1.Dispose();

                                    break;
                                }

                        }

                    }
                }
                else if(imageBytes[0] == (int)TypeOfDeconversion.SoundInPicture)
                {
                  var  wav = HelperClass.SoundOutImage(imageBytes, null);
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
                                string path = saveFileDialog1.FileName;
                                saveFileDialog1.Dispose();
                                File.WriteAllBytes(path, wav);
                                break;
                        }

                    }
                }
                imageBytes = new List<byte>();

           }
        }

        //Chekiranje chekboxova



        #endregion
    }
}
