using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using O2.DotNetWrappers.ExtensionMethods;

namespace O2.Kernel.CodeUtils
{
    //need to change this text (it is using GIT now)
    public class O2Svn
    {
        public const string NO_GAC_TAG = "__NoGAC__";
        public static List<string> AssembliesCheckedIfExists = new List<string>() {"System.Deployment"};        

        public static void clear_AssembliesCheckedIfExists()
        {
            "in clear_AssembliesCheckedIfExists".info();
            AssembliesCheckedIfExists = new List<string>();
        }

        public void tryToFetchAssemblyFromO2SVN(string assemblyToLoad)
        {
            tryToFetchAssemblyFromO2SVN(assemblyToLoad, true);
        }

        public void tryToFetchAssemblyFromO2SVN(string assemblyToLoad, bool useCacheInfo)
        {            
            string localFilePath = "";
            if (assemblyToLoad.contains("".tempDir()))       // don't try to fetch temp assemblies
                return ;
            var thread = O2Kernel_O2Thread.mtaThread(
                ()=>{
						downloadThread(assemblyToLoad, ref localFilePath);
					});
            var maxWait = 60;
            if (thread.Join(maxWait * 1000) == false)
            {
                if (File.Exists(localFilePath))                
                    "TimeOut (of {1} secs) was reached, but Assembly was sucessfully fetched from O2SVN: {0}".info(maxWait,localFilePath);                                    
                else
                    "error while tring to fetchAssembly: {0} (max wait of {1} seconds reached)".error(assemblyToLoad, maxWait);
                return;
            }
            //var localPath = Path.Combine
            //return false;
        }

		public static void downloadThread(string assemblyToLoad, ref string localFilePath)
		{
			try
			{
				if (Path.GetExtension(assemblyToLoad) != ".dll" && Path.GetExtension(assemblyToLoad) != ".exe")
					assemblyToLoad += ".dll";

				if (AssembliesCheckedIfExists.Contains(assemblyToLoad) || AssembliesCheckedIfExists.Contains(Path.GetFileNameWithoutExtension(assemblyToLoad)))     // for performace reasons only check this once                           
					return;
				assemblyToLoad = assemblyToLoad.remove(NO_GAC_TAG); // special tag to allow force downloads
				"Trying to fetch assembly from O2's SVN repository: {0}".info(assemblyToLoad);
				AssembliesCheckedIfExists.Add(assemblyToLoad);
				if (new O2Kernel_Web().online() == false)
				{
					"We are currently offline, skipping the check".debug();
					return;
				}

				var referencesDownloadLocation = PublicDI.config.ReferencesDownloadLocation;
				localFilePath = (assemblyToLoad.contains("/", "\\"))
									? Path.Combine(referencesDownloadLocation, Path.GetFileName(assemblyToLoad))
									: Path.Combine(referencesDownloadLocation, assemblyToLoad);

				if (File.Exists(localFilePath))
				{
					localFilePath.assembly();	// load it									
					return;
				}
				var webLocation1 = "{0}{1}".format(PublicDI.config.O2SVN_ExternalDlls, assemblyToLoad).trim();
				if (new O2Kernel_Web().httpFileExists(webLocation1))
				{
					new O2Kernel_Web().downloadBinaryFile(webLocation1, localFilePath);
				}
				else
				{
					var webLocation2 = "{0}{1}".format(PublicDI.config.O2SVN_Binaries, assemblyToLoad).trim();
					if (new O2Kernel_Web().httpFileExists(webLocation2))
					{
						new O2Kernel_Web().downloadBinaryFile(webLocation2, localFilePath);
					}
					else
					{
						var webLocation3 = "{0}{1}".format(PublicDI.config.O2SVN_FilesWithNoCode, assemblyToLoad).trim();
						if (new O2Kernel_Web().httpFileExists(webLocation3))
						{
							new O2Kernel_Web().downloadBinaryFile(webLocation3, localFilePath);
						}
					}
				}
				if (File.Exists(localFilePath))
				{
					"Assembly sucessfully fetched from O2SVN: {0}".info(localFilePath);
					localFilePath.assembly();		// load it
					return;
				}
			}
			catch (Exception ex)
			{
				ex.log("in O2Svn, tryToFetchAssemblyFromO2SVN");
			}
		}
    }
}
