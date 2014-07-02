using System.Web;
using FluentSharp.CoreLib;

namespace FluentSharp.Web
{
    public static class HttpRequestBase_ExtensionMethods
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
