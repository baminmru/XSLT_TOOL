using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Xceed.Words.NET;
using System.Xml;
using System.IO;

namespace xNS
{
    public partial class frmSpecMaker : Form
    {
        public static string vbCrLf = "\r\n";
        public frmSpecMaker()
        {
            InitializeComponent();
        }

        private void cmdXML_Click(object sender, EventArgs e)
        {
            opf.Multiselect = false;
            opf.Filter = "XML files|*.xml|All files|*.*";
            if (opf.ShowDialog() == DialogResult.OK)
            {
                txtXML.Text = opf.FileName;
                
            }
        }

        private void cmdDocX_Click(object sender, EventArgs e)
        {
            opf2.Multiselect = false;
            if (opf2.ShowDialog() == DialogResult.OK)
            {
                txtDocx.Text = opf2.FileName;
            }
        }

       

        private List<XsltItem> items;
      

        private void LoadTree(bool noChanges)
        {
            tv.Nodes.Clear();
            foreach (XsltItem t in items)
            {

                if (noChanges == false)
                {
                    t.ComaBefore = true;
                    if (t.IsHeader()) { t.ComaBefore = false; t.DotAfter = true; }
                    if (t.IsTOC()) { t.ComaBefore = false; t.DotAfter = true; }
                }


                String Flags = "";
                if (t.ComaBefore) Flags += "Cm";
                if (t.IsTOC()) Flags += "T";
                if (t.IsBold()) Flags += "B";
                if (t.IsHeader()) Flags += "H";
                if (t.IsNewLine()) Flags += "Lf";
                if (t.Capitalize) Flags += "^";
                if (t.WithCaption() && t.IsHeader() == false && t.IsTOC() == false && t.IsBoolean() == false) Flags += "N";
                if (t.IsMulty()) Flags += "*";
                if (t.DotAfter) Flags += "Dt";

                TreeNode n = new TreeNode(t.ItemID);
                n.Text = t.ItemID + " " + Flags + " " + t.Caption;
                n.Tag = t;
                tv.Nodes.Add(n);
                AddChildren(n, t, noChanges);
            }
        }

        private void cmdRead_Click(object sender, EventArgs e)
        {
            if (txtDocx.Text == "") return;
           

            DocX doc;
            try
            {
                doc = DocX.Load(txtDocx.Text);
            }
            catch (System.Exception ex)
            {
                txtOut.Text = ex.Message;
                return;
            }

            

            int tIdx = 0;
            int rIdx;
            int cIdx;
 

        XsltItem[] Levels = new XsltItem[10];
        XsltItem x;

            foreach (var T in doc.Tables)
            {
                items = new List<XsltItem>();
                tIdx += 1;
                //txtOut.Text = txtOut.Text + vbCrLf + "Table begin " + tIdx.ToString();
                rIdx = 0;
                foreach (Row r in T.Rows)
                {
                    rIdx += 1;
                    // txtOut.Text = txtOut.Text & vbCrLf & "Row begin " & rIdx.ToString()
                    if (rIdx > 1)
                    {
                        cIdx = 0;
                        x = new XsltItem();

                        foreach (Cell c in r.Cells)
                        {
                            cIdx += 1;
                            if (cIdx == 1)
                                x.ItemID = c.Xml.Value.Trim();
                            if (cIdx == 3)
                                x.Caption = c.Xml.Value.Trim();
                            if (cIdx == 4)
                                x.FormInfo = c.Xml.Value.Trim();
                            if (cIdx == 5)
                                x.FactorInfo = c.Xml.Value.Trim();
                            if (cIdx == 7)
                                x.Path = c.Xml.Value.Trim();
                            if (cIdx > 7)
                            {
                                if (c.Xml.Value.Contains("show-decimals"))
                                {
                                    x.FactorInfo += "; show-decimals";
                                }
                            }
                        }
                        if (x.FormInfo != "-")
                        {
                            Levels[x.Level()] = x;
                            if (x.Level() > 1)
                            {
                                Levels[x.Level() - 1].Children.Add(x);
                                x.Parent = Levels[x.Level() - 1];
                            }
                            else
                            {
                                items.Add(x);
                            }
                        }
                    }
                }
                LoadTree(false);
                break;

            }
         }

        private void AddChildren(TreeNode n, XsltItem t, bool NoChanges)
        {
            foreach (XsltItem c in t.Children)
            {

                String Flags = "";
                if (NoChanges == false)
                {
                    c.ComaBefore = true;

                    if (c.IsHeader()) { c.ComaBefore = false; c.DotAfter = true; }
                    if (c.IsTOC()) { c.ComaBefore = false; c.DotAfter = true; }
                }

                if (c.ComaBefore) Flags += "Cm";
                if (c.IsTOC()) Flags += "T";
                if (c.IsHeader()) Flags += "H";
                if (c.IsBold()) Flags += "B";
                if (c.IsNewLine()) Flags += "Lf";
                if (c.Capitalize ) Flags += "^";
                if (c.WithCaption() && c.IsHeader() == false && c.IsTOC() == false && c.IsBoolean()==false ) Flags += "N";
                if (c.IsMulty()) Flags += "*";
                if (c.DotAfter) Flags += "Dt";
                TreeNode n2 = new TreeNode(c.ItemID);
                n2.Text = c.ItemID + " " + Flags + " " + c.Caption;
                n2.Tag = c;
                n.Nodes.Add(n2);
                AddChildren(n2, c, NoChanges);
            }
        }

        private  void RenameNode(TreeNode n, XsltItem t)
        {
            String Flags = "";
            if (t.ComaBefore) Flags += "Cm";
            if (t.IsTOC()) Flags += "T";
            if (t.IsHeader()) Flags += "H";
            if (t.IsBold()) Flags += "B";
            if (t.IsNewLine()) Flags += "Lf";
            if (t.Capitalize) Flags += "^";
            if (t.WithCaption() && t.IsHeader() == false && t.IsTOC() == false && t.IsBoolean() == false) Flags += "N";
            if (t.IsMulty()) Flags += "*";
            if (t.DotAfter) Flags += "Dt";
            n.Text = t.ItemID + " " + Flags + " " + t.Caption;
        }

        private void cmdCm_Click(object sender, EventArgs e)
        {
            if (tv.SelectedNode == null) return;

            XsltItem t;
            t = (XsltItem) tv.SelectedNode.Tag;
            t.ComaBefore = ! t.ComaBefore;
            RenameNode(tv.SelectedNode, t);
        }

        private void cmdDt_Click(object sender, EventArgs e)
        {
            if (tv.SelectedNode == null) return;

            XsltItem t;
            t = (XsltItem)tv.SelectedNode.Tag;
            t.DotAfter = !t.DotAfter;
            RenameNode(tv.SelectedNode, t);
        }

        private void cmdCap_Click(object sender, EventArgs e)
        {
            if (tv.SelectedNode == null) return;

            XsltItem t;
            t = (XsltItem)tv.SelectedNode.Tag;
            t.Capitalize = !t.Capitalize ;
            RenameNode(tv.SelectedNode, t);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (svf.ShowDialog() == DialogResult.OK)
            {
                textSaveMap.Text = svf.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            opf.Multiselect = false;
            opf.Filter = "XML files|*.xml|All files|*.*";

            if (opf.ShowDialog() == DialogResult.OK)
            {
                textLoadMap.Text = opf.FileName;
            }
        }

        private  void Show_Message(XsltItem sX)
        {
            Text = sX.ItemID + " "+ sX.Caption ;
            Application.DoEvents(); 
        }

        private XmlNamespaceManager nsmgr;

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
                                catch { xsdChild.oMin = "0"; }
                                if (xsdChild.oMin == "") xsdChild.oMin = "0";

                                try { xsdChild.oMax = el4.GetAttribute("maxOccurs"); }
                                catch { xsdChild.oMax = "1"; }
                                if (xsdChild.oMax == "") xsdChild.oMax = "1";

                                try { xsdChild.Fixed = el4.GetAttribute("fixed"); }
                                catch { xsdChild.Fixed = ""; }




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
                                catch { xsdChild.oMin = "0"; }
                                if (xsdChild.oMin == "") xsdChild.oMin = "0";

                                try { xsdChild.oMax = el4.GetAttribute("maxOccurs"); }
                                catch { xsdChild.oMax = "1"; }
                                if (xsdChild.oMax == "") xsdChild.oMax = "1";

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
    

    private string BuildXML()
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
            sXSD=sXSD.Replace("Провоцирующий_fslash_купирующий_фактор", "Провоцирующий_фактор");
            sXSD = sXSD.Replace("&lt;", "_меньше_");
            sXSD = sXSD.Replace("&gt;", "_больше_");

            xDoc.LoadXml(sXSD);

            nsmgr = new XmlNamespaceManager(xDoc.NameTable);
            nsmgr.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");
            //nsmgr.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");

           
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

            using (var writer = new System.IO.StreamWriter(txtXSD.Text+".map"))
            {
                var serializer = new XmlSerializer(root.GetType());
                serializer.Serialize(writer, root);
                writer.Flush();
            }

            string sOut;
            string testName;
            //if (TestCount > 0)
            //{
            FileInfo fi = new FileInfo(txtXSD.Text);
            testName = fi.DirectoryName;
            for (int i = 1; i <= 10; i++)
            {
                for (int j = 1; j <= 5; j++)
                {
                    root.SetGenPercent( (short)(i * 2));
                    sOut = root.Generate(null).ToString();
                    File.WriteAllText(testName + "\\test_" + i.ToString() + "_" + j.ToString() + ".xml", sOut);
                }
            }
            //}

            root.SetGenPercent(0);
            sOut = root.Generate(null).ToString();
            return sOut;
        }

        private void cmdProcess_Click(object sender, EventArgs e)
        {
            string xmlpath= txtXML.Text;

            SpecPro sp;
            if (chkPDF.Checked)
                sp = new xNS.SpecPro();
            else
                sp = new xNS.ScreenForm();

            SpecPro.DebugPrint = chkDebug.Checked;

            if (txtXML.Text == "")
            {
                if (txtXSD.Text == "")
                    return;
                else
                {
                   string XMLText = BuildXML();
                    File.WriteAllText(txtXSD.Text + ".xml", XMLText);
                    xmlpath = "AUTO";
                    sp.LoadXML(XMLText, "*");
                }

            }
            if (items == null) return;
            
            

            
            sp.Init();

            sp.onNextNode += Show_Message;

            txtOut.Text = "";
            txtErrors.Text = "";
            StringBuilder sb = new StringBuilder();
            foreach (var x1 in items)
            {
                sb.Append(vbCrLf);
                sb.Append(sp.Process(xmlpath, x1, false));
            }

            txtOut.Text = sb.ToString();
            txtErrors.Text = sp.Error();
            //txtOut.Text = txtOut.Text + vbCrLf + "</div>";
            Text = "Обработка спецификации завершена";
            MessageBox.Show("Обработка спецификации завершена");
        }

            private void button3_Click(object sender, EventArgs e)
        {
            if(textSaveMap.Text != "")
            {
                using (var writer = new System.IO.StreamWriter(textSaveMap.Text))
                {
                    var serializer = new XmlSerializer(items.GetType());
                    serializer.Serialize(writer, items);
                    writer.Flush();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textLoadMap.Text != "")
            {

                using (var stream = System.IO.File.OpenRead(textLoadMap.Text))
                {
                    var serializer = new XmlSerializer(typeof(List<XsltItem>));
                    items= serializer.Deserialize(stream) as List<XsltItem>;
                }

                
                foreach (XsltItem c in items)
                {
                    c.RestoreParent();
                }

                LoadTree(true);

            }
        }

        private void cmdLf_Click(object sender, EventArgs e)
        {
            if (tv.SelectedNode == null) return;

            XsltItem t;
            t = (XsltItem)tv.SelectedNode.Tag;
            t.LineFeedManual = true;
            t.LineFeed = !t.LineFeed;
            LoadTree(true);
        }

        private void cmdAutoDot_Click(object sender, EventArgs e)
        {
            if (items == null) return;
             if(chkReInit.Checked)  LoadTree(false);

                foreach (XsltItem x in items)
            {
                //ClearLastDot(x);
                DotBeforeHeader(x);
                ShiftDotToChildren(x);
                ClearFirstComa(x);
                CapAfterDot(x);
            }




            LoadTree(true);

        }


        private void ShiftDotToChildren(XsltItem x)
        {
            XsltItem lc;

            if (x.Children.Count >= 1 )
            {

                

                x.DotAfter = false;  // будем брать точку у последнего сына-дочки
                                     //// последний потомок всегда с точкой
                lc = x.Children[x.Children.Count - 1];
                lc.DotAfter = true;

                foreach (XsltItem nc in x.Children)
                {
                    ShiftDotToChildren(nc);
                }
            }
        }


        private void ClearLastDot(XsltItem x)
        {
            XsltItem lc;
            
            if(x.Children.Count >=1 && x.DotAfter==true)
            {
                // чтобы не было  двух точек,  убираем её у последнего дочернего  узла
                lc = x.Children[x.Children.Count - 1];
                lc.DotAfter = false;
                foreach(XsltItem nc in x.Children)
                {
                    ClearLastDot(nc);
                }
            }
        }

        private void ClearFirstComa(XsltItem x)
        {
            XsltItem lc;

            // у первого потомка после заголовка убрать  запятую
            if (x.Children.Count > 0 && x.IsHeader()  == true)
            {
                lc = x.Children[0];
                lc.ComaBefore = false;
            }

            // у первого потомка после оглавления убрать  запятую
            if (x.Children.Count > 0 && x.IsTOC() == true)
            {
                lc = x.Children[0];
                lc.ComaBefore = false;
            }

            if (x.Children.Count > 0 && x.DotAfter == true  )
            {
                lc = x.Children[0];
                if (lc.ComaBefore   ) lc.ComaBefore = false;
               
            }
            foreach (XsltItem nc in x.Children)
            {
                ClearFirstComa(nc);
            }
        }
        private void DotBeforeHeader(XsltItem x)
        {
            XsltItem lc;
            XsltItem cc; 

            if (x.Children.Count > 0 )
            {
                Int16 i;
                for (i = 0; i < x.Children.Count-1; i++)
                {
                    lc = x.Children[i+1];
                    cc = x.Children[i ];

                    // стаавим точку перед соседом - заголовком
                    if (lc.IsHeader()  && cc.IsHeader()==false) 
                    {
                        lc.Capitalize = true;
                        cc.DotAfter = true;
                    }
                }
                
                
                foreach (XsltItem nc in x.Children)
                {
                    DotBeforeHeader(nc);
                }
            }
        }

        private void CapAfterDot(XsltItem x)
        {

            int i;
            XsltItem cur;
            XsltItem prev;
            for (i=1;i< x.Children.Count;i++)
            {
                cur = x.Children[i];
                prev = x.Children[i-1];

                if( (prev.Children.Count > 0 ||  prev.DotAfter)  )
                {
                    cur.ComaBefore = false;
                    cur.Capitalize = true;
                }
            }


            // shift  capitalization  down  from  generic  header
            if (x.Children.Count > 0)
            {
                if(x.Capitalize  && (x.Path=="" || x.Path.ToLower().Contains("generic")   ))
                {
                    x.Children[0].Capitalize = true;
                }
            }

            foreach (XsltItem nc in x.Children)
            {

                CapAfterDot(nc);
            }
        }

        private void cmdShiftUp_Click(object sender, EventArgs e)
        {
            if (items == null) return;

            XsltItem t;
            XsltItem p;
            t = (XsltItem)tv.SelectedNode.Tag;
            p = t.PrevSibling();
            if (p != null)
            {
                t.Parent.Children.Remove(t);
                p.Children.Add(t);
                t.Parent = p;
                UpLevel(t);


            }
            else
            {
                if (t.Level() == 1)
                {
                    
                    for(int i=0;i < items.Count; i++)
                    {
                        if (t.ItemID == items[i].ItemID)
                        {
                            if(i > 0)
                            {
                                p = items[i - 1];
                                p.Children.Add(t);
                                t.Parent = null;
                                items.Remove(t); 
                                UpLevel(t);
                            }
                        }
                    }

                }
            }
            LoadTree(true);
        }

        private void UpLevel(XsltItem x)
        {

            x.ItemID = "Up." + x.ItemID;

            foreach (XsltItem nc in x.Children)
            {
                UpLevel(nc);
            }
        }

        private void cmdDel_Click(object sender, EventArgs e)
        {
            if (items == null) return;
            if(MessageBox.Show("Удалить текущий узел и всех его потомков  ?","Внимание",MessageBoxButtons.YesNo )== DialogResult.Yes)
            {
                XsltItem t;
                XsltItem p;
                t = (XsltItem)tv.SelectedNode.Tag;
                p = t.Parent;
                if (p != null)
                {
                    p.Children.Remove(t);
                }
                else
                {
                    items.Remove(t);
                }
                LoadTree(true);
            }

            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (items == null) return;

            XsltItem t;
            XsltItem p;
            XsltItem p2;
            t = (XsltItem)tv.SelectedNode.Tag;
            p = t.Parent;
            if (p != null)
            {
                p2 = p.Parent;
                if (p2 != null)
                {
                    t.Parent.Children.Remove(t);
                    t.Parent = p2;
                    p2.Children.Add(p);
                }
                else
                {
                    t.Parent.Children.Remove(t);
                    t.Parent = null;
                    for (int i = 0; i < items.Count; i++)
                    {
                        if (p.ItemID == items[i].ItemID)
                        {
                            
                                LeftLevel(t);
                            
                                items.Insert(i,t);
                            break;
                            
                        }
                    }
                    
                }
                LoadTree(true);
            }
         
            
        }

        private void LeftLevel(XsltItem x)
        {
            int idx = x.ItemID.IndexOf('.');
            String newID;
            if (idx >= 0)
            {
                newID = x.ItemID.Substring(0, idx ) + "_" + x.ItemID.Substring(idx + 1);
            }
            else
            {
                newID = x.ItemID;
            }

            x.ItemID = newID;

            foreach (XsltItem nc in x.Children)
            {
                LeftLevel(nc);
            }
        }

        private void tv_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void tv_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {

            if (tv.SelectedNode == null) return;

            XsltItem t;
            t = (XsltItem)tv.SelectedNode.Tag;

            frmItem fi = new frmItem();
            fi.Text = t.Caption;
            fi.txtItem.Text = t.ToString(false);
            fi.ShowDialog();
        }

        private  void DropInput( XsltItem x)
        {

            List<XsltItem> ToDrop = new List<XsltItem>();
            foreach (XsltItem c in x.Children)
            {
                if((c.Caption.ToLower()=="input" || c.Caption.ToLower().Contains("кнопка добавления") || c.Caption.ToLower() == "input_text")  && c.Children.Count==0)
                {
                    ToDrop.Add(c);
                    
                }
            }

            foreach (XsltItem c in ToDrop)
            {
                x.Children.Remove(c);
            }

            foreach (XsltItem c in x.Children)
            {
                DropInput(c);
            }
        }

        private void cmdDelInput_Click(object sender, EventArgs e)
        {
            if (items == null) return;

            foreach (XsltItem c in items)
            {
                DropInput(c);
            }
            LoadTree(true);
            
        }

        private void cmdSelectFile_Click(object sender, EventArgs e)
        {
            opf.Filter = "XSD files|*.xsd|All files|*.*";
            if (opf.ShowDialog() == DialogResult.OK)
            {
                txtXSD.Text = opf.FileName;
            }
        }

        private List<String> StopStr;
    }



}
