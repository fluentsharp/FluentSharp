using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory;
using ICSharpCode.SharpDevelop.Dom;
using O2.DotNetWrappers.ExtensionMethods;

namespace O2.API.AST.ExtensionMethods.CSharp
{   
    public static class TypeDeclaration_ExtensionMethods
    {
        #region create

        public static TypeDeclaration add_Type(this NamespaceDeclaration namespaceDeclaration, IClass iClass)
        {
            // move to method IClass.typeDeclaration();
            var typeName = iClass.Name;

            var newType = namespaceDeclaration.type(typeName);		// check if already exists and if it does return it
            if (newType != null)
                return newType;

            const Modifiers modifiers = Modifiers.None | Modifiers.Public;
            newType = new TypeDeclaration(modifiers, new List<AttributeSection>());
            newType.Name = typeName;

            foreach (var baseType in iClass.BaseTypes)
            {
                if (baseType.FullyQualifiedName != "System.Object")  // no need to include this one                
                    newType.BaseTypes.Add(new TypeReference(baseType.FullyQualifiedName));
            }

            namespaceDeclaration.AddChild(newType);

            return newType;

            //return namespaceDeclaration.add_Type(iClass.Name);
        }

        /*public static TypeDeclaration add_Type(this CompilationUnit compilationUnit, IReturnType iReturnType)
        { 
           
        }*/

        public static TypeDeclaration add_Type(this CompilationUnit compilationUnit, IClass iClass)
        {
            try
            {       
                if (iClass.Namespace.valid())
                {
                    var namespaceDeclaration = compilationUnit.add_Namespace(iClass.Namespace);
                    return namespaceDeclaration.add_Type(iClass);
                }

                // move to method IClass.typeDeclaration();
                var typeName = iClass.Name;

                var newType = compilationUnit.type(typeName);		// check if already exists and if it does return it
                if (newType != null)
                    return newType;

                const Modifiers modifiers = Modifiers.None | Modifiers.Public;
                newType = new TypeDeclaration(modifiers, new List<AttributeSection>());
                newType.Name = typeName;                

                foreach (var baseType in iClass.BaseTypes)
                {
                    newType.BaseTypes.Add(new TypeReference(baseType.FullyQualifiedName));
                }

                compilationUnit.AddChild(newType);

                return newType;
              //  return newType;


                /*var classFinder = new ClassFinder(iClass,0,0);
                var typeReference = (TypeReference)ICSharpCode.SharpDevelop.Dom.Refactoring.CodeGenerator.ConvertType(iClass.DefaultReturnType, classFinder);
                if (typeReference != null)
                { 
                    compilationUnit.AddChild(typeReference);
                    return typeReference;
                }*/
                //return compilationUnit.add_Type_(iClass.Namespace, iClass.Name);
            }
            catch (Exception ex)
            {
                ex.log("in TypeReference.add_Type");                
            }
            return compilationUnit.add_Type(iClass.Namespace, iClass.Name);
        }

        // should be merged with the one using CompilationUnit
        public static TypeDeclaration add_Type(this NamespaceDeclaration namespaceDeclaration, string typeName)
        {
            var newType = namespaceDeclaration.type(typeName);		// check if already exists and if it does return it
            if (newType != null)
                return newType;            

            const Modifiers modifiers = Modifiers.None | Modifiers.Public;
            newType = new TypeDeclaration(modifiers, new List<AttributeSection>());
            newType.Name = typeName;
            namespaceDeclaration.AddChild(newType);
            return newType;
        }

        public static TypeDeclaration add_Type(this CompilationUnit compilationUnit, string @namespace, string typeName)
        {
            if (@namespace.valid())
            {
                var typeNamespace = compilationUnit.add_Namespace(@namespace);
                return typeNamespace.add_Type(typeName);
            }
            else
                return compilationUnit.add_Type(typeName);
        }
        
        public static TypeDeclaration add_Type(this CompilationUnit compilationUnit, string typeName)
        {
            const Modifiers modifiers = Modifiers.None | Modifiers.Public;
            var newType = new TypeDeclaration(modifiers, new List<AttributeSection>());
            newType.Name = typeName;
            compilationUnit.AddChild(newType);
            return newType;
        }

        #endregion

        #region query

        public static List<TypeDeclaration> types(this IParser parser)
        {
            return parser.CompilationUnit.types(false);
        }

        public static List<TypeDeclaration> types(this IParser parser, bool recursive)
        {
            return parser.CompilationUnit.types(recursive);
        }

        public static List<TypeDeclaration> types(this CompilationUnit compilationUnit)
        {
            return ((AbstractNode)compilationUnit).types(false);
        }

        public static List<TypeDeclaration> types(this CompilationUnit compilationUnit, bool recursive)
        {
            return ((AbstractNode)compilationUnit).types(recursive);            
        }

        public static List<TypeDeclaration> types(this TypeDeclaration typeDeclaration)
        {
            return ((AbstractNode)typeDeclaration).types(false);
        }

        public static List<TypeDeclaration> types(this TypeDeclaration typeDeclaration, bool recursive)
        {
            return ((AbstractNode)typeDeclaration).types(recursive);
        }

        public static List<TypeDeclaration> types(this List<TypeDeclaration> typeDeclarations)
        {
            return typeDeclarations.types(false);
        }

        public static List<TypeDeclaration> types(this List<TypeDeclaration> typeDeclarations, string typeToFind)
        { 
            return (from typeDeclaration in typeDeclarations
                    where typeDeclaration.Name == typeToFind
                    select typeDeclaration).ToList();
        }

        public static List<TypeDeclaration> types(this List<TypeDeclaration> typeDeclarations, bool recursive)
        {
            var types = new List<TypeDeclaration>();
            foreach (var typeDeclaration in typeDeclarations)
                types.AddRange(((AbstractNode)typeDeclaration).types(recursive));
            return types;
        }

        public static List<TypeDeclaration> types(this AbstractNode abstractNode)
        {
            return abstractNode.types(false);
        }

        public static List<TypeDeclaration> types(this AbstractNode abstractNode, bool recursive)
        {
            if (abstractNode == null)
                return new List<TypeDeclaration>();
            var types = (from child in abstractNode.Children
                         where child is TypeDeclaration
                         select (TypeDeclaration)child).ToList();
            if (recursive)
            {
                var childTypes = new List<TypeDeclaration>();
                foreach (AbstractNode child in abstractNode.Children)
                    childTypes.AddRange(child.types(true));
                types.AddRange(childTypes);
            }
            return types;
        }

        /*public static TypeDeclaration type(this CompilationUnit compilationUnit, string typeToFind)
        {
            foreach (var type in compilationUnit.types())
                if (type.Name == typeToFind)
                    return type;
            return null;
        }*/

        public static TypeDeclaration type(this NamespaceDeclaration namespaceDeclaration, string typeToFind)
        {
            foreach (var type in namespaceDeclaration.types())
                if (type.Name == typeToFind)
                    return type;
            return null;
        }

        public static List<string> values(this List<TypeDeclaration> typeDeclarations)
        {
            var values = new List<string>();
            foreach (var typeDeclaration in typeDeclarations)
                values.Add(typeDeclaration.Name);
            return values;
        }

        public static List<TypeDeclaration> types(this IParser parser, string name)
        {
            return parser.CompilationUnit.types(name);
        }

        public static List<TypeDeclaration> types(this CompilationUnit compilationUnit, string name)
        {
            return compilationUnit.types(true).types(name);
        }

        public static TypeDeclaration type(this IParser parser, string name)
        {
            return parser.CompilationUnit.type(name);
        }

        public static TypeDeclaration type(this CompilationUnit compilationUnit, string name)
        {
            return compilationUnit.types(true).type(name);
        }

        public static TypeDeclaration type(this List<TypeDeclaration> typeDeclarations, string name)
        {
            foreach (var typeDeclaration in typeDeclarations)
                if (typeDeclaration.Name == name)
                    return typeDeclaration;
            return null;
        }

        public static string name(this TypeDeclaration typeDeclaration)
        {
            return typeDeclaration.Name;
        }

        public static List<INode> iNodes(this List<TypeDeclaration> typeDeclarations)
        {
            var iNodes = new List<INode>();
            foreach (var typeDeclaration in typeDeclarations)
                iNodes.AddRange(typeDeclaration.iNodes());
            return iNodes;
        }
        #endregion
        
    }
}
