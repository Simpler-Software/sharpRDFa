using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using sharpRDFa.Processing;
using sharpRDFa.RDFa;

namespace sharpRDFa.Extension
{
    public static class HtmlNodeExtensions
    {
        private static readonly IRDFaProcessor Processor = new RDFaProcessor();

        public static IDictionary<string, string> GetNameSpaceMappings(this HtmlNode node)
        {
            var dic = new Dictionary<string, string>();
            if (node.IsNull()) return dic;
            foreach (var attribute in node.Attributes)
            {
                NameSpace nameSpace = Processor.IsNameSpace(attribute.Name);
                if (!nameSpace.IsNotNull()) continue;
                if (!dic.ContainsKey(nameSpace.NCName))
                    dic.Add(nameSpace.NCName, attribute.Value);
            }

            return dic;
        }

        public static string GetLanguage(this HtmlNode node)
        {
            if (node.IsNull() || node.Attributes["xml:lang"] == null) return string.Empty;
            return node.Attributes["xml:lang"].Value;
        }

        public static bool HasAttribute(this HtmlNode node, string attributeName)
        {
            return node != null && node.Attributes[attributeName] != null;
        }

        public static HtmlAttribute GetAttribute(this HtmlNode node, string attributeName)
        {
            if (!node.HasAttribute(attributeName)) return null;
            return node.Attributes[attributeName];
        }

        public static bool IsTextNode(this HtmlNode node)
        {
            return node.ChildNodes.All(childNode => childNode.NodeType == HtmlNodeType.Text);
        }

        public static string GetChildrenText(this HtmlNode node)
        {
            var result = string.Empty;

            foreach (var childNode in node.ChildNodes)
            {
                if (childNode.NodeType == HtmlNodeType.Text)
                {
                    result += childNode.InnerText;
                }
                else if (childNode.NodeType == HtmlNodeType.Element)
                {
                    result += childNode.GetChildrenText();
                }
            }

            return result;
        }
    }
}
