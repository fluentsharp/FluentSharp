using System.Collections.Generic;
using FluentSharp.CoreLib;
using ICSharpCode.NRefactory.Ast;

namespace FluentSharp.CSharpAST
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