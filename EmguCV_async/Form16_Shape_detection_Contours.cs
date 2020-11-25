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
using Emgu.CV.Util;

namespace EmguCV_async
{
    public partial class Form16_Shape_detection_Contours : Form
    {
        Image<Bgr, byte> imgInput;
        public Form16_Shape_detection_Contours()
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

        private void detectShapeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imgInput == null)
            {
                return;
            }

            try
            {
                var tempImg = imgInput.SmoothGaussian(5).Convert<Gray, byte>().ThresholdBinaryInv(new Gray(230), new Gray(255));
                VectorOfVectorOfPoint countors = new VectorOfVectorOfPoint();
                Mat m = new Mat();
                CvInvoke.FindContours(tempImg,countors, m, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
                for (int i=0; i<countors.Size; i++)
                {
                    double perimeter = CvInvoke.ArcLength(countors[i], true);
                    VectorOfPoint approx = new VectorOfPoint();
                    CvInvoke.ApproxPolyDP(countors[i], approx, 0.04 * perimeter, true);
                    CvInvoke.DrawContours(imgInput, countors, i, new MCvScalar(0, 0, 255), 2);

                    //moments center of the shape
                    var moments = CvInvoke.Moments(countors[i]);
                    int x =(int)(moments.M10 / moments.M00);
                    int y = (int)(moments.M01 / moments.M00);

                    if (approx.Size == 3)
                    {
                        CvInvoke.PutText(imgInput, "Triangle", new Point(x, y), Emgu.CV.CvEnum.FontFace.HersheySimplex, 0.5, new MCvScalar(0, 0, 255),2);
                    }
                    if (approx.Size == 4)
                    {
                        Rectangle rec = CvInvoke.BoundingRectangle(countors[i]);
                        double ar = (double)rec.Height / rec.Width;

                        if(ar>=0.95 && ar <= 1.05)
                        {
                            CvInvoke.PutText(imgInput, "Square", new Point(x, y), Emgu.CV.CvEnum.FontFace.HersheySimplex, 0.5, new MCvScalar(0, 0, 255), 2);
                        }
                        else
                        {
                            CvInvoke.PutText(imgInput, "Rectangle", new Point(x, y), Emgu.CV.CvEnum.FontFace.HersheySimplex, 0.5, new MCvScalar(0, 0, 255), 2);
                        }
                        
                    }
                    if (approx.Size == 6)
                    {
                        CvInvoke.PutText(imgInput, "Hexagon", new Point(x, y), Emgu.CV.CvEnum.FontFace.HersheySimplex, 0.5, new MCvScalar(0, 0, 255), 2);
                    }
                    if (approx.Size> 6)
                    {
                        CvInvoke.PutText(imgInput, "Circle", new Point(x, y), Emgu.CV.CvEnum.FontFace.HersheySimplex, 0.5, new MCvScalar(0, 0, 255), 2);
                    }
                    pictureBox2.Image = imgInput.ToBitmap();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
