using System;
using FluentSharp.CoreLib.API;
using System.Reflection;

namespace FluentSharp.CoreLib
{
    public static class CompileEngine_ExtensionMethods
    {
        public static string    local(this string fileName)
        {
            return fileName.localScript();
        }
        public static string    localScript(this string fileName)
        {
            return CompileEngine.findFileOnLocalScriptFolder(fileName.trim());
        }
        public static Assembly  download_Assembly_From_O2_GitHub(this string assemblyName)
        {
            return assemblyName.download_Assembly_From_O2_GitHub(false);
        }
        public static Assembly  download_Assembly_From_O2_GitHub(this string assemblyName, bool ignoreSslErrors)
        {
            if (assemblyName.assembly().notNull())
                "in download_Assembly_From_O2_GitHub, the requests assembly already exists".error(assemblyName);
            else
            {
                if (ignoreSslErrors)
                    Web.Https.ignoreServerSslErrors();

                //add option to ignore cache
                
                new O2GitHub().tryToFetchAssemblyFromO2GitHub(assemblyName,false);
            }
            return assemblyName.assembly();
        }

        public static object    compileAndExecuteCodeSnippet(this string snippet)
        {
            return snippet.compileAndExecuteCodeSnippet((msg) => msg.info(), (msg) => msg.error());
        }
        public static object    compileAndExecuteCodeSnippet(this string snippet, Action<string> onCompileOk, Action<string> onCompileFail)
        {
            var assembly = snippet.compileCodeSnippet(onCompileOk, onCompileFail);
            if (assembly.notNull())
                return assembly.type("DynamicType").ctor().invoke("dynamicMethod");
            return null;
        }
        public static Assembly  compileCodeSnippet(this string snippet)
        {
            return snippet.compileCodeSnippet((msg) => msg.info(), (msg) => msg.error());
        }
        public static Assembly  compileCodeSnippet(this string snippet, Action<string> onCompileOk, Action<string> onCompileFail)
        {
            try
            {
                var codeFile = createCSharpFileFromCodeSnippet(snippet);

                var compileEngine = new CompileEngine();
                var assembly = compileEngine.compileSourceFile(codeFile);
                if (assembly.notNull())
                {
                    onCompileOk("[compileAndExecuteCodeSnippet] Snippet assembly created OK: {0}".format(assembly.location()));
                    return assembly;
                }
                onCompileFail("[compileAndExecuteCodeSnippet] Compilation failed: ".line() + compileEngine.sbErrorMessage.str());
            }
            catch (Exception ex)
            {
                ex.log("[compileAndExecuteCodeSnippet]");
            }
            return null;
        }
        public static string    createCSharpFileFromCodeSnippet(this string snippet)
        {
            var code = createCSharpCodeFromCodeSnippet(snippet);
            var codeFile = code.saveAs(".cs".tempFile());
            "Snippet code saved to : {0}".info(codeFile);
            return codeFile;
        }
        public static string    createCSharpCodeFromCodeSnippet(this string snippet)
        {
            "[compileAndExecuteCodeSnippet] Compiling code with size: {0}".info(snippet.size());

            var codeTemplate = @"
using System.Linq;
using System.Xml.Linq;
using System.Xml;
using System.Collections.Generic;
using System;
%%EXTRA_USING%%

public class DynamicType
{
    public object dynamicMethod()
    {
//*** Code Start	

%%CODE%%

//*** Code Ends
        //return null;
    }
}";

            //extra extra usings
            var extraUsing = "";
            foreach (var usingStatement in snippet.lines().starting("//using"))
                extraUsing += usingStatement.subString(2).line();
            //do replacements	
            var code = codeTemplate.replace("%%EXTRA_USING%%", extraUsing)
                                   .replace("%%CODE%%", snippet);
            return code;
        }
        public static Assembly  compileSourceFile(this string sourceCodeFile)
        {
            return new CompileEngine().compileSourceFiles(sourceCodeFile.wrapOnList());            
        }
    }
}
