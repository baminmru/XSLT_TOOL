using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace xNS
{
    public class xsdItem
    {

        [XmlIgnoreAttribute]
        public static XMLBuilder Builder { get; set; } = null;


        [XmlIgnoreAttribute]
        public xsdItem Parent = null;


        [XmlIgnoreAttribute]
        public static Random Rnd = null;

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
        public List<string> Patterns =  new List<string>() ;

        public List<xsdItem> Children = new List<xsdItem>();
        public List<xsdItem> Choice = new List<xsdItem>();

        [XmlIgnoreAttribute]
        public bool NeedGenerateItem=false;

        [XmlIgnoreAttribute]
        private xsdItem _prevSibling;
        [XmlIgnoreAttribute]
        private xsdItem _nextSibling;
        [XmlIgnoreAttribute]
        private Boolean prevSiblingOk = false;
        [XmlIgnoreAttribute]
        private Boolean nextSiblingOk = false;


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

        public void SetGenPercent( Int16 gPrc)
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

        public void SetNeedGenerate( bool NewValue)
        {
            NeedGenerateItem = NewValue;
            foreach (xsdItem c in Children)
            {
                c.NeedGenerateItem = NewValue;
                c.SetNeedGenerate(NewValue);
            }

            foreach (xsdItem c in Choice)
            {
                c.NeedGenerateItem = NewValue;
                c.SetNeedGenerate(NewValue);
            }
        }

        public void SetMax(Int16 Max)
        {
            oMax = Max.ToString() ;
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

        public bool IsMarkedForGen()
        {

            if (NeedGenerateItem) return true;

            if (NodeLevel() > 4)
            {
                if (Name.ToLower() == "value" ||
                    Name.ToLower() == "magnitude" ||
                    Name.ToLower() == "units" ||
                    Name.ToLower() == "presision")
                {
                    if (Parent.NeedGenerateItem) return true;

                    if (Parent.Parent.NeedGenerateItem) return true;
                }

                //if (Parent.Parent.Parent.NeedGenerateItem) return true;

                //if (Parent.Parent.Parent.Parent.NeedGenerateItem) return true;
            }

            return false ;
            
        }

        private void NeedAllParents()
        {
            xsdItem i;
            i = this;
            i = i.Parent;
            while (i != null)
            {

                i.NeedGenerateItem = true;    
                i = i.Parent;
            }
        }

        private string SmartString(string s)
        {
            string tmp;
            tmp = s.ToLower();
            tmp = tmp.Replace("_col_", "__");
            tmp = tmp.Replace("_equals_", "__");
            tmp = tmp.Replace("_openbrkt_", "__");
            tmp = tmp.Replace("-", "_");
            tmp = tmp.Replace("_closebrkt_", "__");
            tmp = tmp.Replace("_comma_", "__");
            tmp = tmp.Replace("_prd_", "__");
            tmp = tmp.Replace("_fslash_", "__");
            tmp = tmp.Replace(":", "_");
            tmp = tmp.Replace("=", "_");
            tmp = tmp.Replace("(", "_");
            tmp = tmp.Replace(")", "_");
            tmp = tmp.Replace(",", "_");
            tmp = tmp.Replace(".", "_");
            //tmp = tmp.Replace("/", "_");

            int ltmp = tmp.Length + 1;
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


        private string FullPath()
        {
            string sOut;
            sOut = Name;
            xsdItem i;
            i = this;
            i = i.Parent;
            while( i != null){

                sOut = i.Name + "/" + sOut;
                i = i.Parent;
            }
            return sOut;
        }

        private Boolean Finder(string Path)
        {
            string[] Find = Path.Split('/');

            if (Find == null) return true;
            string[] Test = FullPath().Split('/');
            int i, j, start, idxfound;
            string tmp;

            if (true)
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
                   
                   tmp = SmartString(tmp);
                  


                    if (Find[i].ToLower() == tmp)
                    {
                        start = j + 1; // position for next test
                        idxfound = i; // last found index
                        break;
                    }
                    else
                    {
                        // допускаем только наличие  служебных  компонентов внутри пути 
                        if (tmp != "" && !(tmp.ToLower().StartsWith("любое_событие_as_point")
                       || tmp.ToLower().StartsWith("любые_события") || tmp.ToLower() == "точка_во_времени") && tmp != "data" && tmp != "protocol" && tmp != "value")
                        {
                            return false;
                        }
                    }
                }
                if (idxfound < i)
                {
                    return false;
                }


            }

            // все пути нашлись 
            if (idxfound == Find.Length - 1)
            {
                Found = true;
            }

            if (Found)
            {

                if (start <= Test.Length - 1) Found = false;

                //if (Tails != null)
                //{
                //    Found = false;
                //    int t;
                //    int tail;
                //    string[] CurTail;
                //    for (i = 0; i < Tails.Length; i++)
                //    {
                //        CurTail = Tails[i].Split('/');
                //        idxfound = 0;
                //        tail = Test.Length - 1;
                //        // обратный цикл от хвоста
                //        if (CurTail != null)
                //        {
                //            if (CurTail.Length > 0)
                //            {
                //                for (t = CurTail.Length - 1; t >= 0; t--)
                //                {
                //                    for (j = tail; j >= 0; j--) // цикл по проверяемому пути в обратную сторону
                //                    {

                //                        tmp = Test[j].ToLower();
                //                        if (SmartPath)
                //                        {
                //                            tmp = SmartString(tmp);
                //                        }
                //                        string tmpTail;
                //                        tmpTail = CurTail[t].ToLower();
                //                        if (SmartPath)
                //                        {
                //                            tmpTail = SmartString(CurTail[t]);
                //                        }
                //                        if (tmpTail == tmp)
                //                        {
                //                            tail = j - 1; // position for next test
                //                            idxfound++; // last found index
                //                            break;
                //                        }
                //                    }
                //                }

                //                if (idxfound == CurTail.Length)
                //                {
                //                    Found = true;
                //                    break;
                //                }
                //            }
                //        }


                //    }
                //}
            }



            return Found;
        }



        private void MarkForGenerate(xsdItem x, string gPath)
        {
            if (x.NeedGenerateItem == false) { 
                if (x.Finder(gPath))
                {
                    x.NeedGenerateItem = true;
                    System.Diagnostics.Debug.Print("Mark: " + x.FullPath()  +" for "+ gPath );
                    x.NeedAllParents();
                }
            }
            foreach (xsdItem i in x.Children)
            {
                i.MarkForGenerate(i, gPath);
            }
            foreach (xsdItem i in x.Choice)
            {
                i.MarkForGenerate(i, gPath);
            }
        }

        // собственно генерация  узла
        public StringBuilder GeneratePaths(StringBuilder sb, List<string> Paths)
        {
            if (sb == null)
            {
                sb = new StringBuilder();
                Cnt = 0;
                sb.Append(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
                Rnd = new Random();
                System.Diagnostics.Debug.Print("\r\nSTart XML generation\r\n");
                SetNeedGenerate(false);

                // отмечаем все узлы, которые будем генерировать
                foreach(string gPath in Paths)
                {
                    MarkForGenerate(this,gPath);
                }

            }

            string sShift = Level();

            

            if (!XMLBuilder.StopStr.Contains(Name.ToLower()) && Name != "defining_code")
            {


                if (IsMarkedForGen()) { 

                    int ItemCount = 1;

                    if (oMax == "unbounded" || oMax == "99" || oMax == "100" || oMax == "2" || oMax == "4")
                    {
                        ItemCount = Rnd.Next(1, 2);
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
                                sb.Append(vbCrLf + sShift + "\t<units>" + Builder.GetRandomUnit() + "</units>" + vbCrLf + sShift);
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
                                        sb.Append(Builder.GetPatternSample(Patterns[0]));
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
                                                sb.Append("строка \r\n№ " + Cnt.ToString() + " \r\n[" + Parent.Parent.Name.ToLower() + "]");
                                                passValue = true;
                                            }
                                            else
                                            {
                                                sb.Append("строка \r\n№ " + Cnt.ToString() + " \r\n[" + Name.ToLower() + "]");
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
                                    sb.Append(vbCrLf + sShift + "\t<value>" + "текст \r\n№ " + Cnt.ToString() + " \r\n[" + Parent.Name.ToLower() + "]</value>" + vbCrLf + sShift);
                                    passValue = true;
                                }
                                else
                                {
                                    sb.Append(vbCrLf + sShift + "\t<value>" + "текст \r\n№ " + Cnt.ToString() + " \r\n[" + Name.ToLower() + "]</value>" + vbCrLf + sShift);
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
                                    sb.Append(vbCrLf + sShift + "\t<value>" + "Код " + v.ToString() + " - расшифровка \r\n№ " + v.ToString() + " \r\n[" + Parent.Name.ToLower() + "]</value>" + vbCrLf + sShift);
                                    passValue = true;
                                }
                                else
                                {
                                    sb.Append(vbCrLf + sShift + "\t<value>" + "Код " + v.ToString() + " - расшифровка \r\n№ " + v.ToString() + " \r\n[" + Name.ToLower() + "]</value>" + vbCrLf + sShift);
                                    passValue = true;
                                }


                                //sb.Append(vbCrLf + sShift + "\t<value>" + "код " + Cnt.ToString() +"- текст \r\n№ " + Cnt.ToString() + "</value>" + vbCrLf + sShift);
                                Cnt++;

                            }

                            //'

                            if (this.Type == "xs:double")
                            {
                                if (Patterns.Count > 0)
                                {
                                    sb.Append(Builder.GetPatternSample(Patterns[0]));
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
                                //int v = Rnd.Next(0, 2);
                                //if (v == 1)
                                    sb.Append(vbCrLf + sShift + "\t<value>" + "true" + "</value>" + vbCrLf + sShift);
                                //else
                                //    sb.Append(vbCrLf + sShift + "\t<value>" + "false" + "</value>" + vbCrLf + sShift);
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
                                sb.Append("REF \r\n№ " + Cnt.ToString());
                                passValue = true;
                                Cnt++;
                            }



                            if (this.Type == "" && Children.Count == 0 && Choice.Count == 0)
                            {
                                sb.Append("Значение \r\n№ " + Cnt.ToString());
                                passValue = true;
                                Cnt++;
                            }

                            if (passValue == false && this.Type != "" && Children.Count == 0 && Choice.Count == 0)
                            {
                                sb.Append("Значение ещё неизвестного типа " + Cnt.ToString() + ". Тип=" + this.Type);
                            }
                        }


                        if (Choice.Count == 1 || Children.Count == 1) // name + value\value
                        {
                            if (Children.Count == 1)
                            {
                                Children[0].GeneratePaths(sb,Paths);
                                System.Diagnostics.Debug.Print("single child " + Children[0].Name);
                            }
                            if (Choice.Count == 1)
                            {
                                Choice[0].GeneratePaths(sb,Paths);
                                System.Diagnostics.Debug.Print("single choice " + Choice[0].Name);
                            }
                        }
                        else
                        {

                            foreach (xsdItem i in Children)
                            {
                                    i.GeneratePaths(sb,Paths);
                            }

                            if (Choice.Count > 0)
                            {
                                int ifgen =  100;
                                int v = Rnd.Next(Choice.Count);
                                if (Choice[v].NodeLevel() >= 2 && (ifgen >= Choice[v].GenPercent || Choice[v].oMin == "1"))
                                {
                                    Choice[v].GeneratePaths(sb,Paths);
                                    System.Diagnostics.Debug.Print("gen choice " + ifgen.ToString() + " -> " + Choice[v].Name);
                                }
                            }
                        }

                        if (Children.Count > 0 || Choice.Count > 0)
                        {
                            sb.Append(vbCrLf + sShift);
                        }
                        sb.Append("</" + Name + ">");
                    }
                }
            }
            return sb;
        }



            // собственно генерация  узла
            public StringBuilder Generate(StringBuilder sb)
        {
            if (sb == null)
            {
                sb = new StringBuilder();
                Cnt = 0;
                sb.Append(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
                Rnd = new Random();
                System.Diagnostics.Debug.Print("\r\nSTart XML generation\r\n");
            }

            if (Skip) // пропускаем этот узел
            {
                return sb;
            }
            string sShift = Level();    

           
            if (!XMLBuilder.StopStr.Contains(Name.ToLower()) && Name != "defining_code" )
            {
                int ItemCount = 1;

                if(oMax =="unbounded" || oMax == "99" || oMax == "2" || oMax == "4")
                {

                    int ifgen = Rnd.Next(1, 100);
                    if (ifgen >= GenPercent)
                    {
                        ItemCount = Rnd.Next(2, 3);
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
                            sb.Append(vbCrLf + sShift + "\t<magnitude>" + v.ToString() + "</magnitude>" + vbCrLf + sShift );
                            passValue = true;
                            Cnt++;
                        }

                        if (this.Type == "oe:DV_QUANTITY")
                        {

                            int v = Rnd.Next(1, Cnt + 1);
                            sb.Append(vbCrLf + sShift + "\t<magnitude>" + v.ToString() + "</magnitude>");
                            sb.Append(vbCrLf + sShift + "\t<units>" + Builder.GetRandomUnit() + "</units>" + vbCrLf + sShift);
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

                                if (Restrictions != null  && Restrictions != "") {
                                    string[] s = Restrictions.Split(';');
                                    
                                    int v = Rnd.Next(s.Length);
                                    sb.Append(s[v]);
                                    passValue = true;
                                }
                                else if (Patterns.Count > 0)
                                {
                                    sb.Append(Builder.GetPatternSample(Patterns[0]));
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
                                            sb.Append("строка \r\n№ " + Cnt.ToString() + " \r\n[" + Parent.Parent.Name.ToLower() + "]");
                                            passValue = true;
                                        }
                                        else
                                        {
                                            sb.Append("строка \r\n№ " + Cnt.ToString() + " \r\n[" + Name.ToLower() + "]");
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
                                sb.Append(vbCrLf + sShift + "\t<value>" + "текст \r\n№ " + Cnt.ToString() + " \r\n[" + Parent.Name.ToLower() + "]</value>" + vbCrLf + sShift);
                                passValue = true;
                            }
                            else
                            {
                                sb.Append(vbCrLf + sShift + "\t<value>" + "текст \r\n№ " + Cnt.ToString() + " \r\n[" + Name.ToLower() + "]</value>" + vbCrLf + sShift);
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
                                sb.Append(vbCrLf + sShift + "\t<value>" + "Код " + v.ToString() + " - расшифровка \r\n№ " + v.ToString() + " \r\n[" + Parent.Name.ToLower() + "]</value>" + vbCrLf + sShift);
                                passValue = true;
                            }
                            else
                            {
                                sb.Append(vbCrLf + sShift + "\t<value>" + "Код " + v.ToString() + " - расшифровка \r\n№ " + v.ToString() + " \r\n[" + Name.ToLower() + "]</value>" + vbCrLf + sShift);
                                passValue = true;
                            }


                            //sb.Append(vbCrLf + sShift + "\t<value>" + "код " + Cnt.ToString() +"- текст \r\n№ " + Cnt.ToString() + "</value>" + vbCrLf + sShift);
                            Cnt++;

                        }

                        //'

                        if (this.Type == "xs:double")
                        {
                            if (Patterns.Count > 0)
                            {
                                sb.Append(Builder.GetPatternSample(Patterns[0]));
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
                            if(v==1)
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

                        if(this.Type== "oe:LOCATABLE_REF")
                        {
                            sb.Append("REF \r\n№ " + Cnt.ToString());
                            passValue = true;
                            Cnt++;
                        }

                        

                        if (this.Type == "" && Children.Count == 0 && Choice.Count == 0)
                        {
                            sb.Append("Значение \r\n№ " + Cnt.ToString());
                            passValue = true;
                            Cnt++;
                        }

                        if(passValue==false && this.Type != "" && Children.Count == 0 && Choice.Count == 0)
                        {
                            sb.Append("Значение ещё неизвестного типа " + Cnt.ToString() + ". Тип=" +this.Type);
                        }
                    }


                    if (Choice.Count == 1 || Children.Count == 1) // name + value\value
                    {
                        if (Children.Count == 1) {
                            Children[0].Generate(sb);
                            System.Diagnostics.Debug.Print("single child " + Children[0].Name);
                        }
                        if (Choice.Count == 1) {
                            Choice[0].Generate(sb);
                            System.Diagnostics.Debug.Print("single choice " + Choice[0].Name);
                        }
                    }
                    else
                    {

                        foreach (xsdItem i in Children)
                        {

                            int ifgen = Rnd.Next(1, 100);
                            //if (i.NodeLevel() >= 2 && (ifgen >= i.GenPercent || i.oMin == "1"))
                            if ((ifgen >= i.GenPercent || i.oMin == "1"))
                            {
                                i.Generate(sb);
                                System.Diagnostics.Debug.Print("gen children " + ifgen.ToString() +" -> " + i.Name);
                            }
                            else
                            {
                                if(i.NodeLevel()<=3) i.Generate(sb);
                                System.Diagnostics.Debug.Print("gen top children " + ifgen.ToString() + " -> " + i.Name);
                            }
                        }

                        if (Choice.Count > 0)
                        {
                            int ifgen = Rnd.Next(1, 100);
                            int v = Rnd.Next(Choice.Count);
                            if (Choice[v].NodeLevel() >= 2 && (ifgen >= Choice[v].GenPercent || Choice[v].oMin == "1"))
                            {
                                Choice[v].Generate(sb);
                                System.Diagnostics.Debug.Print("gen choice " + ifgen.ToString() + " -> " + Choice[v].Name);
                            }
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
