// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Drawing;
using System.Windows.Forms;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.Interfaces;
using FluentSharp.WinForms.Utils;

namespace FluentSharp.WinForms.Controls
{
    public class WinFormsUILog : MarshalByRefObject, IO2Log
    {        
        public bool     alsoShowInConsole    { get; set; }
        public IO2Log   LogRedirectionTarget { get; set; }
        public string   LogName              { get; set; }

        public WinFormsUILog()
        {
        }
        public WinFormsUILog(string logName)
        {
            LogName = logName;
        }
        
        public void         info(string infoMessageFormat, params Object[] variables)
        {
            DebugMsg._Info(infoMessageFormat + getLogNameText(), variables);
        }
        public void         debug(string debugMessageFormat, params Object[] variables)
        {
            DebugMsg._Debug(debugMessageFormat + getLogNameText(), variables);
        }
        public void         error(string errorMessageFormat, params Object[] variables)
        {
            DebugMsg._Error(errorMessageFormat + getLogNameText(), variables);
        }
        public void         ex(Exception exception)
        {
            DebugMsg.LogException(exception);
        }
        public void         ex(Exception exception, string message)
        {
            DebugMsg.LogException(exception, message + getLogNameText());
        }
        public void         ex(Exception exception, bool showStackTrace)
        {
            DebugMsg.LogException(exception, showStackTrace + getLogNameText());
        }
        public void         ex(Exception exception, string message, bool showStackTrace)
        {
            DebugMsg.LogException(exception, message + getLogNameText(), showStackTrace);
        }
        public void         write(string messageFormat, params object[] variables)
        {
            DebugMsg.insertText(messageFormat.format(variables), Color.Black);
        }
        public void         showMessageBox(string message)
        {
            DebugMsg.showMessageBox(message);
        }
        public DialogResult showMessageBox(string message, string messageBoxTitle,MessageBoxButtons messageBoxButtons)
        {
            return DebugMsg.showMessageBox(message, messageBoxTitle, messageBoxButtons);
        }
        public void         logToChache(string text)
        {
            DebugMsg.sbLogCache.Append(text);
        }
        public string       getLogNameText()
        {
            return (LogName != null) ? "                                [... in " + LogName + "]" : "";
        }
    }
}