using System;
using System.Diagnostics;
using System.Text;
using FluentSharp.CoreLib.API;
using FluentSharp.CoreLib.Interfaces;

namespace FluentSharp.CoreLib
{
    public static class IO2Log_Extra_Extensionmethods
    {
        public static IO2Log writeToDebug(this IO2Log log)
        {
            return log.writeToDebug(false);
        }
    
        public static IO2Log writeToDebug(this IO2Log log, bool alsoShowInConsole)
        {
            log.LogRedirectionTarget = new Logger_DiagnosticsDebug {alsoShowInConsole = alsoShowInConsole};			
            return log;
        }
    }
}

namespace FluentSharp.CoreLib.API
{	
    public class Logger_DiagnosticsDebug : IO2Log
    {        
        public IO2Log           LogRedirectionTarget   { get ; set;}
        public bool             alsoShowInConsole      { get ; set;}
        public bool             alsoWriteToDebug       { get ; set;}        
        public StringBuilder    LogData                { get ; set;}     

        public Logger_DiagnosticsDebug()
        {
            alsoWriteToDebug = true;
            LogData = new StringBuilder();
        }

        private void writeLine(string message)
        {
            write(message.line());
        }
        public void write(string messageFormat, params object[] variables)
        {
            try
            {
                var message = variables.isNull()
                              ? messageFormat
                              : messageFormat.format(variables);
                if (alsoWriteToDebug)
                    Debug.Write(message);
                if (alsoShowInConsole)
                    Console.Write(message);
                LogData.Append(message);
            }
            catch (Exception ex)
            {
                Debug.Write("[FluentSharp][ERROR IN Logger_DiagnosticsDebug] " + ex.Message);		        
            }			
        }
        public void debug(string debugMessageFormat, params object[] variables)
        {
            writeLine("DEBUG: " + debugMessageFormat.format(variables));
        }
        public void error(string errorMessageFormat, params object[] variables)
        {
            writeLine("ERROR: " + errorMessageFormat.format(variables));
        }
        public void ex(Exception ex, string comment, bool showStackTrace)
        {
            writeLine("Exception: {0} {1}".format(comment, ex.Message));
            if (showStackTrace)
                writeLine("            " + ex.StackTrace);
        }
        public void ex(Exception ex, bool showStackTrace)
        {
            this.ex(ex, "", showStackTrace);
        }
        public void ex(Exception ex, string comment)
        {
            this.ex(ex, comment, false);
        }
        public void ex(Exception ex)
        {
            this.ex(ex, "", false);
        }
        public void info(string infoMessageFormat, params object[] variables)
        {
            writeLine("INFO: " + infoMessageFormat.format(variables));
        }
        public void logToChache(string text)
        {
            writeLine(text);
        }        
    }
}
