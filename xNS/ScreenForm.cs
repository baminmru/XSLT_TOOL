using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xNS
{
    class ScreenForm: SpecPro
    {
        protected override string TocStart(string Caption)
        {
            return "<tr><td class='myth' ><span class='part'>" + Caption + "</span></td></tr><tr><td class='mytd'>";
        }

        protected override string HeaderStart(string Caption, XsltItem sX)
        {
            string sOut = "";
            sOut+= "<p><span class='part'>" + Caption + "</span></p>";

            return sOut;
        }
        protected override string TocEnd(XsltItem sX)
        {
            string sOut = "";
            sOut += "</td></tr>";
            return sOut;
        }
        protected override string HeaderEnd(XsltItem sX)
        {
            string sOut = @"";
            if (sX.DotAfter) sOut += ". ";
            //if(sX.NextSibling() != null )
            //if(sX.Level()==2)
            //    sOut += "<p class='gap'></p>";
            return sOut;
        }
        protected override string ItemStart(string Caption, XsltItem sX)
        {

            string sOut="" ;
            if (sX.ComaBefore) sOut += ", ";
            if (Caption.Trim() !="")
                sOut += @"<span>" + Caption + "-</span>";
           
            return sOut;
        }

        protected override string ItemEnd(XsltItem sX)
        {
            string sOut = @"";
            if (sX.DotAfter) sOut += ". ";
            return sOut;
        }

        protected override SpecPro Processor() { return new ScreenForm(); }

    }
}
