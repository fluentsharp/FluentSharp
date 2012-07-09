using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory;

namespace O2.API.AST.ExtensionMethods.CSharp
{
    public static class ParameterDeclarationExpression_ExtensionMethods
    {      

        public static List<ParameterDeclarationExpression> parameters(this IParser parser)
        {
            return parser.CompilationUnit.parameters();
        }

        public static List<ParameterDeclarationExpression> parameters(this CompilationUnit compilationUnit)
        {
            return compilationUnit.types(true).parameters();
        }

        public static List<ParameterDeclarationExpression> parameters(this List<TypeDeclaration> typeDeclarations)
        {
            return typeDeclarations.methods().parameters();
        }

        public static List<ParameterDeclarationExpression> parameters(this List<MethodDeclaration> methodDeclarations)
        {
            var parameters = new List<ParameterDeclarationExpression>();
            foreach (var methodDeclaration in methodDeclarations)
                parameters.AddRange(methodDeclaration.parameters());
            return parameters;
        }

        public static List<ParameterDeclarationExpression> parameters(this MethodDeclaration methodDeclaration)
        {
            return methodDeclaration.Parameters;
        }

        public static List<TypeReference> types(this List<ParameterDeclarationExpression> parameters)
        {
            var types = from parameter in parameters select parameter.type();
            return types.ToList();
        }

        public static TypeReference type(this ParameterDeclarationExpression parameter)
        {
            return parameter.TypeReference;
        }

        public static string name(this ParameterDeclarationExpression parameter)
        {
            return parameter.ParameterName;
        }

        public static List<string> names(this List<ParameterDeclarationExpression> parameters)
        {
            var names = from parameter in parameters select parameter.ParameterName;
            return names.ToList();
        }

        public static ParameterDeclarationExpression name(this List<ParameterDeclarationExpression> parameters, string name)
        {
            foreach (var parameter in parameters)
                if (parameter.ParameterName == name)
                    return parameter;
            return null;
        }        
    }
}
