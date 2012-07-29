using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using O2.DotNetWrappers.ExtensionMethods;
using O2.Kernel;
using System.Reflection;

namespace O2.DotNetWrappers.ExtensionMethods
{
    public class O2Setup
    {
        public static string Data_Name { get; set;} 
        public static string Scripts_Name { get; set;} 

        static O2Setup()
        {
            Data_Name = PublicDI.config.CurrentExecutableFileName + ".Data";
            Scripts_Name = "O2.Platform.Scripts";
        }
        public static void extractEmbededConfigZips()
        {
            Data_Name.extract_EmbeddedResource_into_O2RootDir();
            Scripts_Name.extract_EmbeddedResource_into_O2RootDir();
        }

        public static string createEmbeddedFolder_Scripts(string targetDir)
        {
            var o2ScriptsFolder = targetDir.pathCombine(Scripts_Name).createDir();
			"O2_Logo.gif".local().file_Copy(o2ScriptsFolder);
            return o2ScriptsFolder;
        }

        public static string createEmbeddedFolder_Data(string targetDir, string name)
        {
            var dataFolder = targetDir.pathCombine(name + ".Data").createDir();
            "O2_Logo.gif".local().file_Copy(dataFolder);
            return dataFolder;
        }
    }

    public static class EmbeddedResources_Zip_ExtensionMethods
    {
        public static string extract_EmbeddedResource_into_O2RootDir(this string resourceName)
		{
			var sourceZip = resourceName + ".Zip";
			try
			{
				var targetFolder = PublicDI.config.O2TempDir.pathCombine("..\\..\\" + resourceName);
				if (targetFolder.dirExists())
					"[Embedded resources extraction] targetFolder already existed, skipping extration: {0}".info(targetFolder);
				else
				{
					var stream = sourceZip.resourceStream();
					if (stream.notNull())
					{
						var zipFile = stream.bytes().saveAs(sourceZip.tempFile());													  					
						zipFile.unzip(targetFolder);
						if (targetFolder.dirExists())
							"[Embedded resources extraction] extraced {0} ok into {1}".info(resourceName, targetFolder);
						return targetFolder;
					}
					//"[Embedded resources extraction] extraced of {0} into {1} failed".error(resourceName, targetFolder);
				}
			}
			catch(Exception ex)
			{
				ex.log("[Embedded resources extraction]");
			}
			return null;
		}
    }    
}
