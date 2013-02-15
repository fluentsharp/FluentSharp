// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Daniel Grunwald" email="daniel@danielgrunwald.de"/>
//     <version>$Revision: 1661 $</version>
// </file>

using System;
using O2.DotNetWrappers.ExtensionMethods;
//using log4net;

namespace ICSharpCode.SharpDevelop.Dom
{
	/// <summary>
	/// We don't reference ICSharpCode.Core but still need the logging interface.
	/// </summary>
	internal static class LoggingService
	{
        public static bool IsDebugEnabled   { get; set; }
        public static bool Info_Enabled     { get; set; }
        public static bool Debug_Enabled    { get; set; }
        public static bool Error_Enabled    { get; set; }

        static LoggingService()
        {
            Error_Enabled = true;
        }
		
        //static ILog log = LogManager.GetLogger(typeof(LoggingService));
		
		public static void Debug(object message)
		{
            if (Debug_Enabled)
                "[LoggingService][Debug] {0}".debug(message);
			//log.Debug(message);
		}
		
		public static void Info(object message)
		{
            if (Info_Enabled)
                "[LoggingService][Info] {0}".info(message);
			//log.Info(message);
		}
		
		public static void Warn(object message)
		{
            if (Debug_Enabled)    
                "[LoggingService][Warn] {0}".debug(message);
			//log.Warn(message);
		}
		
		public static void Warn(object message, Exception exception)
		{
            if (Debug_Enabled)
                "[LoggingService][Warn] {0} : {1}".debug(message, exception.Message);
			//log.Warn(message, exception);
		}
		
		public static void Error(object message)
		{
            if (Error_Enabled)
                "[LoggingService][Error] {0}".error(message);
			//log.Error(message);
		}
		
		public static void Error(object message, Exception exception)
		{
            if (Error_Enabled)
                "[LoggingService][Error] {0} : {1}".error(message, exception.Message);
			//log.Error(message, exception);
		}
		
		/*public static bool IsDebugEnabled {
			get {
				return log.IsDebugEnabled;
			}*/
		//}
	}
}
