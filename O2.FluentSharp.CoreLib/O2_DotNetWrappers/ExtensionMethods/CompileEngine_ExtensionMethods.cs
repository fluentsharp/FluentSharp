using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using O2.Kernel;

using O2.DotNetWrappers.ExtensionMethods;
using O2.DotNetWrappers.DotNet;
using System.Reflection;
using O2.DotNetWrappers.Network;

namespace O2.DotNetWrappers.ExtensionMethods
{
    public static class CompileEngine_ExtensionMethods
    {
        public static string local(this string fileName)
        {
            return fileName.localScript();
        }

        public static string localScript(this string fileName)
        {
            return CompileEngine.findFileOnLocalScriptFolder(fileName.trim());
        }

		public static Assembly download_Assembly_From_O2_GitHub(this string assemblyName)
		{
			return assemblyName.download_Assembly_From_O2_GitHub(false);
		}
		public static Assembly download_Assembly_From_O2_GitHub(this string assemblyName, bool ignoreSslErrors)
		{
			if (assemblyName.assembly().notNull())
				"in download_Assembly_From_O2_GitHub, the requests assembly already exists".error(assemblyName);
			else
			{
				if (ignoreSslErrors)
					Web.Https.ignoreServerSslErrors();

				//add option to ignore cache
				new O2.Kernel.CodeUtils.O2GitHub().tryToFetchAssemblyFromO2GitHub(assemblyName,false);
			}
			return assemblyName.assembly();
		}
    }
}
