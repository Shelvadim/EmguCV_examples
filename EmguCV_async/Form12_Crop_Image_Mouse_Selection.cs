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
    public partial class Form12_Crop_Image_Mouse_Selection : Form
    {
        Image<Bgr, byte> imgInput = null;
        Rectangle rect;
        Point startLocation;
        Point endLocation;
        bool isMouseDown = false;

        public Form12_Crop_Image_Mouse_Selection()
        {
            InitializeComponent();
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

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

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            startLocation = e.Location;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown == true)
            {
                endLocation = e.Location;
                pictureBox1.Invalidate();
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (rect != null)
            {
                e.Graphics.DrawRectangle(Pens.Red, GetRectangle());
            }
        }

        private Rectangle GetRectangle()
        {
            rect = new Rectangle();
            rect.X = Math.Min(startLocation.X, endLocation.X);
            rect.Y = Math.Min(startLocation.Y, endLocation.Y);
            rect.Width = Math.Abs(startLocation.X - endLocation.X);
            rect.Height = Math.Abs(startLocation.Y- endLocation.Y);

            return rect;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (isMouseDown == true)
            {
                endLocation = e.Location;
                isMouseDown = false;

                if (rect != null)
                {
                    imgInput.ROI = rect;
                    Image<Bgr, byte> temp = imgInput.CopyBlank();
                    imgInput.CopyTo(temp);
                    imgInput.ROI = Rectangle.Empty;
                    pictureBox2.Image = temp.ToBitmap();
                }
            }
        }
    }
}
