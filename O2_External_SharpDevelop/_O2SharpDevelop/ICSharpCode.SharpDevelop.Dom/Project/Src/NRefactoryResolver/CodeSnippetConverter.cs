// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <author name="Daniel Grunwald"/>
//     <version>$Revision: 5221 $</version>
// </file>

using System;
using System.Collections.Generic;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory.PrettyPrinter;
using O2.DotNetWrappers.ExtensionMethods;

namespace ICSharpCode.SharpDevelop.Dom.NRefactoryResolver
{
	/// <summary>
	/// Allows converting code snippets between C# and VB.
	/// This class isn't used by SharpDevelop itself (because it doesn't support projects).
	/// It works by creating a dummy project for the file to convert with a set of default references.
	/// </summary>
	public class CodeSnippetConverter
	{
		/// <summary>
		/// Project-wide imports to add to all files when converting VB to C#.
		/// </summary>
		public IList<string> DefaultImportsToAdd = new List<string>();
		
		/// <summary>
		/// Imports to remove (because they will become project-wide imports) when converting C# to VB.
		/// </summary>
		public IList<string> DefaultImportsToRemove = new List<string>();
		
		/// <summary>
		/// References project contents, for resolving type references during the conversion.
		/// </summary>
		public IList<IProjectContent> ReferencedContents = new List<IProjectContent>();
		
		DefaultProjectContent project;
		List<ISpecial> specials;
		CompilationUnit compilationUnit;
		ParseInformation parseInfo;
		bool wasExpression;
		
		CodeSnippetConverter()
		{
			DefaultImportsToAdd.Add("System.Diagnostics");
			DefaultImportsToAdd.Add("System.Data");
			DefaultImportsToAdd.Add("System.Collections.Generic");
			DefaultImportsToAdd.Add("System.Collections");
			DefaultImportsToAdd.Add("System");
			DefaultImportsToAdd.Add("Microsoft.VisualBasic");
			
			DefaultImportsToRemove.Add("Microsoft.VisualBasic");
			DefaultImportsToRemove.Add("System");
		}
		
		#region Parsing
		INode Parse(SupportedLanguage sourceLanguage, string sourceCode, out string error)
		{
			project = new DefaultProjectContent();
			project.ReferencedContents.AddRange(ReferencedContents);
			if (sourceLanguage == SupportedLanguage.VBNet) {
				project.DefaultImports = new DefaultUsing(project);
				project.DefaultImports.Usings.AddRange(DefaultImportsToAdd);
			}
			SnippetParser parser = new SnippetParser(sourceLanguage);
			INode result = parser.Parse(sourceCode);
			error = parser.Errors.ErrorOutput;
			specials = parser.Specials;
			if (parser.Errors.Count != 0)
				return null;
			
			wasExpression = parser.SnippetType == SnippetType.Expression;
			if (wasExpression) {
				// Special case 'Expression': expressions may be replaced with other statements in the AST by the ConvertVisitor,
				// but we need to return a 'stable' node so that the correct transformed AST is returned.
				// Thus, we wrap any expressions into a statement block.
				result = MakeBlockFromExpression((Expression)result);
			}
			
			// now create a dummy compilation unit around the snippet result
			switch (parser.SnippetType) {
				case SnippetType.CompilationUnit:
					compilationUnit = (CompilationUnit)result;
					break;
				case SnippetType.Expression:
				case SnippetType.Statements:
					compilationUnit = MakeCompilationUnitFromTypeMembers(
						MakeMethodFromBlock(
							(BlockStatement)result
						));
					break;
				case SnippetType.TypeMembers:
					compilationUnit = MakeCompilationUnitFromTypeMembers(result.Children);
					break;
				default:
					throw new NotSupportedException("Unknown snippet type: " + parser.SnippetType);
			}
			
			// convert NRefactory CU in DOM CU
			NRefactoryASTConvertVisitor visitor = new NRefactoryASTConvertVisitor(project);
			visitor.VisitCompilationUnit(compilationUnit, null);
			visitor.Cu.FileName = sourceLanguage == SupportedLanguage.CSharp ? "a.cs" : "a.vb";
			
			// and register the compilation unit in the DOM
			foreach (IClass c in visitor.Cu.Classes) {
				project.AddClassToNamespaceList(c);
			}
			parseInfo = new ParseInformation();
			parseInfo.SetCompilationUnit(visitor.Cu);
			
			return result;
		}
		
		/// <summary>
		/// Unpacks the expression from a statement block; if it was wrapped earlier.
		/// </summary>
		INode UnpackExpression(INode node)
		{
			if (wasExpression) {
				BlockStatement block = node as BlockStatement;
				if (block != null && block.Children.Count == 1) {
					ExpressionStatement es = block.Children[0] as ExpressionStatement;
					if (es != null)
						return es.Expression;
				}
			}
			return node;
		}
		
		BlockStatement MakeBlockFromExpression(Expression expr)
		{
			var blockStatement = new BlockStatement();
			blockStatement.Children = new List<INode>().add(new ExpressionStatement(expr));
			blockStatement.StartLocation = expr.StartLocation;
			blockStatement.EndLocation = expr.EndLocation;
			return blockStatement;
		}
		
		INode[] MakeMethodFromBlock(BlockStatement block)
		{
			var methodDeclaration = new MethodDeclaration();
			methodDeclaration.EndLocation = block.EndLocation;
			methodDeclaration.StartLocation = block.StartLocation;
			methodDeclaration.Body = block;
			methodDeclaration.Name = "DummyMethodForConversion";
			return new INode[] { methodDeclaration };
		}
		
		CompilationUnit MakeCompilationUnitFromTypeMembers(IList<INode> members)
		{
			TypeDeclaration type = new TypeDeclaration(Modifiers.None, null);
			type.EndLocation = members[members.Count - 1].EndLocation;
			type.StartLocation = members[0].StartLocation;
			type.Name = "DummyTypeForConversion";
			type.Children.AddRange(members);
			
			var compilationUnit = new CompilationUnit();
			compilationUnit.Children = new List<INode>().add(type);
			return compilationUnit;			
		}
		#endregion
		
		public string CSharpToVB(string input, out string errors)
		{
			INode node = Parse(SupportedLanguage.CSharp, input, out errors);
			if (node == null)
				return null;
			// apply conversion logic:
            var visitor = new CSharpToVBNetConvertVisitor(project, parseInfo);
            visitor.DefaultImportsToRemove = DefaultImportsToRemove;
            compilationUnit.AcceptVisitor(visitor,null);
			PreprocessingDirective.CSharpToVB(specials);
			return CreateCode(UnpackExpression(node), new VBNetOutputVisitor());
		}
		
		public string VBToCSharp(string input, out string errors)
		{
			INode node = Parse(SupportedLanguage.VBNet, input, out errors);
			if (node == null)
				return null;
			// apply conversion logic:
			compilationUnit.AcceptVisitor(
				new VBNetToCSharpConvertVisitor(project, parseInfo),
				null);
			PreprocessingDirective.VBToCSharp(specials);
			return CreateCode(UnpackExpression(node), new CSharpOutputVisitor());
		}
		
		string CreateCode(INode node, IOutputAstVisitor outputVisitor)
		{
			using (SpecialNodesInserter.Install(specials, outputVisitor)) {
				node.AcceptVisitor(outputVisitor, null);
			}
			return outputVisitor.Text;
		}
	}
}
