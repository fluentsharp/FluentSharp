using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.Ast;

using O2.DotNetWrappers.ExtensionMethods;

namespace O2.API.AST.ExtensionMethods.CSharp
{
    public static class BlockStatement_ExtensionMethod
    {
        public static BlockStatement body(this INode iNode)
        {
            if (iNode is MethodDeclaration)
                return (iNode as MethodDeclaration).Body;
            var methodDeclaration = iNode.methodDeclaration();
            if (methodDeclaration.notNull())
                return methodDeclaration.Body;
            "method declaration for iNode: {0} was null".error(iNode);
            return null;
        }

        public static BlockStatement parentBlock(this INode iNode)
        {
            return iNode.parent<BlockStatement>();
        }

        public static BlockStatement add_Return(this BlockStatement blockStatement, object returnData)
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
 
    }
}
