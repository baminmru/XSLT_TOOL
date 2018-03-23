using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace xNS
{
    public partial class frmScaner : Form
    {
        public frmScaner()
        {
            InitializeComponent();
        }

        private void cmdXML_Click(object sender, EventArgs e)
        {
            opf.Multiselect = false;
            if(opf.ShowDialog()==DialogResult.OK)
            {
                txtXML.Text = opf.FileName;
            }
        }


        string [] Ignore;
        string [] Find;
        string [] Tails;


        private string SmartString(string s)
        {
            string tmp;
            tmp = s.ToLower();
            tmp = tmp.Replace("_openbrkt_", "__");
            tmp = tmp.Replace("_closebrkt_", "__");
            tmp = tmp.Replace("_comma_", "__");
            tmp = tmp.Replace("_prd_", "__");
            int ltmp = tmp.Length + 1;
            while (ltmp != tmp.Length)
            {
                ltmp = tmp.Length;
                tmp = tmp.Replace("__", "_");
            }
            return tmp;
        }

        private Boolean Finder(string Path)
        {
            if (Find == null) return true;
            string[] Test=Path.Split('/');
            int i, j, start, idxfound;
            string tmp;

            if (chkSmartPath.Checked)
            {
                for (i = 0; i < Find.Length; i++)
                {
                    Find[i] = SmartString(Find[i]);
                }
            }

            
            Boolean Found = false;
            start = 0;
            idxfound = -1;
            for (i = 0; i < Find.Length; i++)
            {
                for (j = start; j < Test.Length; j++)
                {

                    tmp = Test[j].ToLower();
                    if (chkSmartPath.Checked)
                    {
                        tmp = SmartString(tmp);
                    }


                    if (Find[i].ToLower() == tmp)
                    {
                        start = j+1; // position for next test
                        idxfound = i; // last found index
                        break;
                    }
                }
                if (idxfound < i)
                {
                    return false;
                }


            }
            if (idxfound == Find.Length - 1)
            {
                Found= true;
            }

            if (Found)
            {
                if (Tails!=null)
                {
                    Found = false;
                    int t;
                    int tail;
                    string [] CurTail;
                    for (i = 0; i < Tails.Length; i++)
                    {
                        CurTail = Tails[i].Split('/');
                        idxfound = 0;
                        tail = Test.Length-1;
                        // обратный цикл от хвоста
                        if (CurTail != null)
                        {
                            if (CurTail.Length > 0)
                            {
                                for (t = CurTail.Length - 1; t >= 0; t--)
                                {
                                    for (j = tail; j >= 0; j--) // цикл по проверяемому пути в обратную сторону
                                    {

                                        tmp = Test[j].ToLower();
                                        if (chkSmartPath.Checked)
                                        {
                                            tmp = SmartString(tmp);
                                        }


                                        if (CurTail[t].ToLower() == tmp)
                                        {
                                            tail = j - 1; // position for next test
                                            idxfound++; // last found index
                                            break;
                                        }
                                    }
                                }

                                if (idxfound == CurTail.Length)
                                {
                                    Found = true;
                                    break;
                                }
                            }
                        }


                    }
                }
            }



            return Found;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            txtState.Text = "scan started";

            if (txtIgnore.Text != "")
            {
                Ignore = txtIgnore.Text.Split(';');
            }else
            {
                Ignore = null;
            }

            if (txtFind.Text != "")
            {
                Find = txtFind.Text.Split('/');
            }
            else
            {
                Find = null;
            }

            if (txtTail.Text != "")
            {
                Tails = txtTail.Text.Split(';');
            }
            else
            {
                Tails = null;
            }


            if (txtXML.Text != "")
            {
                XmlDocument xdoc = new XmlDocument();
                try
                {
                    xdoc.Load(txtXML.Text);

                    List<XmlPlusItem> PathList = XmlTools.IterateThroughAllNodes(xdoc, txtNS.Text);
                    StringBuilder sb = new StringBuilder();
                    Boolean printNode = true;
                    foreach (XmlPlusItem xpi in PathList)
                    {
                        printNode = false;

                        if (chkTextOnly.Checked) {
                            if (xpi.node.Attributes.Count > 0 || (xpi.NodeText != null && xpi.NodeText != ""))
                            {
                                printNode = true;
                                
                            }

                        }
                        else
                        {
                            printNode = true;
                        }

                        if (printNode)
                        {
                            Boolean ok=true;
                            if (Ignore != null)
                            {
                                foreach (string s in Ignore)
                                {
                                    if (s.Length > 0)
                                    {
                                        if (xpi.Path.IndexOf(s) >= 0)
                                        {
                                            ok = false;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (ok)
                            {
                                ok = Finder(xpi.Path);
                            }
                            if (ok)
                            {
                                sb.AppendLine("<!-- ------------------------------------------------------------------ -->");
                                sb.AppendLine("<!-- XPath    : --> " + xpi.Path);
                                sb.AppendLine("");
                                sb.AppendLine("<!-- Node     : --> " + XmlTools.NodeSelf(xpi.node));
                                sb.AppendLine("");
                                if (chkXPATH.Checked)
                                {
                                    sb.AppendLine("<!-- XPath+NS : --> " + xpi.PathNs);
                                    sb.AppendLine("");
                                }
                                if (chkTemplate.Checked)
                                {
                                    sb.Append("<!-- Template : -->  <xsl:template match=\"" + xpi.PathNs + "\">");

                                    if (xpi.node.Attributes.Count > 0)
                                    {
                                        sb.Append("<xsl:value-of select=\"@" + xpi.node.Attributes[0].Name + "\"/>");
                                    }

                                    if (xpi.NodeText != null && xpi.NodeText != "")
                                    {
                                        sb.Append("<xsl:value-of select=\".\"/>");
                                    }
                                    sb.AppendLine("</xsl:template>");
                                    sb.AppendLine("");
                                }
                                

                                if (xpi.NodeText != null && xpi.NodeText != "")
                                {
                                    if (chkDirect.Checked)
                                    {
                                        sb.Append("<!-- Direct   : --> ");
                                        sb.AppendLine("<xsl:value-of select=\"" + xpi.PathNs + "\"/>");
                                        sb.AppendLine("");
                                    }

                                    if (chkIfPost.Checked)
                                    {
                                        sb.Append(" < !-- If Post  : --> ");
                                        sb.AppendLine(@"<xsl:if test  =""" + xpi.PathNs + @" != ''"">");
                                        sb.AppendLine(@"<xsl:call-template name=""PostProcess""><xsl:with-param name=""size"" select=""0"" /><xsl:with-param name=""string""  select=""" + xpi.PathNs + @"""/></xsl:call-template>");
                                        sb.AppendLine(@"</xsl:if>");
                                        sb.AppendLine("");
                                    }

                                    if (chkPostProcess.Checked)
                                    {
                                        sb.Append(" < !-- Post Prc.: --> ");
                                        sb.AppendLine(@"<xsl:call-template name=""PostProcess""><xsl:with-param name=""string"" select=""" + xpi.PathNs + @""" /><xsl:with-param name=""size"" select=""0"" /></xsl:call-template>");
                                        sb.AppendLine("");
                                    }


                                    if (chkIf.Checked)
                                    {
                                        sb.Append("<!-- If-value : --> ");
                                        sb.AppendLine(@"<xsl:if 
test  =""" + xpi.PathNs + @" != ''""
>" + txtAddText.Text + @"<xsl:value-of 
select=""" + xpi.PathNs + @"""/></xsl:if
>");
                                        sb.AppendLine("");
                                    }

                                    if (chkFor.Checked)
                                    {
                                        sb.Append("<!-- For      : --> ");
                                        sb.AppendLine(@"<xsl:for-each 
select="""+ xpi.PathNs + @""">
" + txtAddText.Text + @"<xsl:value-of select ="".""/></xsl:for-each>");
                                        sb.AppendLine("");
                                    }
                                }

                               
                                if (xpi.NodeText != null && xpi.NodeText != "")
                                {
                                    sb.AppendLine("<!-- Node text: --> <!-- " + xpi.NodeText +" -->");
                                    sb.AppendLine("");
                                }
                                sb.AppendLine();
                            }
                        }
                    }
                    txtOut.Text = sb.ToString();
                    txtState.Text= "scan finished";
                }catch(System.Exception ex )
                {
                    MessageBox.Show(ex.Message);
                }

            }
            else
            {
                txtState.Text = "xml file not selected";
            }
        }

        

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtFind.Text = "";
            if (Clipboard.ContainsText())
            {
                string s = Clipboard.GetText(TextDataFormat.Text);
                s = s.Replace("\r", "");
                s = s.Replace("\n", "");
                if (chkSmartPath.Checked)
                {
                    s = SmartString(s);
                }
                txtFind.Text = s;
                btnStart_Click(sender,e);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtTail.Text = "value/value;value/rm:value;value/magnitude;value/units;value/rm:defining_code/rm:code_string";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtTail.Text = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            txtIgnore.Text = "mappings;links;language;encoding;provider;subject;other_participations;context;setting;uid;composer";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            txtIgnore.Text = "";
        }
    }
}

