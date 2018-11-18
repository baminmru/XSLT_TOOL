using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace xNS
{
    public class XmlPlusItem
    {
        public XmlNode Node;
        public string NodeText;
        public string Path;
        public string PathNs;
    }

    public static class XmlTools
    {
        public static string NodeSelf(XmlNode node)
        {
            try
            {
                var sOut = new StringBuilder();

                sOut.Append("<");
                sOut.Append(node.Name + " ");

                foreach (XmlAttribute a in node.Attributes)
                {
                    sOut.AppendLine("");
                    sOut.Append("            " + a.Name + "=\"" + a.Value + "\" ");
                }

                sOut.Append("/>");
                return sOut.ToString();
            }
            catch
            {
                return node.Name;
            }
        }

        private static string UseDefaultNameSpace(string path, string ns)
        {
            var sOut = "";
            string[] items;
            items = path.Split('/');
            foreach (var s in items)
            {
                if (sOut != "") sOut = sOut + "/";
                if (s == "" || s.Contains(":") || s.Contains("@") || s.StartsWith("["))
                {
                    sOut += s;
                }
                else
                {
                    if (ns != "")
                        sOut += ns + ":" + s;
                    else
                        sOut += s;
                }
            }

            return "/" + sOut;
        }

        private static string FindXPath(XmlNode node)
        {
            var builder = new StringBuilder();
            while (node != null)
                switch (node.NodeType)
                {
                    case XmlNodeType.Attribute:
                        builder.Insert(0, "/@" + node.Name);
                        node = ((XmlAttribute) node).OwnerElement;
                        break;
                    case XmlNodeType.Element:
                        var index = FindElementIndex(node);
                        if (index == 1)
                            builder.Insert(0, "/" + node.Name);
                        else
                            builder.Insert(0, "/" + node.Name + "[" + index + "]");
                        node = node.ParentNode;
                        break;
                    case XmlNodeType.Text:
                        var index2 = FindElementIndex(node);
                        if (index2 == 1)
                            builder.Insert(0, "/" + node.Name);
                        else
                            builder.Insert(0, "/" + node.Name + "[" + index2 + "]");
                        node = node.ParentNode;
                        break;

                    case XmlNodeType.Document:
                        return builder.ToString();

                    case XmlNodeType.XmlDeclaration:
                        return "";

                    default:
                        throw new ArgumentException("Only elements and attributes are supported");
                }
            throw new ArgumentException("Node was not in a document");
        }

        private static int FindElementIndex(XmlNode element)
        {
            var parentNode = element.ParentNode;
            if (parentNode is XmlDocument) return 1;
            var parent = parentNode;
            var index = 1;
            foreach (XmlNode candidate in parent.ChildNodes)
                if (candidate is XmlNode && candidate.Name == element.Name)
                {
                    if (candidate == element) return index;
                    index++;
                }

            throw new ArgumentException("Couldn't find element within parent");
        }

        public static List<XmlPlusItem> IterateThroughAllNodes(
            this XmlDocument doc, string nameSpace
        )
        {
            var pathList = new List<XmlPlusItem>();
            if (doc != null)
                foreach (XmlNode node in doc.ChildNodes)
                    DoIterateNode(node, ref pathList, nameSpace);
            return pathList;
        }

        private static void DoIterateNode(XmlNode node, ref List<XmlPlusItem> pathList, string nameSpace)
        {
            var xpi = new XmlPlusItem();
            xpi.Node = node;
            bool ok;
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
                xpi.PathNs = UseDefaultNameSpace(xpi.Path, nameSpace);
                if (node.ChildNodes.Count == 1)
                    if (node.FirstChild.NodeType == XmlNodeType.Text)
                        xpi.NodeText = node.FirstChild.InnerText;
                pathList.Add(xpi);

                foreach (XmlNode childNode in node.ChildNodes) DoIterateNode(childNode, ref pathList, nameSpace);
            }
        }
    }
}