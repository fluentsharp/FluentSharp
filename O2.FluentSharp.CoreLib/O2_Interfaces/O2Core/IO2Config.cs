// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections;
using System.Collections.Generic;

namespace O2.Interfaces.O2Core
{
    public interface IO2Config
    {
        string hardCodedO2LocalTempFolder { get; set; }
//        string hardCodedO2LocalBuildDir { get; set; }
//        string hardCodedO2LocalSourceCodeDir { get; set; }
        
        string O2TempDir                        { get; set; }
        string LocalScriptsFolder               { get; set; }        
        string LocallyDevelopedScriptsFolder    { get; set; }
        string AutoSavedScripts                 { get; set;}
        string ScriptsTemplatesFolder           { get; set; }        
        string SvnO2RootFolder                  { get; set; }
        string SvnO2DatabaseRulesFolder { get; set; }
        string O2SVN_ExternalDlls { get; set; }
        string O2SVN_Binaries { get; set; }
        string O2SVN_FilesWithNoCode { get; set; }
        string O2DownloadLocation { get; set; }
        string Version { get; }
        string O2ConfigFile { get; set; }
        string CurrentExecutableDirectory { get; }
        string CurrentExecutableFileName { get; }
        string ExecutingAssembly { get; }
        string TempFileNameInTempDirectory { get; }
        string TempFolderInTempDirectory { get; }
        //string O2KernelAssemblyName { get; }
		string ReferencesDownloadLocation { get; }
        string ToolsOrApis { get; }
        
        //String setDefaultDir_TempFolder();

        void setLocalScriptsFolder(string newLocalScriptsFolder);
        string getTempFileInTempDirectory(string extension);
        string getTempFolderInTempDirectory(string stringToAddToTempDirectoryName);

        void closeO2Process();            

        // DI helper
        bool setDI(Type typeToInjectDependency, string propertyToInject, Object dependencyObject);
        bool setDI(string assemblyName, string typeToInjectDependency, string propertyToInject, Object dependencyObject);

        // need to see if this is needed
        void addPathToCurrentExecutableEnvironmentPathVariable(String sPathToAdd);

        // misc global vars
        string O2FindingsFileExtension { get; set;}		
	}
}