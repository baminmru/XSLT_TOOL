using System;
using System.Windows.Forms;

namespace xNS
{
    public partial class FrmItem : Form
    {
        public FrmItem()
        {
            InitializeComponent();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}