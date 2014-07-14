using System;

namespace FluentSharp.AST
{
    public class CSharp_FastCompiler_Events
    {
        public Action OnAstFail                 { get; set; }
        public Action OnAstOK                   { get; set; }
        public Action OnCompileFail             { get; set; }
        public Action OnCompileOK               { get; set; }
        public Action BeforeSnippetAst          { get; set; }
        public Action BeforeCompile             { get; set; }
    }
}