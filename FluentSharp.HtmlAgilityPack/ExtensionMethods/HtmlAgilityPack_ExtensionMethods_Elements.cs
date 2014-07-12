using System.Collections.Generic;
using FluentSharp.CoreLib;

namespace FluentSharp.HtmlAgilityPacK
{
    public static class HtmlAgilityPack_ExtensionMethods_Elements
    {

        public static List<HtmlAgilityPack.HtmlNode> nodes(this List<HtmlAgilityPack.HtmlNode> htmlNodes)
        {
            return htmlNodes.nodes("");
        }

        public static List<HtmlAgilityPack.HtmlNode> nodes(this List<HtmlAgilityPack.HtmlNode> htmlNodes, string nodeName)
        {
            var allNodes = new List<HtmlAgilityPack.HtmlNode>();
            foreach (var htmlNode in htmlNodes)
                allNodes.add(htmlNode.nodes(nodeName));
            return allNodes;
        }

        public static List<HtmlAgilityPack.HtmlNode> nodes(this HtmlAgilityPack.HtmlNode htmlNode)
        {
            return htmlNode.nodes("");
        }
        public static List<HtmlAgilityPack.HtmlNode> nodes(this HtmlAgilityPack.HtmlNode htmlNode, string nodeName)
        {
            var htmlNodes = new List<HtmlAgilityPack.HtmlNode>();
            foreach (var node in htmlNode.ChildNodes)
                if (nodeName.valid().isFalse() || node.Name == nodeName)
                    htmlNodes.add(node);
            return htmlNodes;
        }

        public static HtmlAgilityPack.HtmlNode node(this HtmlAgilityPack.HtmlNode htmlNode, string nodeName)
        {
            foreach (var node in htmlNode.ChildNodes)
                if (nodeName.valid().isFalse() || node.Name == nodeName)
                    return htmlNode;
            return null;
        }
    }
}