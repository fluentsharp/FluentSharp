using System;
using System.Collections.Specialized;
using System.Web;
using FluentSharp.WinForms.Utils;

namespace FluentSharp.WinForms
{
    public static class Web_Encoding_ExtensionMethods
    {
        public static string attributeEncode(this String stringToEncode)
        {
            return stringToEncode.htmlAttributeEncode();
        }
        public static string htmlAttributeEncode(this String stringToEncode)
        {
            return HttpUtility.HtmlAttributeEncode(stringToEncode);                        
        }
        public static NameValueCollection parseQueryString(this String queryString)
        {
            return HttpUtility.ParseQueryString(queryString);                        
        }
        public static string urlPathEncode(this String stringToEncode)
        {
            return HttpUtility.UrlPathEncode(stringToEncode);                        
        }
        public static string urlEncode(this String stringToEncode)
        {
            return HttpUtility.UrlEncode(stringToEncode);            
        }
        public static string urlDecode(this String stringToDecode)
        {
            return HttpUtility.UrlDecode(stringToDecode);            
        }
        public static string htmlEncode(this String stringToEncode)
        {
            return HttpUtility.HtmlEncode(stringToEncode);            
        }
        public static string htmlDecode(this String stringToDecode)
        {
            return HttpUtility.HtmlDecode(stringToDecode);                        
        }        
    }
}
