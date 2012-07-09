using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickGraph;
using O2.DotNetWrappers.ExtensionMethods;

namespace O2.API.AST.ExtensionMethods
{
    public static class BidirectionalGraph_ExtensionMethods
    {
        public static BidirectionalGraph<object, IEdge<object>> vertex(this BidirectionalGraph<object, IEdge<object>> graph, object vertexToAdd)
        {
            return graph.add_Node(vertexToAdd);           
        }

        public static BidirectionalGraph<object, IEdge<object>> node(this BidirectionalGraph<object, IEdge<object>> graph, object vertexToAdd)
        {
            return graph.add_Node(vertexToAdd);
        }

        public static BidirectionalGraph<object, IEdge<object>> add_Node(this BidirectionalGraph<object, IEdge<object>> graph, object vertexToAdd)
        {
            if (vertexToAdd != null)
            {
                "adding vertex: {0}".info(vertexToAdd.str());
                graph.AddVertex(vertexToAdd);
            }
            else
            { 
            }
            return graph;
        }

        public static BidirectionalGraph<object, IEdge<object>> edge(this BidirectionalGraph<object, IEdge<object>> graph, object fromVertex, object toVertex)
        {
            try
            {
                if (fromVertex != null)
                    graph.AddVertex(fromVertex);
                if (toVertex != null)
                    graph.AddVertex(toVertex);
                if (fromVertex != null && toVertex != null)
                    graph.AddEdge(new Edge<object>(fromVertex, toVertex));
                else
                { 
                }
            }
            catch (System.Exception ex)
            { 
                ex.log("in edge"); 
            }
            return graph;
        }

        public static void edges(this BidirectionalGraph<object, IEdge<object>> bidirectionalGraph, List<IEdge<object>> edges)
        {
            try
            {
                bidirectionalGraph.AddVerticesAndEdgeRange(edges.ToArray());
            }
            catch (System.Exception ex)
            { ex.log("in edges"); }
        }

        public static void addList<T>(this BidirectionalGraph<object, IEdge<object>> graph, IEnumerable<T> list, string nodeText)
        {
            foreach (var item in list)
                graph.edge(nodeText, item.str());
        }


        #region search
        public static object firstOutEdge(this BidirectionalGraph<object, IEdge<object>> graph, object node)
        {
            var edges = graph.OutEdges(node).ToList();

            if (edges.Count > 0)
                return edges[0].Target;

            "in graph.firstOutEdge there were no edges found for provided node".error();
            return null;
        }
        #endregion
    }
}
