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
    public partial class Form9_capture_particular_video_frame : Form
    {
        private double totalFrame;
        private double fps;
        private int frameN;

        private VideoCapture capture = null;
        private bool pause = false;

        public Form9_capture_particular_video_frame()
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
                    totalFrame = capture.GetCaptureProperty(CapProp.FrameCount);
                    fps = capture.GetCaptureProperty(CapProp.Fps);

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (capture == null)
            {
                return;
            }
            pause = false;
            ReadAllFrames();
        }

        private async void ReadAllFrames()
        {
            try
            {
                Mat m = new Mat();
                int skipFrame = Convert.ToInt16(numericUpDown1.Value);

                while (frameN < (totalFrame - skipFrame) && pause == false)
                {
                    frameN += skipFrame;
                    capture.SetCaptureProperty(CapProp.PosFrames, frameN);
                    capture.Read(m);
                    pictureBox1.Image = m.ToBitmap();
                    await Task.Delay(1000 / Convert.ToInt16(fps));
                    label1.Text = frameN.ToString() + " / " + totalFrame.ToString();
                }

                if (pause == false)
                {
                    frameN = 0;
                }
            

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            pause = true;
        }
    }
}
