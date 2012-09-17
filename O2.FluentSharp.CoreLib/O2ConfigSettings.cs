using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace O2.Kernel
{
    public class O2ConfigSettings
    {
        public static string O2Version = "4_3";
        //public static string defaultLocalScriptFolder = @"C:\O2\O2Scripts_Database\_Scripts";
        public static string defaultLocalScriptName = "O2.Platform.Scripts";
        public static string defaultO2LocalTempName = @"_TempDir_v" + O2Version;        
        public static string defaultLocallyDevelopedScriptsFolder = "_XRules_Local";
        public static string defaultO2GitHub_ExternalDlls = ""; //"http://o2platform.googlecode.com/svn/trunk/O2 - All Active Projects/_3rdPartyDlls/";
        public static string defaultO2GitHub_FilesWithNoCode = ""; // http://o2platform.googlecode.com/svn/trunk/O2 - All Active Projects/_3rdPartyDlls/FilesWithNoCode/";
        public static string defaultO2GitHub_Binaries = ""; //http://o2platform.googlecode.com/svn/trunk/O2_Binaries/";
    }
}
