using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.PrettyPrinter;
using FluentSharp.CoreLib;

namespace FluentSharp.CSharpAST
{
    public static class CSharpSourceCode_ExtensionMethods
    {
        public static string csharpCode(this INode iNode)
        {
            try
            {
                var outputVisitor = new CSharpOutputVisitor();
                iNode.AcceptVisitor(outputVisitor, null);
                return outputVisitor.Text;
            }
            catch (Exception ex)
            {
                ex.log("in CSharpSourceCode_ExtensionMethods.csharpCode");
                return "error creating source code for iNode. Error message was: ".format(ex.Message) ;
            }
        }

        public static string csharpCode(this IParser parser)
        {
            return parser.CompilationUnit.csharpCode();
        }

        /*public static string csharpCode(this CompilationUnit compilationUnit)
        {
            var outputVisitor = new CSharpOutputVisitor();
            //using (SpecialNodesInserter.Install(specials, outputVisitor)) {
            compilationUnit.AcceptVisitor(outputVisitor, null);
            //}            
            return outputVisitor.Text;
        }*/
    }
}
