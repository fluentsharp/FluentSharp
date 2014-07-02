using System;
using FluentSharp.CoreLib;
using FluentSharp.Web35.API;

namespace FluentSharp.Web35
{
    public static class HTTP_ExtensionMethods_POST
    {
        public static string POST(this Uri uri, byte[] postData)
        {
            return uri.get_Html(postData);
        }
        public static string POST(this Uri uri, string postData)
        {
            return uri.get_Html(postData);
        }
        public static string POST(this string url, string postData)
        {
            return url.html(postData);
        }
        public static string html(this string url, string postData)
        {
            return url.get_Html(postData);
        }
        public static string get_Html(this string url, string postData)
        {
            if (url.isUri())
                return url.uri().get_Html(postData);
            "in get_Html (POST), url provided was not  valid URI: {0}".error(url);
            return null;
        }
        public static string get_Html(this Uri url, string postData)
        {
            return new Web().getUrlContents_POST(url.str(), postData);
        }
        public static string get_Html(this Uri url, byte[] postData)
        {
            return new Web().getUrlContents_POST(url.str(), postData);
        }
    }
}