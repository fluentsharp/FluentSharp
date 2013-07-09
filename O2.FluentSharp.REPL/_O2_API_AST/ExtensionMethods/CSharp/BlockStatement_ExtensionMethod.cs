using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.Ast;

using FluentSharp.CoreLib;

namespace FluentSharp.CSharpAST
{
    public static class BlockStatement_ExtensionMethod
    {
        public static BlockStatement        body(this INode iNode)
        {
            if (iNode is MethodDeclaration)
                return (iNode as MethodDeclaration).Body;
            var methodDeclaration = iNode.methodDeclaration();
            if (methodDeclaration.notNull())
                return methodDeclaration.Body;
            "method declaration for iNode: {0} was null".error(iNode);
            return null;
        }
        public static BlockStatement        parentBlock(this INode iNode)
        {
            return iNode.parent<BlockStatement>();
        }
        public static BlockStatement        add_Return(this BlockStatement blockStatement, object returnData)
        {
			Expression returnStatement;
			if (returnData.isNull())
				returnStatement = new PrimitiveExpression(null);
			else
			{
				if (returnData is Expression)
					returnStatement = (returnData as Expression);
				else
					returnStatement = new PrimitiveExpression(returnData, returnData.str());

			}
			blockStatement.append(new ReturnStatement(returnStatement));
            return blockStatement;
        }
        public static BlockStatement        append_AsStatement(this BlockStatement blockExpression, Expression expression)
        {
            return blockExpression.append(expression.expressionStatement());
        }
        public static VariableDeclaration   add_Variable_with_NewObject(this BlockStatement blockStatement, string variableName, string typeName)
        {
            return blockStatement.add_Variable_with_NewObject(variableName, typeName.ast_TypeReference());
        }
        public static VariableDeclaration   add_Variable_with_NewObject(this BlockStatement blockStatement, string variableName, TypeDeclaration typeDeclaration)
        {
            return blockStatement.add_Variable_with_NewObject(variableName, typeDeclaration.Name.ast_TypeReference());
        }
        public static VariableDeclaration   add_Variable_with_NewObject(this BlockStatement blockStatement, string variableName, TypeReference typeReference)
        {
            return blockStatement.add_Variable(variableName, typeReference.ast_ObjectCreate(), typeReference);
        }
    }
}
