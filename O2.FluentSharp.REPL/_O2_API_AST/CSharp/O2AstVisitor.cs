using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory.Visitors;

namespace O2.API.AST.CSharp
{
    //helper class to aid in the quick creation of AstVisitors
    public class O2AstVisitor : AbstractAstVisitor
    {
        public Func<MethodDeclaration, object,object> methodDeclarationVisit { get; set; }
        public Func<TypeReference, object, object> typeReferenceVisit { get; set; }
        public Func<LocalVariableDeclaration, object, object> localVariableDeclarationVisit { get; set; }
        public Func<VariableDeclaration, object, object> variableDeclarationVisit { get; set; }
        public Func<PrimitiveExpression, object, object> primitiveExpressionVisit { get; set; }
        public Func<ReturnStatement, object, object> returnStatementVisit { get; set; }
        public Func<IdentifierExpression, object, object> identifierExpressionVisit { get; set; }
        public Func<AssignmentExpression, object, object> assignmentExpressionVisit { get; set; }
        public Func<InvocationExpression, object, object> invocationExpressionVisit { get; set; }
        public Func<ExpressionStatement, object, object> expressionStatementVisit { get; set; }
        public Func<MemberReferenceExpression, object, object> memberReferenceExpressionVisit { get; set; }
        public Func<ParameterDeclarationExpression, object, object> parameterDeclarationExpressionVisit { get; set; }
        public Func<ObjectCreateExpression, object, object> objectCreateExpressionVisit { get; set; }

        public override object VisitMethodDeclaration(MethodDeclaration methodDeclaration, object data)
        {
            if (methodDeclarationVisit != null)
                data = methodDeclarationVisit(methodDeclaration, data);
            return base.VisitMethodDeclaration(methodDeclaration, data);
        }
        public override object VisitTypeReference(TypeReference typeReference, object data)
        {
            if (typeReferenceVisit != null)
                data = typeReferenceVisit(typeReference, data);
            return base.VisitTypeReference(typeReference, data);
        }
        public override object VisitLocalVariableDeclaration(LocalVariableDeclaration localVariableDeclaration, object data)
        {
            if (localVariableDeclarationVisit != null)
                data = localVariableDeclarationVisit(localVariableDeclaration, data);
            return base.VisitLocalVariableDeclaration(localVariableDeclaration, data);
        }
        public override object VisitVariableDeclaration(VariableDeclaration variableDeclaration, object data)
        {
            if (variableDeclarationVisit != null)
                data = variableDeclarationVisit(variableDeclaration, data);
            return base.VisitVariableDeclaration(variableDeclaration, data);
        }
        public override object VisitPrimitiveExpression(PrimitiveExpression primitiveExpression, object data)
        {
            if (primitiveExpressionVisit != null)
                data = primitiveExpressionVisit(primitiveExpression, data);
            return base.VisitPrimitiveExpression(primitiveExpression, data);
        }
        public override object VisitReturnStatement(ReturnStatement returnStatement, object data)
        {
            if (returnStatementVisit != null)
                data = returnStatementVisit(returnStatement, data);
            return base.VisitReturnStatement(returnStatement, data);
        }
        public override object VisitIdentifierExpression(IdentifierExpression identifierExpression, object data)
        {
            if (identifierExpressionVisit != null)
                data = identifierExpressionVisit(identifierExpression, data);
            return base.VisitIdentifierExpression(identifierExpression, data);
        }
        public override object VisitAssignmentExpression(AssignmentExpression assignmentExpression, object data)
        {
            if (assignmentExpressionVisit != null)
                data = assignmentExpressionVisit(assignmentExpression, data);
            return base.VisitAssignmentExpression(assignmentExpression, data);
        }
        public override object VisitInvocationExpression(InvocationExpression invocationExpression, object data)
        {
            if (invocationExpressionVisit != null)
                data = invocationExpressionVisit(invocationExpression, data);   
            return base.VisitInvocationExpression(invocationExpression, data);
        }
        public override object VisitExpressionStatement(ExpressionStatement expressionStatement, object data)
        {
            if (expressionStatementVisit != null)
                data = expressionStatementVisit(expressionStatement, data); 
            return base.VisitExpressionStatement(expressionStatement, data);
        }
        public override object VisitMemberReferenceExpression(MemberReferenceExpression memberReferenceExpression, object data)
        {
            if (memberReferenceExpressionVisit != null)
                data = memberReferenceExpressionVisit(memberReferenceExpression, data); 
            return base.VisitMemberReferenceExpression(memberReferenceExpression, data);
        }
        public override object VisitParameterDeclarationExpression(ParameterDeclarationExpression parameterDeclarationExpression, object data)
        {
            if (parameterDeclarationExpressionVisit != null)
                data = parameterDeclarationExpressionVisit(parameterDeclarationExpression, data); 
            return base.VisitParameterDeclarationExpression(parameterDeclarationExpression, data);
        }
        public override object VisitObjectCreateExpression(ObjectCreateExpression objectCreateExpression, object data)
        {
            if (objectCreateExpressionVisit != null)
                data = objectCreateExpressionVisit(objectCreateExpression, data); 
            return base.VisitObjectCreateExpression(objectCreateExpression, data);
        }
    }
}
