﻿// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Drawing;
using System.Threading;
using System.Collections.Generic;
using System.Windows.Forms;
using O2.Kernel;
using O2.Kernel.CodeUtils;
using O2.DotNetWrappers.DotNet;
using O2.DotNetWrappers.Windows;
using FluentSharp.ExtensionMethods;
using O2.Views.ASCX.classes.MainGUI;
using O2.Views.ASCX.ExtensionMethods;
using O2.External.SharpDevelop.Ascx;
using O2.External.SharpDevelop.AST;
using O2.External.SharpDevelop.ExtensionMethods;
using O2.DotNetWrappers.H2Scripts;
using O2.Views.ASCX.DataViewers;
using O2.API.AST.ExtensionMethods.CSharp;
using O2.Views.ASCX.Forms;
using O2.FluentSharp.REPL;
using O2.Platform.BCL.O2_Views_ASCX;

namespace O2.XRules.Database.Utils
{

    public class ascx_Simple_Script_Editor_Test
    {
        public void testGui()
        {			
            //DebugMsg.debugBreak();
            //ascx_Simple_Script_Editor.startControl();
            ascx_Simple_Script_Editor.startControl_NoCodeComplete();
        }
    }
    // Comments: (24 Feb 2010): This was the 6th pass at building an O2 Command Prompt (i.e. simple script editor)	

    public sealed class ascx_Simple_Script_Editor : UserControl
    {
        //private static IO2Log log = PublicDI.log;
        public ascx_SourceCodeViewer sourceCodeViewer   { get; set; }
        public ascx_SourceCodeViewer commandsToExecute  { get; set; }
        public RichTextBox result_RichTextBox           { get; set; }
        public Panel result_Panel                       { get; set; }
        public Button executeButton                     { get; set; }
        //public TextBox currentScriptPath 				{ get; set;}
        public ToolStrip ToolStrip                      { get; set; }
        //public SplitContainer topLevelSplitContainer {get ; set;}
        public SplitContainer splitContainer_CommandBox { get; set; }
        public SplitContainer splitContainer_TopLevel   { get; set; }
        public SplitContainer splitContainer_Results    { get; set; }
        public bool AddToolStrip                { get; set; }
        public Action OnAstFail                 { get; set; }
        public Action OnAstOK                   { get; set; }
        public Action onCompileFail             { get; set; }
        public Action onCompileOK               { get; set; }
        public Action<object> onExecute         { get; set; }
        public CSharp_FastCompiler csharpCompiler { get; set; }
        public string GeneratedCode             { get; set; }
        public Dictionary<string, object> InvocationParameters { get; set; }
        //public bool 		  				ResolveInvocationParametersType { get; set; }
        public bool AutoCompileOnCodeChange     { get; set; }
        public bool AutoSaveOnCompileSuccess    { get; set; }
        public string AutoSaveDir               { get; set; }
        public int width = 300;
        public int height = 300;
        public bool ExecuteOnEnter              { get; set; }
        public bool ExecuteOnCompile            { get; set; }
        public string previousCompiledCodeText  { get; set; }
        public Thread TriggerCompilationThread  { get; set; }
        public Thread ExecutionThread           { get; set; }
        public object LastExecutionResult       { get; set; }

        public string defaultCode = "PublicDI.log.info(\"Dynamic Data: {0},{1}\", testString, testNumber);"
                               .line("return \"Dynamic Data = \" + testString + \" :: \" + testNumber;");        

        public ascx_Simple_Script_Editor() : this(true)
        {			
        }
        public ascx_Simple_Script_Editor(bool codeCompleteSupport)
        {
            AddToolStrip = true;
            Width = width;
            Height = height;
            previousCompiledCodeText = "";
            InvocationParameters = new Dictionary<string, object>(); 
            AutoCompileOnCodeChange = true; 
            AutoSaveOnCompileSuccess = true; //  false;  // for now default to true
            AutoSaveDir = PublicDI.config.AutoSavedScripts;
            setCompilerEnvironment();


            createGui();
            configureGui();

            //enabled code complete
            ExecuteOnEnter = false;
            if (codeCompleteSupport)
                enableCodeComplete_afterACoupleSeconds();
            Refresh();

        }
        public void enableCodeComplete_afterACoupleSeconds()
        {
            O2Thread.mtaThread(
                () =>
                {
                    this.sleep(500, false);
                    "Loading Code Complete data".info();
                    commandsToExecute.enableCodeComplete();
                    commandsToExecute.editor().o2CodeCompletion.UseParseCodeThread = false;
                    commandsToExecute.editor().o2CodeCompletion.addReferences(csharpCompiler.ReferencedAssemblies);
                });
        }
        private void createGui()
        {
            // create controls

            var groupBoxes = this.add_1x1("Command To Execute", "Invoke and Result", true, this.Width - 150);

            commandsToExecute = groupBoxes[0].add_SourceCodeViewer();
            commandsToExecute.document().TextEditorProperties.EnableFolding = false;

            //currentScriptPath = commandsToExecute.insert_Below<TextBox>();

            splitContainer_CommandBox = (SplitContainer)groupBoxes[0].Parent.Parent;
            splitContainer_CommandBox.Panel2MinSize = 110;
            splitContainer_CommandBox.FixedPanel = FixedPanel.Panel2;
            var bottomPanel = this.add_GroupBox("Generated Source Code");  //create on thread            
            sourceCodeViewer = bottomPanel.add_SourceCodeViewer();

            splitContainer_CommandBox.insert_Right(bottomPanel, 200);		//insert 'GeneratedSourceCode on the right

            executeButton = this.add_Button("Execute");
            executeButton.fill();

            var outputPanel = groupBoxes[1].add_GroupBox("Output");

            var controls = outputPanel.insert_Above(executeButton, 60);

            splitContainer_TopLevel = (SplitContainer)this.Controls[0];
            splitContainer_Results = (SplitContainer)controls[0];

            result_RichTextBox = outputPanel.add_RichTextBox();

            result_Panel = outputPanel.add_Panel();

            executeButton.insert_Below(20).add_Link("stop execution", () => stopCurrentExecution());


            if (AddToolStrip)
                addToolStrip();

        }
        public void configureGui()
        {
            result_Panel.visible(false);
            executeButton.enabled(false);

            commandsToExecute.textEditor().showInvalidLines(false);

            swapGeneratedCodeViewMode();         	// make it not visible by default        	

            executeButton.onClick(execute);
            commandsToExecute.onTextChanged(onTextChanged);

            commandsToExecute.editor().vScroolBar_Enabled(false);
            commandsToExecute.editor().hScroolBar_Enabled(false);
            commandsToExecute.set_ColorsForCSharp();

            commandsToExecute.allowCompile(false);
            sourceCodeViewer.allowCompile(false);

            sourceCodeViewer.astDetails(false);
            commandsToExecute.astDetails(false);

            //remove original ContextMenu and add new one        	
            sourceCodeViewer.textEditorControl().remove_ContextMenu();
            var contextMenu = commandsToExecute.textEditorControl().add_ContextMenu();

            commandsToExecute.textArea().KeyUp += (sender, e) => handlePressedKeys(e);
            commandsToExecute.textArea().KeyEventHandler += handleKeyEventHandler;

            contextMenu.add_MenuItem("current source code")
                       .add_MenuItem("compile", (menuitem) => compile())
                       .add_MenuItem("copy to clipboard", (menuitem) => currentCode().toClipboard())
                       .add_MenuItem("save", (menuitem) => saveScript())
                       .add_MenuItem("save As", (menuitem) => saveAsScript())
                       .add_MenuItem("what is the current loaded filename", (menuitem) => "The path of the currently loaded file is: {0}".info(currentSourceCodeFilePath()))
                       .add_MenuItem("force compilation", (menuitem) => forceCompilation())
                       .add_MenuItem("replace with generated source code", (menuitem) => replaceMainCodeWithGeneratedSource())
                       .add_MenuItem("show/hide generated source code", (menuitem) => swapGeneratedCodeViewMode())
                       .add_MenuItem("-------------------------------")
                       .add_MenuItem("clear AssembliesCheckedIfExists list", (menuitem) => O2GitHub.clear_AssembliesCheckedIfExists())
                       .add_MenuItem("clear CachedCompiledAssemblies list", (menuitem) => CompileEngine.clearCompilationCache())
                       .add_MenuItem("clear LocalScriptFileMappings list", (menuitem) => CompileEngine.clearLocalScriptFileMappings())
                       .add_MenuItem("clear CompilationPathMappings list", (menuitem) => CompileEngine.clearCompilationPathMappings());

            contextMenu.add_MenuItem("selected text")
                       .add_MenuItem("cut", () => commandsToExecute.editor().clipboard_Cut())
                       .add_MenuItem("copy", () => commandsToExecute.editor().clipboard_Copy())
                       .add_MenuItem("paste", () => commandsToExecute.editor().clipboard_Paste())
                       .add_MenuItem("(if a local file) open selected text in code Editor", () => openSelectedTextInCodeEditor());                                   
            contextMenu.add_MenuItem("execution options")
                       .add_MenuItem("execute", (menuitem) => execute())
                       .add_MenuItem("enable /disable Execute on compile", (menuitem) => ExecuteOnCompile = !ExecuteOnCompile)
                       .add_MenuItem("enable /disable Execute on enter", (menuitem) => ExecuteOnEnter = !ExecuteOnEnter)
                       .add_MenuItem("disable / enabled Auto compile on code change", (menuitem) => AutoCompileOnCodeChange = !AutoCompileOnCodeChange);
            contextMenu.add_MenuItem("auto saved scripts")
                       .add_MenuItem("show auto saved scripts", (menuitem) => showAutoSavedScripts())
                       .add_MenuItem("enable / disable Auto Save on compile success", (menuitem) => AutoSaveOnCompileSuccess = !AutoSaveOnCompileSuccess);
            contextMenu.add_MenuItem("code complete")
                       .add_MenuItem("enable Code Complete", (menuitem) => commandsToExecute.enableCodeComplete())
                       .add_MenuItem("enable /disabled 'Only Show Code Complete Results From O2 Namespace'", (menuitem) => enableDisableFullCodeComplete());
            contextMenu.add_MenuItem("code snippets (helper)")
                       .add_MenuItem("when compiling: Dont use Cached Assembly if available", (menuitem) => insertCodeSnipptet(menuitem.Text))
                       .add_MenuItem("when compiling: remove all auto references to O2 scripts and dlls (same as next two options)", (menuitem) => insertCodeSnipptet(menuitem.Text))
                       //.add_MenuItem("when compiling: don't include extra cs file (with extension methods)", (menuitem) => insertCodeSnipptet(menuitem.Text))
                       .add_MenuItem("when compiling, only add referenced assemblies", (menuitem) => insertCodeSnipptet(menuitem.Text))
                       .add_MenuItem("when compiling, set InvocationParameters to dynamic", (menuitem) => insertCodeSnipptet(menuitem.Text))
                       .add_MenuItem("generate debug symbols (and create temp assembly)", (menuitem) => insertCodeSnipptet(menuitem.Text))
                       .add_MenuItem("add using statement", (menuitem) => insertCodeSnipptet(menuitem.Text))
                       .add_MenuItem("add additional source code (when compile)", (menuitem) => insertCodeSnipptet(menuitem.Text))
                       .add_MenuItem("add external reference (dll or exe or GAC assembly)", (menuitem) => insertCodeSnipptet(menuitem.Text))
                       .add_MenuItem("run in STA thread (when invoke)", (menuitem) => insertCodeSnipptet(menuitem.Text))
                       .add_MenuItem("run in MTA thread (when invoke)", (menuitem) => insertCodeSnipptet(menuitem.Text))
                       .add_MenuItem("clear 'AssembliesCheckedIfExists' cache", (menuitem) => insertCodeSnipptet(menuitem.Text));
            contextMenu.add_MenuItem("This Script Editor")
                        .add_MenuItem("REPL script this Script"                      , scriptTheCurrentScript)
                        .add_MenuItem("REPL script this Script's codeComplete object", scriptTheCurrentScript_CodeComplete);
            contextMenu.add_MenuItem("O2 Scripts")
                       .add_MenuItem("Download/Update O2 Scripts (via http)", O2Scripts.downloadO2Scripts)
                       .add_MenuItem("O2 Script: find WinForms Control and REPL it ", ()=>"Util - Find WinForms Control and REPL it.h2".local().executeH2Script())
                       .add_MenuItem("O2 Script: new C# Script Editor", () => open.scriptEditor())                                              
                       .add_MenuItem("O2 Script: Find Script to execute", () => "Util - O2 Available scripts.h2".local().executeH2Script())
                       .add_MenuItem("O2 Script: open Main O2 GUI", () => "Main O2 Gui.h2".local().executeH2Script())
                       .add_MenuItem("O2 Script: open ConsoleOut", () => "Util - ConsoleOut.h2".local().executeH2Script());

            contextMenu.add_MenuItem("package current Script as StandAlone Exe", () => packageCurrentScriptAsStandAloneExe());            
            contextMenu.add_MenuItem("show O2 Object Model", () => open.o2ObjectModel());            
            contextMenu.add_MenuItem("report a bug to O2 developers", () => ReportBug.showGui(commandsToExecute));
            contextMenu.add_MenuItem("show Log Viewer", (menuitem) => showLogViewer());

        }
        public void openSelectedTextInCodeEditor()
        {
            var selectedText = this.commandsToExecute.editor()
                                                    .selectedText()
                                                    .remove("//O2File:");
            var file = selectedText.local();
            if (file.exists())
                file.showInCodeEditor();
        }
        public void packageCurrentScriptAsStandAloneExe()
        {
            var h2File = currentSourceCodeFilePath();
            if (h2File.valid())
                saveScript();
            else
                h2File = Code.h2_File();
            var packageScript = (Action<string>)"Util - Package O2 Script into separate Folder.h2".executeFirstMethod();
            packageScript(h2File);
        }
        public void insertCodeSnipptet(string snippetToInsert)
        {
            switch (snippetToInsert)
            {
                case "generate debug symbols (and create temp assembly)":
                    commandsToExecute.insert_Text("".line() + "//generateDebugSymbols".line());
                    break;
                case "when compiling: Dont use Cached Assembly if available":
                    commandsToExecute.insert_Text("".line() + "//O2Tag_DontUseCachedAssemblyIfAvailable".line());
                    break;
                case "when compiling: remove all auto references to O2 scripts and dlls (same as next two options)":
                    commandsToExecute.insert_Text("".line() + "//O2Tag_CleanCompilation".line());
                    break;
//                case "when compiling: don't include extra cs file (with extension methods)":
//                  commandsToExecute.insert_Text("".line() + "//O2Tag_DontAddExtraO2Files".line());
//                    break;
                case "when compiling, only add referenced assemblies":
                    commandsToExecute.insert_Text("".line() + "//O2Tag_OnlyAddReferencedAssemblies".line());
                    break;
                case "when compiling, set InvocationParameters to dynamic":
                    commandsToExecute.insert_Text("".line() + "//O2Tag_SetInvocationParametersToDynamic".line());
                    break;
                case "add using statement":
                    commandsToExecute.insert_Text("".line() + "//using:".line());
                    break;
                case "add additional source code (when compile)":
                    commandsToExecute.insert_Text("".line() + "//O2File:".line());
                    break;
                case "add external reference (dll or exe or GAC assembly)":
                    commandsToExecute.insert_Text("".line() + "//O2Ref:".line());
                    break;
                case "run in STA thread (when invoke)":
                    commandsToExecute.insert_Text("".line() + "//StaThread:".line());
                    break;
                case "run in MTA thread (when invoke)":
                    commandsToExecute.insert_Text("".line() + "//MtaThread:".line());
                    break;
                case "clear 'AssembliesCheckedIfExists' cache":
                    commandsToExecute.insert_Text("".line() + "//ClearAssembliesCheckedIfExists".line());
                    break;                
            }
        }
        public void addToolStrip()
        {
            this.ToolStrip = this.insert_Above(30).add_Control<ToolStrip>()
                                                  .splitContainerFixed();			
            
            try
            { 
                ToolStrip.add_Button("run"		, () => { this.execute(); }).with_Icon(FormImages.btExecuteSelectedMethod_Image)	
                         .add_Button("new"		, () => { this.commandsToExecute.editor().newFile() ; }).with_Icon_New()
                         .add_Button("open"		, () => { this.commandsToExecute.editor().openFile(); }).with_Icon_Open()
                         .add_Button("save as"	, () => { this.saveAsScript(); }).with_Icon_Save()
                         //.add_Button("log viewer", () => { this.showLogViewer(); })
                         .add_Label("search:").add_TextBox("").onEnter(searchInText)
                         ;
            }
            catch (Exception ex)
            {                
                ex.log().logStackTrace();
            }
        }
        public ascx_Simple_Script_Editor openFile(string file)
        {
            commandsToExecute.open(file);
            return this;
        }
        public void setCompilerEnvironment()
        {
            var o2Timer = new O2Timer("Code Compiled in");

            this.csharpCompiler = new CSharp_FastCompiler();
            //csharpCompiler.field("forceAstBuildDelay",250); 		// try to prevent the problem with missing the compilation of the last char
            this.csharpCompiler.beforeSnippetAst =
                () =>
                {
                    this.csharpCompiler.InvocationParameters = this.InvocationParameters;
                };


            this.csharpCompiler.onAstFail =
                () =>
                {
                    //"AST creation failed".error();
                    //	this.sourceCodeViewer.enabled(false);    	 				
                    executeButton.enabled(false);
                    result_RichTextBox.textColor(Color.Red).set_Text("Ast Parsing Errors:\r\n\r\n");
                    //.append_Text(csharpCompiler.AstErrors);						
                    commandsToExecute.updateCodeComplete(csharpCompiler);
                    sourceCodeViewer.setDocumentContents(csharpCompiler.SourceCode);
                    OnAstFail.invoke();
                };

            this.csharpCompiler.onAstOK =
                () =>
                {
                    o2Timer.start();
                    commandsToExecute.editor().refresh();
                    sourceCodeViewer.enabled(true);
                    commandsToExecute.invokeOnThread(() => commandsToExecute.Refresh());
                    GeneratedCode = csharpCompiler.SourceCode;
                    commandsToExecute.updateCodeComplete(csharpCompiler);
                    sourceCodeViewer.setDocumentContents(csharpCompiler.SourceCode);
                    OnAstOK.invoke();
                };

            csharpCompiler.onCompileFail =
               () =>
               {
                   //"AST OK, but compilation failed".error();
                   executeButton.enabled(false);
                   var codeOffset = csharpCompiler.getGeneratedSourceCodeMethodLineOffset();
                   csharpCompiler.CompilationErrors.runForEachCompilationError(
                                             (row, col) =>
                                             {
                                                 sourceCodeViewer.editor().setSelectedText(row, col, true, false);
                                                 commandsToExecute.editor().setSelectedText(
                                                     row - codeOffset.Line,
                                                     col - codeOffset.Column,
                                                     true, /*showAsError*/
                                                     false, /*showAsBookMark*/
                                                     false); /*decrementLineAndColumn*/

                                             });
                   result_RichTextBox.textColor(Color.Red)
                                     .set_Text("Compilation Errors:\r\n\r\n")
                                     .append_Text(csharpCompiler.CompilationErrors);
                   onCompileFail.invoke();

               };
            csharpCompiler.onCompileOK =
               () =>
               {
                   o2Timer.stop();
                   "Compilation OK".debug();
                   commandsToExecute.editor().refresh();
                   sourceCodeViewer.editor().refresh();
                   result_RichTextBox.set_Text("Compilation OK:\r\n\r\n")
                                          .textColor(Color.Green);
                   executeButton.enabled(true);
                   if (AutoSaveOnCompileSuccess && Code != defaultCode)
                   {
                       AutoSaveDir.createDir();    // make sure it exits
                       var targetFile = AutoSaveDir.pathCombine(Files.getFileSaveDateTime_Now().trim() + ".cs");
                       targetFile.fileWrite(Code);
                   }

                   onCompileOK.invoke();
                   // once all is done update the codeComplete information
                   if (commandsToExecute.editor().o2CodeCompletion.notNull())
                       commandsToExecute.editor().o2CodeCompletion.addReferences(csharpCompiler.ReferencedAssemblies);

                   //add_ExtraMethodsFile();									// restore previous mappings here

                   //register cacheAssmbly
                   var codeMd5 = previousCompiledCodeText.md5Hash();
                   CompileEngine.CachedCompiledAssemblies.add(codeMd5, this.csharpCompiler.CompiledAssembly.Location);

                   executeButton.enabled(true);

                   if (ExecuteOnCompile)
                       execute();
               };		

        }      
        public string handleExtraO2TagsInSourceCode(string codeToCompile)
        {            
            if (codeToCompile.contains("O2Tag_CleanCompilation"))
                codeToCompile = codeToCompile.insertAfter( "".line() + " //O2Tag_OnlyAddReferencedAssemblies");
      
            csharpCompiler.ResolveInvocationParametersType = codeToCompile.contains("//O2Tag_SetInvocationParametersToDynamic")
                                                                           .isFalse();

            csharpCompiler.UseCachedAssemblyIfAvailable = codeToCompile.contains("//O2Tag_DontUseCachedAssemblyIfAvailable")
                                                                       .isFalse();
            return codeToCompile;
        }    
        public void triggerCompilation()
        {
            TriggerCompilationThread = O2Thread.mtaThread(
                () =>
                {
                    this.sleep(500, false);
                    // this will only trigger the compilation when the current thread matches the last one created (which doesn't happen when the user types fast
                    if (Thread.CurrentThread.ManagedThreadId == TriggerCompilationThread.ManagedThreadId)
                    {
                        executeButton.enabled(false);
                        result_RichTextBox.textColor(Color.Gray).set_Text("... compiling ...");
                        previousCompiledCodeText = currentCode();
                        compileCodeSnippet(previousCompiledCodeText);
                    }
                });
        }
        public void onTextChanged(string newText)
        {
            if (AutoCompileOnCodeChange)
            {
                triggerCompilation();
            }
        }
        public ascx_Simple_Script_Editor compile()
        {
            compileCodeSnippet(currentCode());
            return this;
        }
        public string currentCode()
        {
            return commandsToExecute.editor().getSourceCode();
        }
        public void compileCodeSnippet(string codeSnippet)
        {
            if (codeSnippet.notValid())
            {
                //"[compileCodeSnippet] there was no code provided to compile".error();
                return;
            }
            try
            {
                codeSnippet = handleExtraO2TagsInSourceCode(codeSnippet);
                result_RichTextBox.visible(true);
                result_Panel.visible(false);
                if (codeSnippet.size() > 200)
                {
                    commandsToExecute.editor().vScroolBar_Enabled(true);
                    commandsToExecute.editor().hScroolBar_Enabled(true);
                }
                commandsToExecute.editor().clearBookmarksAndMarkers();
                var currentFile = commandsToExecute.editor().getFullPathTOCurrentSourceCodeFile();
                if (currentFile.fileExists())
                    csharpCompiler.SourceCodeFile = currentFile;
                csharpCompiler.compileSnippet(codeSnippet);
            }
            catch (Exception ex)
            {
                ex.log("in ascx_Simple_Script_Editor compileCodeSnippet", true);
            }
        }
        public void forceCompilation()
        {
            /*var dummyclass = "public class test".line() + 
                             "{".line() +
                             "	public void testMethod()".line() +
                             "	{".line() +
                             commandsToExecute.get_Text().line() + 
                             "	}".line() +
                             "}".line();
            sourceCodeViewer.enabled(true);
            sourceCodeViewer.set_Text(dummyclass);
            csharpCompiler.compileSourceCode(dummyclass);*/
            csharpCompiler.compileSourceCode(sourceCodeViewer.get_Text());
        }
        public void replaceMainCodeWithGeneratedSource()
        {
            commandsToExecute.set_Text(sourceCodeViewer.get_Text());
        }
        public void showLogViewer()
        {
            this.insert_LogViewer();
        }
        public void showAutoSavedScripts()
        {
            if ("Util - Search AutoSaved Scripts (starting with Today).h2".local().fileExists())
                "Util - Search AutoSaved Scripts (starting with Today).h2".local().executeH2Script();
            else            
                O2Thread.mtaThread(() =>
                    {
                        var panel2 = O2Gui.open<Panel>("Search AutoSaved Scripts", 800, 300);
                        var autoSaveDir = PublicDI.config.AutoSavedScripts;
                        var controls = panel2.add_1x1("Files", "Content", true, 200);
                        var sourceViewer = controls[1].add_SourceCodeViewer();
                        var directory = controls[0].add_Directory();
                        directory.afterFileSelect((file) => sourceViewer.open(file));
                        directory.open(autoSaveDir);
                    });
        }
        public bool handleKeyEventHandler(char key)
        {
            if (ExecuteOnEnter && key == 10)
            {
                "Executing method".debug();
                execute();
                return true;
            }
            return false;
        }        
        public void execute()
        {
            if (executeButton.Enabled)
            {
                var currentScript = currentSourceCodeFilePath();
                if (currentScript.fileExists())
                    PublicDI.CurrentScript = currentScript;
                result_RichTextBox.textColor(Color.Black).set_Text("Executing code");

                stopCurrentExecution();
                ExecutionThread = O2Thread.mtaThread(() => showResult(csharpCompiler.executeFirstMethod()));
            }
        }
        public void stopCurrentExecution()
        {
            if (ExecutionThread.notNull() && ExecutionThread.IsAlive)
            {
                "ExecutionThread is alive, so stopping it".info();
                ExecutionThread.Abort();
                result_RichTextBox.textColor(Color.Red).set_Text("...current thread stopped...");
            }
        }
        public void showResult(object result)
        {
            LastExecutionResult = result;
            result_RichTextBox.visible(false);
            result_Panel.visible(false);
            result_Panel.clear();

            if (result == null)
                result = "[null value]";

            switch (result.typeName())
            {
                case "Boolean":
                case "String":
                case "Int64":
                case "Int32":
                case "Int16":
                case "Byte":
                    result_RichTextBox.visible(true);
                    result_RichTextBox.textColor(Color.Black).set_Text(result.ToString());
                    break;
                case "Bitmap":
                    result_Panel.visible(true);
                    result_Panel.add_PictureBox().load((Bitmap)result);
                    break;
                default:
                    result_Panel.visible(true);
                    var showInfo = result_Panel.add_Control<ascx_ShowInfo>();
                    //result_PropertyGrid.visible(true);
                    showInfo.show(result);
                    break;
            }

            onExecute.invoke(result);

        }
        public string currentSourceCodeFilePath()
        {
            return commandsToExecute.editor().getFullPathTOCurrentSourceCodeFile();
        }
        public void saveScript()
        {
            if (currentSourceCodeFilePath().fileExists())
            {
                "Script saved to: {0}".info(currentSourceCodeFilePath());
                commandsToExecute.editor().saveSourceCode();
            }
            else
                saveAsScript();
        }
        public void saveAsScript()
        {
            this.invokeOnThread(
                () =>
                {
                    //var defaultH2ScriptFolder = @"C:\O2\_XRules_Local\H2Scripts";
                    var defaultH2ScriptFolder = currentSourceCodeFilePath().valid()
                                                    ? currentSourceCodeFilePath().directoryName()
                                                    : PublicDI.config.O2TempDir.pathCombine("..");
                    Files.checkIfDirectoryExistsAndCreateIfNot(defaultH2ScriptFolder);
                    var targetFile = O2Forms.askUserForFileToSave(defaultH2ScriptFolder, ".h2");
                    if (targetFile.valid())
                    {
                        var h2Script = new H2(currentCode());
                        h2Script.save(targetFile);
                        commandsToExecute.open(targetFile);
                    }
                    "target: {0}".info(targetFile);
                });
        }
        public void swapGeneratedCodeViewMode()
        {
            this.invokeOnThread(
                () =>
                {
                    splitContainer_TopLevel.Panel2Collapsed = !splitContainer_TopLevel.Panel2Collapsed;
                    if (isGeneratedSourceCodeVisible())
                        compileCodeSnippet(currentCode());
                });
        }
        public void showGeneratedSourceCode()
        {
            if (splitContainer_TopLevel.Panel2Collapsed)
                swapGeneratedCodeViewMode();
        }
        public bool isGeneratedSourceCodeVisible()
        {
            return !splitContainer_TopLevel.Panel2Collapsed;
        }
        public void enableCodeComplete()
        {
            commandsToExecute.enableCodeComplete();
            commandsToExecute.editor().o2CodeCompletion.UseParseCodeThread = false;
            commandsToExecute.editor().o2CodeCompletion.addReferences(csharpCompiler.ReferencedAssemblies);
        }
        public void enableDisableFullCodeComplete()
        {
            var o2CodeCompletion = commandsToExecute.editor().o2CodeCompletion;
            if (o2CodeCompletion.notNull())
                o2CodeCompletion.OnlyShowCodeCompleteResultsFromO2Namespace = !o2CodeCompletion.OnlyShowCodeCompleteResultsFromO2Namespace;
        }
        public string Code
        {
            get
            {
                return currentCode();
            }

            set
            {
                commandsToExecute.set_Text(value);
            }
        }
        public ascx_Simple_Script_Editor set_Script(string script)
        {
            this.Code = script;
            return this;
        }
        public void searchInText(string text)
        {
            this.commandsToExecute.editor().invoke("searchForTextInTextEditor_findNext", text);
        }
        public void scriptTheCurrentScript_CodeComplete()
        {
            commandsToExecute.editor().o2CodeCompletion.script_Me();
        }
        public void scriptTheCurrentScript()
        {
            var code = "return script.Code;";
            this.script_Me("script").set_Code(code);

            /*var topPanel = "PoC - Script the Script".popupWindow(700, 400);

            var scriptHost = topPanel.title("Script").add_Script(false);
            scriptHost.onCompileExecuteOnce();
            scriptHost.InvocationParameters.add("script", this);
            var code = @"
var _script = (script as object).castViaTypeConfusion<ascx_Simple_Script_Editor>();
//_script.execute();
return _script.Code;
            
  
//O2" + @"Tag_SetInvocationParametersToDynamic
//O2" + @"Tag_DontUseCachedAssemblyIfAvailable
//O2" + @"File:_Extra_methods_TypeConfusion.cs
            ".trim();

            scriptHost.set_Script(code);

            scriptHost//.compile()
                      .execute();
            */
        }

        private void handlePressedKeys(KeyEventArgs e)
        {
            //var currentOffset = commandsToExecute.textArea().Document.PositionToOffset(commandsToExecute.editor().caretLine);
            //commandsToExecute.textArea().Document.Remove(currentOffset, 1);
            //e.KeyCode.str().info();
            //O2.Kernel.PublicDI.log.debug("KeyUp: " + e.KeyValue.ToString()); ;                
            if (e.Modifiers == Keys.Control && e.KeyValue == 'B')           // Ctrl+B compiles code
            {
                "Compiling code".debug();
                compileCodeSnippet(Code);

            }
            if (e.Modifiers == Keys.Control && e.KeyValue == 'S')           // Ctrl+S saves code
            {
                "Saving Script".debug();
                saveScript();

            }
            else if (e.KeyValue == 116 ||                                       // F5 (key 116) and 
                (e.Modifiers == Keys.Control && e.KeyValue == 'R') ||           // Ctrl+R  executes it
                (e.Modifiers == Keys.Control && e.KeyValue == 13))              // Ctrl+Enter  executes it
            {
                "Executing method".debug();
                execute();
            }
        }

                public static ascx_Simple_Script_Editor startControl()
        {
            var control = O2Gui.load<ascx_Simple_Script_Editor>("O2 Simple Script Editor", 700, 300);

            control.invokeOnThread(() =>
            {
                control.InvocationParameters.Add("testString", "Hello World");
                control.InvocationParameters.Add("testNumber", 42);
                control.commandsToExecute.set_Text(control.defaultCode);
            });
            return control;
        }
        public static ascx_Simple_Script_Editor startControl_NoCodeComplete()
        {
            var host = O2Gui.load<Panel>("O2 Simple Script Editor", 700, 300);
            return host.invokeOnThread(
                () =>
                {
                    var scriptEditor = new ascx_Simple_Script_Editor(false);
                    scriptEditor.fill();
                    host.add_Control(scriptEditor);
                    return scriptEditor;
                });
        }		 

    }
}