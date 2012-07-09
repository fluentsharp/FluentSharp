using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.Ast;

using O2.DotNetWrappers.ExtensionMethods;

namespace O2.API.AST.ExtensionMethods.CSharp
{
    public static class InvocationExpression_ExtensionMethods
    {
        public static InvocationExpression add_Invocation(this BlockStatement blockStatement, string methodName)
        {
            return blockStatement.add_Invocation("", methodName);
        }

        public static InvocationExpression add_Invocation(this BlockStatement blockStatement, string typeName, string methodName, params object[] parameters) //, AstExpression expression)
        {
            if (methodName.valid().isFalse())
                return null;

            Expression memberExpression = null;
            if (typeName.valid())
                memberExpression = new MemberReferenceExpression(new IdentifierExpression(typeName), methodName);
            else
                memberExpression = new IdentifierExpression(methodName);

            var memberReference = new InvocationExpression(memberExpression);
            if (parameters != null)
            {
                var arguments = new List<Expression>();
                foreach (var parameter in parameters)
                    if (parameter is Expression)
                        arguments.add(parameter as Expression);
                    else
                        arguments.add(new PrimitiveExpression(parameter, parameter.str()));

                memberReference.Arguments = arguments;
            }

            blockStatement.append(memberReference.expressionStatement());

            return memberReference;
        }
 


        public static int argumentPosition(this InvocationExpression invocationExpression, IdentifierExpression identifierExpression)
        {
            if (invocationExpression != null && identifierExpression != null)
                for (int i = 0; i < invocationExpression.Arguments.Count; i++)								// for each arguments
                    foreach (var iNode in invocationExpression.Arguments[i].iNodes<IdentifierExpression>())	// get the IdentifierExpression
                        if (iNode == identifierExpression)													// and compare the values
                            return i;
            "in InvocationExpression.parameterPosition could not find provided IdentifierExpression as a current parameter".error();
            return -1;
        } 
    }
}
