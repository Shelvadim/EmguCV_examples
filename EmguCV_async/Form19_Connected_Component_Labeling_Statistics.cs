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
    public partial class Form19_Connected_Component_Labeling_Statistics : Form
    {

        Image<Bgr, byte> imgInput;
        Image<Gray, byte> cc;
        CCStatsOP[] statsOP;
        MCvPoint2D64f[] centrPoints;
        public struct CCStatsOP
        {
            public Rectangle Rectangle;
            public int Area;
        }
        public Form19_Connected_Component_Labeling_Statistics()
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
                Mat stats = new Mat();
                Mat centr = new Mat();

                int nLabels = CvInvoke.ConnectedComponentsWithStats(tempImg, mLabel,stats,centr);

                cc = mLabel.ToImage<Gray, byte>();
                centrPoints = new MCvPoint2D64f[nLabels];
                centr.CopyTo(centrPoints);

                statsOP = new CCStatsOP[nLabels];
                stats.CopyTo(statsOP);

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

                    int x = (int)centrPoints[label].X;
                    int y = (int)centrPoints[label].Y;

                    var imgCopy = imgInput.Copy();
                    CvInvoke.PutText(imgCopy, "o", new Point(x, y), Emgu.CV.CvEnum.FontFace.HersheyPlain, 0.8, new MCvScalar(0, 0, 255), 2);

                    Rectangle rectBox = statsOP[label].Rectangle;
                    imgCopy.Draw(rectBox, new Bgr(0, 0, 255), 2);

                    label1.Text = statsOP[label].Area.ToString();

                    pictureBox1.Image = imgCopy.ToBitmap();
                    pictureBox2.Image = tempImg1.ToBitmap();

                }

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
