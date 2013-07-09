// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Drawing;
using System.Windows.Forms;
using FluentSharp.BCL;
using FluentSharp.BCL.Controls;
using FluentSharp.BCL.Utils;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.SharpDevelop;
using FluentSharp.SharpDevelop.Utils;

namespace FluentSharp.REPL.Utils
{
    public class ascx_Execute_Scripts : UserControl
    {        
        public string welcomeMessage = "Hello! To start right-click on the logo";        
        public string currentScript = "";
        public CSharp_FastCompiler csharpCompiler;
        public Action csharpCompiler_OnAstFail;
        public Action csharpCompiler_OnAstOk;
        public Action csharpCompiler_OnCompileFail;
        public Action csharpCompiler_OnCompileOk;

        //public Label statusLabel;
        public RichTextBox results_RichTextBox;
        public SplitContainer mainSplitContainer;
        public ToolStripStatusLabel statusLabel;

        public static string NEW_GUI_SCRIPT = "Main O2 Gui.h2";

        public static void startControl_No_Args()
        {            
            startControl("");
        }

        public static void startControl_With_Args(string[] args)
        {
            RegisterWindowsExtension.registerO2Extensions();
            startControl(args);
        }

        public static void startControl(string scriptToExecute)
        {
            //"O2Logo.ico".local().icon().set_As_Default_Form_Icon();
            // load Execute_Scripts GUI
            //var formTitle = "O2 XRules Database ({0})".format("O2_XRules_Database.exe".assembly().version());
            var formTitle = "OWASP O2 Platform - Launcher (v5.1)";
            "{0}".info(formTitle);
            var executeScripts = (ascx_Execute_Scripts)typeof(ascx_Execute_Scripts).showAsForm(formTitle, 340, 300);
            if (executeScripts.isNull())
            {
                MessageBox.Show(@"Object Creation error in startControl", @"Start_O2 (ascx_Execute_Scripts)");
                return;
            }
            "LocalScriptsFolder: {0}".debug(PublicDI.config.LocalScriptsFolder);
            executeScripts.buildGui();
            executeScripts.insert_LogViewer();

            // see if there there is a script to execute 
            //(first via normal process arguments 
            if (scriptToExecute.valid() && scriptToExecute.fileExists())
                executeScripts.runScriptAndCloseGui(scriptToExecute);
            else
            {                
                // load new gui       
                var newGuiScript = NEW_GUI_SCRIPT.local();
                if (newGuiScript.fileExists())
                {
                    executeScripts.welcomeMessage = "New O2 GUI detected, launching it now...";
                    executeScripts.status(executeScripts.welcomeMessage);
                    {
                        O2Thread.mtaThread(() =>
                        {
                            loadNewGui(executeScripts);
                        });
                    }
                }
                //}
            }
        }

        public static void startControl(string[] args)
        {
            if (args.notNull() && args.size() > 0)
            {
                var fileToExecute = args[0];
                if (fileToExecute.fileExists().isFalse())
                    fileToExecute = fileToExecute.local();
                startControl(fileToExecute);
            }
            else
                startControl("");
        }

        public void runScriptAndCloseGui(string scriptToExecute)
        {
            "Found startup script (for execution): ".info(scriptToExecute);
            this.loadFile(scriptToExecute);
            this.csharpCompiler_OnCompileOk = () => { this.close(); };
        }
        private static void loadNewGui(ascx_Execute_Scripts executeScripts)
        {
            try
            {
                /*                var cached_O2Main_WPF_GUI = PublicDI.config.CurrentExecutableDirectory.pathCombine(NEW_GUI_SCRIPT + ".cached.h2");
                                var cached_O2Main_WPF_GUI_Dll = PublicDI.config.CurrentExecutableDirectory.pathCombine(NEW_GUI_SCRIPT + ".cached.Dll");
                                if (cached_O2Main_WPF_GUI.fileExists().isFalse() ||
                                    cached_O2Main_WPF_GUI.fileContents() != NEW_GUI_SCRIPT.local().fileContents())
                                {
                                    Files.deleteFile(cached_O2Main_WPF_GUI);
                                    Files.deleteFile(cached_O2Main_WPF_GUI_Dll);
                                    Files.Copy(NEW_GUI_SCRIPT.local(), cached_O2Main_WPF_GUI);
                                    var assembly = cached_O2Main_WPF_GUI.compile_H2Script();
                                    var dllLocation = assembly.Location;                    
                                    Files.Copy(dllLocation, cached_O2Main_WPF_GUI_Dll);
                                    //we need to make this or the 2nd time around the dynamic compiled references will not be found
                                    foreach (var referencedAssembly in assembly.GetReferencedAssemblies().toList())
                                    {
                                        var localPath = "{0}\\{1}.dll".format("".o2Temp2Dir(), referencedAssembly.Name);
                                        if (localPath.fileExists())
                                     //   if (referencedAssembly.E.Location.starts())
                                            Files.Copy(localPath, PublicDI.config.CurrentExecutableDirectory);
                                    }
                                }
                                if (cached_O2Main_WPF_GUI_Dll.fileExists())
                                {
                                    "Found local copy of precompiled {0} as {1}, so executing it".info(NEW_GUI_SCRIPT, cached_O2Main_WPF_GUI_Dll);
                                    cached_O2Main_WPF_GUI_Dll.assembly().executeFirstMethod();                                        
                                }
                                else
                                {
                 */
                var newGui = NEW_GUI_SCRIPT.compile_H2Script();
                if (newGui.notNull())
                {
                    newGui.executeFirstMethod();
                    "Welcome to O2 :)".info();
                }
                else
                {
                    executeScripts.status("There was an error compiling the new GUI!");
                   /* if ("There was an error compiling the main O2 Script, do you want to clear the Compiled Cache and try again?".askUserQuestion())
                    {
                        CompileEngine.clearCompilationCache();
                        NEW_GUI_SCRIPT.compile_H2Script().executeFirstMethod();
                    }*/
                }
                //             }
                var currentWinForms = executeScripts.applicationWinForms();
                if (currentWinForms.size() == 1)
                {
                    //   if ("The new GUI could not be launched, do you want download the latest version?".askUserQuestion())
                    //       openBrowserWithDownloadLink();
                }
                else
                    executeScripts.close();
            }
            catch (Exception ex)
            {
                ex.log("in ascx_ExecuteScripts.loadNewGui()");
            }
        }

        /*private static void openBrowserWithDownloadLink()
        {
            O2Gui.open<Panel>("Download new version of O2", 700, 500)
                 .add_Browser()
                 .open(PublicDI.config.O2DownloadLocation);
        } */

        public ascx_Execute_Scripts()
        {
            handleSpecialControlKeys();                        
        }

        public bool is32Bit()
        {
            if (@"C:\Program Files (x86)".dirExists())
            {
                "This is a 64bit OS".debug();
                return false;
            }
            return true;

        }

        private void handleSpecialControlKeys()
        {
            if (Control.ModifierKeys == Keys.Shift)
                openO2LogViewer();
        }

        private void buildGui()
        {
            this.invokeOnThread(
                () =>
                {
                    "Buliding Gui for ascx_ExecuteScripts.cs.o2".info();
                    // add controls    
                    statusLabel = this.parentForm().add_StatusStrip(Color.White);
                    BackColor = Color.White;
                    var topPanel = this.add_Panel();
                    var pictureBox = topPanel.add_PictureBox();                    
                    statusLabel.IsLink = true;
                    statusLabel.LinkBehavior = LinkBehavior.NeverUnderline;
                    statusLabel.set_Text(welcomeMessage).textColor(this, Color.Black);

                    results_RichTextBox = this.add_RichTextBox();

                    mainSplitContainer = topPanel.insert_Below(results_RichTextBox, 100)
                                                 .get<SplitContainer>();

                    mainSplitContainer.panel2Collapsed(true);

                    setupPictureBoxContextMenu(pictureBox);

                    pictureBox.load(FormImages.O2_Logo);
                    this.onDrop((file) => loadFile(file));

                    statusLabel.Click += (sender, e) => openSimpleScriptEditor();
                    //statusLabel.textColor(this, Color.Black);

                    // put this on menu item
                    //this.add_Link("reload",10,2,()=>loadH2Script());                    
                    "Buliding Gui complete".info();                    
                });
        }

        public void setupPictureBoxContextMenu(PictureBox pictureBox)
        {
            pictureBox.onDrop((file) => loadFile(file));
            pictureBox.DoubleClick += (sender, e) => showAvailableScripts();

            var contextMenu = pictureBox.add_ContextMenu();
            contextMenu.add_MenuItem("View available scripts", () => showAvailableScripts());
            contextMenu.add_MenuItem("Open O2 Command Prompt", () => openO2CommandPromt());
            contextMenu.add_MenuItem("O2 Script Development &&  Debug")
                       .add_MenuItem("Open Source Code Editor", () => openSourceCodeEditor())
                       .add_MenuItem("Open Simple Script Editor", () => openSimpleScriptEditor())
                       .add_MenuItem("Open Quick development gui", () => executeScript("Opening Quick Development GUI", "ascx_Quick_Development_GUI.cs.o2"))
                       .add_MenuItem("Open 'Graph' development gui", () => executeScript("Opening Graph With Inspector)", "ascx_GraphWithInspector.cs"))
                //.add_MenuItem("Open XRules/Script Editor", () => open.scriptEditor())
                       .add_MenuItem("Show O2 Object Model", () => open.o2ObjectModel())
                       .add_MenuItem("Show O2 Executable Directry", () => open.directory(PublicDI.config.CurrentExecutableDirectory, true))
                       .add_MenuItem("Current Script", false)
                                    .add_MenuItem("View loaded script source Code", () => showLoadedScriptSourceCode())
                                    .add_MenuItem("Execute Again", executeCompiledCode);
            contextMenu.add_MenuItem("Tools")
                //.add_MenuItem("Image Screenshot tool", () => screenShotTool())
                       .add_MenuItem("MediaWiki Editor", () => executeScript("Opening MediaWiki Client)", "MediaWikiEditor.cs.o2"))
                       .add_MenuItem("Twitter Client", () => executeScript("Opening Twitter Client)", "Twitter Client.h2"))
                       .add_MenuItem("Simple Text Editor", () => executeScript("Opening Simple Text Editor", "Simple Text Editor.h2"))
                       .add_MenuItem("O2 'Secret Data' Editor", () => executeScript("Opening 'Secret Data Editor'", "SecretDataEditor.cs.o2"));
            //.add_MenuItem("O2 'Secret Data' Editor", () => new SecretDataEditor().showGui());

            contextMenu.add_MenuItem("Open O2 Log Viewer", openO2LogViewer);
            contextMenu.add_MenuItem("Exit Application (and close all windows)", () => PublicDI.config.closeO2Process());
        }



        public ascx_Execute_Scripts execute(string infoMessage, MethodInvoker codeToExecute)
        {
            showInfo(infoMessage);
            codeToExecute();
            return this;
        }

        public ascx_Execute_Scripts executeScript(string infoMessage, string scriptToExecute)
        {
            showInfo(infoMessage);
            scriptToExecute.executeFirstMethod();
            return this;
        }

        public void loadH2Script()
        {
            loadFile(currentScript);
        }

        public object loadFile(string fileToLoad)
        {
            if (fileToLoad.isImage())
            {
                statusLabel.set_Text("showing image: {0}".format(fileToLoad));
                return show.image(fileToLoad);
            }
            if (fileToLoad.isText())
            {
                statusLabel.set_Text("showing text file: {0}".format(fileToLoad));
                return show.file(fileToLoad);
            }
            if (fileToLoad.isDocument())
            {
                statusLabel.set_Text("showing rtf document: {0}".format(fileToLoad));
                return show.document(fileToLoad);
            }
            loadH2Script(fileToLoad);
            return null;
        }

        public void loadH2Script(string scriptToLoad)
        {
            O2Thread.mtaThread(() =>
            {
                if (scriptToLoad.fileName().starts(this.typeName()))
                {
                    PublicDI.log.error("We can execute the current type of we will get a recursive load :)");
                    return;
                }
                currentScript = scriptToLoad;
                statusLabel.set_Text("loading script: {0}".format(scriptToLoad.fileName()));

                csharpCompiler = new CSharp_FastCompiler();

                csharpCompiler.onAstFail =
                    () =>
                    {
                        showError("Ast creation failed", csharpCompiler.AstErrors);
                        csharpCompiler_OnAstFail.invoke();
                    };

                csharpCompiler.onAstOK =
                    () =>
                    {
                        showInfo("Ast creation Ok");
                        csharpCompiler_OnAstOk.invoke();
                    };

                csharpCompiler.onCompileFail =
                    () =>
                    {
                        showError("Compilation failed", csharpCompiler.CompilationErrors);
                        csharpCompiler_OnCompileFail.invoke();
                    };

                csharpCompiler.onCompileOK =
                    () =>
                    {
                        showInfo("Compilation Ok: Executing 1st method");
                        csharpCompiler_OnCompileOk.invoke();
                        executeCompiledCode();
                    };

                var sourceCode = "";
                PublicDI.CurrentScript = scriptToLoad;
                csharpCompiler.SourceCodeFile = scriptToLoad;
                if (scriptToLoad.extension(".h2"))
                    sourceCode = H2.load(scriptToLoad).SourceCode;
                if (scriptToLoad.extension(".o2") || scriptToLoad.extension(".cs"))
                    sourceCode = scriptToLoad.contents();
                if (sourceCode.valid())
                    csharpCompiler.compileSnippet(sourceCode);
                else
                    statusLabel.set_Text("Non supported file").textColor(this, Color.Red);
            });
        }

        /*public string currentScript()
        {
            if (csharpCompiler.notNull() && csharpCompiler.SourceCodeFile.fileExists())
                return csharpCompiler.SourceCodeFile.fileName();
            return "";
        }*/

        public void showLoadedScriptSourceCode()
        {
            if (currentScript.fileExists())
            {
                var sourceCodeEditor = (SourceCodeEditor)typeof(SourceCodeEditor)
                                            .showAsForm("source code for: {0}".format(currentScript), 600, 200);
                sourceCodeEditor.loadSourceCodeFile(currentScript);
            }
        }

        public void mapFoldersAndFiles(TreeView targetTreeView, TreeNode treeNode)
        {    
            targetTreeView.clear(treeNode);
            var folder = treeNode.Tag.ToString();
            foreach (var dir in folder.dirs())
                if (dir.contains(".git").isFalse())
                    targetTreeView.add_Node(treeNode, dir.fileName(), dir, true)
                        .ForeColor = Color.SaddleBrown;
            foreach (var file in folder.files())
                targetTreeView.add_Node(treeNode, file.fileName(), file, false)
                    .ForeColor = Color.DarkBlue;
        }

        public void showAvailableScripts()
        {
            status("Showing available scripts");
            //var path = XRules_Config.PathTo_XRulesDatabase_fromO2;
            var path = PublicDI.config.LocalScriptsFolder;
            
            //var treeView = new TreeView();
            var treeView = (TreeView)WinForms.showAscxInForm(typeof(TreeView), "O2 - Available scripts", 300, 300);

            treeView.BeforeExpand += (sender, e) => mapFoldersAndFiles(treeView, e.Node);

            treeView.NodeMouseDoubleClick += (sender, e) =>
                {
                    var target = e.Node.Tag.ToString();
                    if (target.isFile())
                        loadFile(target);
                };
            // ReSharper disable ImplicitlyCapturedClosure
            treeView.ItemDrag += (sender, e) =>
                {                    
                    var node = (TreeNode)e.Item;
                    treeView.SelectedNode = node;
                    var target = node.Tag.ToString();
                    if (target.isFile())
                        treeView.DoDragDrop(target, DragDropEffects.Copy);
                };
            // ReSharper restore ImplicitlyCapturedClosure
            var rootNode = treeView.add_Node("Scripts in: " + path + "", path, true);
            mapFoldersAndFiles(treeView, rootNode);
            treeView.expand(rootNode);
        }

        public void openSourceCodeEditor()
        {
            var sourceCodeEditor = O2Gui.open<Panel>("Source Code Editor", 600, 400).add_SourceCodeEditor();
            var defaultSourceCode = CompileEngine.findScriptOnLocalScriptFolder("Hello_O2_World.cs").fileContents();
            sourceCodeEditor.open(defaultSourceCode.saveWithExtension(".cs"));
        }

        public void openO2CommandPromt()
        {
            executeScript("Showing O2 CommandPromt", "ascx_O2_Command_Prompt.cs.o2");
            //O2Utils.O2CommandPrompt.run();
        }

        public void openSimpleScriptEditor()
        {
            open.scriptEditor();
            //executeScript("Showing Simple Script Editor", "ascx_Simple_Script_Editor.cs.o2");
            //typeof(ascx_Simple_Script_Editor).showAsForm("Simple Script Editor", 600,400);     		
        }
   
        public void openO2LogViewer()
        {
            O2Gui.open<ascx_LogViewer>();
        }

        public void executeCompiledCode()
        {
            O2Thread.mtaThread(() =>
            {
                try
                {
                    if (csharpCompiler != null)
                        //if (csharpCompiler.CompilerResults != null)
                        if (csharpCompiler.CompiledAssembly != null)
                        {
                            var result = csharpCompiler.executeFirstMethod();
                            showInfo("Execution Completed", result);
                        }

                }
                catch (Exception ex)
                {
                    PublicDI.log.ex(ex, "in executeCompiledCode");
                }
            });
        }

        public void showError(string errorMessage, string errorDetails)
        {
            statusLabel.set_Text(errorMessage).textColor(this, Color.Red);
            results_RichTextBox.set_Text(errorDetails).textColor(Color.Red);
            mainSplitContainer.panel2Collapsed(false);
        }

        public void showInfo(string infoMessage)
        {
            showInfo(infoMessage, null);
        }

        public void showInfo(string infoMessage, object details)
        {
            infoMessage.info();
            statusLabel.set_Text(infoMessage).textColor(this, Color.Green);
            if (details != null)
            {
                var data = details.ToString();
                if (data.valid())
                {
                    results_RichTextBox.textColor(Color.Black).set_Text(data);
                    mainSplitContainer.panel2Collapsed(false);
                    return;
                }
            }
            mainSplitContainer.panel2Collapsed(true);
        }

        public void status(string newStatusText)
        {
            statusLabel.set_Text(newStatusText).textColor(this, Color.Black);
        }
    }

}
