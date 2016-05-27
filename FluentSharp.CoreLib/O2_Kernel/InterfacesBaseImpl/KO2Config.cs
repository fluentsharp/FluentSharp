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
                	    
		public static string defaultLocalScriptFolder = "";		

        public int      MAX_LOCALSCRIPTFOLDER_PARENTPATHSIZE = 120;
        public string   defaultO2LocalTempFolder             = @""; 
        public string   hardCodedO2LocalTempFolder      { get; set; }               
        public string   AutoSavedScripts                { get; set; }        
        public string   LocalScriptsFolder              { get; set; }
        public string   LocallyDevelopedScriptsFolder   { get; set; }
        public string   ScriptsTemplatesFolder          { get; set; }        
        public string   O2GitHub_ExternalDlls		    { get; set; }
        public string   O2GitHub_Binaries			    { get; set; }
        public string   O2GitHub_FilesWithNoCode        { get; set; }
        public string   O2DownloadLocation              { get; set; }       
        public string   UserData                        { get; set; }
        public string   ReferencesDownloadLocation      { get; set; }        
        public string   EmbeddedAssemblies              { get; set; }                      
        public string   O2FindingsFileExtension         { get; set; }                

        //public static string dependencyInjectionTest { get; set; }

        public KO2Config()
        {	        		        
			Calculate_O2TempDir();
            MapFolders_And_Set_DefaultValues();			    	        
        }	
	
        public KO2Config MapFolders_And_Set_DefaultValues(string newRootFolderForAllO2Temp)
        {
            defaultO2LocalTempFolder = newRootFolderForAllO2Temp;
            defaultLocalScriptFolder = defaultO2LocalTempFolder.pathCombine(O2ConfigSettings.defaultLocalScriptName);
            return MapFolders_And_Set_DefaultValues();            
        }
        public KO2Config MapFolders_And_Set_DefaultValues()     
        {
            try
            {                

			    hardCodedO2LocalTempFolder = defaultO2LocalTempFolder.pathCombine(DateTime.Now.ToShortDateString().Replace("/", "_"));
		    
		        var o2TempDir = hardCodedO2LocalTempFolder; // so that we don't trigger the auto creation of the tempDir
                		        
		        
                ReferencesDownloadLocation    = o2TempDir.pathCombine(@"../_ReferencesDownloaded");
		        EmbeddedAssemblies            = o2TempDir.pathCombine(@"../_EmbeddedAssemblies");
                LocallyDevelopedScriptsFolder = o2TempDir.pathCombine(@"../_Scripts_Local");
                UserData                      = o2TempDir.pathCombine(@"../_USERDATA"); 

		        O2FindingsFileExtension = ".O2Findings";		        
		        
		        setLocalScriptsFolder(defaultLocalScriptFolder);
		        ScriptsTemplatesFolder = defaultLocalScriptFolder + @"\_Templates";
		        		        
		        O2GitHub_ExternalDlls    = O2ConfigSettings.defaultO2GitHub_ExternalDlls;
		        O2GitHub_Binaries        = O2ConfigSettings.defaultO2GitHub_Binaries;
		        O2GitHub_FilesWithNoCode = O2ConfigSettings.defaultO2GitHub_FilesWithNoCode;

		        AutoSavedScripts = o2TempDir.pathCombine(@"../_AutoSavedScripts")
		                                    .pathCombine(DateTime.Now.ToShortDateString().Replace("/", "_")); // can't used safeFileName() here because the DI object is not created

		        

		        AssemblyResolver.Init();
            }
	        catch (Exception ex)
	        {
				ex.logWithStackTrace("[KO2Config][MapFolders_And_Set_DefaultValues] usually from KO2Config.ctor");
                ex.logWithStackTrace("[KO2Config][Calculate_O2TempDir]");
	        }
            return this;
        }
        public KO2Config Calculate_O2TempDir()                  
		{
            try
            {
			    defaultO2LocalTempFolder = O2ConfigSettings.defaultO2LocalTempName;
			    defaultLocalScriptFolder = O2ConfigSettings.defaultLocalScriptName;


            //if we are running from an installed version 
            if (CurrentExecutableDirectory.contains(new string[] { "Program Files (x86)", "Program Files" }))   // need a better way to identify an install scenario
                if (CurrentExecutableDirectory.contains(@"OWASP\OWASP O2 Platform"))
                {
                    defaultO2LocalTempFolder = getValidLocalSystemTempFolder().path_Combine(defaultO2LocalTempFolder);
                    defaultLocalScriptFolder = getValidLocalSystemTempFolder().path_Combine(defaultLocalScriptFolder);
                    return this;
                }

            // Use locally folder (happens when the main zip is downloaded directly
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
            catch(Exception ex)
            {
                ex.logWithStackTrace("[KO2Config][Calculate_O2TempDir]");
            }
            return this;
		}
		public string    getValidLocalSystemTempFolder()        
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
        
        public string   O2TempDir                   
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
        public string   ToolsOrApis                 
        {
            get
            {
                return O2TempDir.pathCombine("..//_ToolsOrApis");
            }
        }
        public string   EmbededLibrariesFolder      
        {
            get 
            {
                return defaultO2LocalTempFolder.pathCombine(@"_EmbeddedAssemblies");
            }
        }
        public string   Version                     
        {
            get 
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }
        public void     setLocalScriptsFolder(string newLocalScriptsFolder) 
        {           
            LocalScriptsFolder = newLocalScriptsFolder;            
        }
        public string   CurrentExecutableDirectory
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
        public String   CurrentExecutableFileName
        {
            get { return Path.GetFileName(Process.GetCurrentProcess().ProcessName); }            
        }
        public string   ExecutingAssembly
        {
            get { return Assembly.GetExecutingAssembly().Location; }            
        }
        public string   TempFileNameInTempDirectory
        {
			get { return O2TempDir.pathCombine(O2Kernel_Files.getTempFileName()); }            
        }
        public string   TempFolderInTempDirectory
        {
            get
            {
				string tempFolder = O2TempDir.pathCombine(O2Kernel_Files.getTempFolderName());
                Directory.CreateDirectory(tempFolder);
                return tempFolder;
            }            
        }
        public string   getTempFileInTempDirectory(string extension)
        {
            return TempFileNameInTempDirectory +
                   (extension.StartsWith(".") ? extension : ("." + extension));
        }

        public string   getTempFolderInTempDirectory(string stringToAddToTempDirectoryName)
        {
            var tempFolder = TempFolderInTempDirectory;
			var tempFolderWithExtraString = Path.GetDirectoryName(tempFolder).pathCombine(stringToAddToTempDirectoryName + "_" + Path.GetFileName(tempFolder));
            Directory.CreateDirectory(tempFolderWithExtraString);
            Directory.Delete(tempFolder);
            return tempFolderWithExtraString;

        }
        public void     addPathToCurrentExecutableEnvironmentPathVariable(String sPathToAdd)
        {
            string sCurrentEnvironmentPath = Environment.GetEnvironmentVariable("Path");
            if (sCurrentEnvironmentPath != null && sCurrentEnvironmentPath.index(sPathToAdd) == -1)
            {
                String sUpdatedPathValue = String.Format("{0};{1}", sCurrentEnvironmentPath, sPathToAdd);
                Environment.SetEnvironmentVariable("Path", sUpdatedPathValue);
            }
        }
        public void     closeO2Process()
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
