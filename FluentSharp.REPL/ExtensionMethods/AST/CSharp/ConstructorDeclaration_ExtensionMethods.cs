using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentSharp.CoreLib;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory;
using ICSharpCode.SharpDevelop.Dom;

namespace FluentSharp.CSharpAST
{
    public static class ConstructorDeclaration_ExtensionMethods
    {

        // add
        public static TypeDeclaration           add_Ctor(this TypeDeclaration typeDeclaration, ConstructorDeclaration constructorDeclaration)
        {
            if (typeDeclaration != null && constructorDeclaration != null)
                typeDeclaration.AddChild(constructorDeclaration);
            return typeDeclaration;
        }
        public static CompilationUnit           add_Ctor(this CompilationUnit compilationUnit, IClass iClass,  ConstructorDeclaration constructorDeclaration)
        {
            var typeDeclaration = compilationUnit.add_Type(iClass);
            typeDeclaration.add_Ctor(constructorDeclaration);
            return compilationUnit;
        }
        public static CompilationUnit           add_Ctor_(this CompilationUnit compilationUnit, string @namespace, string typeName, ConstructorDeclaration constructorDeclaration)
        {
            var myNamespace = compilationUnit.add_Namespace(@namespace);
            var type = myNamespace.add_Type(typeName);
            type.add_Ctor(constructorDeclaration);
            return compilationUnit;
        }
        public static ConstructorDeclaration    add_Ctor(this TypeDeclaration typeDeclaration)
        {
            var name = "";
            var modifier = Modifiers.Public;
            var _parameters = new List<ParameterDeclarationExpression>();
            var _attributes = new List<AttributeSection>();

            var ctorDeclaration = new ConstructorDeclaration(name, modifier, _parameters, _attributes);
            ctorDeclaration.Body = new BlockStatement();
            typeDeclaration.add_Ctor(ctorDeclaration);
            return ctorDeclaration;
        }
        // query
        public static List<Expression>              parameters(this ConstructorDeclaration constructorDeclaration)
        {
            if (constructorDeclaration.ConstructorInitializer != null)
                return constructorDeclaration.ConstructorInitializer.Arguments;
            return new List<Expression>();
        }
        public static List<ConstructorDeclaration>  constructors(this IParser iParser)
        {
            return iParser.CompilationUnit.constructors();
        }
        public static List<ConstructorDeclaration>  constructors(this CompilationUnit compilationUnit)
        {
            return compilationUnit.types(true).constructors();
        }
        public static List<ConstructorDeclaration>  constructors(this List<TypeDeclaration> typeDeclarations)
        {
            var constructors = new List<ConstructorDeclaration>();
            foreach (var typeDeclaration in typeDeclarations)
                constructors.AddRange(typeDeclaration.constructors());
            return constructors;
        }
        public static List<ConstructorDeclaration>  constructors(this TypeDeclaration typeDeclaration)
        {
            var constructors = from child in typeDeclaration.Children
                               where child is ConstructorDeclaration
                               select (ConstructorDeclaration)child;
            return constructors.ToList();
        }
        
        //MISC (to map)
        public static ObjectCreateExpression        ast_ObjectCreate(this TypeDeclaration typeDeclaration)
        {
            return typeDeclaration.typeReference().ast_ObjectCreate();
        }
        public static ObjectCreateExpression        ast_ObjectCreate(this TypeReference typeReference)
        {
            return new ObjectCreateExpression(typeReference, null);
        }
        public static TypeReference                 ast_TypeReference(this string type)
        {
            return new TypeReference(type, true);
        }        
        public static BlockStatement                body(this ConstructorDeclaration constructorDeclaration)
        {
            if (constructorDeclaration.Body.isNull())
                constructorDeclaration.Body = new BlockStatement();
            return constructorDeclaration.Body;
        }
    }
}
