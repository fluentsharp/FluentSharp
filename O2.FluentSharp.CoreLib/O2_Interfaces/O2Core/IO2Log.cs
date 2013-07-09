// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;

namespace FluentSharp.CoreLib.Interfaces
{
    public interface IO2Log
    {
        IO2Log LogRedirectionTarget { get; set; }
        bool alsoShowInConsole { get; set; }                       
        void info(string infoMessageFormat, params Object[] variables);                
        void debug(string debugMessageFormat, params Object[] variables);                
        void error(string errorMessageFormat, params Object[] variables);
        void ex(Exception ex);
        void ex(Exception ex, string comment);
        void ex(Exception ex, bool showStackTrace);
        void ex(Exception ex, string comment, bool showStackTrace);        
        void write(string messageFormat, params Object[] variables);
        void logToChache(string text);
        
    }
}