using System.Reflection;
using O2.DotNetWrappers.ExtensionMethods;

using O2.External.SharpDevelop.AST;
using O2.DotNetWrappers.Windows;
using O2.DotNetWrappers.DotNet;
using System;
using O2.DotNetWrappers.H2Scripts;
using O2.Kernel;
using System.Windows.Forms;
using System.Collections.Generic;

namespace O2.External.SharpDevelop.ExtensionMethods
{
    public static class FastCompiler_ExtensionMethods
    {
        public static Assembly compileScriptFile(this string scriptToCompile)
        {
            if (scriptToCompile.valid())
            {
                if (scriptToCompile.fileExists().isFalse())
                    scriptToCompile = scriptToCompile.local();
                if (scriptToCompile.fileExists())
                    return (scriptToCompile.extension(".h2"))
                                ? scriptToCompile.compile_H2Script()
                                : scriptToCompile.compile();
            }
            "[compileScriptFile] could not find file to compile: {0}".error(scriptToCompile);
            return null;
        }

        public static Assembly compile(this string pathToFileToCompile)
        {
            return pathToFileToCompile.compile(false);
        }
        public static Assembly compile(this string pathToFileToCompile, bool generateDebugSymbols)
        {
            PublicDI.CurrentScript = pathToFileToCompile;
            var csharpCompiler = new CSharp_FastCompiler();
            csharpCompiler.generateDebugSymbols= generateDebugSymbols;
			var compileProcess = new System.Threading.AutoResetEvent(false);            
            csharpCompiler.onCompileFail = () => compileProcess.Set();
            csharpCompiler.onCompileOK = () => compileProcess.Set();

			O2Thread.mtaThread(()=> {            
										csharpCompiler.compileSourceCode(pathToFileToCompile.contents());
									});
            compileProcess.WaitOne();
            return csharpCompiler.assembly();
        }
        public static Assembly compile_CodeSnippet(this string codeSnipptet)
        {
            return codeSnipptet.compile_CodeSnippet(false);
        }
        public static Assembly compile_CodeSnippet(this string codeSnipptet, bool generateDebugSymbols)
        {   
            //Note we can't use the precompiled engines here since there is an issue of the resolution of this code dependencies

            var csharpCompiler = new CSharp_FastCompiler();
            csharpCompiler.DebugMode = true;
            csharpCompiler.generateDebugSymbols= generateDebugSymbols;
            var compileProcess = new System.Threading.AutoResetEvent(false);
            //csharpCompiler.compileSourceCode(pathToFileToCompile.contents());            
            csharpCompiler.onAstFail = () => compileProcess.Set();
            //csharpCompiler.onAstOK = () => compileProcess.Set();
            csharpCompiler.onCompileFail = () => compileProcess.Set();
            csharpCompiler.onCompileOK = () => compileProcess.Set();
            csharpCompiler.compileSnippet(codeSnipptet);
            compileProcess.WaitOne();
            var assembly = csharpCompiler.assembly();
            return assembly;
        }

        public static Assembly compile_H2Script(this string h2Script)
        {
            try
            {                
                var sourceCode = "";
                if (h2Script.extension(".h2"))
                {
                    PublicDI.CurrentScript = h2Script;
                    if (h2Script.fileExists().isFalse())
                        h2Script = h2Script.local();
                    sourceCode = H2.load(h2Script).SourceCode;
                }
                if (sourceCode.valid())
                    return sourceCode.compile_CodeSnippet();
            }
            catch (Exception ex)
            {
                ex.log("in string compile_H2Script");
            }
            return null;
        }
        public static Assembly assembly(this CSharp_FastCompiler csharpCompiler)
        {
			if (csharpCompiler != null)
			{
				if (csharpCompiler.CompiledAssembly.notNull())
					return csharpCompiler.CompiledAssembly;
				if (csharpCompiler.CompilerResults != null)
					if (csharpCompiler.CompilerResults.Errors.HasErrors == false)
					{
						if (csharpCompiler.CompilerResults.CompiledAssembly != null)
							return csharpCompiler.CompilerResults.CompiledAssembly;
					}
					else
						"CompilationErrors:".line().add(csharpCompiler.CompilationErrors).error();
			}
            return null;
        }
        public static Assembly compile(this string pathToFileToCompile, string targetAssembly)
        {
            var assembly = pathToFileToCompile.compile(true);
            Files.Copy(assembly.Location, targetAssembly);
            return assembly;
        }
        /*public static Assembly compile(this string pathToFileToCompile, bool compileToFileAndWithDebugSymbols)
        {
            string generateDebugSymbolsTag = @"//debugSymbols".line();
            if (pathToFileToCompile.fileContains(generateDebugSymbolsTag).isFalse())
                pathToFileToCompile.fileInsertAt(0, generateDebugSymbolsTag);
            return pathToFileToCompile.compile();

        } */       
        public static object executeFirstMethod(this string pathToFileToCompileAndExecute)
        {
            try
            {
                return pathToFileToCompileAndExecute.executeFirstMethod(new object[] { });
            }
            catch (Exception ex)
            {
                ex.log("in executeFirstMethod");
                return null;
            }
        }
        public static object executeFirstMethod(this string pathToFileToCompileAndExecute, object[] parameters)
        {
            PublicDI.CurrentScript = pathToFileToCompileAndExecute;
            if (pathToFileToCompileAndExecute.fileExists().isFalse())
            { 
                // if we were not provided a complete path, try to find it on the local o2 script folder                
                pathToFileToCompileAndExecute = pathToFileToCompileAndExecute.local();                
            }
            if (pathToFileToCompileAndExecute.extension(".h2"))
                return executeH2Script(pathToFileToCompileAndExecute);
            else
            {
                var assembly = pathToFileToCompileAndExecute.compile(true /* generatedDebug symbols */);
                return assembly.executeFirstMethod(parameters);
            }
        }
        public static object executeFirstMethod(this Assembly assembly)
        {
            return assembly.executeFirstMethod(false, false, new object[] {});
        }
        public static object executeFirstMethod(this Assembly assembly ,  object[] parameters)
        {
            return assembly.executeFirstMethod(false, false, parameters);
        }
        public static object executeFirstMethod(this Assembly assembly ,  bool executeInStaThread, bool executeInMtaThread, object[] parameters)
        {            
            if (assembly != null)
            {
                var methods = assembly.methods();
                foreach (var method in methods)
                    if (method.IsSpecialName == false && method.IsPublic)  // we need to do this since Properties get_ and set_ also look like methods
                    //if (methods.Count >0)        		
                    //{
                    {
                        if (executeInStaThread)
                            return O2Thread.staThread(() => method.executeMethod(parameters));
                        if (executeInMtaThread)
                            return O2Thread.mtaThread(() => method.executeMethod(parameters));

                        return method.executeMethod(parameters);
                    }
            }
            return null;
        }
        public static object executeMethod(this MethodInfo method, params object[] parameters)
        {
            try
            {
                if (method.parameters().size() == parameters.size())
                    return method.invoke(parameters);
                return method.invoke();
            }
            catch (Exception ex)
            {
                ex.log("in CSharp_FastCompiler.executeMethod");
                return null;
            }
        }
        public static object executeCodeSnippet(this string sourceCodeToExecute)
        {
            return sourceCodeToExecute.executeSourceCode();
        }
        public static object executeSourceCode(this string sourceCodeToExecute)
        {
            try
            {
                var assembly = sourceCodeToExecute.compile_CodeSnippet(true);
                return assembly.executeFirstMethod();
            }
            catch (Exception ex)
            {
                ex.log("in CSharp_FastCompiler.executeSourceCode");
                return null;
            }            
        }

        public static object executeH2Script(this string h2ScriptFile)
        {
            try
            {
                if (h2ScriptFile.extension(".h2").isFalse())
                    "[in executeH2Script]: file to execute must be a *.h2 file, it was:{0}".error(h2ScriptFile);
                else
                {
                    PublicDI.CurrentScript = h2ScriptFile;
                    var h2Script = H2.load(h2ScriptFile);
                    return h2Script.execute();                    
                }
            }
            catch (Exception ex)
            {
                ex.log("in CSharp_FastCompiler.executeH2Script");
            }
            return null;
        }
        public static object execute(this H2 h2Script)
        {
            try
            {                
                var assembly = h2Script.SourceCode.compile_CodeSnippet();
                return assembly.executeFirstMethod();                
            }
            catch (Exception ex)
            {
                ex.log("in CSharp_FastCompiler.executeH2Script");
            }
            return null;
        }

        //add links (which execute o2 scripts		
		public static LinkLabel add_Link(this Control control, string label, string script)
		{
			return control.add_Link(label, ()=> script.executeH2Script());
		}		
		public static LinkLabel append_Link(this Control control, string label, string script)
		{
			return control.append_Link(label, ()=> script.executeH2Script());
		}		
		public static LinkLabel append_Below_Link(this Control control, string label, string script)
		{
			return control.append_Below_Link(label, ()=> script.executeH2Script());
		}		


        //ComboBox execution
        public static ComboBox add_ExecutionComboBox(this Control control, string labelText, int top, int left, Items scriptMappings)
		{
			return control.add_ExecutionComboBox(labelText, top, left, scriptMappings, scriptMappings.keys());
		}		
		public static ComboBox add_ExecutionComboBox(this Control control, string labelText, int top, int left, Items scriptMappings, List<string> comboBoxItems)
		{						
			return control.add_Label(labelText, top, left)
		 				  .append_Control<ComboBox>().top(top-3).dropDownList() // .width(100)		 				  
 						  .add_Items(comboBoxItems.insert("... select one...").ToArray())
 						  .executeScriptOnSelection(scriptMappings)		 							
 						  .selectFirst(); 
		}		
        public static ComboBox executeScriptOnSelection(this ComboBox comboBox, Items mappings)
		{			
			comboBox.onSelection<string>(
						(key)=>{
									if (mappings.hasKey(key))
									{
                                        comboBox.parent().focus();// do this in order to prevent a nasty user experience that happens if the user uses the up and down arrows to navigate the comboBox	
										"executing script mapped to '{0}: {1}".info(key, mappings[key]);
										var itemToExecute = mappings[key];
										if (itemToExecute.isUri())
											Processes.startProcess(itemToExecute);
											//itemToExecute.startProcess();
										else
										{
											if(itemToExecute.fileExists().isFalse() && itemToExecute.local().fileExists().isFalse())																							
												CompileEngine.clearLocalScriptFileMappings();											
											"itemToExecute: {0}".debug(itemToExecute);	
											//"itemToExecuteextension: {0}".debug(itemToExecute.extension(".o2"));
											if (itemToExecute.extension(".h2") || itemToExecute.extension(".o2"))											
												if (Control.ModifierKeys == Keys.Shift)
												{
													"Shift Key pressed, so launching in new process: {0}".info(itemToExecute);
													itemToExecute.executeH2_or_O2_in_new_Process();
													return;
												}
												
/*												else
												{
													"Shift Key was pressed, so running script in current process".debug(itemToExecute);													
													O2Thread.mtaThread(()=>itemToExecute.executeFirstMethod());
												}
											else*/
											O2Thread.mtaThread(()=>itemToExecute.executeFirstMethod());
										}                                        
									}
								});		
			return comboBox;			
		}	
    }
}
