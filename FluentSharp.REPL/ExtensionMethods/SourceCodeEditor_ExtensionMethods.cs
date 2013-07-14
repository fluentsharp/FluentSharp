using System;
using System.Windows.Forms;
using FluentSharp.WinForms;
using FluentSharp.WinForms.Controls;
using FluentSharp.CSharpAST.Utils;
using FluentSharp.CoreLib.API;
using FluentSharp.REPL.Controls;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using FluentSharp.CoreLib;

namespace FluentSharp.REPL
{
    public static class SourceCodeEditor_ExtensionMethods
    {
        public static TextEditorControl textEditor(this SourceCodeEditor sourceCodeEditor)
        {
            return sourceCodeEditor.textEditorControl();
        }
        public static TextEditorControl textEditor(this ascx_SourceCodeViewer sourceCodeViewer)
        {
            return sourceCodeViewer.textEditorControl();
        }
        public static TextEditorControl textEditorControl(this SourceCodeEditor sourceCodeEditor)
        {
            return sourceCodeEditor.getObject_TextEditorControl();
        }
        public static TextEditorControl textEditorControl(this ascx_SourceCodeViewer sourceCodeViewer)
        {
            return sourceCodeViewer.editor().getObject_TextEditorControl();
        }
        public static TextEditorControl showAstValueInSourceCode(this TextEditorControl textEditorControl, AstValue<object> astValue)
        {
            return textEditorControl.invokeOnThread(
                () =>{
                         //PublicDI.log.error("{0} {1} - {2}", astValue.Text, astValue.StartLocation, astValue.EndLocation);

                         var start = new TextLocation(astValue.StartLocation.X - 1,
                                                      astValue.StartLocation.Y - 1);
                         var end = new TextLocation(astValue.EndLocation.X - 1, astValue.EndLocation.Y - 1);
                         var selection = new DefaultSelection(textEditorControl.Document, start, end);
                         textEditorControl.ActiveTextAreaControl.SelectionManager.SetSelection(selection);
                         setCaretToCurrentSelection(textEditorControl);
                         return textEditorControl;
                });
        }
        public static TextEditorControl setCaretToCurrentSelection(this TextEditorControl textEditorControl)
        {
            return textEditorControl.invokeOnThread(
                () =>{
                         var finalCaretPosition = textEditorControl.ActiveTextAreaControl.TextArea.SelectionManager.SelectionCollection[0].StartPosition;
                         var tempCaretPosition = new TextLocation {X = finalCaretPosition.X, Y = finalCaretPosition.Y + 10};
                         textEditorControl.ActiveTextAreaControl.Caret.Position = tempCaretPosition;
                         textEditorControl.ActiveTextAreaControl.TextArea.ScrollToCaret();
                         textEditorControl.ActiveTextAreaControl.Caret.Position = finalCaretPosition;
                         textEditorControl.ActiveTextAreaControl.TextArea.ScrollToCaret();
                         return textEditorControl;
                });
        }        
 
        public static SourceCodeEditor          add_SourceCodeEditor(this GroupBox groupBox)
        {
            return groupBox.invokeOnThread(
                () =>
                    {
                        var sourceCodeEditor = new SourceCodeEditor();
                        sourceCodeEditor.getObject_TextEditorControl().Document.
                                         FormattingStrategy =
                            new DefaultFormattingStrategy();
                        sourceCodeEditor.Dock = DockStyle.Fill;
                        groupBox.Controls.Add(sourceCodeEditor);
                        return sourceCodeEditor;
                    });
        }
        public static SourceCodeEditor          add_SourceCodeEditor(this Control control)
        {
            return control.invokeOnThread(
                () =>
                    {
                        var sourceCodeEditor = new SourceCodeEditor();
                        sourceCodeEditor.getObject_TextEditorControl().Document.
                                         FormattingStrategy = new DefaultFormattingStrategy();
                        sourceCodeEditor.Dock = DockStyle.Fill;
                        control.Controls.Add(sourceCodeEditor);
                        return sourceCodeEditor;
                    });
        }
        public static ascx_SourceCodeViewer     add_SourceCodeViewer(this Control control)
        {
            return (ascx_SourceCodeViewer) control.add_Control(typeof (ascx_SourceCodeViewer));
        }
        public static SourceCodeEditor          open_InCodeEditor   (this string fileOrCode)
        {
            return fileOrCode.showInCodeEditor();
        }
        public static SourceCodeEditor          showInCodeEditor    (this string fileOrCode)
        {
            if (fileOrCode.fileExists())
                return fileOrCode.showInCodeEditor(fileOrCode.extension());
            return fileOrCode.showInCodeEditor(".cs");
        }
        public static SourceCodeEditor          showInCodeEditor    (this string fileOrCode, string mapAsExtension)
        {
            var codeEditor = O2Gui.open<SourceCodeEditor>();
            if (fileOrCode.fileExists())
                codeEditor.open(fileOrCode);
            else
                codeEditor.setDocumentContents(fileOrCode);
            codeEditor.setDocumentHighlightingStrategy(mapAsExtension);
            return codeEditor;
        }
        public static ascx_SourceCodeViewer     showInCodeViewer    (this string fileOrCode)
        {
            if (fileOrCode.fileExists())
                return fileOrCode.showInCodeViewer(fileOrCode.extension());
            return fileOrCode.showInCodeViewer(".cs");
        }
        public static ascx_SourceCodeViewer     showInCodeViewer    (this string fileOrCode, string mapAsExtension)
        {
            var codeViewer = O2Gui.open<ascx_SourceCodeViewer>();
            if (fileOrCode.fileExists())
                codeViewer.open(fileOrCode);
            else
                codeViewer.setDocumentContents(fileOrCode);
            codeViewer.editor().setDocumentHighlightingStrategy(mapAsExtension);
            return codeViewer;
        }
        public static SourceCodeEditor          open                (this SourceCodeEditor sourceCodeEditor, string fileToOpen)
        {
            sourceCodeEditor.loadSourceCodeFile(fileToOpen);
            return sourceCodeEditor;
        }
        public static ascx_SourceCodeViewer     open                (this ascx_SourceCodeViewer sourceCodeViewer, string fileToOpen)
        {
            sourceCodeViewer.editor().loadSourceCodeFile(fileToOpen);
            return sourceCodeViewer;
        }
        public static ascx_SourceCodeViewer     open                (this ascx_SourceCodeViewer codeViewer, string file , int line)
		{
			codeViewer.editor().open(file, line);
			return codeViewer;
		}		
		public static SourceCodeEditor          open                (this SourceCodeEditor codeEditor, string file , int line)
		{
			codeEditor.open(file);
			codeEditor.gotoLine(line);
			return codeEditor;
		}        
        public static ascx_SourceCodeViewer     load                (this ascx_SourceCodeViewer codeViewer, string fileOrCode)
        {
            codeViewer.editor().load(fileOrCode);
            return codeViewer;
        }
        public static SourceCodeEditor          load                (this SourceCodeEditor codeEditor, string fileOrCode)
        {
            if (fileOrCode.fileExists())
            {
                codeEditor.open(fileOrCode);
            }
            else
            {
                codeEditor.set_ColorsForCSharp();
                codeEditor.setDocumentContents(fileOrCode);
            }
            return codeEditor;
        }
        public static SourceCodeEditor          editScript          (this string scriptOrFile)
        {
            if (scriptOrFile.fileExists().isFalse())
            {
                if (scriptOrFile.local().valid())
                    scriptOrFile = scriptOrFile.local();
                else
                {
                    var h2Script = new H2(scriptOrFile);
                    scriptOrFile = PublicDI.config.getTempFileInTempDirectory(".h2");
                    h2Script.save(scriptOrFile);
                }
            }
            return O2Gui.open<Panel>(scriptOrFile.fileName(), 800, 400)
                        .add_SourceCodeEditor()
                        .open(scriptOrFile);
        }    
        public static SourceCodeEditor showCodeEditorForFilesInFolder(this string path, string filter)		
		{	
			var topPanel = "Code Editor for {0} files in folder {1}".format(filter,path).popupWindow(900,400);
			var codeEditor = topPanel.add_GroupBox("SourceCode Editor")
									 .add_SourceCodeEditor();										 
									 
			var treeView = topPanel .insert_Left(200,"Files: {0}".format(filter))
									.add_TreeView()
									.onDrag<string>()					
									.afterSelect<string>((file)=>codeEditor.open(file));
			Action loadFiles = 
				()=>{
						treeView.clear();
						treeView.add_Files(path,filter)
								.selectFirst();
					};
			treeView.add_ContextMenu().add_MenuItem("reload files", ()=>loadFiles());			
			loadFiles();
			return codeEditor;		  
		}
    }

    public static class WinForms_ExtensionMethods_MDIForms_O2Controls_CodeEditor
    {        

        public static ascx_Simple_Script_Editor add_Mdi_ScriptEditor(this Form parentForm)
        {
            return parentForm.add_MdiChild()
                             .add_Script();
        }
    }
}