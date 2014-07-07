using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;

namespace FluentSharp.Zip
{
    public class O2Setup
    {        
        public static string Scripts_Name { get; set;} 

        static O2Setup()
        {            
            Scripts_Name = "O2.Platform.Scripts";
        }
        public static void extractEmbededConfigZips()
        {
            Scripts_Name.extract_EmbeddedResource_into_O2RootFolder();
            "_ToolsOrApis".extract_EmbeddedResource_into_Folder(PublicDI.config.ToolsOrApis);
        }

        public static string createEmbeddedFolder_Scripts(string targetDir)
        {
            var o2ScriptsFolder = targetDir.pathCombine(Scripts_Name).createDir();
            "O2_Logo.gif".local().file_Copy(o2ScriptsFolder);
            return o2ScriptsFolder;
        }

    }
}