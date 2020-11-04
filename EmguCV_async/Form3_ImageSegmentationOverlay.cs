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
    public partial class Form3_ImageSegmentationOverlay : Form
    {
        private Image<Bgr, byte> imgInput = null;
        private Image<Gray, byte> imgOutput = null;
        public Form3_ImageSegmentationOverlay()
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

        private void rangeFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3_Parameters fp = new Form3_Parameters(this);
            fp.Show();
        }

        public void ApplyRangeFilter(int min, int max)
        {
            try
            {
                imgOutput = imgInput.Convert<Gray, byte>().InRange(new Gray(min), new Gray(max)).Canny(10,50);

                pictureBox2.Image = imgOutput.ToBitmap();
                pictureBox2.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void overlayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (imgInput != null)
                {
                    Image<Bgr, byte> temp = imgInput.Clone();
                    temp.SetValue(new Bgr(0, 0, 255), imgOutput);
                    pictureBox2.Image=temp.ToBitmap();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
