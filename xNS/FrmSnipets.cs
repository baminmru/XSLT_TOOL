using System;
using System.Text;
using System.Windows.Forms;

namespace xNS
{
    public partial class FrmSnipets : Form
    {
        public FrmSnipets()
        {
            InitializeComponent();
        }

        private void btnChoose_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<!-- " + txtName.Text + " -->");
            sb.AppendLine("<xsl:template name=\"" + txtName.Text + "\" match=\"" + txtSelect.Text + "\" >");
            sb.AppendLine("");
            sb.AppendLine("<xsl:variable name=\"" + txtName.Text + "_VarName\"   select =\"" + txtSelect.Text + "\">");
            sb.AppendLine("</xsl:variable>");
            sb.AppendLine("");
            sb.AppendLine("<xsl:choose>");
            int i;
            int iCnt;
            try
            {
                iCnt = short.Parse(txtCnt.Text);
            }
            catch
            {
                iCnt = 5;
            }

            var splitChar = ',';
            string[] vals;

            int v;


            vals = txtInsert.Text.Split(splitChar);
            if (vals.Length != iCnt)
            {
                splitChar = ';';
                vals = txtInsert.Text.Split(splitChar);
            }

            for (i = 0; i < iCnt; i++)
            {
                sb.AppendLine("<xsl:when test=\"$" + txtName.Text + "_VarName=" + i + "\" >");
                sb.AppendLine("<!-- " + i + " start -->");

                if (vals.Length > 0 && vals.Length == iCnt)
                {
                    for (v = 0; v < vals.Length; v++)
                    {
                        if (v > 0) sb.Append("<span>" + splitChar + "</span>");

                        if (v == i)
                            sb.Append("<u><span>" + vals[v] + "</span></u>");
                        else
                            sb.Append("<span>" + vals[v] + "</span>");
                    }

                    sb.AppendLine("");
                }
                else
                {
                    sb.AppendLine(txtInsert.Text);
                }


                sb.AppendLine("<!-- " + i + " end -->");
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
            var sb = new StringBuilder();
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
            var sb = new StringBuilder();
            sb.AppendLine(@"<xsl:if 
test  =""" + txtSelect.Text + @" != ''""
><xsl:value-of 
select=""" + txtSelect.Text + @"""/></xsl:if
>");
            txtOut.Text = sb.ToString();
        }
    }
}