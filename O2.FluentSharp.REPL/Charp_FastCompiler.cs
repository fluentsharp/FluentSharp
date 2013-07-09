using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using FluentSharp.CSharpAST.Utils;
using FluentSharp.CoreLib.API;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory;
using FluentSharp.CoreLib;
using FluentSharp.CSharpAST;
using System.Reflection;

namespace FluentSharp.REPL.Utils
{
    public class CSharp_FastCompiler
    {
        public int forceAstBuildDelay = 0;
        public string SourceCode { get; set; }						// I think there is a small race condition with the use of this variable
        public string SourceCodeFile { get; set; }
        //public string OriginalCodeSnippet { get; set; }	
        public bool             CreatedFromSnipptet             { get; set; }
        public bool             ResolveInvocationParametersType { get; set; }
        public bool             UseCachedAssemblyIfAvailable { get; set; }
        public List<string>     ReferencedAssemblies            { get; set; }
        public Dictionary<string, object> InvocationParameters { get; set; }
        public CompilationUnit  CompilationUnit { get; set; }
        public List<string>     ExtraSourceCodeFilesToCompile { get; set; }
        public AstDetails       AstDetails { get; set; }
        public string			AstErrors { get; set; }
        public bool				generateDebugSymbols	{ get; set; }
        
        public string			CompilationErrors		{ get; set; }      
  		public Assembly			CompiledAssembly		{ get; set; }
        public CompilerResults	CompilerResults			{ get; set; }
        public bool				ExecuteInStaThread		{ get; set; }
        public bool				ExecuteInMtaThread		{ get; set; }
        public bool				WorkOffline				{ get; set; }
        public string           CompilationVersion      { get; set; }

        public Action onAstFail { get; set; }
        public Action onAstOK { get; set; }
        public Action onCompileFail { get; set; }
        public Action onCompileOK { get; set; }
        public Action beforeSnippetAst { get; set; }
        public Action beforeCompile { get; set; }

        public string default_MethodName {get; set;}
        public string default_TypeName { get; set; }

		public bool   DebugMode {get ; set;}
		
		Stack<string> _createAstStack = new Stack<string>();
        Stack<string> _compileStack = new Stack<string>();		
		bool          _creatingAst;
		bool          _compiling;
                

        public System.Threading.ManualResetEvent FinishedCompilingCode { get;set;}

        static CSharp_FastCompiler()
        {
            setDefaultUsingStatements();
            setDefaultReferencedAssemblies();
        }
        public CSharp_FastCompiler()
        {        
        	DebugMode = false;				// set to true to see details about each AstCreation and Compilation stage
            //DebugMode = true;	
            ResolveInvocationParametersType = true;
            UseCachedAssemblyIfAvailable = true;
        	InvocationParameters = new Dictionary<string, object>();
            ExtraSourceCodeFilesToCompile = new List<String>();
            ReferencedAssemblies = CompileEngine.DefaultReferencedAssemblies;
            default_MethodName = "dynamicMethod";
            default_TypeName = "DynamicType";            
            generateDebugSymbols = false;
            //OriginalCodeSnippet = "";
            SourceCode = "";
            FinishedCompilingCode = new System.Threading.ManualResetEvent(true);
            CompilationVersion = (Environment.Version.Major.eq(4)) ? "v4.0" : "v3.5";
            // defaults

        }
        
        public static void setDefaultUsingStatements()
        {
            CompileEngine.DefaultUsingStatements
                            .add_OnlyNewItems("O2.External.SharpDevelop.ExtensionMethods",
                                              "O2.External.SharpDevelop.Ascx",
                                              "O2.Views.ASCX.classes.MainGUI",
                                              "O2.Views.ASCX.ExtensionMethods",
                                              "O2.XRules.Database.APIs",
                                              "O2.XRules.Database.Utils",
											  "O2.FluentSharp");                                     
        }
        public static void setDefaultReferencedAssemblies()
        {
            CompileEngine.DefaultReferencedAssemblies
                            .add_OnlyNewItems("FluentSharp.BCL.dll",                                                                
                                              "FluentSharp.REPL.exe",
											  "O2_Platform_External_SharpDevelop.dll",
                                              "WeifenLuo.WinFormsUI.Docking.dll");          // required or some scripts that don't need this will still need it to compile (due to FluentSharp.REPL use of it)
                                              
        }                		
		public Dictionary<string,object> getDefaultInvocationParameters()
		{
			return new Dictionary<string, object>();
		}              
        public void compileSnippet(string codeSnippet)
        {
            try
            {
                if (codeSnippet.valid())
                {
			//		if (getCachedAssemblyForCode_and_RaiseEvents(codeSnippet))
			//			return;
					
                    FinishedCompilingCode.Reset();
                    _createAstStack = new Stack<string>();
                    //      createAstStack.Clear();
                    if (_createAstStack.Count == 0)
                        _creatingAst = false;
                    _createAstStack.Push(codeSnippet);
                    compileSnippet();
                }
            }
            catch (Exception ex)
            {
                ex.log("in compileSnippet");
            }
        }
        public bool removeCachedAssemblyForCode(string codeSnippet)
        {
            if (codeSnippet.notValid())
            {
                "in removeCachedAssemblyforCode, there was no codeSnippet passed".error();
                return false;
            }
            var filesMd5 = codeSnippet.md5Hash();
            return CompileEngine.removeCachedAssemblyForCode_MD5(filesMd5);
        }
		public bool getCachedAssemblyForCode_and_RaiseEvents(string codeSnippet)
		{
            if (codeSnippet.notValid())
                return false;
			var filesMd5 = codeSnippet.md5Hash();
			var cachedCompilation = CompileEngine.getCachedCompiledAssembly_MD5(filesMd5);
			if (cachedCompilation.notNull())
			{
				this.CompiledAssembly = cachedCompilation;

				if (CompileEngine.loadReferencedAssembliesIntoMemory(this.CompiledAssembly))
				{
					this.onCompileOK.invoke();				
					FinishedCompilingCode.Set();
					return true;
				}
			}
			return false;
		}
        public void compileSnippet()
        {
            O2Thread.mtaThread(
                () =>
                {
                    if (_creatingAst == false && _createAstStack.Count > 0)
                    {
                        _creatingAst = true;
                        var codeSnippet = _createAstStack.Pop();
                        this.sleep(forceAstBuildDelay, DebugMode);            // wait a bit to allow more entries to be cleared from the stack
                        if (_createAstStack.Count > 0)
                            codeSnippet = _createAstStack.Pop();

                        _createAstStack.Clear();

                        InvocationParameters = getDefaultInvocationParameters();
                        beforeSnippetAst.invoke();
                        DebugMode.ifInfo("Compiling Source Snippet (Size: {0})", codeSnippet.size());
                        var sourceCode = createCSharpCodeWith_Class_Method_WithMethodText(codeSnippet);
                        if (UseCachedAssemblyIfAvailable && getCachedAssemblyForCode_and_RaiseEvents(sourceCode))   // see if we have already compiled this snippet before
                            return;
                        if (sourceCode != null)
                            compileSourceCode(sourceCode, CreatedFromSnipptet);
                        else
                            FinishedCompilingCode.Set();
                        _creatingAst = false;
                        //compileSnippet();                 // this was there to try to see if the current in the editor was compiled (this should be detected in a different way)
                    }
                });
        }
        private void compileSourceCode(string sourceCode, bool createdFromSnipptet)
        {
            CreatedFromSnipptet = createdFromSnipptet;
            _compileStack.Push(sourceCode);
            compileSourceCode();
        }
		public void compileSourceCode(string sourceCode)
        {
            if (sourceCode.valid().isFalse())
            {
                "in CSharp_FastCompiler,compileSourceCode, provided sourceCode code was empty".error();
                if (onCompileFail !=null)
                    onCompileFail();
            }
            else
            {
				if (getCachedAssemblyForCode_and_RaiseEvents(sourceCode))
					return;
                // we need to do make sure we include any extra references included in the code
                var astCSharp = new Ast_CSharp(sourceCode);
                mapCodeO2References(astCSharp);
                compileSourceCode(sourceCode, false);
            }
       	}       	
       	public void compileSourceCode()
       	{
       	    O2Thread.mtaThread(() => compileSourceCode_Thread() );
        }

        //so that we can do EnC
        public void compileSourceCode_Thread()
        {
            try
            {
                if (_compiling == false && _compileStack.Count > 0)
                {
                    _compiling = true;
                    CompiledAssembly = null;
                    FinishedCompilingCode.Reset();
                    compileExtraSourceCodeReferencesAndUpdateReferencedAssemblies();
                    //this.sleep(forceAstBuildDelay, DebugMode);            // wait a bit to allow more entries to be cleared from the stack
                    var sourceCode = _compileStack.Pop();
                    _compileStack.Clear();
                    // remove all previous compile requests (since their source code is now out of date
                                   			                		
                    Environment.CurrentDirectory = PublicDI.config.CurrentExecutableDirectory;
                    
                    beforeCompile.invoke();
                    DebugMode.ifInfo("Compiling Source Code (Size: {0})", sourceCode.size());
                    SourceCode = sourceCode;
                    
                    if (sourceCode.lines().starting("//CLR_3.5").notEmpty())        // allow setting compilation into 2.0 CLR
                    {
                        CompilationVersion= "v3.5";
                    }

                    var providerOptions = new Dictionary<string, string>().add("CompilerVersion", CompilationVersion);

                    var csharpCodeProvider = new Microsoft.CSharp.CSharpCodeProvider(providerOptions);
					var compilerParams = new CompilerParameters();
					compilerParams.OutputAssembly = "_o2_Script.dll".tempFile();
					compilerParams.IncludeDebugInformation = generateDebugSymbols;
					compilerParams.GenerateInMemory = !generateDebugSymbols;

                    foreach (var referencedAssembly in ReferencedAssemblies)
                        compilerParams.ReferencedAssemblies.Add(referencedAssembly);

                    CompilerResults = (generateDebugSymbols) 
                                            ? csharpCodeProvider.CompileAssemblyFromFile(compilerParams, sourceCode.saveWithExtension(".cs"))
                                            : csharpCodeProvider.CompileAssemblyFromSource(compilerParams, sourceCode);

                    if (CompilerResults.Errors.Count > 0 || CompilerResults.CompiledAssembly == null)
                    {
                        CompilationErrors = "";
                        foreach (CompilerError error in CompilerResults.Errors)
                        {
                            //CompilationErrors.Add(CompilationErrors.line(error.ToString());
                            var errorMessage = String.Format("{0}::{1}::{2}::{3}::{4}", error.Line,
                                                                     error.Column, error.ErrorNumber,
                                                                     error.ErrorText, error.FileName);
                            CompilationErrors = CompilationErrors.line(errorMessage);
                            "[CSharp_FastCompiler] Compilation Error: {0}".error(errorMessage);
                        }
                        DebugMode.ifError("Compilation failed");
                        onCompileFail.invoke();
                    }
                    else
                    {
                        CompiledAssembly = CompilerResults.CompiledAssembly;
                        if (CompiledAssembly.Location.fileExists())
                            CompileEngine.setCachedCompiledAssembly_toMD5(sourceCode, CompiledAssembly);
                        DebugMode.ifDebug("Compilation was OK");
						onCompileOK.invoke();
                    }
                    _compiling = false;
                    FinishedCompilingCode.Set();
                    compileSourceCode();
                }
            }
            catch (Exception ex)
            {
                ex.log("in compileSourceCode");
                _compiling = false;
                FinishedCompilingCode.Set();
            }
        }
        // we need to use CompileEngine (which is slower but supports multiple file compilation 
        public void compileExtraSourceCodeReferencesAndUpdateReferencedAssemblies()
        {            
            if (ExtraSourceCodeFilesToCompile.size() > 0)
            {
                "[CSharp Compiler] Compiling provided {0} external source code references".info(ExtraSourceCodeFilesToCompile.size());
                var assembly = new CompileEngine(UseCachedAssemblyIfAvailable).compileSourceFiles(ExtraSourceCodeFilesToCompile);                
                if (assembly != null)
                {
                    
                    ReferencedAssemblies.Add(assembly.Location);
                    CompileEngine.setCachedCompiledAssembly(ExtraSourceCodeFilesToCompile, assembly);
                    generateDebugSymbols = true;                // if there are extra assemblies we can't generate the assembly in memory                    
                }
            }
        }
        /*public string getAstErrors(string sourceCode)
        {
            return new Ast_CSharp(sourceCode).Errors;
        }*/
        public string createCSharpCodeWith_Class_Method_WithMethodText(string code)
        {                        
            //var parsedCode = TextToCodeMappings.tryToFixRawCode(code, tryToCreateCSharpCodeWith_Class_Method_WithMethodText);            

            var parsedCode = tryToCreateCSharpCodeWith_Class_Method_WithMethodText(code);
        
            if (parsedCode == null)
            {
                DebugMode.ifError("Ast parsing Failed");
                onAstFail.invoke();
            }
            return parsedCode;
        }
        //this code needs to be completely rewritten
        public string tryToCreateCSharpCodeWith_Class_Method_WithMethodText(string code)
        {
            if (code.empty())
                return null;
			code = code.line();	// make sure there is an empty line at the end            
                      
            try
            {
                //handle special incudes in source code
                var lines = code.fixCRLF().lines();
                foreach (var originalLine in lines)
                {
                    string line = originalLine;
                    originalLine.starts("//O2Include:", (includeText) => 
                        {
                            var file = includeText;
                            var baseFile = SourceCodeFile ?? PublicDI.CurrentScript;
                            var parentFolder = baseFile.parentFolder();
                            if (parentFolder.notValid())
                                "[CSharpFastCompiled] in O2Include mapping, could not get parent folder of current script".error();
                            var resolvedFile = CompileEngine.findFileOnLocalScriptFolder(file,parentFolder);
                            if (resolvedFile.fileExists())
                            {
                                var fileContents = resolvedFile.contents();
                                code = code.Replace(line, line.line().add(fileContents).line());
                            }
                            else
                                "[CSharpFastCompiled] in O2Include mapping, could not a mapping for: {0}".error(includeText);
                        });
                }

                var snippetParser = new SnippetParser(SupportedLanguage.CSharp);
                
                var parsedCode = snippetParser.Parse(code);
				AstErrors = snippetParser.errors();
                CompilationUnit = new CompilationUnit();

                if (parsedCode is BlockStatement || parsedCode is CompilationUnit)
                {
                    Ast_CSharp astCSharp;
                    if (parsedCode is BlockStatement)
                    {
                        // map parsedCode into a new type and method 
                        var blockStatement = (BlockStatement)parsedCode;
                        CompilationUnit.add_Type(default_TypeName)
                            .add_Method(default_MethodName, InvocationParameters, this.ResolveInvocationParametersType, blockStatement);
                        
                        // remove comments from parsed code
                        astCSharp = new Ast_CSharp(CompilationUnit, snippetParser.Specials);
                        
                        // add references included in the original source code file
                        mapCodeO2References(astCSharp);

                        astCSharp.mapAstDetails();

                        astCSharp.ExtraSpecials.Clear();
                        var method = CompilationUnit.method(default_MethodName);
                        var returntype = method.returnType();
                        var type = CompilationUnit.type(default_TypeName);
                        
                        type.Children.Clear();
                        var tempBlockStatement = new BlockStatement();
                        tempBlockStatement.add_Variable("a", 0);                        
                        method.Body = tempBlockStatement;                      
                        var newMethod = type.add_Method(default_MethodName, InvocationParameters, this.ResolveInvocationParametersType, tempBlockStatement);
                        newMethod.TypeReference = returntype;
                        astCSharp.mapAstDetails();
                        var csharpCode = astCSharp.AstDetails.CSharpCode
                                                  .replace("Int32 a = 0;", code);

                        AstDetails = new Ast_CSharp(csharpCode).AstDetails;
                        CreatedFromSnipptet = true;
                        DebugMode.ifDebug("Ast parsing was OK");
                        SourceCode = csharpCode;
                        onAstOK.invoke();
                        
                        return csharpCode;
                    }
                    
                    
                    CompilationUnit = (CompilationUnit)parsedCode;
                    if (CompilationUnit.Children.Count == 0)
                        return null;

                    astCSharp = new Ast_CSharp(CompilationUnit, snippetParser.Specials);
                    // add the comments from the original code                        

                    mapCodeO2References(astCSharp);
                    CreatedFromSnipptet = false;                    
                    
                    // create sourceCode using Ast_CSharp & AstDetails		
                    if(CompilationUnit.Children.Count > 0)
                    {                        
                        // reset the astCSharp.AstDetails object        	
                        astCSharp.mapAstDetails();
                        // add the comments from the original code
                        astCSharp.ExtraSpecials.AddRange(snippetParser.Specials);	                 

	                    SourceCode = astCSharp.AstDetails.CSharpCode;

                        //once we have the created SourceCode we need to create a new AST with it
                        var tempAstDetails = new Ast_CSharp(SourceCode).AstDetails;
                        //note we should try to add back the specials here (so that comments make it to the generated code
                        AstDetails = tempAstDetails;
	                    DebugMode.ifDebug("Ast parsing was OK");
	                    onAstOK.invoke();
	                    return SourceCode;
                    }
                }            
            }
            catch (Exception ex)
            {                            	
				DebugMode.ifError("in createCSharpCodeWith_Class_Method_WithMethodText:{0}", ex.Message);                
            }      			
			return null;                
        }        
        public void mapCodeO2References(Ast_CSharp astCSharp)
        {            
            bool onlyAddReferencedAssemblies = false;			
            ExtraSourceCodeFilesToCompile = new List<string>();                                
        	var compilationUnit = astCSharp.CompilationUnit;
            ReferencedAssemblies = new List<string>();
            var filesToDownload = new List<string>();

        	var currentUsingDeclarations = new List<string>();
        	foreach(var usingDeclaration in astCSharp.AstDetails.UsingDeclarations)
        		currentUsingDeclarations.Add(usingDeclaration.Text);        	
        	
            foreach (var comment in astCSharp.AstDetails.Comments)
            {
                comment.Text.eq    ("O2Tag_OnlyAddReferencedAssemblies", () => onlyAddReferencedAssemblies = true);				
                comment.Text.starts("using ", false, value => astCSharp.CompilationUnit.add_Using(value));
                comment.Text.starts(new [] {"ref ", "O2Ref:"}, false,  value => ReferencedAssemblies.Add(value));
                comment.Text.starts(new[]  { "Download:","download:", "O2Download:" }, false, value => filesToDownload.Add(value));
                comment.Text.starts(new[]  { "include", "file ", "O2File:" }, false, value => ExtraSourceCodeFilesToCompile.Add(value));
                comment.Text.starts(new[]  { "dir ", "O2Dir:" }, false, value => ExtraSourceCodeFilesToCompile.AddRange(value.files("*.cs",true))); 
               
                comment.Text.starts(new[]  { "O2:debugSymbols",
                                            "generateDebugSymbols", 
                                            "debugSymbols"}, true, (value) => generateDebugSymbols = true);
                comment.Text.starts(new[]  {"SetInvocationParametersToDynamic"}, (value) => ResolveInvocationParametersType = false);
                comment.Text.starts(new[]  { "DontSetInvocationParametersToDynamic" }, (value) => ResolveInvocationParametersType = true);                    
                comment.Text.eq("StaThread", () => { ExecuteInStaThread = true; });
                comment.Text.eq("MtaThread", () => { ExecuteInMtaThread = true; });
                comment.Text.eq("WorkOffline", () => { WorkOffline = true; });                
            }

            //resolve location of ExtraSourceCodeFilesToCompile
            resolveFileLocationsOfExtraSourceCodeFilesToCompile();

            CompileEngine.handleReferencedAssembliesInstallRequirements(astCSharp.AstDetails.CSharpCode);

            //use the same technique to download files that are needed for this script (for example *.zip files or other unmanaged/support files)
            CompileEngine.tryToResolveReferencesForCompilation(filesToDownload, WorkOffline);            

            if (onlyAddReferencedAssemblies.isFalse())
            {
                foreach (var defaultRefAssembly in CompileEngine.DefaultReferencedAssemblies)
                    if (ReferencedAssemblies.Contains(defaultRefAssembly).isFalse())
                        ReferencedAssemblies.add(defaultRefAssembly);
                foreach (var usingStatement in CompileEngine.DefaultUsingStatements)
                    if (false == currentUsingDeclarations.Contains(usingStatement))
                        compilationUnit.add_Using(usingStatement);
            }

			//make sure the referenced assemblies are in the current execution directory
			CompileEngine.tryToResolveReferencesForCompilation(ReferencedAssemblies, WorkOffline);            

        }
        public void resolveFileLocationsOfExtraSourceCodeFilesToCompile()
        {
            if (ExtraSourceCodeFilesToCompile.size() > 0)
            {                
                // try to resolve local file references
                try
                {                    
                    if (this.SourceCodeFile.isNull())           // in case this is not set
                        SourceCodeFile = PublicDI.CurrentScript;
                    for (int i = 0; i < ExtraSourceCodeFilesToCompile.size(); i++)
                    {
                        var fileToResolve = ExtraSourceCodeFilesToCompile[i].trim();

                        var resolved = false;
                        // try using SourceCodeFile.directoryName()
                        if (fileToResolve.fileExists().isFalse())
                            if (SourceCodeFile.valid() && SourceCodeFile.isFile())
                            {
                                var resolvedFile = SourceCodeFile.directoryName().pathCombine(fileToResolve);
                                if (resolvedFile.fileExists())
                                {
                                    ExtraSourceCodeFilesToCompile[i] = resolvedFile;
                                    resolved = true;
                                }
                            }    
                        // try using the localscripts folders                                            
                        var localMapping = fileToResolve.local();
                        if (localMapping.valid())
                        {
                            ExtraSourceCodeFilesToCompile[i] = localMapping;
                            resolved = true;
                        }
                        
                        if (resolved.isFalse() && fileToResolve.fileExists().isFalse())
                            ExtraSourceCodeFilesToCompile[i] = ExtraSourceCodeFilesToCompile[i].fullPath();                        
                    }
                }
                catch (Exception ex)
                {
                    ex.log("in compileExtraSourceCodeReferencesAndUpdateReferencedAssemblies while resolving ExtraSourceCodeFilesToCompile");
                }
            }
        }
        public object executeFirstMethod()
        {
			if (CompiledAssembly.notNull())
			{
				var parametersValues = InvocationParameters.valuesArray();
				return CompiledAssembly.executeFirstMethod(ExecuteInStaThread, ExecuteInMtaThread, parametersValues);
			}
			return null;
        }                       
        public Location getGeneratedSourceCodeMethodLineOffset()
        {
            if (CreatedFromSnipptet && SourceCode.valid())                
                    if (AstDetails.Methods.size() > 0)
                    {
                        return AstDetails.Methods[0].OriginalObject.firstLineOfCode();
                    }
            return new Location(0, 0) ;
        }
        public void waitForCompilationComplete()
        {            
            if (FinishedCompilingCode.WaitOne(20 * 1000).isFalse())
                "in CSharp_FastCompiler, the compilation lasted more than 20 seconds".error();
            FinishedCompilingCode.WaitOne();
        }
    }
}
