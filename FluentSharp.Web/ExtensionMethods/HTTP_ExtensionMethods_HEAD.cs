using System;
using System.Net;
using System.Web;
using FluentSharp.CoreLib;

namespace FluentSharp.Web35
{
    public static class HTTP_ExtensionMethods_HEAD
    {
        //HEAD requests
        public static WebHeaderCollection HEAD_Headers(this Uri uri)
        {
            var request = (HttpWebRequest) WebRequest.Create(uri);
            request.Timeout = FluentSharp_Consts.TIMEOUT_HTTP_HEAD_REQUESTS;
            request.AllowAutoRedirect = false;            
            request.Method = "HEAD";
            try
            {
                using(var response = request.GetResponse())
                {
                    return response.Headers;
                }                
            }
            catch (WebException)
            {                
                return null;
            }
        }
        /// <summary>
        /// Returns the HttpStatusCode of the provided url. If an HTTP exception occurs, then it will be handeled
        /// and the corresponding HttpStatusCode returned
        /// </summary>
        /// <param name="url">if this is not valid (or not an uri) the default(HttpStatusCode) is returned</param>
        /// <returns>HttpStatusCode</returns>
        public static HttpStatusCode           HEAD_StatusCode(this string url)
        {
            if(url.isNotUri())
                return default(HttpStatusCode);

            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Timeout = FluentSharp_Consts.TIMEOUT_HTTP_HEAD_REQUESTS;
            webRequest.Method = "HEAD";
            try
            {
                using(var webResponse = (HttpWebResponse)webRequest.GetResponse())
                {                
                    return webResponse.StatusCode;
                }
            }
            catch (WebException webException)
            {
                using(var webResponse = (HttpWebResponse)webException.Response)
                { 
                    return webResponse.StatusCode;
                }
            }            
        }
        /// <summary>
        /// Makes a HEAD request and returns true if successfull
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static bool                HEAD(this Uri uri)
        {
            return uri.HEAD_Headers().notNull();
        }
        public static bool                httpFileExists(this string url)
        {
            return url.httpFileExists(false);
        }
        /// <summary>
        /// Makes a HEAD request and returns true if successfull (uri passed as string       
        /// </summary>
        /// <param name="url">note that the  webResponse.ResponseUri.str() will be used to confirm the validity of the request.
        /// This is done to deal with Web Proxys (like wifi logins), but can cause false negative if the server does a redirect or URL rewrite</param>
        /// <param name="showError">writes the </param>
        /// <returns></returns>
        public static bool                httpFileExists(this string url, bool showError)
        {            
            if (url.empty() || url.isNotUri())
                return false;

            var webRequest               = (HttpWebRequest)WebRequest.Create(url);            
            webRequest.Timeout           = FluentSharp_Consts.TIMEOUT_HTTP_HEAD_REQUESTS;
            webRequest.Method            = "HEAD";
            try
            {
                using(var webResponse = (HttpWebResponse)webRequest.GetResponse())
                { 
                    var statusCode  = webResponse.StatusCode;
                    var responseUrl = webResponse.ResponseUri.str();                
                    return (statusCode == HttpStatusCode.OK && responseUrl== url);
                }
            }
            catch (Exception ex)
            {
                if (showError)
                    ex.log("in Web.httpFileExists");
                if (ex.Message.contains("SSL"))
                    ex.log("in Web.httpFileExists ({0}) got SSL error: {1}".format(url, ex.Message));
                return false;
            }
        }

        public static bool      exists(this Uri uri)
        {
            return uri.str().httpFileExists();
        }
    }
}