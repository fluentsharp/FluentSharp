// Tshis file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using FluentSharp.CoreLib.API;

namespace FluentSharp.CoreLib
{
    public static class AppDomain_ExtensionMethods_Load
    {
        public static bool			             load       (this AppDomain appDomain, params string[] assembliesName)
        {
            var result = true;
            foreach(var assemblyName in assembliesName)
                if(appDomain.load(assemblyName, false).isFalse() && result)
                    result = false;
            return result;

        }
        public static bool                       load       (this AppDomain appDomain, string assemblyName, bool copyToAppDomainBaseDirectoryBeforeLoad)
        {
            return appDomain.load(assemblyName, assemblyName, copyToAppDomainBaseDirectoryBeforeLoad);
        }
        public static bool			             load       (this AppDomain appDomain, string fullAssemblyName, string pathToAssemblyToLoad,bool copyToAppDomainBaseDirectoryBeforeLoad)
        {
            try
            {
                // copy if asked to
                if (copyToAppDomainBaseDirectoryBeforeLoad)
                    try
                    {
                        if (File.Exists(pathToAssemblyToLoad))
                            O2Kernel_Files.Copy(pathToAssemblyToLoad, appDomain.BaseDirectory);
                        else
                            PublicDI.log.error(
                                "copyToAppDomainBaseDirectoryBeforeLoad was set but pathToAssemblyToLoad was set to a file that didn't exist: {0}",
                                pathToAssemblyToLoad);
                    }
                    catch (Exception ex)
                    {
                        ex.log("in load copyToAppDomainBaseDirectoryBeforeLoad");
                    }
                // load assembly into AppDomain
                //First try directly
                appDomain.Load(fullAssemblyName);                
            }
            catch (Exception ex1)
            {
                //then try using AssemblyName
                try
                {
                    appDomain.Load(AssemblyName.GetAssemblyName(fullAssemblyName));
                }
                catch (Exception ex2)
                {
                    // then load assembly into current appdomain to get its full name and try again
                    try
                    {
                        appDomain.Load(fullAssemblyName.assembly().FullName);
                    }
                    catch (Exception ex3)
                    {
                        // last change load assembly into current appdomain to try to use the full path of the assembly (if it is known to the current process)
                        var assemblyLocation = fullAssemblyName.assembly_Location();
                        if (assemblyLocation.fileExists())
                        try
                        {
                            appDomain.Load(assemblyLocation);
                        }
                        catch (Exception ex4)
                        {
                            PublicDI.log.ex(ex1, "could not load assembly (method1): " + fullAssemblyName);
                            PublicDI.log.ex(ex2, "could not load assembly (method2): " + fullAssemblyName);
                            PublicDI.log.ex(ex3, "could not load assembly (method3): " + fullAssemblyName);
                            PublicDI.log.ex(ex3, "could not load assembly (method4): " + assemblyLocation);
                            return false;
                        }                        
                    }
                }
            }
            return true;
        }
        public static Dictionary<string, string> load       (this AppDomain appDomain, Dictionary<string, string> assemblyDependencies)
        {
            var assemblyDependenciesNotLoaded = new Dictionary<string, string>();
            foreach (string assemblyDependency in assemblyDependencies.Keys)
                if (!appDomain.load(assemblyDependency, assemblyDependencies[assemblyDependency], true))
                    assemblyDependenciesNotLoaded.Add(assemblyDependency, assemblyDependencies[assemblyDependency]);
            return assemblyDependenciesNotLoaded;
        }
    
        public static AppDomain                  load_FluentSharp_CoreLib(this AppDomain appDomain, bool copyToAppDomainBaseDirectoryBeforeLoad = false)
        {
            if(appDomain.assemblies().empty() ||                        // means we can't get the assembly references
               appDomain.isAssemblyNotLoaded("FluentSharp.CoreLib"))    // or it has not been loaded
            {
                if (appDomain.load("FluentSharp.CoreLib".assembly_Location(), copyToAppDomainBaseDirectoryBeforeLoad).isFalse())
                    "[AppDomain][load_FluentSharp_CoreLib] load failed".error();
            }
            return appDomain;
        }
    }
    public static class AppDomain_ExtensionMethods
    {
        public static List<String>  assemblies              (this AppDomain appDomain, bool showFulName = false)
        {
            var assemblies = new List<String>();
            try
            {
                if (appDomain != null)
                    foreach (Assembly assembly in appDomain.GetAssemblies())
                        assemblies.add(showFulName ? assembly.FullName : assembly.name());
            }
            catch (Exception ex)
            {
                ex.log("in O2AppDomainFactory getAssemblies");
            }
            return assemblies;
        }  
        public static string        binFolder               (this AppDomain appDomain)
        {
            return appDomain.mapPath("bin").createDir();
        }
        public static bool          isAssemblyLoaded         (this AppDomain appDomain, string assemblyName)
        {
            return appDomain.assemblies().contains(assemblyName);
        }
        public static bool          isAssemblyNotLoaded     (this AppDomain appDomain, string assemblyName)
        {
            return appDomain.assemblies().notContains(assemblyName);
        }
        public static object		createAndUnWrap         (this AppDomain appDomain, string dllWithType, string typeToCreateAndUnwrap)
        {
            return appDomain.CreateInstanceAndUnwrap(dllWithType, typeToCreateAndUnwrap);
        }        
        /// <summary>
        /// Copies the provided assemblies into the provided AppDomain's bin folder
        /// 
        /// Returns the path to the assembly in the AppDomain bin folder
        /// 
        /// Note that this does NOT overide the assembly if it already existed
        /// </summary>
        /// <param name="appDomain"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static string        copy_To_Bin_Folder      (this AppDomain appDomain, Assembly assembly)
        {
            if (appDomain.notNull() && assembly.notNull())
            {
                var assemblyLocation = assembly.location();
                if (assemblyLocation.fileExists())
                {
                    var targetFile = appDomain.binFolder().mapPath(assemblyLocation.fileName());
                    if(targetFile.file_Doesnt_Exist())
                        assemblyLocation.file_Copy(targetFile);
                    if(targetFile.file_Exists())
                        return targetFile;
                }
            }
            return null;;
        }
        public static bool	        isNotCurrentAppDomain   (this AppDomain appDomain)
        {
            return appDomain.isCurrentAppDomain().isFalse();
        }
        public static bool			isCurrentAppDomain      (this AppDomain appDomain)
        {
            return appDomain != AppDomain.CurrentDomain;
        }
        public static AppDomain	    loadMainO2Dlls          (this AppDomain appDomain)
        {
            appDomain.load("FluentSharp.CoreLib.dll",
                           "FluentSharp.WinForms.dll");
            return appDomain;
        }
        public static AppDomain     loadAssembliesIntoAppDomain (this AppDomain appDomain, List<string> assembliesToLoad)
        {
            var assembliesInNewAppDomain = new List<String>();
            // first copy the assemblies
            foreach(var assemblyToLoad in assembliesToLoad)
            {                
                if (assemblyToLoad.extension(".dll") || assemblyToLoad.extension().ToLower() == ".exe") // since other wise these might be GAC assemblies
                {
                    if (File.Exists(assemblyToLoad))
                    {
                        var targetFileName = Path.Combine(appDomain.BaseDirectory, assemblyToLoad.fileName());
						if (targetFileName.fileExists().isFalse())
							O2Kernel_Files.Copy(assemblyToLoad, targetFileName);
                        assembliesInNewAppDomain.Add(targetFileName);
                    }
                    else
                    {
                        var resolvedName = Path.Combine(PublicDI.config.CurrentExecutableDirectory, assemblyToLoad);
                        if (File.Exists(resolvedName))
                        {
                            var targetFileName = Path.Combine(appDomain.BaseDirectory, resolvedName.fileName());
							if (targetFileName.fileExists().isFalse())
								O2Kernel_Files.Copy(resolvedName, targetFileName);
                            assembliesInNewAppDomain.Add(targetFileName);
                        }
                        else
                            PublicDI.log.error("in loadAssembliesIntoAppDomain , could not find dll to copy: {0}",
                                         assemblyToLoad);
                    }
                }
            }
            // then load them (and if there are no missing dependencies ALL should load ok
            foreach (var assemblyToLoad in assembliesInNewAppDomain)
                try
                {
                    appDomain.Load(Path.GetFileNameWithoutExtension(assemblyToLoad));
                }
                catch (Exception ex)
                {
                    PublicDI.log.ex(ex,
                              "in O2AppDomainFactory.loadAssembliesIntoAppDomain, could not load assembly: " +
                              assemblyToLoad);                    
                } 
                  
            return appDomain;
        } 
        public static string        mapPath                 (this AppDomain appDomain, string virtualPath)
        {
            return appDomain.rootFolder().mapPath(virtualPath);
        }
        public static string	    name                    (this AppDomain appDomain)
        {
            return appDomain.notNull() ? appDomain.FriendlyName : null;
        }
        public static string        rootFolder              (this AppDomain appDomain)
        {
            return (appDomain.notNull()) ? appDomain.BaseDirectory : null;
        }
        public static AppDomain     rootFolder_Open_In_Explorer(this AppDomain appDomain)
        {
            appDomain.rootFolder().startProcess();
            return appDomain;
        }
        public static bool          unLoadAppDomain         (this AppDomain appDomain)
        {
            try
            {
                PublicDI.log.info("Forcibly Unloading appDomain: {0}", appDomain.name());
                //removeDomainFromManagedList();
                AppDomain.Unload(appDomain);                
                return true;
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex, "in O2AppDomainFactory.unLoadAppDomain");
                return false;
            }            
        }
        public static bool          unLoadAppDomainAndDeleteTempFolder(this AppDomain appDomain)
        {
            try
            {
                var appDomainBaseDirectory = appDomain.BaseDirectory;// we need to get this value before we unload the appDomain
                if (appDomain.unLoadAppDomain())
                {
                    PublicDI.log.info("Deleting (recursively) appDomain Base Directory: {0}", appDomainBaseDirectory);
                    return Files.delete_Folder_Recursively(appDomainBaseDirectory);                    
                }
                return true;
            }
            catch (Exception ex)
            {
                ex.log("in O2AppDomainFactory.unLoadAppDomainAndDeleteTempFolder");
                return false;
            }            
        }
    }
}