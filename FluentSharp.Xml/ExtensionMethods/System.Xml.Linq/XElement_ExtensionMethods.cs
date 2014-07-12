using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using FluentSharp.CoreLib;

namespace FluentSharp.Xml
{
    public static class XElement_ExtensionMethods
    {
        public static XElement xRoot(this string xml)
        {
            if (xml.valid())    // checks if the string is not empty
            {                
                var xDocument = xml.xDocument();
                if (xDocument != null)
                    return xDocument.Root;
            }
            return null;
        }
        public static IEnumerable<XElement> allNodes(this XElement xElement)
        {
            return xElement.DescendantsAndSelf();
        }

        public static string name(this XElement xElement)
        {
            return xElement.Name.str();
        }

        // Descendants returns all child xElements
        public static List<XElement> elementsAll(this XElement xElement)
        {
            return xElement.Descendants().ToList();
        }

        public static List<XElement> elementsAll(this XElement xElement, string name)
        {
            return xElement.Descendants(name.xName()).ToList();
        }

        public static List<XElement> elements(this XElement xElement)
        {
            return xElement.elements(false);
        }


        public static List<XElement> elements(this XElement xElement, string elementName)
        {
            return xElement.elements(elementName, false);
        }

        public static List<XElement> elements(this XElement xElement, bool includeSelf)
        {
            return xElement.elements("", includeSelf);
        }

        // Elements returns just the direct childs    	    	
        public static List<XElement> elements(this XElement xElement, string elementName, bool includeSelf)
        {
            var xElements = (elementName.valid())
                ? xElement.Elements(elementName).ToList()
                : xElement.Elements().ToList();
            if (includeSelf)
                xElements.Add(xElement);
            return xElements;
        }

        public static List<XElement> elements(this IEnumerable<XElement> xElements)
        {
            return xElements.elements(false);
        }

        public static List<XElement> elements(this IEnumerable<XElement> xElements, string elementName)
        {
            return xElements.elements(elementName, false);
        }
        public static List<XElement> elements(this IEnumerable<XElement> xElements, bool includeSelf)
        {
            return xElements.elements("", includeSelf);
        }

        public static List<XElement> elements(this IEnumerable<XElement> xElements, string elementName, bool includeSelf)
        {
            var childXElements = new List<XElement>();
            xElements.toList().forEach<XElement>((xElement) => childXElements.AddRange(xElement.elements(elementName, includeSelf)));
            return childXElements;
        }
        public static string value(this XElement xElement)
        {
            return xElement.notNull() ? xElement.Value : null;
        }

        public static List<string> values(this IEnumerable<XElement> xElements)
        {
            var values = new List<string>();
            xElements.toList().forEach<XElement>((xElement) => values.Add(xElement.value()));
            return values;
        }

        public static bool name(this XElement xElement, string name)
        {
            return xElement.name() == name;
        }

        public static XElement element(this XElement xElement, string elementName)
        {
            if (xElement != null)
                return xElement.elements().FirstOrDefault(childElement => childElement.name() == elementName);
            return null;
        }


        public static XElement element(this IEnumerable<XElement> xElements, string elementName)
        {
            if (xElements != null)
            {
                // first search in the current list
                var enumerable = xElements as IList<XElement> ?? xElements.ToList();
                foreach (var xElement in enumerable.Where(xElement => xElement.name() == elementName))
                    return xElement;
                // then search in the current list childs
                return enumerable.elements().FirstOrDefault(childElement => childElement.name() == elementName);
            }
            return null;
        }

        public static string innerXml(this XElement xElement)
        {
            if (xElement == null)
                return "";
            var reader = xElement.CreateReader();
            reader.MoveToContent();
            return reader.ReadInnerXml();
        }
        public static XElement parent(this XElement element)
        {
            return element.Parent;
        }

        public static XElement element(this XElement elementToSearch, string name, bool createIfNotExistant)
        {
            var foundElement = elementToSearch.element(name);
            if (foundElement.notNull())
                return foundElement;
            return createIfNotExistant
                ? elementToSearch.add_Element(name)
                : null;
        }

        public static XElement add_Element(this XElement rootElement, string text)
        {
            var newElement = new XElement(text);
            rootElement.Add(newElement);
            return newElement;
        }
        public static string prepend_AttributeValue(this string name, XElement xElement, string attributeName)
        {
            var xmlns = xElement.attribute(attributeName).value();
            return "{" + xmlns + "}" + name;
        }
        public static XElement innerXml(this XElement xElement, string value)
        {
            return xElement.value(value);
        }

        public static XElement value(this XElement xElement, string value)
        {
            xElement.Value = value;
            return xElement;
        }
        public static string add_xmlns(this string name, XElement xElement)
        {
            return name.prepend_AttributeValue(xElement, "xmlns");
        }        
    }
}