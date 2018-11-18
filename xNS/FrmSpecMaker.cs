using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using Xceed.Words.NET;

namespace xNS
{
    public partial class FrmSpecMaker : Form
    {
        private static readonly string vbCrLf = "\r\n";

        private List<XsltItem> _items;

        public FrmSpecMaker()
        {
            InitializeComponent();
        }

        private void cmdXML_Click(object sender, EventArgs e)
        {
            opf.Multiselect = false;
            if (opf.ShowDialog() == DialogResult.OK) txtXML.Text = opf.FileName;
        }

        private void cmdDocX_Click(object sender, EventArgs e)
        {
            opf2.Multiselect = false;
            if (opf2.ShowDialog() == DialogResult.OK) txtDocx.Text = opf2.FileName;
        }

        private void LoadTree(bool noChanges)
        {
            tv.Nodes.Clear();
            foreach (var t in _items)
            {
                if (noChanges == false)
                {
                    t.ComaBefore = true;
                    if (t.IsHeader())
                    {
                        t.ComaBefore = false;
                        t.DotAfter = true;
                    }

                    if (t.IsToc())
                    {
                        t.ComaBefore = false;
                        t.DotAfter = true;
                    }
                }


                var flags = "";
                if (t.ComaBefore) flags += "Cm";
                if (t.IsToc()) flags += "T";
                if (t.IsBold()) flags += "B";
                if (t.IsHeader()) flags += "H";
                if (t.IsNewLine()) flags += "Lf";
                if (t.Capitalize) flags += "^";
                if (t.WithHeader() && t.IsHeader() == false && t.IsToc() == false && t.IsBoolean() == false)
                    flags += "N";
                if (t.IsMulty()) flags += "*";
                if (t.DotAfter) flags += "Dt";

                var n = new TreeNode(t.ItemId);
                n.Text = t.ItemId + '_' + flags + '_' + t.Caption;
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
            catch (Exception ex)
            {
                txtOut.Text = ex.Message;
                return;
            }


            int rIdx;
            int cIdx;

            var levels = new XsltItem[10];
            XsltItem x;

            foreach (var T in doc.Tables)
            {
                _items = new List<XsltItem>();
                rIdx = 0;
                foreach (var r in T.Rows)
                {
                    rIdx += 1;
                    if (rIdx > 1)
                    {
                        cIdx = 0;
                        x = new XsltItem();

                        foreach (var c in r.Cells)
                        {
                            cIdx += 1;
                            if (cIdx == 1)
                                x.ItemId = c.Xml.Value.Trim();
                            if (cIdx == 3)
                                x.Caption = c.Xml.Value.Trim();
                            if (cIdx == 4)
                                x.FormInfo = c.Xml.Value.Trim();
                            if (cIdx == 5)
                                x.FactorInfo = c.Xml.Value.Trim();
                            if (cIdx == 7)
                                x.Path = c.Xml.Value.Trim();
                        }

                        if (x.FormInfo != "-")
                        {
                            levels[x.Level()] = x;
                            if (x.Level() > 1)
                            {
                                levels[x.Level() - 1].Children.Add(x);
                                x.Parent = levels[x.Level() - 1];
                            }
                            else
                            {
                                _items.Add(x);
                            }
                        }
                    }
                }

                LoadTree(false);
                break;
            }
        }

        private void AddChildren(TreeNode n, XsltItem t, bool noChanges)
        {
            foreach (var c in t.Children)
            {
                var flags = "";
                if (noChanges == false)
                {
                    c.ComaBefore = true;

                    if (c.IsHeader())
                    {
                        c.ComaBefore = false;
                        c.DotAfter = true;
                    }

                    if (c.IsToc())
                    {
                        c.ComaBefore = false;
                        c.DotAfter = true;
                    }
                }

                if (c.ComaBefore) flags += "Cm";
                if (c.IsToc()) flags += "T";
                if (c.IsHeader()) flags += "H";
                if (c.IsBold()) flags += "B";
                if (c.IsNewLine()) flags += "Lf";
                if (c.Capitalize) flags += "^";
                if (c.WithHeader() && c.IsHeader() == false && c.IsToc() == false && c.IsBoolean() == false)
                    flags += "N";
                if (c.IsMulty()) flags += "*";
                if (c.DotAfter) flags += "Dt";
                var n2 = new TreeNode(c.ItemId);
                n2.Text = c.ItemId + '_' + flags + '_' + c.Caption;
                n2.Tag = c;
                n.Nodes.Add(n2);
                AddChildren(n2, c, noChanges);
            }
        }

        private void RenameNode(TreeNode n, XsltItem t)
        {
            var flags = "";
            if (t.ComaBefore) flags += "Cm";
            if (t.IsToc()) flags += "T";
            if (t.IsHeader()) flags += "H";
            if (t.IsBold()) flags += "B";
            if (t.IsNewLine()) flags += "Lf";
            if (t.Capitalize) flags += "^";
            if (t.WithHeader() && t.IsHeader() == false && t.IsToc() == false && t.IsBoolean() == false) flags += "N";
            if (t.IsMulty()) flags += "*";
            if (t.DotAfter) flags += "Dt";
            n.Text = t.ItemId + '_' + flags + '_' + t.Caption;
        }

        private void cmdCm_Click(object sender, EventArgs e)
        {
            if (tv.SelectedNode == null) return;

            XsltItem t;
            t = (XsltItem) tv.SelectedNode.Tag;
            t.ComaBefore = !t.ComaBefore;
            RenameNode(tv.SelectedNode, t);
        }

        private void cmdDt_Click(object sender, EventArgs e)
        {
            if (tv.SelectedNode == null) return;

            XsltItem t;
            t = (XsltItem) tv.SelectedNode.Tag;
            t.DotAfter = !t.DotAfter;
            RenameNode(tv.SelectedNode, t);
        }

        private void cmdCap_Click(object sender, EventArgs e)
        {
            if (tv.SelectedNode == null) return;

            XsltItem t;
            t = (XsltItem) tv.SelectedNode.Tag;
            t.Capitalize = !t.Capitalize;
            RenameNode(tv.SelectedNode, t);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (svf.ShowDialog() == DialogResult.OK) textSaveMap.Text = svf.FileName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            opf.Multiselect = false;
            if (opf.ShowDialog() == DialogResult.OK) textLoadMap.Text = opf.FileName;
        }

        private void cmdProcess_Click(object sender, EventArgs e)
        {
            if (txtXML.Text == "") return;
            if (_items == null) return;
            SpecPro sp;
            if (chkPDF.Checked)
                sp = new SpecPro();
            else
                sp = new ScreenForm();

            SpecPro.DebugPrint = chkDebug.Checked;
            sp.Init();
            txtOut.Text = "";
            txtErrors.Text = "";

            foreach (var x1 in _items)
            {
                Application.DoEvents();
                txtOut.Text = txtOut.Text + vbCrLf + sp.Process(txtXML.Text, x1, false);
                txtErrors.Text = sp.Error();
                Application.DoEvents();
            }

            MessageBox.Show(@"Обработка спецификации завершена");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textSaveMap.Text != "")
                using (var writer = new StreamWriter(textSaveMap.Text))
                {
                    var serializer = new XmlSerializer(_items.GetType());
                    serializer.Serialize(writer, _items);
                    writer.Flush();
                }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textLoadMap.Text != "")
            {
                using (var stream = File.OpenRead(textLoadMap.Text))
                {
                    var serializer = new XmlSerializer(typeof(List<XsltItem>));
                    _items = serializer.Deserialize(stream) as List<XsltItem>;
                }


                foreach (var c in _items) c.RestoreParent();

                LoadTree(true);
            }
        }

        private void cmdLf_Click(object sender, EventArgs e)
        {
            if (tv.SelectedNode == null) return;

            XsltItem t;
            t = (XsltItem) tv.SelectedNode.Tag;
            t.LineFeedManual = true;
            t.LineFeed = !t.LineFeed;
            LoadTree(true);
        }

        private void cmdAutoDot_Click(object sender, EventArgs e)
        {
            if (_items == null) return;
            if (chkReInit.Checked) LoadTree(false);

            foreach (var x in _items)
            {
                DotBeforeHeader(x);
                if (chkShiftDot.Checked)
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

            if (x.Children.Count >= 1)
            {
                var hdrChild = false;
                foreach (var nc in x.Children)
                    if (nc.IsHeader())
                    {
                        hdrChild = true;
                        break;
                    }

                // если есть дочерние с заголовком
                if (hdrChild)
                {
                    x.DotAfter = false; // будем брать точку у последнего сына-дочки
                    //// последний потомок всегда с точкой
                    lc = x.Children[x.Children.Count - 1];
                    lc.DotAfter = true;
                }

                foreach (var nc in x.Children) ShiftDotToChildren(nc);
            }
        }

        private void ClearLastDot(XsltItem x)
        {
            XsltItem lc;

            if (x.Children.Count >= 1 && x.DotAfter)
            {
                // чтобы не было  двух точек,  убираем её у последнего дочернего  узла
                lc = x.Children[x.Children.Count - 1];
                lc.DotAfter = false;
                foreach (var nc in x.Children) ClearLastDot(nc);
            }
        }

        private void ClearFirstComa(XsltItem x)
        {
            XsltItem lc;

            // у первого потомка после заголовка убрать  запятую
            if (x.Children.Count > 0 && x.IsHeader())
            {
                lc = x.Children[0];
                lc.ComaBefore = false;
            }

            // у первого потомка после оглавления убрать  запятую
            if (x.Children.Count > 0 && x.IsToc())
            {
                lc = x.Children[0];
                lc.ComaBefore = false;
            }

            if (x.Children.Count > 0 && x.DotAfter)
            {
                lc = x.Children[0];
                if (lc.ComaBefore) lc.ComaBefore = false;
            }

            foreach (var nc in x.Children) ClearFirstComa(nc);
        }

        private void DotBeforeHeader(XsltItem x)
        {
            XsltItem lc;
            XsltItem cc;

            if (x.Children.Count > 0)
            {
                short i;
                for (i = 0; i < x.Children.Count - 1; i++)
                {
                    lc = x.Children[i + 1];
                    cc = x.Children[i];

                    // стаавим точку перед соседом - заголовком
                    if (lc.IsHeader() && cc.IsHeader() == false)
                    {
                        lc.Capitalize = true;
                        cc.DotAfter = true;
                    }
                }


                foreach (var nc in x.Children) DotBeforeHeader(nc);
            }
        }

        private void CapAfterDot(XsltItem x)
        {
            int i;
            XsltItem cur;
            XsltItem prev;
            for (i = 1; i < x.Children.Count; i++)
            {
                cur = x.Children[i];
                prev = x.Children[i - 1];

                if (prev.DotAfter && !cur.ComaBefore)
                {
                    cur.ComaBefore = false;
                    cur.Capitalize = true;
                }
            }

            foreach (var nc in x.Children) CapAfterDot(nc);
        }

        private void cmdShiftUp_Click(object sender, EventArgs e)
        {
            if (_items == null) return;

            XsltItem t;
            XsltItem p;
            t = (XsltItem) tv.SelectedNode.Tag;
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
                    for (var i = 0; i < _items.Count; i++)
                        if (t.ItemId == _items[i].ItemId)
                            if (i > 0)
                            {
                                p = _items[i - 1];
                                p.Children.Add(t);
                                t.Parent = null;
                                _items.Remove(t);
                                UpLevel(t);
                            }
            }

            LoadTree(true);
        }

        private void UpLevel(XsltItem x)
        {
            x.ItemId = "Up." + x.ItemId;

            foreach (var nc in x.Children) UpLevel(nc);
        }

        private void cmdDel_Click(object sender, EventArgs e)
        {
            if (_items == null) return;
            if (MessageBox.Show(@"Удалить текущий узел и всех его потомков  ?", @"Внимание", MessageBoxButtons.YesNo) ==
                DialogResult.Yes)
            {
                XsltItem t;
                XsltItem p;
                t = (XsltItem) tv.SelectedNode.Tag;
                p = t.Parent;
                if (p != null)
                    p.Children.Remove(t);
                else
                    _items.Remove(t);
                LoadTree(true);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (_items == null) return;

            XsltItem t;
            XsltItem p;
            XsltItem p2;
            t = (XsltItem) tv.SelectedNode.Tag;
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
                    for (var i = 0; i < _items.Count; i++)
                        if (p.ItemId == _items[i].ItemId)
                        {
                            LeftLevel(t);

                            _items.Insert(i, t);
                            break;
                        }
                }

                LoadTree(true);
            }
        }

        private void LeftLevel(XsltItem x)
        {
            var idx = x.ItemId.IndexOf('.');
            string newId;
            if (idx >= 0)
                newId = x.ItemId.Substring(0, idx) + "_" + x.ItemId.Substring(idx + 1);
            else
                newId = x.ItemId;

            x.ItemId = newId;

            foreach (var nc in x.Children) LeftLevel(nc);
        }

        private void tv_AfterSelect(object sender, TreeViewEventArgs e)
        {
        }

        private void tv_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (tv.SelectedNode == null) return;

            XsltItem t;
            t = (XsltItem) tv.SelectedNode.Tag;

            var fi = new FrmItem();
            fi.Text = t.Caption;
            fi.txtItem.Text = t.ToString(false);
            fi.ShowDialog();
        }
    }
}