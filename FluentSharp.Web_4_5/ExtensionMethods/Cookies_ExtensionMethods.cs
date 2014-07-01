using System.Web;
using FluentSharp.CoreLib;

namespace FluentSharp.Net40
{
    public static class Extra_ExtensioMethods_Cookies
    {
        public static HttpCookie value(this HttpCookie httpCookie, string newValue)
        {
            if (httpCookie.notNull())
                httpCookie.Value = newValue;
            return httpCookie;
        }

        public static string value(this HttpCookie httpCookie)
        {
            return httpCookie.notNull()
                       ? httpCookie.Value
                       : null;
        }

        public static bool hasCookie(this HttpResponseBase response, string cookieName)
        {
            return response.notNull() &&
                   response.Cookies.notNull() &&
                   response.Cookies[cookieName].notNull();
        }

        public static bool hasCookie(this HttpRequestBase response, string cookieName)
        {
            return response.notNull() &&
                   response.Cookies.notNull() &&
                   response.Cookies[cookieName].notNull();
        }

        public static string cookie(this HttpResponseBase response, string cookieName)
        {
            if (response.hasCookie(cookieName))
                return response.Cookies[cookieName].value();
            return null;
        }

        public static HttpCookie set_Cookie(this HttpResponseBase response, string name, string value)
        {
            if (response.isNull())
                return null;
            var httpCookie = (response.hasCookie(name))
                                 ? response.Cookies[name]
                                 : new HttpCookie(name);
            if (httpCookie.isNull())
                return null;
            httpCookie.Value = value;

            if ((response.hasCookie(name).isFalse()))
                response.Cookies.Add(httpCookie);
            return httpCookie;
        }

        public static HttpCookie set_Cookie(this HttpRequestBase request, string name, string value)
        {
            if (request.isNull())
                return null;
            var httpCookie = (request.hasCookie(name))
                                 ? request.Cookies[name]
                                 : new HttpCookie(name);
            if (httpCookie.isNull())
                return null;
            httpCookie.Value = value;

            if ((request.hasCookie(name).isFalse()))
                request.Cookies.Add(httpCookie);
            return httpCookie;
        }

        public static HttpCookie httpOnly(this HttpCookie httpCookie)
        {
            return httpCookie.httpOnly(true);
        }

        public static HttpCookie httpOnly(this HttpCookie httpCookie, bool value)
        {
            if (httpCookie.notNull())
                httpCookie.HttpOnly = value;
            return httpCookie;
        }

    }
}