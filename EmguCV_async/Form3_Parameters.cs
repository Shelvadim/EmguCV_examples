using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmguCV_async
{
    public partial class Form3_Parameters : Form
    {
        Form3_ImageSegmentationOverlay form;
        public Form3_Parameters(Form3_ImageSegmentationOverlay f)
        {
            InitializeComponent();
            form = f;
        }

        private void Form3_Parameters_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (form != null)
            {
                form.ApplyRangeFilter(trackBar1.Value, trackBar2.Value);
            }
        }
    }
}
