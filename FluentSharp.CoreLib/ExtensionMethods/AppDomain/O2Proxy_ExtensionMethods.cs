using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentSharp.CoreLib.API;

namespace FluentSharp.CoreLib
{
    public static class O2Proxy_ExtensionMethods
    {
        public static  O2Proxy o2Proxy(this AppDomain appDomain, bool copyToAppDomainBaseDirectoryBeforeLoad = false)
        {
            appDomain.load_FluentSharp_CoreLib(copyToAppDomainBaseDirectoryBeforeLoad);   
            var o2Proxy = (O2Proxy)appDomain.getProxyObject("O2Proxy FluentSharp.CoreLib");      
            return o2Proxy;
        }

        public static List<string> assemblies(this O2Proxy o2Proxy)
        {
            return (o2Proxy.notNull())
                      ? o2Proxy.getAssemblies()
                      : new List<string>();
        }
    }
}
