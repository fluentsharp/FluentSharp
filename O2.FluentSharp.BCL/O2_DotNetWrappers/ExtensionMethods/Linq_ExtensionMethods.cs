using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace O2.DotNetWrappers.ExtensionMethods
{
    public static class Linq_ExtensionMethods
    {

        //TODO: Organize these type topic/area

        public static XDocument xDocument(this string xml)
        {
            var xmlToLoad = xml.fileExists() ? xml.fileContents() : xml;
            if (xmlToLoad.valid())
            {
                if (xmlToLoad.starts("\n"))       // checks for the cases where there the text starts with \n (which will prevent the document to be loaded
                    xmlToLoad = xmlToLoad.trim();
                XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
                xmlReaderSettings.XmlResolver = null;
                xmlReaderSettings.ProhibitDtd = false;
                using (StringReader stringReader = new StringReader(xmlToLoad))
                using (XmlReader xmlReader = XmlReader.Create(stringReader, xmlReaderSettings))
                    return XDocument.Load(xmlReader);
            }
            return null;

        }

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

        public static bool hasDataForChildTreeNodes(this XElement xElement)
        {
            return xElement.Nodes().Any() ||
                   xElement.Attributes().Any() ||
                   (xElement.Nodes().Any() && xElement.Value.valid());
        }

        public static XName xName(this string name)
        {
            return XName.Get(name);
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
            xElements.forEach<XElement>((xElement) => childXElements.AddRange(xElement.elements(elementName, includeSelf)));
            return childXElements;
        }

        public static XAttribute attribute(this XElement xElement, string name)
        {
            if (xElement != null)
                return xElement.Attribute(name);
            "in XElement.attribute(...), xElement was null (name = {0})".error(name);
            return null;
        }

        public static string attributeValue(this XElement xElement, string name)
        {
            return xElement.attribute(name).value();
        }

        public static string value(this XAttribute xAttribute)
        {
            if (xAttribute != null)
                return xAttribute.Value;
            "in XAttribute.value(...), xAttribute was null".error();
            return null;
        }

        public static List<XAttribute> attributes(this XElement xElement)
        {
            return xElement.attributes("");
        }
        public static List<XAttribute> attributes(this XElement xElement, string attributeName)
        {
            return attributeName.valid()
                ? xElement.Attributes(attributeName.xName()).ToList()
                : xElement.Attributes().ToList();
        }

        public static List<XAttribute> attributes(this IEnumerable<XElement> xElements)
        {
            return xElements.attributes("");
        }

        public static List<XAttribute> attributes(this IEnumerable<XElement> xElements, string attributeName)
        {
            var attributes = new List<XAttribute>();
            xElements.forEach<XElement>((xElement) => attributes.AddRange(xElement.attributes(attributeName)));
            return attributes;
        }

        public static List<string> values(this IEnumerable<XAttribute> xAttributes)
        {
            return xAttributes.stringList();
        }
    
        public static List<string> stringList(this IEnumerable<XAttribute> xAttributes)
        {
            var stringList = new List<String>();
            xAttributes.forEach<XAttribute>((xAttribute) => stringList.Add(xAttribute.Value));
            return stringList;
        }

        public static string value(this XElement xElement)
        {
            return xElement.Value;
        }

        public static List<string> values(this IEnumerable<XElement> xElements)
        {
            var values = new List<string>();
            xElements.forEach<XElement>((xElement) => values.Add(xElement.value()));
            return values;
        }

        public static bool name(this XElement xElement, string name)
        {
            return xElement.name() == name;
        }

        public static XElement element(this XElement xElement, string elementName)
        {
            if (xElement != null)
                foreach (var childElement in xElement.elements())
                    if (childElement.name() == elementName)
                        return childElement;
            return null;
        }

        public static XElement element(this IEnumerable<XElement> xElements, string elementName)
        {
            if (xElements != null)
            {
                // first search in the current list
                foreach (var xElement in xElements)
                    if (xElement.name() == elementName)
                        return xElement;
                // then search in the current list childs
                foreach (var childElement in xElements.elements())
                    if (childElement.name() == elementName)
                        return childElement;
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

        public static XElement xElement(this XNode xNode)
        {
            if (xNode is XElement)
                return (XElement)xNode;
            return null;
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

        public static XAttribute add_Attribute(this XElement rootElement, string text, object value)
        {
            var newAttribute = new XAttribute(text.xName(), value);
            rootElement.Add(newAttribute);
            return newAttribute;
        }

        public static XAttribute attribute(this XElement elementToSearch, string name, bool createIfNotExistant)
        {
            return elementToSearch.attribute(name, createIfNotExistant, null);
        }

        public static XAttribute attribute(this XElement elementToSearch, string name, bool createIfNotExistant, object value)
        {
            var foundAttribute = elementToSearch.attribute(name);
            if (foundAttribute.notNull())
                return foundAttribute;
            return createIfNotExistant
                    ? elementToSearch.add_Attribute(name, value ?? "")
                    : null;
        }

        public static string add_xmlns(this string name, XElement xElement)
        {
            return name.prepend_AttributeValue(xElement, "xmlns");
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


        public static XAttribute value(this XAttribute xAttribute, string value)
        {
            if (xAttribute.notNull())
                xAttribute.SetValue(value);
            return xAttribute;
        }	
                
    }

    public static class XmlLinq_ExtensionMethods_ProcessingInstruction
    {
        public static XProcessingInstruction processingInstruction(this XDocument xDocument, string target)
        {
            foreach (var processingInstruction in xDocument.processingInstructions())
                if (processingInstruction.Target == target)
                    return processingInstruction;
            return null;
        }

        public static List<XProcessingInstruction> processingInstructions(this XDocument xDocument)
        {
            return xDocument.Document.Nodes().OfType<XProcessingInstruction>().toList();
        }

        public static XDocument set_ProcessingInstruction(this XDocument xDocument, string target, string data)
        {
            var processingInstruntion = xDocument.processingInstruction(target);
            if (processingInstruntion.notNull())
                processingInstruntion.Data = data;
            else
            {
                var newProcessingInstruction = new XProcessingInstruction(target, data);//"xsl-stylesheet", "type=\"text/xsl\" href=\"LogStyle.xsl\"");				
                xDocument.AddFirst(newProcessingInstruction);
            }
            return xDocument;
        }


    }
}
