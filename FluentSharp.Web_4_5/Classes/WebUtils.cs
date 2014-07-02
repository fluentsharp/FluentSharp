using System;
using FluentSharp.CoreLib;
using FluentSharp.Web35;

namespace FluentSharp.Web
{
    public class WebUtils
    {
        public static Uri googleIpAddress = "http://173.194.66.99".uri();
        public static bool online()
        {
            return googleIpAddress.HEAD();
        }
        public static bool offline()
        {
            return online().isFalse();
        }

        public static bool runningOnLocalHost()
        {
            try
            {
                if (HttpContextFactory.Context.notNull() && HttpContextFactory.Request.notNull())                
                    return HttpContextFactory.Request.IsLocal;
            }
            catch (Exception ex)
            {
                ex.log("[runningOnLocalHost");                
            }
            return true;        //default to true
        }
    }
}
