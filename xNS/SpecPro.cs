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
            tmp = tmp.Replace(".", "_");
            tmp = tmp.Replace(",", "_");
            tmp = tmp.Replace("(", "_");
            tmp = tmp.Replace(")", "_");

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

        private static int HeaderLevel = 0;

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
                "value/value", "value/rm:value", "value/rm:defining_code/rm:code_string", "lower/magnitude",
                "upper/magnitude", "value/magnitude", "value/rm:magnitude", "magnitude"
            };
            allTails[1] = new[]  // single period
            {
                "value/magnitude", "d_value/magnitude", "a_value/magnitude", "wk_value/magnitude",
                "mo_value/magnitude", "_1_per_d_value/magnitude", "_1_per_a_value/magnitude", "_1_per_wk_value/magnitude",
                "_1_per_mo_value/magnitude", "_1_per_yr_value/magnitude","_1_per_h_value/magnitude","h_value/magnitude"
            };
            allTails[2] = new[]   // quantity
            {
                "value/magnitude", "h_value/magnitude", "min_value/magnitude", "s_value/magnitude"
            };
            allTails[3] = new[]   // size
            {
                "mm_value/magnitude", "cm_value/magnitude"
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

        private XmlPlusItem FindNSPath(XsltItem sX)
        {
            string sFind = sX.Path;

            if (sX.Path.Trim() == "" || sX.Path.Trim().ToLower().StartsWith("generic"))
            {
                XmlPlusItem x = new XmlPlusItem();
                x.Path = "";
                x.PathNs = "";
                x.NodeText = sX.Caption;
                return x;
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
                        return xpi;
                    }
                }

            }
            return null;
        }

        private static XmlDocument xdoc = null;
        private static string xdocPath = "";
        private static List<XmlPlusItem> PathList = null;
        private static Boolean FirstAfterTOC = false;

        private void ReadXML(string xmlPath, string sNS)
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
            return realpath;
        }

        public string Process(string xmlPath, XsltItem sX, Boolean excludeFor)
        {

            string sNS = "*";

            try
            {
                ReadXML(xmlPath, sNS); // read once
            }
            catch (System.Exception ex)
            {
                return "<!-- Error: " + ex.Message + " -->";
            }
            //if (sX.ItemID=="3.5")
            //{
            //    System.Diagnostics.Debug.Print("!!!"); 
            //}

            try
            {
                StringBuilder sb = new StringBuilder();
                XmlPlusItem xpi = FindNSPath(sX);

                if (xpi != null)
                {
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
                        if (OpenVariable[0] != null)
                        {
                            if (DebugPrint) sb.AppendLine("<!-- Close prev var before FOR -->");
                            //if (OpenVariable[0].DotAfter) sb.Append(". ");
                            sb.AppendLine("</xsl:variable>");
                            //if (OpenVariable[0].Capitalize)
                            //{
                            //    sb.AppendLine(@"<xsl:call-template name=""string-capltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                            //}
                            //else
                            //{
                            sb.AppendLine(@"<xsl:call-template name=""string-ltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                            //}
                            OpenVariable[0] = null;
                        }

                    }
                    else if (sX.IsTOC())
                    {
                        if (OpenVariable[0] != null)
                        {
                            if (DebugPrint) sb.AppendLine("<!-- Close prev var TOC  -->");
                            sb.AppendLine("</xsl:variable>");
                            //if (OpenVariable[0].Capitalize)
                            //{
                            //    sb.AppendLine(@"<xsl:call-template name=""string-capltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                            //}
                            //else
                            //{
                            sb.AppendLine(@"<xsl:call-template name=""string-ltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                            //}
                            OpenVariable[0] = null;
                        }


                    }
                    else if (sX.IsHeader())
                    {

                        if (OpenVariable[0] != null)
                        {
                            if (DebugPrint) sb.AppendLine("<!-- Close prev var  Hdr -->");
                            //if (OpenVariable[0].DotAfter) sb.Append(". ");
                            sb.AppendLine("</xsl:variable>");
                            //if (OpenVariable[0].Capitalize)
                            //{
                            //    sb.AppendLine(@"<xsl:call-template name=""string-capltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                            //}
                            //else
                            //{
                            sb.AppendLine(@"<xsl:call-template name=""string-ltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                            //}
                            OpenVariable[0] = null;
                        }

                    }





                    // условие  на вывод  блока  
                    if (sX.IsBoolean())
                    {
                        sb.AppendLine(@"<xsl:if ");
                        sb.AppendLine(@" test  =""" + CutFor(sX, xpi.PathNs, excludeFor) + @" = 'true' "" ");
                        sb.Append(">");
                    }
                    else
                    {
                        List<XmlPlusItem> ChildList = null;
                        ChildList = FindChildList(sX);
                        string sTest;

                        if (sX.IsSinglePeriod())
                        {
                            /* string realpath = xpi.PathNs;
                            realpath = realpath.Replace("d_value", "?_value");
                            realpath = realpath.Replace("mo_value", "?_value");
                            realpath = realpath.Replace("a_value", "?_value");
                            realpath = realpath.Replace("wk_value", "?_value");
                            realpath = realpath.Replace("yr_value", "?_value");
                            realpath = realpath.Replace("h_value", "?_value");
*/
                            string realpath = SinglePeriodPathPatch(xpi.PathNs);

                            sTest = CutFor(sX, realpath, excludeFor) + @" != '' ";
                            //sTest = CutFor(sX, realpath.Replace("?_value", "d_value"), excludeFor) + @" != '' ";
                            //sTest += "\r\n or " + CutFor(sX, realpath.Replace("?_value", "wk_value"), excludeFor) + @" != '' ";
                            //sTest += "\r\n or " + CutFor(sX, realpath.Replace("?_value", "mo_value"), excludeFor) + @" != '' ";
                            //sTest += "\r\n or " + CutFor(sX, realpath.Replace("?_value", "a_value"), excludeFor) + @" != '' ";
                            //sTest += "\r\n or " + CutFor(sX, realpath.Replace("?_value", "yr_value"), excludeFor) + @" != '' ";
                            //sTest += "\r\n or " + CutFor(sX, realpath.Replace("?_value", "h_value"), excludeFor) + @" != '' ";
                        }
                        else if (sX.IsSize())
                        {
                            string realpath = xpi.PathNs;
                            realpath = realpath.Replace("mm_value", "?_value");
                            realpath = realpath.Replace("cm_value", "?_value");


                            sTest = CutFor(sX, realpath.Replace("?_value", "mm_value"), excludeFor) + @" != '' ";
                            sTest += "\r\n or " + CutFor(sX, realpath.Replace("?_value", "cm_alue"), excludeFor) + @" != '' ";

                        }else

                        if (xpi.PathNs != "")
                        {
                            sTest = CutFor(sX, xpi.PathNs, excludeFor) + @" != '' ";
                        }
                        else
                        {
                            sTest = " '1' = '0' ";
                        }
                        foreach (XmlPlusItem child in ChildList)
                        {
                            if (child.PathNs != "")
                            {
                                    sTest = sTest + "\r\n or " + CutFor(sX, SinglePeriodPathPatch(child.PathNs), excludeFor) + @" != '' ";
                            }
                        }
                        sb.AppendLine(@"<xsl:if ");



                        sb.AppendLine(@" test  =""" + sTest + @""" ");



                        sb.Append(">");
                    }

                    if (sX.IsMulty() == false)
                    {
                        if (sX.IsNewLine())
                        {
                            if (DebugPrint) sb.Append("<!-- new line -->");
                            sb.Append("<br/>");
                        }


                        // вывод заголока
                        if (sX.WithHeader())
                        {
                            if (sX.IsTOC())
                            {
                                // началась  левая колонка
                                sb.AppendLine(TocStart(sX.Caption));

                                // все до первого заголовка  забираем в переменную чтобы убрать  запятые и капитализировать ???
                                sb.AppendLine("<xsl:variable name='content" + sX.ItemID.Replace(".", "_") + "' >");
                                OpenVariable[0] = sX;


                            }
                            else if (sX.IsHeader())
                            {

                                //sb.Append("<strong>" + sX.Caption + ": </strong>");
                                sb.Append(HeaderStart(sX.Caption, sX));

                                sb.AppendLine("<xsl:variable name='content" + sX.ItemID.Replace(".", "_") + "' >");
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
                            // no body output, only header ???
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


                            //string realpath = xpi.PathNs;
                            //realpath = realpath.Replace("d_value", "?_value");
                            //realpath = realpath.Replace("mo_value", "?_value");
                            //realpath = realpath.Replace("a_value", "?_value");
                            //realpath = realpath.Replace("wk_value", "?_value");
                            //realpath = realpath.Replace("yr_value", "?_value");
                            //realpath = realpath.Replace("h_value", "?_value");

                            //string tmp;

                            //tmp = @"<xsl:call-template name=""SinglePeriodFormat"">";
                            //tmp += @"<xsl:with-param name=""v"" select=""" + CutFor(sX, realpath, excludeFor) + @"""/>";
                            //tmp += @"<xsl:with-param name=""pString"" select=""" + CutFor(sX, realpath, excludeFor).Replace(":magnitude", ":units") + @"""/>";
                            //tmp += @"</xsl:call-template>";

                            //sb.AppendLine(tmp.Replace("?_value", "d_value"));
                            //sb.AppendLine(tmp.Replace("?_value", "wk_value"));
                            //sb.AppendLine(tmp.Replace("?_value", "mo_value"));
                            //sb.AppendLine(tmp.Replace("?_value", "a_value"));
                            //sb.AppendLine(tmp.Replace("?_value", "yr_value"));
                            //sb.AppendLine(tmp.Replace("?_value", "h_value"));

                        }
                        else if (sX.IsSize())
                        {
                            string realpath = xpi.PathNs;
                            realpath = realpath.Replace("mm_value", "?_value");
                            realpath = realpath.Replace("cm_value", "?_value");

                            string tmp;

                            tmp = @"<xsl:call-template name=""SinglePeriodFormat"">";
                            tmp += @"<xsl:with-param name=""v"" select=""" + CutFor(sX, realpath, excludeFor) + @"""/>";
                            tmp += @"<xsl:with-param name=""pString"" select=""" + CutFor(sX, realpath, excludeFor).Replace(":magnitude", ":units") + @"""/>";
                            tmp += @"</xsl:call-template>";

                            sb.AppendLine(tmp.Replace("?_value", "mm_value"));
                            sb.AppendLine(tmp.Replace("?_value", "cm_value"));


                        }

                        else if (sX.IsQuantity())
                        {

                            if (sX.IsDecimal())
                            {
                                sb.AppendLine(@"<xsl:call-template name=""showDecimal"">");
                                sb.AppendLine(@"<xsl:with-param name=""string"" select=""" + CutFor(sX, xpi.PathNs, excludeFor) + @"""/></xsl:call-template>");
                            }
                            else
                            {
                                sb.AppendLine(@"<xsl:call-template name=""PostProcess"">");
                                sb.AppendLine(@"<xsl:with-param name=""size"" select=""0"" />");
                                sb.AppendLine(@"<xsl:with-param name=""string"" select=""" + CutFor(sX, xpi.PathNs, excludeFor) + @"""/></xsl:call-template>");
                            }

                            sb.AppendLine(@"<xsl:if test=""" + CutFor(sX, xpi.PathNs, excludeFor).Replace(":magnitude", ":units") + @" !='' "" >");
                            sb.AppendLine(@"<span> </span>");
                            sb.AppendLine(@"<xsl:call-template name=""edizm"">");
                            sb.AppendLine(@"<xsl:with-param name=""val"" select=""" + CutFor(sX, xpi.PathNs, excludeFor).Replace(":magnitude", ":units") + @"""/>");
                            sb.Append(@"</xsl:call-template>");
                            sb.Append(@"</xsl:if>");
                        }
                        else if (sX.IsDate())
                        {
                            sb.AppendLine(@" <xsl:call-template name=""DateFormat"">");
                            sb.AppendLine(@"<xsl:with-param name=""dateString"" select=""" + CutFor(sX, xpi.PathNs, excludeFor) + @"""/>");
                            sb.Append(@"</xsl:call-template>");
                        }
                        else if (sX.IsPeriod())
                        {
                            sb.AppendLine(@"<xsl:call-template name=""PeriodFormatIN"">");
                            sb.AppendLine(@"<xsl:with-param name=""pString"" select=""" + CutFor(sX, xpi.PathNs, excludeFor) + @"""/>");
                            sb.Append(@"</xsl:call-template>");
                        }
                        else if (sX.IsBoolean())
                        {
                            sb.Append("<span>" + sX.Caption.ToLower() + "</span>");
                        }
                        else
                        {
                            sb.AppendLine(@"<xsl:call-template name=""PostProcess"">");
                            sb.AppendLine(@"<xsl:with-param name=""size"" select=""0"" />");
                            sb.AppendLine(@"<xsl:with-param name=""string"" select=""" + CutFor(sX, xpi.PathNs, excludeFor) + @"""/>");
                            sb.Append(@"</xsl:call-template>");
                        }

                        // process children from specification
                        if (sX.Children.Count > 0)
                        {
                            SpecPro sp2 = Processor();
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
                        if (sX.WithHeader())
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
                            sb.AppendLine(@"<xsl:text> [ОШИБКА: ПУСТОЙ ПУТЬ ДЛЯ  ЦИКЛА, ID:" + sX.ItemID + "] </xsl:text>");
                        }
                        sb.AppendLine(@"<xsl:for-each select=""" + SelPath + @""">");
                        if (sX.IsNewLine())
                        {
                            sb.AppendLine(@"<xsl:if test  =""position()>0 ""><br/></xsl:if>");
                        }

                        sb.AppendLine(@"<xsl:if test  =""position() >1  ""><span>, </span></xsl:if>");


                        if (sX.Children.Count > 0)
                        {

                            // содержимое цикла забираем в переменную чтобы убрать запятые и капитализировать ???
                            sb.AppendLine("<xsl:variable name='content" + sX.ItemID.Replace(".", "_") + "' >");
                            OpenVariable[0] = sX;

                            SpecPro sp2 = Processor();
                            foreach (var sX2 in sX.Children)
                            {
                                sb.Append(sp2.Process(xmlPath, sX2, true));
                            }

                            // закрываем переменную, чтобы не  пересекать границы  цикла
                            if (OpenVariable[0] != null)
                            {
                                if (DebugPrint) sb.AppendLine("<!-- END of TOC  -->");
                                //if (sX.DotAfter) sb.Append(". "); // точку ? это  конец секции ??
                                sb.AppendLine("</xsl:variable>");
                                //if (OpenVariable[0].Capitalize)
                                //{
                                //    sb.AppendLine(@"<xsl:call-template name=""string-capltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                                //}
                                //else
                                //{
                                sb.AppendLine(@"<xsl:call-template name=""string-ltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                                //}
                                OpenVariable[0] = null;
                            }


                        }
                        else
                        {
                            sb.AppendLine(@"<xsl:value-of select ="".""/>");
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
                            //if (OpenVariable[0].Capitalize)
                            //{
                            //    sb.AppendLine(@"<xsl:call-template name=""string-capltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                            //}
                            //else
                            //{
                            sb.AppendLine(@"<xsl:call-template name=""string-ltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                            //}
                            OpenVariable[0] = null;
                        }

                        sb.AppendLine(TocEnd(sX));
                        if (DebugPrint) sb.AppendLine("<!-- END of TOC  -->");
                    }
                    else if (sX.IsHeader())
                    {
                        //if (sX.DotAfter) sb.Append(". "); // точку ? это  конец секции ??
                        sb.Append(HeaderEnd(sX.Caption, sX));
                        if (DebugPrint) sb.AppendLine("<!-- END of Header  -->");
                        if (OpenVariable[0] != null)
                        {
                            sb.AppendLine("</xsl:variable>");

                            sb.AppendLine(@"<xsl:call-template name=""string-ltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                            OpenVariable[0] = null;
                        }


                    }
                    else
                    {
                        sb.Append(ItemEnd(sX));

                    }



                    sb.Append(@"</xsl:if>");
                    if (DebugPrint) sb.AppendLine(@"<!-- End of #" + sX.ItemID + " -->");

                }
                else
                {
                    if (DebugPrint) sb.AppendLine("<!-- " + sX.ToString(false) + " -->");
                    if (DebugPrint) sb.AppendLine("<!-- Error: Данные не найдены в XML файле -->");
                    sb.AppendLine("<xsl:text> [ОШИБКА: Нет данных для поля (" + sX.Caption + "), ID:" + sX.ItemID + "] </xsl:text>");
                    Errors.AppendLine(sX.ToString(false));
                }


                return sb.ToString();
            }
            catch (System.Exception ex)
            {
                // Errors.AppendLine(ex.Message);
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
            if (sX.DotAfter) sOut += ". ";
            sOut += @"</td></tr>";
            return sOut;

        }

        protected virtual string HeaderEnd(string Caption, XsltItem sX)
        {
            string sOut = @"";
            if (sX.DotAfter) sOut += ". ";
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
                    sOut += @"<span> " + Caption + ": </span>";
                }

            return sOut;

        }

        protected virtual string ItemStart(string Caption, XsltItem sX)
        {
            string sOut = "";
            if (sX.ComaBefore) sOut += ", ";
            if (Caption.Trim() != "")
                sOut += @"<span>" + Caption + ": </span>";
            return sOut;
        }

        protected virtual string ItemEnd(XsltItem sX)
        {
            string sOut = @"";
            if (sX.DotAfter) sOut += ". ";
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
