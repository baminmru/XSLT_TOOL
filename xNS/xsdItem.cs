using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace xNS
{
    public class xsdItem
    {
        [XmlIgnoreAttribute]
        public xsdItem Parent = null;

        [XmlIgnoreAttribute]
        public static Dictionary<string, string> PatternSamples ;


        [XmlIgnoreAttribute]
        public static string vbCrLf = "\r\n";


        public string Name;
        public string Type;
        public string Fixed;
        public string Comments;
        public string oMin;
        public string oMax;
        public string Restrictions;
        public List<string> Patterns =  new List<string>() ;

        public List<xsdItem> Children = new List<xsdItem>();
        public List<xsdItem> Choice = new List<xsdItem>();

        [XmlIgnoreAttribute]
        private xsdItem _prevSibling;
        [XmlIgnoreAttribute]
        private xsdItem _nextSibling;
        [XmlIgnoreAttribute]
        private Boolean prevSiblingOk = false;
        [XmlIgnoreAttribute]
        private Boolean nextSiblingOk = false;



        public static void InitPatternSamples()
        {
            PatternSamples.Add(@"P(\d+[wW])?(\d+[dD])?", "P1W;P1D;P2W3D;P2W5D");
            PatternSamples.Add(@"P(\d+[yY])?(\d+[mM])?(\d+[wW])?(\d+[dD])?", "P1Y2M3W4D;P1Y;P2W;P3D");
            PatternSamples.Add(@"P(\d+[yY])?(\d+[mM])?(\d+[dD])?", "P1Y2M4D;P1Y;P2M;P3D");
            PatternSamples.Add(@"P(\d+Y)?(\d+M)?(\d+W)?(\d+D)?(T(\d+H)?(\d+M)?(\d+(\.\d+)?S)?)?", "P1Y2M4DT5H6M.30S;P1YT2M;P2MT4H;P3D");
            PatternSamples.Add(@"P(\d+[yY])?(\d+[mM])?(\d+[wW])?(\d+[dD])?(T(\d+[hH])?)?", "P1Y2M4DT5H;P1YT2H;P2MT4H;P3D");
            PatternSamples.Add(@"P(T(\d+[mM])?)?", "PT1M;PT10M;PT5M;PT15M");
            PatternSamples.Add(@"P(\d+[dD])?(T(\d+[hH])?(\d+[mM])?)?", "P1DT1H1M;P2DT1H10M;P2DT3H5M;P3DT4H15M");


            PatternSamples.Add(@"(\+|\-)?(0|[1-9][0-9]*)?", "+2;-3;+1;-1;10;15;120;80;1200;500;003");
            PatternSamples.Add(@"(\+|\-)?(0|[1-9][0-9]*)?(\.[0-9]{1})?", "+2;-3;+1;-1;10.00;15.1;120.3;80;1200.1;500.0;003.2");
            PatternSamples.Add(@"(\+|\-)?(0|[1-9][0-9]*)?(\.[0-9]{2})?", "+2;-3;+1;-1;10.00;15.1;120.34;80;1200.1;500.10;003.12");
            PatternSamples.Add(@"(\+|\-)?(0|[1-9][0-9]*)?(\.[0-9]{3})?", "+2;-3;+1;-1;10.000;15.123;120.345;80;1200.12;500.100;003.123");


            //(\+|\-)?(0|[1-9][0-9]*)?
        }

        private string GetPatternSample(string Pattern)
        {
            if (PatternSamples==null)
            {
                PatternSamples = new Dictionary<string, string>();
            }
            if (PatternSamples.Count == 0)
            {
                InitPatternSamples();
            }
            if (PatternSamples.Keys.Contains(Pattern))
            {
                string Variants = PatternSamples[Pattern];
                string[] s = Variants.Split(';');
                Random r = new Random();
                int v = r.Next(s.Length);
                return s[v];
            }
            else
            {
                return Pattern;
            }


        }


        public xsdItem NextSibling()
        {
            if (nextSiblingOk) return _nextSibling;
            _nextSibling = FindNextSibling();
            nextSiblingOk = true;
            return _nextSibling;
        }

        public xsdItem PrevSibling()
        {
            if (prevSiblingOk) return _prevSibling;
            _prevSibling = FindPrevSibling();
            prevSiblingOk = true;
            return _prevSibling;
        }

        private xsdItem FindNextSibling()
        {
            if (Parent != null)
            {
                // find self in parent Children collection
                Boolean returnNext = false;
                foreach (var sibl in Parent.Children)
                {
                    if (returnNext) return sibl;
                    if (Name == sibl.Name) returnNext = true;
                }
            }
            return null;
        }

        private xsdItem FindPrevSibling()
        {
            if (Parent != null)
            {
                // find self in parent Children collection
                Boolean returnNext = false;
                int i;
                xsdItem sibl;
                for (i = Parent.Children.Count - 1; i >= 0; i--)
                {
                    sibl = Parent.Children[i];
                    if (returnNext) return sibl;
                    if (Name == sibl.Name) returnNext = true;
                }
            }
            return null;
        }


        public void RestoreParent()
        {
            foreach (xsdItem c in Children)
            {
                c.Parent = this;
                c.RestoreParent();
            }

            foreach (xsdItem c in Choice)
            {
                c.Parent = this;
                c.RestoreParent();
            }
        }

        public string Level()
        {
            string sOut = "";
            xsdItem xP = Parent;
            while (xP != null)
            {
                sOut += "\t";
                xP = xP.Parent;
            }
            return sOut;
        }

        public static int Cnt;

        public StringBuilder Generate(StringBuilder sb)
        {
            if (sb == null)
            {
                sb = new StringBuilder();
                Cnt = 0;
                sb.Append(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
            }
            string sShift = Level();    

            if (Name != "defining_code")
            {
                int ItemCount = 1;

                if(oMax =="unbounded" || oMax == "99") {
                    Random r = new Random();
                    ItemCount = r.Next(2, 5);
                }
                for (int idx = 1; idx <= ItemCount; idx++)
                {
                    sb.Append(vbCrLf + sShift + "<" + Name + ">");

                    if (Fixed != "")
                    {
                        sb.Append(Fixed);
                    }
                    else
                    {
                        if (this.Type == "xs:int")
                        {
                            Random r = new Random();
                            int v = r.Next(1, Cnt + 1);
                            sb.Append(v.ToString());
                            Cnt++;
                        }
                        if (this.Type == "xs:string")
                        {
                            bool OK = false;
                            if (NextSibling() != null)
                            {
                                if (NextSibling().Name == "defining_code")
                                {

                                    string[] s = NextSibling().Restrictions.Split(';');
                                    Random r = new Random();
                                    int v = r.Next(s.Length);
                                    sb.Append(s[v]);
                                    OK = true;
                                }
                            }
                            if (!OK)
                            {


                                if (Patterns.Count > 0)
                                {
                                    sb.Append(GetPatternSample(Patterns[0]));
                                }
                                else
                                {

                                    if (Parent.Name == "name")
                                    {
                                        sb.Append(Parent.Parent.Name);
                                    }
                                    else
                                    {
                                        if (Parent != null && Parent.Parent != null)
                                        {
                                            sb.Append("строка № " + Cnt.ToString() + " для [" + Parent.Parent.Name.ToLower() + "]");
                                        }
                                        else
                                        {
                                            sb.Append("строка № " + Cnt.ToString() + " для [" + Name.ToLower() + "]");
                                        }

                                        Cnt++;
                                    }
                                }

                            }

                        }

                        if (this.Type == "oe:DV_TEXT")
                        {
                            Random r = new Random();
                            int v = r.Next(1, Cnt + 1);

                            if (Parent != null)
                            {
                                sb.Append(vbCrLf + sShift + "\t<value>" + "текст № " + v.ToString() + " для [" + Parent.Name.ToLower() + "]</value>" + vbCrLf + sShift);
                            }
                            else
                            {
                                sb.Append(vbCrLf + sShift + "\t<value>" + "текст № " + v.ToString() + " для [" + Name.ToLower() + "]</value>" + vbCrLf + sShift);
                            }

                            Cnt++;

                        }

                        if (this.Type == "oe:DV_CODED_TEXT")
                        {
                            Random r = new Random();
                            int v = r.Next(1, Cnt + 1);

                            if (Parent != null)
                            {
                                sb.Append(vbCrLf + sShift + "\t<value>" + "код " + v.ToString() + " - расшифровка № " + v.ToString() + " для [" + Parent.Name.ToLower() + "]</value>" + vbCrLf + sShift);
                            }
                            else
                            {
                                sb.Append(vbCrLf + sShift + "\t<value>" + "код " + v.ToString() + " - расшифровка № " + v.ToString() + " для [" + Name.ToLower() + "]</value>" + vbCrLf + sShift);
                            }


                            //sb.Append(vbCrLf + sShift + "\t<value>" + "код " + Cnt.ToString() +"- текст № " + Cnt.ToString() + "</value>" + vbCrLf + sShift);
                            Cnt++;

                        }

                        //'

                        if (this.Type == "xs:double")
                        {
                            if (Patterns.Count > 0)
                            {
                                sb.Append(GetPatternSample(Patterns[0]));
                            }
                            else
                            {
                                sb.Append(Cnt.ToString() + "." + ((Cnt + 3) % 10).ToString());
                                Cnt++;
                            }
                        }

                        if (this.Type == "oe:DV_BOOLEAN")
                        {
                            sb.Append(vbCrLf + sShift + "\t<value>" + "true" + "</value>" + vbCrLf + sShift);
                            Cnt++;
                        }

                        if (this.Type == "oe:DV_DATE_TIME" || this.Type == "oe:DV_DATE")
                        {
                            sb.Append(vbCrLf + sShift + "\t<value>" + DateTime.Now.AddDays(-Cnt).ToString("yyyy-MM-ddThh:mm:ss") + "</value>" + vbCrLf + sShift);
                        }

                        if (this.Type == "" && Children.Count == 0 && Choice.Count == 0)
                        {
                            sb.Append("Значение № " + Cnt.ToString());
                            Cnt++;
                        }


                    }


                    foreach (xsdItem i in Children)
                    {
                        i.Generate(sb);
                    }

                    foreach (xsdItem i in Choice)
                    {
                        i.Generate(sb);
                    }

                    if (Children.Count > 0 || Choice.Count > 0)
                    {
                        sb.Append(vbCrLf + sShift);
                    }
                    sb.Append("</" + Name + ">");
                }
            }
            return sb; 
        }

    }
}
