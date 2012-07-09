using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;

namespace O2.API.AST.ExtensionMethods.CSharp
{
    public static class Attribute_ExtensionMethods
    {

        public static T add_Attribute<T>(this T attributedNode, string attributeName)
    where T : AttributedNode
        {
            var attribute = new Attribute(attributeName, null, null);
            return attributedNode.add_Attribute(attribute);
        }

        public static T add_Attribute<T>(this T attributedNode, Attribute attribute)
            where T : AttributedNode
        {
            var attributeSection = new AttributeSection();
            attributeSection.Attributes.Add(attribute);
            return attributedNode.add_Attribute(attributeSection);
        }

        public static T add_Attribute<T>(this T attributedNode, AttributeSection attributeSection)
            where T : AttributedNode
        {
            attributedNode.Attributes.Add(attributeSection);
            return attributedNode;
        }


        public static List<Attribute> attributesAll(this IParser parser)
        {
            return parser.CompilationUnit.attributesAll();
        }

        public static List<Attribute> attributesAll(this CompilationUnit compilationUnit)
        {
            var allAttributes = new List<Attribute>();
            allAttributes.AddRange(compilationUnit.types(true).attributes()); // add all type's attributes
            allAttributes.AddRange(compilationUnit.methods().attributes());					// add all methods's attribtues
            return allAttributes;
        }

        public static List<Attribute> attributes(this List<TypeDeclaration> typeDeclarations)
        {
            var attributes = new List<Attribute>();
            foreach (var typeDeclaration in typeDeclarations)
                attributes.AddRange(typeDeclaration.attributes());
            return attributes;
        }

        public static List<Attribute> attributes(this TypeDeclaration typeDeclaration)
        {
            var result = from attributes in typeDeclaration.Attributes
                         from attribute in attributes.Attributes
                         select attribute;
            return result.ToList();

        }

        public static List<Attribute> attributes(this List<MethodDeclaration> methodDeclarations)
        {
            var attributes = new List<Attribute>();
            foreach (var methodDeclaration in methodDeclarations)
                attributes.AddRange(methodDeclaration.attributes());
            return attributes;
        }

        public static List<Attribute> attributes(this MethodDeclaration methodDeclaration)
        {
            var result = from attributes in methodDeclaration.Attributes
                         from attribute in attributes.Attributes
                         select attribute;
            return result.ToList();
        }

        public static string name(this Attribute attribute)
        {
            return attribute.Name;
        }

        public static List<string> names(this List<Attribute> attributes)
        {
            var names = from attribute in attributes select attribute.name();
            return names.ToList();
        }

        public static Attribute name(this List<Attribute> attributes, string name)
        {
            foreach (var attribute in attributes)
                if (attribute.name() == name)
                    return attribute;
            return null;
        }
        
    }
}
