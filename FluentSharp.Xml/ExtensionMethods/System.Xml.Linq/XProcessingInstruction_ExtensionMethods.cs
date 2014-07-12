using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using FluentSharp.CoreLib;

namespace FluentSharp.Xml
{
    public static class XProcessingInstruction_ExtensionMethods
    {
        public static XProcessingInstruction processingInstruction(this XDocument xDocument, string target)
        {
            return xDocument.processingInstructions().FirstOrDefault(processingInstruction => processingInstruction.Target == target);
        }

        public static List<XProcessingInstruction> processingInstructions(this XDocument xDocument)
        {
            if (xDocument.notNull() && xDocument.Document.notNull())            
                return xDocument.Document.Nodes().OfType<XProcessingInstruction>().toList();
            return default(List<XProcessingInstruction>);
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