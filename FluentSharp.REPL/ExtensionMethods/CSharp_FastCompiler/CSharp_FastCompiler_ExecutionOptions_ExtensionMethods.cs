using System.Collections.Generic;
using FluentSharp.CoreLib;
using FluentSharp.REPL.Utils;

namespace FluentSharp.REPL
{
    public static class CSharp_FastCompiler_ExecutionOptions_ExtensionMethods
    {
        public static Dictionary<string, object> invocationParameters(this CSharp_FastCompiler csharpCompiler)
        {
            return csharpCompiler.notNull() ? csharpCompiler.ExecutionOptions.InvocationParameters : null; 
        }
        public static CSharp_FastCompiler        invocationParameters(this CSharp_FastCompiler csharpCompiler, Dictionary<string, object> invocationParameters)
        {
            if (csharpCompiler.notNull())
                csharpCompiler.ExecutionOptions.InvocationParameters = invocationParameters; 
            return csharpCompiler;
        }
    }
}