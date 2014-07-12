using System.Collections.Generic;
using FluentSharp.CoreLib;

namespace FluentSharp.HtmlAgilityPacK
{
    public static class HtmlAgilityPack_ExtensionMethods_HtmlDocument
    {        
        public static HtmlAgilityPack.HtmlDocument htmlDocument(this string htmlCode)
        {
            var htmlDocument = new HtmlAgilityPack.HtmlDocument();
            htmlDocument.LoadHtml(htmlCode);
            return htmlDocument;
        }

        public static string html(this HtmlAgilityPack.HtmlDocument htmlDocument)
        {
            return htmlDocument.DocumentNode.OuterHtml;
        }

        public static List<HtmlAgilityPack.HtmlNode> filter(this HtmlAgilityPack.HtmlDocument htmlDocument, string query)
        {
            return htmlDocument.select(query);
        }

        public static List<HtmlAgilityPack.HtmlNode> select(this HtmlAgilityPack.HtmlDocument htmlDocument, string query)
        {
            return htmlDocument.DocumentNode.SelectNodes(query).toList();
        }

        public static List<HtmlAgilityPack.HtmlNode> links(this HtmlAgilityPack.HtmlDocument htmlDocument)
        {
            return htmlDocument.select("//a");
        }

        
    }
}