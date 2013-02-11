using System;
using System.Reflection;
using O2.DotNetWrappers.ExtensionMethods;

namespace O2.Kernel
{
    public class O2ConfigSettings
    {   
        private static bool      _checkForTempDirMaxSizeCheck;

		public static string    O2Version                            { get; set; } 
        public static string    defaultLocalScriptName               = "O2.Platform.Scripts";
        public static string    defaultO2LocalTempName               = @"O2.Temp";            
        public static string    defaultLocallyDevelopedScriptsFolder = "_XRules_Local";
        public static string    defaultO2GitHub_ExternalDlls         = "https://raw.github.com/o2platform/O2_Platform_ReferencedAssemblies/master/3rdParty_Assemblies_withCode/";
        public static string    defaultO2GitHub_FilesWithNoCode      = "https://raw.github.com/o2platform/O2_Platform_ReferencedAssemblies/master/3rdParty_Assemblies_withNoCode/";
        public static string    defaultO2GitHub_Binaries             = "https://raw.github.com/o2platform/O2_Platform_ReferencedAssemblies/master/O2_Assemblies/";

        static O2ConfigSettings()
        {
            _checkForTempDirMaxSizeCheck = true;
            O2Version = "".append_CurrentAssemblyVersion();
        }

        public static bool CheckForTempDirMaxSizeCheck
        {
            get
            {                
                var entryAssembly = Assembly.GetEntryAssembly();
                if (entryAssembly.notNull() && entryAssembly.hasAttribute<SkipTempPathLengthVerification>())                                    
                    return false;
                return _checkForTempDirMaxSizeCheck;
            }
            set
            {
                _checkForTempDirMaxSizeCheck = value;
            }
        }
            
    }

    public class SkipTempPathLengthVerification : Attribute
    {
    }
}
