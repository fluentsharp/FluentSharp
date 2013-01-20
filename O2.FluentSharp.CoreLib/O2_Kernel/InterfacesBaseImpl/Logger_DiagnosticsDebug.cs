using System;
using System.Diagnostics;
using O2.Interfaces.O2Core;
using O2.DotNetWrappers.ExtensionMethods;
using O2.Kernel.InterfacesBaseImpl;

namespace O2.DotNetWrappers.ExtensionMethods
{
	public static class Extra_Extensionmethods_IO2Log
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

namespace O2.Kernel.InterfacesBaseImpl
{	
    public class Logger_DiagnosticsDebug : IO2Log
    {        
        public IO2Log LogRedirectionTarget   { get ; set;}
        public bool alsoShowInConsole        { get ; set;}
	    
	    /*public DiagnosticsDebug_Logger()
        {            
        }*/

		private void writeLine(string message)
		{
			write(message.line());
		}

		public void write(string messageFormat, params object[] variables)
		{
			var message = variables.isNull()
				              ? messageFormat
				              : messageFormat.format(variables);
			Debug.Write(message);
			if (alsoShowInConsole)
				Console.WriteLine(message);			
		}

		public void write(string message)
		{
			write(message, null);
		}
        public void d(string debugMessage)
        {
			writeLine("DEBUG: " + debugMessage);
        }

        public void debug(string debugMessageFormat, params object[] variables)
        {
			writeLine("DEBUG: " + debugMessageFormat.format(variables));
        }

        public void debug(string debugMessage)
        {
			writeLine("DEBUG: " + debugMessage);
        }

        public void e(string errorMessage)
        {
			writeLine("ERROR: " + errorMessage);
        }

        public void error(string errorMessageFormat, params object[] variables)
        {
            writeLine("ERROR: " + errorMessageFormat.format(variables));
        }

        public void error(string errorMessage)
        {
            writeLine("ERROR: " + errorMessage);
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

        public void i(string infoMessage)
        {
            writeLine("INFO: " + infoMessage);
        }

        public void info(string infoMessageFormat, params object[] variables)
        {
            writeLine("INFO: " + infoMessageFormat.format(variables));
        }

        public void info(string infoMessage)
        {
            writeLine("INFO: " + infoMessage);
        }

        public void logToChache(string text)
        {
            writeLine(text);
        }        
    }
}
