using System.Net;

namespace FluentSharp.NUnit
{
    public static class NUnitTests_ExtensionMethods_Web
    {
        public static HttpStatusCode assert_Http_OK(this HttpStatusCode statusCode)
        {
            return statusCode.assert_HttpStatusCode(HttpStatusCode.OK);
            
        }
        public static HttpStatusCode assert_Http_NotFound(this HttpStatusCode statusCode)
        {
            return statusCode.assert_HttpStatusCode(HttpStatusCode.NotFound);            
        }
        public static HttpStatusCode assert_HttpStatusCode(this HttpStatusCode statusCode, HttpStatusCode expectedStatus)
        {
            statusCode.assert_Equal_To(expectedStatus);
            return statusCode;
        }
        
    }
}