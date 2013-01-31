// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Diagnostics;
using System.Text;
using O2.DotNetWrappers.ExtensionMethods;
using O2.Interfaces.O2Core;

namespace O2.Kernel.InterfacesBaseImpl
{
    public class KO2Log : IO2Log
    {
        public static string logHost = "";

        public bool alsoShowInConsole { get; set; }

        public IO2Log LogRedirectionTarget { get; set; }
        
        public KO2Log()
        {
			//LogRedirectionTarget = new Logger_Memory();		  // default to Log to memory
            LogRedirectionTarget = new Logger_DiagnosticsDebug(); // log to diagnostics
        }

        public KO2Log(string _logHost)
        {
            logHost = _logHost;
        }

        public void setLogRedirection(IO2Log _logRedirectionTarget)
        {
            LogRedirectionTarget = _logRedirectionTarget;
        }
        public void info(string infoMessageFormat, params Object[] variables)
        {
            if (LogRedirectionTarget != null)
                LogRedirectionTarget.info(infoMessageFormat, variables);
            else
                writeToDebug("[Info]: " + infoMessageFormat, variables);            
        }
        public void debug(string debugMessageFormat, params Object[] variables)
        {
            if (LogRedirectionTarget != null)
                LogRedirectionTarget.debug(debugMessageFormat, variables);            
            else
                writeToDebug("[Debug]: " + debugMessageFormat, variables);            
        }
        public void error(string errorMessageFormat, params Object[] variables)
        {
            if (LogRedirectionTarget != null)
                LogRedirectionTarget.error(errorMessageFormat, variables);
            else
                writeToDebug("[Error]: " + errorMessageFormat, variables);            
        }
        public void ex(Exception exception)
        {
            ex(exception, "", false);
            //PublicDI.log.ex(ex);
        }
        public void ex(Exception exception, string comment)
        {
            ex(exception, comment, false);
            //PublicDI.log.ex(ex, message);
        }
        public void ex(Exception exception, bool showStackTrace)
        {
            ex(exception, "", showStackTrace);
            //PublicDI.log.ex(ex, showStackTrace);
        }
        public void ex(Exception exception, string comment, bool showStackTrace)
        {
            if (LogRedirectionTarget != null)
                LogRedirectionTarget.ex(exception, comment, showStackTrace);
            else
            {
                string exceptionFormat = "[Exception]: {0} " +
                                         ((comment != "") ? Environment.NewLine + "Comment: {1} " : "") +
                                         ((showStackTrace) ? Environment.NewLine + "StackTrace: {2} " : "");
                if (exception.InnerException != null)
                {
                    exceptionFormat += "\n   InnerException: " + exception.InnerException.Message;
                    if (exception.InnerException.InnerException != null)
                        exceptionFormat += "\n   InnerException.InnerException: " + exception.InnerException.InnerException.Message;
                }
                writeToDebug(exceptionFormat, exception.Message, comment, exception.StackTrace);
            }
            //PublicDI.log.ex(ex, message,showStackTrace);
        }
        public void write(string messageFormat, params Object[] variables)
        {
            if (LogRedirectionTarget != null)
                LogRedirectionTarget.write(messageFormat, variables);
            else
                writeToDebug(messageFormat, variables);
        }
        public void logToChache(string text)
        {
            info("[logToChache] {0}",text);
        }
        private void writeToDebug(string messageFormat, params Object[] variables)
        {
            var message = messageFormat.format(variables);
            if (messageFormat.IndexOf("[Error]") > -1)
                Debug.WriteLine("[O2 Kernel msg][ERROR RECEIVED]*******:");
            Debug.WriteLine("[O2 Kernel msg]" + message);

            if (alsoShowInConsole)
                Console.WriteLine(message);
                //+((logHost != "") ? "                             ... in " + logHost : ""));
        }
    }
}
