using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Serialization;


namespace xNS
{
    public class XMLBuilder
    {

        public xsdItem root { get; set; }
        public static List<String> StopStr;
        private XmlNamespaceManager nsmgr;
        public string XSDPath { get; set; }
        public string OutFolder { get; set; }

        // списки для проверки
        public static List<String> RegularExpressions { get; set; } = null;
        public static List<String> SeekExpressions { get; set; } = null;


        // конфигурационный  файл
        public static string ConfigPath { get; set; } = "";
        public static XmlDocument Config { get; set; } = null;

        // шаблоны для  генерации  данных в соответствии с требованиями XSD
        [XmlIgnoreAttribute]
        private static Dictionary<string, string> PatternSamples;

        // список  единиц измерений для генерации
        [XmlIgnoreAttribute]
        private static Dictionary<string, string> Units;

        [XmlIgnoreAttribute]
        private static Random Rnd;



        public XMLBuilder()
        {
            root = null;

            if (Rnd == null)
            {
                Rnd = new Random();
            }

            

            if (Config == null)
            {
                string cfgPath;
                cfgPath = this.GetType().Assembly.Location;
                FileInfo fi = new FileInfo(cfgPath);
                ConfigPath = fi.DirectoryName + "\\config.xml";

                StopStr = new List<String>();
                RegularExpressions = new List<String>();
                SeekExpressions = new List<String>();

                Config = new XmlDocument();
                try
                {
                    Config.Load(ConfigPath);
                }
                catch
                {
                    MessageBox.Show("Не найден файл конфигурации: " + ConfigPath);
                }


                XmlNodeList xl = Config.GetElementsByTagName("Stopper");
            
                if (xl.Count > 0)
                {
                    foreach (XmlNode xn in xl)
                    {
                        StopStr.Add(xn.InnerText.ToLower());
                    }
                }
               

                xl = Config.GetElementsByTagName("regexp");

                if (xl.Count > 0)
                {
                    foreach (XmlNode xn in xl)
                    {
                        RegularExpressions.Add(xn.InnerText);
                        System.Diagnostics.Debug.Print("REG : " + xn.InnerText);
                    }
                }


                xl = Config.GetElementsByTagName("seek");

                if (xl.Count > 0)
                {
                    foreach (XmlNode xn in xl)
                    {
                        SeekExpressions.Add(xn.InnerText);
                        System.Diagnostics.Debug.Print("SEEK: " + xn.InnerText);
                    }
                }




                InitPatternSamples();
                InitUnits();
            }

            // устанавлиываем  класс для генерации
            if (xsdItem.Builder == null)
            {
                xsdItem.Builder = this;
            }

        }


        // загрузка  структуры  данных из   файла XML 
        public void LoadXSD(string newXSDPath)
        {
            XSDPath = newXSDPath;
            XmlDocument xDoc = new XmlDocument();

            string sXSD = File.ReadAllText(XSDPath);

            //  patch Factor problem
            sXSD = sXSD.Replace("Провоцирующий_fslash_купирующий_фактор", "Провоцирующий_фактор");
            sXSD = sXSD.Replace("&lt;", "_меньше_");
            sXSD = sXSD.Replace("&gt;", "_больше_");

            //sXSD = sXSD.Replace("купирующий_фактор", "провоцирующий_фактор");



            xDoc.LoadXml(sXSD);

            nsmgr = new XmlNamespaceManager(xDoc.NameTable);
            nsmgr.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");
        
            StringBuilder sb = new StringBuilder();

            root = new xsdItem();

            XmlNodeList rootElements = xDoc.LastChild.ChildNodes;
            XmlElement rootEl;

            foreach (XmlNode node in rootElements)
            {
                if (node.Name == "xs:element")
                {
                    rootEl = (XmlElement)node;
                    string Name = rootEl.GetAttribute("name");

                    root.Name = Name;
                    root.Type = "root";
                    root.oMax = "1";
                    root.oMin = "1";

                    readChild(root, rootEl);
                }
            }

            root.RestoreParent();
        }

        // генерация по готовой структуре
        public string BuildXML(xsdItem newRoot,string GenPaths)
        {
            root = newRoot;
            root.RestoreParent();

            string sOut;
            string testName;
            List<string> Paths=null;

           testName = OutFolder + "\\" + root.Name +"_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xml";

            if (GenPaths == "")
            {
                sOut = root.Generate(null).ToString();
            }
            else
            {
                Paths = new List<string>();
                string[] s = GenPaths.Split('\r');
                foreach (string s1 in s)
                {
                    if (s1.Length > 1)
                        Paths.Add(s1.Replace("\n", ""));
                }
                sOut = root.GeneratePaths(null, Paths).ToString();
            }
            File.WriteAllText(testName, sOut);
            return testName; // sOut;
        }

        //  генерация по XSD
        public string BuildXML(string GenPaths)
        {

            
            LoadXSD(XSDPath);

            string sOut;
            string testName;
            List<string> Paths = null;

            //sOut = root.Generate(null).ToString();

            if (GenPaths == "")
            {
                sOut = root.Generate(null).ToString();
            }
            else
            {
                Paths = new List<string>();
                string[] s = GenPaths.Split('\r');
                foreach (string s1 in s)
                {
                    if (s1.Length > 1)
                        Paths.Add(s1.Replace("\n","" ));
                }
                sOut = root.GeneratePaths(null, Paths).ToString();
            }

            testName = OutFolder + "\\" + root.Name + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xml";

            File.WriteAllText(testName, sOut);
            return testName; // sOut;
        }


        // попытка вытащить варианты значений для поля из комментариев
        private string processRestrictions(string res)
        {
            string sOut = "";
            string[] stringSeparators = new string[] { "<!--", "-->" };
            string[] items = res.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in items)
            {
                if (s.Contains("Value = "))
                {
                    if (sOut != "") sOut += ";";
                    sOut += s.Replace("Value = ", "").Trim();

                }
            }
            return sOut;
        }


        //  чтение структуры 
        private void readChild(xsdItem xsd, XmlElement el)
        {
            XmlNodeList ct = el.SelectNodes("./xs:complexType", nsmgr);
            foreach (XmlNode node in ct)
            {

                XmlElement el2 = (XmlElement)node;
                XmlNodeList sq = el2.SelectNodes("./xs:sequence", nsmgr);
                foreach (XmlNode node2 in sq)
                {

                    XmlElement el3 = (XmlElement)node2;
                    XmlNodeList children = el3.SelectNodes("./xs:element", nsmgr);
                    foreach (XmlNode node3 in children)
                    {

                        XmlElement el4 = (XmlElement)node3;


                        xsdItem xsdChild = new xsdItem();
                        try { xsdChild.Name = el4.GetAttribute("name"); }
                        catch { }

                        if (xsdChild.Name != "")
                        {
                            if (!StopStr.Contains(xsdChild.Name.ToLower()))
                            {
                                try { xsdChild.Type = el4.GetAttribute("type"); }
                                catch { xsdChild.Type = ""; }

                                if (xsdChild.Type == "")
                                {
                                    XmlNodeList restricts = el4.SelectNodes("./xs:simpleType/xs:restriction", nsmgr);
                                    if (restricts != null && restricts.Count > 0)
                                    {
                                        XmlElement r = (XmlElement)restricts[0];
                                        try { xsdChild.Type = r.GetAttribute("base"); }
                                        catch { xsdChild.Type = ""; }

                                        XmlNodeList pattern = r.SelectNodes("xs:pattern", nsmgr);

                                        foreach (XmlNode pn in pattern)
                                        {
                                            XmlElement p = (XmlElement)pn;
                                            try { xsdChild.Patterns.Add(p.GetAttribute("value")); }
                                            catch { }
                                        }

                                        XmlNodeList ens = r.SelectNodes("xs:enumeration", nsmgr);

                                        if (ens.Count > 0)
                                        {
                                            string R = "";
                                            foreach (XmlNode pn in ens)
                                            {
                                                if (R != "") R += ";";

                                                XmlElement p = (XmlElement)pn;
                                                try { R += p.GetAttribute("value"); }
                                                catch { }
                                            }
                                            if (R != "")
                                            {
                                                xsdChild.Restrictions = R;
                                            }
                                        }

                                    }
                                }


                                try { xsdChild.oMin = el4.GetAttribute("minOccurs"); }
                                catch { xsdChild.oMin = "0";  }
                                if(xsdChild.oMin=="") xsdChild.oMin = "0";

                                try { xsdChild.oMax = el4.GetAttribute("maxOccurs"); }
                                catch { xsdChild.oMax = "1"; }
                                if (xsdChild.oMax == "") xsdChild.oMax = "1";

                                try { xsdChild.Fixed = el4.GetAttribute("fixed"); }
                                catch { xsdChild.Fixed = ""; }




                                if (xsdChild.Name.ToLower() == "defining_code")
                                {
                                    XmlNodeList restricts = el4.SelectNodes(".//xs:restriction", nsmgr);
                                    if (restricts != null && restricts.Count > 0)
                                    {
                                        xsdChild.Restrictions = processRestrictions(restricts[0].InnerXml);
                                        if (xsdChild.Restrictions != null && xsdChild.Restrictions != "")
                                            xsd.Children.Add(xsdChild);
                                    }
                                    else
                                    {
                                        XmlNodeList seq = el4.SelectNodes("./xs:complexType/xs:sequence", nsmgr);
                                        if (seq != null && seq.Count > 0)
                                        {
                                            xsdChild.Restrictions = processRestrictions(seq[0].InnerXml);
                                            if (xsdChild.Restrictions != null && xsdChild.Restrictions != "")
                                                xsd.Children.Add(xsdChild);
                                        }
                                    }
                                }
                                else
                                {
                                    xsd.Children.Add(xsdChild);
                                    readChild(xsdChild, el4);
                                }



                            }
                        }


                    }

                    // обработка choice варианта 
                    children = el3.SelectNodes("./xs:choice/xs:element", nsmgr);
                    foreach (XmlNode node3 in children)
                    {

                        XmlElement el4 = (XmlElement)node3;


                        xsdItem xsdChild = new xsdItem();
                        try { xsdChild.Name = el4.GetAttribute("name"); }
                        catch { }

                        if (xsdChild.Name != "")
                        {
                            if (!StopStr.Contains(xsdChild.Name.ToLower()))
                            {
                                try { xsdChild.Type = el4.GetAttribute("type"); }
                                catch { xsdChild.Type = ""; }

                                if (xsdChild.Type == "")
                                {
                                    XmlNodeList restricts = el4.SelectNodes("./xs:simpleType/xs:restriction", nsmgr);
                                    if (restricts != null && restricts.Count > 0)
                                    {
                                        XmlElement r = (XmlElement)restricts[0];
                                        try { xsdChild.Type = r.GetAttribute("base"); }
                                        catch { xsdChild.Type = ""; }

                                    }
                                }

                                try { xsdChild.oMin = el4.GetAttribute("minOccurs"); }
                                catch { xsdChild.oMin = "0"; }
                                if (xsdChild.oMin == "") xsdChild.oMin = "0";

                                try { xsdChild.oMax = el4.GetAttribute("maxOccurs"); }
                                catch { xsdChild.oMax = "1"; }
                                if (xsdChild.oMax == "") xsdChild.oMax = "1";





                                if (xsdChild.Name.ToLower() == "defining_code")
                                {
                                    XmlNodeList restricts = el4.SelectNodes(".//xs:restriction", nsmgr);
                                    if (restricts != null && restricts.Count > 0)
                                    {
                                        xsdChild.Restrictions = processRestrictions(restricts[0].InnerXml);
                                        xsd.Choice.Add(xsdChild);
                                    }
                                }
                                else
                                {
                                    xsd.Choice.Add(xsdChild);
                                    readChild(xsdChild, el4);
                                }




                            }
                        }
                    }


                }




            }
        }


        // инициализация списка единиц измерения
        private static void InitUnits()
        {

            XmlNodeList xl = Config.GetElementsByTagName("Unit");

            if (xl.Count > 0)
            {
                if (Units == null)
                {
                    Units = new Dictionary<string, string>();
                }
                Units.Clear();
                foreach (XmlNode xn in xl)
                {
                    string Key;
                    string Value;

                    Key = "";
                    Value = "";
                    foreach(XmlNode xe in xn.ChildNodes)
                    {
                        if (xe.Name == "Value")
                        {
                            Key = xe.InnerText;
                        }

                        if (xe.Name == "Name")
                        {
                            Value = xe.InnerText;
                        }
                    }


                    if(Key !="" && Value != "")
                    {
                        try
                        {
                            Units.Add(Key, Value);
                        }
                        catch
                        {

                        }
                        
                    }

                    
                }
            }

        }

        // инициализация списка шаблонов
        private static void InitPatternSamples()
        {
            XmlNodeList xl = Config.GetElementsByTagName("Pattern");

            if (xl.Count > 0)
            {
                if (PatternSamples == null)
                {
                    PatternSamples = new Dictionary<string, string>();
                }
                PatternSamples.Clear();
                foreach (XmlNode xn in xl)
                {
                    string Key;
                    string Value;

                    Key = "";
                    Value = "";
                    foreach (XmlNode xe in xn.ChildNodes)
                    {
                        if (xe.Name == "Key")
                        {
                            Key = xe.InnerText;
                        }

                        if (xe.Name == "Samples")
                        {
                            Value = xe.InnerText;
                        }
                    }


                    if (Key != "" && Value != "")
                    {
                        try
                        {
                            PatternSamples.Add(Key, Value);
                        }
                        catch
                        {

                        }

                    }


                }
            }
        }


        // генерация единицы измерения, если она не задана для поля явно
        public string GetRandomUnit()
        {
            int v = Rnd.Next(Units.Count);
            string sOut;
            sOut = Units.Keys.ElementAt(v);
            System.Diagnostics.Debug.Print("Unit: " + sOut);
            return sOut;

        }

        // генерация выражения по шаблону регулярного выражния
        public string GetPatternSample(string Pattern)
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
                System.Diagnostics.Debug.Print("Expression: " + s[v]);
                return s[v];
            }
            else
            {

                System.Diagnostics.Debug.Print("Unknown pattern: " + Pattern);
                return Pattern;
            }


        }

    }
}
