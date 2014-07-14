using System.Collections.Generic;

namespace FluentSharp.AST
{
    public class CSharp_FastCompiler_ExecutionOptions
    {
        public Dictionary<string, object> InvocationParameters { get; set; }
        public CSharp_FastCompiler_ExecutionOptions()
        {
            InvocationParameters = new Dictionary<string, object>();
        }
    }
}