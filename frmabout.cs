using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace QColorPicker
{
    public partial class frmabout : Form
    {
        public frmabout()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void linkLabel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Process p = new Process();
                try
                {
                    p.StartInfo.FileName = "https://github.com/DreamVB/QColorPicker";
                    p.Start();
                    p.Dispose();
                }
                catch
                {

                }
            }
        }
    }
}
