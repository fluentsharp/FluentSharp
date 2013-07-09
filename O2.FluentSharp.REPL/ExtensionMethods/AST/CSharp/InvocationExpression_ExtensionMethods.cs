using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.Ast;

using FluentSharp.CoreLib;

namespace FluentSharp.CSharpAST
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
        public static InvocationExpression add_Invocation(this BlockStatement blockStatement, VariableDeclaration variableDeclaration, string methodName, params Expression[] arguments)
        {
            return blockStatement.add_Invocation(variableDeclaration.Name, methodName, arguments);
        }
        public static InvocationExpression add_Invocation(this InvocationExpression parentInvocation, string methodName, params Expression[] arguments)
        {
            return parentInvocation.toMemberReference(methodName)
                                   .toInvocation(arguments);
        }
        public static InvocationExpression ast_Invocation_onType(this string typeName, string methodName, params object[] parameters)
        {
            return new BlockStatement().add_Invocation(typeName, methodName, parameters);
        }
        public static InvocationExpression ast_Invocation(this  string methodName, params object[] parameters)
        {
            return new BlockStatement().add_Invocation("", methodName, parameters);
        }
        public static InvocationExpression toInvocation(this Expression targetObject, params Expression[] arguments)
        {
            return new InvocationExpression(targetObject, arguments.toList());
        }
        public static int                  argumentPosition(this InvocationExpression invocationExpression, IdentifierExpression identifierExpression)
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
