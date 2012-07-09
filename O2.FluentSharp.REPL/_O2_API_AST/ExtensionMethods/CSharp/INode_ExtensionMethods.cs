using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.Ast;

using O2.DotNetWrappers.ExtensionMethods;
using O2.API.AST.CSharp;
using O2.API.AST.Visitors;

namespace O2.API.AST.ExtensionMethods.CSharp
{
    public static class INode_ExtensionMethods
    {
        public static T append<T>(this T iNode, INode iNodeToAppend)
            where T : INode
        {
            if (iNode.hash() != iNodeToAppend.hash())
                iNode.Children.Add(iNodeToAppend);
            return iNode;
        }

        public static List<INode> getAstPath(this INode node, int line, int column)
        {
            var findLocation = new FindLocationInAst(line, column);
            node.AcceptVisitor(findLocation, null);
            return findLocation.Matches;
        }

        public static T parent<T>(this INode iNode)
            where T : AbstractNode
        {
            if (iNode != null)
            {
                while (iNode.Parent != null && (iNode is T).isFalse())
                    iNode = iNode.Parent;
                if (iNode is T)
                    return iNode as T;
            }
            //"Could node find {0} for provided iNode: {1}".format(typeof(T).Name, iNode != null ? iNode.str() : "[null value]").error();
            return null;
        }

        public static CompilationUnit compilationUnit(this INode iNode)
        {
            return iNode.parent<CompilationUnit>();
        }

        public static MethodDeclaration methodDeclaration(this INode iNode)
        {
            return iNode.parent<MethodDeclaration>();
        }

        public static List<T> iNodes<T>(this INode iNode)
        {
            var iNodesInT = new List<T>();
            var childINodes = iNode.iNodes();
            foreach (var childINode in childINodes)
                if (childINode is T)
                    iNodesInT.add((T)childINode);
            return iNodesInT;
        }

        public static List<INode> iNodes(this INode iNode)
        {
            if (iNode == null)
                return null;
            var allINodes = new GetAllINodes();
            iNode.AcceptVisitor(allINodes, null);
            return allINodes.AllNodes;
        }

        public static List<T2> iNodes<T1, T2>(this T1 iNode)
            where T1 : INode
            where T2 : INode
        {
            if (iNode == null)
                return null;
            var results = from node in iNode.iNodes() where node is T2 select (T2)node;
            return results.ToList();
        }

        public static List<T> iNodes<T>(this List<INode> iNodes)
                 where T : INode
        {
            var iNodesInT = new List<T>();
            foreach (var iNode in iNodes)
                iNodesInT.AddRange(iNode.iNodes<T>());
            return iNodesInT;
        }
    
    }
}
