// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Diagnostics;
using System.Text;
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
        }

        public KO2Log(string _logHost)
        {
            logHost = _logHost;
        }

        public void setLogRedirection(IO2Log _logRedirectionTarget)
        {
            LogRedirectionTarget = _logRedirectionTarget;
        }

        #region IO2Log Members

        public void i(string infoMessage)
        {
            info(infoMessage);
        }

        public void info(string infoMessage)
        {
            info("{0}", infoMessage);
            //Debug._Info(infoMessage);            
        }

        public void info(string infoMessageFormat, params Object[] variables)
        {
            if (LogRedirectionTarget != null)
                LogRedirectionTarget.info(infoMessageFormat, variables);
            else
                writeToDebug("[Info]: " + infoMessageFormat, variables);            
        }

        public void d(string debugMessage)
        {
            debug(debugMessage);
        }

        public void debug(string debugMessage)
        {
            debug("{0}", debugMessage);            
        }

        public void debug(string debugMessageFormat, params Object[] variables)
        {
            if (LogRedirectionTarget != null)
                LogRedirectionTarget.debug(debugMessageFormat, variables);            
            else
                writeToDebug("[Debug]: " + debugMessageFormat, variables);            
        }

        public void e(string errorMessage)
        {
            error(errorMessage);
        }

        public void error(string errorMessage)
        {
            error("{0}", errorMessage);
            //DI.log.error(errorMessage);
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
            //DI.log.ex(ex);
        }

        public void ex(Exception exception, string comment)
        {
            ex(exception, comment, false);
            //DI.log.ex(ex, message);
        }

        public void ex(Exception exception, bool showStackTrace)
        {
            ex(exception, "", showStackTrace);
            //DI.log.ex(ex, showStackTrace);
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
            //DI.log.ex(ex, message,showStackTrace);
        }
        public void write(string messageFormat, params Object[] variables)
        {
            if (LogRedirectionTarget != null)
                LogRedirectionTarget.write(messageFormat, variables);
            else
                writeToDebug(messageFormat, variables);
        }

        public void write(string message)
        {
            if (LogRedirectionTarget != null)
                LogRedirectionTarget.write(message);
            else
                writeToDebug("{0}", message);
        }

/*        public virtual void showMessageBox(string message)
        {
            if (LogRedirectionTarget != null)
                LogRedirectionTarget.showMessageBox(message);
            else
                showMessageBox(message, "O2 Kernel Error", MessageBoxButtons.OK);                
        }

        public DialogResult showMessageBox(string message, string messageBoxTitle,
                                                  MessageBoxButtons messageBoxButtons)
        {
            if (LogRedirectionTarget != null)
                return LogRedirectionTarget.showMessageBox(message, messageBoxTitle, messageBoxButtons);

            return MessageBox.Show(message, messageBoxTitle, messageBoxButtons); 
        }*/

/*        public virtual void reportCriticalErrorToO2Developers(object currentObject, Exception ex, string sourceMessage)
        {
            if (LogRedirectionTarget != null)
                LogRedirectionTarget.reportCriticalErrorToO2Developers(currentObject, ex, sourceMessage);
            else
            {
                var errorMessage = new StringBuilder();
                errorMessage.AppendLine("KLOG ERROR: in reportCriticalErrorToO2Developers: " + sourceMessage + " \n\n");
                if (currentObject != null)
                    errorMessage.AppendFormat("current Object: {0}\n\n", currentObject);
                error("source message {0}", sourceMessage);
                if (ex != null)
                {
                    errorMessage.AppendFormat("Exception message: {0}\n\n", ex.Message);
                    errorMessage.AppendFormat("Exception StackTrace: {0}\n\n", ex.StackTrace);
                    if (ex.InnerException != null)
                        errorMessage.AppendFormat("   Inner Exception message: {0}\n\n", ex.InnerException.Message);
                }
                showMessageBox(errorMessage.ToString());
            }
        }*/

        public void logToChache(string text)
        {
            info("[logToChache] {0}",text);
        }

        #endregion

        private void writeToDebug(string messageFormat, params Object[] variables)
        {
            var message = string.Format(messageFormat, variables);
            if (messageFormat.IndexOf("[Error]") > -1)
                Debug.WriteLine("[O2 Kernel msg][ERROR RECEIVED]*******:");
            Debug.WriteLine("[O2 Kernel msg]" + message);

            if (alsoShowInConsole)
                Console.WriteLine(message);
                //+((logHost != "") ? "                             ... in " + logHost : ""));
        }
    }
}
