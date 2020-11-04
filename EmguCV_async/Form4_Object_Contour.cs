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
    public partial class Form4_Object_Contour : Form
    {
        private Image<Bgr, byte> imgInput = null;
        private Image<Gray, byte> imgOutput = null;
        public Form4_Object_Contour()
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

        private void findContourToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imgOutput = imgInput.Convert<Gray, byte>().ThresholdBinaryInv(new Gray(160), new Gray(255));
            Emgu.CV.Util.VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            Mat hierar = new Mat();

            //Image<Gray, byte> imgOut = new Image<Gray, byte>(imgInput.Width, imgInput.Height, new Gray(0));

            CvInvoke.FindContours(imgOutput, contours, hierar, RetrType.External, ChainApproxMethod.ChainApproxSimple);
            CvInvoke.DrawContours(imgInput, contours, -1, new MCvScalar(255, 0, 0));
            pictureBox2.Image = imgInput.ToBitmap();
        }
    }
}
