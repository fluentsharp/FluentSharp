using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace O2.DotNetWrappers.Network
{
    public class WebEncoding
    {
        public static string urlDecode(string url)
        {
            return HttpUtility.UrlDecode(url);
        }

        public static string urlEncode(string url)
        {
            return HttpUtility.UrlEncode(url);
        }

        public static string htmlEncode(string html)
        {
            return HttpUtility.HtmlEncode(html);
        }

        public static string htmlDecode(string html)
        {
            return HttpUtility.HtmlDecode(html);
        }
    }
}
