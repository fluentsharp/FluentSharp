using System.CodeDom.Compiler;
using System.Reflection;
using FluentSharp.CoreLib;
using FluentSharp.CSharpAST.Utils;
using FluentSharp.REPL.Utils;
using ICSharpCode.NRefactory.Ast;

namespace FluentSharp.REPL
{
    public static class CSharp_FastCompiler_CompilerArtifacts_ExtensionMethods
    {
        public static string                astErrors(this CSharp_FastCompiler csharpCompiler)
        {
            return csharpCompiler.notNull() ? csharpCompiler.CompilerArtifacts.AstErrors : null;
        }
        public static CSharp_FastCompiler   astErrors(this CSharp_FastCompiler csharpCompiler, string value)
        {
            if (csharpCompiler.notNull())
                csharpCompiler.CompilerArtifacts.AstErrors = value; 
            return csharpCompiler;
        }
        public static AstDetails            astDetails(this CSharp_FastCompiler csharpCompiler)
        {
            return csharpCompiler.notNull() ? csharpCompiler.CompilerArtifacts.AstDetails : null;
        }
        public static CSharp_FastCompiler   astDetails(this CSharp_FastCompiler csharpCompiler, AstDetails value)
        {
            if (csharpCompiler.notNull())
                csharpCompiler.CompilerArtifacts.AstDetails = value; 
            return csharpCompiler;
        }
        public static string                compilationErrors(this CSharp_FastCompiler csharpCompiler)
        {
            return csharpCompiler.notNull() ? csharpCompiler.CompilerArtifacts.CompilationErrors : null;
        }
        public static CSharp_FastCompiler   compilationErrors(this CSharp_FastCompiler csharpCompiler, string value)
        {
            if (csharpCompiler.notNull())
                csharpCompiler.CompilerArtifacts.CompilationErrors = value; 
            return csharpCompiler;
        }
        public static CompilationUnit       compilationUnit(this CSharp_FastCompiler csharpCompiler)
        {
            return csharpCompiler.notNull() ? csharpCompiler.CompilerArtifacts.CompilationUnit : null;
        }
        public static CSharp_FastCompiler   compilationUnit(this CSharp_FastCompiler csharpCompiler, CompilationUnit value)
        {
            if (csharpCompiler.notNull())
                csharpCompiler.CompilerArtifacts.CompilationUnit = value; 
            return csharpCompiler;
        }
        public static Assembly              compiledAssembly(this CSharp_FastCompiler csharpCompiler)
        {
            return csharpCompiler.notNull() ? csharpCompiler.CompilerArtifacts.CompiledAssembly : null; 
        }
        public static CSharp_FastCompiler   compiledAssembly(this CSharp_FastCompiler csharpCompiler, Assembly assembly)
        {
            if (csharpCompiler.notNull())
                csharpCompiler.CompilerArtifacts.CompiledAssembly = assembly; 
            return csharpCompiler;
        }
        public static CompilerResults       compilerResults(this CSharp_FastCompiler csharpCompiler)
        {
            return csharpCompiler.notNull() ? csharpCompiler.CompilerArtifacts.CompilerResults : null; 
        }
        public static CSharp_FastCompiler   compilerResults(this CSharp_FastCompiler csharpCompiler, CompilerResults compilerResults)
        {
            if (csharpCompiler.notNull())
                csharpCompiler.CompilerArtifacts.CompilerResults = compilerResults; 
            return csharpCompiler;
        }        
        public static bool                  createdFromSnippet(this CSharp_FastCompiler csharpCompiler)
        {
            return csharpCompiler.notNull() && csharpCompiler.CompilerArtifacts.CreatedFromSnippet; 
        }
        public static CSharp_FastCompiler   createdFromSnippet(this CSharp_FastCompiler csharpCompiler, bool value)
        {
            if (csharpCompiler.notNull())
                csharpCompiler.CompilerArtifacts.CreatedFromSnippet = value; 
            return csharpCompiler;
        }
        public static string                sourceCode(this CSharp_FastCompiler csharpCompiler)
        {
            return csharpCompiler.notNull() ? csharpCompiler.CompilerArtifacts.SourceCode : null;
        }
        public static CSharp_FastCompiler   sourceCode(this CSharp_FastCompiler csharpCompiler, string value)
        {
            if (csharpCompiler.notNull())
                csharpCompiler.CompilerArtifacts.SourceCode = value; 
            return csharpCompiler;
        }
    }
}