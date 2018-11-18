using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace xNS
{
    public partial class FrmSizercs : Form
    {
        private int _iDiv;
        private int _iMax;

        private int _iMin;
        private int _iMul;

        public FrmSizercs()
        {
            InitializeComponent();
        }

        private void cmdXML_Click(object sender, EventArgs e)
        {
            opf.Multiselect = false;
            if (opf.ShowDialog() == DialogResult.OK) txtXML.Text = opf.FileName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // width\s{0,}:{\s{0,}[0-9]{1,}\s{0,}px
            // width\s?:\s?[0-9]{1,}\s?px

            _iMin = int.Parse(txtMin.Text);
            _iMax = int.Parse(txtMax.Text);
            _iMul = int.Parse(txtMul.Text);
            _iDiv = int.Parse(txtDiv.Text);

            string fdata;
            fdata = File.ReadAllText(txtXML.Text);
            string nf;

            var pattern = @"width\s?:\s?[0-9]{1,}\s?px";
            var pattern2 = @"width\s?=\s?""[0-9]{1,}\s?px""";

            var pattern3 = @"width\s?:\s?[0-9]{1,}\s?mm";
            var pattern4 = @"width\s?=\s?""[0-9]{1,}\s?mm""";

            nf = Resizer(pattern, fdata, true, "px");
            nf = Resizer(pattern2, nf, false, "px");
            nf = Resizer(pattern3, nf, true, "mm");
            nf = Resizer(pattern4, nf, false, "mm");


            txtOut.Text = nf;
            File.Delete(txtXML.Text + ".resize");
            File.WriteAllText(txtXML.Text + ".resize", nf);
        }


        private string Resizer(string pattern, string fdata, bool style, string units)
        {
            var regex = new Regex(pattern);
            var nf = "";
            // Получаем совпадения в экземпляре класса Match
            var match = regex.Match(fdata);
            var prevIdx = 0;
            int curIdx;

            // отображаем все совпадения
            while (match.Success)
            {
                curIdx = match.Index;
                nf = nf + fdata.Substring(prevIdx, curIdx - prevIdx);
                prevIdx = curIdx + match.Length;
                nf = nf + ProcessMatch(match.Value, style, units);

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
            var tmp = s;
            tmp = tmp.Replace("width", "");
            tmp = tmp.Replace(units, "");
            tmp = tmp.Replace(":", "");
            tmp = tmp.Replace("=", "");
            tmp = tmp.Replace(" ", "");
            tmp = tmp.Replace("\"", "");
            tmp = tmp.Replace("\t", "");

            int iVal;

            iVal = int.Parse(tmp);

            if (iVal > _iMin && iVal < _iMax)
            {
                iVal = iVal * _iMul / _iDiv;
                if (style)
                    sOut = "width: " + iVal + units;
                else
                    sOut = " width= \"" + iVal + units + "\" ";
            }
            else
            {
                sOut = s;
            }

            return sOut;
        }
    }
}