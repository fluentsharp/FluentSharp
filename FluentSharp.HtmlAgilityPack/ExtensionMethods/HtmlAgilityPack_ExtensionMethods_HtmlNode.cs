using System.Collections.Generic;
using FluentSharp.CoreLib;

namespace FluentSharp.HtmlAgilityPacK
{
    public static class HtmlAgilityPack_ExtensionMethods_HtmlNode
    {

        public static List<string> html(this List<HtmlAgilityPack.HtmlNode> htmlNodes)
        {
            return htmlNodes.outerHtml();
        }

        public static List<string> outerHtml(this List<HtmlAgilityPack.HtmlNode> htmlNodes)
        {
            var outerHtml = new List<string>();
            foreach (var htmlNode in htmlNodes)
                outerHtml.add(htmlNode.outerHtml());
            return outerHtml;
        }

        public static string html(this HtmlAgilityPack.HtmlNode htmlNode)
        {
            return htmlNode.outerHtml();
        }

        public static string outerHtml(this HtmlAgilityPack.HtmlNode htmlNode)
        {
            return htmlNode.OuterHtml;
        }

        public static string innerHtml(this HtmlAgilityPack.HtmlNode htmlNode)
        {
            return htmlNode.InnerHtml;
        }

        public static List<string> innerHtml(this List<HtmlAgilityPack.HtmlNode> htmlNodes)
        {
            var outerHtml = new List<string>();
            foreach (var htmlNode in htmlNodes)
                outerHtml.add(htmlNode.innerHtml());
            return outerHtml;
        }

        public static string value(this HtmlAgilityPack.HtmlNode htmlNode)
        {
            return htmlNode.innerHtml();
        }

        public static List<string> values(this List<HtmlAgilityPack.HtmlNode> htmlNodes)
        {
            return htmlNodes.innerHtml();
        }

        public static string value(this HtmlAgilityPack.HtmlAttribute attribute)
        {
            return attribute.Value;
        }        
    }
}