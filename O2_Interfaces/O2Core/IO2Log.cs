// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;

namespace O2.Interfaces.O2Core
{
    public interface IO2Log
    {
        IO2Log LogRedirectionTarget { get; set; }
        bool alsoShowInConsole { get; set; }       
        void i(string infoMessage);
        void info(string infoMessage);
        void info(string infoMessageFormat, params Object[] variables);
        void d(string debugMessage);
        void debug(string debugMessage);
        void debug(string debugMessageFormat, params Object[] variables);
        void e(string errorMessage);
        void error(string errorMessage);
        void error(string errorMessageFormat, params Object[] variables);
        void ex(Exception ex);
        void ex(Exception ex, string comment);
        void ex(Exception ex, bool showStackTrace);
        void ex(Exception ex, string comment, bool showStackTrace);
        void write(string message);
        void write(string messageFormat, params Object[] variables);
        //void showMessageBox(string message);        
        //DialogResult showMessageBox(string message, string messageBoxTitle, MessageBoxButtons messageBoxButtons);
        //void reportCriticalErrorToO2Developers(object currentObject, Exception ex, String sourceMessage);
        void logToChache(string text);
        
    }
}