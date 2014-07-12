using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using FluentSharp.CoreLib;

namespace FluentSharp.Xml
{
    public static class XAttribute_ExtensionMethods
    {        
        public static List<string> values(this IEnumerable<XAttribute> xAttributes)
        {
            return xAttributes.stringList();
        }
    
        public static List<string> stringList(this IEnumerable<XAttribute> xAttributes)
        {
            var stringList = new List<string>();
            xAttributes.toList().forEach<XAttribute>((xAttribute) => stringList.Add(xAttribute.Value));
            return stringList;
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
            xElements.toList().forEach<XElement>((xElement) => attributes.AddRange(xElement.attributes(attributeName)));
            return attributes;
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
        public static XAttribute value(this XAttribute xAttribute, string value)
        {
            if (xAttribute.notNull())
                xAttribute.SetValue(value);
            return xAttribute;
        }
    }
}