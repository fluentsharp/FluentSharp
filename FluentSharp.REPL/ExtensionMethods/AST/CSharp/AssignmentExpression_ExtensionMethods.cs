using ICSharpCode.NRefactory.Ast;

namespace FluentSharp.CSharpAST
{
    public static class AssignmentExpression_ExtensionMethods
    {
        public static AssignmentExpression  assign_To(this Expression left, Expression right)
        {
            return new AssignmentExpression(left, AssignmentOperatorType.Assign, right);
        }
        public static AssignmentExpression  assign_To(this Expression left, AssignmentOperatorType assignmentOperator, Expression right)
        {
            return new AssignmentExpression(left, assignmentOperator, right);
        }
        public static BlockStatement        add_Assignment(this BlockStatement blockStatement, string Identifier_Left, TypeDeclaration typeDeclaration_Right)
        {
            return blockStatement.add_Assignment(Identifier_Left.ast_Identifier(), typeDeclaration_Right.ast_ObjectCreate());
        }
        public static BlockStatement        add_Assignment(this BlockStatement blockStatement, PropertyDeclaration propertyDeclaration_Left, TypeDeclaration typeDeclaration_Right)
        {
            return blockStatement.add_Assignment(propertyDeclaration_Left, typeDeclaration_Right.ast_ObjectCreate());
        }
        public static BlockStatement        add_Assignment(this BlockStatement blockStatement, PropertyDeclaration propertyDeclaration_Left, Expression right)
        {
            return blockStatement.add_Assignment(propertyDeclaration_Left.Name.ast_Identifier(), right);
        }
        public static BlockStatement        add_Assignment(this BlockStatement blockStatement, Expression left, Expression right)
        {
            var assignment = left.assign_To(right);
            return blockStatement.append(assignment.expressionStatement());
        }
    }
}