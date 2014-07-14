using System;
using System.Collections.Generic;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.CSharpAST;
using FluentSharp.CSharpAST.Utils;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;

namespace FluentSharp.AST
{
    public static class Ast_CSharp_ExtensionMethods_Code_Generation
    {
        public static Ast_CSharp resolveFileLocationsOfExtraSourceCodeFilesToCompile(this Ast_CSharp astCSharp , CSharp_FastCompiler_CompilerOptions compilerOptions)
        {
            if (compilerOptions.ExtraSourceCodeFilesToCompile.size() > 0)
            {                
                // try to resolve local file references
                try
                {                    
                    if (compilerOptions.SourceCodeFile.isNull())           // in case this is not set  (note: this should not be done here, better to do it on the REPL code)
                        compilerOptions.SourceCodeFile = PublicDI.CurrentScript;
                    for (int i = 0; i < compilerOptions.ExtraSourceCodeFilesToCompile.size(); i++)
                    {
                        var fileToResolve = compilerOptions.ExtraSourceCodeFilesToCompile[i].trim();

                        var resolved = false;
                        // try using SourceCodeFile.directoryName()
                        if (fileToResolve.fileExists().isFalse())
                            if (compilerOptions.SourceCodeFile.valid() && compilerOptions.SourceCodeFile.isFile())
                            {
                                var resolvedFile = compilerOptions.SourceCodeFile.directoryName().pathCombine(fileToResolve);
                                if (resolvedFile.fileExists())
                                {
                                    compilerOptions.ExtraSourceCodeFilesToCompile[i] = resolvedFile;
                                    resolved = true;
                                }
                            }    
                        // try using the localscripts folders                                            
                        var localMapping = fileToResolve.local();
                        if (localMapping.valid())
                        {
                            compilerOptions.ExtraSourceCodeFilesToCompile[i] = localMapping;
                            resolved = true;
                        }
                        
                        if (resolved.isFalse() && fileToResolve.fileExists().isFalse())
                            compilerOptions.ExtraSourceCodeFilesToCompile[i] = compilerOptions.ExtraSourceCodeFilesToCompile[i].fullPath();                        
                    }
                }
                catch (Exception ex)
                {
                    ex.log("in compileExtraSourceCodeReferencesAndUpdateReferencedAssemblies while resolving ExtraSourceCodeFilesToCompile");
                }
            }
            return astCSharp;
        }

        public static Ast_CSharp mapCodeO2References(this Ast_CSharp astCSharp, CSharp_FastCompiler_CompilerOptions compilerOptions)
        {            
            bool onlyAddReferencedAssemblies = false;			
            compilerOptions.ExtraSourceCodeFilesToCompile = new List<string>();                                
            var compilationUnit = astCSharp.CompilationUnit;
            compilerOptions.ReferencedAssemblies = new List<string>();
            var filesToDownload = new List<string>();

            var currentUsingDeclarations = new List<string>();
            foreach(var usingDeclaration in astCSharp.AstDetails.UsingDeclarations)
                currentUsingDeclarations.Add(usingDeclaration.Text);        	
        	
            foreach (var comment in astCSharp.AstDetails.Comments)
            {
                comment.Text.eq    ("O2Tag_OnlyAddReferencedAssemblies", () => onlyAddReferencedAssemblies = true);				
                comment.Text.starts("using ", false, value => astCSharp.CompilationUnit.add_Using(value));
                comment.Text.starts(new [] {"ref ", "O2Ref:"}, false,  value => compilerOptions.ReferencedAssemblies.Add(value));
                comment.Text.starts(new[]  { "Download:","download:", "O2Download:" }, false, filesToDownload.Add);
                comment.Text.starts(new[]  { "include", "file ", "O2File:" }, false, value => compilerOptions.ExtraSourceCodeFilesToCompile.Add(value));
                comment.Text.starts(new[]  { "dir ", "O2Dir:" }, false, value => compilerOptions.ExtraSourceCodeFilesToCompile.AddRange(value.files("*.cs",true))); 
               
                comment.Text.starts(new[]  { "O2:debugSymbols",
                    "generateDebugSymbols", 
                    "debugSymbols"}, true, (value) => compilerOptions.generateDebugSymbols = true);
                comment.Text.starts(new[]  {"SetInvocationParametersToDynamic"}, (value) => compilerOptions.ResolveInvocationParametersType = false);
                comment.Text.starts(new[]  { "DontSetInvocationParametersToDynamic" }, (value) => compilerOptions.ResolveInvocationParametersType = true);                    
                comment.Text.eq("StaThread", () => { compilerOptions.ExecuteInStaThread = true; });
                comment.Text.eq("MtaThread", () => { compilerOptions.ExecuteInMtaThread = true; });
                comment.Text.eq("WorkOffline", () => { compilerOptions.WorkOffline = true; });                
            }

            //resolve location of ExtraSourceCodeFilesToCompile
            astCSharp.resolveFileLocationsOfExtraSourceCodeFilesToCompile(compilerOptions);

            CompileEngine.handleReferencedAssembliesInstallRequirements(astCSharp.AstDetails.CSharpCode);

            //use the same technique to download files that are needed for this script (for example *.zip files or other unmanaged/support files)
            CompileEngine.tryToResolveReferencesForCompilation(filesToDownload, compilerOptions.WorkOffline);            

            if (onlyAddReferencedAssemblies.isFalse())
            {
                foreach (var defaultRefAssembly in CompileEngine.DefaultReferencedAssemblies)
                    if (compilerOptions.ReferencedAssemblies.Contains(defaultRefAssembly).isFalse())
                        compilerOptions.ReferencedAssemblies.add(defaultRefAssembly);
                foreach (var usingStatement in CompileEngine.DefaultUsingStatements)
                    if (false == currentUsingDeclarations.Contains(usingStatement))
                        compilationUnit.add_Using(usingStatement);
            }

            //make sure the referenced assemblies are in the current execution directory
            CompileEngine.tryToResolveReferencesForCompilation(compilerOptions.ReferencedAssemblies, compilerOptions.WorkOffline);            
            return astCSharp;
        }
    

        /// <summary>
        /// From a provided C# code snippet (for example <code>2+2</code>) create a full compilable C# class file 
        /// 
        /// This uses the using statements provided by the default mappings in CSharp_FastCompiler_CompilerOptions class
        /// </summary>
        /// <param name="codeSnippet"></param>
        /// <returns></returns>
        public static string csharp_FromSnippet(this string codeSnippet)
        {
            return codeSnippet.csharp_CreateClass_FromSnippet();
        }
        public static string csharp_CreateClass_FromSnippet(this string codeSnippet)
        {
            return codeSnippet.ast_CSharp_CreateCompilableClass();
        }
        public static string ast_CSharp_CreateCompilableClass(this string codeSnippet)
        {
            var snippetParser   = new SnippetParser(SupportedLanguage.CSharp);             
            var iNode           = snippetParser.Parse(codeSnippet);
            
            return iNode.cast<BlockStatement>()
                .ast_CSharp_CreateCompilableClass(snippetParser, codeSnippet);                            
        }
        public static string ast_CSharp_CreateCompilableClass(this BlockStatement blockStatement, SnippetParser snippetParser, string codeSnippet)
        {
            if (blockStatement.isNull() || snippetParser.isNull() || codeSnippet.notValid())
                return null;
             
            var compilerOptions   = new CSharp_FastCompiler_CompilerOptions  ();
            var compilerArtifacts = new CSharp_FastCompiler_CompilerArtifacts();
            var executionOptions  = new CSharp_FastCompiler_ExecutionOptions ();
            var csharpCode        = blockStatement.ast_CSharp_CreateCompilableClass(snippetParser, codeSnippet, 
                compilerOptions, compilerArtifacts, executionOptions);
            return csharpCode;            
        }
        public static string ast_CSharp_CreateCompilableClass(this BlockStatement blockStatement, SnippetParser snippetParser, string codeSnippet,
            CSharp_FastCompiler_CompilerOptions   compilerOptions, 
            CSharp_FastCompiler_CompilerArtifacts compilerArtifacts,
            CSharp_FastCompiler_ExecutionOptions  executionOptions)
        {
            if (blockStatement.isNull() || compilerOptions.isNull())
                return null;

            var compilationUnit= compilerArtifacts.CompilationUnit = new CompilationUnit();

            compilationUnit.add_Type(compilerOptions.default_TypeName)
                .add_Method(compilerOptions.default_MethodName, executionOptions.InvocationParameters, 
                    compilerOptions.ResolveInvocationParametersType, blockStatement);
                        
            // remove comments from parsed code
            var astCSharp = compilerArtifacts.AstCSharp = new Ast_CSharp(compilerArtifacts.CompilationUnit, snippetParser.Specials);
                        
            // add references included in the original source code file
            compilerArtifacts.AstCSharp.mapCodeO2References(compilerOptions);

            astCSharp.mapAstDetails();

            astCSharp.ExtraSpecials.Clear();
            var method     = compilationUnit.method(compilerOptions.default_MethodName);
            var returntype = method.returnType();
            var type       = compilationUnit.type(compilerOptions.default_TypeName);
                        
            type.Children.Clear();
            var tempBlockStatement = new BlockStatement();
            tempBlockStatement.add_Variable("a", 0);                        
            method.Body = tempBlockStatement;                      
            var newMethod = type.add_Method(compilerOptions.default_MethodName, executionOptions.InvocationParameters, 
                compilerOptions.ResolveInvocationParametersType, tempBlockStatement);
            newMethod.TypeReference = returntype;

            
            if(blockStatement.returnStatements().size() >1)
                astCSharp.methodDeclarations().first().remove_LastReturnStatement();
            
            astCSharp.mapAstDetails();
            
            var codeToReplace = "Int32 a = 0;";
            var csharpCode = astCSharp.AstDetails.CSharpCode
                .replace("\t\t{0}".format(codeToReplace), codeToReplace)    // remove tabs
                .replace("Int32 a = 0;", codeSnippet);                      // put the actual code (this should be done via AST, but it was affectting code complete)
            return csharpCode;
        }
    }
}