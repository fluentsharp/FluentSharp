using System.Collections.Generic;

using System.Reflection;
using FluentSharp.CoreLib.API;

namespace FluentSharp.CoreLib
{
    public static class Compilation_ExtensionMethods
    {
        public static string compileIntoDll_inFolder(this string fileToCompile, string targetFolder)
        { 
            return fileToCompile.compileIntoDll_inFolder(targetFolder,null);
        }

        public static string compileIntoDll_inFolder(this string fileToCompile, string targetFolder, string compilationVersion)
		{
			"Compiling file: {0} ".debug(fileToCompile);
			//var fileToCompile = currentFolder.pathCombine(file + ".cs");
			var filenameWithoutExtension = fileToCompile.fileName_WithoutExtension();
			var compiledDll = targetFolder.pathCombine(filenameWithoutExtension + ".dll");
			var mainClass = "";
			if (fileToCompile.fileExists().isFalse()) 
				"could not find file to compile: {0}".error(fileToCompile);  
			else
			{ 
				var assembly = new CompileEngine(compilationVersion).compileSourceFiles(new List<string>().add(fileToCompile), 
																	  mainClass, 
																	  filenameWithoutExtension);
				if (assembly.isNull()) 
					"no compiled assembly object created for: {0}".error(fileToCompile);
				else
				{ 
					Files.copy(assembly.Location, compiledDll);
					"Copied: {0} to {1}".info(assembly.Location, compiledDll);
					if (compiledDll.fileExists().isFalse())
						"compiled file not created in: {0}".error(compiledDll);
					else
						return compiledDll;
				}
			}  
			return null;
		}
		
		public static string compileToDll(this string sourceFile, string targetFolder)
		{
			return sourceFile.compileToExtension(".dll", "",targetFolder);
		}
		
		public static string compileToExe(this string sourceFile, string mainClass, string targetFolder)
		{			
			return sourceFile.compileToExtension(".exe", mainClass,targetFolder);
		}
		
		public static string compileToExtension(this string sourceFile, string targetExtension, string mainClass, string targetFolder)
		{
			var name = sourceFile.fileName_WithoutExtension();
			return name.compileToExtension(targetExtension, mainClass, sourceFile.parentFolder(), targetFolder);			
		}
		
		public static string compileToExtension(this string name, string extension,string mainClass, string currentFolder, string targetFolder)
    	{                	
            var fileToCompile = currentFolder.pathCombine(name + ".cs");
            var compiledDll = targetFolder.pathCombine(name + extension);
            if (fileToCompile.fileExists().isFalse())
                "could not find file to compile: {0}".error(fileToCompile); 
            else
            {
                var assembly = (mainClass.valid())   
                                    ? new CompileEngine().compileSourceFiles(new List<string>().add(fileToCompile), mainClass)
                                    : new CompileEngine().compileSourceFiles(new List<string>().add(fileToCompile), mainClass, System.IO.Path.GetFileNameWithoutExtension(compiledDll));
                if (assembly.isNull())
                    "no compiled assembly object created for: {0}".error(fileToCompile);
                else
                {
                	if (compiledDll.fileExists())
                		compiledDll.deleteFile();                		
                    Files.copy(assembly.Location, compiledDll);
                    "Copied: {0} to {1}".info(assembly.Location, compiledDll);
                    if (compiledDll.fileExists().isFalse())
                        "compiled file not created in: {0}".error(compiledDll);
                    else
                    	return compiledDll;
                   
                }
            } 
            return null;
		}

		public static List<Assembly> copyAssembliesToFolder(this string targetFolder,  params AssemblyName[] assemblyNames)
		{
			return targetFolder.copyAssembliesToFolder(true, assemblyNames);
		}
		
		public static List<Assembly> copyAssembliesToFolder(this string targetFolder , bool onlyCopyReferencesInO2ExecutionDir, params AssemblyName[] assemblyNames)
		{
			//"[copyReferencesToTargetFolder] copying {0} assemblies into {1}".info(assemblyNames.size(), targetFolder);
			var assembliesCopied = new List<Assembly>();
			foreach(var assemblyName in assemblyNames)
			{
				var assembly = assemblyName.assembly();
				if (assembly.notNull())
				{
					var location = assembly.Location;
                    if (location.notValid())
                    { 
                        location = AssemblyResolver.saveEmbeddedAssemblyToDisk(assemblyName);
                    }
					if (location.notValid())
						"[copyReferencesToTargetFolder] loaded assembly had no Location info: {0}".error(assembly.str());
					else
					{
						//if (onlyCopyReferencesInO2ExecutionDir.isFalse() || 
						//	location.parentFolder() == PublicDI.config.CurrentExecutableDirectory)
						//{							
						var targetFileName = location.fileName();
						if (targetFileName.isAssemblyName())
							targetFileName = "{0}.dll".format(targetFileName.assembly_Name());
						var targetFile = targetFolder.pathCombine(targetFileName);											
						if (targetFile.fileExists())
							"[copyReferencesToTargetFolder] skipping copy since it already exists in target folder: {0}".info(targetFile);
						else
						{
							Files.copy(location, targetFile);
							"[copyReferencesToTargetFolder] copied '{0}' to '{1}'".info(location, targetFile);
						}
						assembliesCopied.Add(assembly);
						//}
					}																
				}
				else
					"[copyReferencesToTargetFolder] could not load assembly for {0}".error(assemblyName);
			}
			return assembliesCopied;
		}
    }
}
