using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentSharp.CoreLib;
using ICSharpCode.NRefactory.Ast;

namespace FluentSharp.CSharpAST
{
    public static class IdentifierExpression_ExtensionMethods
    {
        public static IdentifierExpression          ast_Identifier(this string identifierName)
        {
            return new IdentifierExpression(identifierName);
        }
        public static List<IdentifierExpression>    ast_Identifiers(this List<string> identifierNames)
        {
            return identifierNames.select((name) => name.ast_Identifier());
        }
    }
}
