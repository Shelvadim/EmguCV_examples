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
    public partial class Form20_Select_ROI_With_Mouse : Form
    {
        Dictionary<String, Image<Bgr, byte>> imgList=new Dictionary<string, Image<Bgr, byte>>();
        Rectangle rect = Rectangle.Empty;
        Point startROI;
        bool selected = false, mousDown;



        public Form20_Select_ROI_With_Mouse()
        {
            InitializeComponent();
        }
         private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                imgList.Clear();
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Image files. | *.jpg; *.bmp";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    var imgInput = new Image<Bgr, byte>(ofd.FileName);
                    AddImage(imgInput, "Input");
                    pictureBox1.Image = imgInput.AsBitmap();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (selected)
            {
                mousDown = true;
                startROI = e.Location;
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (selected)
            {
                int widht = Math.Max(startROI.X, e.X) - Math.Min(startROI.X, e.X);
                int height = Math.Max(startROI.Y, e.Y) - Math.Min(startROI.Y, e.Y);
                rect = new Rectangle(Math.Min(startROI.X, e.X), Math.Min(startROI.Y, e.Y), widht, height);
                Refresh();
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (mousDown)
            {
                using(Pen pen = new Pen(Color.Red, 3))
                {
                    e.Graphics.DrawRectangle(pen, rect);
                }
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (selected)
            {
                selected = false;
                mousDown = false;

            }
        }

        private void getRegionOfROIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (pictureBox1.Image == null || rect==Rectangle.Empty)
                {
                    return;
                }

                var img1 = new Bitmap(pictureBox1.Image).ToImage<Bgr, byte>();
                img1.ROI = rect;
                var imgROI = img1.Copy();
                img1.ROI = Rectangle.Empty;

                pictureBox1.Image = imgROI.ToBitmap();
                AddImage(imgROI, "ROI image");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {                 
                pictureBox1.Image = imgList[e.Node.Text].ToBitmap();                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void selectROIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selected = true;
        }

        private void gausianBlureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (pictureBox1.Image == null || rect == Rectangle.Empty)
                {
                    return;
                }

                var img2 = new Bitmap(pictureBox1.Image).ToImage<Bgr, byte>();
                img2.ROI = rect;
                var imgROI1 = img2.Copy();
                var imgSmooth = imgROI1.SmoothGaussian(25);

                img2.SetValue(new Bgr(1, 1, 1));
                img2._Mul(imgSmooth);


                img2.ROI = Rectangle.Empty;

                pictureBox1.Image = img2.ToBitmap();
                AddImage(img2, "GuassianBlur");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cannyEadgesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (pictureBox1.Image == null || rect == Rectangle.Empty)
                {
                    return;
                }

                var img2 = new Bitmap(pictureBox1.Image).ToImage<Bgr, byte>();
                img2.ROI = rect;
                var imgROI1 = img2.Copy();
                var imgCanny = imgROI1.SmoothGaussian(5).Canny(100,50);
                var imgBgr = imgCanny.Convert<Bgr, byte>();

                img2.SetValue(new Bgr(1, 1, 1));

                img2._Mul(imgBgr);


                img2.ROI = Rectangle.Empty;

                pictureBox1.Image = img2.ToBitmap();
                AddImage(img2, "Canny");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddImage(Image<Bgr, byte> img,string keyName)
        {
            if (!treeView1.Nodes.ContainsKey(keyName))
            {
                TreeNode node = new TreeNode(keyName);
                node.Name = keyName;
                treeView1.Nodes.Add(node);
                treeView1.SelectedNode = node;
            }
            if (!imgList.ContainsKey(keyName))
            {
                imgList.Add(keyName, img);
            }
            else
            {
                imgList[keyName] = img;
            }
        }
    }
}
