using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using O2.Kernel.ExtensionMethods;
using O2.DotNetWrappers.DotNet;
using O2.DotNetWrappers.ExtensionMethods;
using System.Reflection;

namespace O2.Platform.BCL
{
	public class Start_O2
	{
		public Start_O2()
		{
		//	RegisterWindowsExtension.registerO2Extensions();
		}

		public Assembly compileScript(string o2Script)
		{
			var assembly = new CompileEngine().compileSourceFile(o2Script.local());
			return assembly;
		}



		
	}
}
