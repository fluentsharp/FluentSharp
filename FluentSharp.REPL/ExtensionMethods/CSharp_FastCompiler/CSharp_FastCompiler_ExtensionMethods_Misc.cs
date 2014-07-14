using System;
using System.Collections.Generic;
using System.Reflection;
using FluentSharp.CoreLib;
using FluentSharp.REPL.Utils;

namespace FluentSharp.REPL
{
    public static class CSharp_FastCompiler_ExtensionMethods_Misc
    {
        public static List<string> extraSourceCodeFilesToCompile(this CSharp_FastCompiler csharpCompiler)
        {
            return csharpCompiler.notNull() ? csharpCompiler.CompilerOptions.ExtraSourceCodeFilesToCompile 
                                            : new List<string>();
        }
    
        public static Assembly     assembly(this CSharp_FastCompiler csharpCompiler)
        {
			if (csharpCompiler != null)
			{
				if (csharpCompiler.compiledAssembly().notNull())
					return csharpCompiler.compiledAssembly();
				if (csharpCompiler.compilerResults() != null)
					if (csharpCompiler.compilerResults().Errors.HasErrors == false)
					{
						if (csharpCompiler.compilerResults().CompiledAssembly != null)
							return csharpCompiler.compilerResults().CompiledAssembly;
					}
					else
						"CompilationErrors:".line().add(csharpCompiler.compilationErrors()).error();
			}
            return null;
        }
    }
}
