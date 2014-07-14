using System;
using FluentSharp.CoreLib.API;

namespace FluentSharp.CoreLib
{
    public static class O2AppDomainFactory_ExtensionMethods
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
        public static AppDomain		        appDomain(this O2AppDomainFactory appDomainFactory)
        {
            return appDomainFactory.notNull() ? appDomainFactory.AppDomain : null;
        }        
        public static O2AppDomainFactory	appDomain_New(this string appDomainName)
        {
            var appDomain = new O2AppDomainFactory(appDomainName);
            return appDomain;
        }
        
        public static O2AppDomainFactory	createAppDomain(this string appDomainName)
        {
            var o2AppDomainFactory = new O2AppDomainFactory(appDomainName);
            o2AppDomainFactory.appDomain().loadMainO2Dlls();
            return o2AppDomainFactory;
        }     
        public static string				localExeFolder(this string fileName)
        {
            var mappedFile = PublicDI.config.CurrentExecutableDirectory.pathCombine(fileName);
            return (mappedFile.fileExists())
                ? mappedFile
                : null;
        }
        public static string				executeCodeSnippet_InSeparateAppDomain  (this string scriptToExecute)
        {
            return scriptToExecute.executeScriptInSeparateAppDomain(true, false);
        }
        public static string				executeScriptInSeparateAppDomain        (this string scriptToExecute)
        {
            return scriptToExecute.executeScriptInSeparateAppDomain(true, false);
        }
        public static string				executeScriptInSeparateAppDomain        (this string scriptToExecute, bool showLogViewer, bool openScriptGui)
        {
            var o2AppDomain =  12.randomLetters().createAppDomain();            

            o2AppDomain.appDomain().executeScriptInAppDomain(scriptToExecute, showLogViewer, openScriptGui);
            
            //PublicDI.log.showMessageBox
            //	MessageBox.Show("Click OK to close the '{0}' AppDomain (and close all open windows)".format(appDomainName));
            //o2AppDomain.unLoadAppDomain();
            return scriptToExecute;
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
    }
}