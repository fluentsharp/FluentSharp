using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.Ast;
using O2.DotNetWrappers.ExtensionMethods;

namespace O2.API.AST.ExtensionMethods.CSharp
{
    public static class ObjectCreateExpression_ExtensionMethod
    {
        public static int parameterPosition(this ObjectCreateExpression objectCreateExpression, IdentifierExpression identifierExpression)
        {
            if (objectCreateExpression != null && identifierExpression != null)
                for (int i = 0; i < objectCreateExpression.Parameters.Count; i++)								// for each arguments
                    foreach (var iNode in objectCreateExpression.Parameters[i].iNodes<IdentifierExpression>())	// get the IdentifierExpression
                        if (iNode == identifierExpression)													// and compare the values
                            return i;
            "in ObjectCreateExpression.parameterPosition could not find provided IdentifierExpression as a current parameter".error();
            return -1;
        }
    }
}
