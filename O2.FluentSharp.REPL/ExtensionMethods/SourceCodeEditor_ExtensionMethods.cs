using System;
using System.Windows.Forms;
using FluentSharp.BCL;
using FluentSharp.BCL.Controls;
using FluentSharp.CoreLib.API;
using FluentSharp.REPL;
using FluentSharp.REPL.Utils;
using FluentSharp.SharpDevelop.Utils;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using FluentSharp.CoreLib;
using O2.External.SharpDevelop.Ascx;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using O2.External.SharpDevelop.ExtensionMethods;

namespace FluentSharp.SharpDevelop
{
    public static class SourceCodeEditor_TextEditor
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
        public static TextEditorControl _textEditor(this ascx_SourceCodeViewer sourceCodeViewer)
        {
            "here".info();
            return sourceCodeViewer._textEditorControl();
        }
        public static TextEditorControl _textEditorControl(this ascx_SourceCodeViewer sourceCodeViewer)
        {
            "here 22".info();
            return null;
            //return sourceCodeViewer._editor().getObject_TextEditorControl();
        }
    }

    public static class SourceCodeEditor_ExtensionMethods_Open
    {
        public static SourceCodeEditor     add_SourceCodeEditor(this GroupBox groupBox)
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
        public static SourceCodeEditor     add_SourceCodeEditor(this Control control)
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
        public static SourceCodeEditor     open_InCodeEditor   (this string fileOrCode)
        {
            return fileOrCode.showInCodeEditor();
        }
        public static SourceCodeEditor     showInCodeEditor    (this string fileOrCode)
        {
            if (fileOrCode.fileExists())
                return fileOrCode.showInCodeEditor(fileOrCode.extension());
            return fileOrCode.showInCodeEditor(".cs");
        }
        public static SourceCodeEditor     showInCodeEditor    (this string fileOrCode, string mapAsExtension)
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
        public static SourceCodeEditor     open        (this SourceCodeEditor sourceCodeEditor, string fileToOpen)
        {
            sourceCodeEditor.loadSourceCodeFile(fileToOpen);
            return sourceCodeEditor;
        }
        public static ascx_SourceCodeViewer     open        (this ascx_SourceCodeViewer sourceCodeViewer, string fileToOpen)
        {
            sourceCodeViewer.editor().loadSourceCodeFile(fileToOpen);
            return sourceCodeViewer;
        }
        public static ascx_SourceCodeViewer     open        (this ascx_SourceCodeViewer codeViewer, string file , int line)
		{
			codeViewer.editor().open(file, line);
			return codeViewer;
		}		
		public static SourceCodeEditor     open        (this SourceCodeEditor codeEditor, string file , int line)
		{
			codeEditor.open(file);
			codeEditor.gotoLine(line);
			return codeEditor;
		}
        
        public static ascx_SourceCodeViewer     load        (this ascx_SourceCodeViewer codeViewer, string fileOrCode)
        {
            codeViewer.editor().load(fileOrCode);
            return codeViewer;
        }
        public static SourceCodeEditor     load        (this SourceCodeEditor codeEditor, string fileOrCode)
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
        public static SourceCodeEditor     editScript  (this string scriptOrFile)
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
    }
    public static class SourceCodeEditor_ExtensionMethods_Misc
    {
        public static SourceCodeEditor     colorCodeForExtension(this SourceCodeEditor sourceCodeEditor, string extension)
        {
            return sourceCodeEditor.invokeOnThread(() =>
                {

                    var tecSourceCode = sourceCodeEditor.textEditorControl();
                    var dummyFileName = string.Format("aaa.{0}", extension);
                    tecSourceCode.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategyForFile(dummyFileName);
                    return sourceCodeEditor;
                });
        }        
                
        public static ascx_SourceCodeViewer     onClick(this ascx_SourceCodeViewer codeViewer, MethodInvoker callback)
        {
            codeViewer.editor().onClick(callback);
            return codeViewer;
        }
        public static SourceCodeEditor     onClick(this SourceCodeEditor codeEditor, MethodInvoker callback)
        {
            codeEditor.invokeOnThread(() => codeEditor.textArea().MouseClick += (sender, e) => callback());
            return codeEditor;
        }

        public static ascx_SourceCodeViewer     onTextChanged(this ascx_SourceCodeViewer sourceCodeViewer, Action<string> textChanged)
        {
            sourceCodeViewer.editor().onTextChanged(textChanged);
            return sourceCodeViewer;
        }
        public static SourceCodeEditor     onTextChanged(this SourceCodeEditor sourceCodeEditor, Action<string> textChanged)
        {
            sourceCodeEditor.eDocumentDataChanged += textChanged;
            return sourceCodeEditor;
        }
        public static SourceCodeEditor     onCompile(this SourceCodeEditor sourceCodeEditor, Action<Assembly> onCompile)
        {
            sourceCodeEditor.editor().eCompile += onCompile;
            return sourceCodeEditor;
        }
        public static SourceCodeEditor     onFileOpen(this SourceCodeEditor sourceCodeEditor, Action<string> onFileOpen)
        {
            sourceCodeEditor.editor().eFileOpen += onFileOpen;
            return sourceCodeEditor;
        }

        public static SourceCodeEditor     editor(this SourceCodeEditor sourceCodeEditor)
        {
            return sourceCodeEditor;
        }
        public static SourceCodeEditor     editor(this ascx_SourceCodeViewer sourceCodeViewer)
        {
            return sourceCodeViewer.getSourceCodeEditor();
        }

        public static ascx_SourceCodeViewer     allowCompile(this ascx_SourceCodeViewer sourceCodeViewer, bool value)
        {
            sourceCodeViewer.editor().allowCompile(value);
            return sourceCodeViewer;
        }
        public static SourceCodeEditor     allowCompile(this SourceCodeEditor sourceCodeEditor, bool value)
        {
            sourceCodeEditor.allowCodeCompilation = value;
            return sourceCodeEditor;
        }
        public static ascx_SourceCodeViewer     astDetails(this ascx_SourceCodeViewer sourceCodeViewer, bool value)
        {
            sourceCodeViewer.editor().astDetails(value);
            return sourceCodeViewer;
        }
        public static SourceCodeEditor     astDetails(this SourceCodeEditor sourceCodeEditor, bool value)
        {
            sourceCodeEditor.invokeOnThread(() =>
                {
                    sourceCodeEditor._ShowSearchAndAstDetails = value;
                });
            return sourceCodeEditor;
        }                

        public static SourceCodeEditor     vScroolBar_Enabled(this SourceCodeEditor sourceCodeEditor, bool value)
        {
            sourceCodeEditor.invokeOnThread(() =>
                {
                    sourceCodeEditor.getObject_TextEditorControl().ActiveTextAreaControl.VScrollBar.Enabled = value;
                });
            return sourceCodeEditor;
        }
        public static SourceCodeEditor     vScroolBar_Visible(this SourceCodeEditor sourceCodeEditor, bool value)
        {
            sourceCodeEditor.invokeOnThread(() =>
                {
                    sourceCodeEditor.getObject_TextEditorControl().ActiveTextAreaControl.VScrollBar.Visible = value;
                });
            return sourceCodeEditor;
        }
        public static SourceCodeEditor     hScroolBar_Enabled(this SourceCodeEditor sourceCodeEditor, bool value)
        {
            sourceCodeEditor.invokeOnThread(() =>
                {
                    sourceCodeEditor.getObject_TextEditorControl().ActiveTextAreaControl.HScrollBar.Enabled = value;
                });
            return sourceCodeEditor;
        }
        public static SourceCodeEditor     hScroolBar_Visible(this SourceCodeEditor sourceCodeEditor, bool value)
        {
            sourceCodeEditor.invokeOnThread(() =>
                {
                    sourceCodeEditor.getObject_TextEditorControl().ActiveTextAreaControl.HScrollBar.Visible = value;
                });
            return sourceCodeEditor;
        }
        public static ascx_SourceCodeViewer     set_ColorsForCSharp(this ascx_SourceCodeViewer sourceCodeViewer)
        {
            sourceCodeViewer.invokeOnThread(() => sourceCodeViewer.editor().setDocumentHighlightingStrategy("aa.cs"));
            return sourceCodeViewer;
        }
        public static SourceCodeEditor     set_ColorsForCSharp(this SourceCodeEditor sourceCodeEditor)
        {
            sourceCodeEditor.setDocumentHighlightingStrategy("aa.cs");
            return sourceCodeEditor;
        }        
        public static ascx_SourceCodeViewer     insert_Text(this ascx_SourceCodeViewer sourceCodeViewer, string text)
        {
            sourceCodeViewer.editor().insert_Text(text);
            return sourceCodeViewer;
        }
        public static SourceCodeEditor     insert_Text(this SourceCodeEditor sourceCodeEditor, string text)
        {
            var textArea = sourceCodeEditor.textEditorControl().textArea();
            return textArea.invokeOnThread(
                () =>
                    {
                        textArea.InsertString(text);
                        return sourceCodeEditor;
                    });
            
        }            
        public static ascx_SourceCodeViewer     set_Text(this ascx_SourceCodeViewer codeViewer, string text, string highlightForExtension)
        {
            codeViewer.editor().set_Text(text, highlightForExtension);
            return codeViewer;
        }
        public static SourceCodeEditor     set_Text(this SourceCodeEditor codeEditor, string text, string highlightForExtension)
        {
            if(codeEditor.notNull())
            {
                codeEditor.set_Text(text);
                codeEditor.setDocumentHighlightingStrategy(highlightForExtension);
            }
            return codeEditor;
        }
        public static ascx_SourceCodeViewer     set_Text(this ascx_SourceCodeViewer codeViewer, string text)
        {             
            return codeViewer.invokeOnThread(           
                () =>
                    {
                        codeViewer.setDocumentContents(text);
                        return codeViewer;
                    });
            
        }
        public static SourceCodeEditor     set_Text(this SourceCodeEditor codeEditor, string text)
        {
            if(codeEditor.notNull())
                codeEditor.setDocumentContents(text);
            return codeEditor;
        }        
        public static ascx_SourceCodeViewer     set_Code(this ascx_SourceCodeViewer codeViewer, string code)
        {
            return codeViewer.set_Text(code);

        }
        public static SourceCodeEditor     set_Code(this SourceCodeEditor codeEditor, string code)
        {
            return codeEditor.set_Text(code);
        }        
        
        public static SourceCodeEditor     caret(this SourceCodeEditor codeEditor, int line, int column)
        {
            return codeEditor.caret(line, column, 3);
        }
        public static SourceCodeEditor     caret(this SourceCodeEditor codeEditor, int line, int column, int viewOffset)
        {
            return codeEditor.invokeOnThread(
                ()=>{
                        codeEditor.caret().Line = line - 1 + viewOffset;  // so that the selected line is not at the bottom of the screen
                        codeEditor.caret().Line = line - 1;
                        codeEditor.caret().Column = column - 1;
                        return codeEditor;
                    });
        }
        public static SourceCodeEditor     caret_Line(this SourceCodeEditor codeEditor, int value)
        {
            return codeEditor.caret_Line(value, 3);
        }
        public static SourceCodeEditor     caret_Line(this SourceCodeEditor codeEditor, int value, int viewOffset)
        {
            return codeEditor.invokeOnThread(
                ()=>{
                        codeEditor.caret().Line = value + viewOffset;  // so that the selected line is not at the bottom of the screen
                        codeEditor.caret().Line = value;
                        return codeEditor;
                    });
        }
        public static SourceCodeEditor     caret_Column(this SourceCodeEditor codeEditor, int value)
        {
            return codeEditor.caret_Column(value, 3);
        }
        public static SourceCodeEditor     caret_Column(this SourceCodeEditor codeEditor, int value, int viewOffset)
        {
            return codeEditor.invokeOnThread(
                ()=>{
                        codeEditor.caret().Column = value + viewOffset;  // so that the selected line is not at the bottom of the screen
                        codeEditor.caret().Column = value;
                        return codeEditor;
                    });
        }
        public static ascx_SourceCodeViewer     onCaretMove(this ascx_SourceCodeViewer codeViewer, Action<Caret> callback)
        {
            codeViewer.editor().onCaretMove(callback);
            return codeViewer;
        }
        public static SourceCodeEditor     onCaretMove(this SourceCodeEditor codeEditor, Action<Caret> callback)
        {
            codeEditor.textArea().Caret.PositionChanged += 
                (sender, e)=>{
                                if (codeEditor.notNull())
                                    callback(codeEditor.caret());
                             };
            return codeEditor;
        }
        public static string                    selectedText(this SourceCodeEditor codeEditor)
        {
            return codeEditor.invokeOnThread(() => codeEditor.textArea().SelectionManager.SelectedText);
        }
        public static SourceCodeEditor     setSelectionText(this SourceCodeEditor codeEditor, Location startLocation, Location endLocation)
        {
            return codeEditor.invokeOnThread(
                () =>{
                        var start = new TextLocation(startLocation.X - 1, startLocation.Y - 1);
                        var end = new TextLocation(endLocation.X - 1, endLocation.Y - 1);
                        var selection = new DefaultSelection(codeEditor.document(), start, end);
                        codeEditor.textArea().SelectionManager.SetSelection(selection);
                        codeEditor.caret_Line(start.Line);
                        codeEditor.caret_Column(start.Column);
                        return codeEditor;
                    });
        }
        public static SourceCodeEditor     selectTextWithColor(this SourceCodeEditor codeEditor, int startLine, int startColumn, int endLine, int endColumn)
        {
            return codeEditor.selectTextWithColor(new TextLocation(startColumn - 1, startLine - 1), new TextLocation(endColumn - 1, endLine - 1));
        }
        public static SourceCodeEditor     selectTextWithColor(this SourceCodeEditor codeEditor, TextLocation startLocation, TextLocation endLocation)
        {
            if (startLocation > endLocation)
            {
                "in SourceCodeEditor.selectTextWithColor startLocation > endLocation".error();
                return codeEditor;
            }
            if (startLocation.Column == -1 || startLocation.Line == -1 ||
                endLocation.Column == -1 || endLocation.Line == -1)
            {
                "in SourceCodeEditor.selectTextWithColor one of start or end location values was -1".error();
                return codeEditor;
            }
            return codeEditor.selectTextWithColor(new DefaultSelection(codeEditor.document(), startLocation, endLocation));
        }
        public static SourceCodeEditor     selectTextWithColor(this SourceCodeEditor codeEditor, DefaultSelection selection)
        {
            return codeEditor.selectTextWithColor(selection, TextMarkerType.SolidBlock, Color.LightBlue);
        }
        public static SourceCodeEditor     selectTextWithColor(this SourceCodeEditor codeEditor, DefaultSelection selection, TextMarkerType textMarkerType, Color color)
        {
            if (selection.Length < 0)
            {
                "in SourceCodeEditor.selectTextWithColor selection.Length was <  0".error();
                return codeEditor;
            }            
            return codeEditor.invokeOnThread(
                ()=>{                        
                        var newMarker = new TextMarker(
                            selection.Offset,
                            selection.Length,
                            textMarkerType, color);
                        codeEditor.document().MarkerStrategy.AddMarker(newMarker);
                        return codeEditor;
                    });
        }
        
        public static SourceCodeEditor     selectTextWithColor(this SourceCodeEditor codeEditor, INode node)
        {
            if (node.StartLocation.Column == 0 && node.StartLocation.Line == 0 && node.Parent != null && node != node.Parent)
                return codeEditor.selectTextWithColor(node.Parent);
            return codeEditor.selectTextWithColor(node.StartLocation.textLocation(), node.EndLocation.textLocation());
        }

        public static SourceCodeEditor     colorINodes(this SourceCodeEditor codeEditor, List<INode> nodes)
        {
            codeEditor.clearMarkers();
            foreach (var node in nodes)                            
                codeEditor.selectTextWithColor(node);            
            codeEditor.refresh();
            return codeEditor;
        }
        public static ascx_SourceCodeViewer     showFilesInSourceCodeViewer(this TreeView treeView)
        {
            var splitterDistance = (treeView.width() / 3) * 2;
            var sourceViewer = treeView.insert_Right<Panel>(splitterDistance).add_SourceCodeViewer();
            treeView.afterSelect<string>(
                (fileOrText) =>
                {
                    if (fileOrText.fileExists())
                        sourceViewer.open(fileOrText);
                    else
                        sourceViewer.editor().set_Text(fileOrText);
                });
            return sourceViewer;
        }
        public static SourceCodeEditor     _editor(this ascx_SourceCodeViewer sourceCodeViewer)
		{
			return sourceCodeViewer.getSourceCodeEditor();
		}


        public static ascx_SourceCodeViewer     maximizeViewer(this ascx_SourceCodeViewer codeViewer)
		{
			codeViewer.textEditorControl().fill().bringToFront();
			return codeViewer;
		}		
		public static ascx_SourceCodeViewer     onTextChange(this ascx_SourceCodeViewer codeViewer, Action<string> callback)
		{
			codeViewer.editor().onTextChange(callback);
			return codeViewer;
		}		
		public static SourceCodeEditor     onTextChange(this SourceCodeEditor codeEditor, Action<string> callback)
		{
			codeEditor.invokeOnThread(
				()=>{
						codeEditor.eDocumentDataChanged+= callback;
					});
			return codeEditor;
		}		
		
		public static ascx_SourceCodeViewer     gotoLine(this ascx_SourceCodeViewer codeViewer, int line)
		{
			codeViewer.editor().gotoLine(line);
			return codeViewer;
		}		
		public static ascx_SourceCodeViewer     gotoLine(this ascx_SourceCodeViewer codeViewer, int line, int showLinesBelow)
		{
			codeViewer.editor().gotoLine(line,showLinesBelow);
			return codeViewer;
		}		
		public static SourceCodeEditor     gotoLine(this SourceCodeEditor codeEditor, int line, int showLinesBelow)
		{
			if (line>0)
			{
				codeEditor.caret_Line(line,-showLinesBelow);			
				codeEditor.caret_Line(line,showLinesBelow);						
				codeEditor.gotoLine(line);
			}
			return codeEditor;
		}

        public static TextArea                  textArea(this ascx_SourceCodeViewer sourceCodeViewer)
        {
            return sourceCodeViewer.editor().textArea();
        }
        public static TextArea                  textArea(this SourceCodeEditor sourceCodeEditor)
        {
            return sourceCodeEditor.textEditorControl().ActiveTextAreaControl.TextArea;
        }

        public static IDocument                 document(this SourceCodeEditor sourceCodeEditor)
        {
            return sourceCodeEditor.invokeOnThread(() => sourceCodeEditor.getObject_TextEditorControl().Document);
        }
        public static IDocument                 document(this ascx_SourceCodeViewer sourceCodeViewer)
        {
            return sourceCodeViewer.editor().document();
        }
                
        public static string                    get_Text(this ascx_SourceCodeViewer sourceCodeViewer)
        {
            return sourceCodeViewer.editor().get_Text();
        }
        public static string                    get_Text(this SourceCodeEditor sourceCodeEditor)
        {
            return sourceCodeEditor.getSourceCode();
        }
        public static string                    get_Code(this ascx_SourceCodeViewer sourceCodeViewer)
        {
            return sourceCodeViewer.get_Text();
        }
        public static string                    get_Code(this SourceCodeEditor sourceCodeEditor)
        {
            return sourceCodeEditor.get_Text();
        }
        public static int                       caretLine(this SourceCodeEditor sourceCodeEditor)
        {
            return sourceCodeEditor.textEditorControl().ActiveTextAreaControl.Caret.Line;
        }
        public static Caret                     caret(this ascx_SourceCodeViewer codeViewer)
        {
            return codeViewer.editor().caret();
        }
        public static Caret                     caret(this SourceCodeEditor codeEditor)
        {
            return codeEditor.textArea().Caret;
        }
        public static TextLocation              textLocation(this Location location)
        {
            return new TextLocation(location.Column - 1, location.Line - 1);
        }

        public static SourceCodeEditor     clipboard_Copy(this SourceCodeEditor sourceCodeEditor)
        {
            sourceCodeEditor.invokeOnThread(()=> sourceCodeEditor.textArea().ClipboardHandler.Copy(null, null));
            return sourceCodeEditor;
        }
        public static SourceCodeEditor     clipboard_Cut(this SourceCodeEditor sourceCodeEditor)
        {
            sourceCodeEditor.invokeOnThread(()=> sourceCodeEditor.textArea().ClipboardHandler.Cut(null, null));
            return sourceCodeEditor;
        }
        public static SourceCodeEditor     clipboard_Paste(this SourceCodeEditor sourceCodeEditor)
        {
            sourceCodeEditor.invokeOnThread(()=> sourceCodeEditor.textArea().ClipboardHandler.Paste(null, null));
            return sourceCodeEditor;
        }


        public static Location location(this TextLocation textLocation)
        {
            return new Location(textLocation.X + 1, textLocation.Y + 1);
        }

        public static ascx_SourceCodeViewer selectText(this ascx_SourceCodeViewer codeViewer, int offsetStart, int offsetEnd)
        {
            codeViewer.editor().selectText(offsetStart, offsetEnd);
            return codeViewer;
        }

        public static SourceCodeEditor selectText(this SourceCodeEditor codeEditor, int offsetStart, int offsetEnd)
        {
            codeEditor.textEditor().invokeOnThread(
                () =>
                {
                    try
                    {
                        var position1 = codeEditor.document().OffsetToPosition(offsetStart).location();
                        var position2 = codeEditor.document().OffsetToPosition(offsetEnd).location();
                        codeEditor.setSelectionText(position1, position2);
                    }
                    catch (Exception ex)
                    {
                        ex.log("in SourceCodeEditor selectText");
                    }
                });
            return codeEditor;
        }
        public static ascx_SourceCodeViewer add_CodeMarker(this ascx_SourceCodeViewer codeViewer, int offsetStart, int offsetEnd)
        {
            codeViewer.editor().add_CodeMarker(offsetStart, offsetEnd);
            return codeViewer;
        }

        public static SourceCodeEditor add_CodeMarker(this SourceCodeEditor codeEditor, int offsetStart, int offsetEnd)
        {
            codeEditor.textEditor().invokeOnThread(
                () =>
                {
                    var position1 = codeEditor.document().OffsetToPosition(offsetStart);
                    var position2 = codeEditor.document().OffsetToPosition(offsetEnd);
                    codeEditor.clearMarkers();
                    codeEditor.selectTextWithColor(position1, position2)
                                          .refresh();
                    codeEditor.document().MarkerStrategy.TextMarker.first().field("color", Color.Azure);
                });
            return codeEditor;
        }

        public static ascx_SourceCodeViewer markerColor(this ascx_SourceCodeViewer codeViewer, Color color)
        {
            codeViewer.editor().markerColor(color);
            return codeViewer;
        }

        public static SourceCodeEditor markerColor(this SourceCodeEditor codeEditor, Color color)
        {
            codeEditor.textEditor().invokeOnThread(
                () =>
                {
                    foreach (var marker in codeEditor.document().MarkerStrategy.TextMarker)
                        marker.field("color", color);
                });
            return codeEditor;
        }
        
    }

    public static class SourceCodeEditor_ExtensionMethods_WinForm_Guis
    {
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

    public static class SourceCodeEditor_ExtensionMethods_CodeComplete
    { 
        public static ascx_SourceCodeViewer enableCodeComplete(this ascx_SourceCodeViewer sourceCodeViewer)
        {
            sourceCodeViewer.editor().enableCodeComplete();
            return sourceCodeViewer;
        }
        public static O2CodeCompletion      updateCodeComplete(this ascx_SourceCodeViewer sourceCodeViewer, CSharp_FastCompiler csharpFastCompiler)
        { 
            return sourceCodeViewer.editor().updateCodeComplete(csharpFastCompiler);
        }
        public static O2CodeCompletion      updateCodeComplete(this SourceCodeEditor sourceCodeEditor, CSharp_FastCompiler csharpFastCompiler)
        {
            if (sourceCodeEditor.o2CodeCompletion != null)
            {
                foreach (var extraReference in csharpFastCompiler.ExtraSourceCodeFilesToCompile)
                    sourceCodeEditor.o2CodeCompletion.parseFile(extraReference);
                //var currentCode = csharpFastCompiler.processedCode();
               var currentCode = csharpFastCompiler.SourceCode;
               sourceCodeEditor.o2CodeCompletion.parseSourceCode(currentCode);
               sourceCodeEditor.o2CodeCompletion.CodeCompleteCaretLocationOffset = csharpFastCompiler.getGeneratedSourceCodeMethodLineOffset();
                
                sourceCodeEditor.o2CodeCompletion.CodeCompleteTargetText = currentCode;
                // i might not need these
                sourceCodeEditor.textArea().CodeCompleteCaretLocationOffset = csharpFastCompiler.getGeneratedSourceCodeMethodLineOffset();

            }
            return sourceCodeEditor.o2CodeCompletion;
        }
        // this wasn't working as expected
        /*public static void enableCodeComplete(this SourceCodeEditor sourceCodeEditor, SourceCodeEditor sourceCodeEditorToGrabCodeFrom)
        {
            var o2CodeComplete = sourceCodeEditor.enableCodeComplete();
            o2CodeComplete.textEditorToGrabCodeFrom = sourceCodeEditorToGrabCodeFrom.getObject_TextEditorControl();
        }
        public static void enableCodeComplete(this ascx_SourceCodeViewer sourceCodeViewer, ascx_SourceCodeViewer sourceCodeViewerToGrabCodeFrom)
        {
            sourceCodeViewer.editor().enableCodeComplete(sourceCodeViewerToGrabCodeFrom.editor());
        }*/

    }

    public static class WinForms_ExtensionMethods_MDIForms_O2Controls_CodeEditor
    {
        public static ascx_LogViewer add_Mdi_LogViewer(this Form parentForm)
        {
            return parentForm.add_MdiChild()
                             .add_LogViewer();
        }

        public static ascx_Simple_Script_Editor add_Mdi_ScriptEditor(this Form parentForm)
        {
            return parentForm.add_MdiChild()
                             .add_Script();
        }
    }
}