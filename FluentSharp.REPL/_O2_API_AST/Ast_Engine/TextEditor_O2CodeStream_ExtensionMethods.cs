using FluentSharp.REPL;
using FluentSharp.REPL.Controls;

//O2File:Ast_Engine_ExtensionMethods.cs


namespace FluentSharp.CSharpAST.Utils
{
    public static class TextEditor_O2CodeStream_ExtensionMethods
    {
        public static O2CodeStream show(this O2CodeStream o2CodeStream, SourceCodeEditor codeEditor)
        {
            if (o2CodeStream != null)
            {
                codeEditor.open(o2CodeStream.SourceFile);
                codeEditor.colorINodes(o2CodeStream.iNodes());
            }           
            return o2CodeStream;
        }
    }
}
