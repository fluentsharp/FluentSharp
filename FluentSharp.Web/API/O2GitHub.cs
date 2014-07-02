using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;


namespace FluentSharp.Web35.API
{
    //need to change this text (it is using GIT now)
    public class O2GitHub
    {
        public static string        NO_GAC_TAG = "__NoGAC__";
        public static List<string>  AssembliesCheckedIfExists { get; set; }

        static O2GitHub()
        {
             AssembliesCheckedIfExists = new List<string>().add("System.Deployment");
        }

        public static void clear_AssembliesCheckedIfExists()
        {
            "in clear_AssembliesCheckedIfExists".info();
            AssembliesCheckedIfExists = new List<string>();
        }

        public bool tryToFetchAssemblyFromO2GitHub(string assemblyToLoad)
        {
            return tryToFetchAssemblyFromO2GitHub(assemblyToLoad, true);
        }

        public bool tryToFetchAssemblyFromO2GitHub(string assemblyToLoad, bool useCacheInfo)
        {            
            string localFilePath = "";
            if (assemblyToLoad.contains("".tempDir()))       // don't try to fetch temp assemblies
                return false;
            var thread = O2Thread.mtaThread(()=> downloadThread(assemblyToLoad, ref localFilePath, useCacheInfo));
            var maxWait = 60;
            if (thread.Join(maxWait * 1000) == false)
            {
                if (localFilePath.fileExists())                
                    "TimeOut (of {1} secs) was reached, but Assembly was sucessfully fetched from O2GitHub: {0}".info(maxWait,localFilePath);                                    
                else
                    "error while tring to fetchAssembly: {0} (max wait of {1} seconds reached)".error(assemblyToLoad, maxWait);                                
            }        
            return localFilePath.fileExists();
        }

		public static bool downloadThread(string assemblyToLoad, ref string localFilePath, bool useCacheInfo)
		{
			try
			{
				if (Path.GetExtension(assemblyToLoad) != ".dll" && Path.GetExtension(assemblyToLoad) != ".exe")
					assemblyToLoad += ".dll";
				if (useCacheInfo)
					if (AssembliesCheckedIfExists.Contains(assemblyToLoad) || AssembliesCheckedIfExists.Contains(Path.GetFileNameWithoutExtension(assemblyToLoad)))     // for performace reasons only check this once                           
						return true;
				assemblyToLoad = assemblyToLoad.remove(NO_GAC_TAG); // special tag to allow force downloads
				"Trying to fetch assembly from O2's GitHub repository: {0}".info(assemblyToLoad);
				AssembliesCheckedIfExists.add(assemblyToLoad);
				if (O2Kernel_Web.SkipOnlineCheck.isFalse() && new O2Kernel_Web().online() == false)
				{
					"We are currently offline, skipping the check".debug();
					return false;
				}

				var referencesDownloadLocation = PublicDI.config.ReferencesDownloadLocation.createDir();
				localFilePath = (assemblyToLoad.contains("/", "\\"))
									? referencesDownloadLocation.pathCombine(assemblyToLoad.fileName())
									: referencesDownloadLocation.pathCombine(assemblyToLoad);

				if (File.Exists(localFilePath))
				{
					localFilePath.assembly();	// load it									
					return true;
				}
				var webLocation1 = "{0}{1}".format(PublicDI.config.O2GitHub_ExternalDlls, assemblyToLoad).trim();
				if (webLocation1.httpFileExists())
				{
					new O2Kernel_Web().downloadBinaryFile(webLocation1, localFilePath);
				}
				else
				{
					var webLocation2 = "{0}{1}".format(PublicDI.config.O2GitHub_Binaries, assemblyToLoad).trim();

                    //new O2Kernel_Web().downloadBinaryFile(webLocation2, localFilePath);

                    if (webLocation2.httpFileExists())
					{
						new O2Kernel_Web().downloadBinaryFile(webLocation2, localFilePath);
					}
					else 
					{
						var webLocation3 = "{0}{1}".format(PublicDI.config.O2GitHub_FilesWithNoCode, assemblyToLoad).trim();
                        if (webLocation3.httpFileExists())
						{
							new O2Kernel_Web().downloadBinaryFile(webLocation3, localFilePath);
						}
					}
				}
				if (File.Exists(localFilePath))
				{
					"Assembly sucessfully fetched from O2GitHub: {0}".info(localFilePath);
					localFilePath.assembly();		// load it					
                    return true;
				}
			}
			catch (Exception ex)
			{
				ex.log("in O2GitHub, tryToFetchAssemblyFromO2GitHub");
			}
            return false;
		}
    
        /// <summary>
        /// Configure the ExternalAssemblerResolver of the AssemblyResolver to use tryToFetchAssemblyFromO2GitHub
        /// </summary>
        public static void register_GitHub_As_ExternalAssemblerResolver()
        {
            if(AssemblyResolver.ExternalAssemblerResolver.notContains(O2GitHub.gitHubResolver))
                AssemblyResolver.ExternalAssemblerResolver.add(O2GitHub.gitHubResolver);            
        }

        public static Assembly gitHubResolver(string assemblyToLoad)
        {
            if (new O2GitHub().tryToFetchAssemblyFromO2GitHub(assemblyToLoad))
                return assemblyToLoad.assembly();
            return null;
        }
    }
}
