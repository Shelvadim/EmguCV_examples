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
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace EmguCV_async
{
    

    public partial class Form8_Real_Time_Text_Detection : Form
    {
        private VideoCapture capture = null;
        private bool pause = false;
        public Form8_Real_Time_Text_Detection()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    capture = new VideoCapture(ofd.FileName);
                    Mat m = new Mat();
                    capture.Read(m);
                    pictureBox1.Image = m.ToBitmap();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void detectTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (capture == null)
            {
                return;
            }
            try
            {
                //pause = false;
                while (true)
                {
                    Mat m = new Mat();
                    capture.Read(m);
                    if (!m.IsEmpty)
                    {
                        pictureBox2.Image = m.ToBitmap();
                        DetectText(m.ToImage<Bgr, byte>());
                        double fps = capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps);
                        await Task.Delay(1000 / Convert.ToInt32(fps));
                    }
                    else
                    {

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DetectText(Image<Bgr, byte> img)
        {
            /*
                1. Edge detection using Sobel 
                2. Morphological Operation (Dilation)
                3. Contour Extraction
                4. Applying geometrical Constraints
                5. Display localized text
             */

            // 1 Sobel
            Image<Gray, byte> sobel = img.Convert<Gray, byte>().Sobel(1, 0, 3).AbsDiff(new Gray(0.0)).Convert<Gray, byte>().ThresholdBinary(new Gray(50), new Gray(255));

            // 2 Dilation
            Mat SE = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle, new Size(10, 2), new Point(-1, -1));
            sobel = sobel.MorphologyEx(Emgu.CV.CvEnum.MorphOp.Dilate, SE, new Point(-1, -1), 1, Emgu.CV.CvEnum.BorderType.Reflect, new MCvScalar(255));

            // 3 Contours
            Emgu.CV.Util.VectorOfVectorOfPoint contours = new Emgu.CV.Util.VectorOfVectorOfPoint();
            Mat m = new Mat();
            CvInvoke.FindContours(sobel, contours, m, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);

            // 4 geometrical Constraints
            List<Rectangle> list = new List<Rectangle>();

            for(int i=0; i<contours.Size; i++)
            {
                Rectangle brect = CvInvoke.BoundingRectangle(contours[i]);
                double ar = brect.Width / brect.Height;

                if (ar>2 && brect.Width>15 && brect.Height>8 && brect.Width < 650)
                {
                    list.Add(brect);
                }
            }

            Image<Bgr, byte> imgout = img.CopyBlank();

            foreach (var item in list)
            {
                CvInvoke.Rectangle(img, item, new MCvScalar(0, 0, 255), 2);
                CvInvoke.Rectangle(imgout, item, new MCvScalar(0, 255, 255), -1);
            }

            imgout._And(img);
            pictureBox1.Image = img.ToBitmap();
            pictureBox2.Image = imgout.ToBitmap();

        }
    }
}
