using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;


namespace xNS
{
    public partial class frmSizercs : Form
    {
        public frmSizercs()
        {
            InitializeComponent();
        }

        private int iMin;
        private int iMax;
        private int iMul;
        private int iDiv;

        private void cmdXML_Click(object sender, EventArgs e)
        {
            opf.Multiselect = false;
            if (opf.ShowDialog() == DialogResult.OK)
            {
                txtXML.Text = opf.FileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            // width\s{0,}:{\s{0,}[0-9]{1,}\s{0,}px
            // width\s?:\s?[0-9]{1,}\s?px

            iMin = int.Parse(txtMin.Text);
            iMax = int.Parse(txtMax.Text);
            iMul = int.Parse(txtMul.Text);
            iDiv = int.Parse(txtDiv.Text);

            String fdata;
            fdata = File.ReadAllText(txtXML.Text);
            string nf="";
            
            string pattern = @"width\s?:\s?[0-9]{1,}\s?px";
            string pattern2 = @"width\s?=\s?""[0-9]{1,}\s?px""";

            string pattern3 = @"width\s?:\s?[0-9]{1,}\s?mm";
            string pattern4 = @"width\s?=\s?""[0-9]{1,}\s?mm""";

            nf = Resizer(pattern, fdata,true,"px");
            nf = Resizer(pattern2, nf,false, "px");
            nf = Resizer(pattern3, nf, true, "mm");
            nf = Resizer(pattern4, nf, false, "mm");


            txtOut.Text = nf;
            File.Delete(txtXML.Text + ".resize");
            File.WriteAllText(txtXML.Text+".resize",nf);
        }


        private string Resizer( string pattern, string fdata,bool style, string units)
        {
            Regex regex = new Regex(pattern);
            string nf="";
            // Получаем совпадения в экземпляре класса Match
            Match match = regex.Match(fdata);
            int prevIdx = 0;
            int curIdx;

            // отображаем все совпадения
            while (match.Success)
            {
                curIdx = match.Index;
                nf = nf + fdata.Substring(prevIdx, curIdx - prevIdx);
                prevIdx = curIdx + match.Length;
                nf = nf + ProcessMatch(match.Value, style,units);

                // Переходим к следующему совпадению
                match = match.NextMatch();
            }


            curIdx = fdata.Length;
            if (prevIdx < curIdx)
                nf = nf + fdata.Substring(prevIdx, curIdx - prevIdx);
            return nf;
        }



        private string ProcessMatch(string s, bool style, string units)
        {
            string sOut;
            string tmp = s;
            tmp = tmp.Replace("width", "");
            tmp = tmp.Replace(units, "");
            tmp = tmp.Replace(":", "");
            tmp = tmp.Replace("=", "");
            tmp = tmp.Replace(" ", "");
            tmp = tmp.Replace("\"", "");
            tmp = tmp.Replace("\t", "");

            int iVal;

            iVal = int.Parse(tmp);

            if(iVal >iMin && iVal < iMax)
            {
                iVal = iVal * iMul / iDiv;
                if(style)
                    sOut = "width: " + iVal.ToString() + units;
                else
                    sOut = " width= \"" + iVal.ToString() + units +"\" ";
            }
            else
            {
                sOut = s;
            }

            return sOut;

        }
    }
}
