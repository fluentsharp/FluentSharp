using System.Xml.Linq;

namespace FluentSharp.Xml
{
    public static class XNode_ExtensionMethods
    {
        public static XElement xElement(this XNode xNode)
        {
            var node = xNode as XElement;
            if (node != null)
                return node;
            return null;
        }        
    }
}
