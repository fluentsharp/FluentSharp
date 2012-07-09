using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;

using O2.DotNetWrappers.ExtensionMethods;
using O2.API.AST.ExtensionMethods.CSharp;
using O2.API.AST.Visitors;

namespace O2.API.AST.ExtensionMethods
{
    public static class IAstVisitor_ExtensionMethods
    {
        // IAST VIsitor extensionMethods

        public static IAstVisitor loadCode(this IAstVisitor astVisitor, string code)
        {
            return astVisitor.loadCode(code, null);
        }

        public static IAstVisitor loadCode(this IAstVisitor astVisitor, string code, object data)
        {
            code = (code.fileExists()) ? code.fileContents() : code;
            var parser = code.csharpAst();
            astVisitor.loadINode(parser.CompilationUnit, data);
            return astVisitor;
        }

        public static IAstVisitor loadINode<T>(this IAstVisitor astVisitor, T node) where T : INode
        {
            return astVisitor.loadINode(node, null);
        }

        public static IAstVisitor loadINode<T>(this IAstVisitor astVisitor, T node, object data) where T : INode
        {
            node.AcceptVisitor(astVisitor, data);
            return astVisitor;
        }

        public static List<T> allByType<T>(this GetAllINodes getAllNodes)
        where T : INode
        {
            var results = from node in getAllNodes.AllNodes
                          where node is T
                          select (T)node;
            return results.ToList();
        }

        public static INode getINodeAt(this GetAllINodes getAllNodes, int line, int column)
        {
            var iNodes = getAllNodes.getINodesAt(line, column);
            INode match = null;
            // find which one of the iNodes is the one closer to the provided line and column
            foreach (var node in iNodes)
            {
                if (match == null)
                    match = node;
                else
                    if (node.StartLocation >= match.StartLocation || node.EndLocation <= match.EndLocation)
                        match = node;
            }
            return match;
        }

        public static List<INode> getINodesAt(this GetAllINodes getAllNodes, int line, int column)
        {
            return getAllNodes.getINodesAt(new Location(column, line));
        }

        public static List<INode> getINodesAt(this GetAllINodes getAllNodes, Location innerLocation)
        {
            var matches = new List<INode>();
            foreach (var node in getAllNodes.AllNodes)
                if (node.StartLocation <= innerLocation && innerLocation <= node.EndLocation)
                    matches.add(node);
            return matches;
        }

        public static List<INode> find<T>(this GetAllINodes getAllNodes, string propertyName, object value)
            where T : INode
        {
            var matches = new List<INode>();
            var allByType = getAllNodes.allByType<T>();
            foreach (var node in allByType)
            {
                var propValue = node.prop(propertyName);
                if (propValue != null && value != null && propValue.str().contains(value.str()))
                    matches.add(node);
            }
            return matches;
        }

        public static List<INode> getINodesWithIdentifier(this GetAllINodes getAllNodes, string identifier)
        {
            //"in getINodesWithIdentifier".debug();;
            var matches = new List<INode>();
            //var result = getAllNodes.find<IdentifierExpression>();
            //var identifiers =
            matches.add(getAllNodes.find<IdentifierExpression>("Identifier", identifier));
            matches.add(getAllNodes.find<ParameterDeclarationExpression>("ParameterName", identifier));
            matches.add(getAllNodes.find<VariableDeclaration>("Name", identifier));


            //"in {0} result".format(result.size()).error();
            //matches.add(result);
            //	matches.add(getAllNodes.allByType<LocalVariableDeclaration>());
            return matches;
        }                

        
    
    }
}
