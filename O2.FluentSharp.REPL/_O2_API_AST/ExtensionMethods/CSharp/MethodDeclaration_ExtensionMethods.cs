using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory;
using O2.API.AST.CSharp;

using O2.DotNetWrappers.ExtensionMethods;
using ICSharpCode.SharpDevelop.Dom;

namespace O2.API.AST.ExtensionMethods.CSharp
{
	public static class MethodDeclaration_ExtensionMethods
    {
        #region create

        public static TypeDeclaration add_Method(this TypeDeclaration typeDeclaration, MethodDeclaration methodDeclaration)
        {
            if (typeDeclaration != null && methodDeclaration != null)
                typeDeclaration.AddChild(methodDeclaration);
            return typeDeclaration;
        }

        public static CompilationUnit add_Method(this CompilationUnit compilationUnit, IClass iClass, MethodDeclaration methodDeclaration)
        {
            var typeDeclaration = compilationUnit.add_Type(iClass);
            compilationUnit.add_Method(typeDeclaration, methodDeclaration);
            return compilationUnit;
        }        

        public static CompilationUnit add_Method(this CompilationUnit compilationUnit, string @namespace, string typeName, MethodDeclaration methodDeclaration)
        {
            var typeDeclaration = compilationUnit.add_Type(@namespace, typeName);
            compilationUnit.add_Method(typeDeclaration, methodDeclaration);
            return compilationUnit;
        }
               
        public static CompilationUnit add_Method(this CompilationUnit compilationUnit, TypeDeclaration typeDeclaration, MethodDeclaration methodDeclaration)
        {                                   
            typeDeclaration.add_Method(methodDeclaration);
            return compilationUnit;
        }

        public static MethodDeclaration add_Method(this TypeDeclaration typeDeclaration, string methodName)
        {
            return typeDeclaration.add_Method(methodName, null, true,  null);
        }

        public static MethodDeclaration add_Method(this TypeDeclaration typeDeclaration, string methodName, BlockStatement body)
        {
            return typeDeclaration.add_Method(methodName, null, true, body);
        }

        public static MethodDeclaration add_Method(this TypeDeclaration typeDeclaration, string methodName, Dictionary<string, object> invocationParameters, bool resolveInvocationParametersType, BlockStatement body)
        {
			var newMethod = new MethodDeclaration();
			newMethod.Body = body;
			newMethod.Modifier = Modifiers.None | Modifiers.Public;
			newMethod.Name = methodName;
            newMethod.setReturnType();
            if (invocationParameters != null)

                foreach (var invocationParameter in invocationParameters)
                {
                    var resolvedType = (resolveInvocationParametersType && invocationParameter.Value != null && invocationParameter.Key != "returnData")
                                               ? invocationParameter.Value.typeFullName()
                                               : "dynamic";
                                               //: "System.Object";
                    var parameterType = new TypeReference(resolvedType, true);
                    var parameter = new ParameterDeclarationExpression(parameterType, invocationParameter.Key);
                    newMethod.Parameters.Add(parameter);

                }
            typeDeclaration.AddChild(newMethod);
            return newMethod;
        }

        public static TypeReference setReturnType(this MethodDeclaration methodDeclaration)
        {
            var blockStatement = methodDeclaration.Body;
            if (false == blockStatement.hasReturnStatement())
                methodDeclaration.TypeReference = new TypeReference("void", true);
            else
            {
				methodDeclaration.TypeReference = new TypeReference("System.Object", true);                

				blockStatement.add_Return(null); // add an extra default return value;

				//trying to set the return type was creating problems like  the error caused by not have a final return statement
				/*
                if (methodDeclaration.iNodes<ReturnStatement>().size() > 1) // if there are more than one return statement default to return object
                {
                    methodDeclaration.TypeReference = new TypeReference("System.Object", true);
                }
                else
                {
                    var returnValue = blockStatement.getLastReturnValue() ?? new object();  // see if we can resolve it to a known type (or default to object)
                    methodDeclaration.TypeReference = new TypeReference(returnValue.typeFullName(), true);

                }
				*/
				
            }
            return methodDeclaration.TypeReference;
        }

        public static MethodDeclaration returnType(this MethodDeclaration methodDeclaration, string returnType)
        {
            methodDeclaration.TypeReference = new TypeReference(returnType);
            return methodDeclaration;
        }

        public static BlockStatement add_Body(this MethodDeclaration methodDeclaration)
        {
            var blockDeclaration = new BlockStatement();
            methodDeclaration.Body = blockDeclaration;
            return blockDeclaration;
        }
        #endregion

        #region query

        public static List<MethodDeclaration> methods(this TypeDeclaration typeDeclaration)
        {
            var methods = from child in typeDeclaration.Children
                          where child is MethodDeclaration
                          select (MethodDeclaration)child;
            return methods.ToList();
        }

        public static List<MethodDeclaration> methods(this IParser parser)
        {
            return parser.CompilationUnit.methods();
        }

        public static List<MethodDeclaration> methods(this CompilationUnit compilationUnit)
        {
            return compilationUnit.types(true).methods();
        }

        public static List<MethodDeclaration> methods(this List<TypeDeclaration> typeDeclarations)
        {
            var methods = new List<MethodDeclaration>();
            foreach (var typeDeclaration in typeDeclarations)
                methods.AddRange(typeDeclaration.methods());
            return methods;
        }

        public static MethodDeclaration method(this IParser parser, string name)
        {
            return parser.CompilationUnit.method(name);
        }

        public static MethodDeclaration method(this CompilationUnit compilationUnit, string name)
        {
            return compilationUnit.types(true).methods().method(name);
        }

        public static MethodDeclaration method(this List<MethodDeclaration> methodDeclarations, string name)
        {
            foreach (var methodDeclaration in methodDeclarations)
                if (methodDeclaration.name() == name)
                    return methodDeclaration;
            return null;
        }

        public static TypeReference returnType(this MethodDeclaration methodDeclaration)
        {
            return methodDeclaration.TypeReference;
        }

        public static List<TypeReference> returnTypes(this List<MethodDeclaration> methodDeclarations)
        {
            var returnTypes = from methodDeclaration in methodDeclarations
                              select methodDeclaration.returnType();
            return returnTypes.ToList();
        }

        public static MethodDeclaration name(this List<MethodDeclaration> methodDeclarations, string name)
        {
            return methodDeclarations.method(name);
        }

        public static string name(this MethodDeclaration methodDeclaration)
        {
            return methodDeclaration.Name;
        }

        public static List<string> names(this List<MethodDeclaration> methodDeclarations)
        {
            var names = from methodDeclaration in methodDeclarations
                          select methodDeclaration.Name;
            return names.ToList();
        }

        public static List<LocalVariableDeclaration> variables(this List<MethodDeclaration> methodDeclarations)
        {
            var variables = new List<LocalVariableDeclaration>();
            foreach (var methodDeclaration in methodDeclarations)
                variables.AddRange(methodDeclaration.variables());
            return variables;
        }

        public static List<LocalVariableDeclaration> variables(this MethodDeclaration methodDeclaration)
        {
            var astVisitors = new AstVisitors();
            methodDeclaration.AcceptVisitor(astVisitors, null);
            return astVisitors.localVariableDeclarations;
        }

        public static List<InvocationExpression> invocations(this List<MethodDeclaration> methodDeclarations)
        {
            var invocations = new List<InvocationExpression>();
            foreach (var methodDeclaration in methodDeclarations)
                invocations.AddRange(methodDeclaration.invocations());
            return invocations;
        }

        public static List<InvocationExpression> invocations(this MethodDeclaration methodDeclaration)
        {
            var astVisitors = new AstVisitors();
            methodDeclaration.AcceptVisitor(astVisitors, null);
            return astVisitors.invocationExpressions;
        }

        public static bool validBody(this MethodDeclaration methodDeclaration)
        {
            return (methodDeclaration.Body != null && methodDeclaration.Body.Children != null && methodDeclaration.Body.Children.Count > 0);
        }

        #endregion

        #region sourcecode

        public static string sourceCode(this MethodDeclaration methodDeclaration, string sourceCodeFile)
        {
            var startLine = methodDeclaration.StartLocation.Line;
            var endLine = (methodDeclaration.Body != null)
                            ? methodDeclaration.Body.EndLocation.Line
                            : startLine + 1;
            return sourceCodeFile.fileSnippet(startLine - 1, endLine);
        }

        
        public static Location firstLineOfCode(this MethodDeclaration methodDeclaration)
        {
            if (methodDeclaration.validBody())
                return methodDeclaration.Body.Children[0].StartLocation;
            return new Location(0, 0);
        }
        #endregion
    }
}
