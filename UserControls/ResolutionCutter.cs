using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2.UserControls
{
    class ResolutionCutter : UserControl
    {
        private PictureBox pictureBox1;
        private OpenFileDialog openFileDialog1;
        private SaveFileDialog saveFileDialog1;
        private List<byte> mainImageBytes;
        private Button button11;
        private Button button4;
        private Button button5;
        private Button button1;
        private Label label1;
        private Label label2;
        private Image mainImage;
        private Image mainLoadedImage = null;
        private List<byte> mainLoadedImageBytes;
        public ResolutionCutter()
        {
            mainLoadedImageBytes = new List<byte>();
            mainImageBytes = new List<byte>();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResolutionCutter));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.button11 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            //
            // pictureBox1
            //
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(594, 419);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            //
            // openFileDialog1
            //
            this.openFileDialog1.FileName = "openFileDialog1";
            //
            // button11
            //
            this.button11.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.button11.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.button11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button11.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button11.ForeColor = System.Drawing.Color.White;
            this.button11.Location = new System.Drawing.Point(648, 66);
            this.button11.Margin = new System.Windows.Forms.Padding(13, 12, 13, 1);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(238, 60);
            this.button11.TabIndex = 15;
            this.button11.Text = "Учитај слику";
            this.button11.UseVisualStyleBackColor = false;
            this.button11.Click += new System.EventHandler(this.button1_Click);
            //
            // button4
            //
            this.button4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.button4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.ForeColor = System.Drawing.Color.White;
            this.button4.Location = new System.Drawing.Point(648, 153);
            this.button4.Margin = new System.Windows.Forms.Padding(13, 12, 13, 1);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(238, 64);
            this.button4.TabIndex = 16;
            this.button4.Text = "Смањи слику";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button2_Click);
            //
            // button5
            //
            this.button5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.button5.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button5.ForeColor = System.Drawing.Color.White;
            this.button5.Location = new System.Drawing.Point(648, 245);
            this.button5.Margin = new System.Windows.Forms.Padding(13, 12, 13, 1);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(238, 57);
            this.button5.TabIndex = 17;
            this.button5.Text = "Сачувај";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button3_Click);
            //
            // button1
            //
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(648, 329);
            this.button1.Margin = new System.Windows.Forms.Padding(13, 12, 13, 1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(238, 57);
            this.button1.TabIndex = 18;
            this.button1.Text = "Рестарт";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(643, 405);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(195, 27);
            this.label1.TabIndex = 19;
            this.label1.Text = "Резолуција слике:";
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(643, 432);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 27);
            this.label2.TabIndex = 20;
            //
            // ResolutionCutter
            //
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.pictureBox1);
            this.Name = "ResolutionCutter";
            this.Size = new System.Drawing.Size(946, 608);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void EditResolution()
        {
            string result = "";
            if (mainImage!=null)
            {
                result += mainImage.Width + " x " + mainImage.Height + " пиксела";
                label2.Text = result;
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
                    mainLoadedImage = image;
                    mainLoadedImageBytes = new List<byte>(mainImageBytes);
                    mainImage = image;
                    EditResolution();
                }
                catch
                {
                    MessageBox.Show("Грешка при учитавању");
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (mainImageBytes.Count > 0)
            {
                int newWidth = mainImage.Width / 2;
                int newHeight = mainImage.Height / 2;
                Bitmap bitmap = new Bitmap(newWidth, newHeight);
                Bitmap oldPict = new Bitmap(mainImage);
                List<byte> listt = new List<byte>();
                for (int i = 0; i < mainImage.Width - 1; i += 2)
                {
                    for (int j = 0; j < mainImage.Height - 1; j += 2)
                    {
                        var pix1 = oldPict.GetPixel(i, j);
                        var pix2 = oldPict.GetPixel(i + 1, j);
                        var pix3 = oldPict.GetPixel(i, j + 1);
                        var pix4 = oldPict.GetPixel(i + 1, j + 1);
                        var sumR = pix1.R + pix2.R + pix4.R + pix3.R;
                        var sumG = pix1.G + pix2.G + pix4.G + pix3.G;
                        var sumB = pix1.B + pix2.B + pix4.B + pix3.B;
                        listt.Add((byte)(sumR / 4));
                        listt.Add((byte)(sumG / 4));
                        listt.Add((byte)(sumB / 4));
                    }
                }
                int pos = 0;
                for (int i = 0; i < newWidth; i++)
                {
                    for (int j = 0; j < newHeight; j++)
                    {
                        var pixl1 = listt[pos++];
                        var pixl2 = listt[pos++];
                        var pixl3 = listt[pos++];
                        bitmap.SetPixel(i, j, Color.FromArgb(pixl1, pixl2, pixl3));
                    }
                }
                mainImageBytes = new List<byte>(listt);
                pictureBox1.Image = bitmap;
                mainImage = pictureBox1.Image;
                EditResolution();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
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
                        pictureBox1.Image.Save(fs,
                           System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                }

                fs.Close();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            mainImage = mainLoadedImage;
            mainImageBytes = mainLoadedImageBytes;
            pictureBox1.Image = mainImage;
            EditResolution();
        }
    }
}
