using O2.DotNetWrappers.ExtensionMethods;

namespace UnitTests.FluentSharp_REPL
{
    public class Default_Helpers
    {
        public static string CodeSnippet_HelloWord { get; set; }
        public static string CSharpFile_HelloWord { get; set; }

        static  Default_Helpers()
        {
            CodeSnippet_HelloWord = @"return ""Hello World"";";
            CSharpFile_HelloWord  = CodeSnippet_HelloWord.createCSharpFileFromCodeSnippet();
        }
    }
}
