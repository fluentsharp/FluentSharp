using System;
using System.Text;
using FluentSharp.CoreLib.Interfaces;


namespace FluentSharp.CoreLib.API
{
    public class Logger_Memory : IO2Log
    {
        public StringBuilder LogData         { get ; set;}     
        public IO2Log LogRedirectionTarget   { get ; set;}
        public bool alsoShowInConsole        { get ; set;}

        public Logger_Memory()
        {
            LogData = new StringBuilder();
        }

        public void debug(string debugMessageFormat, params object[] variables)
        {
            LogData.AppendLine("DEBUG: " + debugMessageFormat.format(variables));
        }
        public void error(string errorMessageFormat, params object[] variables)
        {
            LogData.AppendLine("ERROR: " + errorMessageFormat.format(variables));
        }
        public void ex(Exception ex, string comment, bool showStackTrace)
        {
            LogData.AppendLine("Exception: {0} {1}".format(comment, ex.Message));
            if (showStackTrace)
                LogData.AppendLine("            " + ex.StackTrace);
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
            LogData.AppendLine("INFO: " + infoMessageFormat.format(variables));
        }
        public void logToChache(string text)
        {
            LogData.AppendLine(text);
        }
        public void write(string messageFormat, params object[] variables)
        {
            LogData.AppendLine(messageFormat.format(variables));
        }        
    }
}
