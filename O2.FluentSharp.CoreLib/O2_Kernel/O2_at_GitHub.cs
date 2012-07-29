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
			PublicDI.config.O2GitHub_Binaries = "https://raw.github.com/o2platform/O2_Platform_ReferencedAssemblies/master/O2_Assemblies/";
            PublicDI.config.O2GitHub_ExternalDlls = "https://raw.github.com/o2platform/O2_Platform_ReferencedAssemblies/master/3rdParty_Assemblies_withCode/";
            PublicDI.config.O2GitHub_FilesWithNoCode = "https://raw.github.com/o2platform/O2_Platform_ReferencedAssemblies/master/3rdParty_Assemblies_withNoCode/";
			
		}
	}
}
