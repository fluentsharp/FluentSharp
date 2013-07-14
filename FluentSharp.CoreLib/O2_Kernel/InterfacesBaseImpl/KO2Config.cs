// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace FluentSharp.CoreLib.API
{
    public class KO2Config // : IO2Config
    {
                
	    public int MAX_LOCALSCRIPTFOLDER_PARENTPATHSIZE = 120;

		public string hardCodedO2LocalTempFolder { get; set; }

		public static string defaultLocalScriptFolder = "";		
        public string  defaultO2LocalTempFolder = @"";        

        public KO2Config()
        {
	        try
	        {		        
			    calculate_O2TempDir();

			    O2ConfigSettings.defaultLocallyDevelopedScriptsFolder =
				    defaultO2LocalTempFolder.pathCombine(O2ConfigSettings.defaultLocallyDevelopedScriptsFolder);

			    hardCodedO2LocalTempFolder =
				    defaultO2LocalTempFolder.pathCombine(DateTime.Now.ToShortDateString().Replace("/", "_"));
		    
		        var o2TempDir = hardCodedO2LocalTempFolder; // so that we don't trigger the auto creation of the tempDir

		        UserData = defaultO2LocalTempFolder.pathCombine("_USERDATA"); //"C:\\O2\\_USERDATA"
		        
		        O2FindingsFileExtension = ".O2Findings";
		        extraSettings = new List<Setting>();
		        dependenciesInjection = new List<DependencyInjection>();
		        setLocalScriptsFolder(defaultLocalScriptFolder);
		        ScriptsTemplatesFolder = defaultLocalScriptFolder + @"\_Templates";
		        		        
		        O2GitHub_ExternalDlls = O2ConfigSettings.defaultO2GitHub_ExternalDlls;
		        O2GitHub_Binaries = O2ConfigSettings.defaultO2GitHub_Binaries;
		        O2GitHub_FilesWithNoCode = O2ConfigSettings.defaultO2GitHub_FilesWithNoCode;

		        AutoSavedScripts = o2TempDir.pathCombine(@"../_AutoSavedScripts")
		                                    .pathCombine(DateTime.Now.ToShortDateString().Replace("/", "_"));
		        // can't used safeFileName() here because the DI object is not created
		        ReferencesDownloadLocation = o2TempDir.pathCombine(@"../_ReferencesDownloaded");
		        EmbeddedAssemblies = o2TempDir.pathCombine(@"../_EmbeddedAssemblies");

		        AssemblyResolver.Init();
	        }
	        catch (Exception ex)
	        {
				ex.logWithStackTrace("in KO2Config.ctor");
	        }
        }

		public void calculate_O2TempDir()
		{
			defaultO2LocalTempFolder = O2ConfigSettings.defaultO2LocalTempName;
			defaultLocalScriptFolder = O2ConfigSettings.defaultLocalScriptName;
			//detect if we are running O2 as a stand alone exe
			if (Assembly.GetEntryAssembly().isNull() || Assembly.GetEntryAssembly().name().starts("O2 Platform"))
			{
				if (CurrentExecutableDirectory.pathCombine(@"..\..\" + O2ConfigSettings.defaultLocalScriptName).dirExists()) // check if the GitHub synced Scripts Folder exists
				{
					defaultO2LocalTempFolder = @"..\..\" + O2ConfigSettings.defaultO2LocalTempName;
					defaultLocalScriptFolder = @"..\..\" + O2ConfigSettings.defaultLocalScriptName;
				}
			}


			defaultO2LocalTempFolder = CurrentExecutableDirectory.pathCombine(defaultO2LocalTempFolder);
			defaultLocalScriptFolder = CurrentExecutableDirectory.pathCombine(defaultLocalScriptFolder);
            if (O2ConfigSettings.CheckForTempDirMaxSizeCheck)            
			    if (defaultLocalScriptFolder.size() > MAX_LOCALSCRIPTFOLDER_PARENTPATHSIZE)
			    {
				    "[o2setup] defaultLocalScriptFolder path was more than 120 chars: {0}".debug(defaultLocalScriptFolder);
				    var applicationData = getValidLocalSystemTempFolder();
				    var baseTempFolder = applicationData.pathCombine("O2_" + O2ConfigSettings.O2Version);
				    "[o2setup] using as baseTempFolder: {0}".debug(baseTempFolder);
				    defaultLocalScriptFolder = baseTempFolder.pathCombine(O2ConfigSettings.defaultLocalScriptName);
				    defaultO2LocalTempFolder = baseTempFolder.pathCombine(O2ConfigSettings.defaultO2LocalTempName);
				    //"[o2setup] set LocalScriptsFolder to: {0}".debug(defaultLocalScriptFolder);
			    }
		}

		public string getValidLocalSystemTempFolder()
		{
			var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			if (folder.dirExists())
				return folder;
			folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			if (folder.dirExists())
				return folder;
			folder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
			if (folder.dirExists())
				return folder;
			"in getValidLocalSystemTempFolder could not find a valid folder".error();
			return null;
		}
                       
        public KO2Config(string o2ConfigFile) :this ()
        {       
            O2ConfigFile = o2ConfigFile;            
        }

        // non-interface property
        public List<Setting> extraSettings { get; set; }
        public List<DependencyInjection> dependenciesInjection { get; set; }

        // non-interface properties
        public string O2ConfigFile { get; set; }        
        public string O2FindingsFileExtension { get; set; }                
//        public string hardCodedO2LocalBuildDir { get; set; }
//        public string hardCodedO2LocalSourceCodeDir { get; set; }
        public static string dependencyInjectionTest { get; set; }

        //interaface properties

        public string O2TempDir
        {
            get
            {
                if (hardCodedO2LocalTempFolder == null)
                    return null;
                O2Kernel_Files.checkIfDirectoryExistsAndCreateIfNot(hardCodedO2LocalTempFolder);
                return hardCodedO2LocalTempFolder;
            }
            set 
			{             
                   hardCodedO2LocalTempFolder = value; 
            }
        }

        public string AutoSavedScripts { get; set; }        
        public string LocalScriptsFolder { get; set; }
        public string LocallyDevelopedScriptsFolder { get; set; }
        public string ScriptsTemplatesFolder { get; set; }        
        public string O2GitHub_ExternalDlls		{ get; set; }
        public string O2GitHub_Binaries			{ get; set; }
        public string O2GitHub_FilesWithNoCode  { get; set; }
        public string O2DownloadLocation        { get; set; }       
        public string UserData                  { get; set; }

        public string ToolsOrApis
        {
            get
            {
                return O2TempDir.pathCombine("..//_ToolsOrApis");
            }
        }

        public string EmbededLibrariesFolder
        {
            get 
            {
                return defaultO2LocalTempFolder.pathCombine(@"_EmbeddedAssemblies");
            }
        }

        public string Version 
        {
            get 
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public void setLocalScriptsFolder(string newLocalScriptsFolder)
        {           
            LocalScriptsFolder = newLocalScriptsFolder;
            LocallyDevelopedScriptsFolder = O2ConfigSettings.defaultLocallyDevelopedScriptsFolder;
        }

		public string ReferencesDownloadLocation
		{
			get; set; 
/*			{
				//*return hardCodedO2LocalTempFolder.remove(DateTime.Today.ToShortDateString().Replace("/", "-"))
//												 .pathCombine("")
												 .createDir();
			}
 * */
		}

        public string EmbeddedAssemblies { get; set; }

        public string CurrentExecutableDirectory
        {
            get
            {
				try
				{
					var entryAssembly = Assembly.GetEntryAssembly();
					if (entryAssembly.isNull() || entryAssembly.GetName().Name.contains("FluentSharp").isFalse()) // if the main Assembly is not called O2 (for example under VisualStudio), then use the location of this assembly (FluentSharp.WinForms) 
						entryAssembly = Assembly.GetExecutingAssembly();					
					return Path.GetDirectoryName(entryAssembly.Location);					
				}
				catch (Exception ex)
				{
					ex.log();
				}
                return AppDomain.CurrentDomain.BaseDirectory;                
            }            
        }

        public String CurrentExecutableFileName
        {
            get { return Path.GetFileName(Process.GetCurrentProcess().ProcessName); }            
        }

        public string ExecutingAssembly
        {
            get { return Assembly.GetExecutingAssembly().Location; }            
        }

        public string TempFileNameInTempDirectory
        {
			get { return O2TempDir.pathCombine(O2Kernel_Files.getTempFileName()); }            
        }

        public string TempFolderInTempDirectory
        {
            get
            {
				string tempFolder = O2TempDir.pathCombine(O2Kernel_Files.getTempFolderName());
                Directory.CreateDirectory(tempFolder);
                return tempFolder;
            }            
        }

        /*public string O2KernelAssemblyName
        {
            get { return "O2_Kernel.dll"; }
            set { }
        }*/



        /// <summary>
        /// returns a tempfile in the temp directory with the provided extension 
        ///(no need to include the dot in the extension)
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public string getTempFileInTempDirectory(string extension)
        {
            return TempFileNameInTempDirectory +
                   (extension.StartsWith(".") ? extension : ("." + extension));
        }

        public string getTempFolderInTempDirectory(string stringToAddToTempDirectoryName)
        {
            var tempFolder = TempFolderInTempDirectory;
			var tempFolderWithExtraString = Path.GetDirectoryName(tempFolder).pathCombine(stringToAddToTempDirectoryName + "_" + Path.GetFileName(tempFolder));
            Directory.CreateDirectory(tempFolderWithExtraString);
            Directory.Delete(tempFolder);
            return tempFolderWithExtraString;

        }

        public bool setDI(Type typeToInjectDependency, string propertyToInject, Object dependencyObject)
        {
            //var diType = PublicDI.reflection.getType(typeToInjectDependency, "DI");
            if (typeToInjectDependency != null)
                if (PublicDI.reflection.setProperty(propertyToInject, typeToInjectDependency, dependencyObject))
                    return true;
            return false;
        }

        public bool setDI(string assemblyName, string typeToInjectDependency, string propertyToInject,
                          Object dependencyObject)
        {
            Type diType = PublicDI.reflection.getType(assemblyName, typeToInjectDependency);
            if (diType != null)
                if (PublicDI.reflection.setProperty(propertyToInject, diType, dependencyObject))
                {
                    PublicDI.log.info("setDI of object {0} into property {1} in Type {2} in Assembly {3}",
                                dependencyObject.GetType().Name, propertyToInject, typeToInjectDependency, assemblyName);
                    return true;
                }
            return false;
        }
        
        public void addPathToCurrentExecutableEnvironmentPathVariable(String sPathToAdd)
        {
            string sCurrentEnvironmentPath = Environment.GetEnvironmentVariable("Path");
            if (sCurrentEnvironmentPath != null && sCurrentEnvironmentPath.index(sPathToAdd) == -1)
            {
                String sUpdatedPathValue = String.Format("{0};{1}", sCurrentEnvironmentPath, sPathToAdd);
                Environment.SetEnvironmentVariable("Path", sUpdatedPathValue);
            }
        }


        public void closeO2Process()
        {
            PublicDI.log.info("Received request to close down current O2 Process");
            O2Kernel_Processes.KillCurrentO2Process(2000); // wait 2 seconds before killing the process            
            //System.Windows.Forms.Application.Exit();            
        }
    }

    [Serializable]
    public class Setting
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
    [Serializable]
    public class DependencyInjection
    {
        [XmlAttribute]
        public string Type { get; set; }
        [XmlAttribute]
        public string Parameter { get; set; }
        [XmlAttribute]
        public string Value { get; set; }
    }
}
