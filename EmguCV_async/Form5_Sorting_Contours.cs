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
    public partial class Form5_Sorting_Contours : Form
    {
        private Image<Bgr, byte> imgInput = null;
        private Image<Gray, byte> imgOutput = null;
        public Form5_Sorting_Contours()
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
                    imgInput = new Image<Bgr, byte>(ofd.FileName);
                    pictureBox1.Image = imgInput.ToBitmap();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void findContoursToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imgOutput = imgInput.Convert<Gray, byte>().ThresholdBinary(new Gray(50), new Gray(255));
            Emgu.CV.Util.VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            Mat hierar = new Mat();

            //Image<Gray, byte> imgOut = new Image<Gray, byte>(imgInput.Width, imgInput.Height, new Gray(0));

            CvInvoke.FindContours(imgOutput, contours, hierar, RetrType.External, ChainApproxMethod.ChainApproxSimple);

            Dictionary<int, double> dict = new Dictionary<int, double>();

            if (contours.Size > 0)
            {
                for(int i=0; i < contours.Size; i++)
                {
                    double area = CvInvoke.ContourArea(contours[i]);
                    Rectangle rect = CvInvoke.BoundingRectangle(contours[i]);
                    if (rect.Width > 20 && rect.Height>20 && area>4000)
                    {
                        dict.Add(i, area);
                    }
                    
                }
            }

            var item = dict.OrderByDescending(v => v.Value);

            Image<Bgr, byte> imgOut = new Image<Bgr, byte>(imgInput.Width, imgInput.Height, new Bgr(0,0,0));

            foreach (var it in item)
            {
                int key = int.Parse(it.Key.ToString());
                Rectangle rect = CvInvoke.BoundingRectangle(contours[key]);
                //CvInvoke.DrawContours(imgInput, contours, key, new MCvScalar(255, 255, 255), 4);
                CvInvoke.Rectangle(imgOut, rect, new MCvScalar(255, 255, 255), 3);
                //CvInvoke.Rectangle(imgInput, rect, new MCvScalar(255, 0, 0), 3);
            }

            
            //pictureBox2.Image = imgInput.ToBitmap();
            pictureBox2.Image = imgOut.ToBitmap();
        }
    }
}
