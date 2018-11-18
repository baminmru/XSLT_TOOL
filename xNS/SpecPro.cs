using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace xNS
{
    internal class SpecPro
    {
        private static readonly XsltItem[] OpenVariable = new XsltItem[10];

        private static XmlDocument _xdoc;
        private static List<XmlPlusItem> _pathList;

        private static StringBuilder _errors;
        private string[] _find;
        private string[] _ignore;
        private string[] _tails;

        public static bool DebugPrint { get; set; }

        public bool SmartPath { get; set; }

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

            var ltmp = tmp.Length + 1;
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

        private bool Finder(string path)
        {
            if (_find == null) return true;
            var test = path.Split('/');
            int i, j, start, idxfound;
            string tmp;

            if (SmartPath)
                for (i = 0; i < _find.Length; i++)
                    _find[i] = SmartString(_find[i]);


            var found = false;
            start = 0;
            idxfound = -1;
            for (i = 0; i < _find.Length; i++)
            {
                for (j = start; j < test.Length; j++)
                {
                    tmp = test[j].ToLower();
                    if (SmartPath) tmp = SmartString(tmp);


                    if (_find[i].ToLower() == tmp)
                    {
                        start = j + 1; // position for next test
                        idxfound = i; // last found index
                        break;
                    }

                    // допускаем только наличие  служебных  компонентов внутри пути 
                    if (tmp != "" && !(tmp.ToLower().StartsWith("любое_событие")
                                       || tmp.ToLower().StartsWith("любые_события")) && tmp != "data")
                        return false;
                }

                if (idxfound < i) return false;
            }

            // все пути нашлись 
            if (idxfound == _find.Length - 1) found = true;

            if (found)
                if (_tails != null)
                {
                    found = false;
                    int t;
                    int tail;
                    string[] CurTail;
                    for (i = 0; i < _tails.Length; i++)
                    {
                        CurTail = _tails[i].Split('/');
                        idxfound = 0;
                        tail = test.Length - 1;
                        // обратный цикл от хвоста
                        if (CurTail != null)
                            if (CurTail.Length > 0)
                            {
                                for (t = CurTail.Length - 1; t >= 0; t--)
                                for (j = tail; j >= 0; j--) // цикл по проверяемому пути в обратную сторону
                                {
                                    tmp = test[j].ToLower();
                                    if (SmartPath) tmp = SmartString(tmp);


                                    if (CurTail[t].ToLower() == tmp)
                                    {
                                        tail = j - 1; // position for next test
                                        idxfound++; // last found index
                                        break;
                                    }
                                }

                                if (idxfound == CurTail.Length)
                                {
                                    found = true;
                                    break;
                                }
                            }
                    }
                }


            return found;
        }

        // find ns-paths for child nodes of current node
        private List<XmlPlusItem> FindChildList(XsltItem sX)
        {
            var childList = new List<XmlPlusItem>();
            foreach (var chX in sX.Children)
            {
                var xpi = FindNSPath(chX);
                if (xpi != null)
                    childList.Add(xpi);
            }

            return childList;
        }

        private XmlPlusItem FindNSPath(XsltItem sX)
        {
            var sIgnore =
                "mappings;links;language;encoding;provider;subject;other_participations;context;setting;uid;composer;protocol";
            var sTail =
                "value/value;value/rm:value;value/rm:defining_code/rm:code_string;lower/magnitude;upper/magnitude;value/magnitude;value/rm:magnitude";
            var sFind = sX.Path;


            if (sX.Path.Trim() == "" || sX.Path.Trim().ToLower().StartsWith("generic"))
            {
                var x = new XmlPlusItem();
                x.Path = "";
                x.PathNs = "";
                x.NodeText = sX.Caption;
                return x;
            }

            if (sX.IsSinglePeriod())
                sTail = "value/magnitude;d_value/magnitude;a_value/magnitude;wk_value/magnitude;mo_value/magnitude";


            if (sX.IsQuantity())
                sTail =
                    "value/magnitude;mm_value/magnitude;cm_value/magnitude;h_value/magnitude;min_value/magnitude;s_value/magnitude";


            SmartPath = true;

            if (sIgnore != "")
                _ignore = sIgnore.Split(';');
            else
                _ignore = null;

            if (sFind != "")
                _find = sFind.Split('/');
            else
                _find = null;


            if (sX.IsMulty() && sX.Children.Count > 0) sTail = "";

            if (sTail != "")
                _tails = sTail.Split(';');
            else
                _tails = null;


            bool printNode;
            int xIdx;
            for (xIdx = 0; xIdx < _pathList.Count; xIdx++)
            {
                var xpi = _pathList[xIdx];

                printNode = false;

                if (sX.IsMulty() == false)
                {
                    if (xpi.Node.Attributes.Count > 0 || xpi.NodeText != null && xpi.NodeText != "") printNode = true;
                }
                else
                {
                    printNode = true;
                }

                if (printNode)
                {
                    var ok = true;
                    if (_ignore != null)
                        foreach (var s in _ignore)
                            if (s.Length > 0)
                                if (xpi.Path.IndexOf(s) >= 0)
                                {
                                    ok = false;
                                    break;
                                }

                    if (ok) ok = Finder(xpi.Path);
                    if (ok) return xpi;
                }
            }

            return null;
        }

        private void ReadXML(string xmlPath, string sNS)
        {
            if (_xdoc == null)
            {
                _xdoc = new XmlDocument();
                _xdoc.Load(xmlPath);

                _pathList = _xdoc.IterateThroughAllNodes(sNS);
            }
        }

        protected virtual SpecPro Processor()
        {
            return new SpecPro();
        }

        public string Error()
        {
            if (_errors == null) return "";
            return _errors.ToString();
        }

        public void Init()
        {
            _errors = new StringBuilder();
        }

        public string Process(string xmlPath, XsltItem sX, bool excludeFor)
        {
            var sNS = "*";

            try
            {
                ReadXML(xmlPath, sNS); // read once
            }
            catch (Exception ex)
            {
                return "<!-- Error: " + ex.Message + " -->";
            }

            try
            {
                var sb = new StringBuilder();
                var xpi = FindNSPath(sX);

                if (xpi != null)
                {
                    if (DebugPrint)
                    {
                        sb.AppendLine("<!-- " + sX.ToString(false) + " -->");
                        sb.AppendLine("<!-- XPath    : " + xpi.PathNs + " -->");
                        if (xpi.NodeText != null && xpi.NodeText != "")
                            sb.AppendLine("<!-- Node text: " + xpi.NodeText + " -->");
                    }

                    // закрытие предыдущих переменных
                    if (sX.IsMulty()) // перед циклом закрываем переменную
                    {
                        if (OpenVariable[0] != null)
                        {
                            if (DebugPrint) sb.AppendLine("<!-- Close prev var before FOR -->");
                            sb.AppendLine("</xsl:variable>");
                            sb.AppendLine(
                                @"<xsl:call-template name=""string-ltrim_br""><xsl:with-param name = ""string"" select = '$content" +
                                OpenVariable[0].ItemId.Replace(".", "_") + "' /></xsl:call-template>");
                            OpenVariable[0] = null;
                        }
                    }
                    else if (sX.IsToc())
                    {
                        if (OpenVariable[0] != null)
                        {
                            if (DebugPrint) sb.AppendLine("<!-- Close prev var TOC  -->");
                            sb.AppendLine("</xsl:variable>");
                            sb.AppendLine(
                                @"<xsl:call-template name=""string-ltrim_br""><xsl:with-param name = ""string"" select = '$content" +
                                OpenVariable[0].ItemId.Replace(".", "_") + "' /></xsl:call-template>");
                            OpenVariable[0] = null;
                        }
                    }
                    else if (sX.IsHeader())
                    {
                        if (OpenVariable[0] != null)
                        {
                            if (DebugPrint) sb.AppendLine("<!-- Close prev var  Hdr -->");
                            sb.AppendLine("</xsl:variable>");
                            sb.AppendLine(
                                @"<xsl:call-template name=""string-ltrim_br""><xsl:with-param name = ""string"" select = '$content" +
                                OpenVariable[0].ItemId.Replace(".", "_") + "' /></xsl:call-template>");
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
                        List<XmlPlusItem> childList = FindChildList(sX);
                        string sTest;

                        if (sX.IsSinglePeriod())
                        {
                            var realpath = xpi.PathNs;
                            realpath = realpath.Replace("d_value", "?_value");
                            realpath = realpath.Replace("mo_value", "?_value");
                            realpath = realpath.Replace("a_value", "?_value");
                            realpath = realpath.Replace("wk_value", "?_value");

                            sTest = CutFor(sX, realpath.Replace("?_value", "d_value"), excludeFor) + @" != '' ";
                            sTest += "\r\n or " + CutFor(sX, realpath.Replace("?_value", "wk_alue"), excludeFor) +
                                     @" != '' ";
                            sTest += "\r\n or " + CutFor(sX, realpath.Replace("?_value", "mo_alue"), excludeFor) +
                                     @" != '' ";
                            sTest += "\r\n or " + CutFor(sX, realpath.Replace("?_value", "a_alue"), excludeFor) +
                                     @" != '' ";
                        }
                        else if (sX.IsSize())
                        {
                            var realpath = xpi.PathNs;
                            realpath = realpath.Replace("mm_value", "?_value");
                            realpath = realpath.Replace("cm_value", "?_value");


                            sTest = CutFor(sX, realpath.Replace("?_value", "mm_value"), excludeFor) + @" != '' ";
                            sTest += "\r\n or " + CutFor(sX, realpath.Replace("?_value", "cm_alue"), excludeFor) +
                                     @" != '' ";
                        }

                        if (xpi.PathNs != "")
                            sTest = CutFor(sX, xpi.PathNs, excludeFor) + @" != '' ";
                        else
                            sTest = " '1' = '0' ";
                        foreach (var child in childList)
                            if (child.PathNs != "")
                                sTest = sTest + "\r\n or " + CutFor(sX, child.PathNs, excludeFor) + @" != '' ";
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
                            if (sX.IsToc())
                            {
                                // началась  левая колонка
                                sb.AppendLine(TocStart(sX.Caption));

                                // все до первого заголовка  забираем в переменную чтобы убрать  запятые и капитализировать ???
                                sb.AppendLine("<xsl:variable name='content" + sX.ItemId.Replace(".", "_") + "' >");
                                OpenVariable[0] = sX;
                            }
                            else if (sX.IsHeader())
                            {
                                sb.Append(HeaderStart(sX.Caption, sX));

                                sb.AppendLine("<xsl:variable name='content" + sX.ItemId.Replace(".", "_") + "' >");
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
                                        sb.Append(ItemStart(sX.Caption, sX));
                                    else
                                        sb.Append(ItemStart(sX.Caption.ToLower(), sX));
                                }
                            }
                        }
                        else
                        {
                            if (sX.ComaBefore) sb.Append(", ");
                        }

                        // собственно вывод данных                                        
                        if (sX.IsToc() || sX.IsHeader())
                        {
                            // no body output, only header ???
                        }
                        else if (sX.IsSinglePeriod())
                        {
                            var realpath = xpi.PathNs;
                            realpath = realpath.Replace("d_value", "?_value");
                            realpath = realpath.Replace("mo_value", "?_value");
                            realpath = realpath.Replace("a_value", "?_value");
                            realpath = realpath.Replace("wk_value", "?_value");

                            string tmp;

                            tmp = @"<xsl:call-template name=""SinglePeriodFormat"">";
                            tmp += @"<xsl:with-param name=""v"" select=""" + CutFor(sX, realpath, excludeFor) + @"""/>";
                            tmp += @"<xsl:with-param name=""pString"" select=""" +
                                   CutFor(sX, realpath, excludeFor).Replace(":magnitude", ":units") + @"""/>";
                            tmp += @"</xsl:call-template>";

                            sb.AppendLine(tmp.Replace("?_value", "d_value"));
                            sb.AppendLine(tmp.Replace("?_value", "wk_value"));
                            sb.AppendLine(tmp.Replace("?_value", "mo_value"));
                            sb.AppendLine(tmp.Replace("?_value", "a_value"));
                        }
                        else if (sX.IsSize())
                        {
                            var realpath = xpi.PathNs;
                            realpath = realpath.Replace("mm_value", "?_value");
                            realpath = realpath.Replace("cm_value", "?_value");

                            string tmp;

                            tmp = @"<xsl:call-template name=""SinglePeriodFormat"">";
                            tmp += @"<xsl:with-param name=""v"" select=""" + CutFor(sX, realpath, excludeFor) + @"""/>";
                            tmp += @"<xsl:with-param name=""pString"" select=""" +
                                   CutFor(sX, realpath, excludeFor).Replace(":magnitude", ":units") + @"""/>";
                            tmp += @"</xsl:call-template>";

                            sb.AppendLine(tmp.Replace("?_value", "mm_value"));
                            sb.AppendLine(tmp.Replace("?_value", "cm_value"));
                        }

                        else if (sX.IsQuantity())
                        {
                            sb.AppendLine(@"<xsl:call-template name=""PostProcess"">");
                            sb.AppendLine(@"<xsl:with-param name=""size"" select=""0"" />");
                            sb.AppendLine(@"<xsl:with-param name=""string"" select=""" +
                                          CutFor(sX, xpi.PathNs, excludeFor) + @"""/></xsl:call-template>");
                            sb.AppendLine(@"<xsl:if test=""" +
                                          CutFor(sX, xpi.PathNs, excludeFor).Replace(":magnitude", ":units") +
                                          @" !='' "" >");
                            sb.AppendLine(@"<span> </span>");
                            sb.AppendLine(@"<xsl:call-template name=""edizm"">");
                            sb.AppendLine(@"<xsl:with-param name=""val"" select=""" +
                                          CutFor(sX, xpi.PathNs, excludeFor).Replace(":magnitude", ":units") + @"""/>");
                            sb.Append(@"</xsl:call-template>");
                            sb.Append(@"</xsl:if>");
                        }
                        else if (sX.IsDate())
                        {
                            sb.AppendLine(@" <xsl:call-template name=""DateFormat"">");
                            sb.AppendLine(@"<xsl:with-param name=""dateString"" select=""" +
                                          CutFor(sX, xpi.PathNs, excludeFor) + @"""/>");
                            sb.Append(@"</xsl:call-template>");
                        }
                        else if (sX.IsPeriod())
                        {
                            sb.AppendLine(@"<xsl:call-template name=""PeriodFormatIN"">");
                            sb.AppendLine(@"<xsl:with-param name=""pString"" select=""" +
                                          CutFor(sX, xpi.PathNs, excludeFor) + @"""/>");
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
                            sb.AppendLine(@"<xsl:with-param name=""string"" select=""" +
                                          CutFor(sX, xpi.PathNs, excludeFor) + @"""/>");
                            sb.Append(@"</xsl:call-template>");
                        }

                        // process children from specification
                        if (sX.Children.Count > 0)
                        {
                            var sp2 = Processor();
                            foreach (var sX2 in sX.Children) sb.Append(sp2.Process(xmlPath, sX2, excludeFor));
                        }
                    } // not multy
                    else
                        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////                                                                       
                    {
                        // multy row

                        // вывод заголовка           
                        if (sX.WithHeader())
                        {
                            if (sX.IsToc())
                            {
                                // началась  левая колонка
                                sb.Append(TocStart(sX.Caption));
                            }
                            else if (sX.IsHeader())
                            {
                                sb.Append(HeaderStart(sX.Caption, sX));
                            }
                            else
                            {
                                if (sX.IsBoolean())
                                    sb.Append(ItemStart("", sX));
                                else
                                    sb.Append(ItemStart(sX.Caption.ToLower(), sX));
                            }
                        }
                        else
                        {
                            if (sX.Children.Count == 0) sb.Append(ItemStart("", sX));
                        }

                        sX.XslFor = CutFor(sX, xpi.PathNs, excludeFor);
                        string SelPath;
                        SelPath = CutFor(sX, xpi.PathNs, excludeFor);
                        if (SelPath == "")
                        {
                            SelPath = "/ERROR";
                            sb.AppendLine(@"<xsl:text> [ОШИБКА: ПУСТОЙ ПУТЬ ДЛЯ  ЦИКЛА, ID:" + sX.ItemId +
                                          "] </xsl:text>");
                        }

                        sb.AppendLine(@"<xsl:for-each select=""" + SelPath + @""">");
                        if (sX.IsNewLine()) sb.AppendLine(@"<xsl:if test  =""position()>0 ""><br/></xsl:if>");

                        sb.AppendLine(@"<xsl:if test  =""position() >1  ""><span>, </span></xsl:if>");


                        if (sX.Children.Count > 0)
                        {
                            // содержимое цикла забираем в переменную чтобы убрать запятые и капитализировать ???
                            sb.AppendLine("<xsl:variable name='content" + sX.ItemId.Replace(".", "_") + "' >");
                            OpenVariable[0] = sX;

                            var sp2 = Processor();
                            foreach (var sX2 in sX.Children) sb.Append(sp2.Process(xmlPath, sX2, true));

                            // закрываем переменную, чтобы не  пересекать границы  цикла
                            if (OpenVariable[0] != null)
                            {
                                if (DebugPrint) sb.AppendLine("<!-- END of TOC  -->");
                                sb.AppendLine("</xsl:variable>");
                                sb.AppendLine(
                                    @"<xsl:call-template name=""string-ltrim_br""><xsl:with-param name = ""string"" select = '$content" +
                                    OpenVariable[0].ItemId.Replace(".", "_") + "' /></xsl:call-template>");
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
                    if (sX.IsToc())
                    {
                        // закрываем строку

                        if (OpenVariable[0] != null)
                        {
                            sb.AppendLine("</xsl:variable>");
                            sb.AppendLine(
                                @"<xsl:call-template name=""string-ltrim_br""><xsl:with-param name = ""string"" select = '$content" +
                                OpenVariable[0].ItemId.Replace(".", "_") + "' /></xsl:call-template>");
                            OpenVariable[0] = null;
                        }

                        sb.AppendLine(TocEnd(sX));
                        if (DebugPrint) sb.AppendLine("<!-- END of TOC  -->");
                    }
                    else if (sX.IsHeader())
                    {
                        sb.Append(HeaderEnd(sX));
                        if (DebugPrint) sb.AppendLine("<!-- END of Header  -->");
                        if (OpenVariable[0] != null)
                        {
                            sb.AppendLine("</xsl:variable>");

                            sb.AppendLine(
                                @"<xsl:call-template name=""string-ltrim_br""><xsl:with-param name = ""string"" select = '$content" +
                                OpenVariable[0].ItemId.Replace(".", "_") + "' /></xsl:call-template>");
                            OpenVariable[0] = null;
                        }
                    }
                    else
                    {
                        sb.Append(ItemEnd(sX));
                    }


                    sb.Append(@"</xsl:if>");
                    if (DebugPrint) sb.AppendLine(@"<!-- End of #" + sX.ItemId + " -->");
                }
                else
                {
                    if (DebugPrint) sb.AppendLine("<!-- " + sX.ToString(false) + " -->");
                    if (DebugPrint) sb.AppendLine("<!-- Error: Данные не найдены в XML файле -->");
                    sb.AppendLine("<xsl:text> [ОШИБКА: Нет данных для поля (" + sX.Caption + "), ID:" + sX.ItemId +
                                  "] </xsl:text>");
                    _errors.AppendLine(sX.ToString(false));
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                return "<!-- Error: " + ex.Message + " -->";
            }
        }

        protected virtual string TocStart(string caption)
        {
            var sOut = @"<tr>
	            <td colspan=""2"" class=""myline"" style=""border-top: 0.5pt solid rgba(0,0,0,0.4); height:1pt""/>
                </tr>
                <tr>
	                <td class=""mytd"" valign=""top"" align=""left"" width=""116pt"">
		                <table width=""116pt"" style=""border-collapse: collapse;"" border=""0"" cellspacing=""0"" cellpadding=""0"">
			                <thead>
				                <tr>
					                <th class=""myth"" valign=""top"" align=""left"">
						                <span class=""part"">" + caption + @"</span>
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
            var sOut = "";
            if (sX.DotAfter) sOut += ". ";
            sOut += @"</td></tr>";
            return sOut;
        }

        protected virtual string HeaderEnd(XsltItem sX)
        {
            var sOut = @"";
            if (sX.DotAfter) sOut += ". ";
            return sOut;
        }

        protected virtual string HeaderStart(string caption, XsltItem sX)
        {
            var sOut = "";

            if (caption.Trim() != "")
                if (sX.IsBold())
                    sOut += @"<strong>" + caption + ": </strong>";
                else
                    sOut += @"<span> " + caption + ": </span>";

            return sOut;
        }

        protected virtual string ItemStart(string caption, XsltItem sX)
        {
            var sOut = "";
            if (sX.ComaBefore) sOut += ", ";
            if (caption.Trim() != "")
                sOut += @"<span>" + caption + ": </span>";
            return sOut;
        }

        protected virtual string ItemEnd(XsltItem sX)
        {
            var sOut = @"";
            if (sX.DotAfter) sOut += ". ";
            return sOut;
        }

        private string CutFor(XsltItem x, string path, bool excludeFor)
        {
            string cf;
            XsltItem xParent;
            xParent = x.Parent;
            cf = path;
            if (excludeFor)
                while (xParent != null)
                {
                    if (xParent.XslFor != null && xParent.XslFor != "")
                        if (cf.Contains(xParent.XslFor))
                            cf = cf.Replace(xParent.XslFor + "/", "");
                    xParent = xParent.Parent;
                }

            return cf;
        }
    }
}