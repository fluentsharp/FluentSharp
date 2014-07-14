using System.Collections.Generic;
using FluentSharp.CoreLib;
using FluentSharp.CSharpAST.Utils;
using ICSharpCode.NRefactory.Ast;

namespace FluentSharp.AST
{
    public static class Ast_CSharp_ExtensionMethods
    {
        public static Ast_CSharp csharp_Ast(this string sourceCode)
        {
            return sourceCode.ast_CSharp();
        }
        public static Ast_CSharp csharp_Ast_From_Snippet(this string codeSnippet)
        {
            return codeSnippet.ast_CSharp_From_Snippet();
        }
        public static Ast_CSharp ast_CSharp(this string sourceCode)
        {
            return sourceCode.valid() ? new Ast_CSharp(sourceCode) : null;
        }
        public static Ast_CSharp ast_CSharp_From_Snippet(this string codeSnippet)
        {
            var cSharpCode  =  codeSnippet.csharp_FromSnippet();
            if(cSharpCode.valid())
                return cSharpCode.ast_CSharp();
            return null;
        }

        public static AstDetails astDetails(this Ast_CSharp astCSharp)
        {
            return astCSharp.notNull() ? astCSharp.AstDetails : null;
        }
        public static List<MethodDeclaration> methodDeclarations(this Ast_CSharp astCSharp)
        {
            return astCSharp.astDetails().methodDeclarations();
        }
        public static List<AstValue<MethodDeclaration>> methods(this Ast_CSharp astCSharp)
        {
            return astCSharp.astDetails().methods();
        }

        public static string        csharpCode(this Ast_CSharp astCSharp)
        {
            return (astCSharp.notNull() && astCSharp.SourceCode.valid())
                        ? astCSharp.SourceCode
                        : "";
        }
    }
}
