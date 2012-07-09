using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.Ast;

namespace O2.API.AST.ExtensionMethods.CSharp
{
    public static class TypeReference_ExtensionMethods
    {        
        public static string name(this TypeReference typeReference)
        {
            return typeReference.Type;
        }        
    }
}
