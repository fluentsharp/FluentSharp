using ICSharpCode.NRefactory.Ast;
using O2.DotNetWrappers.ExtensionMethods;

namespace O2.API.AST.ExtensionMethods.CSharp
{
    public static class VariableDeclaration_ExtensionMethods
    {
        public static VariableDeclaration add_Variable(this BlockStatement blockDeclaration, string name, object value)
        {
            var primitiveValue = new PrimitiveExpression(value, value.str());
            var typeReference = new TypeReference(value.typeName());
            return blockDeclaration.add_Variable(name, primitiveValue, typeReference);
        }
        //new TypeReference("String");
        public static VariableDeclaration add_Variable(this BlockStatement blockDeclaration, string name, Expression expression)
        {
            return blockDeclaration.add_Variable(name, expression, TypeReference.Null);
        }

        public static VariableDeclaration add_Variable(this BlockStatement blockDeclaration, string name, Expression expression, string typeReference)
        {
            return blockDeclaration.add_Variable(name, expression, new TypeReference(typeReference));
        }

        public static VariableDeclaration add_Variable(this BlockStatement blockDeclaration, string name, Expression expression, TypeReference typeReference)
        {
            var variableDeclaration = new VariableDeclaration(name, expression);
            variableDeclaration.TypeReference = typeReference;
            var localVariableDeclaration = new LocalVariableDeclaration(variableDeclaration);
            blockDeclaration.append(localVariableDeclaration);
            return variableDeclaration;
        }

    }
}
