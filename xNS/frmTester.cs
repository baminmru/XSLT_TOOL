using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;
using Saxon.Api;
using System.Text.RegularExpressions;

namespace xNS
{
    public partial class frmTester : Form
    {
        public frmTester()
        {
            InitializeComponent();
        }

        private void cmdSelectFile_Click(object sender, EventArgs e)
        {
            opf.Filter = "XSD files|*.xsd|All files|*.*";
            if (opf.ShowDialog() == DialogResult.OK)
            {
                txtXSD.Text = opf.FileName;
            }
        }

        private void cmdXML_Click(object sender, EventArgs e)
        {
            opf.Filter = "XSLT files|*.xslt|All files|*.*";
            if (opf.ShowDialog() == DialogResult.OK)
            {
                txtXSLT.Text = opf.FileName;
            }
        }

        private void cmdRun_Click(object sender, EventArgs e)
        {
          
            string xmlPath = BuildXML((int)prcNum.Value);
            FileInfo fi = new FileInfo(txtXSD.Text);
            string htmlPath = fi.DirectoryName + "\\output_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".html";
            string errPath = fi.DirectoryName + "\\error_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";

            var input = new FileInfo(xmlPath);
            var output = new FileInfo(htmlPath);
          

            // Compile stylesheet
            var processor = new Processor();
            var compiler = processor.NewXsltCompiler();
            var executable = compiler.Compile(new Uri(txtXSLT.Text));

            // Do transformation to a destination
            var destination = new DomDestination();
            using (var inputStream = input.OpenRead())
            {
                var transformer = executable.Load();
                transformer.SetInputStream(inputStream, new Uri(input.DirectoryName));
                transformer.Run(destination);
            }
            destination.XmlDocument.Save(output.FullName);

            StringBuilder sError = new StringBuilder(); 
            string sHtml = File.ReadAllText(output.FullName);
            sHtml = sHtml.Replace(" ", " ");
            sHtml = sHtml.Replace("&nbsp;", " ");
            sHtml = sHtml.Replace('\t', ' ');
            sHtml = sHtml.Replace('\r', ' ');
            sHtml = sHtml.Replace('\n', ' ');
            sHtml = sHtml.Replace("<br/>", " ");
            sHtml = sHtml.Replace("<br />", " ");

            int sLen;
            int eStart;
            int eStop;
            int pos;
            int bodyPos;
            int gap = 110;

            txtError.Text = "";
           sLen = sHtml.Length+1;
            while( sLen != sHtml.Length)
            {
                sLen = sHtml.Length;
                sHtml = sHtml.Replace("  ", " ");
            }

            sHtml = sHtml.Replace("<span>", "");
            sHtml = sHtml.Replace("</span>", "");
            sHtml = sHtml.Replace("<strong>", "");
            sHtml = sHtml.Replace("</strong>", "");

            bodyPos = sHtml.IndexOf("<body");
            if (bodyPos < 0) bodyPos = 0;

            if (sHtml.Contains(". ."))
            {
               
                pos = sHtml.IndexOf(". .", bodyPos);
                do
                {
                    if (pos >= 0)
                    {
                        eStart = pos - gap;
                        eStop = pos + gap;
                        if (eStart < 0) eStart = 0;
                        if (eStop >= sHtml.Length) eStop = sHtml.Length - 1;
                        sError.AppendLine("{. .}  ..." + sHtml.Substring(eStart, eStop - eStart + 1));
                        pos = sHtml.IndexOf(". .", pos + 1);
                    }
                } while (pos >= 0);
            }


            if (sHtml.Contains(" ."))
            {

                
                pos = sHtml.IndexOf(" .", bodyPos);
                do
                {
                    if (pos >= 0)
                    {
                        eStart = pos - gap;
                        eStop = pos + gap;
                        if (eStart < 0) eStart = 0;
                        if (eStop >= sHtml.Length) eStop = sHtml.Length - 1;
                        sError.AppendLine("{ .}  ..." + sHtml.Substring(eStart, eStop - eStart + 1));
                        pos = sHtml.IndexOf(" .", pos + 1);
                    }
                } while (pos >= 0);
            }

            if (sHtml.Contains(" .<"))
            {

                pos = sHtml.IndexOf(" .<", bodyPos);
                do
                {
                    if (pos >= 0)
                    {
                        eStart = pos - gap;
                        eStop = pos + gap;
                        if (eStart < 0) eStart = 0;
                        if (eStop >= sHtml.Length) eStop = sHtml.Length - 1;
                        sError.AppendLine("{ .< }  ..." + sHtml.Substring(eStart, eStop - eStart + 1));
                        pos = sHtml.IndexOf(" .<", pos + 1);
                    }
                } while (pos >= 0);
            }

            if (sHtml.Contains("<td>,"))
            {

                pos = sHtml.IndexOf("<td>,", bodyPos);
                do
                {
                    if (pos >= 0)
                    {
                        eStart = pos - gap;
                        eStop = pos + gap;
                        if (eStart < 0) eStart = 0;
                        if (eStop >= sHtml.Length) eStop = sHtml.Length - 1;
                        sError.AppendLine("{<td>,}  ..." + sHtml.Substring(eStart, eStop - eStart + 1));
                        pos = sHtml.IndexOf("<td>,", pos + 1);
                    }
                } while (pos >= 0);
            }

            if (sHtml.Contains(". ,"))
            {
                pos = sHtml.IndexOf(". ,", bodyPos);
                do
                {
                    if (pos >= 0)
                    {
                        eStart = pos - gap;
                        eStop = pos + gap;
                        if (eStart < 0) eStart = 0;
                        if (eStop >= sHtml.Length) eStop = sHtml.Length - 1;
                        sError.AppendLine("{. ,}  ..." + sHtml.Substring(eStart, eStop - eStart + 1));
                        pos = sHtml.IndexOf(". ,", pos + 1);
                    }
                } while (pos >= 0);
            }

            if (sHtml.Contains(": ,"))
            {
                pos = sHtml.IndexOf(": ,", bodyPos);
                do
                {
                   
                    eStart = pos - gap;
                    eStop = pos + gap;
                    if (eStart < 0) eStart = 0;
                    if (eStop >= sHtml.Length) eStop = sHtml.Length - 1;
                    sError.AppendLine("{: ,}  ..." + sHtml.Substring(eStart, eStop - eStart + 1));
                    pos = sHtml.IndexOf(": ,", pos + 1);
                    
                } while (pos >= 0);
            }
            {
                Regex regex = new Regex(@"\.\s*[а-я]");
                MatchCollection matches = regex.Matches(sHtml, bodyPos);
                if (matches.Count > 0)
                {
                    foreach (Match match in matches)
                    {
                        
                        eStart = match.Index - gap;
                        eStop = match.Index + gap;
                        if (eStart < 0) eStart = 0;
                        if (eStop >= sHtml.Length) eStop = sHtml.Length - 1;
                        sError.AppendLine("{" + match.Value + "} ..." + sHtml.Substring(eStart, eStop - eStart + 1));
                       
                    }

                }
            }
            {
                Regex regex = new Regex(@":\s*[А-Я]");
                MatchCollection matches = regex.Matches(sHtml,bodyPos);
                if (matches.Count > 0)
                {
                    foreach (Match match in matches)
                    {
                        if (match.Index > bodyPos)
                        {
                            eStart = match.Index - gap;
                            eStop = match.Index + gap;
                            if (eStart < 0) eStart = 0;
                            if (eStop >= sHtml.Length) eStop = sHtml.Length - 1;
                            sError.AppendLine("{" + match.Value + "} ..." + sHtml.Substring(eStart, eStop - eStart + 1));
                        }
                    }

                }
            }


            if (sError.ToString() != "") {
                File.WriteAllText(errPath, sError.ToString());
                txtError.Text = sError.ToString(); 
            }
            wb.Navigate(htmlPath);

        }



        private List<String> StopStr;
        private XmlNamespaceManager nsmgr;

        private string BuildXML(int RandomPrc)
        {
            StopStr = new List<String>();
            string sIg = "state;magnitude_status;math_function;origin;time;width;normal_status;other_reference_ranges;normal_range;null_flavour;terminology_id;mappings;links;language;encoding;provider;subject;other_participations;context;setting;uid;composer;territory;category;context;Любое_событие_as_Interval_Event";

            String[] stops = sIg.ToLower().Split(';');

            foreach (string s in stops)
            {
                StopStr.Add(s);
            }



            XmlDocument xDoc = new XmlDocument();

            string sXSD = File.ReadAllText(txtXSD.Text);

            //  patch Factor problem
            sXSD = sXSD.Replace("Провоцирующий_fslash_купирующий_фактор", "Провоцирующий_фактор");
            sXSD = sXSD.Replace("&lt;", "_меньше_");
            sXSD = sXSD.Replace("&gt;", "_больше_");

            xDoc.LoadXml(sXSD);

            nsmgr = new XmlNamespaceManager(xDoc.NameTable);
            nsmgr.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");
            //nsmgr.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");

            XmlElement xe;
            StringBuilder sb = new StringBuilder();

            xsdItem root = new xsdItem();

            XmlNodeList rootElements = xDoc.LastChild.ChildNodes;
            XmlElement rootEl;

            foreach (XmlNode node in rootElements)
            {
                if (node.Name == "xs:element")
                {
                    rootEl = (XmlElement)node;
                    Name = rootEl.GetAttribute("name");

                    root.Name = Name;
                    root.Type = "root";

                    readChild(root, rootEl);
                }
            }

            root.RestoreParent();

            string sOut;
            string testName;
           
            FileInfo fi = new FileInfo(txtXSD.Text);
            testName = fi.DirectoryName + "\\test_" + RandomPrc.ToString() + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xml";
                

            xsdItem.RandomPercent = RandomPrc;
            sOut = root.Generate(null).ToString();
            File.WriteAllText(testName, sOut);
            return testName; // sOut;
        }

        private string processRestrictions(string res)
        {
            string sOut = "";
            string[] stringSeparators = new string[] { "<!--", "-->" };
            string[] items = res.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in items)
            {
                if (s.Contains("Value = "))
                {
                    if (sOut != "") sOut += ";";
                    sOut += s.Replace("Value = ", "").Trim();

                }
            }
            return sOut;
        }

        private void readChild(xsdItem xsd, XmlElement el)
        {
            XmlNodeList ct = el.SelectNodes("./xs:complexType", nsmgr);
            foreach (XmlNode node in ct)
            {

                XmlElement el2 = (XmlElement)node;
                XmlNodeList sq = el2.SelectNodes("./xs:sequence", nsmgr);
                foreach (XmlNode node2 in sq)
                {

                    XmlElement el3 = (XmlElement)node2;
                    XmlNodeList children = el3.SelectNodes("./xs:element", nsmgr);
                    foreach (XmlNode node3 in children)
                    {

                        XmlElement el4 = (XmlElement)node3;


                        xsdItem xsdChild = new xsdItem();
                        try { xsdChild.Name = el4.GetAttribute("name"); }
                        catch { }

                        if (xsdChild.Name != "")
                        {
                            if (!StopStr.Contains(xsdChild.Name.ToLower()))
                            {
                                try { xsdChild.Type = el4.GetAttribute("type"); }
                                catch { xsdChild.Type = ""; }

                                if (xsdChild.Type == "")
                                {
                                    XmlNodeList restricts = el4.SelectNodes("./xs:simpleType/xs:restriction", nsmgr);
                                    if (restricts != null && restricts.Count > 0)
                                    {
                                        XmlElement r = (XmlElement)restricts[0];
                                        try { xsdChild.Type = r.GetAttribute("base"); }
                                        catch { xsdChild.Type = ""; }

                                        XmlNodeList pattern = r.SelectNodes("xs:pattern", nsmgr);

                                        foreach (XmlNode pn in pattern)
                                        {
                                            XmlElement p = (XmlElement)pn;
                                            try { xsdChild.Patterns.Add(p.GetAttribute("value")); }
                                            catch { }
                                        }

                                        XmlNodeList ens = r.SelectNodes("xs:enumeration", nsmgr);

                                        if (ens.Count > 0)
                                        {
                                            string R = "";
                                            foreach (XmlNode pn in ens)
                                            {
                                                if (R != "") R += ";";

                                                XmlElement p = (XmlElement)pn;
                                                try { R += p.GetAttribute("value"); }
                                                catch { }
                                            }
                                            if (R != "")
                                            {
                                                xsdChild.Restrictions = R;
                                            }
                                        }

                                    }
                                }


                                try { xsdChild.oMin = el4.GetAttribute("minOccurs"); }
                                catch { }

                                try { xsdChild.oMax = el4.GetAttribute("maxOccurs"); }
                                catch { }

                                try { xsdChild.Fixed = el4.GetAttribute("fixed"); }
                                catch { }




                                if (xsdChild.Name.ToLower() == "defining_code")
                                {
                                    XmlNodeList restricts = el4.SelectNodes(".//xs:restriction", nsmgr);
                                    if (restricts != null && restricts.Count > 0)
                                    {
                                        xsdChild.Restrictions = processRestrictions(restricts[0].InnerXml);
                                        if (xsdChild.Restrictions != null && xsdChild.Restrictions != "")
                                            xsd.Children.Add(xsdChild);
                                    }
                                    else
                                    {
                                        XmlNodeList seq = el4.SelectNodes("./xs:complexType/xs:sequence", nsmgr);
                                        if (seq != null && seq.Count > 0)
                                        {
                                            xsdChild.Restrictions = processRestrictions(seq[0].InnerXml);
                                            if (xsdChild.Restrictions != null && xsdChild.Restrictions != "")
                                                xsd.Children.Add(xsdChild);
                                        }
                                    }
                                }
                                else
                                {
                                    xsd.Children.Add(xsdChild);
                                    readChild(xsdChild, el4);
                                }



                            }
                        }


                    }


                    children = el3.SelectNodes("./xs:choice/xs:element", nsmgr);
                    foreach (XmlNode node3 in children)
                    {

                        XmlElement el4 = (XmlElement)node3;


                        xsdItem xsdChild = new xsdItem();
                        try { xsdChild.Name = el4.GetAttribute("name"); }
                        catch { }

                        if (xsdChild.Name != "")
                        {
                            if (!StopStr.Contains(xsdChild.Name.ToLower()))
                            {
                                try { xsdChild.Type = el4.GetAttribute("type"); }
                                catch { xsdChild.Type = ""; }

                                if (xsdChild.Type == "")
                                {
                                    XmlNodeList restricts = el4.SelectNodes("./xs:simpleType/xs:restriction", nsmgr);
                                    if (restricts != null && restricts.Count > 0)
                                    {
                                        XmlElement r = (XmlElement)restricts[0];
                                        try { xsdChild.Type = r.GetAttribute("base"); }
                                        catch { xsdChild.Type = ""; }

                                    }
                                }

                                try { xsdChild.oMin = el4.GetAttribute("minOccurs"); }
                                catch { }

                                try { xsdChild.oMax = el4.GetAttribute("maxOccurs"); }
                                catch { }

                                try { xsdChild.Fixed = el4.GetAttribute("fixed"); }
                                catch { }





                                if (xsdChild.Name.ToLower() == "defining_code")
                                {
                                    XmlNodeList restricts = el4.SelectNodes(".//xs:restriction", nsmgr);
                                    if (restricts != null && restricts.Count > 0)
                                    {
                                        xsdChild.Restrictions = processRestrictions(restricts[0].InnerXml);
                                        xsd.Choice.Add(xsdChild);
                                    }
                                }
                                else
                                {
                                    xsd.Choice.Add(xsdChild);
                                    readChild(xsdChild, el4);
                                }




                            }
                        }
                    }


                }




            }
        }
    }
}
