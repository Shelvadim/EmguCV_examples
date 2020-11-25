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
    public partial class Form17_Connected_Component_Labeling : Form
    {
        Image<Bgr, byte> imgInput;
        Image<Gray, byte> cc;

        public Form17_Connected_Component_Labeling()
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
            try
            {
                if(imgInput==null)
                {
                    return;
                }

                var tempImg = imgInput.Convert<Gray, byte>().ThresholdBinary(new Gray(65), new Gray(255)).Dilate(1).Erode(1);

                Mat mLabel = new Mat();
                int nLabels=CvInvoke.ConnectedComponents(tempImg, mLabel);
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
            //try
            //{
                if (cc == null)
                {
                    return;
                }

                int label = (int) cc[e.Y, e.X].Intensity;

                if (label > 0)
                {
                    var tempImg1 = cc.InRange(new Gray(label), new Gray(label));
                    pictureBox2.Image = tempImg1.ToBitmap();
                }                

            //}
           /* catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }*/
        }
    }
}
