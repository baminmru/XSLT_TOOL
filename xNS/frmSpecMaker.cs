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

        //private void btnStart_Click(object sender, EventArgs e)
        //{
        //    if (txtDocx.Text == "") return;
        //    DocX doc;
        //    try
        //    {
        //        doc = DocX.Load(txtDocx.Text);
        //    }catch(System.Exception ex)
        //    {
        //        txtOut.Text = ex.Message;
        //        return;
        //    }
                
                
               
        //        int tIdx = 0;
        //        int rIdx;
        //        int cIdx;
              
        //        int i;

        //    XsltItem[] Levels = new XsltItem[10];
        //    XsltItem x;

        //        foreach (var T in doc.Tables)
        //        {
        //            items = new List<XsltItem>();
        //            tIdx += 1;
        //            //txtOut.Text = txtOut.Text + vbCrLf + "Table begin " + tIdx.ToString();
        //            rIdx = 0;
        //            foreach (Row r in T.Rows)
        //            {
        //                rIdx += 1;
        //                // txtOut.Text = txtOut.Text & vbCrLf & "Row begin " & rIdx.ToString()
        //                if (rIdx > 1)
        //                {
        //                    cIdx = 0;
        //                    x = new XsltItem();

        //                    foreach (Cell c in r.Cells)
        //                    {
        //                        cIdx += 1;
        //                        if (cIdx == 1)
        //                            x.ItemID = c.Xml.Value.Trim();
        //                        if (cIdx == 3)
        //                            x.Caption = c.Xml.Value.Trim();
        //                        if (cIdx == 4)
        //                            x.FormInfo = c.Xml.Value.Trim();
        //                        if (cIdx == 5)
        //                            x.FactorInfo = c.Xml.Value.Trim();
        //                        if (cIdx == 7)
        //                            x.Path = c.Xml.Value.Trim();
        //                    }
        //                    if (x.FormInfo != "-")
        //                    {
        //                        Levels[x.Level()] = x;
        //                        if (x.Level() > 1)
        //                        {
        //                            Levels[x.Level() - 1].Children.Add(x);
        //                            x.Parent = Levels[x.Level() - 1];
        //                        }
        //                        else
        //                        {
        //                            items.Add(x);
        //                        }
        //                    }
        //                }
        //            }

        //            LoadTree(false);
        //            SpecPro sp;
        //            if (chkPDF.Checked)
        //                sp = new xNS.SpecPro();
        //            else
        //                sp = new xNS.ScreenForm();
        //            SpecPro.DebugPrint = chkDebug.Checked;
        //            txtOut.Text = "";

        //            foreach (var x1 in items)
        //            {
        //                Application.DoEvents();
        //                txtOut.Text = txtOut.Text + vbCrLf + sp.Process(txtXML.Text,  x1,false);
                      
        //                Application.DoEvents();
        //            }
        //            //txtOut.Text = txtOut.Text + vbCrLf + "</div>";
        //            MessageBox.Show("Обработка спецификации завершена");
        //            break;
        //        }
        //    }

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
                if (t.WithHeader() && t.IsHeader() == false && t.IsTOC() == false && t.IsBoolean() == false) Flags += "N";
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
           

            
            int i;


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
                if (c.WithHeader() && c.IsHeader() == false && c.IsTOC() == false && c.IsBoolean()==false ) Flags += "N";
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
            if (t.WithHeader() && t.IsHeader() == false && t.IsTOC() == false && t.IsBoolean() == false) Flags += "N";
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
            if (opf.ShowDialog() == DialogResult.OK)
            {
                textLoadMap.Text = opf.FileName;
            }
        }

        private void cmdProcess_Click(object sender, EventArgs e)
        {
            if (txtXML.Text == "") return;
            if (items == null) return;
            SpecPro sp;
            if (chkPDF.Checked)
                sp = new xNS.SpecPro();
            else
                sp = new xNS.ScreenForm();

            SpecPro.DebugPrint = chkDebug.Checked;
            sp.Init();
            txtOut.Text = "";
            txtErrors.Text = "";

            foreach (var x1 in items)
            {
                Application.DoEvents();
                txtOut.Text = txtOut.Text + vbCrLf + sp.Process(txtXML.Text, x1, false);
                txtErrors.Text = sp.Error(); 
                Application.DoEvents();
            }
            //txtOut.Text = txtOut.Text + vbCrLf + "</div>";
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
                if(chkShiftDot.Checked)
                    ShiftDotToChildren(x);
                else
                    ClearLastDot(x);
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
                
                Boolean hdrChild = false;
                foreach (XsltItem nc in x.Children)
                {
                    if (nc.IsHeader())
                    {
                        hdrChild = true;
                        break;
                    }
                }

                // если есть дочерние с заголовком
                if (hdrChild)
                {
                    x.DotAfter = false;  // будем брать точку у последнего сына-дочки
                                         //// последний потомок всегда с точкой
                    lc = x.Children[x.Children.Count - 1];
                    lc.DotAfter = true;

                    // все дочерние переделать на точку
                    //foreach (XsltItem nc in x.Children)
                    //{
                    //    nc.ComaBefore = false;
                    //    nc.DotAfter = true;
                    //}
                }

                

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

                if(prev.DotAfter  && !cur.ComaBefore)
                {
                    cur.ComaBefore = false;
                    cur.Capitalize = true;
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
                if(c.Caption.ToLower()=="input" && c.Children.Count==0)
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
    }



}

