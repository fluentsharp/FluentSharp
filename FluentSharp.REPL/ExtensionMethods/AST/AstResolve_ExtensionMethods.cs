using System.Collections.Generic;
using FluentSharp.CSharpAST.Utils;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.SharpDevelop.Dom;
using FluentSharp.CoreLib;
using FluentSharp.CSharpAST;

namespace FluentSharp.REPL
{
    public static class AstResolve_ExtensionMethods
    {
        public static void initialize(this O2AstResolver o2AstResolver, Expression expression)
        {
            o2AstResolver.resolver.Initialize(o2AstResolver.parseInformation, expression.StartLocation.Line, expression.StartLocation.Column);            
        }

        public static ResolveResult resolve(this O2AstResolver o2AstResolver, Expression expression)
        {
            //return o2AstResolver.resolve(expression,null,null);
            return o2AstResolver.resolve(expression,null,null);
        }

        public static ResolveResult resolve(this O2AstResolver o2AstResolver, Expression expression, IClass callingClass, IMember callingMember)
        {
            o2AstResolver.initialize(expression);
            if (callingClass != null)                   // this is needed on the cases where we need to resolve expressions from partial files
            {
                o2AstResolver.resolver.CallingClass = callingClass;
                o2AstResolver.resolver.CallingMember = callingMember;
                o2AstResolver.resolver.CaretColumn = 0;
                o2AstResolver.resolver.CaretLine = 0;
            }
            var methodDeclaration = expression.parent<MethodDeclaration>();
            if (methodDeclaration != null)
                o2AstResolver.resolver.RunLookupTableVisitor(methodDeclaration);
            else
            {
                var constructorDeclaration = expression.parent<ConstructorDeclaration>();
                if (constructorDeclaration != null)
                    o2AstResolver.resolver.RunLookupTableVisitor(constructorDeclaration);
            }
            //resolver.Initialize(parseInformation, memberReferenceExpression.StartLocation.Line, memberReferenceExpression.StartLocation.Column);
            //resolver.RunLookupTableVisitor(memberReferenceExpression);

            return o2AstResolver.resolver.ResolveInternal(expression, ExpressionContext.Default);            
        }
        
        /*public static object resolve(this O2AstResolver o2AstResolver, MemberReferenceExpression memberReferenceExpression)
        {
            o2AstResolver.initialize(memberReferenceExpression as Expression);
            // populate the local variables table
            var methodDeclaration = memberReferenceExpression.parent<MethodDeclaration>();
            if (methodDeclaration != null)
                o2AstResolver.resolver.RunLookupTableVisitor(methodDeclaration);
            return o2AstResolver.resolve((memberReferenceExpression as Expression));
           
        }*/

        public static string getNodeText(this ResolveResult resolveResult)
        {
            if (resolveResult is MemberResolveResult)
                return (resolveResult as MemberResolveResult).ResolvedMember.DotNetName;

            else if (resolveResult is MethodGroupResolveResult)
            {
                var  resolvedNames = new List<string>();
                foreach (var groupResult in (resolveResult as MethodGroupResolveResult).Methods)
                    foreach (var method in groupResult)
                        resolvedNames.Add(method.DotNetName);
                if (resolvedNames.Count == 1)
                    return resolvedNames[0];
                return resolvedNames.str(); ;
            }
            else
                if (resolveResult != null)
                    return resolveResult.typeName();
                else
                    return "[RESOLVED WAS NULL]";                         
        }


        
    }
}
