using System;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using System.Reflection;
using System.Windows.Forms;

namespace FluentSharp.BCL.Utils
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
