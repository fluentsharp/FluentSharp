using System.Collections.Generic;
using FluentSharp.CoreLib;
using FluentSharp.REPL.Utils;

namespace FluentSharp.REPL
{
    public static class CSharp_FastCompiler_CompilerOptions_ExtensionMethods
    {
        public static List<string>          referencedAssemblies(this CSharp_FastCompiler csharpCompiler)
        {
            return csharpCompiler.notNull() ? csharpCompiler.CompilerOptions.ReferencedAssemblies 
                : new List<string>();
        }
        
        public static string                compilationVersion(this CSharp_FastCompiler csharpCompiler)
        {
            return csharpCompiler.notNull() ?  csharpCompiler.CompilerOptions.CompilationVersion : null; 
        }
        public static CSharp_FastCompiler   compilationVersion(this CSharp_FastCompiler csharpCompiler, string value)
        {
            if (csharpCompiler.notNull())
                csharpCompiler.CompilerOptions.CompilationVersion = value; 
            return csharpCompiler;
        }
        public static bool                  debugMode(this CSharp_FastCompiler csharpCompiler)
        {
            return csharpCompiler.notNull() && csharpCompiler.DebugMode; 
        }
        public static CSharp_FastCompiler   debugMode(this CSharp_FastCompiler csharpCompiler, bool value)
        {
            if (csharpCompiler.notNull())
                csharpCompiler.DebugMode = value; 
            return csharpCompiler;
        }
        public static string                default_TypeName(this CSharp_FastCompiler csharpCompiler)
        {
            return csharpCompiler.notNull() ?  csharpCompiler.CompilerOptions.default_TypeName : null; 
        }
        public static CSharp_FastCompiler   default_TypeName(this CSharp_FastCompiler csharpCompiler, string value)
        {
            if (csharpCompiler.notNull())
                csharpCompiler.CompilerOptions.default_TypeName = value; 
            return csharpCompiler;
        }
        public static string                default_MethodName(this CSharp_FastCompiler csharpCompiler)
        {
            return csharpCompiler.notNull() ?  csharpCompiler.CompilerOptions.default_MethodName : null; 
        }
        public static CSharp_FastCompiler   default_MethodName(this CSharp_FastCompiler csharpCompiler, string value)
        {
            if (csharpCompiler.notNull())
                csharpCompiler.CompilerOptions.default_MethodName = value; 
            return csharpCompiler;
        }
        public static bool                  generateDebugSymbols(this CSharp_FastCompiler csharpCompiler)
        {
            return csharpCompiler.notNull() && csharpCompiler.CompilerOptions.generateDebugSymbols; 
        }
        public static CSharp_FastCompiler   generateDebugSymbols(this CSharp_FastCompiler csharpCompiler, bool value)
        {
            if (csharpCompiler.notNull())
                csharpCompiler.CompilerOptions.generateDebugSymbols = value; 
            return csharpCompiler;
        }
        public static bool                  resolveInvocationParametersType(this CSharp_FastCompiler csharpCompiler)
        {
            return csharpCompiler.notNull() && csharpCompiler.CompilerOptions.ResolveInvocationParametersType; 
        }
        public static CSharp_FastCompiler   resolveInvocationParametersType(this CSharp_FastCompiler csharpCompiler, bool value)
        {
            if (csharpCompiler.notNull())
                csharpCompiler.CompilerOptions.ResolveInvocationParametersType = value; 
            return csharpCompiler;
        }
        public static string                sourceCodeFile(this CSharp_FastCompiler csharpCompiler)
        {
            return csharpCompiler.notNull() ? csharpCompiler.CompilerOptions.SourceCodeFile : null; 
        }
        public static CSharp_FastCompiler   sourceCodeFile(this CSharp_FastCompiler csharpCompiler, string value)
        {
            if (csharpCompiler.notNull())
                csharpCompiler.CompilerOptions.SourceCodeFile = value; 
            return csharpCompiler;
        }
    
        public static bool                  useCachedAssemblyIfAvailable(this CSharp_FastCompiler csharpCompiler)
        {
            return csharpCompiler.notNull() && csharpCompiler.CompilerOptions.UseCachedAssemblyIfAvailable; 
        }
        public static CSharp_FastCompiler   useCachedAssemblyIfAvailable(this CSharp_FastCompiler csharpCompiler, bool value)
        {
            if (csharpCompiler.notNull())
                csharpCompiler.CompilerOptions.UseCachedAssemblyIfAvailable = value; 
            return csharpCompiler;
        }
    }
}