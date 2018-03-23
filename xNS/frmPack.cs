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

namespace xNS
{
    public partial class frmPack : Form
    {
        public frmPack()
        {
            InitializeComponent();
        }

        private void cmdXML_Click(object sender, EventArgs e)
        {
            opf.Multiselect = false;
            if (opf.ShowDialog() == DialogResult.OK)
            {
                txtXML.Text = opf.FileName;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            String fdata;
            fdata = File.ReadAllText(txtXML.Text);
            string nf;
            nf = fdata;
            nf = nf.Replace('\r', ' ');
            nf = nf.Replace('\n', ' ');
            int slen;
            slen = 0;
            while(slen != nf.Length)
            {
                slen = nf.Length;
                nf =nf.Replace("  ", " ");
            }

          
            nf = nf.Replace("match=", "\nmatch  =");
            nf = nf.Replace("test=",   "\ntest  =");
            nf = nf.Replace("name=",   "\nname  =");
            nf = nf.Replace("select=", "\nselect=");
            nf = nf.Replace("<span> ", "<span>&#160;");
            nf = nf.Replace("> . ", ">. ");
            nf = nf.Replace("> , ", ">, ");
            nf = nf.Replace("</span> ", "</span>");
            //nf = nf.Replace("<xsl:for-each", "<xsl:for-each\n");
            //nf = nf.Replace("<xsl:value-of", "<xsl:value-of\n");
            //nf = nf.Replace("<xsl:variable", "<xsl:variable\n");
            //nf = nf.Replace("<xsl:param", "<xsl:param\n");
            nf = nf.Replace("<xsl:template", "<xsl:template\n");
            nf = nf.Replace("<xsl:call-template", "<xsl:call-template\n");
            nf = nf.Replace("> <xsl:", "><xsl:");
            nf = nf.Replace("> </xsl:", "></xsl:");
            nf = nf.Replace(" <!--", "<!--");
            nf = nf.Replace("<!--", "<!--\n");
            nf = nf.Replace("-->", "\n-->");
            nf = nf.Replace("--> <xsl", "--><xsl");
            nf = nf.Replace("> , ", ">, ");
            //nf = nf.Replace("<xsl:if", "<xsl:if\n");
            nf = nf.Replace("</xsl:if", "</xsl:if\n");
            nf = nf.Replace("</xsl:for-each", "</xsl:for-each\n");
            nf = nf.Replace("</xsl:variable", "</xsl:variable\n");
            nf = nf.Replace("</xsl:param", "</xsl:param\n");
            nf = nf.Replace("</xsl:template", "</xsl:template\n");
            nf = nf.Replace("</xsl:apply-template", "</xsl:apply-template\n");
            nf = nf.Replace("</xsl:call-template", "</xsl:call-template\n");
            nf = nf.Replace("> <span>", "><span>");
            nf = nf.Replace("> <br", "><br");
            nf = nf.Replace("\">", "\"\n>");
            nf = nf.Replace("> <strong>", "><strong>");

            txtOut.Text = nf;
        }

        private void frmPack_Load(object sender, EventArgs e)
        {

        }
    }
}
