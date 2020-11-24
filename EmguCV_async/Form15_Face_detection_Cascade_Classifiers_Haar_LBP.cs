using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using Emgu.CV;
using Emgu.CV.Structure;

namespace EmguCV_async
{
    public partial class Form15_Face_detection_Cascade_Classifiers_Haar_LBP : Form
    {
        Image<Bgr, byte> imgInput;
        public Form15_Face_detection_Cascade_Classifiers_Haar_LBP()
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

        private void detectFaceHaarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (imgInput==null)
                {
                    throw new Exception("Please select image file.");
                }

                DetectFaceHaar();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void DetectFaceHaar()
        {
            try
            {
                string facePath = Path.GetFullPath(@"../../Data/haarcascade_frontalface_default.xml");
                string eyePath = Path.GetFullPath(@"../../Data/haarcascade_eye.xml");

                CascadeClassifier casClassifierFace = new CascadeClassifier(facePath);
                CascadeClassifier casClassifierEye = new CascadeClassifier(eyePath);

                var imgGray = imgInput.Convert<Gray, byte>().Clone();
               Rectangle[] faces= casClassifierFace.DetectMultiScale(imgGray, 1.1, 4);
                foreach (var face in faces)
                {
                    imgInput.Draw(face, new Bgr(0, 0, 255), 3);

                    imgGray.ROI = face;
                    Rectangle[] eyes = casClassifierEye.DetectMultiScale(imgGray, 1.1, 4);

                    
                    foreach (var eye in eyes)
                    {
                        var e = eye;
                        e.X += face.X;
                        e.Y += face.Y;

                        imgInput.Draw(e, new Bgr(0, 255, 255), 2);
                    }
                }
                pictureBox1.Image = imgInput.ToBitmap();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void detectFaceLBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (imgInput == null)
                {
                    throw new Exception("Please select image file.");
                }

                DetectFaceLBP();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void DetectFaceLBP()
        {
            try
            {
                string facePath = Path.GetFullPath(@"../../Data/lbpcascade_frontalface_improved.xml");                

                CascadeClassifier casClassifierFace = new CascadeClassifier(facePath);
                

                var imgGray = imgInput.Convert<Gray, byte>().Clone();
                Rectangle[] faces = casClassifierFace.DetectMultiScale(imgGray, 1.1, 4);
                foreach (var face in faces)
                {
                    imgInput.Draw(face, new Bgr(255, 0, 255), 3);                    
                    
                }
                pictureBox1.Image = imgInput.ToBitmap();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
