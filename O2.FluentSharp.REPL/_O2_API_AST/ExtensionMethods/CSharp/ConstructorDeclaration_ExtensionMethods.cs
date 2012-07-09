using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory;
using ICSharpCode.SharpDevelop.Dom;

namespace O2.API.AST.ExtensionMethods.CSharp
{
    public static class ConstructorDeclaration_ExtensionMethods
    {

        #region add

        public static TypeDeclaration add_Ctor(this TypeDeclaration typeDeclaration, ConstructorDeclaration constructorDeclaration)
        {
            if (typeDeclaration != null && constructorDeclaration != null)
                typeDeclaration.AddChild(constructorDeclaration);
            return typeDeclaration;
        }

        public static CompilationUnit add_Ctor(this CompilationUnit compilationUnit, IClass iClass,  ConstructorDeclaration constructorDeclaration)
        {
            var typeDeclaration = compilationUnit.add_Type(iClass);
            typeDeclaration.add_Ctor(constructorDeclaration);
            return compilationUnit;
        }

        public static CompilationUnit add_Ctor_(this CompilationUnit compilationUnit, string @namespace, string typeName, ConstructorDeclaration constructorDeclaration)
        {
            var myNamespace = compilationUnit.add_Namespace(@namespace);
            var type = myNamespace.add_Type(typeName);
            type.add_Ctor(constructorDeclaration);
            return compilationUnit;
        }

        #endregion

        #region query

        public static List<Expression> parameters(this ConstructorDeclaration constructorDeclaration)
        {
            if (constructorDeclaration.ConstructorInitializer != null)
                return constructorDeclaration.ConstructorInitializer.Arguments;
            return new List<Expression>();
        }

        public static List<ConstructorDeclaration> constructors(this IParser iParser)
        {
            return iParser.CompilationUnit.constructors();
        }

        public static List<ConstructorDeclaration> constructors(this CompilationUnit compilationUnit)
        {
            return compilationUnit.types(true).constructors();
        }

        public static List<ConstructorDeclaration> constructors(this List<TypeDeclaration> typeDeclarations)
        {
            var constructors = new List<ConstructorDeclaration>();
            foreach (var typeDeclaration in typeDeclarations)
                constructors.AddRange(typeDeclaration.constructors());
            return constructors;
        }

        public static List<ConstructorDeclaration> constructors(this TypeDeclaration typeDeclaration)
        {
            var constructors = from child in typeDeclaration.Children
                               where child is ConstructorDeclaration
                               select (ConstructorDeclaration)child;
            return constructors.ToList();
        }
        #endregion
    }
}
