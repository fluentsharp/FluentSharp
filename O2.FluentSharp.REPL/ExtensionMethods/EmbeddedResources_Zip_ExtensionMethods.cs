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
    public static class EmbeddedResources_Zip_ExtensionMethods
    {
		public static string extract_EmbeddedResource_into_O2RootFolder(this string resourceName)
		{
			if (resourceName.notValid())
				return resourceName;
			var targetFolder = PublicDI.config.O2TempDir.pathCombine("..\\..\\" + resourceName);
			return resourceName.extract_EmbeddedResource_into_Folder(targetFolder);
		}

		public static string extract_EmbeddedResource_into_Folder(this string resourceName, string targetFolder)
		{
			if (resourceName.notValid() || targetFolder.notValid())
				return null;
			var sourceZip = resourceName + ".Zip";
			try
			{				
				if (targetFolder.dirExists())
					"[Embedded resources extraction] targetFolder already existed, skipping extration: {0}".info(targetFolder);
				else
				{
					var stream = sourceZip.resourceStream();
					if (stream.isNull())
						"[Embedded resources extraction] failed to find sourceZip embeded reference: {0} ".info(sourceZip);
					else
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
