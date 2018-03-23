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
    public partial class frmSnipets : Form
    {
        public frmSnipets()
        {
            InitializeComponent();
        }

        private void btnChoose_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<!-- " + txtName.Text + " -->"); 
            sb.AppendLine("<xsl:template name=\"" + txtName.Text + "\" match=\""+txtSelect.Text +"\" >");
            sb.AppendLine("");
            sb.AppendLine("<xsl:variable name=\"" + txtName.Text + "_VarName\"   select =\"" + txtSelect.Text + "\">");
            sb.AppendLine("</xsl:variable>");
            sb.AppendLine("");
            sb.AppendLine("<xsl:choose>");
            int i;
            int iCnt;
            try
            {
                iCnt = Int16.Parse(txtCnt.Text);
            }
            catch
            {
                iCnt = 5;
            }
            char SplitChar = ',';
            string[] Vals;

            int v;

            
            Vals = txtInsert.Text.Split(SplitChar);
            if (Vals.Length != iCnt)
            {
                SplitChar = ';';
                Vals = txtInsert.Text.Split(SplitChar);
            }

            for (i = 0; i < iCnt; i++)
            {
                sb.AppendLine("<xsl:when test=\"$" + txtName.Text + "_VarName=" + i.ToString() +"\" >");
                sb.AppendLine("<!-- " + i.ToString() + " start -->");

                if (Vals.Length > 0 && Vals.Length==iCnt)
                {
                    for (v = 0; v < Vals.Length; v++)
                    {
                        if (v > 0)
                        {
                            sb.Append("<span>"+ SplitChar +"</span>");
                        }

                        if (v == i)
                        {
                            sb.Append("<u><span>" + Vals[v] + "</span></u>");
                        }
                        else
                        {
                            sb.Append("<span>" + Vals[v] + "</span>");
                        }
                    }
                    sb.AppendLine("");
                }
                else { sb.AppendLine(txtInsert.Text); }

                
                sb.AppendLine("<!-- " + i.ToString() + " end -->");
                sb.AppendLine("</xsl:when>");
            }

            sb.AppendLine("<xsl:otherwise >");
            sb.AppendLine("<!-- otherwise start -->");
            sb.AppendLine(txtInsert.Text);
            sb.AppendLine("<!-- otherwise end -->");
            sb.AppendLine("</xsl:otherwise>");

            sb.AppendLine("</xsl:choose>");
            sb.AppendLine("");
            sb.AppendLine("</xsl:template>");
            txtOut.Text = sb.ToString();
        }

        private void cmdRow_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<xsl:choose>");
            sb.AppendLine("  <xls:variable name=\"" + txtName.Text + "_Count\" select =count(" + txtSelect.Text + ")>");
            sb.AppendLine("  </xls:variable>");
            sb.AppendLine("   <!-- no items in expression -->");
            sb.AppendLine("   <xsl:when test=\"$" + txtName.Text + "_Count = 0\">");
            sb.AppendLine("");
            sb.AppendLine("   </xsl:when>");
            sb.AppendLine("   ");
            sb.AppendLine("   <!-- has some items -->");
            sb.AppendLine("   <xsl:otherwise>");
            sb.AppendLine("   ");
            sb.AppendLine("	 <xsl:apply-templates select=\"" + txtSelect.Text + "\" />");
            sb.AppendLine("   ");
            sb.AppendLine("   </xsl:otherwise>");
            sb.AppendLine("</xsl:choose>       ");
            sb.AppendLine("");
            sb.AppendLine("<!-- template for show item data -->");
            sb.AppendLine("<xsl:template name=\"" + txtName.Text + "\"  match=\"" + txtSelect.Text + "\">");
            sb.AppendLine("   <xsl:variable name=\"vPos\" select=\"position()\"/>");
            sb.AppendLine("   <xsl:choose> ");
            sb.AppendLine("	 <!-- '$vPos mod 2'  for odd / even style -->");
            sb.AppendLine("	 <xsl:when test=\"$vPos = 1\">");
            sb.AppendLine("		<!--  first row in set -->");
            sb.AppendLine("	");
            sb.AppendLine("	 </xsl:when> ");
            sb.AppendLine("	 <xsl:otherwise>");
            sb.AppendLine("		<!--  not first row in set -->");
            sb.AppendLine("");
            sb.AppendLine("	 </xsl:otherwise>");
            sb.AppendLine("   </xsl:choose>       ");
            sb.AppendLine("</xsl:template>   ");
            txtOut.Text = sb.ToString();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"<xsl:if 
test  =""" + txtSelect.Text + @" != ''""
><xsl:value-of 
select=""" + txtSelect.Text + @"""/></xsl:if
>");
            txtOut.Text = sb.ToString();
        }
    }
}
