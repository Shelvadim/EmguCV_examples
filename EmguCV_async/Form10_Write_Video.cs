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
    public partial class Form10_Write_Video : Form
    {
        private double totalFrame;
        private double fps;
        private int frameN;
        private VideoCapture capture = null;
        public Form10_Write_Video()
        {
            InitializeComponent();
        }

        private void readVideoToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void writeVideoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (capture == null)
            {
                return;
            }
            int fourcc = Convert.ToInt32(capture.GetCaptureProperty(CapProp.FourCC));
            int weidht = Convert.ToInt32(capture.GetCaptureProperty(CapProp.FrameWidth));
            int height = Convert.ToInt32(capture.GetCaptureProperty(CapProp.FrameHeight));
            string destinationPath= @"C:\Video\out.mp4";
            VideoWriter writer = new VideoWriter(destinationPath, fourcc, fps, new Size(weidht,height), true);
            Image<Bgr, byte> logo = new Image<Bgr, byte>("C:\\Pictures\\3.jpg");
            Mat m = new Mat();

            while (frameN < 500)
            {
                capture.Read(m);
                //add logo part
                Image<Bgr, byte> img = m.ToImage<Bgr, byte>();
                img.ROI = new Rectangle(0, 0, logo.Width, logo.Height);
                logo.CopyTo(img);
                img.ROI = Rectangle.Empty;
                //
                writer.Write(img.Mat);
                frameN++;
            }
            if (writer.IsOpened)
            {
                writer.Dispose();
            }
            MessageBox.Show("Complited.");
        }
    }
}
