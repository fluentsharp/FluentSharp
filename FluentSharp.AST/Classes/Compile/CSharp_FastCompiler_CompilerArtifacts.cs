using System.CodeDom.Compiler;
using System.Reflection;
using FluentSharp.CSharpAST.Utils;
using ICSharpCode.NRefactory.Ast;

namespace FluentSharp.AST
{
    public class CSharp_FastCompiler_CompilerArtifacts
    {
        public Ast_CSharp       AstCSharp                       { get; set; }
        public AstDetails       AstDetails                      { get; set; }
        public string			AstErrors                       { get; set; }        
        public CompilationUnit  CompilationUnit                 { get; set; }        

        public string			CompilationErrors		        { get; set; }      
        public Assembly			CompiledAssembly		        { get; set; }
        public CompilerResults	CompilerResults			        { get; set; }
        public bool             CreatedFromSnippet              { get; set; }        
        public string           SourceCode                      { get; set; }
        

        public CSharp_FastCompiler_CompilerArtifacts()
        {
            SourceCode = "";
        }
    }
}