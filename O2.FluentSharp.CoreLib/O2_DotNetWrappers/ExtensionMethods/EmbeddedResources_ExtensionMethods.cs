using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace O2.DotNetWrappers.ExtensionMethods
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
        public static byte[] bytes(this Stream stream)
        {
            return new BinaryReader(stream).ReadBytes((int)stream.Length);
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
            var sourceCode = fileToParse.local().contents();
            if (sourceCode.notValid())
            {
                "[mapExtraEmbebbedResources] could not get source code for file: {0}".format(fileToParse);
            }
            var tag = "//O2Embed:";
            foreach (var line in sourceCode.fixCRLF().lines())
            {                
                if (line.starts(tag))
                {
                    var file = line.remove(tag).local();
                    if (file.fileExists())
                    {
                        "[mapExtraEmbebbedResources] found To Embed reference: {0} -> targetFolder: {1}".debug(file, targetFolder);
                        //file.file_Copy(targetFolder);
                        extraEmbebbedResources.add(file);
                    }
                    else
                        "[mapExtraEmbebbedResources] could not find Embedded reference: {0}".error(file);
                }
            }
            return extraEmbebbedResources;
        }
        public static string copyFileReferencesToEmbeddedFolder(this string targetFolder, string fileToParse)
        {
            //"[copyFileReferencesToEmbeddedFolder] analyzing file: {0} with {1} lines".error(sourceToParse.local(), sourceToParse.local().lines().size());
            var tag = "//O2Package:";
            var sourceCode = fileToParse.extension(".h2") ? fileToParse.local().h2_SourceCode().fixCRLF()
                                                          : fileToParse.local().fileContents().fixCRLF();
            foreach (var line in sourceCode.lines())
                if (line.starts(tag))
                {
                    var item = line.remove(tag);
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
