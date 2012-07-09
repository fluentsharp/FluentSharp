using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.SharpDevelop.Dom.NRefactoryResolver;
using ICSharpCode.SharpDevelop.Dom;
using ICSharpCode.NRefactory.Ast;
using O2.DotNetWrappers.ExtensionMethods;

using O2.API.AST.ExtensionMethods;
using O2.API.AST.ExtensionMethods.CSharp;

namespace O2.API.AST.CSharp
{
    public class O2AstResolver
    {
        public NRefactoryResolver resolver = new NRefactoryResolver(LanguageProperties.CSharp);
        public ParseInformation parseInformation = new ParseInformation();
        public DefaultProjectContent myProjectContent = new DefaultProjectContent();
        public ProjectContentRegistry pcRegistry = new ProjectContentRegistry();
        public Dictionary<string, CompilationUnit> parsedCompilationUnits = new Dictionary<string, CompilationUnit>();

        public O2AstResolver()
        {            
        }

        public void addReference(string assemblyToLoad)
        {
            if (myProjectContent.NamespaceExists("System").isFalse())
                myProjectContent.AddReferencedContent(pcRegistry.Mscorlib);
            if (assemblyToLoad.valid().isFalse())
                return;
            IProjectContent referenceProjectContent = pcRegistry.GetProjectContentForReference(assemblyToLoad, assemblyToLoad);
            if (referenceProjectContent == null)
                "referenceProjectContent was null".error();
            else
            {
                myProjectContent.AddReferencedContent(referenceProjectContent);
                if (referenceProjectContent is ReflectionProjectContent)
                    (referenceProjectContent as ReflectionProjectContent).InitializeReferences();
                else
                    "something when wrong in DefaultProjectContent.add_Reference".error();
            }

            //var supportedLanguage = SupportedLanguage.CSharp;            
        }
        public void mapSourceCode(string fileName)
        {
            "mapping file: {0}".format(fileName).debug();
            var parser = fileName.csharpAst();
            if (parsedCompilationUnits.ContainsKey(fileName))
                parsedCompilationUnits[fileName] = parser.CompilationUnit;
            else
                parsedCompilationUnits.Add(fileName, parser.CompilationUnit);
            var iCompilationUnit = setCurrentCompilationUnit(parser.CompilationUnit);
            myProjectContent.UpdateCompilationUnit(null, iCompilationUnit, fileName);
        }

        public ICompilationUnit setCurrentCompilationUnit(CompilationUnit compilationUnit)
        {
            if (compilationUnit == null)
                return null;
            NRefactoryASTConvertVisitor converter;
            converter = new NRefactoryASTConvertVisitor(myProjectContent);
            compilationUnit.AcceptVisitor(converter, null);
            var newCompilationUnit = converter.Cu;
            parseInformation.SetCompilationUnit(newCompilationUnit);
            return newCompilationUnit;            
        }


        public IMethod getMappedIMethod(MethodDeclaration methodDeclaration)
        {
            if (!methodDeclaration.Body.IsNull &&
                resolver.Initialize(
                parseInformation,
                methodDeclaration.Body.StartLocation.Y,
                methodDeclaration.Body.StartLocation.X))
            {
                resolver.RunLookupTableVisitor(methodDeclaration);
            }
            return resolver.CallingMember as ICSharpCode.SharpDevelop.Dom.IMethod;
        }

        public CompilationUnit getCompilationUnit(string file)
        {
            if (parsedCompilationUnits.ContainsKey(file))
                return parsedCompilationUnits[file];
            mapSourceCode(file);
            return parsedCompilationUnits[file];
        }

      
    }
}
