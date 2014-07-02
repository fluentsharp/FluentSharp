using System;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.Web35.API;

namespace FluentSharp.Web35
{
    public static class Web_ExtensionMethods_Ping
    {     
        public static bool      ping(this string address)
        {
            return new Ping().ping(address);
        }
        public static bool      online(this object _object)
        {
            return ping("www.google.com") ||                                    // first ping, 
                   "https://www.google.com/favicon.ico".httpFileExists();       // then try making a HEAD connection                                                                   
        }     
        public static bool      offline(this object _object)
        {
            return _object.online().isFalse();            
        }
        
    }
}