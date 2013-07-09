using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.Ast;

namespace FluentSharp.CSharpAST
{
    public static class TypeReference_ExtensionMethods
    {        
        public static string name(this TypeReference typeReference)
        {
            return typeReference.Type;
        }        
    }
}
