using System;
using FluentSharp.BCL.Utils;

namespace FluentSharp.BCL
{
    public static class Web_Encoding_ExtensionMethods
    {
        public static string urlEncode(this String stringToEncode)
        {
            return WebEncoding.urlEncode(stringToEncode);
        }
        public static string urlDecode(this String stringToEncode)
        {
            return WebEncoding.urlDecode(stringToEncode);
        }
        public static string htmlEncode(this String stringToEncode)
        {
            return WebEncoding.htmlEncode(stringToEncode);
        }
        public static string htmlDecode(this String stringToEncode)
        {
            return WebEncoding.htmlDecode(stringToEncode);
        }        
    }
}
