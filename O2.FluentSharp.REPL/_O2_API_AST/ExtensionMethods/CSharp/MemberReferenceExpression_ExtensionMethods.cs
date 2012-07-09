using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.Ast;
using O2.API.AST.ExtensionMethods;
using O2.DotNetWrappers.ExtensionMethods;


namespace O2.API.AST.ExtensionMethods.CSharp
{
    public static class MemberReferenceExpression_ExtensionMethods
    {
        public static MemberReferenceExpression add_MemberReference(this BlockStatement blockStatement, string memberName, string className) //, AstExpression expression)
        {
            var identifier = new IdentifierExpression(memberName);
            var memberReference = new MemberReferenceExpression(identifier, className);
            blockStatement.append(memberReference.expressionStatement());
            return memberReference;
        }


        public static string sourceCode(this MemberReferenceExpression memberReferenceExpression, string file)
        {
            var methodDeclaration = memberReferenceExpression.methodDeclaration();
            if (methodDeclaration != null)
            {
                var sourceCode = methodDeclaration.sourceCode(file);
                if (sourceCode.valid())
                    return sourceCode;
            }
            "could not find sourcecode in {0} for provided memberReferenceExpression: {1}".format(file, memberReferenceExpression).error();
            return "";
        }
    }
}
