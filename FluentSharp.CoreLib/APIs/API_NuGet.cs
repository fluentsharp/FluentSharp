using System;
using System.Collections.Generic;
using FluentSharp.CoreLib.API;

namespace FluentSharp.CoreLib.APIs
{
    public class API_NuGet
    {
        public string Packages_Folder       { get; set;}
        public string NuGet_Exe             { get; set;}
		public Uri    NuGet_Exe_Download_Uri { get; set;}
		//public Program NuGet_Program { get; set; }
		//public Console NuGet_Console { get; set; }
		
		public API_NuGet() : this ("..//_Nuget".inTempDir())
		{            
		}	
        public API_NuGet(string packages_Folder)
        {
            NuGet_Exe_Download_Uri = "https://www.nuget.org/nuget.exe".uri();
            Packages_Folder        = packages_Folder;
            NuGet_Exe              = Packages_Folder.mapPath("nuget.exe");			
        }
		
		public API_NuGet SetUp()
		{
			//NuGet_Program = new Program();
			//NuGet_Console = new Console();
            return this;
		}
		public string execute(string command)
		{
			var nuGetExe = this.setup().NuGet_Exe;
			if (clr.mono())				
			{
				var workingDirectory = nuGetExe.parent_Folder();
				return "mono".startProcess_getConsoleOut ("\"{0}\" {1}".format (nuGetExe, command), workingDirectory);
			}		
			return nuGetExe.startProcess_getConsoleOut (command);
		}
    }

    public static class API_NuGet_ExtensionMethods
    {
        public static API_NuGet setup(this API_NuGet nuGet)
        {
            if (nuGet.NuGet_Exe.file_Doesnt_Exist())
            {
                nuGet.NuGet_Exe.parent_Folder().createDir();
                new O2Kernel_Web().downloadBinaryFile(nuGet.NuGet_Exe_Download_Uri.str(), nuGet.NuGet_Exe); 
            }
            return nuGet.NuGet_Exe.fileExists() ? nuGet : null;
        }

        public static List<string> list(this API_NuGet nuGet, string filter)
		{
			return nuGet.execute("list " + filter).split_onLines();
		}
		public static string install(this API_NuGet nuGet, string packageName)
		{
			var installMessage   = nuGet.execute("install " + packageName);
            var installedPackage = nuGet.extract_Installed_PackageName(installMessage);            
            return nuGet.NuGet_Exe.parentFolder().mapPath(installedPackage);
		}
        public static string extract_Installed_PackageName(this API_NuGet nuGet, string installMessage)
        {
            return installMessage.subString_After("'").subString_Before("'")
                                 .replace(" ",".");
        }
        public static string help(this API_NuGet nuGet)
        {
            return nuGet.execute("help");
        }
        /// <summary>
        /// Returns the path to the package name provided (note that this doesn't handle very well the package version
        /// </summary>
        /// <param name="nuGet"></param>
        /// <param name="packageName"></param>
        /// <returns></returns>
        public static string path_Package(this API_NuGet nuGet, string packageName)
        {
            return nuGet.Packages_Folder.folders("{0}*".format(packageName)).last();
        }
        public static bool  has_Package(this API_NuGet nuGet, string packageName)
        {
            return nuGet.path_Package(packageName).folder_Exists();
        }
        public static bool  does_Not_Have_Package(this API_NuGet nuGet, string packageName)
        {
            return nuGet.path_Package(packageName).folder_Not_Exists();
        }
        public static List<string>  packages_FluentSharp(this API_NuGet nuGet)
		{
			return nuGet.list("FluentSharp");
		}
        
    }
}
