using System;
using System.Reflection;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;

namespace FluentSharp.REPL
{
    public static class CSharp_FastCompiler_ExtensionMethods_Execute
    {
        public static object invoke_FirstMethod(this Assembly assembly)
        {
            return assembly.invoke_FirstMethod<object>();
        }

        public static T invoke_FirstMethod<T>(this Assembly assembly)
        {
            var result = assembly.executeFirstMethod();
            if (result is T)
                return (T)result;
            "in invoke_FirstMethod, returned value was not the expected type ('{0}') it was: '{1}'".error(typeof(T), result.type());
            return default(T);
        }

        public static object executeFirstMethod(this string pathToFileToCompileAndExecute)
        {
            try
            {
                return pathToFileToCompileAndExecute.executeFirstMethod(new object[] { });
            }
            catch (Exception ex)
            {
                ex.log("in executeFirstMethod");
                return null;
            }
        }
        public static object executeFirstMethod(this string pathToFileToCompileAndExecute, object[] parameters)
        {
            PublicDI.CurrentScript = pathToFileToCompileAndExecute;
            if (pathToFileToCompileAndExecute.fileExists().isFalse())
            { 
                // if we were not provided a complete path, try to find it on the local o2 script folder                
                pathToFileToCompileAndExecute = pathToFileToCompileAndExecute.local();                
            }
            if (pathToFileToCompileAndExecute.extension(".h2"))
                return executeH2Script(pathToFileToCompileAndExecute);

            var assembly = pathToFileToCompileAndExecute.compile(true /* generatedDebug symbols */);
            return assembly.executeFirstMethod(parameters);
        }
        public static object executeFirstMethod(this Assembly assembly)
        {
            return assembly.executeFirstMethod(false, false, new object[] {});
        }
        public static object executeFirstMethod(this Assembly assembly ,  object[] parameters)
        {
            return assembly.executeFirstMethod(false, false, parameters);
        }
        public static object executeFirstMethod(this Assembly assembly ,  bool executeInStaThread, bool executeInMtaThread, object[] parameters)
        {            
            if (assembly != null)
            {
                var methods = assembly.methods();
                foreach (var method in methods)
                    if (method.IsSpecialName == false && method.IsPublic)  // we need to do this since Properties get_ and set_ also look like methods
                        //if (methods.Count >0)        		
                        //{
                    {
                        MethodInfo method1 = method;       
                        if (executeInStaThread)
                        {                            
                            return O2Thread.staThread(() => method1.executeMethod(parameters));
                        }
                        if (executeInMtaThread)
                        {                            
                            return O2Thread.mtaThread(() => method1.executeMethod(parameters));
                        }

                        return method.executeMethod(parameters);
                    }
            }
            return null;
        }
        public static object executeMethod(this MethodInfo method, params object[] parameters)
        {
            try
            {
                if (method.parameters().size() == parameters.size())
                    return method.invoke(parameters);
                return method.invoke();
            }
            catch (Exception ex)
            {
                ex.log("in CSharp_FastCompiler.executeMethod");
                return null;
            }
        }
        public static object executeCodeSnippet(this string sourceCodeToExecute)
        {
            return sourceCodeToExecute.executeSourceCode();
        }
        public static object executeSourceCode(this string sourceCodeToExecute)
        {
            try
            {
                var assembly = sourceCodeToExecute.compile_CodeSnippet(true);
                return assembly.executeFirstMethod();
            }
            catch (Exception ex)
            {
                ex.log("in CSharp_FastCompiler.executeSourceCode");
                return null;
            }            
        }

        public static object executeH2Script(this string h2ScriptFile)
        {
            try
            {
                if (h2ScriptFile.extension(".h2").isFalse())
                    "[in executeH2Script]: file to execute must be a *.h2 file, it was:{0}".error(h2ScriptFile);
                else
                {
                    PublicDI.CurrentScript = h2ScriptFile;
                    var h2Script = H2.load(h2ScriptFile);
                    return h2Script.execute();                    
                }
            }
            catch (Exception ex)
            {
                ex.log("in CSharp_FastCompiler.executeH2Script");
            }
            return null;
        }
        public static object execute(this H2 h2Script)
        {
            try
            {                
                var assembly = h2Script.SourceCode.compile_CodeSnippet();
                return assembly.executeFirstMethod();                
            }
            catch (Exception ex)
            {
                ex.log("in CSharp_FastCompiler.executeH2Script");
            }
            return null;
        }
    }
}