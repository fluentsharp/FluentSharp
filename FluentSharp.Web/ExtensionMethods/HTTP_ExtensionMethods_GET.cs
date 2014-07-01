using System;

namespace FluentSharp.CoreLib
{
    public static class HTTP_ExtensionMethods_GET
    {
        public static string GET(this Uri uri)
        {
            return uri.get_Html();
        }
        public static string GET(this string url)
        {
            return url.get_Html();
        }
        public static string html(this string url)
        {
            return url.get_Html();
        }
        public static string get_Html(this string url)
        {
            if (url.isUri())
                return url.uri().get_Html();
            "in get_Html (GET), url provided was not valid URI: {0}".error(url);
            return null;
        }
        public static string get_Html(this Uri url)	// this is a better way to represent it
        {
            return url.getHtml();
        }

    }
}