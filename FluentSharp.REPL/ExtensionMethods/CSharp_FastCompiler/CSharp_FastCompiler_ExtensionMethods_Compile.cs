using System;
using System.Reflection;
using System.Threading;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.REPL.Utils;

namespace FluentSharp.REPL
{
    public static class CSharp_FastCompiler_ExtensionMethods_Compile
    {
        /*static CSharp_FastCompiler_ExtensionMethods()
        {
            CSharp_FastCompiler.setDefaultUsingStatements();        //make sure these are set
            CSharp_FastCompiler.setDefaultReferencedAssemblies();
        }*/

        public static string    compileScriptFile_into_SeparateFolder(this string scriptFile)
        {
            var assembly = scriptFile.compileScriptFile(true);
            if (assembly.notNull() && assembly.Location.fileExists())
            {
                "Compiled Script {0} ok to: {0}".info(scriptFile.fileName(), assembly.Location);
                var targetDir = "{0} [{1}]".format(scriptFile.fileName_WithoutExtension(), 5.randomNumbers()).tempDir(false);

                var compiledScript = assembly.Location.file_Copy(targetDir);
                //copy assembly references to targetDir
                var referenceAssemblies = assembly.referencedAssemblies(true);
                targetDir.copyAssembliesToFolder(referenceAssemblies.ToArray());

                "Copied compiled script and its dependencies into folder {0}".info(targetDir);
                return compiledScript;
            }
            return null;
        }

        public static Assembly  compileScriptFile(this string scriptToCompile)
        { 
            return scriptToCompile.compileScriptFile(false);
        }
        public static Assembly  compileScriptFile(this string scriptToCompile, bool generateDebugSymbols)
        {
            if (scriptToCompile.valid())
            {
                if (scriptToCompile.fileExists().isFalse())
                    scriptToCompile = scriptToCompile.local();
                if (scriptToCompile.fileExists())
                    return (scriptToCompile.extension(".h2"))
                        ? scriptToCompile.compile_H2Script(generateDebugSymbols)
                        : scriptToCompile.compile(generateDebugSymbols);
            }
            "[compileScriptFile] could not find file to compile: {0}".error(scriptToCompile);
            return null;
        }

        public static Assembly  compile(this string pathToFileToCompile)
        {
            return pathToFileToCompile.compile(false);
        }
        public static Assembly  compile(this string pathToFileToCompile, bool generateDebugSymbols)
        {
            PublicDI.CurrentScript = pathToFileToCompile;
            var csharpCompiler = new CSharp_FastCompiler().generateDebugSymbols(generateDebugSymbols);            
            var compileProcess = new AutoResetEvent(false);            
            csharpCompiler.set_OnCompileFail(() => compileProcess.Set());
            csharpCompiler.set_OnCompileOK  (() => compileProcess.Set());

            O2Thread.mtaThread(()=> csharpCompiler.compileSourceCode(pathToFileToCompile.contents()));
            compileProcess.WaitOne();
            return csharpCompiler.assembly();
        }
        public static Assembly  compile_CodeSnippet(this string codeSnipptet)
        {
            return codeSnipptet.compile_CodeSnippet(false);
        }
        public static Assembly  compile_CodeSnippet(this string codeSnipptet, bool generateDebugSymbols)
        {   
            //Note we can't use the precompiled engines here since there is an issue of the resolution of this code dependencies

            var csharpCompiler = new CSharp_FastCompiler().debugMode(true)
                                                          .generateDebugSymbols(generateDebugSymbols);                        
            var compileProcess = new AutoResetEvent(false);            
            csharpCompiler.set_OnAstFail    (() => compileProcess.Set());            
            csharpCompiler.set_OnCompileFail(() => compileProcess.Set());
            csharpCompiler.set_OnCompileOK  (() => compileProcess.Set());
            csharpCompiler.compileSnippet(codeSnipptet);
            compileProcess.WaitOne();
            var assembly = csharpCompiler.assembly();
            return assembly;
        }
        public static Assembly  compile_H2Script(this string h2Script)
        {
            return h2Script.compile_H2Script(false);
        }
        public static Assembly  compile_H2Script(this string h2Script, bool generateDebugSymbols)            
        {
            try
            {                
                string sourceCode;
                if (h2Script.extension(".h2"))
                {
                    PublicDI.CurrentScript = h2Script;
                    if (h2Script.fileExists().isFalse())
                        h2Script = h2Script.local();
                    sourceCode = H2.load(h2Script).SourceCode;
                }
                else
                    sourceCode = h2Script;
                if (sourceCode.valid())
                    return sourceCode.compile_CodeSnippet(generateDebugSymbols);
            }
            catch (Exception ex)
            {
                ex.log("in string compile_H2Script");
            }
            return null;
        }
        public static Assembly compile(this string pathToFileToCompile, string targetAssembly)
        {
            var assembly = pathToFileToCompile.compile(true);
            Files.copy(assembly.Location, targetAssembly);
            return assembly;
        }  
    }
}