using System.IO;
using System.Xml;
using System.Xml.Linq;
using FluentSharp.CoreLib;

namespace FluentSharp.Xml
{
    public static class XDocument_ExtensionMethods
    {
        public static XDocument xDocument(this string xml)
        {
            var xmlToLoad = xml.fileExists() ? xml.fileContents() : xml;
            if (xmlToLoad.valid())
            {
                if (xmlToLoad.starts("\n"))       // checks for the cases where there the text starts with \n (which will prevent the document to be loaded
                    xmlToLoad = xmlToLoad.trim();
                var xmlReaderSettings = new XmlReaderSettings {XmlResolver = null, ProhibitDtd = false};
                using (var stringReader = new StringReader(xmlToLoad))
                using (var xmlReader = XmlReader.Create(stringReader, xmlReaderSettings))
                    return XDocument.Load(xmlReader);
            }
            return null;
        }
    }
}