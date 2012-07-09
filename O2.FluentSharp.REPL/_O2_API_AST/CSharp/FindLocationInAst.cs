using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.Visitors;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;

namespace O2.API.AST.CSharp
{
    public class FindLocationInAst : NodeTrackingAstVisitor
    {
        public Location StartLocation { get; set; }
        public Location EndLocation { get; set; }
        public Location InnerLocation { get; set; }
        public INode FirstMatch { get; set; }
        public INode LastMatch { get; set; }
        public List<INode> Matches { get; set; }

        private FindLocationInAst()
        {
            Matches = new List<INode>();
        }

        public FindLocationInAst(int line, int column)
            : this(new Location(column, line))
        {
        }

        public FindLocationInAst(Location innerLocation)
            : this()
        {
            InnerLocation = innerLocation;
        }

        public FindLocationInAst(Location startLocation, Location endLocation)
            : this()
        {
            StartLocation = startLocation;
            EndLocation = endLocation;
        }

        protected override void BeginVisit(INode node)
        {
            if (InnerLocation != null)			// 
            {
                if (node.StartLocation <= InnerLocation && InnerLocation <= node.EndLocation)
                    addMattch(node);
            }
            else if (node.StartLocation == StartLocation && node.EndLocation == EndLocation)
                addMattch(node);
            //"in Begin Visit for : {0}".format(node.typeName()).info();    	
        }

        protected override void EndVisit(INode node)
        {
            //"in End Visit for : {0}".format(node.typeName()).debug();
        }

        private void addMattch(INode node)
        {            
            Matches.Add(node);
            LastMatch = node;
            if (FirstMatch == null)
                FirstMatch = node;
        }
    }
}
