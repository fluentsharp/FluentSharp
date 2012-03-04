// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
//O2Tag_OnlyAddReferencedAssemblies
using System;
using System.Windows.Forms;
using FluentSharp.O2.Kernel.ExtensionMethods;
using FluentSharp.O2.Kernel.Objects;
using FluentSharp.O2.DotNetWrappers.DotNet;
using FluentSharp.O2.DotNetWrappers.ExtensionMethods;
using FluentSharp.O2.Views.ASCX.Ascx.MainGUI;
using FluentSharp.O2.Views.ASCX.classes.MainGUI; 

//_O2Ref:C:\O2\O2Scripts_Database\O2_Core_APIs\_compiled_Dlls\FluentSharp_O2.dll 
//O2Ref:FluentSharp_O2_Interfaces.dll
//O2Ref:FluentSharp_O2_Kernel.dll
//O2Ref:FluentSharp_O2_DotNetWrappers.dll
//O2Ref:FluentSharp_O2_Views_ASCX.dll
   
namespace V2.O2.Platform
{  
    static class Launcher 
    { 
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        [STAThread] 
        static void Main(string[] args)
        {
        	if (Control.ModifierKeys == Keys.Shift)
        		showLogViewer().parentForm().width(1000).height(400);  
        	var firstScript = "O2_1st_Script.cs";        	
			Console.WriteLine("Welcome to the O2 Platform v2.0");
			
			"Current AppDomain: {0}".info(AppDomain.CurrentDomain.BaseDirectory);
			
			CompileEngine.lsGACExtraReferencesToAdd.Clear();
			var assembly = new CompileEngine().compileSourceFile(firstScript);
			if (assembly.notNull())			
			{
				Console.WriteLine("Executing script {0} from location {1}".info(firstScript, assembly.Location));
				if (assembly.methods().size()>0)
				{ 
					assembly.methods()[0].invoke();
					Console.WriteLine("Invocation complete");
				}
				else
					Console.WriteLine("Error: there were no methods in compiled assembly");			
			}
			else
				Console.WriteLine("Error: could not find, compile or execute first script ({0})".format(firstScript));
        }                
        
        public static ascx_LogViewer showLogViewer()
        {
        	return O2Gui.open<ascx_LogViewer>();
        }
        
        public static string executeScriptInSeparateAppDomain(this string scriptToExecute, bool showLogViewer, bool openScriptGui)
		{
			var appDomainName = 12.randomLetters();
			var o2AppDomain =  new O2AppDomainFactory(appDomainName);
/*			o2AppDomain.load("O2_XRules_Database"); 	
			o2AppDomain.load("O2_Kernel");
			o2AppDomain.load("O2_DotNetWrappers");
			
			var o2Proxy =  (O2Proxy)o2AppDomain.getProxyObject("O2Proxy");
			if (o2Proxy.isNull())
			{
				"in executeScriptInSeparateAppDomain, could not create O2Proxy object".error();
				return null;
			}
			o2Proxy.InvokeInStaThread = true;
			if (showLogViewer)
				o2Proxy.executeScript( "O2Gui.open<Panel>(\"Util - LogViewer\", 400,140).add_LogViewer();");
			if (openScriptGui)
				o2Proxy.executeScript("O2Gui.open<Panel>(\"Script editor\", 700, 300)".line() + 
 	  								  "		.add_Script(false)".line() + 
									  " 	.onCompileExecuteOnce()".line() + 
									  " 	.Code = \"hello\";".line() + 
									  "//O2File:Scripts_ExtensionMethods.cs");
			o2Proxy.executeScript(scriptToExecute);
			PublicDI.log.showMessageBox("Click OK to close the '{0}' AppDomain (and close all open windows)".format(appDomainName));										
*/			
			o2AppDomain.unLoadAppDomain();
			return scriptToExecute;
		}
    }
}