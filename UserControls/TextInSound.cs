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
    public partial class TextInSound : UserControl
    {
        private byte[] wav = null;
        public TextInSound()
        {
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

                    wav = File.ReadAllBytes(file);
                    int level;
                    if (radioButton1.Checked == true)
                    {
                        level = 1;
                    }
                    else if (radioButton2.Checked == true)
                    {
                        level = 2;
                    }
                    else
                    {
                        level = 4;
                    }
                    long lenghtOfFile = wav.Length;
                    lenghtOfFile -= 50;
 
                    var numOfCharacters = Math.Floor((decimal)(lenghtOfFile / (8 / level)));
                    
                    label2.Text = numOfCharacters.ToString();
                }
                catch
                {
                    MessageBox.Show("Грешка при учитавању");
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (wav != null && textBox1.Text.Length > 0)
            {
                string text = textBox1.Text.ToString();
                var level = 2;
                if (radioButton1.Checked == true)
                {
                    level = 1;
                }else if (radioButton2.Checked == true)
                {
                    level = 2;
                }else
                {
                    level = 4;
                }

                HelperClass.TextToSound(text, wav, level);
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

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (wav == null)
            {
                return;
            }
            int level;
            if (radioButton1.Checked == true)
            {
                level = 1;
            }
            else if (radioButton2.Checked == true)
            {
                level = 2;
            }
            else
            {
                level = 4;
            }
            long lenghtOfFile = wav.Length;
            lenghtOfFile -= 50;
            var numOfCharacters = Math.Floor((decimal)(lenghtOfFile / (8 / level)));
            label2.Text = numOfCharacters.ToString();
        }
    }
}
