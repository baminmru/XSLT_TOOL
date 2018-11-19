using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xNS
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualBasic;
    using System.Xml.Serialization;

    public class XsltItem
    {
        [XmlIgnoreAttribute]
        public XsltItem Parent=null;

        public string ItemID;

        [XmlIgnoreAttribute]
        public static string vbCrLf = "\r\n";

        public string xslFor;
        public string Caption;
        public string FormInfo;
        public string FactorInfo;
        public string Path;
        public Boolean ComaBefore;
        public Boolean DotAfter;
        public Boolean Capitalize;
        public Boolean LineFeed;
        public Boolean LineFeedManual;
        public List<XsltItem> Children = new List<XsltItem>();

        public override string ToString()
        {
            string s;
            int i;
            s = "";
            for (i = 1; i <= Level() - 1; i++)
                s = s + "***|";
            s = s + "#" + ItemID + " (" + Level() + ") " + vbCrLf + "Cap: " + Caption + vbCrLf + "Form: " + FormInfo + vbCrLf + "Factor: " + FactorInfo + vbCrLf + "Path: " + Path + vbCrLf;
            foreach (XsltItem x in Children)
                s = s + vbCrLf + x.ToString();

            return s;
        }
        public  string ToString(Boolean withChildren)
        {
            string s;
            int i;
            s = "";
            if (withChildren)
            {
                for (i = 1; i <= Level() - 1; i++)
                    s = s + "***|";
            }
            s = s + "#" + ItemID  + vbCrLf + "Cap: " + Caption + vbCrLf + "Form: " + FormInfo + vbCrLf + "Factor: " + FactorInfo + vbCrLf + "Path: " + Path + vbCrLf;
            if (withChildren)
            {
                foreach (XsltItem x in Children)
                    s = s + vbCrLf + x.ToString(withChildren);
            }
            return s;
        }
        public int Level()
        {
            if (ItemID.Trim() == "")
                return 0;
            string[] s;
            s = ItemID.Trim().Split('.');
            return s.Count();
        }


        [XmlIgnoreAttribute]
        private XsltItem _prevSibling;
        [XmlIgnoreAttribute]
        private XsltItem _nextSibling;
        [XmlIgnoreAttribute]
        private Boolean prevSiblingOk=false;
        [XmlIgnoreAttribute]
        private Boolean nextSiblingOk=false;

        


        // form helper
        public Boolean WithHeader()
        {
            if (FormInfo.Contains("без заголовка")) return false;
            if (FormInfo.Contains("+")) return true;
            if (IsTOC()) return true;
            return false;
        }

        public Boolean IsTOC()
        {
            if (FormInfo.ToLower().Contains("пункт «оглавления»")) return true;
            return false;
        }

        public Boolean IsNewLine()
        {
            if (LineFeedManual) return LineFeed;
            LineFeed = FormInfo.ToLower().Contains("с новой строки");
            return LineFeed;
        }

         // factor helper
        public Boolean IsMulty()
        {
            if (FactorInfo.Contains(".*")) return true;
            if (FactorInfo.Contains(".2")) return true;
            if (FactorInfo.Contains(".3")) return true;
            if (FactorInfo.Contains(".4")) return true;
            if (FactorInfo.Contains(".5")) return true;
            if (FactorInfo.Contains(".6")) return true;
            if (FactorInfo.Contains(".7")) return true;
            if (FactorInfo.Contains(".8")) return true;
            if (FactorInfo.Contains(".9")) return true;
            return false;
        }

       

        public Boolean IsHeader()
        {
            if (FactorInfo.ToLower().Contains("заголовок раздела")) return true;
            //if (FormInfo.ToLower().Contains("заголовок жирным")) return true;
            return false;
        }
        public Boolean IsBold()
        {
            //if (FactorInfo.ToLower().Contains("заголовок раздела")) return true;
            /* if (FormInfo.ToLower().Contains("жирны"))
                 return true; */
            if (WithHeader())
            {
                if (Level() == 2)
                {
                    return true;
                }

                if(Level()==3 )
                    if( Parent.ItemID.StartsWith("Up"))
                    {
                        return true;
                    }
            }
            return false;
        }

        public Boolean IsDate()
        {
            if (FactorInfo.Contains("DV_DATE_TIME")) return true;
            if (FactorInfo.Contains("DV_DATE")) return true;
            return false;
        }

        public Boolean IsBoolean()
        {
            if (FactorInfo.Contains("DV_BOOLEAN")) return true;
            return false;
        }

        public Boolean IsQuantity()
        {
            if (FactorInfo.Contains("DV_QUANTITY")) return true;
            return false;
        }

        public Boolean IsPeriod()
        {
            if (FactorInfo.Contains("DV_DURATION")) return true;
            return false;
        }

        public Boolean IsSinglePeriod()
        {
            if (FactorInfo.Contains("DV_QUANTITY")) {
                if(FactorInfo.Contains("wk") && FactorInfo.Contains("mo"))
                    return true;
            } 
            return false;
        }


        public Boolean IsSize()
        {
            if (FactorInfo.Contains("DV_QUANTITY"))
            {
                if (FactorInfo.Contains("mm") && FactorInfo.Contains("cm"))
                    return true;
            }
            return false;
        }



        public XsltItem NextSibling()
        {
            if (nextSiblingOk) return _nextSibling;
            _nextSibling = FindNextSibling();
            nextSiblingOk = true;
            return _nextSibling;
        }

        public XsltItem PrevSibling()
        {
            if (prevSiblingOk) return _prevSibling;
            _prevSibling = FindPrevSibling();
            prevSiblingOk = true;
            return _prevSibling;
        }

        private XsltItem FindNextSibling()
        {
            if (Parent != null)
            {
                // find self in parent Children collection
                Boolean returnNext = false;
                foreach(var sibl in Parent.Children)
                {
                    if (returnNext) return sibl;
                    if (ItemID == sibl.ItemID) returnNext = true;
                }
            }
            return null;
        }

        private XsltItem FindPrevSibling()
        {
            if (Parent != null)
            {
                // find self in parent Children collection
                Boolean returnNext = false;
                int i;
                XsltItem sibl;
                for (i =Parent.Children.Count-1; i >= 0; i--)
                {
                    sibl = Parent.Children[i];
                    if (returnNext) return sibl;
                    if (ItemID == sibl.ItemID) returnNext = true;
                }
            }
            return null;
        }
        public void RestoreParent()
        {
            foreach(XsltItem c in Children)
            {
                c.Parent = this;
                c.RestoreParent(); 
            }
        }

    }

}
