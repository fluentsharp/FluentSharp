using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using O2.DotNetWrappers.ExtensionMethods;
using ICSharpCode.SharpDevelop.Dom;
using ICSharpCode.NRefactory.Ast;

namespace O2.API.AST.ExtensionMethods.CSharp
{
    public static class PropertyDeclaration_ExtensionMethods
    {

        public static PropertyDeclaration add_Property(this CompilationUnit compilationUnit, IProperty iProperty,  PropertyDeclaration propertyDeclaration)
        {
            var propertyType = compilationUnit.add_Type(iProperty.DeclaringType);
            return propertyType.add_Property(propertyDeclaration);
        }

        public static PropertyDeclaration add_Property(this CompilationUnit compilationUnit, IProperty iProperty)
        {
            var propertyType = compilationUnit.add_Type(iProperty.DeclaringType);
            return propertyType.add_Property(iProperty);            
        }

        public static PropertyDeclaration add_Property(this TypeDeclaration typeDeclaration, PropertyDeclaration propertyDeclaration)
        {
            if (typeDeclaration.notNull() && propertyDeclaration.notNull() && typeDeclaration.Children.notNull())
            {
                var insertPosition = typeDeclaration.Children.Count;
                typeDeclaration.Children.Insert(insertPosition, propertyDeclaration);                 
            }
            return propertyDeclaration;
            
        }

        public static PropertyDeclaration add_Property(this TypeDeclaration typeDeclaration, IProperty iProperty)
        {           
            foreach (var child in typeDeclaration.Children)
                if (child is PropertyDeclaration)
                    if ((child as PropertyDeclaration).Name == iProperty.Name)
                        return (child as PropertyDeclaration);                    
            //if (field != null)
            //    return field;
            AttributedNode property = null;
            var classFinder = new ClassFinder(iProperty.DeclaringType, 0, 0);

            property = ICSharpCode.SharpDevelop.Dom.Refactoring.CodeGenerator.ConvertMember(iProperty, classFinder);
            if (property != null && property is PropertyDeclaration)
                return typeDeclaration.add_Property(property as  PropertyDeclaration);
                //typeDeclaration.Children.Insert(0, property);
            if (property is PropertyDeclaration)
                return (PropertyDeclaration)property;

            "in add_Property could not convert property into PropertyDeclaration, because it is: {0}".error(property.typeName());
            return null;
        }

        public static PropertyDeclaration add_Property(this TypeDeclaration typeDeclaration, string propertyType, string propertyName)
        {
            return null;
        }
    }
}
