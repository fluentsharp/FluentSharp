// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.ComponentModel;
using System.Windows.Forms;
using FluentSharp.WinForms;
using FluentSharp.WinForms.Utils;
using FluentSharp.CSharpAST.Utils;
using FluentSharp.CoreLib.API;
using ICSharpCode.TextEditor.Document;
using FluentSharp.CoreLib;
using FluentSharp.SharpDevelopEditor.Resources;


namespace FluentSharp.REPL.Controls
{
    public partial class SourceCodeEditor : UserControl
    {

        public SourceCodeEditor()
        {			
            setDefaultValues();
            InitializeComponent();
			
            setControlImages();
        }



        public SourceCodeEditor(String sFileToOpen) : this()
        {
            //InitializeComponent();
            this.sFileToOpen = sFileToOpen;
        }


        private void SourceCodeEditor_Load(object sender, EventArgs e)
        {

            onLoad();
        }

        
        //assign the images here so that we don't need to embed resources into the current assembly (which is a mess and not properly supported by Roslyn)
        public void setControlImages()
        {
            this.btCompileCode.Image = ImagesForControls.btCompileCode_Image;
            this.btDebugMethod.Image = ImagesForControls.btDebugMethod_Image;
            this.btDragAssemblyCreated.Image = ImagesForControls.btDragAssemblyCreated_Image;            
            this.btExecuteOnExternalEngine.Image = ImagesForControls.btExecuteOnExternalEngine_Image;
            this.btExecuteSelectedMethod.Image = ImagesForControls.btExecuteSelectedMethod_Image;
            this.btOpenFile.Image = ImagesForControls.btOpenFile_Image;
            this.btSaveFile.Image = ImagesForControls.btSaveFile_Image;
            this.btSeachAndViewAst.Image = ImagesForControls.btSeachAndViewAst_Image;
            this.btSelectedLineHistory.Image = ImagesForControls.btSelectedLineHistory_Image;
            this.btSettings.Image = ImagesForControls.btSettings_Image;            
            this.btShowHideCompilationErrors.Image = ImagesForControls.btShowHideCompilationErrors_Image;
            this.btShowHidePythonLogExecutionOutputData.Image = ImagesForControls.btShowHidePythonLogExecutionOutputData_Image;
            this.btShowLogs.Image = ImagesForControls.btShowLogs_Image;
            this.tbShowO2ObjectModel.Image = ImagesForControls.tbShowO2ObjectModel_Image;
        }
        private bool TextArea_KeyEventHandler(char ch)
        {
            //PublicDI.log.debug("KeyEventHandler: " + ch.ToString()); ;
            if (ch == '\n')
            {
                if (tecSourceCode.ActiveTextAreaControl.TextArea.SelectionManager.HasSomethingSelected)
                {
                    // if we are supporting compilation, see if there must be another controls that want to handle compilation on TextSelected+Enter                    
                    if (btCompileCode.Visible && eEnterInSource_Event != null)
                        eEnterInSource_Event();
                    else
                        compileSourceCode();
                    return true;
                }
            }
            return false;
        }


        private void Document_DocumentChanged(object sender, DocumentEventArgs e)
        {
            btSave.Enabled = true;
            lbSourceCode_UnsavedChanges.Visible = true;
            btSaveFile.Enabled = true;
            lbSource_CodeFileSaved.Visible = false;
            if (null != eDocumentDataChanged)
                foreach (Delegate dDelegate in eDocumentDataChanged.GetInvocationList())
                    dDelegate.DynamicInvoke(new Object[] { tecSourceCode.Text });
        }

        private void SelectionManager_SelectionChanged(object sender, EventArgs e)
        {
            if (null != eDocumentSelectionChanged_WordAndLine)
                foreach (Delegate dDelegate in eDocumentSelectionChanged_WordAndLine.GetInvocationList())
                {
                    String sWord = "", sObject = "";
                    getAtCaret_WordAndObject(ref sWord, ref sObject);
                    dDelegate.DynamicInvoke(new Object[] { sWord, sObject });
                }
        }

        private void cboxLineNumbers_CheckedChanged(object sender, EventArgs e)
        {
            tecSourceCode.ShowLineNumbers = cboxLineNumbers.Checked;
        }

        private void cboxTabs_CheckedChanged(object sender, EventArgs e)
        {
            tecSourceCode.ShowTabs = cboxTabs.Checked;
        }

        private void cboxSpaces_CheckedChanged(object sender, EventArgs e)
        {
            tecSourceCode.ShowSpaces = cboxSpaces.Checked;
        }

        private void cboxInvalidLines_CheckedChanged(object sender, EventArgs e)
        {
            tecSourceCode.ShowInvalidLines = cboxInvalidLines.Checked;
        }

        private void cboxEOLMarkers_CheckedChanged(object sender, EventArgs e)
        {
            tecSourceCode.ShowEOLMarkers = cboxEOLMarkers.Checked;
        }

        private void cboxHRuler_CheckedChanged(object sender, EventArgs e)
        {
            tecSourceCode.ShowHRuler = cboxHRuler.Checked;
        }

        private void cboxVRuler_CheckedChanged(object sender, EventArgs e)
        {
            tecSourceCode.ShowVRuler = cboxVRuler.Checked;
        }


        private void tecSourceCode_Load(object sender, EventArgs e)
        {
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            saveSourceCode();
        }



        private void tbSourceCode_FileLoaded_DragEnter(object sender, DragEventArgs e)
        {
            Dnd.setEffect(e);
        }

        private void tecSourceCode_DragEnter(object sender, DragEventArgs e)
        {
            Dnd.setEffect(e);
        }

        private void SourceCodeEditor_DragEnter(object sender, DragEventArgs e)
        {
            Dnd.setEffect(e);
        }


        private void tbSourceCode_FileLoaded_DragDrop(object sender, DragEventArgs e)
        {
            processDragAndDropFile(e);
        }


        private void tecSourceCode_DragDrop(object sender, DragEventArgs e)
        {
            processDragAndDropFile(e);
        }

        private void SourceCodeEditor_DragDrop(object sender, DragEventArgs e)
        {
            processDragAndDropFile(e);
        }

        private void processDragAndDropFile(DragEventArgs e)
        {
            O2Thread.mtaThread(() =>
                {
                    var droppedObject = (FilteredSignature)Dnd.tryToGetObjectFromDroppedObject(e, typeof(FilteredSignature));
					if (droppedObject != null)
						tbTextSearch.set_Text(droppedObject.sSignature);
					else
					{
						var fileOrFolder = Dnd.tryToGetFileOrDirectoryFromDroppedObject(e);
						if (fileOrFolder.fileExists())
							loadSourceCodeFile(fileOrFolder);
					}
                });
        }

        /*  private void pbSourceCode_MouseDown(object sender, MouseEventArgs e)
        {
            Dnd.DragAndDropAction ddaSourceCode = Dnd.getSourceCodeAction(sPathToFileLoaded);
            DoDragDrop(ddaSourceCode, DragDropEffects.Copy);
        }

        private void pbSourceCode_Click(object sender, EventArgs e)
        {
        }*/

        /*    private void tecSourceCode_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void tecSourceCode_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void tecSourceCode_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
        }*/

        // this is not working at the moment (need to find a way to implement this highlight

        private void tbTextSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                searchForTextInTextEditor_findNext(tbTextSearch.Text);
        }

        private void tbTextSearch_TextChanged(object sender, EventArgs e)
        {
            searchForTextInTextEditor(tbTextSearch.Text);
        }

        /*        public static SourceCodeEditor loadFile(string file)
                {
                    var sourceCodeEditor =
                        (SourceCodeEditor)
                        PublicDI.windowsForms.openAscx(typeof (SourceCodeEditor), false, "Editing: " + file);
                    sourceCodeEditor.loadSourceCodeFile(file);
                    return sourceCodeEditor;
                }*/

        /*  public static void loadFile(string file, int lineToSelect)
        {
            loadFile(file).gotoLine(lineToSelect);
        }*/

        private void tbSourceCode_FileLoaded_TextChanged(object sender, EventArgs e)
        {
            lbSourceCode_UnsavedChanges.Visible = true;
            lbSource_CodeFileSaved.Visible = false;
            btSaveFile.Enabled = true;
        }

        private void tbSourceCode_FileLoaded_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                saveSourceCode();
        }

        private void settings_Click(object sender, EventArgs e)
        {
            groupBoxWithFileAndSaveSettings.Visible = !groupBoxWithFileAndSaveSettings.Visible;
        }

        private void compile_Click(object sender, EventArgs e)
        {
            O2Thread.mtaThread(compileSourceCode);
        }

        private void executeSelectedMethod_Click(object sender, EventArgs e)
        {
            O2Thread.mtaThread(executeMethod);
        }

        private void showLogs_Click(object sender, EventArgs e)
        {
            O2Thread.mtaThread(showLogViewerControl);
        }

        private void cBoxSampleScripts_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadSampleScript(cBoxSampleScripts.Text);
        }

        private void tecSourceCode_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void tbMaxLoadSize_TextChanged(object sender, EventArgs e)
        {
            setMaxLoadSize(tbMaxLoadSize.Text);
        }

        private void btSelectedLineHistory_Click(object sender, EventArgs e)
        {
            tbExecutionHistoryOrLog.Visible = !tbExecutionHistoryOrLog.Visible;
        }        

        private void llCurrenlyLoadedObjectModel_MouseDown(object sender, MouseEventArgs e)
        {
            DoDragDrop(GetCurrentCSharpObjectModel(), DragDropEffects.Copy);
        }

        private void llReload_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            reloadCurrentFile();
        }

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {
            O2Thread.mtaThread(compileSourceCode);
        }

        private void btSaveFile_Click(object sender, EventArgs e)
        {
            saveSourceCode();
        }

        private void btDragAssemblyCreated_MouseDown(object sender, MouseEventArgs e)
        {
            if (compiledAssembly != null)
                DoDragDrop(compiledAssembly.Location, DragDropEffects.Copy);
        }

        private void btExecutePythonScript_Click(object sender, EventArgs e)
        {
            executeOnExternalEngine(cbExternalEngineToUse.Text);
        }

        private void btShowHidePythonLogExecutionOutputData_Click(object sender, EventArgs e)
        {
            tbExecutionHistoryOrLog.Visible = !tbExecutionHistoryOrLog.Visible;
        }

        private void cbAutoTryToFixSourceCodeFileReferences_CheckedChanged(object sender, EventArgs e)
        {
//            HandleO2MessageOnSD.
			autoTryToFixSourceCodeFileReferences = cbAutoTryToFixSourceCodeFileReferences.Checked;
        }

        private void btShowHideCompilationErrors_Click(object sender, EventArgs e)
        {
            tvCompilationErrors.Visible = !tvCompilationErrors.Visible;
        }

        private void tbShowO2ObjectModel_Click(object sender, EventArgs e)
        {
            openO2ObjectModel();
        }

        private void compileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            compile_Click(null, null);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tecSourceCode.ActiveTextAreaControl.TextArea.ClipboardHandler.Copy(null, null);
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tecSourceCode.ActiveTextAreaControl.TextArea.ClipboardHandler.Paste(null, null);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveSourceCode();
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFile();
        }

        private void btOpenFile_Click(object sender, EventArgs e)
        {
            openFile();
        }

        private void cboxCompliledSourceCodeMethods_Click(object sender, EventArgs e)
        {

        }

        private void autoCompileEvery10SecondsToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            setAutoCompileStatus(autoCompileEvery10SecondsToolStripMenuItem.Checked);
        }

        private void executeSelectedMethodToolStripMenuItem_Click(object sender, EventArgs e)
        {
            O2Thread.mtaThread(executeMethod);
        }

        private void addBreakpointOnCurrentLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addBreakpointOnCurrentLine();
        }

        private void btDebugMethod_Click(object sender, EventArgs e)
        {
            createStandAloneExeAndDebugMethod("");
        }

        private void cboxCompliledSourceCodeMethods_TextChanged(object sender, EventArgs e)
        {

        }

        private void cboxCompliledSourceCodeMethods_SelectedIndexChanged(object sender, EventArgs e)
        {   
            // this was causing a lot of messages when SourceCodeEditor was used without debugger
            //  setDebugButtonEnableState();
        }

        private void menuStripForSourceEdition_Opening(object sender, CancelEventArgs e)
        {
            //addBreakpointOnCurrentLineToolStripMenuItem.Visible = O2Messages.isDebuggerAvailable();
        }

        private void tvCompilationErrors_AfterSelect(object sender, TreeViewEventArgs e)
        {
            showSelectedErrorOnSourceCodeFile();
        }       

        private void listinLogViewCurrentAssemblyRefernecesAutomaticallyAddedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listinLogViewCurrentAssemblyReferencesAutomaticallyAdded();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [Bindable(true)]
        public bool _ShowSearchAndAstDetails
        {
            set
            {
                // the idea is to skip the creation of the Ast_CSharp_ShowDetailsInViewer unless the user choses to view it
                if (showAstDetails != null || value != false)
                {
                    // create it, if this is the first time we use showAstDetails
                    if (showAstDetails == null)
                    {
                        showAstDetails = new Ast_CSharp_ShowDetailsInViewer(tecSourceCode, tcSourceInfo);
                        showAstDetails.update();
                    }

                    showAstDetails.enabled = value;

                    this.invokeOnThread(() => scCodeAndAst.Panel2Collapsed = !value);
                }
            }

            get
            {
                return scCodeAndAst.Panel2Collapsed;
            }
        }
            

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [Bindable(true)]
        public bool _ShowTopMenu
        {
            set
            {
                toolStripWithSourceCodeActions.Visible = value;

                scCodeAndAst.fill();

                //this was causing the nasty error of not having scroll bars in the Script REPL
                /*if (toolStripWithSourceCodeActions.Visible)
                {
                    scCodeAndAst.Top = 30;
                    scCodeAndAst.Height = Height - scCodeAndAst.Top;
                    //tecSourceCode.Dock = DockStyle.None;
                    //tecSourceCode.Top = 30;
                    //tecSourceCode.Left = 0;
                    //tecSourceCode.Height = scCodeAndAst.Panel1.Height - tecSourceCode.Top;
                    //tecSourceCode.Width = scCodeAndAst.Panel1.Width - tecSourceCode.Left;
                }
                else
                {
                    scCodeAndAst.Top = 0;
                    scCodeAndAst.Height = Height;
                    //tecSourceCode.Dock = DockStyle.Fill;
                }*/
            }

            get
            {
                return toolStripWithSourceCodeActions.Visible;
            }
        }

        private void lbFileLoaded_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(sPathToFileLoaded);
        }

        private void toolStripSeparator6_Click(object sender, EventArgs e)
        {

        }

        private void btSeachAndViewAst_Click(object sender, EventArgs e)
        {
            scCodeAndAst.Panel2Collapsed = !scCodeAndAst.Panel2Collapsed;
            _ShowSearchAndAstDetails = ! scCodeAndAst.Panel2Collapsed;
        }

        private void llPutFilePathInClipboard_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Clipboard.SetText(sPathToFileLoaded);
        }

        private void enableCodeCompleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            enableCodeComplete();
        }

        private void enableOrDisableAutoBackupOnCompileSucessforCSharpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            autoBackUpOnCompileSuccess = !autoBackUpOnCompileSuccess;
        }

        private void tecSourceCode_MouseMove(object sender, MouseEventArgs e)
        {
            "tecSourceCode_MouseMove".info();
        }

        private void tecSourceCode_MouseClick(object sender, MouseEventArgs e)
        {
            "tecSourceCode_MouseClick".info();
        }

        private void tbExecutionHistoryOrLog_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            tbExecutionHistoryOrLog.Visible = false; 
        }

		private void clearCompileCacheToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CompileEngine.CachedCompiledAssemblies.Clear();			
		}

		private void clearLocalFileMappingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CompileEngine.clearLocalScriptFileMappings();
		}
        

        private void tstbTextSearch_KeyUp(object sender, KeyEventArgs e)
        {            
                
        }

        private void tstbTextSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                searchForTextInTextEditor_findNext(tstbTextSearch.Text);
                e.SuppressKeyPress = true;
            }
        }

    
    }
}
