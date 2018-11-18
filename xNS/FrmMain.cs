using System;
using System.Windows.Forms;

namespace xNS
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            var f = new FrmScaner();
            f.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var f = new FrmPack();
            f.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var f = new FrmSnipets();
            f.Show();
        }

        private void cmdSizer_Click(object sender, EventArgs e)
        {
            var f = new FrmSizercs();
            f.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var f = new FrmSpecMaker();
            f.Show();
        }
    }
}