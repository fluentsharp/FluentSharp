using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.Ast;

namespace O2.API.AST.ExtensionMethods.CSharp
{
    public static class CompilationUnit_ExtensionMethods
    {
        public static CompilationUnit insert(this CompilationUnit compilationUnit, INode node)
        {
            compilationUnit.children().Insert(0, node);
            return compilationUnit;
        }

        public static List<INode> children(this CompilationUnit compilationUnit)
        {
            return compilationUnit.CurrentBock.Children;
        }

        

    }
}
