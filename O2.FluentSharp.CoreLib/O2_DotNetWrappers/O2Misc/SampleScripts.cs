// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;




namespace FluentSharp.CoreLib.API
{
    public class SampleScripts
    {
        /// <summary>
        /// returns dictionary the scriptName as Key and script code as Value
        /// </summary>
        /// <param name="o2SampleScripts"></param>
        /// <returns></returns>
        public static Dictionary<string, string> getDictionaryWithSampleScripts(object o2SampleScripts)
        {
            return getDictionaryWithSampleScripts(o2SampleScripts, ".cs");
        }

        public static Dictionary<string, string> getDictionaryWithSampleScripts(object o2SampleScripts, string fileExtension)
        {            
            var sampleScrips = new Dictionary<string,string>();
            try
            {
                var targetType = (o2SampleScripts is Type) ? ((Type) o2SampleScripts) : o2SampleScripts.GetType();
                foreach (MethodInfo method in PublicDI.reflection.getMethods(targetType))
                    if (method.Name.index("get_") > -1 && (method.Name != "get_ResourceManager" && (method.Name != "get_Culture")))
                    {
                        var scriptName = method.Name.Replace("get_", "");
                        // hack to handle IronPhyton sample files
                        //scriptName += scriptName.IndexOf("IronPython") > -1 ? ".py" : fileExtension;
                        if (scriptName.index("_py") > -1)
                            scriptName = scriptName.Replace("_py", ".py");
                        else if (scriptName.index("_java") > -1)
                            scriptName = scriptName.Replace("_java", ".java");
                        else
                            scriptName += fileExtension;

                        //var fixedName = string.Format("get_{0}", sampleScriptName).Replace(".cs", "");
                        var scriptContent = PublicDI.reflection.invokeMethod_Static(targetType, method.Name, null).ToString();
                        if (scriptContent != "")
                            sampleScrips.Add(scriptName, scriptContent);
                    }
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex, "in getDictionaryWithSampleScripts");
            }
            
            return sampleScrips;
        }

        public static string getFileWithSampleScript(object o2SampleScripts, string scriptToProcess,string fileExtension)
        {
            return getFileWithSampleScript(o2SampleScripts, scriptToProcess, fileExtension, false /*createUniqueName*/);
        }

        public static string getFileWithSampleScript(object o2SampleScripts, string scriptToProcess,string fileExtension, bool createUniqueName)
        {
            var sampleScripts = getDictionaryWithSampleScripts(o2SampleScripts,"");
            if (sampleScripts.ContainsKey(scriptToProcess))
            {
                var tempFileName = (createUniqueName) ? PublicDI.config.TempFileNameInTempDirectory + "_" : PublicDI.config.O2TempDir + "\\";
                tempFileName += scriptToProcess + ((fileExtension.IndexOf('.')>-1)?fileExtension : "." + fileExtension);
                Files.WriteFileContent(tempFileName, sampleScripts[scriptToProcess]);
                return tempFileName;
            }
            return "";
        }


        public static void copyResourceFilesIntoDirectory(object resourceObject, string pathToInstall)
        {
            copyResourceFilesIntoDirectory(resourceObject, pathToInstall, false);
        }

        public static void copyResourceFilesIntoDirectory(object resourceObject, string pathToInstall, bool overrideExistingFile)
        {
            Files.checkIfDirectoryExistsAndCreateIfNot(pathToInstall);
            var resourceFiles = getDictionaryWithSampleScripts(resourceObject);
            foreach (var resourceFile in resourceFiles)
            {
                var xRuleFile = Path.Combine(pathToInstall, resourceFile.Key);
                if (overrideExistingFile || false == File.Exists(xRuleFile))
                    Files.WriteFileContent(xRuleFile, resourceFile.Value,true);
            }
        }
    }
}
