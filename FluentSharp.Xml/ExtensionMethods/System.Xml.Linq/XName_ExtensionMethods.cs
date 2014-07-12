using System.Xml.Linq;

namespace FluentSharp.Xml
{
    public static class XName_ExtensionMethods
    {        
        public static XName xName(this string name)
        {
            return XName.Get(name);
        }
    }
}