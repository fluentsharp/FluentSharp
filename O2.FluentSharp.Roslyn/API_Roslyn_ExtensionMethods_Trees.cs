using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roslyn.Compilers.CSharp;
using O2.DotNetWrappers.ExtensionMethods;
using Roslyn.Services;
using Roslyn.Services.Formatting;
using Roslyn.Compilers;
using Roslyn.Compilers.Common;

namespace O2.XRules.Database.APIs
{
    public static class API_Roslyn_ExtensionMethods_Trees
	{
		public static SyntaxTree tree(this string code)
		{
			return code.parse();
		}
		
		public static SyntaxTree parse(this string code)
		{
			return SyntaxTree.ParseCompilationUnit(code);			
		}
		
		public static StatementSyntax parse_Statement(this string code)
		{
			return Syntax.ParseStatement(code);
		}
		
		
		public static List<Diagnostic> errors(this SyntaxTree tree)
		{
			return (from diagnostic in tree.GetDiagnostics()
					where diagnostic.Info.Severity == DiagnosticSeverity.Error
					select diagnostic).toList();
		}

        public static string errors_Details(this SyntaxTree tree)
        {
            var details = new StringBuilder();
            foreach (var error in tree.errors())
                details.AppendLine(error.str());
            return details.str();
        }

		public static bool astOk(this string code)
		{
			return code.hasErrors();
		}
		
		public static bool hasErrors(this string code)
		{
			return code.tree().errors().size() > 0;
		}

        public static string formatedCode(this CommonSyntaxNode tree)
        {
            return tree.Format()
                       .GetFormattedRoot()
                       .GetFullText();
        }
	}    
}
