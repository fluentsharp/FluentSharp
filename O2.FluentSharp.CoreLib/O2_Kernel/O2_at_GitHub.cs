using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace O2.Kernel
{
	public class O2_at_GitHub
	{
		//this should only be called after the O2Config file has been loaded (or created)
		public static void configureReferencesDownloadLocations()
		{
			DI.config.O2SVN_Binaries = "https://raw.github.com/o2platform/O2_Platform_ReferencedAssemblies/master/O2_Assemblies/";										
			DI.config.O2SVN_ExternalDlls = "https://raw.github.com/o2platform/O2_Platform_ReferencedAssemblies/master/3rdParty_Assemblies_withCode/";
			DI.config.O2SVN_FilesWithNoCode = "https://raw.github.com/o2platform/O2_Platform_ReferencedAssemblies/master/3rdParty_Assemblies_withNoCode/";
			
		}
	}
}
