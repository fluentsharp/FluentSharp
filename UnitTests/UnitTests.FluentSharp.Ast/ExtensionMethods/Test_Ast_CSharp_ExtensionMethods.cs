using FluentSharp.AST;
using FluentSharp.NUnit;
using FluentSharp.WinForms;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;
using NUnit.Framework;

namespace UnitTests.FluentSharp.Ast
{
    [TestFixture]
    public class Test_Ast_CSharp_ExtensionMethods
    {
        [Test] public void ast_CSharp()
        {
            var codeSnippet = "var a = 12;";
            var cSharpCode  =  codeSnippet.csharp_FromSnippet().assert_Valid();
            var astCSharp   = cSharpCode.ast_CSharp();
            astCSharp.csharpCode().assert_Not_Null()
                                  .assert_Equals(astCSharp.SourceCode)
                                  .assert_Equals(cSharpCode);            
        }
        [Test]
        public void ast_CSharp_From_Snippet()
        {
            var codeSnippet = "var a = 12;";
            var astCSharp   =  codeSnippet.ast_CSharp_From_Snippet().assert_Not_Null();
            astCSharp.csharpCode().assert_Not_Null()
                                  .assert_Equals(codeSnippet.csharp_FromSnippet());            
        }
        [Test] public void ast_CSharp_CreateCompilableClass()
        {
            var codeSnippet = "var a = 1;\nreturn a;";
            var csharpCode  = codeSnippet.ast_CSharp_CreateCompilableClass();
            csharpCode.assert_Valid()
                      .assert_Contains("using System")
                      .assert_Contains(csharpCode);
        }
    }
}
