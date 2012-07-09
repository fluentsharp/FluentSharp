using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using O2.DotNetWrappers.ExtensionMethods;
using System.Data;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Reflection;

namespace O2.External.SharpDevelop.ExtensionMethods
{
    public static class XmlToXsd_ExtensionMethods
    {        
        #region XML to XSD to Assembly

        public static string xmlCreateXSD(this string xml)
        {
            try
            {
                "Creating XSD from XML".info();
                var dataSet = new DataSet();
                dataSet.ReadXml(xml.xmlReader());
                var stringWriter = new StringWriter();
                XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
                xmlTextWriter.Formatting = Formatting.Indented;
                xmlTextWriter.field("encoding", new UTF8Encoding());	//DC: is there another to set this
                //xmlTextWriter.WriteStartDocument();
                dataSet.WriteXmlSchema(xmlTextWriter);
                xmlTextWriter.Close();
                stringWriter.Close();
                return stringWriter.ToString();
            }
            catch (Exception ex)
            {
                ex.log("in createXSDfromXmlFile");
                return "";
            }
        }

        public static string xmlCreateCSharpFile(this string xmlFile)
        {
            var xsd = xmlFile.xmlCreateXSD();
            if (xsd.valid())
                return xsd.xsdCreateCSharpFile();
            return "";
        }

        public static Assembly xmlCreateAssembly(this string xmlFile)
        {
            var xsd = xmlFile.xmlCreateXSD();
            if (xsd.valid())
                return xsd.xsdCreateAssembly();
            return null;
        }

        public static object xmlCreateObject(this string xml)
        {
            var assembly = xml.xmlCreateAssembly();
            return assembly.xsdAssemblyGetObject(xml);
        }

        public static object xsdAssemblyGetObject(this Assembly assembly, string xml)
        {
            "finding document element".info();
            var documentElement = xml.xmlDocumentElement();
            "finding document element to asssembly types".info();
            foreach (var type in assembly.xsdAssemblyGetRootTypes())
                if (type.name() == documentElement.replaceAllWith("", "-"))
                    return type.xmlTypeGetObject(xml);
            "was not able to map the document element ({0})to an asssembly type".format(documentElement).error();
            return null;
        }

        public static object xmlTypeGetObject(this Type type, string xml)
        {
            "Creating Object from Xml".debug();
            return type.invokeStatic(
                "Parse",
                (xml.isFile()) ? xml.fileContents() : xml);
        }

        public static string xsdCreateCSharpFile(this string xsdFile)
        {
            if (xsdFile.fileExists().isFalse())
                xsdFile = xsdFile.save();
            if (xsdFile.fileExists().isFalse())
                return "";
            var targetCsFile = xsdFile.extensionChange(".cs");
            return xsdFile.xsdCreateCSharpFile(targetCsFile);
        }

        public static string xsdCreateCSharpFile(this string xsdFile, string targetCsFile)
        {
            try
            {
                if (xsdFile.fileExists().isFalse())
                    xsdFile = xsdFile.save();
                if (xsdFile.fileExists().isFalse())
                    return "";
                "Creating CSharp from XSD: {0}".format(xsdFile).info();
                XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
                //XmlReaderSettings settings = new XmlReaderSettings();
                //settings.XmlResolver = null;
                //ettings.ProhibitDtd = true;
                //var xmlReader = XmlReader.Create(xsdFile, settings);
                xmlSchemaSet.Add(null, xsdFile.xmlReader());
                string csFileName = targetCsFile;
                string targetAssembly = String.Empty;  // don't use their compile method
                string configFileName = null;
                bool xmlSerializable = false;
                bool nameMangler2 = false;
                var xObjects = "O2_Misc_Microsoft_MPL_Libs.dll".type("XObjectsGenerator");
                xObjects.invokeStatic("GenerateXObjects", xmlSchemaSet, csFileName, configFileName, targetAssembly, xmlSerializable, nameMangler2);
                //xsdFile.fromXsdCreateCSFileAndAssembly(targetCsFile,string.Empty);

                if (targetCsFile.fileExists())
                    return targetCsFile;
            }
            catch (Exception ex)
            {
                ex.log("in xsdCreateCSharpFile");
            }
            return "";
        }

        public static Assembly xsdCreateAssembly(this string xsd)
        {
            var targetAssembly = (xsd.fileExists())
                                    ? xsd.extensionChange(".dll")
                                    : xsd.save().extensionChange(".dll");
            return xsd.xsdCreateAssembly(targetAssembly);
        }

        public static Assembly xsdCreateAssembly(this string xsdFile, string targetAssembly)
        {
            //"Creating Assembly from XSD".info();
            var csFile = xsdFile.xsdCreateCSharpFile(targetAssembly);
            "Compiling CSharp File".info();
            if (csFile.fileExists())
                return csFile.compile(targetAssembly);
            return null;
        }

        public static List<Type> xsdCsFileGetRootTypes(this string csFile)
        {
            var assembly = (csFile.fileExists())
                                ? csFile.compile()
                                : csFile.save().compile();
            return (assembly != null)
                ? assembly.xsdAssemblyGetRootTypes()    
                : null;
        }

        public static List<Type> xsdAssemblyGetRootTypes(this Assembly assembly)
        {
            var rootTypes = new List<Type>();
            foreach (var property in assembly.type("XRoot").properties())
                if (property.Name.neq("XDocument"))
                    rootTypes.Add(property.propertyType());
            return rootTypes;
        }

        #endregion
    }
}
