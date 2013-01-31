using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using O2.DotNetWrappers.ExtensionMethods;

namespace O2.Kernel
{
    public class O2ConfigSettings
    {
		public static string    o2Version                            = Assembly.GetExecutingAssembly().version();        
        public static string    defaultLocalScriptName               = "O2.Platform.Scripts";
        public static string    defaultO2LocalTempName               = @"O2.Temp";        //@"O2\_TempDir_v" + O2Version;        
        public static bool      checkForTempDirMaxSizeCheck          = true;
        public static string    defaultLocallyDevelopedScriptsFolder = "_XRules_Local";
        public static string    defaultO2GitHub_ExternalDlls         = "https://raw.github.com/o2platform/O2_Platform_ReferencedAssemblies/master/3rdParty_Assemblies_withCode/";
        public static string    defaultO2GitHub_FilesWithNoCode      = "https://raw.github.com/o2platform/O2_Platform_ReferencedAssemblies/master/3rdParty_Assemblies_withNoCode/";
        public static string    defaultO2GitHub_Binaries             = "https://raw.github.com/o2platform/O2_Platform_ReferencedAssemblies/master/O2_Assemblies/";        
    }
}
