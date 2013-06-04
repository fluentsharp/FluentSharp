using System.Collections.Generic;
using FluentSharp.ExtensionMethods;
using ICSharpCode.NRefactory.Ast;

namespace O2.API.AST.ExtensionMethods.CSharp
{
    public static class PrimitiveExpression_ExtensionMethods
    {
        public static PrimitiveExpression           ast_Primitive(this string primitiveName)
        {
            return new PrimitiveExpression(primitiveName);
        }
        public static List<PrimitiveExpression>     ast_Primitive(this List<string> primitiveNames)
        {
            return primitiveNames.@select((name) => ast_Primitive((string) name));
        }
    }
}