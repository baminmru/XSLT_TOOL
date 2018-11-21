using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xNS
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            frmScaner f = new frmScaner();
            f.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmPack f = new frmPack();
            f.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            frmSnipets f = new frmSnipets();
            f.Show();
        }

        private void cmdSizer_Click(object sender, EventArgs e)
        {
            frmSizercs f = new frmSizercs();
            f.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            frmSpecMaker f = new frmSpecMaker();
            f.Show();
        }

      
    }
}
