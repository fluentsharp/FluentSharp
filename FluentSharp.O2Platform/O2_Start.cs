using System;
using System.Reflection;
using System.Windows.Forms;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.O2Platform.Controls;

namespace FluentSharp.O2Platform.Utils
{
    public class O2_Start
    {
        public O2_Platform_Config  o2PlatformConfig;
        public O2_Platform_Scripts o2PlatformScripts;

        public O2_Start()
        {
            o2PlatformConfig = O2_Platform_Config.Current;
            o2PlatformScripts = new O2_Platform_Scripts();
        }
        public O2_Start OpenStartGui(params string[] args)
        {        
            if (O2PlatformScriptsExist())
            {
                //RegisterO2Extensions();

                ascx_Execute_Scripts.startControl_With_Args(args);
            }
            
            return this;
        }

        public bool O2PlatformScriptsExist()
        {            
            if(o2PlatformScripts.SetUp())
            {                
                return true;
            }
            "Something is wrong since the O2.Platform.Scripts repository is not available at the expected location: {0}".format(o2PlatformConfig.Folder_Scripts);
            return false;
        }
        /*public O2_Start RegisterO2Extensions()
        {
            
            var versionName      = o2PlatformConfig.Version_Name;
            var h2Logo           = o2PlatformConfig.Folder_Scripts.pathCombine(@"_DataFiles\_Images\H2Logo.ico");
            var o2Logo           = o2PlatformConfig.Folder_Scripts.pathCombine(@"_DataFiles\_Images\O2Logo.ico");
            RegisterWindowsExtension_WinForms.register_CurrentApplication(".h2", versionName, h2Logo);
            RegisterWindowsExtension_WinForms.register_CurrentApplication(".o2", versionName, o2Logo);
            return this;
        }*/



        public Assembly compileScript(string o2Script)
		{
            var compileEngine = new CompileEngine();
			var assembly = compileEngine.compileSourceFile(o2Script.local());
            if (assembly.isNull())
                MessageBox.Show(compileEngine.sbErrorMessage.str(), @"Compilation error in Start_O2:");
			return assembly;
        }                
        public bool complileO2StartupScriptAndExecuteIt(string[] args)
        {
            try
            {               
                
                var assembly = compileScript("ascx_Execute_Scripts.cs");
                if (assembly == null)
                {
                    MessageBox.Show(@"There was a problem compiling the ascx_Execute_Scripts.cs script file", @"O2 Start error");
                    return false;
                }
                // ReSharper disable CoVariantArrayConversion
                assembly.type("ascx_Execute_Scripts").invokeStatic("startControl_With_Args",args);                
                // ReSharper restore CoVariantArrayConversion
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Error in O2 Initialization (try deleting the CachedCompiledAssembliesMappings.xml file from the temp dir): " + ex.Message, @"O2 Start error");
                return false;
            }
        }
    }
}
