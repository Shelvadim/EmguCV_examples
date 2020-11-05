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
    public partial class Form6_Text_Segmentation : Form
    {
        private Image<Bgr, byte> imgInput = null;
        private Image<Gray, byte> imgOutput = null;
        Bitmap img = null;
        bool show = false;

        public Form6_Text_Segmentation()
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

        private async void segmentTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imgOutput = imgInput.Convert<Gray, byte>().ThresholdBinaryInv(new Gray(50), new Gray(255));
            Emgu.CV.Util.VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            Mat hierar = new Mat();

            CvInvoke.FindContours(imgOutput, contours, hierar, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
            show = true;

            if (contours.Size > 0)
            {
                for (int i = 0; i < contours.Size; i++)
                {
                    //double area = CvInvoke.ContourArea(contours[i]);
                    Rectangle rect = CvInvoke.BoundingRectangle(contours[i]);

                    imgInput.ROI = rect;
                    img = imgInput.Copy().ToBitmap();
                    imgInput.ROI = Rectangle.Empty;

                    this.Invalidate();
                    await Task.Delay(1000);
                }
                show = false;
            }

        }

        private void Form6_Text_Segmentation_Paint(object sender, PaintEventArgs e)
        {
            if (show == true)
            {
                pictureBox2.Image = img;
            }
        }
    }
}
