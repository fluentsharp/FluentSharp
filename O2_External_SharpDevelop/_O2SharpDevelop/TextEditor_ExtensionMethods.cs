using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.TextEditor;

using O2.DotNetWrappers.DotNet;
using O2.DotNetWrappers.ExtensionMethods;
using O2.External.SharpDevelop.Ascx;
using O2.Kernel;
using ICSharpCode.TextEditor.Document;

namespace O2.External.SharpDevelop.ExtensionMethods
{
    public static class TextEditor_ExtensionMethods
    {
        
        public static TextArea          textArea        (this TextEditorControl textEditorControl)
        {
            return textEditorControl.ActiveTextAreaControl.TextArea;
        }
        public static string            get_Text        (this TextArea textArea)
        {
            return (string)textArea.invokeOnThread(() => { return textArea.Document.TextContent; });
        }        
        public static string            get_Text        (this TextEditorControl textEditorControl)
        {            
            return (string)textEditorControl.textArea().invokeOnThread(
                () =>
                {
                    var text = textEditorControl.Text; ;//textEditorControl.textArea().Text
                    return text;
                });
        }
        public static string            get_Text        (this TextEditorControl textEditorControl, int offset, int lenght)
        {
            return (string)textEditorControl.textArea().invokeOnThread(
                () =>
                {
                    var text = textEditorControl.textArea().Document.GetText(offset, lenght);
                    return text;
                });
        }
        public static int               currentOffset   (this TextEditorControl textEditorControl)
        {
            return (int)textEditorControl.textArea().invokeOnThread(
                () =>
                {
                    return textEditorControl.textArea().Caret.Offset;                    
                });
        }

        public static TextEditorControl     open            (this TextEditorControl textEditorControl, string sourceCodeFile)
        {
            return (TextEditorControl)textEditorControl.invokeOnThread(
                () =>
                {
                    if (sourceCodeFile.fileExists())
                        textEditorControl.LoadFile(sourceCodeFile);
                    else
                    {
                        textEditorControl.SetHighlighting("C#");
                        textEditorControl.Document.TextContent = sourceCodeFile;
                    }
                    return textEditorControl;
                });
        }     
        public static TextEditorControl     insertTextAtCurrentCaretLocation(this TextEditorControl textEditorControl, string textToInsert)
        {
            return (TextEditorControl)textEditorControl.textArea().invokeOnThread(
                () =>
                {
                    var caret = textEditorControl.textArea().Caret;
                    textEditorControl.ActiveTextAreaControl.Document.Insert(caret.Offset, textToInsert);
                    //caret.Offset += textToInsert.Length;
                    return textEditorControl;
                });
        }
        public static TextEditorControl     showLineNumbers (this TextEditorControl textEditorControl, bool value)
        {
            return (TextEditorControl)textEditorControl.textArea().invokeOnThread(
                () =>
                {
                    textEditorControl.ShowLineNumbers = value;
                    return textEditorControl;
                });
        }
        public static TextEditorControl     showTabs        (this TextEditorControl textEditorControl, bool value)
        {
            return (TextEditorControl)textEditorControl.textArea().invokeOnThread(
                () =>
                {
                    textEditorControl.ShowTabs = value;
                    return textEditorControl;
                });
        }
        public static TextEditorControl     showSpaces      (this TextEditorControl textEditorControl, bool value)
        {
            return (TextEditorControl)textEditorControl.textArea().invokeOnThread(
                () =>
                {
                    textEditorControl.ShowSpaces = value;
                    return textEditorControl;
                });
        }
        public static TextEditorControl     showInvalidLines(this TextEditorControl textEditorControl, bool value)
        {
            return (TextEditorControl)textEditorControl.textArea().invokeOnThread(
                () =>
                {
                    textEditorControl.ShowInvalidLines = value;
                    return textEditorControl;
                });
        }
        public static TextEditorControl     showEOLMarkers  (this TextEditorControl textEditorControl, bool value)
        {
            return (TextEditorControl)textEditorControl.textArea().invokeOnThread(
                () =>
                {
                    textEditorControl.ShowEOLMarkers = value;
                    return textEditorControl;
                });
        }
        public static TextEditorControl     showHRuler      (this TextEditorControl textEditorControl, bool value)
        {
            return (TextEditorControl)textEditorControl.textArea().invokeOnThread(
                () =>
                {
                    textEditorControl.ShowHRuler = value;
                    return textEditorControl;
                });
        }
        public static TextEditorControl     showVRuler      (this TextEditorControl textEditorControl, bool value)
        {
            return (TextEditorControl)textEditorControl.textArea().invokeOnThread(
                () =>
                {
                    textEditorControl.ShowVRuler = value;
                    return textEditorControl;
                });
        }

    }
}
