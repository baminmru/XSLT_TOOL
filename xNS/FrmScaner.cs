using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace xNS
{
    public partial class FrmScaner : Form
    {
        private string[] _find;


        private string[] _ignore;
        private string[] _tails;

        public FrmScaner()
        {
            InitializeComponent();
        }

        private void cmdXML_Click(object sender, EventArgs e)
        {
            opf.Multiselect = false;
            if (opf.ShowDialog() == DialogResult.OK) txtXML.Text = opf.FileName;
        }


        private string SmartString(string s)
        {
            string tmp;
            tmp = s.ToLower();
            tmp = tmp.Replace("_openbrkt_", "__");
            tmp = tmp.Replace("-", "_");
            tmp = tmp.Replace("_closebrkt_", "__");
            tmp = tmp.Replace("_comma_", "__");
            tmp = tmp.Replace("_prd_", "__");
            tmp = tmp.Replace("_fslash_", "__");
            tmp = tmp.Replace(".", "_");
            tmp = tmp.Replace(",", "_");
            tmp = tmp.Replace("(", "_");
            tmp = tmp.Replace(")", "_");

            var ltmp = tmp.Length + 1;
            while (ltmp != tmp.Length)
            {
                ltmp = tmp.Length;
                tmp = tmp.Replace("__", "_");
            }

            if (tmp.StartsWith("_"))
                tmp = tmp.Substring(1);
            if (tmp.EndsWith("_"))
                tmp = tmp.Substring(0, tmp.Length - 1);


            return tmp;
        }

        private bool Finder(string path)
        {
            if (_find == null) return true;
            var test = path.Split('/');
            int i, j, start, idxfound;
            string tmp;

            if (chkSmartPath.Checked)
                for (i = 0; i < _find.Length; i++)
                    _find[i] = SmartString(_find[i]);


            var found = false;
            start = 0;
            idxfound = -1;
            for (i = 0; i < _find.Length; i++)
            {
                for (j = start; j < test.Length; j++)
                {
                    tmp = test[j].ToLower();
                    if (chkSmartPath.Checked) tmp = SmartString(tmp);


                    if (_find[i].ToLower() == tmp)
                    {
                        start = j + 1; // position for next test
                        idxfound = i; // last found index
                        break;
                    }
                }

                if (idxfound < i) return false;
            }

            if (idxfound == _find.Length - 1) found = true;

            if (found)
                if (_tails != null)
                {
                    found = false;
                    int t;
                    int tail;
                    string[] CurTail;
                    for (i = 0; i < _tails.Length; i++)
                    {
                        CurTail = _tails[i].Split('/');
                        idxfound = 0;
                        tail = test.Length - 1;
                        // обратный цикл от хвоста
                        if (CurTail != null)
                            if (CurTail.Length > 0)
                            {
                                for (t = CurTail.Length - 1; t >= 0; t--)
                                for (j = tail; j >= 0; j--) // цикл по проверяемому пути в обратную сторону
                                {
                                    tmp = test[j].ToLower();
                                    if (chkSmartPath.Checked) tmp = SmartString(tmp);


                                    if (CurTail[t].ToLower() == tmp)
                                    {
                                        tail = j - 1; // position for next test
                                        idxfound++; // last found index
                                        break;
                                    }
                                }

                                if (idxfound == CurTail.Length)
                                {
                                    found = true;
                                    break;
                                }
                            }
                    }
                }


            return found;
        }


        // find child nodes for current node
        private List<XmlPlusItem> FindChildList(XmlPlusItem parent, List<XmlPlusItem> pathList, int xIdx)
        {
            int childIdx;
            var childList = new List<XmlPlusItem>();
            //Boolean printNode = true;

            for (childIdx = xIdx + 1; childIdx < pathList.Count; childIdx++)
            {
                var xpi = pathList[childIdx];
                //if (chkTextOnly.Checked)
                //{
                //    if (xpi.node.Attributes.Count > 0 || (xpi.NodeText != null && xpi.NodeText != ""))
                //    {
                //        printNode = true;
                //    }
                //}
                //else
                //{
                //    printNode = true;
                //}

                //if (printNode)
                {
                    var ok = true;
                    if (_ignore != null)
                        foreach (var s in _ignore)
                            if (s.Length > 0)
                                if (xpi.Path.IndexOf(s) >= 0)
                                {
                                    ok = false;
                                    break;
                                }

                    if (ok) ok = Finder(xpi.Path);

                    if (ok)
                        if (xpi.PathNs.StartsWith(parent.PathNs))
                            childList.Add(xpi);
                }
            }

            return childList;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            txtState.Text = @"scan started";

            if (txtIgnore.Text != "")
                _ignore = txtIgnore.Text.Split(';');
            else
                _ignore = null;

            if (txtFind.Text != "")
                _find = txtFind.Text.Split('/');
            else
                _find = null;

            if (txtTail.Text != "")
                _tails = txtTail.Text.Split(';');
            else
                _tails = null;


            if (txtXML.Text != "")
            {
                var xdoc = new XmlDocument();
                try
                {
                    xdoc.Load(txtXML.Text);

                    var pathList = xdoc.IterateThroughAllNodes(txtNS.Text);
                    var sb = new StringBuilder();
                    bool printNode;
                    int xIdx;
                    for (xIdx = 0; xIdx < pathList.Count; xIdx++)
                    {
                        var xpi = pathList[xIdx];

                        printNode = false;

                        if (chkTextOnly.Checked)
                        {
                            if (xpi.Node.Attributes.Count > 0 || xpi.NodeText != null && xpi.NodeText != "")
                                printNode = true;
                        }
                        else
                        {
                            printNode = true;
                        }

                        if (printNode)
                        {
                            var ok = true;
                            if (_ignore != null)
                                foreach (var s in _ignore)
                                    if (s.Length > 0)
                                        if (xpi.Path.IndexOf(s) >= 0)
                                        {
                                            ok = false;
                                            break;
                                        }

                            if (ok) ok = Finder(xpi.Path);

                            if (ok)
                            {
                                sb.AppendLine(
                                    "<!-- ------------------------------------------------------------------ -->");
                                sb.AppendLine("<!-- XPath    : --> " + xpi.Path);
                                sb.AppendLine("");
                                sb.AppendLine("<!-- Node     : --> " + XmlTools.NodeSelf(xpi.Node));
                                sb.AppendLine("");
                                if (chkXPATH.Checked)
                                {
                                    sb.AppendLine("<!-- XPath+NS : --> " + xpi.PathNs);
                                    sb.AppendLine("");
                                }

                                if (chkTemplate.Checked)
                                {
                                    sb.Append("<!-- Template : -->  <xsl:template match=\"" + xpi.PathNs + "\">");

                                    if (xpi.Node.Attributes.Count > 0)
                                        sb.Append("<xsl:value-of select=\"@" + xpi.Node.Attributes[0].Name + "\"/>");

                                    if (xpi.NodeText != null && xpi.NodeText != "")
                                        sb.Append("<xsl:value-of select=\".\"/>");
                                    sb.AppendLine("</xsl:template>");
                                    sb.AppendLine("");
                                }


                                // if (xpi.NodeText != null && xpi.NodeText != "")
                                {
                                    List<XmlPlusItem> childList = null;
                                    if (chkIf.Checked || chkIfPost.Checked)
                                    {
                                        childList = FindChildList(xpi, pathList, xIdx);
                                        if (childList.Count > 0) Debug.Print(xpi.PathNs);
                                    }


                                    if (chkDirect.Checked)
                                    {
                                        sb.Append("<!-- Direct   : --> ");
                                        sb.AppendLine("<xsl:value-of select=\"" + xpi.PathNs + "\"/>");
                                        sb.AppendLine("");
                                    }

                                    if (chkIfPost.Checked)
                                    {
                                        sb.Append(" < !-- If Post  : --> ");

                                        if (childList != null && childList.Count > 0)
                                        {
                                            sb.AppendLine(@"<xsl:if test  =""" + xpi.PathNs + @" != '' ");
                                            foreach (var child in childList)
                                                sb.AppendLine(@" or " + child.PathNs + @" != '' ");
                                            sb.AppendLine(@"""  > ");

                                            sb.AppendLine(@"<xsl:call-template name=""PostProcess"">
<xsl:with-param name=""size"" select=""0"" />
<xsl:with-param name=""string""  select=""" + xpi.PathNs + @"""/></xsl:call-template>");
                                            sb.AppendLine(@"</xsl:if>");
                                        }
                                        else
                                        {
                                            sb.AppendLine(@"<xsl:if test  =""" + xpi.PathNs + @" != ''"">");
                                            sb.AppendLine(@"<xsl:call-template name=""PostProcess"">
<xsl:with-param name=""size"" select=""0"" />
<xsl:with-param name=""string""  select=""" + xpi.PathNs + @"""/></xsl:call-template>");
                                            sb.AppendLine(@"</xsl:if>");
                                        }

                                        sb.AppendLine("");
                                    }

                                    if (chkPostProcess.Checked)
                                    {
                                        sb.Append(" < !-- Post Prc.: --> ");
                                        sb.AppendLine(
                                            @"<xsl:call-template name=""PostProcess""><xsl:with-param name=""string"" select=""" +
                                            xpi.PathNs +
                                            @""" /><xsl:with-param name=""size"" select=""0"" /></xsl:call-template>");
                                        sb.AppendLine("");
                                    }


                                    if (chkIf.Checked)
                                    {
                                        sb.Append("<!-- If-value : --> ");


                                        if (childList != null && childList.Count > 0)
                                        {
                                            sb.AppendLine(@"<xsl:if test  =""" + xpi.PathNs + @" != '' ");
                                            foreach (var child in childList)
                                                sb.AppendLine(@" or " + child.PathNs + @" != '' ");
                                            sb.AppendLine(@"""   ");

                                            sb.AppendLine(@" > " + txtAddText.Text + @"<xsl:value-of 
select=""" + xpi.PathNs + @"""/></xsl:if
>");
                                        }
                                        else
                                        {
                                            sb.AppendLine(@"<xsl:if 
test  =""" + xpi.PathNs + @" != ''""
>" + txtAddText.Text + @"<xsl:value-of 
select=""" + xpi.PathNs + @"""/></xsl:if
>");
                                        }

                                        sb.AppendLine("");
                                    }

                                    if (chkFor.Checked)
                                    {
                                        sb.Append("<!-- For      : --> ");
                                        sb.AppendLine(@"<xsl:for-each 
select=""" + xpi.PathNs + @""">
" + txtAddText.Text + @"<xsl:value-of select ="".""/></xsl:for-each>");
                                        sb.AppendLine("");
                                    }
                                }


                                if (xpi.NodeText != null && xpi.NodeText != "")
                                {
                                    sb.AppendLine("<!-- Node text: --> <!-- " + xpi.NodeText + " -->");
                                    sb.AppendLine("");
                                }

                                sb.AppendLine();
                            }
                        }
                    }

                    txtOut.Text = sb.ToString();
                    txtState.Text = @"scan finished";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                txtState.Text = @"xml file not selected";
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
                var s = Clipboard.GetText(TextDataFormat.Text);
                s = s.Replace("\r", "");
                s = s.Replace("\n", "");
                if (chkSmartPath.Checked) s = SmartString(s);
                txtFind.Text = s;
                btnStart_Click(sender, e);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtTail.Text =
                @"value/value;value/rm:value;value/magnitude;value/units;value/rm:defining_code/rm:code_string";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtTail.Text = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            txtIgnore.Text =
                @"mappings;links;language;encoding;provider;subject;other_participations;context;setting;uid;composer";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            txtIgnore.Text = "";
        }

        private void txtXML_TextChanged(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void opf_FileOk(object sender, CancelEventArgs e)
        {
        }
    }
}