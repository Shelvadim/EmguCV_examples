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
    public partial class Form1_async : Form
    {
        private Image<Bgr, byte> imgInput = null;
        private OpenFileDialog ofd = null;
        public Form1_async()
        {
            InitializeComponent();
        }

        private void openSyncToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "TIF|*.tif";

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

        private void openAsyncToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ofd = new OpenFileDialog();
                ofd.Filter = "TIF|*.tif";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    BackgroundWorker bw = new BackgroundWorker();
                    bw.WorkerReportsProgress = false;
                    bw.WorkerSupportsCancellation = false;
                    bw.DoWork += Bw_DoWork;
                    bw.RunWorkerCompleted += Bw_RunWorkerCompleted;

                    bw.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pictureBox1.Image = imgInput.ToBitmap();
        }

        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            imgInput = new Image<Bgr, byte>(ofd.FileName);
        }
    }
}
