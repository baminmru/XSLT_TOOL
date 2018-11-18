namespace xNS
{
    internal class ScreenForm : SpecPro
    {
        protected override string TocStart(string caption)
        {
            return "<tr><td class='myth' ><span class='part'>" + caption + "</span></td></tr><tr><td class='mytd'>";
        }

        protected override string HeaderStart(string caption, XsltItem sX)
        {
            var sOut = "";
            sOut += "<p><span class='part'>" + caption + "</span></p>";

            return sOut;
        }

        protected override string TocEnd(XsltItem sX)
        {
            var sOut = "";
            sOut += "</td></tr>";
            return sOut;
        }

        protected override string HeaderEnd(XsltItem sX)
        {
            var sOut = @"";
            if (sX.DotAfter) sOut += ". ";
            return sOut;
        }

        protected override string ItemStart(string caption, XsltItem sX)
        {
            var sOut = "";
            if (sX.ComaBefore) sOut += ", ";
            if (caption.Trim() != "")
                sOut += @"<span>" + caption + "-</span>";
            return sOut;
        }

        protected override string ItemEnd(XsltItem sX)
        {
            var sOut = @"";
            if (sX.DotAfter) sOut += ". ";
            return sOut;
        }

        protected override SpecPro Processor()
        {
            return new ScreenForm();
        }
    }
}