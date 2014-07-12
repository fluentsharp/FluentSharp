using System;
using System.IO;
using System.Xml;
using FluentSharp.CoreLib;
using FluentSharp.Xml;

namespace FluentSharp.HtmlAgilityPacK
{
    public static class HtmlAgilityPack_ExtensionMethods_Xml
    {
        public static string htmlToXml(this string htmlCode)
        {
            return htmlCode.htmlToXml(true);
        }

        public static string htmlToXml(this string htmlCode, bool xmlFormat)
        {
            try
            {
                var stringWriter = new StringWriter();
                var xmlWriter = XmlWriter.Create(stringWriter);
                xmlWriter.Flush();
                var htmlDocument = htmlCode.htmlDocument();

                htmlDocument.Save(xmlWriter);
                if (xmlFormat)
                    return stringWriter.str().xmlFormat();
                return stringWriter.str();
            }
            catch (Exception ex)
            {
                ex.log("[string.htmlToXml]");
                return ex.Message;
            }
        }
        
		
		public static string tidyHtml(this HtmlAgilityPack.HtmlDocument htmlDocument)
		{
			try
			{	
				//htmlDocument.OptionCheckSyntax = true;
				htmlDocument.OptionFixNestedTags = true;
				htmlDocument.OptionAutoCloseOnEnd = true;
				htmlDocument.OptionOutputAsXml = true;				
				//htmlDocument.OptionDefaultStreamEncoding = Encoding.Default;
                var documentNode = htmlDocument.DocumentNode;
                                
                if (documentNode.InnerHtml == documentNode.InnerText)        //nothing to do since there are no Html tags
                    return documentNode.InnerHtml.fix_CRLF(); 
                                
				var formatedCode = documentNode.OuterHtml
                                               .xmlFormat()
											   .xRoot()
                                               .innerXml()
                                               .trim();
				return formatedCode.fix_CRLF();			
			}
        	catch(Exception ex)
        	{
        		ex.log("[string.tidyHtml]");
        		return null;
        	}
		}
        
        public static string tidyHtml(this string htmlCode)
        {
            var htmlDocument = htmlCode.htmlDocument();
            var tidiedhtml = htmlDocument.tidyHtml();
            if (tidiedhtml.valid())
                return tidiedhtml;
            return htmlCode;
        }
    }
}
