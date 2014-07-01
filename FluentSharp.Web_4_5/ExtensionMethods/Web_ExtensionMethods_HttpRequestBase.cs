using System.Web;
using FluentSharp.CoreLib;

namespace FluentSharp.WinForms.ExtensionMethods.Web
{
    public static class Web_ExtensionMethods_HttpRequestBase
    {
         public static bool hasHeader(this HttpRequestBase response, string headerName)
        {
            return response.notNull() &&
                   response.Headers.notNull() &&
                   response.Headers[headerName].notNull();
        }

        public static string header(this HttpRequestBase request, string headerName)
        {
            if (request.hasHeader(headerName))
                return request.Headers[headerName];
            return null;                       
        } 
    }
}
