using System;
using System.Collections.Generic;
using O2.DotNetWrappers.DotNet;

namespace O2.DotNetWrappers.Windows
{
    public class Netsh
    {
        public static string netshExe = "netsh.exe";

        public static bool isAvailable()
        {                    	
         	return execute("?").Contains("Usage:");
        }
		
		public static string interfaces_getIPConfig()
		{
			return execute("interface ip show config");
		}
		
		public static string interfaces_getDetails()
		{
			return execute("interface show interface");
		}	
		
		public static List<String> interfaces_getNames()
		{
			var names = new List<String>();
			var details = interfaces_getDetails(); 	
			//return execute("interface show interface");
			var lines = StringsAndLists.fromTextGetLines(details);
			if (lines.Count > 3)
			for(int i = 2 ; i < lines.Count; i++)
			{
				var items = lines[i].Split(new [] {"   "},StringSplitOptions.RemoveEmptyEntries );
				if (items.Length == 4)
					names.Add(items[3].Trim());
			}
			return names;
		}
		
		public static string interface_Disable(string interfaceName)
		{
			var execParams = string.Format("interface set interface \"{0}\" DISABLED", interfaceName);
			return execute(execParams);
		}
		
		public static string interface_Enable(string interfaceName)
		{
			var execParams = string.Format("interface set interface \"{0}\" ENABLE", interfaceName);
			return execute(execParams);
		}
		
		//public stati
		

        public static string execute(string parameters)
        {
            return Processes.startProcessAsConsoleApplicationAndReturnConsoleOutput(netshExe, parameters);
        }
    }
}
