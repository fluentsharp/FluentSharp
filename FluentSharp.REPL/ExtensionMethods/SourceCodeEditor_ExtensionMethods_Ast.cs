using System.Windows.Forms;
using FluentSharp.CoreLib;
using FluentSharp.CSharpAST.Utils;
using FluentSharp.REPL.Controls;
using FluentSharp.WinForms;
using ICSharpCode.NRefactory.Ast;

namespace FluentSharp.REPL
{
    public static class SourceCodeEditor_ExtensionMethods_Ast
    {
        public static TreeView afterSelect_ShowAstInSourceCodeEditor(this TreeView treeView, SourceCodeEditor codeEditor)
        {
            return (TreeView)codeEditor.invokeOnThread(() =>
            {
                treeView.afterSelect<AstTreeView.ElementNode>((node) =>
                {
                    var element = (INode)node.field("element");
                    codeEditor.setSelectionText(element.StartLocation, element.EndLocation);
                });
                treeView.afterSelect<INode>((node) =>
                {
                    codeEditor.setSelectionText(node.StartLocation, node.EndLocation);
                });
                return treeView;
            });
        }
    }
}