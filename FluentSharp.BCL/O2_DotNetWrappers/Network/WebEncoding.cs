using System.Web;

namespace FluentSharp.BCL.Utils
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
