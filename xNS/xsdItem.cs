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
        public static Dictionary<string, string> PatternSamples;

        [XmlIgnoreAttribute]
        public static Random Rnd;

        //[XmlIgnoreAttribute]
        //public static int RandomPercent=0;  



        [XmlIgnoreAttribute]
        public static string vbCrLf = "\r\n";


        public xsdItem()
        {
            oMin = "0";
            oMax = "1";
            Fixed = "";
            Type = "";
            GenPercent = 0;

        }

        public string Name;
        public string Type;
        public string Fixed;
        public string oMin;
        public string oMax;
        public string Restrictions;
        public Int16 GenPercent;
        public bool Skip;
        public List<string> Patterns = new List<string>();

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

        private string GetRandomUnit()
        {
            string KnownUnits = "Cel|°С;week|неделя;hour|час;month|месяц;year|год;day|день;second|секунда;minute|минута;Sv|Зв;mg/l|мг/л;/yr|/год;10*12/l|10*12/л;cm2|см2;mm/h|мм/ч;Minutes|минуты;/ml|/мл;/mo|/месяц;mm3|мм3;/d|/день;mg|мг;10*9/l|10*9/л;ml|мл;mm|мм;mo|мес;J/min|Дж/мин;ft3|фут3;mm[Hg]|мм.рт.ст.;dioptre|дптр;nanomol/d|нмоль/день;in3|дюйм3;mSv|мЗв;Hz|Гц;gm/l|гм/л;U/l|Е/л;U/ml|Е/мл;/wk|/неделю;fl|фл;IU/ml|МЕ/мл;m/s|м/с;µg|мкг;min|мин;wk|недель;/min|/мин;U|Е;millisec|мс;nanogm/ml|нг/мл;kg|кг;dB|дБ;cc|см3;a|лет;d|дней;m2|м2;gm|г;h|ч;cm|см;kg/m2|кг/м2;m|м;mmol/l|ммоль/л;pg/ml|пг/мл;s|сек;lb|фунты;pg|пг;1/min|в мин";

            string[] s = KnownUnits.Split(';');

            int v = Rnd.Next(s.Length);
            string[] u = s[v].Split('|');
            return u[0];

        }

        private string GetPatternSample(string Pattern)
        {
            if (PatternSamples == null)
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

                int v = Rnd.Next(s.Length);
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

        public void SetGenPercent(Int16 gPrc)
        {
            GenPercent = gPrc;
            foreach (xsdItem c in Children)
            {
                c.GenPercent = gPrc;
                c.SetGenPercent(gPrc);
            }

            foreach (xsdItem c in Choice)
            {
                c.GenPercent = gPrc;
                c.SetGenPercent(gPrc);
            }
        }

        public void SetMax(Int16 Max)
        {
            oMax = Max.ToString();
            foreach (xsdItem c in Children)
            {
                c.oMax = Max.ToString();
                c.SetMax(Max);
            }

            foreach (xsdItem c in Choice)
            {
                c.oMax = Max.ToString();
                c.SetMax(Max);
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

        public int NodeLevel()
        {
            int sOut = 1;
            xsdItem xP = Parent;
            while (xP != null)
            {
                sOut++;
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
                Rnd = new Random();
            }

            if (Skip) // пропускаем этот узел
            {
                return sb;
            }
            string sShift = Level();

            if (Name != "defining_code")
            {
                int ItemCount = 1;

                if (oMax == "unbounded" || oMax == "99" || oMax == "2" || oMax == "4")
                {

                    int ifgen = Rnd.Next(1, 100);
                    if (ifgen >= GenPercent)
                    {
                        ItemCount = Rnd.Next(2, 5);
                    }
                    else
                    {
                        ItemCount = 1;
                    }
                }

                bool passValue;

                for (int idx = 1; idx <= ItemCount; idx++)
                {
                    sb.Append(vbCrLf + sShift + "<" + Name + ">");
                    passValue = false;

                    if (Fixed != "")
                    {
                        sb.Append(Fixed);
                        passValue = true;
                    }
                    else
                    {
                        if (this.Type == "xs:int")
                        {

                            int v = Rnd.Next(1, Cnt + 1);
                            sb.Append(v.ToString());
                            passValue = true;
                            Cnt++;
                        }

                        if (this.Type == "oe:DV_COUNT")
                        {

                            int v = Rnd.Next(1, Cnt + 1);
                            sb.Append(vbCrLf + sShift + "\t<magnitude>" + v.ToString() + "</magnitude>" + vbCrLf + sShift);
                            passValue = true;
                            Cnt++;
                        }

                        if (this.Type == "oe:DV_QUANTITY")
                        {

                            int v = Rnd.Next(1, Cnt + 1);
                            sb.Append(vbCrLf + sShift + "\t<magnitude>" + v.ToString() + "</magnitude>");
                            sb.Append(vbCrLf + sShift + "\t<units>" + GetRandomUnit() + "</units>" + vbCrLf + sShift);
                            passValue = true;
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

                                    int v = Rnd.Next(s.Length);
                                    sb.Append(s[v]);
                                    passValue = true;
                                    OK = true;
                                }
                            }
                            if (!OK)
                            {

                                if (Restrictions != null && Restrictions != "")
                                {
                                    string[] s = Restrictions.Split(';');

                                    int v = Rnd.Next(s.Length);
                                    sb.Append(s[v]);
                                    passValue = true;
                                }
                                else if (Patterns.Count > 0)
                                {
                                    sb.Append(GetPatternSample(Patterns[0]));
                                    passValue = true;
                                }
                                else
                                {

                                    if (Parent.Name == "name")
                                    {
                                        sb.Append(Parent.Parent.Name);
                                        passValue = true;
                                    }
                                    else
                                    {
                                        if (Parent != null && Parent.Parent != null)
                                        {
                                            sb.Append("строка № " + Cnt.ToString() + " [" + Parent.Parent.Name.ToLower() + "]");
                                            passValue = true;
                                        }
                                        else
                                        {
                                            sb.Append("строка № " + Cnt.ToString() + " [" + Name.ToLower() + "]");
                                            passValue = true;
                                        }

                                        Cnt++;
                                    }
                                }

                            }

                        }

                        if (this.Type == "oe:DV_TEXT")
                        {
                            //
                            //int v = Rnd.Next(1, Cnt + 1);

                            if (Parent != null)
                            {
                                sb.Append(vbCrLf + sShift + "\t<value>" + "текст № " + Cnt.ToString() + " [" + Parent.Name.ToLower() + "]</value>" + vbCrLf + sShift);
                                passValue = true;
                            }
                            else
                            {
                                sb.Append(vbCrLf + sShift + "\t<value>" + "текст № " + Cnt.ToString() + " [" + Name.ToLower() + "]</value>" + vbCrLf + sShift);
                                passValue = true;
                            }

                            Cnt++;

                        }

                        if (this.Type == "oe:DV_CODED_TEXT")
                        {
                            //
                            int v = Cnt; //Rnd.Next(1, Cnt + 1);

                            if (Parent != null)
                            {
                                sb.Append(vbCrLf + sShift + "\t<value>" + "Код " + v.ToString() + " - расшифровка № " + v.ToString() + " [" + Parent.Name.ToLower() + "]</value>" + vbCrLf + sShift);
                                passValue = true;
                            }
                            else
                            {
                                sb.Append(vbCrLf + sShift + "\t<value>" + "Код " + v.ToString() + " - расшифровка № " + v.ToString() + " [" + Name.ToLower() + "]</value>" + vbCrLf + sShift);
                                passValue = true;
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
                                passValue = true;
                            }
                            else
                            {

                                int v = Rnd.Next(0, 100);
                                sb.Append(Cnt.ToString() + "." + v.ToString());
                                passValue = true;
                                Cnt++;
                            }
                        }

                        if (this.Type == "oe:DV_BOOLEAN")
                        {
                            int v = Rnd.Next(0, 2);
                            if (v == 1)
                                sb.Append(vbCrLf + sShift + "\t<value>" + "true" + "</value>" + vbCrLf + sShift);
                            else
                                sb.Append(vbCrLf + sShift + "\t<value>" + "false" + "</value>" + vbCrLf + sShift);
                            passValue = true;
                            Cnt++;
                        }

                        if (this.Type == "oe:DV_DATE_TIME" || this.Type == "oe:DV_DATE" || this.Type == "oe:DV_TIME" || this.Type == "oe:Iso8601Date")
                        {
                            sb.Append(vbCrLf + sShift + "\t<value>" + DateTime.Now.AddDays(-Cnt).ToString("yyyy-MM-ddThh:mm:ss") + "</value>" + vbCrLf + sShift);
                            passValue = true;
                        }

                        if (this.Type == "oe:LOCATABLE_REF")
                        {
                            sb.Append("REF № " + Cnt.ToString());
                            passValue = true;
                            Cnt++;
                        }



                        if (this.Type == "" && Children.Count == 0 && Choice.Count == 0)
                        {
                            sb.Append("Значение № " + Cnt.ToString());
                            passValue = true;
                            Cnt++;
                        }

                        if (passValue == false && this.Type != "" && Children.Count == 0 && Choice.Count == 0)
                        {
                            sb.Append("Значение ещё неизвестного типа " + Cnt.ToString() + ". Тип=" + this.Type);
                        }


                    }


                    foreach (xsdItem i in Children)
                    {

                        int ifgen = Rnd.Next(1, 100);
                        if (i.NodeLevel() > 2 && (ifgen >= i.GenPercent || i.oMin == "1"))
                        {
                            i.Generate(sb);
                        }
                        else
                        {
                            i.Generate(sb);
                        }
                    }

                    if (Choice.Count > 0)
                    {
                        int ifgen = Rnd.Next(1, 100);
                        int v = Rnd.Next(Choice.Count);
                        if (Choice[v].NodeLevel() > 2 && (ifgen >= Choice[v].GenPercent || Choice[v].oMin == "1"))
                        {
                            Choice[v].Generate(sb);
                        }
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
