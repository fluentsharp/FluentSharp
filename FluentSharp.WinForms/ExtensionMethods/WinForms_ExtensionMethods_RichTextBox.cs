using System;
using System.Drawing;
using System.Windows.Forms;
using FluentSharp.CoreLib;

namespace FluentSharp.WinForms
{
    public static class WinForms_ExtensionMethods_RichTextBox
    { 
        public static RichTextBox add_RichTextBox(this Control control)
        {
            return control.add_RichTextBox("");
        }
        public static RichTextBox add_RichTextBox(this Control control, string text)
        {
            return (RichTextBox) control.invokeOnThread(
                ()=>{
                        var richTextBox = new RichTextBox();
                        richTextBox.Text = text;
                        richTextBox.Dock = DockStyle.Fill;
                        control.Controls.Add(richTextBox);
                        return richTextBox;
                });
        }
        public static RichTextBox set_Text(this RichTextBox richTextBox, string contents)
        {
            return (RichTextBox)richTextBox.invokeOnThread(
                () =>
                    {
                        richTextBox.SuspendLayout();
                        richTextBox.Text = contents;
                        richTextBox.ResumeLayout();
                        return richTextBox;
                    });
            
        }
        public static RichTextBox set_Rtf(this RichTextBox richTextBox, string contents)
        {
            return (RichTextBox)richTextBox.invokeOnThread(
                () =>
                    {
                        try
                        {
                            richTextBox.Rtf = contents;
                        }
                        catch
                        {
                            richTextBox.Text = contents;
                        }
                        return richTextBox;
                    });

        }
        public static RichTextBox append_Line(this RichTextBox richTextBox, string contents)
        {
            return (RichTextBox) richTextBox.invokeOnThread(
                () =>
                    {
                        richTextBox.append_Text(Environment.NewLine + contents);
                        return richTextBox;
                    });
            
        }
        public static RichTextBox append_Text(this RichTextBox richTextBox, string contents)
        {
            return (RichTextBox)richTextBox.invokeOnThread(
                () =>
                    {
                        if (contents!= null)
                            richTextBox.AppendText(contents);
                        return richTextBox;
                    });            
        }
        public static RichTextBox textColor(this RichTextBox richTextBox, Color color)
        {
            return (RichTextBox) richTextBox.invokeOnThread(
                () =>
                    {
                        richTextBox.ForeColor = color;
                        return richTextBox;
                    });
        }
        public static string get_Text(this RichTextBox richTextBox)
        {
            return (string)richTextBox.invokeOnThread(() => richTextBox.Text);
        }
        public static string get_Rtf(this RichTextBox richTextBox)
        {
            return (string)richTextBox.invokeOnThread(() => richTextBox.Rtf);
        }
        public static RichTextBox insertText(this RichTextBox richTextBox, string textToInsert)
        {

            return (RichTextBox)richTextBox.invokeOnThread(
                () =>
                    {
                        richTextBox.SelectionLength = 0;
                        richTextBox.SelectedText = textToInsert;
                        return richTextBox;
                    });
        }
        public static RichTextBox replaceText(this RichTextBox richTextBox, string textToFind, string textToInsert)
        {

            return (RichTextBox)richTextBox.invokeOnThread(
                () =>
                    {
                        var selectionStart = richTextBox.SelectionStart;
                        richTextBox.Rtf = richTextBox.Rtf.Replace(textToFind, textToInsert);
                        richTextBox.SelectionStart = selectionStart;            // put the cursor roughly about where it was
                        return richTextBox;
                    });
        }
        public static RichTextBox scrollToCaret(this RichTextBox richTextBox)
        {
            return (RichTextBox)richTextBox.invokeOnThread(
                ()=>{
                        richTextBox.ScrollToCaret();
                        return richTextBox;
                });						
        }		
        public static RichTextBox scrollToEnd(this RichTextBox richTextBox)
        {
            return (RichTextBox)richTextBox.invokeOnThread(
                ()=>{
                        richTextBox.SelectionStart = richTextBox.get_Text().size()-1;
                        richTextBox.ScrollToCaret();
                        return richTextBox;
                });						
        }
        public static RichTextBox scrollToStart(this RichTextBox richTextBox)
        {
            return (RichTextBox)richTextBox.invokeOnThread(
                ()=>{
                        richTextBox.SelectionStart = 0;
                        richTextBox.ScrollToCaret();
                        return richTextBox;
                });						
        }		
        public static RichTextBox wordWrap(this RichTextBox richTextBox, bool value)
        {
            return (RichTextBox)richTextBox.invokeOnThread(
                ()=>{
                        richTextBox.WordWrap = value;
                        return richTextBox;
                });						
        }		
        public static RichTextBox hideSelection(this RichTextBox richTextBox, bool value)
        {
            return (RichTextBox)richTextBox.invokeOnThread(
                ()=>{
                        richTextBox.HideSelection = value;
                        return richTextBox;
                });						
        }		
        public static RichTextBox hideSelection(this RichTextBox richTextBox)
        {
            return richTextBox.hideSelection(true);
        }		
        public static RichTextBox showSelection(this RichTextBox richTextBox)
        {
            return richTextBox.hideSelection(false);
        }	
    }
}