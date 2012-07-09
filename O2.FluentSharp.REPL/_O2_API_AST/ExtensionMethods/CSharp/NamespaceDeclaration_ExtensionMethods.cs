using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory;

namespace O2.API.AST.ExtensionMethods.CSharp
{
    public static class NamespaceDeclaration_ExtensionMethods
    {
        

        public static NamespaceDeclaration namespaces(this CompilationUnit compilationUnit, string namespaceToFind)
        {
            foreach (var @namespace in compilationUnit.namespaces())
                if (@namespace.Name == namespaceToFind)
                    return @namespace;
            return null;
        }

        public static List<NamespaceDeclaration> namespaces(this IParser parser)
        {
            return parser.CompilationUnit.namespaces();
        }

        public static List<NamespaceDeclaration> namespaces(this CompilationUnit compilationUnit)
        {
            var namespaces = from child in compilationUnit.Children
                             where child is NamespaceDeclaration
                             select (NamespaceDeclaration)child;
            return namespaces.ToList();
        }

        public static NamespaceDeclaration add_Namespace(this CompilationUnit compilationUnit, string @namespace)
        {
            var newNamespace = compilationUnit.namespaces(@namespace);		// check if already exists and if it does return it
            if (newNamespace != null)
                return newNamespace;
            newNamespace = new NamespaceDeclaration(@namespace);
            compilationUnit.Children.Add(newNamespace);
            return newNamespace;
        }

        /*public static IParser add_Namespace(this IParser parser, string @namespace)
        {
            parser.CompilationUnit.add_Namespace(@namespace);
            return parser;
        }*/

        /*public static CompilationUnit add_Namespace(this CompilationUnit compilationUnit, string @namespace)
        {
            var newNamespace = new NamespaceDeclaration(@namespace);
            compilationUnit.Children.Add(newNamespace);
            return compilationUnit;
        } */       
    }
}
