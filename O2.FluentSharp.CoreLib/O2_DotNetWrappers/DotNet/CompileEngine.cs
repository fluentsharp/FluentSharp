// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.CSharp;
using O2.DotNetWrappers.ExtensionMethods;
using O2.DotNetWrappers.Filters;
using O2.DotNetWrappers.O2Misc;
using O2.DotNetWrappers.Windows;
using O2.Kernel;
using O2.Kernel.ExtensionMethods;
using O2.Kernel.CodeUtils;
using O2.Kernel.Objects;

namespace O2.DotNetWrappers.DotNet
{
    public class CompileEngine
    {                
        static string onlyAddReferencedAssemblies = "O2Tag_OnlyAddReferencedAssemblies";        
        static List<string> specialO2Tag_ExtraReferences 	= new List<string>();
        static List<string> specialO2Tag_Download 			= new List<string>();
        static List<string> specialO2Tag_PathMapping 		= new List<string>();
        static List<string> specialO2Tag_ExtraSourceFile 	= new List<string>();
        static List<string> specialO2Tag_ExtraFolder 		= new List<string>();
        static List<string> specialO2Tag_DontCompile		= new List<string>();

        public Assembly				compiledAssembly;
        public CompilerParameters	cpCompilerParameters;
        public CompilerResults		crCompilerResults;
        public CSharpCodeProvider	cscpCSharpCodeProvider;
        public StringBuilder		sbErrorMessage;
        public bool DebugMode;
        public bool UseCachedAssemblyIfAvailable;
        public static Dictionary<string, string> LocalScriptFileMappings = new Dictionary<string, string>();
		public static List<string> LocalReferenceFolders				 = new List<string>();
        public static Dictionary<string, string> CachedCompiledAssemblies = new Dictionary<string, string>();
        public static string					 CachedCompiledAssembliesMappingsFile = PublicDI.config.O2TempDir.pathCombine("..\\CachedCompiledAssembliesMappings.xml");
        public static Dictionary<string,string>  CompilationPathMappings = new Dictionary<string, string>();
		

        // the first time were here, load up the mappings from the CachedCompiledAssembliesMappingsFile
        static CompileEngine() 
        {
            loadCachedCompiledAssembliesMappings();
            setDefaultLocalReferenceFolders();
            specialO2Tag_ExtraReferences	.add("//O2Tag_AddReferenceFile:")
            						  		.add("//O2Ref:");
            specialO2Tag_Download			.add("//Download:")
            								.add("//O2Download:");
            specialO2Tag_PathMapping 		.add("//PathMapping:")
            								.add("//O2PathMapping:");
            specialO2Tag_ExtraSourceFile  	.add("//O2Tag_AddSourceFile:")
            								.add("//O2File:");
            specialO2Tag_ExtraFolder		.add("//O2Tag_AddSourceFolder:")
                        					.add("//O2Folder:")
                        					.add("//O2Dir:");
            specialO2Tag_DontCompile   		.add("//O2NoCompile");
        }

        public CompileEngine() : this (true)
        {
            
        }

        public CompileEngine(bool useCachedAssemblyIfAvailable)
        {
            UseCachedAssemblyIfAvailable = useCachedAssemblyIfAvailable;            
        }

        public static List<String> get_GACExtraReferencesToAdd()
        {
            return new [] {
                                "System.Windows.Forms.dll",
                                "System.Drawing.dll",
                                "System.Data.dll",
                                "System.Xml.dll",
                                "System.Web.dll",
                                "System.Core.dll",
                                "System.Xml.Linq.dll",
                                "System.Xml.dll",
                                "System.dll",
                            //O2Related
                                "O2_FluentSharp_CoreLib.dll",
                                "O2_FluentSharp_BCL.dll",
                                "O2_External_SharpDevelop.dll",
                                "O2SharpDevelop.dll",
                                //,
                            //WPF 
//                                                                                    "PresentationCore.dll",
//                                                                                    "PresentationFramework.dll",
//                                                                                    "WindowsBase.dll",
//                                                                                    "System.Core.dll"
                            // to support the use of dynamic:
                               "Microsoft.CSharp.dll" 
                            }.toList();
        
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
                var keyValueStrings = CompileEngine.CachedCompiledAssemblies.toKeyValueStrings();
                keyValueStrings.saveAs(CachedCompiledAssembliesMappingsFile);                 
            }
            catch (Exception ex)
            {
                ex.log("in loadCachedCompiledAssembliesMappings");
            }
        }

        public static void setDefaultLocalReferenceFolders()
        { 
            LocalReferenceFolders.Add(PublicDI.config.ReferencesDownloadLocation);
            LocalReferenceFolders.Add(PublicDI.config.ToolsOrApis);   
        }

        public Assembly compileSourceCode(String sourceCodeFile)
        {
            return compileSourceCode(sourceCodeFile, "", "");
        }
        public Assembly compileSourceCode(String sourceCodeFile, string mainClass,
                                                 string outputAssemblyName)
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

        public Assembly compileSourceFiles(List<String> sourceCodeFiles, string mainClass,
                                                 string outputAssemblyName)
        {
            return compileSourceFiles(sourceCodeFiles, mainClass, outputAssemblyName, false);
        }

        public Assembly compileSourceFiles(List<String> sourceCodeFiles, string mainClass,
                                                 string outputAssemblyName, bool workOffline)
        {            
            sourceCodeFiles = checkForNoCompileFiles(sourceCodeFiles);
            var filesMd5 = sourceCodeFiles.filesContents().md5Hash();

            if (UseCachedAssemblyIfAvailable)
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
            //foreach (var assembly in referencedAssemblies)
            //    "R: {0}".info(assembly);
            PublicDI.log.error("Compilation failed: {0}", errorMessages);
            return null;
        }

		public static bool loadReferencedAssembliesIntoMemory(Assembly targetAssembly)
		{			
			foreach (var assemblyName in targetAssembly.GetReferencedAssemblies())
			{
				Assembly assembly = null;
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

        private void setCachedCompiledAssembly(List<string> sourceCodeFiles, Assembly compiledAssembly)
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

        public static void setCachedCompiledAssembly(string key, Assembly compiledAssembly)
        {
            if (key.valid() && 
                compiledAssembly.notNull() && 
                compiledAssembly.Location.valid() &&
                compiledAssembly.Location.fileExists())
            {                                            
                CachedCompiledAssemblies.add(key, compiledAssembly.Location);
                CachedCompiledAssemblies.add(compiledAssembly.GetName().Name, compiledAssembly.Location);
                saveCachedCompiledAssembliesMappings();                
            }
        }

        public static void setCachedCompiledAssembly(string key, string mapping)
        {
            if (key.valid() && mapping.valid())
            {
                CachedCompiledAssemblies.add(key, mapping);
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
                    if (assembly.ImageRuntimeVersion == Assembly.GetExecutingAssembly().ImageRuntimeVersion)
                        return assembly;
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
                if (DebugMode)
                    foreach (var referencedAssembly in referencedAssemblies)
                        PublicDI.log.debug("   {0}", referencedAssembly);
            }
            
        }

        public static Dictionary<string,string> clearCompilationPathMappings()
        {
            "in clearCompilationPathMappings".info();
            CompilationPathMappings.Clear();
            return CompilationPathMappings;
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
                    if ("" != StringsAndLists.InFileTextStartsWithStringListItem(sourceCodeFile, specialO2Tag_DontCompile))
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
			Action<string,string> add_to_FilesToLoad = 
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
                        var match = StringsAndLists.TextStartsWithStringListItem(fileLine, specialO2Tag_ExtraSourceFile);
                        if (match != "")
                        {
                            //   var file = fileLine.Replace(specialO2Tag_ExtraSourceFile, "").Trim();
                            var file = fileLine.Replace(match, "").Trim();
							if (false == sourceCodeFiles.Contains(file) && false == filesToLoad.Contains(file))
                            {
                                //handle the File:xxx:Ref:xxx case
                                if (CompileEngine.isFileAReferenceARequestToUseThePrevioulsyCompiledVersion(file, referencedAssemblies)
                                                 .isFalse())
									add_to_FilesToLoad(sourceCodeFile, file);
                            }
                        }
                        //else if (fileLine.StartsWith(specialO2Tag_ExtraFolder))
                        else
                        {
                            match = StringsAndLists.TextStartsWithStringListItem(fileLine, specialO2Tag_ExtraFolder);
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
										add_to_FilesToLoad(sourceCodeFile, file);
                            }

                            else
                            {
                                match = StringsAndLists.TextStartsWithStringListItem(fileLine, specialO2Tag_PathMapping);
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
				PublicDI.log.info("There are {0} extra files to add to the list of source code files to compile: {0}", filesToLoad.Count);
				foreach (var file in filesToLoad)
                {                    
                    var filePath = "";
                    if (File.Exists(file))
                        filePath = file;
                    else
                    {
						if (file.local().fileExists())
							filePath = file.local();
						else
						{
							// try to find the file in the current sourceCodeFiles directories                        
							foreach (var directory in currentSourceDirectories)
								if (File.Exists(Path.Combine(directory, file)))
								{
									filePath = Path.Combine(directory, file);
									break;
								}							
						}
						if (filePath == "")
						{
							PublicDI.log.error("in addSourceFileOrFolderIncludedInSourceCode, could not file file to add: {0}", file);
							filesToLoad.add(file);
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
                if (sourceCode.starts("//"+ onlyAddReferencedAssemblies))
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
                    var match = StringsAndLists.TextStartsWithStringListItem(fileLine, specialO2Tag_ExtraReferences);
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
                                DI.log.error("(this could be a problem for execution) in addReferencesIncludedInSourceCode could not load assembly :{0}", extraReference);
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
            referencedAssemblies.add_OnlyNewItems(get_GACExtraReferencesToAdd()); // the a couple from the GAC
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
                if (!Directory.Exists(Path.GetDirectoryName(outputAssemblyName)))
                    outputAssemblyName = Path.Combine(PublicDI.config.O2TempDir, outputAssemblyName);
                if (bVerbose)
                    PublicDI.log.debug("Dynamically compiling Source code...");
                String sDefaultPathToResolveReferenceAssesmblies = PublicDI.config.O2TempDir;
                
                if (cscpCSharpCodeProvider == null)
                {
                    //var providerOptions = new Dictionary<string, string> {{"CompilerVersion", "v3.5"}};
                    var providerOptions = new Dictionary<string, string>().add("CompilerVersion", "v4.0" );
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
						if (File.Exists(sReferenceAssembly))
							cpCompilerParameters.ReferencedAssemblies.Add(sReferenceAssembly);
				//		else
				//			"[compileSourceFiles] in cpCompilerParameters.ReferencedAssemblies.Add, could not find: {0}".error(sReferenceAssembly);
/*                        try
                        {
                            // first try to resolve it in the current directory
                            String sResolvedAssemblyName = Path.Combine(PublicDI.config.CurrentExecutableDirectory,
                                                                        sReferenceAssembly);
                            if (File.Exists(sResolvedAssemblyName))
                                cpCompilerParameters.ReferencedAssemblies.Add(sResolvedAssemblyName);
                            else
                            {
                                // if not try the _temp directory
                                sResolvedAssemblyName = Path.Combine(sDefaultPathToResolveReferenceAssesmblies,
                                                                     sReferenceAssembly);
                                if (File.Exists(sResolvedAssemblyName))
                                    cpCompilerParameters.ReferencedAssemblies.Add(sResolvedAssemblyName);
                                else
                                    // in case it is in the GAC, just add the name directly
                                    cpCompilerParameters.ReferencedAssemblies.Add(sReferenceAssembly);
                            }
                        }
                        catch (Exception)
                        { }
						*/
                        //PublicDI.log.error("in compileSourceCode_CSharp, could not resolve path to reference assembly :{0}", sReferenceAssembly);
                    }
                //cpCompilerParameters.ReferencedAssemblies.AddRange(sReferenceAssembliesToAdd);                
                //CompilerResults crCompilerResults =
                //    cscpCSharpCodeProvider.CompileAssemblyFromSource(cpCompilerParameters, sSourceCode);
                //cpCompilerParameters.LinkedResources.Add(@"C:\O2\_XRules_Local\CodeCompletion\O2CodeComplete.resx");                
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
        public static string findFileOnLocalScriptFolder(string file)
        {
            //if (LocalScriptFileMappings.size() == 0)  // if there are no mappings create the cached list
			if (LocalScriptFileMappings.hasKey("Util - LogViewer.h2".lower()).isFalse()) // this script should be there, if not it means we need to refresh this list
                resetLocalScriptsFileMappings();

            var key = file.ToLower();
            if (CompileEngine.LocalScriptFileMappings.hasKey(key))
                return CompileEngine.LocalScriptFileMappings[key];
            PublicDI.log.error("in CompileEngine, could NOT map file reference '{0}'", file);
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
                return  CachedCompiledAssemblies[reference];
			if (reference.fileExists().isFalse())
			{
				if (reference.IndexOf(",") > 0)
					reference = reference.split(",").first();
				/*var resolvedFile = PublicDI.config.ReferencesDownloadLocation.pathCombine(reference);
				if (resolvedFile.fileExists())
					return resolvedFile;*/
                var resolvedFile = PublicDI.CurrentScript.directoryName().pathCombine(reference);   
                if (resolvedFile.fileExists())
					return resolvedFile;
				foreach (var localReferenceFolder in LocalReferenceFolders)
				{
					resolvedFile = localReferenceFolder.pathCombine(reference);
					if (resolvedFile.fileExists())
						return resolvedFile;
					/*resolvedFile = localReferenceFolder.pathCombine(reference + ".dll");
					if (resolvedFile.fileExists())
						return resolvedFile;
					resolvedFile = localReferenceFolder.pathCombine(reference + ".exe");
					if (resolvedFile.fileExists())
						return resolvedFile;*/
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
            if (assembly.notNull() && assembly.Location.fileExists())
            {
                if (resolvedReference != assembly.Location)
                {
                    resolvedReference = assembly.Location;
                    CompileEngine.setCachedCompiledAssembly(originalReference, resolvedReference);
                }                
                return resolvedReference;
            }
            else
            {
                if (CachedCompiledAssemblies.ContainsKey(originalReference))    // removed it form here and try again)
                {
                    CachedCompiledAssemblies.Remove(originalReference);
                    return resolve_Assembly_ToAddTo_ReferencedAssemblies_List(originalReference, workOffline);
                }
                else
                {
                    "[tryToResolveReferencesForCompilation] failed to resolve or load assembly reference: {0}".error(resolvedReference);
                    return null;
                }
            }
        }

        public static void add_Assembly_Resolved_ReferencedAssemblies_List(List<string> resolvedAssemblies, string originalReference, bool workOffline)
        {
            try
			{  
                if (resolvedAssemblies.contains(originalReference))
                    return;
                var resolvedAssembly = resolve_Assembly_ToAddTo_ReferencedAssemblies_List(originalReference, workOffline);
                //if (resolvedAssembly.notNull() && originalReference != resolvedAssembly && resolvedAssemblies.contains(resolvedAssembly).isFalse())
                if (resolvedAssembly.notNull() && resolvedAssemblies.contains(resolvedAssembly).isFalse())
                {                      
                    resolvedAssemblies.Add(resolvedAssembly);
                    return;

                    foreach (var referencedAssembly in resolvedAssembly.assembly().GetReferencedAssemblies())
                    {
                        var fullName = referencedAssembly.FullName;
                        //var moduleAssembly = module.Assembly.Location;
                        add_Assembly_Resolved_ReferencedAssemblies_List(resolvedAssemblies, fullName, workOffline);
                    }
                }
            }
			catch (Exception ex)
			{
                "[tryToResolveReferencesForCompilation][resolve_Assembly_ToAddTo_ReferencedAssemblies_List]  {0} for '{1}'".error(ex.Message, originalReference);
			}
        }

        public static void tryToResolveReferencesForCompilation(List<string> referencedAssemblies, bool workOffline)
        {
            var o2Timer = new O2Timer("tryToResolveReferencesForCompilation").start();
            var currentExecutablePath = PublicDI.config.CurrentExecutableDirectory;
            var resolvedAssemblies = new List<string>();
			for (int i = 0; i < referencedAssemblies.size(); i++)
            {
                add_Assembly_Resolved_ReferencedAssemblies_List(resolvedAssemblies, referencedAssemblies[i].trim(), workOffline);			
            }
            referencedAssemblies.clear()
                                .add(resolvedAssemblies);
            
            //"[tryToResolveReferencesForCompilation]: There were {0} assemblies resolved".info(resolvedAssemblies.size());
            //o2Timer.stop();
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

        public static bool isFileAReferenceARequestToUseThePrevioulsyCompiledVersion(string fileToResolve, List<string> ReferencedAssemblies)
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
        }
    }
}
