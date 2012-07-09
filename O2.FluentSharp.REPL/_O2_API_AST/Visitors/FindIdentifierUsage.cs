using System;
using System.Collections.Generic;
using O2.API.AST.CSharp;
using ICSharpCode.NRefactory.Ast;

namespace O2.API.AST.Visitors
{
    public class FindIdentifierUsage : O2AstVisitor
    {
        public String IdentifierName { get; set; }
        public List<IdentifierExpression> Usages { get; set; }

        public FindIdentifierUsage(string identifierName)
        {
            IdentifierName = identifierName;
            Usages = new List<IdentifierExpression>();

            identifierExpressionVisit = (identifierExpression, data) =>
            {
                if (identifierExpression.Identifier == IdentifierName)
                    Usages.Add(identifierExpression);
                return data;
            };
        }
    }
}
