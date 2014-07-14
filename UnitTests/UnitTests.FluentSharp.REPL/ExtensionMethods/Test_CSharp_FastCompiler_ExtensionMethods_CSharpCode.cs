using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentSharp.AST;
using FluentSharp.CoreLib;
using FluentSharp.NUnit;
using FluentSharp.REPL;
using FluentSharp.WinForms;
using NUnit.Framework;
using FluentSharp.CSharpAST;

namespace UnitTests.FluentSharp_REPL.ExtensionMethods
{
    [TestFixture]
    public class Test_CSharp_FastCompiler_ExtensionMethods_CSharpCode
    {        
        /// <summary>
        /// This test will also check if the code generated will not contain an extra <code>return null;</code>command for the cases 
        /// where the code snippet does already provide a return command (which is a common practice in the repl).
        /// This is quite important since some compilers (like mono) will throw and 'unreacheable code exeception' if there are
        /// two return statements in a row
        /// </summary>
        [Test]
        public void csharp_CreateClass_FromSnippet()
        {
            var codeSnippet_With_Return    = "return 40+2;";            
            var codeSnippet_Without_Return = "var a = 40+2; a.str().info();";
            
            // confirm that the code snippets compile and execute ok
            codeSnippet_With_Return.executeCodeSnippet().cast<int>()   .assert_Is(42);
            codeSnippet_With_Return.executeCodeSnippet().cast<int>()   .assert_Is_Not(43);
            codeSnippet_With_Return.executeCodeSnippet().cast<string>().assert_Default();

            codeSnippet_Without_Return.executeCodeSnippet().assert_Null ();
            codeSnippet_Without_Return.executeCodeSnippet().cast<int>   ().assert_Default();
            codeSnippet_Without_Return.executeCodeSnippet().cast<string>().assert_Default();

            // get code snippets csharpCode (wrapped in a class)
            var cSharpCode_With_Explicit_Return = codeSnippet_With_Return   .csharp_CreateClass_FromSnippet()   .assert_Valid();
            var cSharpCode_With_Default_Return  = codeSnippet_Without_Return.csharp_CreateClass_FromSnippet()   .assert_Valid(); 

            //get AST             
            var ast_With_Explicit_Return       = cSharpCode_With_Explicit_Return.csharp_Ast()                   .assert_Not_Null();
            var ast_With_Default_Return        = cSharpCode_With_Default_Return .csharp_Ast()                   .assert_Not_Null();

            ast_With_Explicit_Return.methodDeclarations().assert_Size_Is(1)
                                    .first().returnStatements().assert_Size_Is(1, "There should only be one return statement");

            ast_With_Default_Return .methodDeclarations().assert_Size_Is(1)
                                    .first().returnStatements().assert_Size_Is(1, "There should only be one return statement");
        }
    }
}
