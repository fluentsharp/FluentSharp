using System;
using System.Web;
using FluentSharp.CoreLib;

namespace FluentSharp.Web
{
    public static class HttpContextFactory_ExtensionMethods
    {
        //Context
        public static HttpContextBase addCookieFromResponseToRequest(this HttpContextBase    httpContext, string cookieName)
        {
            if (httpContext.Response.hasCookie(cookieName))
            {
                var cookieValue = httpContext.Response.cookie(cookieName);
                if (httpContext.Request.hasCookie(cookieName))
                    httpContext.Request.Cookies[cookieName].value(cookieValue);
                else
                {
                    var newCookie = new HttpCookie(cookieName, cookieValue);
                    httpContext.Request.Cookies.Add(newCookie);
                }
            }
            return httpContext;
        }

        public static string serverUrl(this HttpContextBase context)
        {
            try
            {
                if (context.notNull())
                {                    
                    var request = context.Request;                                        
                    var serverName = request.ServerVariables["Server_Name"];
                    var serverPort = request.ServerVariables["Server_Port"];
                    if (serverName.valid() && serverPort.valid())
                    {
                        if (serverPort == "80")
                            return "http://{0}".format(serverName);
                        if (serverPort == "443")
                            return "https://{0}".format(serverName);
                        
                        var scheme = request.IsSecureConnection ? "https" : "http";
                        return "{0}://{1}:{2}".format(scheme, serverName, serverPort);
                    }
                    //return "{0}://localhost".format(scheme);
                }
            }
            catch (Exception ex)
            {
                ex.log("[HttpContextBase] serverUrl");
            }
            return "";
        }

        //Session
        public static string    sessionId(this HttpSessionStateBase sessionState)
        {
            return sessionState.notNull() 
                ? sessionState.SessionID 
                : "";
        }

        //Request
        public static string    ipAddress(this HttpRequestBase request)
        {            
            return request.notNull() 
                ? request.UserHostAddress 
                : "";
        }
        public static bool      isLocal  (this HttpRequestBase request)
        {
            return  request.isNull() || request.IsLocal;
        }
        public static string    referer  (this HttpRequestBase httpRequest)
        {
            if (httpRequest.notNull() && httpRequest.UrlReferrer.notNull())
                return  httpRequest.UrlReferrer.str();
            return "";
        }
        public static string    url      (this HttpRequestBase request)
        {
            if (request.notNull() && request.Url.notNull())
                return  request.Url.str();
            return "";
        } 
        
        public static int       request_Value_Int(this string variableName, int defaultValue = -1)
        {
            return  HttpContextFactory.Request.value_Int(variableName, defaultValue);
        }
        public static int    value_Int(this HttpRequestBase request, string variableName, int defaultValue)
        {
            var value = request.value_String(variableName);
            if (value.valid() && value.isInt())
                return value.toInt();
            return defaultValue;
        }
        public static string    request_Value_String(this string variableName, string defaultValue = "")
        {
            return  HttpContextFactory.Request.value_String(variableName, defaultValue);
        }
        public static string    value_String(this HttpRequestBase request, string variableName, string defaultValue = "")
        {
            if (request.notNull())
                if(request[variableName].notNull())                
                    return request[variableName];                
            return defaultValue;
        }
            
        
        //Cache                
        public static bool            sent304Redirect(this HttpContextBase context)
        {
            try
            {
                if (context.send304Redirect())
                {
                    context.Response.StatusCode = 304;
                    context.Response.StatusDescription = "Not Modified ('\"<b>Your friendly TM Bot</b>'\")";
                    return true;
                }
                context.setCacheHeaders();		    
            }
            catch(Exception ex)
            {
                ex.log("[HttpContextBase] [sent304Redirect]");
            }
            return false;
        }        
        public static bool            send304Redirect(this HttpContextBase context)
        {            
            var ifModifiedSinceHeader = context.Request.Headers["If-Modified-Since"];
            if (ifModifiedSinceHeader.valid() && ifModifiedSinceHeader.isDate())
            {
                var ifModifiedSinceDate = DateTime.Parse(ifModifiedSinceHeader);
                if (HttpContextFactory.LastModified_HeaderDate.str() == ifModifiedSinceDate.str())
                    return true;
            }
            return false;
        }
        public static HttpContextBase setCacheHeaders(this HttpContextBase context)
        {	
            context.Response.Cache.SetLastModified(HttpContextFactory.LastModified_HeaderDate);
            return context;
        }


    }
}