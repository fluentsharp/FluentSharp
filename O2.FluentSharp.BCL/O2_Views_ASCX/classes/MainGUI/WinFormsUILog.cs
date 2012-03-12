// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Drawing;
using System.Windows.Forms;
using O2.Interfaces.O2Core;

namespace O2.Views.ASCX.classes.MainGUI
{
    public class WinFormsUILog : MarshalByRefObject, IO2Log
    {
        private bool _alsoShowInConsole;
        public bool alsoShowInConsole { get { return _alsoShowInConsole; } set { _alsoShowInConsole = value; } }

        public IO2Log LogRedirectionTarget { get; set; }


        private readonly string logName;

        public WinFormsUILog()
        {
        }

        public WinFormsUILog(string _logName)
        {
            logName = _logName;
        }

        #region IO2Log Members

        public void i(string infoMessage)
        {
            info(infoMessage);
        }

        public void info(string infoMessage)
        {
            DebugMsg._Info(infoMessage);
        }

        public void info(string infoMessageFormat, params Object[] variables)
        {
            DebugMsg._Info(infoMessageFormat + getLogNameText(), variables);
        }

        public void d(string debugMessage)
        {
            debug(debugMessage);
        }

        public void debug(string debugMessage)
        {
            DebugMsg._Debug(debugMessage);
        }

        public void debug(string debugMessageFormat, params Object[] variables)
        {
            DebugMsg._Debug(debugMessageFormat + getLogNameText(), variables);
        }

        public void e(string errorMessage)
        {
            error(errorMessage);
        }

        public void error(string errorMessage)
        {
            DebugMsg._Error(errorMessage);
        }

        public void error(string errorMessageFormat, params Object[] variables)
        {
            DebugMsg._Error(errorMessageFormat + getLogNameText(), variables);
        }

        public void ex(Exception exception)
        {
            DebugMsg.LogException(exception);
        }

        public void ex(Exception exception, string message)
        {
            DebugMsg.LogException(exception, message + getLogNameText());
        }

        public void ex(Exception exception, bool showStackTrace)
        {
            DebugMsg.LogException(exception, showStackTrace + getLogNameText());
        }

        public void ex(Exception exception, string message, bool showStackTrace)
        {
            DebugMsg.LogException(exception, message + getLogNameText(), showStackTrace);
        }

        public void write(string message)
        {
            DebugMsg.insertText(message, Color.Black);
        }

        public void write(string messageFormat, params object[] variables)
        {
            DebugMsg.insertText(String.Format(messageFormat, variables), Color.Black);
        }

        public void showMessageBox(string message)
        {
            DebugMsg.showMessageBox(message);
        }

        public DialogResult showMessageBox(string message, string messageBoxTitle,
                                           MessageBoxButtons messageBoxButtons)
        {
            return DebugMsg.showMessageBox(message, messageBoxTitle, messageBoxButtons);
        }

/*        public void reportCriticalErrorToO2Developers(object currentObject, Exception ex, string sourceMessage)
        {
            DebugMsg.reportCriticalErrorToO2Developers(currentObject, ex, sourceMessage);
        }*/

        public void logToChache(string text)
        {
            DebugMsg.sbLogCache.Append(text);
        }

        #endregion

        public string getLogNameText()
        {
            return (logName != null) ? "                                [... in " + logName + "]" : "";
        }
    }
}