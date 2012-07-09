using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory;
using System.IO;
using O2.DotNetWrappers.ExtensionMethods;

using ICSharpCode.NRefactory.Ast;

namespace O2.API.AST.ExtensionMethods.CSharp
{
    public static class Parser_ExtensionMethods
    {
        #region create

        public static UsingDeclaration add_Using(this IParser parser, string @namespace)
        {
            return parser.CompilationUnit.add_Using(@namespace);
            //return parser;
        }

        #endregion

        public static IParser csharpAst(this string csharpCodeOrFile)
        {
            var language = SupportedLanguage.CSharp;
            return csharpCodeOrFile.ast(language);
        }

        public static IParser vbnetAst(this string vbnetCodeOrFile)
        {
            var language = SupportedLanguage.VBNet;
            return vbnetCodeOrFile.ast(language);
        }

        public static IParser ast(this string csharpCodeOrFile,  SupportedLanguage language)
        {
            var codeToParse = (csharpCodeOrFile.fileExists()) ? csharpCodeOrFile.fileContents() : csharpCodeOrFile;

            var parser = ParserFactory.CreateParser(language, new StringReader(codeToParse));

            parser.Parse();
            return parser;
        }

        public static string errors(this SnippetParser snippetParser)
        {
            if (snippetParser.Errors.Count > 0)
                return snippetParser.Errors.ErrorOutput;
            return "";
        }

        public static void runForEachCompilationError(this string compilationErrors, Action<int, int> runOnError)
        {
            foreach (var items in compilationErrors.lines().split("::"))
                if (items.size() > 2)
                    runOnError(items[0].toInt(), items[1].toInt());
        }

        public static void runForEachAstParsingError(this string astParsingErrors, Action<int, int> runOnError)
        {
            foreach (var items in astParsingErrors.lines().split_onSpace())
                if (items.size() > 5 && items[1].eq("line") && items[3].eq("col"))
                {
                    runOnError(items[2].toInt(), items[4].toInt());
                }
        }

    }
}
