using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using O2.DotNetWrappers.ExtensionMethods;
using O2.DotNetWrappers.DotNet;
using System.Reflection;
using System.Windows.Forms;

namespace O2.Platform.BCL
{
	public class Start_O2
	{
		public Start_O2()
		{
			RegisterWindowsExtension.registerO2Extensions();
		}

		public Assembly compileScript(string o2Script)
		{
            var compileEngine = new CompileEngine();
			var assembly = compileEngine.compileSourceFile(o2Script.local());
            if (assembly.isNull())
                MessageBox.Show(compileEngine.sbErrorMessage.str(), "Compilation error in Start_O2:");
			return assembly;
        }		
	}
}
