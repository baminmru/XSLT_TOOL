using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace xNS
{
    class SpecPro
    {
        private string[] Ignore;
        private string[] Find;
        private string[] Tails;
        private static XsltItem[] OpenVariable =  new XsltItem[10];
        
        public static Boolean DebugPrint
        {
            get; set;
        }

        public Boolean SmartPath
        {
            get;set;
        }
        private string SmartString(string s)
        {
            string tmp;
            tmp = s.ToLower();
            tmp = tmp.Replace("_openbrkt_", "__");
            tmp = tmp.Replace("-", "_");
            tmp = tmp.Replace("_closebrkt_", "__");
            tmp = tmp.Replace("_comma_", "__");
            tmp = tmp.Replace("_prd_", "__");
            tmp = tmp.Replace("_fslash_", "__");
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

        private  Boolean Finder(string Path)
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
                }
                if (idxfound < i)
                {
                    return false;
                }


            }
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


                                        if (CurTail[t].ToLower() == tmp)
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
                if(xpi !=null)
                    childList.Add(xpi);
               
             }
             return childList;
        }

        private XmlPlusItem FindNSPath(XsltItem sX)
        {

            string sIgnore = "mappings;links;language;encoding;provider;subject;other_participations;context;setting;uid;composer";
            string sTail = "value/value;value/rm:value;value/rm:defining_code/rm:code_string;lower/magnitude;upper/magnitude;value/magnitude;d_value/magnitude;d_value/units";
            string sFind = sX.Path;


            if (sX.IsSinglePeriod())
            {
                sTail = "value/magnitude;d_value/magnitude;a_value/magnitude;wk_value/magnitude;mo_value/magnitude";
            }

            if (sX.IsQuantity())
            {
                sTail = "value/magnitude";
            }


            SmartPath = true;

            if (sIgnore != "")
            {
                Ignore = sIgnore.Split(';');
            }
            else
            {
                Ignore = null;
            }

            if (sFind != "")
            {
                Find = sFind.Split('/');
            }
            else
            {
                Find = null;
            }


            if (sX.IsMulty() && sX.Children.Count > 0)
            {
                sTail = "";
            }

            if (sTail != "")
            {
                Tails = sTail.Split(';');
            }
            else
            {
                Tails = null;
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

        private static  XmlDocument xdoc=null;
        private static List<XmlPlusItem>  PathList=null;
        private static Boolean FirstAfterTOC = false;

        private void ReadXML( string xmlPath , string sNS)
        {
            if (SpecPro.xdoc == null)
            {
                SpecPro.xdoc = new XmlDocument();
                SpecPro.xdoc.Load(xmlPath);

                SpecPro.PathList = XmlTools.IterateThroughAllNodes(SpecPro.xdoc, sNS);
            }

        }



        public string Process(string xmlPath, XsltItem sX, Boolean excludeFor)
        {

            string sNS = "*";

            //if(sX.ItemID == "Up.10.1")
            //{
            //    sNS = "*";
            //}


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
                            if (DebugPrint) sb.AppendLine("<!-- Close prev var  before FOR -->");
                            //if (OpenVariable[0].DotAfter) sb.Append(". ");
                            sb.AppendLine("</xsl:variable>");
                            if (OpenVariable[0].Capitalize)
                            {
                                sb.AppendLine(@"<xsl:call-template name=""string-capltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                            }
                            else
                            {
                                sb.AppendLine(@"<xsl:call-template name=""string-ltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                            }
                            OpenVariable[0] = null;
                        }

                    }
                    else  if (sX.IsTOC())
                        {
                            if (OpenVariable[0] != null)
                            {
                                if (DebugPrint) sb.AppendLine("<!-- Close prev var TOC  -->");
                                sb.AppendLine("</xsl:variable>");
                                if (OpenVariable[0].Capitalize)
                                {
                                    sb.AppendLine(@"<xsl:call-template name=""string-capltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                                }
                                else
                                {
                                    sb.AppendLine(@"<xsl:call-template name=""string-ltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                                }
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
                                if (OpenVariable[0].Capitalize)
                                {
                                    sb.AppendLine(@"<xsl:call-template name=""string-capltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                                }
                                else
                                {
                                    sb.AppendLine(@"<xsl:call-template name=""string-ltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                                }
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
                            string realpath = xpi.PathNs;
                            realpath = realpath.Replace("d_value", "?_value");
                            realpath = realpath.Replace("mo_value", "?_value");
                            realpath = realpath.Replace("a_value", "?_value");
                            realpath = realpath.Replace("wk_value", "?_value");

                            sTest = CutFor(sX, realpath.Replace("?_value","d_alue"), excludeFor) + @" != '' ";
                            sTest += "\r\n or "+ CutFor(sX, realpath.Replace("?_value", "wk_alue"), excludeFor) + @" != '' ";
                            sTest += "\r\n or " + CutFor(sX, realpath.Replace("?_value", "mo_alue"), excludeFor) + @" != '' ";
                            sTest += "\r\n or " + CutFor(sX, realpath.Replace("?_value", "a_alue"), excludeFor) + @" != '' ";
                        }
                        else
                        {

                        }

                        sTest = CutFor(sX, xpi.PathNs, excludeFor) + @" != '' ";
                        foreach (XmlPlusItem child in ChildList)
                        {
                            sTest = sTest + "\r\n or " + CutFor(sX, child.PathNs, excludeFor) + @" != '' ";
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
                                SpecPro.HeaderLevel++;
                                sb.Append("<strong>" + sX.Caption + ": </strong>");
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
                                    sb.Append("<span>");
                                    if (sX.ComaBefore) sb.Append(", ");
                                    if (sX.Capitalize)
                                    {
                                        sb.Append(sX.Caption + ": </span>");
                                    }
                                    else
                                    {
                                        sb.Append(sX.Caption.ToLower() + ": </span>");
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
                            string realpath = xpi.PathNs;
                            realpath = realpath.Replace("d_value", "?_value");
                            realpath = realpath.Replace("mo_value", "?_value");
                            realpath = realpath.Replace("a_value", "?_value");
                            realpath = realpath.Replace("wk_value", "?_value");

                            string tmp;

                            tmp=@"<xsl:call-template name=""SinglePeriodFormat"">";
                            tmp+=@"<xsl:with-param name=""v"" select=""" + CutFor(sX, realpath, excludeFor) + @"""/>";
                            tmp += @"<xsl:with-param name=""pString"" select=""" + CutFor(sX, realpath, excludeFor).Replace(":magnitude", ":units") + @"""/>";
                            tmp += @"</xsl:call-template>";

                            sb.AppendLine(tmp.Replace("?_value", "d_value"));
                            sb.AppendLine(tmp.Replace("?_value", "wk_value"));
                            sb.AppendLine(tmp.Replace("?_value", "mo_value"));
                            sb.AppendLine(tmp.Replace("?_value", "a_value"));
                            
                        }
                        else if (sX.IsQuantity())
                        {
                            sb.AppendLine(@"<xsl:call-template name=""PostProcess"">");
                            sb.AppendLine(@"<xsl:with-param name=""size"" select=""0"" />");
                            sb.AppendLine(@"<xsl:with-param name=""string"" select=""" + CutFor(sX, xpi.PathNs, excludeFor) + @"""/></xsl:call-template>");
                            sb.AppendLine(@"<span> </span>");
                            sb.AppendLine(@"<xsl:call-template name=""edizm"">");
                            sb.AppendLine(@"<xsl:with-param name=""val"" select=""" + CutFor(sX, xpi.PathNs, excludeFor).Replace(":magnitude", ":units") + @"""/>");
                            sb.Append(@"</xsl:call-template>");
                        }
                        else if (sX.IsDate())
                        {
                            sb.AppendLine(@"<xsl:call-template name=""DateFormat"">");
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
                            SpecPro sp2 = new SpecPro();
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
                                sb.Append("<strong>" + sX.Caption + ": </strong>");
                               
                            }
                            else
                            {
                                if (sX.IsBoolean())
                                {
                                    if(sX.ComaBefore ) sb.Append("<span>, </span>");
                                }
                                else
                                {
                                    sb.Append("<span>");
                                    if (sX.ComaBefore) sb.Append(", ");
                                    sb.Append( sX.Caption.ToLower() + ": </span>");

                                }
                            }
                        }
                        //else
                        //{
                        //    if (sX.Children.Count == 0)
                        //    {
                        //        string sComa = CommaIf(sX, false);
                        //        if (sComa != "") sb.Append(sComa);
                        //    }


                        //}
                        sX.xslFor = CutFor(sX, xpi.PathNs, excludeFor);
                        sb.AppendLine(@"<xsl:for-each select=""" + CutFor(sX, xpi.PathNs, excludeFor) + @""">");
                        if (sX.IsNewLine())
                        {
                            sb.AppendLine(@"<xsl:if test  =""position()>0 ""><br/></xsl:if>");
                        }

                        sb.AppendLine(@"<xsl:if test  =""position() >1  ""><span>, </span></xsl:if>");

                        //if (sX.Children.Count > 0)
                        //{
                        //    sb.Append(@"<xsl:if test  =""position()>1 ""><span>, </span></xsl:if>");
                        //    //sb.AppendLine(@"<xsl:value-of select ="".""/>");
                        //}

                            // содержимое цикла забираем в переменную чтобы убрать запятые и капитализировать ???
                        sb.AppendLine("<xsl:variable name='content" + sX.ItemID.Replace(".", "_") + "' >");
                        OpenVariable[0] = sX;


                        if (sX.Children.Count > 0)
                        {
                            SpecPro sp2 = new SpecPro();
                            foreach (var sX2 in sX.Children)
                            {
                                sb.Append(sp2.Process(xmlPath, sX2, true));
                            }

                        }
                        else
                        {
                            
                            sb.AppendLine(@"<xsl:value-of select ="".""/>");
                        }






                        // закрываем переменную, чтобы не  пересекать границы  цикла
                        if (OpenVariable[0] != null)
                        {
                            if (DebugPrint) sb.AppendLine("<!-- END of TOC  -->");
                            //if (sX.DotAfter) sb.Append(". "); // точку ? это  конец секции ??
                            sb.AppendLine("</xsl:variable>");
                            if (OpenVariable[0].Capitalize)
                            {
                                sb.AppendLine(@"<xsl:call-template name=""string-capltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                            }
                            else
                            {
                                sb.AppendLine(@"<xsl:call-template name=""string-ltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                            }
                            OpenVariable[0] = null;
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
                                if (OpenVariable[0].Capitalize)
                                {
                                    sb.AppendLine(@"<xsl:call-template name=""string-capltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                                }
                                else
                                {
                                    sb.AppendLine(@"<xsl:call-template name=""string-ltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                                }
                                OpenVariable[0] = null;
                            }
                        if (sX.DotAfter) sb.Append(". ");
                            sb.AppendLine(TocEnd());
                            if (DebugPrint) sb.AppendLine("<!-- END of TOC  -->");
                    }
                        else if (sX.IsHeader())
                        {
                           
                            if (DebugPrint) sb.AppendLine("<!-- END of Header  -->");
                            if (sX.DotAfter) sb.Append(". "); // точку ? это  конец секции ??

                            if (OpenVariable[0] != null)
                            {
                                sb.AppendLine("</xsl:variable>");
                                if (OpenVariable[0].Capitalize)
                                {
                                    sb.AppendLine(@"<xsl:call-template name=""string-capltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                                }
                                else
                                {
                                    sb.AppendLine(@"<xsl:call-template name=""string-ltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[0].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                                }
                                OpenVariable[0] = null;
                            }

                        
                        }
                        else
                        {
                            if (sX.DotAfter) sb.Append(". ");
                        }
                    

                    
                    sb.Append(@"</xsl:if>");
                    if (DebugPrint) sb.AppendLine(@"<!-- End of #" + sX.ItemID + " -->");

                }
                else
                {
                    if (DebugPrint) sb.AppendLine("<!-- " + sX.ToString(false) + " -->");
                    if (DebugPrint) sb.AppendLine("<!-- Error: Данные не найдены в XML файле -->");
                }


                return sb.ToString();
            }
            catch (System.Exception ex)
            {
                return "<!-- Error: " + ex.Message + " -->";
            }


        }


        public string Process_OLD(string xmlPath, XsltItem sX, Boolean excludeFor)
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

            try
            {
                StringBuilder sb = new StringBuilder();
                XmlPlusItem xpi = FindNSPath(sX);

                if (xpi !=null)
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
                    if (sX.WithHeader())
                    {
                        if (sX.IsTOC())
                        {
                            if (OpenVariable[sX.Level()] != null)
                            {
                                if (DebugPrint) sb.AppendLine("<!-- Close TOC var on same level -->");
                                sb.AppendLine("</xsl:variable>");
                                sb.AppendLine(@"<xsl:call-template name=""string-ltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[sX.Level()].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                                OpenVariable[sX.Level()] = null;
                            }


                        }
                        else if (sX.IsHeader())
                        {

                            if (OpenVariable[sX.Level()] != null)
                            {
                                if (DebugPrint) sb.AppendLine("<!-- Close prev var on same level -->");
                                sb.AppendLine(". </xsl:variable>");
                                sb.AppendLine(@"<xsl:call-template name=""string-ltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[sX.Level()].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                               
                            }
                            if ( OpenVariable[sX.Level() - 1] != null)
                            {
                                if(OpenVariable[sX.Level()] == null)
                                {
                                    sb.Append(". ");
                                }
                                if (DebugPrint) sb.AppendLine("<!-- close prev var on prev level -->");
                                sb.AppendLine("</xsl:variable>");
                                sb.AppendLine(@"<xsl:call-template name=""string-ltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[sX.Level() - 1].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                                OpenVariable[sX.Level() - 1] = null;
                            }
                            OpenVariable[sX.Level()] = null;
                        }
                    }

                    // условие  на вывод  блока  
                    if (sX.IsBoolean())
                        sb.AppendLine(@"<xsl:if test  =""" + CutFor(sX, xpi.PathNs, excludeFor) + @" = 'true' "">");
                    else
                    {
                        List<XmlPlusItem> ChildList = null;
                        ChildList = FindChildList(sX);
                        string sTest;
                        sTest = CutFor(sX, xpi.PathNs, excludeFor) + @" != '' ";
                        foreach (XmlPlusItem child in ChildList)
                        {
                            sTest = sTest + "\r\n or " + CutFor(sX, child.PathNs, excludeFor) + @" != '' ";
                        }

                        sb.AppendLine(@"<xsl:if test  =""" + sTest + @" "">");
                    }

                    if (sX.IsMulty()==false)
                    {
                        if (sX.IsNewLine())
                        {
                            if(DebugPrint) sb.AppendLine("<!-- new line -->");
                            sb.AppendLine("<br/>");
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
                                OpenVariable[sX.Level()] = sX;
                                SpecPro.HeaderLevel=0;

                            }
                            else if (sX.IsHeader())
                            {
                                SpecPro.HeaderLevel ++;
                                //sb.AppendLine(CommaIf(sX,true));
                                sb.AppendLine("<strong>" + sX.Caption + ": </strong>");
                                sb.AppendLine("<xsl:variable name='content" + sX.ItemID.Replace(".", "_") + "' >");
                                OpenVariable[sX.Level()] = sX;
                            }
                            else
                            {
                                if (sX.IsBoolean())
                                {
                                    string sComa = "<span>, </span>"; // CommaIf(sX, false);
                                    if(sComa !="")sb.Append(sComa);
                                }
                                else
                                {
                                    string sComa = CommaIf(sX, false);
                                    if (sComa != "") sb.Append(sComa);
                                    sb.AppendLine("<span>" + sX.Caption.ToLower() + ": </span>");
             
                                }
                            }
                        }
                        else
                        {
                            string sComa = CommaIf(sX, false);
                            if (sComa != "") sb.Append(sComa);

                        }

                        // собственно вывод данных                                        
                        if (sX.IsTOC()  || sX.IsHeader())
                        {
                            // no body output, only header ???


                        }
                        else if (sX.IsSinglePeriod())
                        {
                            string realpath = xpi.PathNs;
                            realpath = realpath.Replace("d_value", "?_value");
                            realpath = realpath.Replace("mo_value", "?_value");
                            realpath = realpath.Replace("a_value", "?_value");
                            realpath = realpath.Replace("wk_value", "?_value");

                            string tmp;

                            tmp = @"<xsl:call-template name=""SinglePeriodFormat"">";
                            tmp += @"<xsl:with-param name=""v"" select=""" + CutFor(sX, realpath, excludeFor) + @"""/>";
                            tmp += @"<xsl:with-param name=""pString"" select=""" + CutFor(sX, realpath, excludeFor).Replace(":magnitude", ":units") + @"""/>";
                            tmp += @"</xsl:call-template>";

                            sb.AppendLine(tmp.Replace("?_value", "d_value"));
                            sb.AppendLine(tmp.Replace("?_value", "wk_value"));
                            sb.AppendLine(tmp.Replace("?_value", "mo_value"));
                            sb.AppendLine(tmp.Replace("?_value", "a_value"));
                        }
                        else if (sX.IsQuantity())
                        {
                            sb.AppendLine(@"<xsl:call-template name=""PostProcess"">");
                            sb.AppendLine(@"<xsl:with-param name=""size"" select=""0"" />");
                            sb.AppendLine(@"<xsl:with-param name=""string"" select=""" + CutFor(sX, xpi.PathNs, excludeFor) + @"""/></xsl:call-template>");
                            sb.AppendLine(@"<span> </span>");
                            sb.AppendLine(@"<xsl:call-template name=""edizm"">");
                            sb.AppendLine(@"<xsl:with-param name=""val"" select=""" + CutFor(sX, xpi.PathNs, excludeFor).Replace(":magnitude", ":units") + @"""/>");
                            sb.Append(@"</xsl:call-template>");
                        }
                        else if (sX.IsDate())
                        {
                            sb.AppendLine(@"<xsl:call-template name=""DateFormat"">");
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
                            sb.AppendLine("<span>" + sX.Caption.ToLower() + "</span>");
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
                            SpecPro sp2 = new SpecPro();
                            foreach (var sX2 in sX.Children)
                            {
                                sb.AppendLine(sp2.Process_OLD(xmlPath, sX2,excludeFor));
                            }
                        }
                        else
                        {
                            //  если нет  дочерних ...
                            // проверим не поставить ли точку
                            string sDot = DotIf(sX);
                            if (sDot != "")
                                sb.Append(sDot);
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
                                sb.AppendLine(TocStart(sX.Caption));

                                // все до первого заголовка  забираем в переменную чтобы убрать  запятые и капитализировать ???
                                sb.AppendLine("<xsl:variable name='content" + sX.ItemID.Replace(".", "_") + "' >");
                                OpenVariable[sX.Level()] = sX;

                            }
                            else if (sX.IsHeader())
                            {
                                //sb.AppendLine(CommaIf(sX, true));
                                sb.AppendLine("<strong>" + sX.Caption + ": </strong>");
                                sb.AppendLine("<xsl:variable name='content" + sX.ItemID.Replace(".", "_") + "' >");
                                OpenVariable[sX.Level()] = sX;
                            }
                            else
                            {
                                if (sX.IsBoolean())
                                {
                                    string sComa = CommaIf(sX, false);
                                    if (sComa != "") sb.Append(sComa);
                                }
                                else
                                {
                                    string sComa = CommaIf(sX, false);
                                    if (sComa != "") sb.Append(sComa);
                                    sb.AppendLine("<span>" + sX.Caption.ToLower() + ": </span>");

                                }
                            }
                        }
                        else
                        {
                            if (sX.Children.Count == 0)
                            {
                                string sComa = CommaIf(sX, false);
                                if (sComa != "") sb.Append(sComa);
                            }
                                

                        }
                        sX.xslFor = CutFor(sX, xpi.PathNs, excludeFor);
                        sb.AppendLine(@"<xsl:for-each select=""" + CutFor(sX, xpi.PathNs, excludeFor) + @""">");
                        if (sX.IsNewLine()) {
                            sb.AppendLine(@"<xsl:if test  =""position()>0 ""><br/></xsl:if>");
                        }

                        

                        if (sX.Children.Count > 0)
                        {
                            //sb.AppendLine(@"<xsl:if test  =""position()>1 ""><span>, </span></xsl:if>");
                            //sb.AppendLine(@"<xsl:value-of select ="".""/>");

                            SpecPro sp2 = new SpecPro();
                            foreach (var sX2 in sX.Children)
                            {
                                sb.AppendLine(sp2.Process_OLD(xmlPath, sX2, true));
                            }

                        }else
                        {
                            sb.AppendLine(@"<xsl:if test  =""position()>1 ""><span>, </span></xsl:if>");
                            sb.AppendLine(@"<xsl:value-of select ="".""/>");
                        }

                        sb.AppendLine(@"</xsl:for-each>");
                    }

                    // был заголовок -  надо закрыть переменную
                    if (sX.WithHeader())
                    {

                        if (sX.IsTOC())
                        {
                            if (OpenVariable[sX.Level()] != null)
                            {
                                if (DebugPrint) sb.AppendLine("<!-- END of TOC  -->");
                                sb.AppendLine(". </xsl:variable>"); // точку ? это  конец секции ??
                                sb.AppendLine(@"<xsl:call-template name=""string-ltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[sX.Level()].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                                OpenVariable[sX.Level()] = null;
                            }

                            // закрываем строку
                            sb.AppendLine(TocEnd());
                        }
                        else if (sX.IsHeader())
                        {
                            if (OpenVariable[sX.Level()] != null)
                            {
                                if (DebugPrint)  sb.AppendLine("<!-- END OF HEADER -->");
                                sb.AppendLine(". </xsl:variable>"); // точка после блока с заголовком
                                sb.AppendLine(@"<xsl:call-template name=""string-ltrim_br""><xsl:with-param name = ""string"" select = '$content" + OpenVariable[sX.Level()].ItemID.Replace(".", "_") + "' /></xsl:call-template>");
                                OpenVariable[sX.Level()] = null;
                            }

                            SpecPro.HeaderLevel --;
                        }
                    }

                    sb.Append(@"</xsl:if>");
                    if (DebugPrint) sb.AppendLine(@"<!-- End of #" + sX.ItemID + " -->");

                }
                else
                {
                    if (DebugPrint) sb.AppendLine("<!-- " + sX.ToString(false) + " -->");
                    if (DebugPrint) sb.AppendLine("<!-- Error: Данные не найдены в XML файле -->");
                }
                        
                    
                return sb.ToString();
            }
            catch (System.Exception ex)
            {
                return "<!-- Error: " + ex.Message +" -->";
            }

           
        }

        private string DotIf(XsltItem sX)
        {
            if (sX.IsTOC() || sX.IsHeader())
                return "";


            if (sX.WithHeader() == false)
            {
                
                
                //  Обычно комментарий  последний вне именованного блока
                //if (sX.Children.Count == 0 && OpenVariable[sX.Level()] == null && OpenVariable[sX.Level() - 1] == null)
                //    return "<span>. </span>";

                if (sX.IsHeader() == false && sX.PrevSibling() != null && sX.PrevSibling().IsHeader() && sX.NextSibling() == null )
                    return "<span>. </span><!-- dot 1 -->";

                //if (sX.IsHeader()==false && sX.NextSibling() != null && sX.NextSibling().IsHeader())
                //    return "<span>. </span><!-- dot 2 -->";

                //if (sX.IsHeader() == false && sX.NextSibling() == null )
                //    return "<span>. </span><!-- dot 3 -->";
            }

            //// последний блок
            //if (sX.NextSibling() == null && sX.Parent != null && sX.Parent.NextSibling() == null)
            //    return "<span>. </span>";

            //// последний на  корневом уровне
            //if (sX.NextSibling() == null && sX.Parent == null)
            //    return "<span>. </span>";

            return "";
        }

        private string CommaIf(XsltItem sX, Boolean Header)
        {
            if (sX.WithHeader() == false) {
                if (sX.IsTOC() ||  sX.IsHeader())
                    return "";

                //if (SpecPro.HeaderLevel == 0)
                //{
                //    return "<span> </span>";
                //}
                ////  Первый блок после точки ??? Обычно комментарий без заголовка
                //if (OpenVariable[sX.Level()] == null && OpenVariable[sX.Level() - 1] == null)
                //    return "";
                if (sX.PrevSibling() != null && sX.PrevSibling().IsHeader())
                    return "";


            }
            if (sX.IsNewLine()) return "";

            //if (SpecPro.HeaderLevel == 0)
            //{
            //    return "<span> </span>";
            //}

            if (sX.PrevSibling() != null || (sX.Parent != null && sX.Parent.IsTOC() == false && sX.Parent.PrevSibling() != null))
            {
                if (Header)
                    return "<span>. </span>";
                else
                    return "<span>, </span>";
            }

            return "<span>, </span>";
        }

        private string TocStart( string Caption)
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
						                <span class=""part"">" + Caption +@"</span>
					                </th>
				                </tr>
			                </thead>
		                </table>
	                </td>
	                <td class=""mytd"" valign=""top"" align=""left"">";
            return sOut;

        }


        private string TocEnd()
        {
            string sOut = @"</td></tr>";
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
