using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace xNS
{
    class SpecPro
    {
        private string[] Ignore;
        private string[] Find;
        private string[] Tails;
        private static string[][] allTails;
        private static XsltItem[] OpenVariable = new XsltItem[10];

        public delegate void ProcessNode(XsltItem sX);

        public event ProcessNode onNextNode;
        public static Boolean DebugPrint
        {
            get; set;
        }

        public Boolean SmartPath
        {
            get; set;
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
            tmp = tmp.Replace("№", "_");
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

        private Boolean Finder(string Path)
        {
            if (Find == null) return true;
            string[] Test = Path.Split('/');
            int i, j, start, idxfound;
            string tmp;

            if (SmartPath)
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
                    if (SmartPath)
                    {
                        tmp = SmartString(tmp);
                    }


                    if (Find[i].ToLower() == tmp)
                    {
                        start = j + 1; // position for next test
                        idxfound = i; // last found index
                        break;
                    }
                    else
                    {
                        // допускаем только наличие  служебных  компонентов внутри пути 
                        if (tmp != "" && !(tmp.ToLower().StartsWith("любое_событие") 
					   || tmp.ToLower().StartsWith("любые_события")   || tmp.ToLower()=="точка_во_времени") && tmp != "data" && tmp != "protocol" && tmp != "value")
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
                if (Tails != null)
                {
                    Found = false;
                    int t;
                    int tail;
                    string[] CurTail;
                    for (i = 0; i < Tails.Length; i++)
                    {
                        CurTail = Tails[i].Split('/');
                        idxfound = 0;
                        tail = Test.Length - 1;
                        // обратный цикл от хвоста
                        if (CurTail != null)
                        {
                            if (CurTail.Length > 0)
                            {
                                for (t = CurTail.Length - 1; t >= 0; t--)
                                {
                                    for (j = tail; j >= 0; j--) // цикл по проверяемому пути в обратную сторону
                                    {

                                        tmp = Test[j].ToLower();
                                        if (SmartPath)
                                        {
                                            tmp = SmartString(tmp);
                                        }
                                        string tmpTail;
                                        tmpTail = CurTail[t].ToLower();
                                        if (SmartPath)
                                        {
                                            tmpTail = SmartString(CurTail[t]);
                                        }
                                        if (tmpTail == tmp)
                                        {
                                            tail = j - 1; // position for next test
                                            idxfound++; // last found index
                                            break;
                                        }
                                    }
                                }

                                if (idxfound == CurTail.Length)
                                {
                                    Found = true;
                                    break;
                                }
                            }
                        }


                    }
                }
            }



            return Found;
        }

      

        // find ns-paths for child nodes of current node
        private List<XmlPlusItem> FindChildList(XsltItem sX)
        {
            List<XmlPlusItem> childList = new List<XmlPlusItem>();
            foreach (XsltItem chX in sX.Children)
            {

                XmlPlusItem xpi = FindNSPath(chX);
                if (xpi != null)
                    childList.Add(xpi);

            }
            return childList;
        }

        private void initIgnore()
        {
            string sIgnore = "mappings;links;language;encoding;provider;subject;other_participations;context;setting;uid;composer";
            if (sIgnore != "")
            {
                Ignore = sIgnore.Split(';');
            }
            else
            {
                Ignore = null;
            }
        }

        private static void initAllTails()
        {
            allTails = new string[4][];
            allTails[0] = new[]  // common tail
            {
                "value/value", "value/rm:value", "value/rm:defining_code/rm:code_string","value/defining_code/code_string", "lower/magnitude",
                "upper/magnitude", "value/magnitude", "value/rm:magnitude", "magnitude","value/numerator"
            };
            allTails[1] = new[]  // single period
            {
                "value/magnitude", "d_value/magnitude", "a_value/magnitude", "wk_value/magnitude",
                "mo_value/magnitude", "_1_per_d_value/magnitude", "_1_per_a_value/magnitude", "_1_per_wk_value/magnitude",
                "_1_per_mo_value/magnitude", "_1_per_yr_value/magnitude","_1_per_h_value/magnitude","h_value/magnitude",
                "_per_mo_value/magnitude","_per_wk_value/magnitude","_per_d_value/magnitude"
            };
            allTails[2] = new[]   // quantity
            {
                "value/magnitude", "h_value/magnitude", "min_value/magnitude", "s_value/magnitude","gm_value/magnitude",
                "g_per_24hr_value/magnitude","_value/magnitude","mg_value/magnitude","micro_g_value/magnitude",
                 "ml_value/magnitude","_1_value/magnitude"
            };
            allTails[3] = new[]   // size
            {
                "mm_value/magnitude", "cm_value/magnitude", "value/magnitude"
            };
        }


        private void ChooseTails(XsltItem sX)
        {
            Tails = allTails[0];
            if (sX.IsQuantity())
            {
                Tails = allTails[2];
            }

            // size and  single  period  is quantyty, so move it later !!!
            if (sX.IsSize()) 
            {
                Tails = allTails[3];
            }

            if (sX.IsSinglePeriod())
            {
                Tails = allTails[1];
            }
           
            if (sX.IsMulty() && sX.Children.Count > 0)
            {
                Tails = null;
            }

        }

        private static  Dictionary<string, XmlPlusItem> PathCache;

        private XmlPlusItem FindNSPath(XsltItem sX)
        {
            if (sX.Path == null) sX.Path = "";
            string sFind = sX.Path;
            if (sX.Path.Trim() == "" || sX.Path.Trim().ToLower().StartsWith("generic"))
            {
                XmlPlusItem x = new XmlPlusItem();
                x.Path = "";
                x.PathNs = "";
                x.NodeText = sX.Caption;
                return x;
            }

            if (PathCache.ContainsKey(sFind))
            {
                return PathCache[sFind];

            }

            ChooseTails(sX);

            SmartPath = true;


            if (sFind != "")
            {
                Find = sFind.Split('/');
            }
            else
            {
                Find = null;
            }

            Boolean printNode = true;
            int xIdx;
            for (xIdx = 0; xIdx < PathList.Count; xIdx++)
            {
                XmlPlusItem xpi = PathList[xIdx];

                printNode = false;

                if (sX.IsMulty() == false)
                {
                    if (xpi.node.Attributes.Count > 0 || (xpi.NodeText != null && xpi.NodeText != ""))
                    {
                        printNode = true;
                    }
                }
                else
                {
                    printNode = true;
                }

                if (printNode)
                {
                    Boolean ok = true;
                    if (Ignore != null)
                    {
                        foreach (string s in Ignore)
                        {
                            if (s.Length > 0)
                            {
                                if (xpi.Path.IndexOf(s) >= 0)
                                {
                                    ok = false;
                                    break;
                                }
                            }
                        }
                    }
                    if (ok)
                    {

                            ok = Finder(xpi.Path);
                    }
                    if (ok)
                    {
                        PathCache.Add(sFind, xpi);
                        return xpi;
                    }
                }

            }
            return null;
        }

        private static XmlDocument xdoc = null;
        private static string xdocPath = "";
        private static List<XmlPlusItem> PathList = null;
       
        public void ReadXML(string xmlPath, string sNS)
        {
            if (SpecPro.xdocPath != xmlPath)
            {
                SpecPro.xdoc = null;
            }
            if (SpecPro.xdoc == null)
            {
                SpecPro.xdoc = new XmlDocument();
                SpecPro.xdoc.Load(xmlPath);
                SpecPro.xdocPath = xmlPath;
                SpecPro.PathList = XmlTools.IterateThroughAllNodes(SpecPro.xdoc, sNS);
                PathCache = new Dictionary<string, XmlPlusItem>();
            }

        }

        public void LoadXML(string xmlData, string sNS)
        {
            
            SpecPro.xdoc = null;
           
            if (SpecPro.xdoc == null)
            {
                SpecPro.xdoc = new XmlDocument();
                SpecPro.xdoc.LoadXml(xmlData);
                SpecPro.xdocPath = "AUTO";
                SpecPro.PathList = XmlTools.IterateThroughAllNodes(SpecPro.xdoc, sNS);
                PathCache = new Dictionary<string, XmlPlusItem>();
            }

        }

        protected virtual SpecPro Processor() { return new SpecPro(); }

        private static StringBuilder Errors = null;

        public string Error()
        {
            if (Errors == null) return "";
            return Errors.ToString();
        }

        public void Init()
        {
            Errors = new StringBuilder();
            initIgnore();
            initAllTails();
        }

        private bool MultyComa(XsltItem sX)
        {

            bool NoComa = false;
            if (sX.IsMulty())
                if (sX.Children.Count > 0)
                {
                    if (sX.Children[sX.Children.Count - 1].DotAfter || sX.Children[sX.Children.Count - 1].Children.Count > 0)
                    {
                        NoComa = true;
                    }
                }
            return NoComa;
         }

        private  string SinglePeriodPathPatch(string PathNs)
        {
            string realpath = PathNs;


            realpath = realpath.Replace("*:d_value", "");
            realpath = realpath.Replace("*:mo_value", "");
            realpath = realpath.Replace("*:a_value", "");
            realpath = realpath.Replace("*:wk_value", "");
            realpath = realpath.Replace("*:yr_value", "");
            realpath = realpath.Replace("*:h_value", "");
            realpath = realpath.Replace("*:_1_per_d_value", "");
            realpath = realpath.Replace("*:_1_per_mo_value", "");
            realpath = realpath.Replace("*:_1_per_a_value", "");
            realpath = realpath.Replace("*:_1_per_wk_value", "");
            realpath = realpath.Replace("*:_1_per_yr_value", "");
            realpath = realpath.Replace("*:_1_per_h_value", "");

            realpath = realpath.Replace("*:mm_value", "");
            realpath = realpath.Replace("*:cm_value", "");
            realpath = realpath.Replace("*:gm_value", "");
            realpath = realpath.Replace("*:g_per_24hr_value", "");
            realpath = realpath.Replace("*:_value", "");
            realpath = realpath.Replace("*:mg_value", "");
            realpath = realpath.Replace("*:micro_g_value", "");
            realpath = realpath.Replace("*:_per_mo_value", "");
            realpath = realpath.Replace("*:_per_mo_value", "");
            realpath = realpath.Replace("*:_per_d_value", "");
            realpath = realpath.Replace("*:ml_value", "");
            realpath = realpath.Replace("*:_1_value", "");
            

            realpath = realpath.Replace("*:min_value", "");
            realpath = realpath.Replace("*:s_value", "");
            

            return realpath;
        }


        private  string ChildTest(StringBuilder sb , XsltItem sX)
        {
            //StringBuilder sb;

            try
            {


                foreach (XsltItem child in sX.Children)
                {
                    if (child.Children.Count == 0)
                    {

                        if (sX.Caption.ToLower() == "купирующий фактор")
                        {
                            sX.Path = sX.Path.Replace("/купирующий_фактор", "/провоцирующий_фактор");
                            child.Path = child.Path.Replace("/купирующий_фактор", "/провоцирующий_фактор");
                        }
                            XmlPlusItem xpi = FindNSPath(child);
                        if (xpi != null && xpi.PathNs != "")
                        {
                            string childPath = xpi.PathNs;

                            if (sX.Children.Count > 0)
                            {


                                if (sX.Caption.ToLower() == "провоцирующий фактор")
                                {
                                    XmlPlusItem parentXpi = FindNSPath(sX);
                                    string SelPath = parentXpi.PathNs;
                                    SelPath = SelPath + "[contains(*:name/*:value,'Провоцирующий')]";
                                    childPath = childPath.Replace(parentXpi.PathNs, SelPath);

                                }

                                if (sX.Caption.ToLower() == "купирующий фактор")
                                {
                                    XmlPlusItem parentXpi = FindNSPath(sX);
                                    string SelPath = parentXpi.PathNs;
                                    SelPath = SelPath + "[contains(*:name/*:value,'Купирующий')]";
                                    childPath = childPath.Replace(parentXpi.PathNs, SelPath);

                                }
                            }

                            if (child.IsMulty())
                            {
                                sb.AppendLine(" or count(" + CutFor(child, SinglePeriodPathPatch(childPath), false) + @") > 0 ");
                            }
                            else
                            {

                                if (child.IsBoolean())
                                {
                                    sb.AppendLine(" or " + CutFor(child, SinglePeriodPathPatch(childPath), false) + @" = 'true' ");
                                }
                                else
                                {
                                    sb.AppendLine(" or " + CutFor(child, SinglePeriodPathPatch(childPath), false) + @" != '' ");
                                }
                            }

                        }
                    }
                    else
                    {
                        if (child.IsMulty())
                        {
                            XmlPlusItem xpi = FindNSPath(child);
                            if (xpi!=null && xpi.PathNs != "")
                            {
                                sb.AppendLine(" or count(" + CutFor(child, SinglePeriodPathPatch(xpi.PathNs), false) + @") > 0 ");
                            }
                        }
                        ChildTest(sb, child);
                    }
                }
            }
            catch(System.Exception ex)
            {
                sb.AppendLine("<!-- Error: " + ex.Message + " " +sX.ItemID+" " + sX.Caption+ " "  + ex.StackTrace + "-->");
            }

            return sb.ToString();
        }

        public string Process(string xmlPath, XsltItem sX, Boolean excludeFor)
        {

            string sNS = "*";
            sX.Path=sX.Path.Replace("/купирующий_фактор", "/провоцирующий_фактор");

            try
            {
                ReadXML(xmlPath, sNS); // read once
            }
            catch (System.Exception ex)
            {
                return "<!-- Error: " + ex.Message + " -->";
            }
          
            try
            {
                StringBuilder sb = new StringBuilder();
                XmlPlusItem xpi = FindNSPath(sX);

                if (xpi != null)
                {
                    onNextNode?.Invoke(sX);

                    if (DebugPrint)
                    {
                        sb.AppendLine("<!-- " + sX.ToString(false) + " -->");
                        sb.AppendLine("<!-- XPath    : " + xpi.PathNs + " -->");
                        if (xpi.NodeText != null && xpi.NodeText != "")
                        {
                            sb.AppendLine("<!-- Node text: " + xpi.NodeText + " -->");
                        }
                    }

                    // закрытие предыдущих переменных
                    if (sX.IsMulty())  // перед циклом закрываем переменную 
                    {
                        bool CloseVar = true;
                        if(sX.Children.Count==1)
                        {
                            if (sX.Children[0].Children.Count == 0)
                            {
                                CloseVar = false;
                            }
                        }

                        if (sX.Children.Count == 0)
                        {
                            CloseVar = false;
                        }

                        if (CloseVar)
                        {
                            if (OpenVariable[0] != null)
                            {
                                if (DebugPrint) sb.AppendLine("<!-- Close prev var before FOR -->");
                                
                                sb.AppendLine("</xsl:variable>");
                                
                                if (sX.ComaBefore == false)
                                {
                                    if(OpenVariable[0].Parent != null  && OpenVariable[0].Parent.WithCaption() && OpenVariable[0].Parent.IsHeader())
                                    {
                                        sb.AppendLine(@"<xsl:call-template name=""string-ltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                                    }
                                    else
                                    {
                                        sb.AppendLine(@"<xsl:call-template name=""string-capltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                                    }
                                    
                                    sb.AppendLine(@"<xsl:if test = ""$content" + OpenVariable[0].ItemID.Replace(".", "_") + @" !='' and not (ends-with($content" + OpenVariable[0].ItemID.Replace(".", "_") + @",'. '))"" >. </xsl:if>");
                                }
                                else
                                {
                                    sb.AppendLine(@"<xsl:call-template name=""string-ltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                                }

                                OpenVariable[0] = null;
                            }
                        }

                    }
                    else if (sX.IsTOC())
                    {
                        if (OpenVariable[0] != null)
                        {
                            if (DebugPrint) sb.AppendLine("<!-- Close prev var TOC  -->");
                            sb.AppendLine("</xsl:variable>");
                            if (OpenVariable[0].WithCaption()) {
                                sb.AppendLine(@"<xsl:call-template name=""string-ltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                            }
                            else
                            {
                                //sb.AppendLine(@"<xsl:call-template name=""string-capltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                                if (OpenVariable[0].Parent != null && OpenVariable[0].Parent.WithCaption() && OpenVariable[0].Parent.IsHeader())
                                {
                                    sb.AppendLine(@"<xsl:call-template name=""string-ltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                                }
                                else
                                {
                                    sb.AppendLine(@"<xsl:call-template name=""string-capltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                                }
                            }
                            
                            sb.AppendLine(@"<xsl:if test = ""$content" + OpenVariable[0].ItemID.Replace(".", "_") + @" !='' and not (ends-with($content" + OpenVariable[0].ItemID.Replace(".", "_") + @",'. '))"" >. </xsl:if>");
                            OpenVariable[0] = null;
                        }


                    }
                    else if (sX.IsHeader())
                    {

                        if (OpenVariable[0] != null)
                        {
                            if (DebugPrint) sb.AppendLine("<!-- Close prev var Hdr -->");
                            sb.AppendLine("</xsl:variable>");
                            if (OpenVariable[0].WithCaption())
                            {
                                sb.AppendLine(@"<xsl:call-template name=""string-ltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                            }
                            else
                            {
                                //sb.AppendLine(@"<xsl:call-template name=""string-capltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                                if (OpenVariable[0].Parent != null && OpenVariable[0].Parent.WithCaption() && OpenVariable[0].Parent.IsHeader())
                                {
                                    sb.AppendLine(@"<xsl:call-template name=""string-ltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                                }
                                else
                                {
                                    sb.AppendLine(@"<xsl:call-template name=""string-capltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                                }
                            }

                            //sb.AppendLine(@"<xsl:call-template name=""string-capltrim""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                            sb.AppendLine(@"<xsl:if test = ""$content" + OpenVariable[0].ItemID.Replace(".", "_") + @" !='' and not (ends-with($content" + OpenVariable[0].ItemID.Replace(".", "_") + @",'. '))"" >. </xsl:if>");
  
                            OpenVariable[0] = null;
                        }

                    }

                    List<XmlPlusItem> ChildList = null;
                    ChildList = FindChildList(sX);
                    string sTest;
                    if (sX.Children.Count == 0)
                    {
                        if (sX.IsBoolean())
                        {
                            
                            sTest =  CutFor(sX, xpi.PathNs, excludeFor) + @" = 'true'  ";
                            
                        }else if (sX.IsSinglePeriod())
                        {

                            string realpath = SinglePeriodPathPatch(xpi.PathNs);

                            sTest = CutFor(sX, realpath, excludeFor) + @" != '' ";

                        }
                        else if (sX.IsSize())
                        {

                            string realpath = SinglePeriodPathPatch(xpi.PathNs);

                            sTest = CutFor(sX, realpath, excludeFor) + @" != '' ";

                        }
                        else if (xpi.PathNs != "")
                        {
                            sTest = CutFor(sX, xpi.PathNs, excludeFor) + @" != '' ";
                        }
                        else
                        {
                            sTest = " '1' = '1' "; // true 
                        }
                    } else{
                        sTest = " '1' = '0' ";

                        StringBuilder sbChild = new StringBuilder();
                        ChildTest(sbChild, sX);
                        sTest = sTest + sbChild.ToString(); 

                        //foreach (XmlPlusItem child in ChildList)
                        //{
                        //    if (child.PathNs != "")
                        //    {
                        //        sTest = sTest + "\r\n or " + CutFor(sX, SinglePeriodPathPatch(child.PathNs), excludeFor) + @" != '' ";
                        //    }
                        //}

                    }

                        if (sX.IsMulty() == false)
                        {
                            if (sX.WithCaption())
                            {
                                if (sX.IsTOC() == false && sX.IsHeader()==false)
                                {

                                    if (OpenVariable[0] == null)
                                    {
                                        sb.Append("<!-- with Header -->");
                                        sb.Append("<xsl:variable name='content" + sX.ItemID.Replace(".", "_") + "' >");
                                        OpenVariable[0] = sX;
                                    }
                                }
                            }
                            else
                            {
                                if (sX.Children.Count == 0)
                                {
                                    if (OpenVariable[0] == null)
                                    {
                                        sb.Append("<!-- NO Header -->");
                                        sb.Append("<xsl:variable name='content" + sX.ItemID.Replace(".", "_") + "' >");
                                        OpenVariable[0] = sX;
                                    }
                                }

                            }
                        }
                        

                        sb.AppendLine(@"<!-- "+ sX.ItemID+" --><xsl:if ");
                        sb.AppendLine(@" test  =""" + sTest + @""" ");
                        sb.Append(">");
                    

                    if (sX.IsMulty() == false)
                    {
                        if (sX.IsNewLine())
                        {
                            if (DebugPrint) sb.Append("<!-- new line -->");
                            sb.Append("<br/>");
                        }


                        // вывод заголока
                        if (sX.WithCaption())
                        {
                            if (sX.IsTOC())
                            {
                                // началась  левая колонка
                                sb.AppendLine(TocStart(sX.Caption));

                                // все до первого заголовка  забираем в переменную чтобы убрать  запятые и капитализировать ???
                                sb.Append("<xsl:variable name='content" + sX.ItemID.Replace(".", "_") + "' >");
                                OpenVariable[0] = sX;


                            }
                            else if (sX.IsHeader())
                            {

                                //sb.Append("<strong>" + sX.Caption + ": </strong>");
                                sb.Append(HeaderStart(sX.Caption, sX));

                                sb.Append("<xsl:variable name='content" + sX.ItemID.Replace(".", "_") + "' >");
                                OpenVariable[0] = sX;
                            }
                            else
                            {
                                if (sX.IsBoolean())
                                {
                                    if (sX.ComaBefore) sb.Append("<span>, </span>");
                                }
                                else
                                {


                                    if (sX.Capitalize)
                                    {
                                        sb.Append(ItemStart(sX.Caption, sX));
                                    }
                                    else
                                    {
                                        sb.Append(ItemStart(sX.Caption.ToLower(), sX));
                                    }

                                }

                            }
                        }
                        else
                        {
   
                            if (sX.ComaBefore) sb.Append(", ");

                        }

                        // собственно вывод данных                                        
                        if (sX.IsTOC() || sX.IsHeader())
                        {
                            // no body output, only header !!!
                        }
                        else if (sX.IsSinglePeriod())
                        {

                            string realpath = SinglePeriodPathPatch(xpi.PathNs);
                            string tmp;

                            tmp = @"<xsl:call-template name=""SinglePeriodFormat"">";
                            tmp += @"<xsl:with-param name=""v"" select=""" + CutFor(sX, realpath, excludeFor) + @"""/>";
                            tmp += @"<xsl:with-param name=""pString"" select=""" + CutFor(sX, realpath, excludeFor).Replace(":magnitude", ":units") + @"""/>";
                            tmp += @"</xsl:call-template>";
                            sb.AppendLine(tmp);

                        }

                        else if (sX.IsQuantity() || sX.IsSize() )
                        {

                            if (sX.IsDecimal())
                            {
                                sb.AppendLine(@"<xsl:call-template name=""showDecimal"">");
                                sb.AppendLine(@"<xsl:with-param name=""string"" select=""" + CutFor(sX, SinglePeriodPathPatch(xpi.PathNs), excludeFor) + @"""/></xsl:call-template>");
                            }
                            else
                            {
                                sb.AppendLine(@"<xsl:call-template name=""string-ltrim"">");
                                sb.AppendLine(@"<xsl:with-param name=""string"" select=""" + CutFor(sX, SinglePeriodPathPatch(xpi.PathNs), excludeFor) + @"""/></xsl:call-template>");
                            }
                            
                            sb.AppendLine(@"<xsl:if test=""" + CutFor(sX, SinglePeriodPathPatch(xpi.PathNs), excludeFor).Replace(":magnitude", ":units") + @" !='' "" >");
                            //sb.AppendLine(@"<span> </span>");  вставка пробела перенесена внутрь шаблона edizm
                            sb.AppendLine(@"<xsl:call-template name=""edizm"">");
                            sb.AppendLine(@"<xsl:with-param name=""val"" select=""" + CutFor(sX, SinglePeriodPathPatch(xpi.PathNs), excludeFor).Replace(":magnitude", ":units") + @"""/>");
                            sb.Append(@"</xsl:call-template>");
                            sb.Append(@"</xsl:if>"); 
                        }
                        else if (sX.IsDate())
                        {
                            sb.AppendLine(@" <xsl:call-template name=""DateFormat"">");
                            sb.AppendLine(@"<xsl:with-param name=""dateString"" select=""" + CutFor(sX, xpi.PathNs, excludeFor) + @"""/>");
                            sb.Append(@"</xsl:call-template>");
                        }
                        else if (sX.IsOrdinal())
                        {
                            sb.AppendLine(@"(<xsl:call-template name=""string-capltrim_nobr"">");
                            sb.AppendLine(@"<xsl:with-param name=""string"" select=""" + CutFor(sX, xpi.PathNs, excludeFor) + @"""/>");
                            sb.Append(@"</xsl:call-template>) ");
                            sb.Append(@"<xsl:call-template name=""string-capltrim_nobr"">");
                            sb.AppendLine(@"<xsl:with-param name=""string"" select=""" + CutFor(sX, xpi.PathNs.Replace("*:value/*:value", "*:value/*:symbol/*:value"), excludeFor) + @"""/>");
                            sb.Append(@"</xsl:call-template>");
                        }
                        else  if (sX.IsProportion ())
                        {
                            sb.AppendLine(@"<xsl:call-template name=""string-capltrim_nobr"">");
                            sb.AppendLine(@"<xsl:with-param name=""string"" select=""" + CutFor(sX, xpi.PathNs, excludeFor) + @"""/>");
                            sb.Append(@"</xsl:call-template> ");
                            sb.Append(@"/ <xsl:call-template name=""string-capltrim_nobr"">");
                            sb.AppendLine(@"<xsl:with-param name=""string"" select=""" + CutFor(sX, xpi.PathNs.Replace("*:value/*:numerator", "*:value/*:denominator"), excludeFor) + @"""/>");
                            sb.Append(@"</xsl:call-template>");
                        }

                        else if (sX.IsPeriod())
                        {
                            if (sX.Caption.EndsWith("ие"))
                            {
                                sb.AppendLine(@"<xsl:call-template name=""PeriodFormatIN"">");
                                sb.AppendLine(@"<xsl:with-param name=""pString"" select=""" + CutFor(sX, xpi.PathNs, excludeFor) + @"""/>");
                                sb.Append(@"</xsl:call-template>");
                            }
                            else
                            {
                                sb.AppendLine(@"<xsl:call-template name=""PeriodFormat"">");
                                sb.AppendLine(@"<xsl:with-param name=""pString"" select=""" + CutFor(sX, xpi.PathNs, excludeFor) + @"""/>");
                                sb.Append(@"</xsl:call-template>");
                            }
                        }
                        else if (sX.IsBoolean())
                        {
                            sb.Append("<span>" + sX.Caption.ToLower() + "</span>");
                        }
                        else
                        {
                            if (sX.WithCaption() == false)
                            {

                                bool UseCap = false;
                                if (sX.Parent != null)
                                {
                                    bool nc = MultyComa(sX.Parent);
                                    if (nc)
                                    {
                                        if (sX.ItemID == sX.Parent.Children[0].ItemID)
                                        {
                                            UseCap = true;
                                        }
                                    }
                                }

                                if (sX.Capitalize || UseCap)
                                {
                                    if (UseCap)
                                        sb.AppendLine("<xsl:if test='position()>1' >");
                                    if(sX.IsBrField())
                                        sb.AppendLine(@"<xsl:call-template name=""string-capltrim"">");
                                    else
                                        sb.AppendLine(@"<xsl:call-template name=""string-capltrim_nobr"">");
                                    sb.AppendLine(@"<xsl:with-param name=""string"" select=""" + CutFor(sX, xpi.PathNs, excludeFor) + @"""/>");
                                    sb.Append(@"</xsl:call-template>");
                                    if (UseCap)
                                    {
                                        sb.AppendLine("</xsl:if>");
                                        sb.AppendLine("<xsl:if test='position()=1' >");

                                        if (  sX.Parent != null && sX.Parent.WithCaption() && sX.Parent.IsHeader())
                                        {
                                            if (sX.IsBrField())
                                                sb.AppendLine(@"<xsl:call-template name=""string-ltrim"">");
                                            else
                                                sb.AppendLine(@"<xsl:call-template name=""string-ltrim_nobr"">");

                                        }
                                        else
                                        {
                                            if (sX.IsBrField())
                                                sb.AppendLine(@"<xsl:call-template name=""string-capltrim"">");
                                            else
                                                sb.AppendLine(@"<xsl:call-template name=""string-capltrim_nobr"">");
                                        }

                                        sb.AppendLine(@"<xsl:with-param name=""string"" select=""" + CutFor(sX, xpi.PathNs, excludeFor) + @"""/>");
                                        sb.Append(@"</xsl:call-template>");
                                        sb.AppendLine("</xsl:if>");
                                    }
                                }
                                else
                                {
                                    if (sX.IsBrField())
                                        sb.AppendLine(@"<xsl:call-template name=""string-ltrim"">");
                                    else
                                        sb.AppendLine(@"<xsl:call-template name=""string-ltrim_nobr"">");
                                    sb.AppendLine(@"<xsl:with-param name=""string"" select=""" + CutFor(sX, xpi.PathNs, excludeFor) + @"""/>");
                                    sb.Append(@"</xsl:call-template>");
                                }
                            }
                            else
                            {
                                if (sX.IsBrField())
                                    sb.AppendLine(@"<xsl:call-template name=""string-ltrim"">");
                                else
                                    sb.AppendLine(@"<xsl:call-template name=""string-ltrim_nobr"">");
                                sb.AppendLine(@"<xsl:with-param name=""string"" select=""" + CutFor(sX, xpi.PathNs, excludeFor) + @"""/>");
                                sb.Append(@"</xsl:call-template>");
                            }



                        }

                        // process children from specification
                        if (sX.Children.Count > 0)
                        {
                            SpecPro sp2 = Processor();
                            sp2.onNextNode = onNextNode;
                            foreach (var sX2 in sX.Children)
                            {
                                sb.Append(sp2.Process(xmlPath, sX2, excludeFor));
                            }
                        }






                    } // not multy
                    else
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////                                                                       
                    {  // multy row

                        // вывод заголовка           
                        if (sX.WithCaption())
                        {
                            if (sX.IsTOC())
                            {
                                // началась  левая колонка
                                sb.Append(TocStart(sX.Caption));

                            }
                            else if (sX.IsHeader())
                            {
                                //sb.Append("<strong>" + sX.Caption + ": </strong>");
                                sb.Append(HeaderStart(sX.Caption, sX));

                            }
                            else
                            {
                                if (sX.IsBoolean())
                                {
                                    sb.Append(ItemStart("", sX));
                                }
                                else
                                {
                                    sb.Append(ItemStart(sX.Caption.ToLower(), sX));

                                }
                            }
                        }
                        else
                        {
                            if (sX.Children.Count == 0)
                            {
                                sb.Append(ItemStart("", sX));
                            }


                        }
                        sX.xslFor = CutFor(sX, xpi.PathNs, excludeFor);
                        String SelPath;
                        SelPath = CutFor(sX, xpi.PathNs, excludeFor);
                        if (SelPath == "")
                        {
                            SelPath = "/ERROR";
                            sb.AppendLine(@"<xsl:text> [ОШИБКА: ПУСТОЙ ПУТЬ ДЛЯ  ЦИКЛА, ID:"+ sX.ItemID +"] </xsl:text>");
                        }

                        if (sX.Children.Count > 0)
                        {
                            if (sX.Caption.ToLower() == "провоцирующий фактор")
                            {
                                SelPath = SelPath + "[contains(*:name/*:value,'Провоцирующий')]";
                            }

                            if (sX.Caption.ToLower() == "купирующий фактор")
                            {
                                SelPath = SelPath + "[contains(*:name/*:value,'Купирующий')]";
                            }
                        }

                        sb.AppendLine(@"<xsl:for-each select=""" + SelPath + @""">");
                        if (sX.IsNewLine())
                        {
                            sb.AppendLine(@"<xsl:if test  =""position()>0 ""><br/></xsl:if>");
                        }

                        bool NoComa = false;

                        NoComa = MultyComa(sX);

                        
                        if(NoComa==false)
                            sb.Append(@"<xsl:if test  =""position() >1  ""><span>, </span></xsl:if>");


                        if (sX.Children.Count > 0)
                        {
                            //  цикл с подчиненными атрибутами
                            bool CloseVar = true;
                            if (sX.Children.Count == 1)
                            {
                                if (sX.Children[0].Children.Count == 0)
                                {
                                    CloseVar = false;
                                }
                            }

                            if (sX.Children.Count == 0)
                            {
                                CloseVar = false;
                            }

                            if (CloseVar)
                            {

                                // содержимое цикла забираем в переменную чтобы убрать запятые и капитализировать ???
                                sb.Append("<xsl:variable name='content" + sX.ItemID.Replace(".", "_") + "' >");
                                OpenVariable[0] = sX;
                            }

                            SpecPro sp2 = Processor();
                            sp2.onNextNode = onNextNode;
                            foreach (var sX2 in sX.Children)
                            {
                                sb.Append(sp2.Process(xmlPath, sX2, true));
                            }


                            if (CloseVar)
                            {
                                // закрываем переменную, чтобы не  пересекать границы  цикла
                                if (OpenVariable[0] != null)
                                {
                                    sb.AppendLine("</xsl:variable>");
                                    bool CapSecond = false;
                                    if(sX.Children.Count > 0)
                                    {
                                        if(sX.Children[sX.Children.Count-1].DotAfter)
                                        {
                                            CapSecond = true;
                                        }

                                    }

                                    if (CapSecond)
                                    {
                                        sb.AppendLine(@"<xsl:if test='position()=1' ><xsl:call-template name=""string-ltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template></xsl:if>");
                                        sb.AppendLine(@"<xsl:if test='position()>1' ><xsl:call-template name=""string-capltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template></xsl:if>");
                                    }
                                    else {
                                        if (sX.Children.Count > 0)
                                        {
                                            sb.AppendLine(@"<xsl:if test='position()=1' ><xsl:call-template name=""string-ltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template></xsl:if>");
                                            sb.AppendLine(@"<xsl:if test='position()>1' ><xsl:call-template name=""string-cltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template></xsl:if>");
                                        }
                                        else
                                        {
                                            sb.AppendLine(@"<xsl:call-template name=""string-ltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                                        }
                                    }

                                    
                                    sb.AppendLine(@"<xsl:if test = ""$content" + OpenVariable[0].ItemID.Replace(".", "_") + @" !='' and not (ends-with($content" + OpenVariable[0].ItemID.Replace(".", "_") + @",'. '))"" >. </xsl:if>");
                                    OpenVariable[0] = null;
                                }
                            }


                        }
                        else
                        {
                            // дочерних элементов нет
                            // ситуация  когда надо капитализировать первый элемент цикла. Возможно условие придется  уточнять
                            if (sX.WithCaption()==false && (sX.PrevSibling()!=null  && sX.PrevSibling().DotAfter))
                            {
                                sb.AppendLine(@"<xsl:if test='position()=1' ><xsl:call-template name=""string-capltrim_nobr""><xsl:with-param name = ""string"" select = '.' /></xsl:call-template></xsl:if>");
                            }
                            else
                            {
                                sb.AppendLine(@"<xsl:if test='position()=1' ><xsl:call-template name=""string-ltrim_nobr""><xsl:with-param name = ""string"" select = '.' /></xsl:call-template></xsl:if>");
                            }
                                
                            sb.AppendLine(@"<xsl:if test='position()>1' ><xsl:call-template name=""string-ltrim_nobr""><xsl:with-param name = ""string"" select = '.' /></xsl:call-template></xsl:if>");
                        }

                        sb.Append(@"</xsl:for-each>");
                    } // end multy row



                    // был заголовок -  надо закрыть переменную
                    if (sX.IsTOC())
                    {
                        // закрываем строку
                        if (OpenVariable[0] != null)
                        {
                            sb.AppendLine("</xsl:variable>");
                            if (OpenVariable[0].WithCaption())
                            {
                                sb.AppendLine(@"<xsl:call-template name=""string-ltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                            }
                            else
                            {
                                if (OpenVariable[0].Parent != null && OpenVariable[0].Parent.WithCaption() && OpenVariable[0].Parent.IsHeader())
                                {
                                    sb.AppendLine(@"<xsl:call-template name=""string-ltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                                }
                                else
                                {
                                    sb.AppendLine(@"<xsl:call-template name=""string-capltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                                }
                                //sb.AppendLine(@"<xsl:call-template name=""string-capltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                            }
                            //sb.AppendLine(@"<xsl:call-template name=""string-capltrim""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                            sb.AppendLine(@"<xsl:if test = ""$content" + OpenVariable[0].ItemID.Replace(".", "_") + @" !='' and not (ends-with($content" + OpenVariable[0].ItemID.Replace(".", "_") + @",'. '))"" >. </xsl:if>");
                       
                            OpenVariable[0] = null;
                        }

                        sb.AppendLine(TocEnd(sX));
                        if (DebugPrint) sb.AppendLine("<!-- END of TOC  -->");
                    }
                    else if (sX.IsHeader())
                    {
                        sb.Append(HeaderEnd(sX.Caption,sX));
                        if (DebugPrint) sb.Append("<!-- END of Header  -->");
                        if (OpenVariable[0] != null)
                        {
                            sb.AppendLine("</xsl:variable>");
                            if (OpenVariable[0].WithCaption())
                            {
                                sb.AppendLine(@"<xsl:call-template name=""string-ltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                            }
                            else
                            {
                                sb.AppendLine(@"<xsl:call-template name=""string-capltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                            }
                            sb.AppendLine(@"<xsl:if test = ""$content" + OpenVariable[0].ItemID.Replace(".", "_") + @" !='' and not (ends-with($content" + OpenVariable[0].ItemID.Replace(".", "_") + @",'. '))"" >. </xsl:if>");
                            OpenVariable[0] = null;
                        }

                    }
                    else
                    {
                        //  финал  атрибута
                        sb.Append(ItemEnd(sX));
                    }

                    sb.Append(@"</xsl:if>");
                    if (DebugPrint) sb.AppendLine(@"<!-- End of #" + sX.ItemID + " -->");

                }
                else
                {
                    if (DebugPrint) sb.AppendLine("<!-- " + sX.ToString(false) + " -->");
                    if (DebugPrint) sb.AppendLine("<!-- Error: Данные не найдены в XML файле -->");
                    sb.AppendLine("<xsl:text> [ОШИБКА: Нет данных для поля (" + sX.Caption  + "), ID:" + sX.ItemID  + "] </xsl:text>");
                    Errors.AppendLine(sX.ToString(false));
                }


                return sb.ToString();
            }
            catch (System.Exception ex)
            {
                return "<!-- Error: " + ex.Message + " -->";
            }


        }

      
      

       

        protected virtual string TocStart(string Caption)
        {
            string sOut = @"<tr>
	            <td colspan=""2"" class=""myline"" style=""border-top: 0.5pt solid rgba(0,0,0,0.4); height:1pt""/>
                </tr>
                <tr>
	                <td class=""mytd"" valign=""top"" align=""left"" width=""116pt"">
		                <table width=""116pt"" style=""border-collapse: collapse;"" border=""0"" cellspacing=""0"" cellpadding=""0"">
			                <thead>
				                <tr>
					                <th class=""myth"" valign=""top"" align=""left"">
						                <span class=""part"">" + Caption + @"</span>
					                </th>
				                </tr>
			                </thead>
		                </table>
	                </td>
	                <td class=""mytd"" valign=""top"" align=""left"">";
            return sOut;

        }


        protected virtual string TocEnd(XsltItem sX)
        {
            string sOut = "";
            if (sX.DotAfter) sOut += ". "; //<!-- TocEnd -->
            sOut += @"</td></tr>";
            return sOut;

        }

        protected virtual string HeaderEnd(string Caption, XsltItem sX)
        {
            string sOut = @"";
            if (sX.DotAfter) sOut += ". "; // <!-- HdrEnd -->
            return sOut;

        }

        protected virtual string HeaderStart(string Caption, XsltItem sX)
        {
            string sOut = "";

            if (Caption.Trim() != "")
                if (sX.IsBold())
                {
                    sOut += @"<strong>" + Caption + ": </strong>";
                }
                else
                {
                    sOut += @"<span>" + Caption + ": </span>";
                }

            return sOut;

        }

        protected virtual string ItemStart(string Caption, XsltItem sX)
        {
            string sOut = "";
            if (sX.ComaBefore) sOut+=", ";
            if(Caption.Trim() !="")
                sOut += @"<span>" + Caption + ": </span>";
            return sOut;
        }

        protected virtual string ItemEnd(XsltItem sX)
        {
            string sOut = @"";
            if (sX.DotAfter) sOut+= ". "; //<!-- ItemEnd -->
            return sOut;
        }

        private string CutFor(XsltItem x, string Path, Boolean excludeFor)
        {
            string cf;
            XsltItem xParent;
            xParent = x.Parent;
            cf = Path;
            if (excludeFor)
            {
                while (xParent != null)
                {
                    
                    if (xParent.xslFor != null && xParent.xslFor != "")
                    {
                        if (cf.Contains(xParent.xslFor))
                        {
                            cf = cf.Replace(xParent.xslFor + "/", "");
                        }
                    }
                    xParent = xParent.Parent;
                }
            }
            return cf;
        }

    }
}
