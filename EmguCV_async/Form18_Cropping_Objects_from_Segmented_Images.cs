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
using Emgu.CV.Util;

namespace EmguCV_async
{
    public partial class Form18_Cropping_Objects_from_Segmented_Images : Form
    {
        Image<Bgr, byte> imgInput;
        Image<Gray, byte> cc;
        public Form18_Cropping_Objects_from_Segmented_Images()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Image files. | *.jpg; *.bmp";

                if (ofd.ShowDialog() == DialogResult.OK)
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

        private void processToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imgInput == null)
            {
                return;
            }

            try
            {
                var tempImg = imgInput.Convert<Gray, byte>().ThresholdBinary(new Gray(65), new Gray(255)).Dilate(1).Erode(1);

                Mat mLabel = new Mat();
                int nLabels = CvInvoke.ConnectedComponents(tempImg, mLabel);
                cc = mLabel.ToImage<Gray, byte>();

                pictureBox2.Image = tempImg.ToBitmap();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (cc == null)
                {
                    return;
                }

                int label = (int)cc[e.Y, e.X].Intensity;

                if (label > 0)
                {
                    var tempImg1 = cc.InRange(new Gray(label), new Gray(label));
                    VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
                    Mat m = new Mat();

                    CvInvoke.FindContours(tempImg1, contours, m, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
                    
                    if (contours.Size > 0)
                    {
                        Rectangle rectBox = CvInvoke.BoundingRectangle(contours[0]);
                        imgInput.ROI = rectBox;
                        var img = imgInput.Copy();
                        imgInput.ROI = Rectangle.Empty;

                        pictureBox2.Image = img.ToBitmap();
                    }
                                        
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
