using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace xNS
{
    public class XsltItem
    {
        [XmlIgnore] private const string VbCrLf = "\r\n";

        [XmlIgnore] private XsltItem _nextSibling;


        [XmlIgnore] private XsltItem _prevSibling;

        public bool Capitalize;
        public string Caption;
        public List<XsltItem> Children = new List<XsltItem>();
        public bool ComaBefore;
        public bool DotAfter;
        public string FactorInfo;
        public string FormInfo;

        public string ItemId;
        public bool LineFeed;
        public bool LineFeedManual;

        [XmlIgnore] private bool _nextSiblingOk;

        [XmlIgnore] public XsltItem Parent;

        public string Path;

        [XmlIgnore] private bool _prevSiblingOk;

        public string XslFor;

        public override string ToString()
        {
            string s;
            int i;
            s = "";
            for (i = 1; i <= Level() - 1; i++)
                s = s + "***|";
            s = s + "#" + ItemId + " (" + Level() + ") " + VbCrLf + "Cap: " + Caption + VbCrLf + "Form: " + FormInfo +
                VbCrLf + "Factor: " + FactorInfo + VbCrLf + "Path: " + Path + VbCrLf;
            foreach (var x in Children)
                s = s + VbCrLf + x;

            return s;
        }

        public string ToString(bool withChildren)
        {
            string s;
            int i;
            s = "";
            if (withChildren)
                for (i = 1; i <= Level() - 1; i++)
                    s = s + "***|";
            s = s + "#" + ItemId + VbCrLf + "Cap: " + Caption + VbCrLf + "Form: " + FormInfo + VbCrLf + "Factor: " +
                FactorInfo + VbCrLf + "Path: " + Path + VbCrLf;
            if (withChildren)
                foreach (var x in Children)
                    s = s + VbCrLf + x.ToString(withChildren);
            return s;
        }

        public int Level()
        {
            if (ItemId.Trim() == "")
                return 0;
            string[] s;
            s = ItemId.Trim().Split('.');
            return s.Count();
        }


        // form helper
        public bool WithHeader()
        {
            if (FormInfo.Contains("без заголовка")) return false;
            if (FormInfo.Contains("+")) return true;
            if (IsToc()) return true;
            return false;
        }

        public bool IsToc()
        {
            if (FormInfo.ToLower().Contains("пункт «оглавления»")) return true;
            return false;
        }

        public bool IsNewLine()
        {
            if (LineFeedManual) return LineFeed;
            LineFeed = FormInfo.ToLower().Contains("с новой строки");
            return LineFeed;
        }

        // factor helper
        public bool IsMulty()
        {
            return Regex.IsMatch(FactorInfo, "(\\.[0-9*])");
        }


        public bool IsHeader()
        {
            if (FactorInfo.ToLower().Contains("заголовок раздела")) return true;
            return false;
        }

        public bool IsBold()
        {
            if (WithHeader())
                if (Level() == 2)
                    return true;
            return false;
        }

        public bool IsDate()
        {
            if (FactorInfo.Contains("DV_DATE_TIME")) return true;
            if (FactorInfo.Contains("DV_DATE")) return true;
            return false;
        }

        public bool IsBoolean()
        {
            if (FactorInfo.Contains("DV_BOOLEAN")) return true;
            return false;
        }

        public bool IsQuantity()
        {
            if (FactorInfo.Contains("DV_QUANTITY")) return true;
            return false;
        }

        public bool IsPeriod()
        {
            if (FactorInfo.Contains("DV_DURATION")) return true;
            return false;
        }

        public bool IsSinglePeriod()
        {
            if (FactorInfo.Contains("DV_QUANTITY"))
                if (FactorInfo.Contains("wk") && FactorInfo.Contains("mo"))
                    return true;
            return false;
        }


        public bool IsSize()
        {
            if (FactorInfo.Contains("DV_QUANTITY"))
                if (FactorInfo.Contains("mm") && FactorInfo.Contains("cm"))
                    return true;
            return false;
        }


        public XsltItem NextSibling()
        {
            if (_nextSiblingOk) return _nextSibling;
            _nextSibling = FindNextSibling();
            _nextSiblingOk = true;
            return _nextSibling;
        }

        public XsltItem PrevSibling()
        {
            if (_prevSiblingOk) return _prevSibling;
            _prevSibling = FindPrevSibling();
            _prevSiblingOk = true;
            return _prevSibling;
        }

        private XsltItem FindNextSibling()
        {
            if (Parent != null)
            {
                // find self in parent Children collection
                var returnNext = false;
                foreach (var sibl in Parent.Children)
                {
                    if (returnNext) return sibl;
                    if (ItemId == sibl.ItemId) returnNext = true;
                }
            }

            return null;
        }

        private XsltItem FindPrevSibling()
        {
            if (Parent != null)
            {
                // find self in parent Children collection
                var returnNext = false;
                int i;
                XsltItem sibl;
                for (i = Parent.Children.Count - 1; i >= 0; i--)
                {
                    sibl = Parent.Children[i];
                    if (returnNext) return sibl;
                    if (ItemId == sibl.ItemId) returnNext = true;
                }
            }

            return null;
        }

        public void RestoreParent()
        {
            foreach (var c in Children)
            {
                c.Parent = this;
                c.RestoreParent();
            }
        }
    }
}