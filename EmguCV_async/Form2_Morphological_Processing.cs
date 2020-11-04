using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.Structure;

namespace EmguCV_async
{
    
    public partial class Form2_Morphological_Processing : Form
    {
        private Image<Bgr, byte> imgInput = null;

        public Form2_Morphological_Processing()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog()==DialogResult.OK)
                {
                    imgInput = new Image<Bgr, byte>(ofd.FileName);
                    pictureBox1.Image = imgInput.ToBitmap();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void erosionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (imgInput != null)
                {
                    pictureBox2.Image = imgInput.Erode(2).ToBitmap();
                }
                else
                {
                    MessageBox.Show("Select image");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dilationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (imgInput != null)
                {
                    pictureBox2.Image = imgInput.Dilate(3).ToBitmap();
                }
                else
                {
                    MessageBox.Show("Select image");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void openToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                if (imgInput != null)
                {
                    Mat kernel = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle, new Size(5, 5), new Point(-1, -1));
                    pictureBox2.Image = imgInput.MorphologyEx(Emgu.CV.CvEnum.MorphOp.Open,kernel, new Point(-1, -1),1,Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(1.0)).ToBitmap();
                }
                else
                {
                    MessageBox.Show("Select image");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (imgInput != null)
                {
                    Mat kernel = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle, new Size(5, 5), new Point(-1, -1));
                    pictureBox2.Image = imgInput.MorphologyEx(Emgu.CV.CvEnum.MorphOp.Close, kernel, new Point(-1, -1), 1, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(1.0)).ToBitmap();
                }
                else
                {
                    MessageBox.Show("Select image");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void gradientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (imgInput != null)
                {
                    Mat kernel = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle, new Size(5, 5), new Point(-1, -1));
                    pictureBox2.Image = imgInput.MorphologyEx(Emgu.CV.CvEnum.MorphOp.Gradient, kernel, new Point(-1, -1), 1, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(1.0)).ToBitmap();
                }
                else
                {
                    MessageBox.Show("Select image");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void topToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (imgInput != null)
                {
                    Mat kernel = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle, new Size(5, 5), new Point(-1, -1));
                    pictureBox2.Image = imgInput.MorphologyEx(Emgu.CV.CvEnum.MorphOp.Tophat, kernel, new Point(-1, -1), 1, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(1.0)).ToBitmap();
                }
                else
                {
                    MessageBox.Show("Select image");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void blackHatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (imgInput != null)
                {
                    Mat kernel = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle, new Size(5, 5), new Point(-1, -1));
                    pictureBox2.Image = imgInput.MorphologyEx(Emgu.CV.CvEnum.MorphOp.Blackhat, kernel, new Point(-1, -1), 1, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(1.0)).ToBitmap();
                }
                else
                {
                    MessageBox.Show("Select image");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dileteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (imgInput != null)
                {
                    pictureBox2.Image = imgInput.Convert<Gray,byte>().ThresholdBinary(new Gray(120), new Gray(255)).Dilate(1).ToBitmap();
                }
                else
                {
                    MessageBox.Show("Select image");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void erosionOnBinaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (imgInput != null)
                {
                    pictureBox2.Image = imgInput.Convert<Gray, byte>().ThresholdBinary(new Gray(120), new Gray(255)).Erode(1).ToBitmap();
                }
                else
                {
                    MessageBox.Show("Select image");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
