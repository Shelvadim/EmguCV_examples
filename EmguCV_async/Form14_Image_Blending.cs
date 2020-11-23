using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using Emgu.CV;
using Emgu.CV.Structure;

namespace EmguCV_async
{
    public partial class Form14_Image_Blending : Form
    {
        Image<Bgr, byte> imgInput1;
        Image<Bgr, byte> imgInput2;
        public Form14_Image_Blending()
        {
            InitializeComponent();
        }

        private void openImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Multiselect = true;
                ofd.Filter = "Image files. | *.jpg; *.bmp";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string[] fileNames = ofd.FileNames;

                    if (fileNames.Length < 2)
                    {
                        MessageBox.Show("Pleace select more then one image");
                        return;
                    }
                    imgInput1 = new Image<Bgr, byte>(fileNames[0]);
                    imgInput2 = new Image<Bgr, byte>(fileNames[1]);

                    pictureBox1.Image = imgInput1.ToBitmap();
                    pictureBox2.Image = imgInput2.ToBitmap();

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void blendImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                double alfa = (double)numericUpDown1.Value;
                pictureBox2.Image = imgInput1.AddWeighted(imgInput2, alfa, (1 - alfa), 0).ToBitmap();
            }
            
            catch (Exception ex)
            {
                MessageBox.Show("Files should have the same resolution." + ex.Message);
            }
        }

        private async void slideShowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    string[] FileNames = Directory.GetFiles(fbd.SelectedPath, "*.jpg");
                    List<Image<Bgr, byte>> listImages = new List<Image<Bgr, byte>>();
                    foreach (var file in FileNames)
                    {
                        listImages.Add(new Image<Bgr, byte>(file));
                    }
                    for (int i = 0; i < listImages.Count - 2; i++)
                    {
                        for (double alpha = 0.0; alpha <= 1.0; alpha += 0.01)
                        {
                            pictureBox2.Image = listImages[i + 1].AddWeighted(listImages[i], alpha, 1 - alpha, 0).ToBitmap();
                            await Task.Delay(25);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Files should have the same resolution." + ex.Message);
            }

        }
    }
}
