using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace xNS
{
    public class XmlPlusItem
    {
        public XmlNode node;
        public String Path;
        public String PathNs;
        public String NodeText;
    }


    public static class  XmlTools
    {

        public static string NodeSelf(XmlNode node)
        {
            XmlDocument xdoc = new XmlDocument();
            try
            {

                StringBuilder sOut = new StringBuilder();

                sOut.Append("<");
                sOut.Append(node.Name +" " );

                foreach(XmlAttribute a in node.Attributes)
                {
                    sOut.AppendLine("");
                    sOut.Append("            "+a.Name +"=\""+a.Value+"\" " );
                }
                
                sOut.Append("/>");
                return sOut.ToString(); 
            }
            catch  {
                return node.Name;
            }
        }


        public static string UseDefaultNameSpace(string path, string ns)
        {
            string sOut="";
            string[] items;
            items = path.Split('/');
            foreach(string s in items)
            {
                if(sOut != "")
                {
                    sOut = sOut + "/";
                }
                if (s=="" || s.Contains(":")|| s.Contains("@") || s.StartsWith("["))
                {
                    sOut += s;
                }else
                {
                    if (ns !=""){

                        sOut += ns + ":" + s;
                    }else{
                        sOut +=  s;
                    }
                }
            }
            return "/"+ sOut;

        }


        public static string FindXPath(XmlNode node)
        {
            StringBuilder builder = new StringBuilder();
            while (node != null)
            {
                switch (node.NodeType)
                {
                    case XmlNodeType.Attribute:
                        builder.Insert(0, "/@" + node.Name);
                        node = ((XmlAttribute)node).OwnerElement;
                        break;
                    case XmlNodeType.Element:
                        int index = FindElementIndex(node);
                        if (index == 1)
                        {
                            builder.Insert(0, "/" + node.Name );
                        }
                        else
                        {
                            builder.Insert(0, "/" + node.Name + "[" + index + "]");
                        }
                        node = node.ParentNode;
                        break;
                    case XmlNodeType.Text:
                        int index2 = FindElementIndex(node);
                        if (index2 == 1)
                        {
                            builder.Insert(0, "/" + node.Name);
                        }
                        else
                        {
                            builder.Insert(0, "/" + node.Name + "[" + index2 + "]");
                        }
                        node = node.ParentNode;
                        break;

                    case XmlNodeType.Document:
                        return builder.ToString();

                    case XmlNodeType.XmlDeclaration:
                        return "";
                    
                    default:
                        throw new ArgumentException("Only elements and attributes are supported");
                }
            }
            throw new ArgumentException("Node was not in a document");
        }

        public static int FindElementIndex(XmlNode element)
        {
            XmlNode parentNode = element.ParentNode;
            if (parentNode is XmlDocument)
            {
                return 1;
            }
            XmlNode parent = (XmlNode)parentNode;
            int index = 1;
            foreach (XmlNode candidate in parent.ChildNodes)
            {
                if (candidate is XmlNode && candidate.Name == element.Name)
                {
                    if (candidate == element)
                    {
                        return index;
                    }
                    index++;
                }
            }
            throw new ArgumentException("Couldn't find element within parent");
        }


        public static List<XmlPlusItem> IterateThroughAllNodes(
                 this XmlDocument doc, string NameSpace
                )
        {
            List<XmlPlusItem> PathList = new List<XmlPlusItem>();
            if (doc != null )
            {
                foreach (XmlNode node in doc.ChildNodes)
                {
                    doIterateNode(node, ref PathList,NameSpace);
                }
            }
            return PathList;
        }

        private static void doIterateNode(
            XmlNode node
            , ref List<XmlPlusItem> PathList, string NameSpace)
        {

            XmlPlusItem xpi = new XmlPlusItem();
            xpi.node = node;
            Boolean ok;
            switch (node.NodeType)
            {
                case XmlNodeType.Attribute:
                    ok = false;
                    break;
                case XmlNodeType.Element:
                    ok = true;
                    break;
                case XmlNodeType.Text:
                    ok = false;
                    break;

                case XmlNodeType.Document:
                    ok = false;
                    break;
                case XmlNodeType.XmlDeclaration:
                    ok = false;
                    break;

                default:
                    ok = false;
                    break;
            }

            if (ok)
            {
                xpi.Path = FindXPath(node);
                xpi.PathNs = UseDefaultNameSpace(xpi.Path, NameSpace);
                if (node.ChildNodes.Count == 1)
                {
                    if(node.FirstChild.NodeType== XmlNodeType.Text)
                    {
                        xpi.NodeText = node.FirstChild.InnerText; 
                    }
                }
                PathList.Add(xpi);

                foreach (XmlNode childNode in node.ChildNodes)
                {
                    doIterateNode(childNode, ref PathList, NameSpace);
                }
            }
        }


        /*  Usage:
                var doc = new XmlDocument();
                doc.Load(somePath);

                doc.IterateThroughAllNodes(
                delegate(XmlNode node)
                {
                    // ...Do something with the node...
                });
         */
         

    }
}
