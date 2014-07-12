
using System.Collections.Generic;
using FluentSharp.CoreLib;


namespace FluentSharp.HtmlAgilityPacK
{
    public static class HtmlAgilityPack_ExtensionMethods_Attributes
    {
        public static List<HtmlAgilityPack.HtmlAttribute> attributes(this List<HtmlAgilityPack.HtmlNode> htmlNodes)
        {
            return htmlNodes.attributes("");
        }

        public static List<HtmlAgilityPack.HtmlAttribute> attributes(this List<HtmlAgilityPack.HtmlNode> htmlNodes, string attributeName)
        {
            var allAttributes = new List<HtmlAgilityPack.HtmlAttribute>();
            foreach (var htmlNode in htmlNodes)
                allAttributes.add(htmlNode.attributes(attributeName));
            return allAttributes;
        }

        public static List<HtmlAgilityPack.HtmlAttribute> attributes(this HtmlAgilityPack.HtmlNode htmlNode)
        {
            return htmlNode.attributes("");
        }
        public static List<HtmlAgilityPack.HtmlAttribute> attributes(this HtmlAgilityPack.HtmlNode htmlNode, string attributeName)
        {
            var attributes = new List<HtmlAgilityPack.HtmlAttribute>();
            foreach (var htmlAttribute in htmlNode.Attributes)
                if (attributeName.valid().isFalse() || htmlAttribute.Name == attributeName)
                    attributes.add(htmlAttribute);
            return attributes;
        }

        public static HtmlAgilityPack.HtmlAttribute attribute(this HtmlAgilityPack.HtmlNode htmlNode, string attributeName)
        {
            foreach (var htmlAttribute in htmlNode.Attributes)
                if (attributeName.valid().isFalse() || htmlAttribute.Name == attributeName)
                    return htmlAttribute;
            return null;
        }

        public static List<string> names(this List<HtmlAgilityPack.HtmlAttribute> htmlAttributes)
        {
            var names = new List<string>();
            foreach (var htmlAttribute in htmlAttributes)
                if (names.Contains(htmlAttribute.Name).isFalse())
                    names.add(htmlAttribute.Name);
            return names;
        }

        public static List<string> values(this List<HtmlAgilityPack.HtmlAttribute> htmlAttributes)
        {
            var values = new List<string>();
            foreach (var htmlAttribute in htmlAttributes)
                values.add(htmlAttribute.Value);
            return values;
        }

    }
}
