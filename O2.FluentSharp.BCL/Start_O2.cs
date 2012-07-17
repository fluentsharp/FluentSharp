using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using O2.DotNetWrappers.ExtensionMethods;
using O2.DotNetWrappers.DotNet;
using System.Reflection;
using System.Windows.Forms;
using O2.DotNetWrappers.Windows;

namespace O2.Platform.BCL
{
	public class Start_O2
	{
		/*public Start_O2()
		{
			
		}*/

		public Assembly compileScript(string o2Script)
		{
            var compileEngine = new CompileEngine();
			var assembly = compileEngine.compileSourceFile(o2Script.local());
            if (assembly.isNull())
                MessageBox.Show(compileEngine.sbErrorMessage.str(), "Compilation error in Start_O2:");
			return assembly;
        }                
        public bool complileO2StartupScriptAndExecuteIt(string[] args)
        {
            try
            {               

                /*var o2Bcl = load_O2_Assembly("O2_FluentSharp_BCL.dll");
                var startO2_Type = o2Bcl.GetType("O2.Platform.BCL.Start_O2");

                var startO2 = Activator.CreateInstance(startO2_Type);

                var compileScript = startO2.GetType().GetMethod("compileScript");
                var assembly = (Assembly)compileScript.Invoke(startO2, new object[] { "ascx_Execute_Scripts.cs" });*/                
                var assembly = compileScript("ascx_Execute_Scripts.cs");
                if (assembly == null)
                {
                    MessageBox.Show("There was a problem compiling the ascx_Execute_Scripts.cs script file", "O2 Start error");
                    return false;
                }
                assembly.type("ascx_Execute_Scripts").invokeStatic("startControl_With_Args",args);
                /*var types = assembly.GetTypes();
                var ascx_Execute_Scripts = assembly.GetType("O2.XRules.Database.ascx_Execute_Scripts");

                var startControl_No_Args = ascx_Execute_Scripts.GetMethod("startControl_With_Args");
                startControl_No_Args.Invoke(null, new object[] { args });*/
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in O2 Initialization (try deleting the CachedCompiledAssembliesMappings.xml file from the temp dir): " + ex.Message, "O2 Start error");
                return false;
            }
        }
	}
}
