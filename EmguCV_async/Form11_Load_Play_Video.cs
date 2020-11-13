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
    public partial class Form11_Load_Play_Video : Form
    {

        private int totalFrame;
        private int fps;
        private int curFrameN;
        private Mat curFrameM;
        private VideoCapture capture = null;
        private bool isPlaying = false;

        public Form11_Load_Play_Video()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Video files. | *.mp4; *.avi";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    capture = new VideoCapture(ofd.FileName);
                    totalFrame = Convert.ToInt32(capture.GetCaptureProperty(CapProp.FrameCount));
                    fps = Convert.ToInt32(capture.GetCaptureProperty(CapProp.Fps));
                    isPlaying = true;
                    curFrameM = new Mat();
                    curFrameN = 0;
                    trackBar1.Minimum = 0;
                    trackBar1.Maximum = totalFrame - 1;
                    trackBar1.Value = 0;
                    PlayVideo();

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void PlayVideo()
        {
            if (capture == null)
            {
                return;
            }
            try
            {
                while (isPlaying==true && curFrameN<totalFrame)
                {
                    capture.SetCaptureProperty(CapProp.PosFrames, curFrameN);
                    capture.Read(curFrameM);
                    pictureBox1.Image = curFrameM.ToBitmap();
                    trackBar1.Value = curFrameN;
                    curFrameN +=1;
                    await Task.Delay(1000 / fps);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (capture != null)
            {
                isPlaying=true;
                PlayVideo();
            }
            else
            {
                isPlaying = false;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            isPlaying = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            isPlaying = false;
            curFrameN = 0;
            trackBar1.Value = 0;
            pictureBox1.Image = null;
            pictureBox1.Invalidate();
            capture = null;
            curFrameM = null;

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (capture != null)
            {
                curFrameN = trackBar1.Value;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (capture != null && curFrameN != 0)
                {
                    curFrameM.Save("C:\\pictures\\" + curFrameN.ToString() + ".jpg");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
