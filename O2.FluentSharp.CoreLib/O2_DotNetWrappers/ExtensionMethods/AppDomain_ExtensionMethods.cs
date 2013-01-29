// Tshis file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Net;
using System.Text;
using System.Linq;
using System.Drawing;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using O2.Kernel;
using O2.Kernel.Objects;
using O2.Kernel.ExtensionMethods;
using O2.DotNetWrappers.ExtensionMethods;
using O2.DotNetWrappers.DotNet;
using O2.DotNetWrappers.Windows;


namespace O2.DotNetWrappers.ExtensionMethods
{
    public static class _Extra_AppDomain_ExtensionMethods
    {
		public static O2AppDomainFactory	appDomain(this string name)
		{
			return name.appDomain_Get();
		}
		public static O2AppDomainFactory	appDomain_Get(this string name)
		{
			if (O2AppDomainFactory.AppDomains_ControledByO2Kernel.hasKey(name))
				return O2AppDomainFactory.AppDomains_ControledByO2Kernel[name];
			return null;
		}
		public static bool					isNotCurrentAppDomain(this O2AppDomainFactory appDomainFactory)
		{
			return appDomainFactory.isCurrentAppDomain().isFalse();
		}
		public static bool					isCurrentAppDomain(this O2AppDomainFactory appDomainFactory)
		{
			return appDomainFactory.appDomain != AppDomain.CurrentDomain;
		}
        public static O2AppDomainFactory	appDomain_New(this string appDomainName)
        {
            var appDomain = new O2AppDomainFactory(appDomainName);
            return appDomain;
        }
        public static O2AppDomainFactory	loadMainO2Dlls(this O2AppDomainFactory o2AppDomain)
        {
            o2AppDomain.load("FluentSharp.CoreLib.dll");
            o2AppDomain.load("FluentSharp.BCL.dll");
            return o2AppDomain;
        }
        public static string				executeCodeSnippet_InSeparateAppDomain(this string scriptToExecute)
        {
            return scriptToExecute.executeScriptInSeparateAppDomain(true, false);
        }
        public static string				executeScriptInSeparateAppDomain(this string scriptToExecute)
        {
            return scriptToExecute.executeScriptInSeparateAppDomain(true, false);
        }
        public static string				executeScriptInSeparateAppDomain(this string scriptToExecute, bool showLogViewer, bool openScriptGui)
        {
            var o2AppDomain =  12.randomLetters().createAppDomain();            

            o2AppDomain.executeScriptInAppDomain(scriptToExecute, showLogViewer, openScriptGui);
            
            //PublicDI.log.showMessageBox
            //	MessageBox.Show("Click OK to close the '{0}' AppDomain (and close all open windows)".format(appDomainName));
            //o2AppDomain.unLoadAppDomain();
            return scriptToExecute;
        }
        public static O2AppDomainFactory	createAppDomain(this string appDomainName)
        {
            return new O2AppDomainFactory(appDomainName).loadMainO2Dlls();
        }
        public static O2Proxy				executeScriptInAppDomain(this O2AppDomainFactory o2AppDomain, string scriptToExecute)
        {
            return o2AppDomain.executeScriptInAppDomain(scriptToExecute, false, false);
        }
        public static O2Proxy				executeScriptInAppDomain(this O2AppDomainFactory o2AppDomain, string scriptToExecute, bool showLogViewer, bool openScriptGui)
        {
            var o2Proxy = (O2Proxy)o2AppDomain.getProxyObject("O2Proxy");
            if (o2Proxy.isNull())
            {
                "in executeScriptInSeparateAppDomain, could not create O2Proxy object".error();
                return null;
            }
            o2Proxy.InvokeInStaThread = true;
            if (showLogViewer)
                o2Proxy.executeScript("open.logViewer();");
            if (openScriptGui)
                o2Proxy.executeScript("open.scriptEditor().inspector.set_Script(\"return 42;\");");            

            o2Proxy.executeScript(scriptToExecute);
            return o2Proxy;
        }
        public static O2Proxy				executeScript(this O2Proxy o2Proxy, string scriptToExecute)
        {
            o2Proxy.staticInvocation("O2_FluentSharp_REPL", "FastCompiler_ExtensionMethods", "executeSourceCode", new object[] { scriptToExecute });
            return o2Proxy;
        }
        public static string				execute_InScriptEditor_InSeparateAppDomain(this string scriptToExecute)
        {
            var script_Base64Encoded = scriptToExecute.base64Encode();
            var scriptLauncher = "open.scriptEditor().inspector.set_Script(\"{0}\".base64Decode()).waitForClose();".line().format(script_Base64Encoded);

            /*var scriptLauncher = "O2Gui.open<Panel>(\"Script editor\", 700, 300)".line() + 
                                      "		.add_Script(false)".line() + 
                                      " 	.onCompileExecuteOnce()".line() + 
                                      " 	.Code = \"{0}\".base64Decode();".line().format(script_Base64Encoded) + 
                                      "";*/
            scriptLauncher.executeScriptInSeparateAppDomain(false, false);
            return scriptLauncher;
        }
        public static string				localExeFolder(this string fileName)
        {
            var mappedFile = PublicDI.config.CurrentExecutableDirectory.pathCombine(fileName);
            return (mappedFile.fileExists())
                        ? mappedFile
                        : null;
        }
    }
}