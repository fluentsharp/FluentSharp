using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using FluentSharp.CoreLib.API;

namespace FluentSharp.CoreLib
{    
    public static class EmbeddedResources_ExtensionMethods
    {
        public static Stream resourceStream(this string resourceName)
        {
            resourceName = resourceName.lower();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                foreach (var name in assembly.resourcesNames())
                    if (name.lower() == resourceName)
                        return assembly.resourceStream(name);
            return null;
        }
        public static Stream resourceStream(this Assembly assembly, string resourceName)
        {
            return assembly.GetManifestResourceStream(resourceName);
        }
        public static List<string> resourcesNames(this Assembly assembly)
        {
            if (assembly.isDynamic())
                return new List<string>();
            return assembly.GetManifestResourceNames().toList();
        }
                
        public static string local_Or_Resource(this string fileName)
        {
            var mappedFile = fileName.local();
            if (mappedFile.fileExists())
                return mappedFile;
            return fileName.resource_GetFile();
        }
        public static string resource_GetFile(this string resourceName)
        {
            var targetFile = resourceName.inTempDir();
            if (targetFile.fileExists())
                return targetFile;
            resourceName.resourceStream().bytes().saveAs(targetFile);
            if (targetFile.fileExists())
                return targetFile;
            "[EmbededResources]resource_GetFile failed for resourceName :{0}".error(resourceName);
            return null;
        }

        public static List<string> mapExtraEmbebbedResources(this string targetFolder, string fileToParse)
        {
            var extraEmbebbedResources = new List<string>();
            var sourceCode = fileToParse.local().contents().fix_CRLF();
            if (sourceCode.notValid())
            {
                "[mapExtraEmbebbedResources] could not get source code for file: {0}".format(fileToParse);
				return extraEmbebbedResources;
            }
			//Embed File or Assembly
            var tag = "//O2Embed:";
            foreach (var file in sourceCode.lines().starting(tag).replace(tag,""))
            {                                
                if (file.local().fileExists())
                {
                    "[mapExtraEmbebbedResources] found To Embed reference: {0} -> targetFolder: {1}".debug(file, targetFolder);                        
                    extraEmbebbedResources.add(file.local());
                }
				else if (file.assembly_Location().fileExists())
				{
					"[mapExtraEmbebbedResources] found Assembly To Embed reference: {0} -> targetFolder: {1}".debug(file.assembly_Location(), targetFolder);                        
					extraEmbebbedResources.add(file.assembly_Location());
				}
				else
				{						
					"[mapExtraEmbebbedResources] could not find Embedded reference: {0}".error(file);
				}                
            }			
            return extraEmbebbedResources;
        }

/*		//Embed Zip with ToolOrAPI
			tag = "//O2EmbedTool:";
			foreach (var file in sourceCode.lines().starting(tag).remove(tag))
			{
				var toolFolder = file.folderExists() ?  file : PublicDI.config.ToolsOrApis.pathCombine(file);				
				if (toolFolder.folderExists())
				{
					"[mapExtraEmbebbedResources] found To EmbedTool reference: {0} -> targetFolder: {1}".debug(toolFolder, targetFolder);
					var zippedTool = toolFolder.zip()
					"[mapExtraEmbebbedResources] found To EmbedTool reference: {0} -> targetFolder: {1}".debug(toolFolder, targetFolder);
					extraEmbebbedResources.add(file.local());
				}
				else if (file.assembly_Location().fileExists())
				{
					"[mapExtraEmbebbedResources] found Assembly To Embed reference: {0} -> targetFolder: {1}".debug(file.assembly_Location(), targetFolder);
					extraEmbebbedResources.add(file.assembly_Location());
				}
				else
				{
					"[mapExtraEmbebbedResources] could not find Embedded reference: {0}".error(file);
				}
			}
*/
		public static string copyToolReferencesToFolder(this string targetFolder, string fileToParse)
		{
			var sourceCode = fileToParse.local().contents().fix_CRLF();
			var tag = "//O2EmbedTool:";
			foreach (var folder in sourceCode.lines().starting(tag).replace(tag,""))
			{
				var toolFolder = folder.folderExists() ? folder : PublicDI.config.ToolsOrApis.pathCombine(folder);
				if (toolFolder.folderExists())
				{
					"[mapExtraEmbebbedResources] found To EmbedTool reference: {0} -> targetFolder: {1}".debug(toolFolder, targetFolder);
					Files.copyFolder(toolFolder, targetFolder.createDir(), true, false,"");
				}
			}
			return targetFolder;
		}
        public static string copyFileReferencesToFolder(this string targetFolder, string fileToParse)
        {

			var sourceCode = fileToParse.local().contents().fix_CRLF(); 
							/*fileToParse.extension(".h2") ? fileToParse.local().h2_SourceCode().fix_CRLF()
                                                          : fileToParse.local().fileContents().fix_CRLF();*/
			var tag = "//O2Package:";
			foreach (var item in sourceCode.lines().starting(tag).replace(tag, ""))
			{
				if (item == "ALL_SCRIPTS")      //special tag to indicate that we want to copy all scripts
				{
					Files.copyFolder(PublicDI.config.LocalScriptsFolder, targetFolder, true, true, ".git");
					continue;
				}
				foreach (var file in item.split(","))
				//var file = .local();
				{
					var mappedFile = file.local();
					if (mappedFile.fileExists())
					{
						"[copyFileReferencesToEmbeddedFolder] found Package reference: {0}".debug(file);
						mappedFile.file_Copy(targetFolder);
					}
					else
						"[copyFileReferencesToEmbeddedFolder] couldn't find Package reference: {0}".debug(file);
				}
			}		
            return targetFolder;
        }
    }    
}
