// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;
using O2.DotNetWrappers.ExtensionMethods;
using O2.DotNetWrappers.Windows;
using O2.Kernel;
using O2.Kernel.CodeUtils;
using O2.Kernel.Objects;

namespace O2.DotNetWrappers.DotNet
{
    public class CompileEngine
    {                
        static string       _onlyAddReferencedAssemblies = "O2Tag_OnlyAddReferencedAssemblies";        
        static List<string> _specialO2Tag_ExtraReferences 	= new List<string>();
        static List<string> _specialO2Tag_Download 			= new List<string>();
        static List<string> _specialO2Tag_PathMapping 		= new List<string>();
        static List<string> _specialO2Tag_ExtraSourceFile 	= new List<string>();
        static List<string> _specialO2Tag_ExtraFolder 		= new List<string>();
        static List<string> _specialO2Tag_DontCompile		= new List<string>();

        public Assembly				compiledAssembly;
        public CompilerParameters	cpCompilerParameters;
        public CompilerResults		crCompilerResults;
        public CSharpCodeProvider	cscpCSharpCodeProvider;
        public StringBuilder		sbErrorMessage;
        public bool                 debugMode;
        public bool                 useCachedAssemblyIfAvailable;
        public string               compilationVersion;
        public static bool          needCachedCompiledAssembliesSave;

        public static Dictionary<string, string> LocalScriptFileMappings                { get; set; }
        public static List<string>               LocalReferenceFolders                  { get; set; }
        public static List<string>               LocalFoldersToSearchForCodeFiles       { get; set; }
        public static Dictionary<string, string> CachedCompiledAssemblies               { get; set; }
        public static string					 CachedCompiledAssembliesMappingsFile   { get; set; }
        public static Dictionary<string,string>  CompilationPathMappings                { get; set; }
        public static List<string>               DefaultReferencedAssemblies            { get; set; }
        public static List<string>               DefaultUsingStatements                 { get; set; }
        

        // the first time were here, load up the mappings from the CachedCompiledAssembliesMappingsFile
        static CompileEngine() 
        {
            LocalScriptFileMappings = new Dictionary<string, string>();
            LocalReferenceFolders    = new List<string>();
            LocalFoldersToSearchForCodeFiles = new List<string>();
            CachedCompiledAssemblies = new Dictionary<string, string>();
            CachedCompiledAssembliesMappingsFile = PublicDI.config.O2TempDir.pathCombine("..\\CachedCompiledAssembliesMappings.xml");
            CompilationPathMappings = new Dictionary<string, string>();
            DefaultReferencedAssemblies = new List<string>();
            DefaultUsingStatements = new List<string>();


            loadCachedCompiledAssembliesMappings();
            setDefaultLocalReferenceFolders();
            setDefaultReferencedAssemblies();
            setDefaultUsingStatements();
            _specialO2Tag_ExtraReferences	.add("//O2Tag_AddReferenceFile:")
                                            .add("//O2Ref:");
            _specialO2Tag_Download			.add("//Download:")
                                            .add("//O2Download:");
            _specialO2Tag_PathMapping 		.add("//PathMapping:")
                                            .add("//O2PathMapping:");
            _specialO2Tag_ExtraSourceFile  	.add("//O2Tag_AddSourceFile:")
                                            .add("//O2File:");
            _specialO2Tag_ExtraFolder		.add("//O2Tag_AddSourceFolder:")
                                            .add("//O2Folder:")
                                            .add("//O2Dir:");
            _specialO2Tag_DontCompile   	.add("//O2NoCompile");
        }

        public CompileEngine() : this (true)
        {
            
        }

        public CompileEngine(bool useCachedAssemblyIfAvailable)
        {
            this.useCachedAssemblyIfAvailable = useCachedAssemblyIfAvailable;            
            compilationVersion = (Environment.Version.Major.eq(4)) ? "v4.0" : "v3.5";
        }
		
        public CompileEngine(string compilation_Version) 
        {             
			compilationVersion = compilation_Version;
			useCachedAssemblyIfAvailable = true;            
        }

		public CompileEngine(string compilation_Version, bool useCachedAssemblyIfAvailable)
		{
			compilationVersion = compilation_Version;
			this.useCachedAssemblyIfAvailable = useCachedAssemblyIfAvailable;            
		}

        public static void setDefaultReferencedAssemblies()
        {
            DefaultReferencedAssemblies = new[] {
                                "System.Windows.Forms.dll",
                                "System.Drawing.dll",
                                "System.Data.dll",
                                "System.Xml.dll",
                                "System.Web.dll",
                                "System.Core.dll",
                                "System.Xml.Linq.dll",                                
                                "System.dll",
                            //O2Related
                                "FluentSharp.CoreLib.dll"                          
                                //,
                            //WPF 
//                                                                                    "PresentationCore.dll",
//                                                                                    "PresentationFramework.dll",
//                                                                                    "WindowsBase.dll",
//                                                                                    "System.Core.dll"
                            // to support the use of dynamic:
                               //"Microsoft.CSharp.dll" 
                            }.toList();            
        }
        public static void setDefaultUsingStatements()
        {
            DefaultUsingStatements = new List<string>()
                        .add("System")
                        .add("System.Drawing")
                        .add("System.Windows.Forms")
                        .add("System.Collections.Generic")
                        .add("System.Xml")
                        .add("System.Xml.Linq")
                        .add("System.Linq")
                        .add("O2.Interfaces")
                        .add("O2.Kernel")
                        .add("O2.DotNetWrappers.ExtensionMethods")
                        .add("O2.DotNetWrappers.Windows")
                        .add("O2.DotNetWrappers.DotNet")
                        .add("O2.DotNetWrappers.Network");
                // .add("O2.External.IE.ExtensionMethods")
                //.add("O2.XRules.Database.ExtensionMethods")
                //.add("O2.XRules.Database._Rules._Interfaces")
                //.add("O2.XRules.Database._Rules.APIs")
                //.add("O2.XRules.Database.O2Utils")                        
                //O2 XRules Database
//                        .add("O2.XRules.Database.APIs")
                        //.add("O2.XRules.Database.Utils");            

        }        
        public static void loadCachedCompiledAssembliesMappings()
        {
            try
            {
                if (CachedCompiledAssembliesMappingsFile.fileExists())
                {
                    "in loadCachedCompiledAssembliesMappings: {0}".debug(CachedCompiledAssembliesMappingsFile);
                    CachedCompiledAssemblies = CachedCompiledAssembliesMappingsFile.load<KeyValueStrings>().toDictionary();                        					
                }
            }
            catch (Exception ex)
            {
                ex.log("in loadCachedCompiledAssembliesMappings");
            }
            if (CachedCompiledAssemblies.isNull())
                CachedCompiledAssemblies = new Dictionary<string, string>();
        }
        public static void saveCachedCompiledAssembliesMappings()
        {
            try
            {
                if (needCachedCompiledAssembliesSave)
                {
                    "in saveCachedCompiledAssembliesMappings".debug();
                    var keyValueStrings = CompileEngine.CachedCompiledAssemblies.toKeyValueStrings();
                    keyValueStrings.saveAs(CachedCompiledAssembliesMappingsFile);
                    needCachedCompiledAssembliesSave = false;
                }
            }
            catch (Exception ex)
            {
                ex.log("in loadCachedCompiledAssembliesMappings");
            }
        }
        public static void setDefaultLocalReferenceFolders()
        { 
            LocalReferenceFolders.Add(PublicDI.config.ReferencesDownloadLocation);
            LocalReferenceFolders.Add(PublicDI.config.EmbeddedAssemblies);
            LocalReferenceFolders.Add(PublicDI.config.ToolsOrApis);   
        }
        public Assembly compileSourceCode(String sourceCodeFile)
        {
            return compileSourceCode(sourceCodeFile, "", "");
        }
        public Assembly compileSourceCode(String sourceCodeFile, string mainClass, string outputAssemblyName)
        {
            var tempSourceCodeFile = PublicDI.config.getTempFileInTempDirectory("_" +outputAssemblyName + ".cs");
            Files.WriteFileContent(tempSourceCodeFile, sourceCodeFile);
            return compileSourceFiles(new List<string>().add(tempSourceCodeFile)
                                                        .add(mainClass)
                                                        .add(outputAssemblyName));
        }
        public Assembly compileSourceFile(String sourceCodeFile)
        {
            PublicDI.CurrentScript = sourceCodeFile;
            return compileSourceFiles(new List<string>().add(sourceCodeFile));
        }
        public Assembly compileSourceFiles(List<String> sourceCodeFiles)
        {
            return compileSourceFiles(sourceCodeFiles, "" /*mainClass*/);
        }
        public Assembly compileSourceFiles(List<String> sourceCodeFiles, string mainClass)
        {
            
            return compileSourceFiles(sourceCodeFiles, mainClass, "" /*outputAssemblyName*/);
        }
        public Assembly compileSourceFiles(List<String> sourceCodeFiles, string mainClass, string outputAssemblyName)
        {
            return compileSourceFiles(sourceCodeFiles, mainClass, outputAssemblyName, false);
        }
        public Assembly compileSourceFiles(List<String> sourceCodeFiles, string mainClass, string outputAssemblyName, bool workOffline)
        {
            try
            {
                sourceCodeFiles = checkForNoCompileFiles(sourceCodeFiles);
                var filesMd5 = sourceCodeFiles.filesContents().md5Hash();

                if (useCachedAssemblyIfAvailable)
                {
                    // see if we already have a cache of all these files                
                    var cachedCompilation = getCachedCompiledAssembly_MD5(filesMd5);
                    if (cachedCompilation.notNull())
                    {
                        compiledAssembly = cachedCompilation;
                        if (loadReferencedAssembliesIntoMemory(compiledAssembly))
                            return cachedCompilation;
                    }
                }
                //if not compile file
                if (sourceCodeFiles.Count == 0)
                    return null;
                string errorMessages = "";
                compiledAssembly = null;
                var referencedAssemblies = getListOfReferencedAssembliesToUse();

                //handle cases where an install script needs to be executed
                handleReferencedAssembliesInstallRequirements(sourceCodeFiles);
                // see if there are any extra DLL references in the code

                mapReferencesIncludedInSourceCode(sourceCodeFiles, referencedAssemblies);
                sourceCodeFiles = sourceCodeFiles.onlyValidFiles();
                if (sourceCodeFiles.size() == 0)  // means there are no files to compile
                    return null;

                tryToResolveReferencesForCompilation(referencedAssemblies, workOffline);                 // try to make sure all assemblies are available for compilation
                if (compileSourceFiles(sourceCodeFiles, referencedAssemblies.ToArray(), ref compiledAssembly, ref errorMessages, false
                    /*verbose*/, mainClass, outputAssemblyName))
                {
                    PublicDI.log.debug("Compilated OK to: {0}", compiledAssembly.Location);
                    setCachedCompiledAssembly(filesMd5, compiledAssembly);                  // store the assembly under the original files MD5 hash (so that next time we don't have to calculate each file script dependency)
                    return compiledAssembly;
                }             
                PublicDI.log.error("Compilation failed: {0}", errorMessages);
            }
            catch (Exception ex)
            {
				ex.logWithStackTrace("in CompileEngine.compileSourceFiles(...)");
            }
            
            return null;
        }
        public static bool loadReferencedAssembliesIntoMemory(Assembly targetAssembly)
        {
            var referencedAssemblies = targetAssembly.GetReferencedAssemblies();
            "loading ReferencedAssembliesIntoMemory: {0}".info(referencedAssemblies.size());
            foreach (var assemblyName in referencedAssemblies)
            {
                Assembly assembly;
                try
                {
                    if (CachedCompiledAssemblies.ContainsKey(assemblyName.Name))
                        assembly = CachedCompiledAssemblies[assemblyName.Name].assembly();
                    else
                    {
                        var tmpFileLocation = PublicDI.config.O2TempDir.pathCombine(assemblyName.Name + ".dll");
                        if (tmpFileLocation.fileExists())
                            assembly = Assembly.LoadFrom(tmpFileLocation);
                        else
                            assembly = Assembly.Load(assemblyName);
                    }
                }
                catch
                {
                    assembly = assemblyName.Name.assembly();
                    
                }
                if (assembly.isNull())
                {
                    "[loadReferencedAssembliesIntoMemory] failed to load assembly".error();
                    return false;
                }
            }            
            return true;
        }
        public static void setCachedCompiledAssembly(List<string> sourceCodeFiles, Assembly compiledAssembly)
        {
            if (sourceCodeFiles.notNull())
            {
                var filesMd5 = sourceCodeFiles.filesContents().md5Hash();
                setCachedCompiledAssembly(filesMd5, compiledAssembly);
            }            
        }
        public static void setCachedCompiledAssembly_toMD5(string sourceCode, Assembly compiledAssembly)
        {
            var codeMd5 = sourceCode.md5Hash();
            //CompileEngine.CachedCompiledAssemblies.add(codeMd5, CompiledAssembly.Location);
            //CompileEngine.CachedCompiledAssemblies.add(CompiledAssembly.GetName().Name, CompiledAssembly.Location);
            setCachedCompiledAssembly(codeMd5, compiledAssembly);
        }
        public static void setCachedCompiledAssembly(string key, Assembly compiledAssembly, bool triggerSave = false)
        {
            if (key.valid() && 
                compiledAssembly.notNull() && 
                compiledAssembly.Location.valid() &&
                compiledAssembly.Location.fileExists())
            {
                needCachedCompiledAssembliesSave = true;             
                CachedCompiledAssemblies.add(key, compiledAssembly.Location);
                CachedCompiledAssemblies.add(compiledAssembly.GetName().Name, compiledAssembly.Location);
                CachedCompiledAssemblies.add(compiledAssembly.str(), compiledAssembly.Location);
                if (triggerSave)
                    saveCachedCompiledAssembliesMappings();                
            }
        }
        public static void setCachedCompiledAssembly(string key, string mapping, bool triggerSave = true)
        {
            if (key.valid() && mapping.valid())
            {
                needCachedCompiledAssembliesSave = true;
                CachedCompiledAssemblies.add(key, mapping);
                if (triggerSave)
                    saveCachedCompiledAssembliesMappings();
            }
        }        
        public static Assembly getCachedCompiledAssembly_MD5(List<string> sourceCodeFiles)
        {
            var filesMd5 =  sourceCodeFiles.filesContents().md5Hash();
            return getCachedCompiledAssembly_MD5(filesMd5);
        }
        public static Assembly getCachedCompiledAssembly_MD5(string key)
        {
            if (CachedCompiledAssemblies.hasKey(key))
            {
                var cachedAssembly = CachedCompiledAssemblies[key];
                if (cachedAssembly.fileExists())
                {
                    "found cached compiled assembly: {0}".info(cachedAssembly);
                    var assembly = cachedAssembly.assembly();

                    if (assembly.notNull())
                    {
                        if (Assembly.GetEntryAssembly().notNull())
                        {
                            if (assembly.ImageRuntimeVersion == Assembly.GetEntryAssembly().imageRuntimeVersion())
                                return assembly;
                           CachedCompiledAssemblies.Remove(key);
                        }
                        else
                            return assembly;    //happens when runing these scripts from inside visualstudio
                    }
                }
            }
            return null;
        }
        public static bool removeCachedAssemblyForCode_MD5(string key)
        {
            if (CachedCompiledAssemblies.hasKey(key))
            {
                CachedCompiledAssemblies.Remove(key);
                return true;
            }
            return false;
        }
        public void mapReferencesIncludedInSourceCode(string sourceCodeFile, List<string> referencedAssemblies)
        {
            var sourceCodeFiles = new List<string>();
            sourceCodeFiles.add(sourceCodeFile);
            addSourceFileOrFolderIncludedInSourceCode(sourceCodeFiles, referencedAssemblies, new List<string>());
            addReferencesIncludedInSourceCode(sourceCodeFiles, referencedAssemblies);
            
        }
        public void mapReferencesIncludedInSourceCode(List<string> sourceCodeFiles, List<string> referencedAssemblies)
        {
            var o2Timer = new O2Timer("mapReferencesIncludedInSourceCode").start();
            addSourceFileOrFolderIncludedInSourceCode(sourceCodeFiles, referencedAssemblies, new List<string>());
            addReferencesIncludedInSourceCode(sourceCodeFiles, referencedAssemblies);

            if (sourceCodeFiles.Count > 1)
            {
                PublicDI.log.debug("There are {0} files to compile", sourceCodeFiles.Count);
                foreach (var file in sourceCodeFiles)
                    PublicDI.log.debug("   {0}", file);
            }
            if (referencedAssemblies.Count > 1)
            {
                applyCompilationPathMappings(referencedAssemblies);
                PublicDI.log.debug("There are {0} referencedAssemblies used", referencedAssemblies.Count);
                if (debugMode)
                    foreach (var referencedAssembly in referencedAssemblies)
                        PublicDI.log.debug("   {0}", referencedAssembly);
            }
            o2Timer.stop();
        }
        public static Dictionary<string,string> clearCompilationPathMappings()
        {
            "in clearCompilationPathMappings".info();
            CompilationPathMappings.Clear();
            return CompilationPathMappings;
        }
        public static void clearAllCompilationRelatedDownloadsAndCaches()
        {
            "Clearing All Compilation Related Downloads and Caches".info();
            clearDownloadedReferences();
            clearEmbeddedAssemblies();
            clearCompilationCache();
        }
        public static Dictionary<string, string> clearCompilationCache()
        {
            "Clearing Compilation Cache".debug();
            CachedCompiledAssemblies.Clear();
            needCachedCompiledAssembliesSave = true;
            saveCachedCompiledAssembliesMappings();            
            return CachedCompiledAssemblies;
        }
        public static void clearEmbeddedAssemblies()
        {
            "Deleting Embedded dlls Cache".debug();
            foreach (var file in PublicDI.config.EmbeddedAssemblies.files())
                file.delete_File(false);             
        }
        public static void clearDownloadedReferences()
        {
            "Deleting Downloaded References Dlls and Exes".debug();
            foreach (var file in PublicDI.config.ReferencesDownloadLocation.files())
                file.delete_File(false);
        }
        public static Dictionary<string, string> clearLocalScriptFileMappings()
        {
            "in clearLocalScriptFileMappings".info();
            LocalScriptFileMappings.Clear();
            return LocalScriptFileMappings;
        }
        public static Dictionary<string, string> showInLog_CompilationPathMappings()
        {
            "Current CompilationPathMappings".debug();
            foreach (var pathMapping in CompilationPathMappings)
                "    {0}  =  {1}".info(pathMapping.Key, pathMapping.Value);
            return CompilationPathMappings;
        }        
        public static List<string> applyCompilationPathMappings(List<string> itemsToMap)
        {
            foreach(var compilationMapping in CompilationPathMappings)
                for(int i=0; i < itemsToMap.Count; i++)
                    itemsToMap[i] = itemsToMap[i].replace(compilationMapping.Key, compilationMapping.Value);
            return itemsToMap;
        }
        public static Dictionary<string,string> addCompilationPathMappings(string mappingToAdd)
        {
            if (mappingToAdd.valid())
            {
                var splittedMapping = mappingToAdd.split("=");
                if (splittedMapping.Count == 2)
                    addCompilationPathMappings(splittedMapping[0], splittedMapping[1]);
            }
            return CompilationPathMappings; 
        }
        public static Dictionary<string, string> addCompilationPathMappings(string key, string value)
        {
            if (key.valid() && value.valid())
            {
                "Adding CompilationPathMappings: {0} = {1}".info(key, value);
                CompilationPathMappings.add(key, value);
            }
            return CompilationPathMappings;
        }        
        public static List<string> checkForNoCompileFiles(List<string> sourceCodeFiles)
        {
            var filesToNotCompile = new List<string>();
            foreach(var sourceCodeFile in sourceCodeFiles)
                if (sourceCodeFile.fileExists())
                    if ("" != StringsAndLists.InFileTextStartsWithStringListItem(sourceCodeFile, _specialO2Tag_DontCompile))
                        filesToNotCompile.Add(sourceCodeFile);
            foreach (var fileToNotCompile in filesToNotCompile)
            {
                PublicDI.log.debug("Removing from list of files to compile the file: {0}", fileToNotCompile);
                sourceCodeFiles.Remove(fileToNotCompile);
            }
            return sourceCodeFiles;                
        }
        public static void addSourceFileOrFolderIncludedInSourceCode(List<string> sourceCodeFiles, List<string> referencedAssemblies ,List<string> resolvedFiles)
        {
            var currentSourceDirectories = new List<string>(); // in case we need to resolve file names below
            foreach (var sourceCodeFile in sourceCodeFiles)
            {
                if (sourceCodeFile.valid())
                {
                    if (resolvedFiles.contains(sourceCodeFile))
                        continue;
                    var directory = Path.GetDirectoryName(sourceCodeFile);
                    if (false == currentSourceDirectories.Contains(directory))
                        currentSourceDirectories.Add(directory);
                }
            }

            var filesToLoad = new List<string>();
            Action<string,string> add_To_FilesToLoad = 
                (original,mapped) =>
                    {
                        resolvedFiles.add(original);
                        filesToLoad.add(mapped);

                        "Found reference '{0}' in '{1}".debug(mapped.fileName(), original.fileName());
                    };

            // find the extra files to add
            foreach (var sourceCodeFile in sourceCodeFiles)
            {
                if (resolvedFiles.contains(sourceCodeFile))
                    continue;
                if (sourceCodeFile.valid() && sourceCodeFile.extension(".h2").isFalse())
                {
                    var fileLines = Files.getFileLines(sourceCodeFile);
                    foreach (var fileLine in fileLines)
                    {
                        var match = StringsAndLists.TextStartsWithStringListItem(fileLine, _specialO2Tag_ExtraSourceFile);
                        if (match != "")
                        {
                            //   var file = fileLine.Replace(specialO2Tag_ExtraSourceFile, "").Trim();
                            var file = fileLine.Replace(match, "").Trim();
                            if (false == sourceCodeFiles.Contains(file) && false == filesToLoad.Contains(file))
                            {
                                //handle the File:xxx:Ref:xxx case
                               // if (CompileEngine.isFileAReferenceARequestToUseThePrevioulsyCompiledVersion(file, referencedAssemblies)
                                //                 .isFalse())
                                add_To_FilesToLoad(sourceCodeFile, file);
                            }
                        }
                        //else if (fileLine.StartsWith(specialO2Tag_ExtraFolder))
                        else
                        {
                            match = StringsAndLists.TextStartsWithStringListItem(fileLine, _specialO2Tag_ExtraFolder);
                            if (match != "")
                            {
                                var folder = fileLine.Replace(match, "").Trim();
                                if (false == Directory.Exists(folder))
                                    foreach (var path in currentSourceDirectories)
                                        if (Directory.Exists(Path.Combine(path, folder)))
                                        {
                                            folder = Path.Combine(path, folder);
                                            break;
                                        }
                                foreach (var file in Files.getFilesFromDir_returnFullPath(folder, "*.cs", true))
                                    if (false == sourceCodeFiles.Contains(file) && false == filesToLoad.Contains(file))
                                        add_To_FilesToLoad(sourceCodeFile, file);
                            }

                            else
                            {
                                match = StringsAndLists.TextStartsWithStringListItem(fileLine, _specialO2Tag_PathMapping);
                                if (match != "")
                                    addCompilationPathMappings(fileLine.remove(match));
                            }
                        }
                    }
                }
            }

            

            // add them to the list (checking if the file exist)
            if (filesToLoad.Count > 0)
            {
                applyCompilationPathMappings(filesToLoad);
                PublicDI.log.info("There are {0} extra files to add to the list of source code files to compile: {1}", filesToLoad.Count, sourceCodeFiles.size());
                foreach (var file in filesToLoad)
                {                    
                    var filePath = "";
                    if (File.Exists(file))
                        filePath = file;
                    else
                    {
                        // try to find the file in the current sourceCodeFiles directories                        
                        foreach (var directory in currentSourceDirectories)
                            if (File.Exists(Path.Combine(directory, file)))
                            {
                                filePath = Path.Combine(directory, file);
                                break;
                            }							
                        //then look in the O2 Scripts folder
                        if (filePath == "" && file.local().fileExists())
                            filePath = file.local();						
                        if (filePath == "")
                        {
                            PublicDI.log.error("in addSourceFileOrFolderIncludedInSourceCode, could not file file to add: {0}", file);
                            //filesToLoad.add(file);
                        }
                    }
                    if (filePath != "" )
                    {
                        filePath = Path.GetFullPath(filePath);
                        if (false == sourceCodeFiles.lower().Contains(filePath.lower()))
                        {
                            sourceCodeFiles.Add(filePath);
                            addSourceFileOrFolderIncludedInSourceCode(sourceCodeFiles, referencedAssemblies, resolvedFiles); // we need to recursively add new new dependencies 
                        }
                    }
                }               
            }
        }
        /*public static void insertSourceFileInListOfSourceFiles(List<string> sourceCodeFiles, string fileToInsert)
        {
            if (sourceCodeFiles.Contains(fileToInsert))         // check if the file is already in the list of sourceCodeFiles
                sourceCodeFiles.Remove(fileToInsert);           // remove it if required
            sourceCodeFiles.Insert(0, fileToInsert);            // and insert the file at the top of the list (so that it is compiled first)
        }*/
        public static void addReferencesIncludedInSourceCode(string sourceCodeFile, List<string> referencedAssemblies)
        {
            addReferencesIncludedInSourceCode(new List<string>().add(sourceCodeFile),referencedAssemblies);
        }
        public static void addReferencesIncludedInSourceCode(List<string> sourceCodeFiles, List<string> referencedAssemblies)
        {
            // the onlyAddReferencedAssemblies check needs to be done seperately for all files
            foreach( var sourceCodeFile in sourceCodeFiles)
            {
                // handle special case where we want (for performace & other reasons) be explicit on the references we add to the current script
                // note that the special tag must be the first line of the source code file
                // (this case is a bit of a legacy from the earlier versions of this code which did not had good support for References)
                var sourceCode = Files.getFileContents(sourceCodeFile);
                if (sourceCode.starts("//"+ _onlyAddReferencedAssemblies))
                {
                    referencedAssemblies.Clear();
                    break;              // once one the files has the onlyAddReferencedAssemblies ref, we can clear the referencedAssemblies and break the loop
                }
            }

            foreach (var sourceCodeFile in sourceCodeFiles)
            {
                // extract the names from referencedAssemblies
                var referencedAssembliesFileNames = new List<String>();
                foreach(var referencedAssembly in referencedAssemblies)
                    referencedAssembliesFileNames.Add(Path.GetFileName(referencedAssembly));
                // search for references in the source code
                var fileLines = Files.getFileLines(sourceCodeFile);

                foreach (var fileLine in fileLines)
                {
                    var match = StringsAndLists.TextStartsWithStringListItem(fileLine, _specialO2Tag_ExtraReferences);
                    //if (fileLine.StartsWith(specialO2Tag_ExtraReferences))
                    if (match!="")
                    {
                        var extraReference = fileLine.Replace(match, "").Trim();
                        //if (File.Exists(extraReference) && false == referencedAssemblies.Contains(extraReference))
                        var extraReferenceFileName = Path.GetFileName(extraReference);
                        if (false == referencedAssembliesFileNames.Contains(extraReferenceFileName))
                        {
//                            if (true == extraReference.fileExists() &&
//                                false ==Path.Combine(PublicDI.config.O2TempDir,extraReference.fileName()).fileExists())
//                                Files.Copy(extraReference, PublicDI.config.O2TempDir, true);
                            /*var assembly = PublicDI.reflection.getAssembly(extraReference);
                            if (assembly == null)
                                PublicDI.log.error("(this could be a problem for execution) in addReferencesIncludedInSourceCode could not load assembly :{0}", extraReference);
                             */
                            referencedAssembliesFileNames.
                                Add(extraReferenceFileName);
                            referencedAssemblies.Add(extraReference);
                        }
                    }
                }

            }
        }
        public static List<string> getListOfO2AssembliesInExecutionDir()
        {            
            var o2AssembliesInExecutionDir = new List<string>();
            o2AssembliesInExecutionDir.AddRange(Files.getFilesFromDir_returnFullPath(PublicDI.config.CurrentExecutableDirectory, "*O2*.dll"));
            o2AssembliesInExecutionDir.AddRange(Files.getFilesFromDir_returnFullPath(PublicDI.config.CurrentExecutableDirectory, "*O2*.exe"));            
            return o2AssembliesInExecutionDir;
        }
        public List<string> getListOfReferencedAssembliesToUse()
        {
            // let's add everything in the current executabled dir :)
            var referencedAssemblies = new List<string>();
            //referencedAssemblies.add_OnlyNewItems(getListOfO2AssembliesInExecutionDir());
            referencedAssemblies.add_OnlyNewItems(DefaultReferencedAssemblies); // the a couple from the GAC
            return referencedAssemblies;
        }        
        public bool compileSourceFiles(List<String> sourceCodeFiles, String[] sReferenceAssembliesToAdd,
                                             ref Assembly aCompiledAssembly, ref String sErrorMessages,
                                             bool bVerbose, string exeMainClass, string outputAssemblyName)
        {
            try
            {
                sourceCodeFiles = sourceCodeFiles.onlyValidFiles();
                showErrorMessageIfPathHasParentheses(sourceCodeFiles);
                if (outputAssemblyName == "")
                    outputAssemblyName = PublicDI.config.TempFileNameInTempDirectory;
                
                if (!outputAssemblyName.parentFolder().dirExists())
                    outputAssemblyName = Path.Combine(PublicDI.config.O2TempDir, outputAssemblyName);
                if (bVerbose)
                    PublicDI.log.debug("Dynamically compiling Source code...");                
                
                foreach(var file in sourceCodeFiles)
                    if(file.fileContents().lines().starting("//CLR_3.5").notEmpty())        // allow setting compilation into 2.0 CLR
                    {
                        compilationVersion= "v3.5";
                        break;
                    }

                if (cscpCSharpCodeProvider == null)
                {
                    var providerOptions = new Dictionary<string, string>().add("CompilerVersion", compilationVersion);
                    cscpCSharpCodeProvider = new CSharpCodeProvider(providerOptions);
                }
                cpCompilerParameters = new CompilerParameters();
                                               /*{
                                                   GenerateInMemory = false,
                                                   IncludeDebugInformation = true,
                                                   OutputAssembly = outputAssemblyName
                                               };*/  // Object contstuctors are not supported by Roslyn CTP 2002
                cpCompilerParameters.GenerateInMemory = false;
                cpCompilerParameters.IncludeDebugInformation = true;
                cpCompilerParameters.OutputAssembly = outputAssemblyName;
                
                if (exeMainClass == "")
                    cpCompilerParameters.OutputAssembly += ".dll";
                else
                {
                    cpCompilerParameters.OutputAssembly += ".exe";
                    cpCompilerParameters.MainClass = exeMainClass;
                    cpCompilerParameters.GenerateExecutable = true;
                }


                // need to add a solution to add LinkedResources to these dynamic compilation dlls
                //                cpCompilerParameters.LinkedResources.Add();
                

                // I was doing this all in memory but with the Add-ons there were probs running some of the O2JavaScript d
                //                cpCompilerParameters.GenerateInMemory = true;                
                if (null != sReferenceAssembliesToAdd)
                    foreach (String sReferenceAssembly in sReferenceAssembliesToAdd)
                    {
						//HACK to deal with the fact that we are passing 4.0 assemblies as references
						if (compilationVersion == "v3.5" && sReferenceAssembly.contains("v4.0"))
						{
							cpCompilerParameters.ReferencedAssemblies.Add(sReferenceAssembly.fileName());
						}
						else 
							if (File.Exists(sReferenceAssembly))
								cpCompilerParameters.ReferencedAssemblies.Add(sReferenceAssembly);
                    }
                crCompilerResults = cscpCSharpCodeProvider.CompileAssemblyFromFile(cpCompilerParameters, sourceCodeFiles.ToArray());
                

                if (crCompilerResults.Errors.Count == 0)
                {
                    if (bVerbose)
                        PublicDI.log.debug("There were no errors...");
                    aCompiledAssembly = crCompilerResults.CompiledAssembly;
                    return true;                    
                }

                sbErrorMessage = new StringBuilder();
                foreach (CompilerError ceCompilerError in crCompilerResults.Errors)
                {
                    sbErrorMessage.AppendLine(String.Format("{0}::{1}::{2}::{3}::{4}", ceCompilerError.Line,
                                                            ceCompilerError.Column, ceCompilerError.ErrorNumber,
                                                            ceCompilerError.ErrorText, ceCompilerError.FileName));
                    //sErrorMessages += ceCompilerError.ErrorText + Environment.NewLine;
                }
                sErrorMessages = sbErrorMessage.ToString();
                if (bVerbose)
                    PublicDI.log.error("Compilatation errors \n\n {0}", sErrorMessages);                
            }
            catch (Exception ex)
            {
                PublicDI.log.error("In compileSourceCode_CSharp: {0}", ex.Message);
            }
            return false;
        }        
        private void showErrorMessageIfPathHasParentheses(List<string> sourceCodeFiles)
        {
            foreach(var file in sourceCodeFiles)
                if(file.Contains("(") || file.Contains(")"))
                    PublicDI.log.error("File to compile had a parentheses so error messages will not have line numbers (see http://forums.asp.net/p/1009965/1556589.aspx): {0}", file);
        }
        public static Dictionary<string, string> resetLocalScriptsFileMappings()
        {
            clearLocalScriptFileMappings();
            var filesToSearch = PublicDI.config.LocalScriptsFolder.files(true);
            foreach (var localScriptFile in filesToSearch)
            {
                if (localScriptFile.contains(@"\.git").isFalse())
                {
                    var key = localScriptFile.fileName().ToLower();
                    CompileEngine.LocalScriptFileMappings.add(key, localScriptFile);
                }
            }            
            return CompileEngine.LocalScriptFileMappings;                
        }
        public static string findFileOnLocalScriptFolder(string file, string searchInFolder)
        { 
            if (searchInFolder.valid() && searchInFolder.pathCombine(file).fileExists())
               return searchInFolder.pathCombine(file);
            return findFileOnLocalScriptFolder(file);
        }
        public static string findFileOnLocalScriptFolder(string file)
        {
            try
            {               
                //if (LocalScriptFileMappings.size() == 0)  // if there are no mappings create the cached list
                if (LocalScriptFileMappings.hasKey("Util - LogViewer.h2".lower()).isFalse()) // this script should be there, if not it means we need to refresh this list
                    resetLocalScriptsFileMappings();

                var key = file.ToLower();
                if (CompileEngine.LocalScriptFileMappings.hasKey(key))
                    return CompileEngine.LocalScriptFileMappings[key];
                //var localFile = file.pathCombine_With_ExecutingAssembly_Folder();

                var extraFoldersToFindFile = CompileEngine.LocalFoldersToSearchForCodeFiles.toList()
                                                          .add(Environment.CurrentDirectory)
                                                          .add(Assembly.GetExecutingAssembly().location().parentFolder())
                                                          .add(Assembly.GetCallingAssembly().location().parentFolder())
                                                          .add(Assembly.GetEntryAssembly().location().parentFolder())
														  .add(O2.Kernel.PublicDI.config.ToolsOrApis)
                                                          .onlyValidFolders()
                                                          .unique();

                foreach (var extraFolder in extraFoldersToFindFile)
                {
                    var mapping = extraFolder.pathCombine(file);
                    if (mapping.fileExists())
                    {
                        "[findFileOnLocalScriptFolder] mapped '{0}' to file '{1}".info(file, mapping);
                        CompileEngine.LocalScriptFileMappings.add(file, mapping);
                        return mapping;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.log();
            }
            "[findFileOnLocalScriptFolder]in CompileEngine, could NOT map file reference '{0}'".debug(file);
            return "";                    
        }
        public static string findScriptOnLocalScriptFolder(string file)
        {
            return findFileOnLocalScriptFolder(file);

           /* if (file.contains("/", @"\"))       // currenlty relative paths are not supported
                return "";

            //string defaultLocalScriptsFolder = @"C:\O2\O2Scripts_Database\_Scripts";

            if (LocalScriptFileMappings.hasKey(file))
                return LocalScriptFileMappings[file];
            var mappedFilePath = "";

            var filesToSearch = PublicDI.config.LocalScriptsFolder.files(true, "*.cs");
            filesToSearch.add(PublicDI.config.LocalScriptsFolder.files(true, "*.o2"));
            foreach (var localScriptFile in filesToSearch)
            {
                if (localScriptFile.fileName().ToLower().StartsWith(file.ToLower()))
                //if (fileToResolve.lower() == localScriptFile.fileName().lower())
                {
                    PublicDI.log.debug("in CompileEngine, file reference '{0}' was mapped to local O2 Script file '{1}'",file, localScriptFile);
                    mappedFilePath = localScriptFile;
                    break;
                }
            }
            if (mappedFilePath.valid())
                LocalScriptFileMappings.add(file, mappedFilePath);
            return mappedFilePath;
            * */
        }
        public static string getCachedCompiledAssembly(string scriptOrFile)
        {
            return getCachedCompiledAssembly(scriptOrFile, true);
        }
        public static string getCachedCompiledAssembly(string scriptOrFile, bool compileIfNotFound)
        {
            if (CachedCompiledAssemblies.hasKey(scriptOrFile))
            {
                var pathToDll = CachedCompiledAssemblies[scriptOrFile];
                if (scriptOrFile.isFile())
                    "in getCachedCompiledAssembly, mapped file '{0}' to cached assembly '{1}'".debug(scriptOrFile, pathToDll);
                else
                    "in getCachedCompiledAssembly, found cached assembly for script/md5hash with size '{0}' to cached assembly '{1}'".debug(scriptOrFile.size(), pathToDll);
                return pathToDll;
            }
            if (compileIfNotFound.isFalse())
                return null;            
            var mappedFile = CompileEngine.findScriptOnLocalScriptFolder(scriptOrFile);
            //var sourceCode = mappedFile.fileContents();
            //if (sourceCode.contains("//generateDebugSymbols").isFalse())
                //sourceCode += "//generateDebugSymbols".lineBefore();
            var assembly = new CompileEngine().compileSourceFile(mappedFile);
            if (assembly != null && assembly.Location.fileExists())
            {
                var pathToDll = assembly.Location;
                //CachedCompiledAssemblies.add(scriptOrFile, pathToDll);
                //CachedCompiledAssemblies.add(assembly.GetName().Name, pathToDll);
                CompileEngine.setCachedCompiledAssembly(scriptOrFile, assembly);
                "in getCachedCompiledAssembly, compiled file '{0}' to assembly '{1}' (and added it to CachedCompiledAssembly)".debug(scriptOrFile, pathToDll);
                return assembly.Location;
            }
            return "";
        }
        public static string resolveCompilationReferencePath(string reference)
        {
            if (CachedCompiledAssemblies.ContainsKey(reference))    // check in CachedCompiledAssemblies first
            {
                var resolvedRefernece = CachedCompiledAssemblies[reference];
                if (resolvedRefernece.fileExists())               // ensure the file is still there
                    return resolvedRefernece;
                CachedCompiledAssemblies.remove(reference);
            }
            if (reference.fileExists().isFalse())
            {
                if (reference.index(",") > 0)
                    reference = reference.split(",").first();
                var resolvedFile = PublicDI.CurrentScript.directoryName().pathCombine(reference);   
                if (resolvedFile.fileExists())
                    return resolvedFile;
                foreach (var localReferenceFolder in LocalReferenceFolders)
                {
                    resolvedFile = localReferenceFolder.pathCombine(reference);
                    if (resolvedFile.fileExists())
                        return resolvedFile;                    
                    resolvedFile = localReferenceFolder.pathCombine(reference + ".dll");
                    if (resolvedFile.fileExists())
                        return resolvedFile;
                    resolvedFile = localReferenceFolder.pathCombine(reference + ".exe");
                    if (resolvedFile.fileExists())
                            return resolvedFile;                    
                }
                
            }
            return reference;
        }
        public static string resolve_Assembly_ToAddTo_ReferencedAssemblies_List(string originalReference, bool workOffline)
        {            
            var resolvedReference = resolveCompilationReferencePath(originalReference);

            var assembly = resolvedReference.assembly();
            if (assembly.isNull())
            {
                if (workOffline.isFalse() && resolvedReference.fileExists().isFalse())
                {
                    new O2GitHub().tryToFetchAssemblyFromO2GitHub(resolvedReference);
                    assembly = resolvedReference.assembly();
                }
            }
            if (assembly.notNull())
            {
                if (assembly.Location.notValid())
                {
                    var embeddedAssemblyLocation = AssemblyResolver.saveEmbeddedAssemblyToDisk(assembly.GetName());
                    if (embeddedAssemblyLocation.fileExists())
                    {
                        assembly = embeddedAssemblyLocation.assembly();
                        if (assembly.notNull())
                        {
                            resolvedReference = embeddedAssemblyLocation;
                            setCachedCompiledAssembly(originalReference, resolvedReference, true);
                            return resolvedReference;
                        }
                    }
                }
                else
                {
                    if (resolvedReference != assembly.Location)
                    {
                        resolvedReference = assembly.Location;
                        setCachedCompiledAssembly(originalReference, resolvedReference, false);                        
                    }
                    return resolvedReference;
                }
            }
                        
            if (CachedCompiledAssemblies.ContainsKey(originalReference))    // removed it form here and try again)
            {
                CachedCompiledAssemblies.Remove(originalReference);
                return resolve_Assembly_ToAddTo_ReferencedAssemblies_List(originalReference, workOffline);
            }
            
            "[tryToResolveReferencesForCompilation] failed to resolve or load assembly reference: {0}".error(resolvedReference);
            return null;
        }
        public static void add_Assembly_Resolved_ReferencedAssemblies_List(List<string> resolvedAssemblies, string originalReference, bool workOffline)
        {
            try
            {  
                if (resolvedAssemblies.contains(originalReference))
                    return;
                var resolvedAssembly = resolve_Assembly_ToAddTo_ReferencedAssemblies_List(originalReference, workOffline);                
                if (resolvedAssembly.notNull() && resolvedAssemblies.contains(resolvedAssembly).isFalse())
                    resolvedAssemblies.Add(resolvedAssembly);                    
            }
            catch (Exception ex)
            {
                "[tryToResolveReferencesForCompilation][resolve_Assembly_ToAddTo_ReferencedAssemblies_List]  {0} for '{1}'".error(ex.Message, originalReference);
            }
        }
        public static void tryToResolveReferencesForCompilation(List<string> referencedAssemblies, bool workOffline)
        {            
            var resolvedAssemblies = new List<string>();
            for (int i = 0; i < referencedAssemblies.size(); i++)
            {
                add_Assembly_Resolved_ReferencedAssemblies_List(resolvedAssemblies, referencedAssemblies[i].trim(), workOffline);			
            }
            referencedAssemblies.clear()
                                .add(resolvedAssemblies);
            saveCachedCompiledAssembliesMappings();         
        }
        public static void populateCachedListOfGacAssemblies()
        {
            if (O2GitHub.AssembliesCheckedIfExists.size() < 50)
            {
                var gacAssemblies = GacUtils.assemblyNames();   
                if (gacAssemblies.contains("Microsoft.mshtml"))     // have to hard-code this one since there are cases where this is in the GAC but the load fails
                    gacAssemblies.Remove("Microsoft.mshtml");
                O2GitHub.AssembliesCheckedIfExists.add_OnlyNewItems(gacAssemblies);
            }
        }
/*        public static bool isFileAReferenceARequestToUseThePrevioulsyCompiledVersion(string fileToResolve, List<string> ReferencedAssemblies)
        {
            if (fileToResolve.starts("Ref:"))
            {
                fileToResolve = fileToResolve.remove("Ref:");
                var fileRef = CompileEngine.getCachedCompiledAssembly(fileToResolve);

                if (fileRef.valid() && fileRef.fileExists())
                {
                    if (ReferencedAssemblies.contains(fileRef).isFalse())
                        ReferencedAssemblies.add(fileRef);
                }                
                return true;
            }
            else
                return false;
        }*/
        public static void handleReferencedAssembliesInstallRequirements(List<string> files)
        {
            foreach (var file in files)
                handleReferencedAssembliesInstallRequirements(file);
        }
        public static void handleReferencedAssembliesInstallRequirements(string fileOrCode)
        {
            var specialTag = "//Installer:";
            var code = (fileOrCode.isFile()) ? fileOrCode.fileContents() : fileOrCode;
            if (code.contains(specialTag))
            {
                foreach (var line in code.lines().containing(specialTag))
                {
                    var refs = line.remove(specialTag).split("!");
                    if (refs.size() != 2)
                        "[handleReferencedAssembliesInstallRequirements] there should be two values in the {0} reference (1st O2 script, 2nd: expected dll".error(specialTag);
                    else 
                    {
                        var o2Script = refs.first();
                        var expectedFile = refs.second();
                        //if (expectedDll.assembly().isNull())
						if (expectedFile.local().fileExists().isFalse()) //mapping it to the file instead of an assembly (to support unmanaged exes)
                        {
							"[handleReferencedAssembliesInstallRequirements] expected assembly not found ('{0}'), so running installer script: '{1}'".info(expectedFile, o2Script);
                            var assembly = new CompileEngine().compileSourceFile(o2Script.local());
                            if (assembly.notNull())
                            {
                                var installType = assembly.type(o2Script.fileName_WithoutExtension());
                                if (installType.isNull())
                                    "[handleReferencedAssembliesInstallRequirements] could not find expected type: {0}".error(o2Script.fileName_WithoutExtension());
                                else
                                {
                                    installType.ctor(); // the installer is supposed to be triggered by the  constructor
									//if (expectedFile.assembly().isNull())
									if (expectedFile.local().fileExists().isFalse())
										"[handleReferencedAssembliesInstallRequirements] after install requested assembly still not found: '{0}'".error(expectedFile);
                                    else
										"[handleReferencedAssembliesInstallRequirements] after install requested assembly is now available: '{0}'".info(expectedFile);
                                }
                            }
                        }
                    }
                
                }
            }
        }
    }	
}
