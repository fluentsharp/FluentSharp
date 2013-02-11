using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace O2.DotNetWrappers.ExtensionMethods
{
    public static class Xml_ExtensionMethods
    {        
        public static XmlReader     xmlReader(this string xml)
        {
            var xmlToLoad = xml.fileExists() ? xml.fileContents() : xml;
            var xmlReaderSettings = new XmlReaderSettings();
            xmlReaderSettings.XmlResolver = null;
/*#if NET_4
            xmlReaderSettings.DtdProcessing = DtdProcessing.Ignore; 
#endif
#if NET_3_5*/
            xmlReaderSettings.ProhibitDtd = false;
            
//#endif
            var stringReader = new StringReader(xmlToLoad);
            return XmlReader.Create(stringReader, xmlReaderSettings);
        }
        public static XmlDocument   xmlDocument(this string xml)
        {
            try
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.Load(xml.xmlReader());
                return xmlDocument;
            }
            catch (Exception ex)
            {
                ex.log("in xmlDocument");
                return null;
            }
        }
        public static string        xmlDocumentElement(this string xml)
        {
            try
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.Load(xml.xmlReader());
                return xmlDocument.DocumentElement.Name;
            }
            catch (Exception ex)
            {
                ex.log("in xmlDocumentElement");
                return "";
            }
        }        
        public static string        xmlFormat(this string xml)
        {
            return xml.xmlFormat(2, ' ');
        }
        public static string        xmlFormat(this string xml, int indentation, char indentChar)
        {
            var doc = new XmlDocument();
            doc.Load(xml.xmlReader());
            var stringWriter = new StringWriter();
            var xmlWriter = new XmlTextWriter(stringWriter);
            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.Indentation = indentation;
            xmlWriter.IndentChar = indentChar;
            xmlWriter.field("encoding", new UTF8Encoding()); //DC: is there another to set this			
            doc.Save(xmlWriter);
            return stringWriter.str();
        }
        public static string        xmlString(this XmlDocument xmlDocument)
        {
            var stringBuilder = new StringBuilder();
            var stringWriter = new StringWriter(stringBuilder);
            xmlDocument.Save(stringWriter);
            return stringWriter.str();
        }

        public static List<XmlAttribute>    add_XmlAttribute(this List<XmlAttribute> xmlAttributes, string name, string value)
        {
            var xmlDocument = (xmlAttributes.size() > 0) 
                                    ?  xmlAttributes[0].OwnerDocument
                                    : new XmlDocument();						
            var newAttribute = xmlDocument.CreateAttribute(name);
            newAttribute.Value = value;
            xmlAttributes.add(newAttribute);
            return xmlAttributes;
        }		
        public static string                xsl_Transform(this string xmlContent, string xstlFile)
        {
            try
            {
                var xslTransform = new XslCompiledTransform();

                xslTransform.Load(xstlFile);

                var xmlReader = new XmlTextReader(new StringReader(xmlContent));
                var xpathNavigator = new XPathDocument(xmlReader);
                var stringWriter = new StringWriter();

                xslTransform.Transform(xpathNavigator, new XsltArgumentList(), stringWriter);
                return stringWriter.str();
            }
            catch (Exception ex)
            {
                ex.log("[in xsl_Transform]");
                return "";
            }
        }
    }
}
