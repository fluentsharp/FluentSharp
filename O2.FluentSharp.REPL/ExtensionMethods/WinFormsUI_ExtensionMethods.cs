using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using O2.External.SharpDevelop.Ascx;
using O2.External.SharpDevelop.ExtensionMethods;


namespace O2.DotNetWrappers.ExtensionMethods
{    
    public static class WinFormsUI_ExtensionMethods
    {
        public static DockPanel winFormsUI(this string text)
        {
            return text.winFormsUI(400, 400);
        }
        public static DockPanel winFormsUI(this string text, int width, int height)
        {
            var topPanel = text.mdiForm().width(width).height(height);
            return topPanel.add_Control<DockPanel>();
        }
    }

    public static class WinFormsUI_ExtensionMethods_DockContent_Add
    {
        
        public static DockContent add_DockContent(this DockPanel dockPanel)
        {
            return dockPanel.add_DockContent("");
        }
        public static DockContent add_DockContent(this DockPanel dockPanel, string text)
        {
            return dockPanel.add_DockContent(text, DockState.Document);
        }
        public static DockContent add_DockContent(this DockPanel dockPanel, string title, DockState dockState)
        {
            return (DockContent)dockPanel.invokeOnThread(
                () =>
                {
                    var dockContent = new DockContent();
                    dockContent.Width = 200;
                    dockContent.ShowHint = dockState;
                    dockContent.Text = title;
                    dockContent.Show(dockPanel);
                    return dockContent;
                });
        }
        public static DockContent add_DockContent_Top(this DockPanel dockPanel, string title)
        {
            return dockPanel.add_DockContent(title, DockState.DockTop);
        }
        public static DockContent add_DockContent_Bottom(this DockPanel dockPanel, string title)
        {
            return dockPanel.add_DockContent(title, DockState.DockBottom);
        }
        public static DockContent add_DockContent_Left(this DockPanel dockPanel, string title)
        {
            return dockPanel.add_DockContent(title, DockState.DockLeft);
        }
        public static DockContent add_DockContent_Right(this DockPanel dockPanel, string title)
        {
            return dockPanel.add_DockContent(title, DockState.DockRight);
        }
        
        public static Panel dock_Top(this DockPanel dockPanel, string title)
        {
            return dockPanel.dock_Top<Panel>(title);
        }
        public static Panel dock_Bottom(this DockPanel dockPanel, string title)
        {
            return dockPanel.dock_Bottom<Panel>(title);
        }
        public static Panel dock_Left(this DockPanel dockPanel, string title)
        {
            return dockPanel.dock_Left<Panel>(title);
        }
        public static Panel dock_Right(this DockPanel dockPanel, string title)
        {
            return dockPanel.dock_Top<Panel>(title);
        }
        public static Panel dock_Middle(this DockPanel dockPanel, string title)
        {
            return dockPanel.dock_Middle<Panel>(title);
        }

        public static T add_DockContent<T>(this DockPanel dockPanel, string text) where T : Control
        {
            return dockPanel.add_DockContent(text, DockState.Document).add_Control<T>();
        }
        public static T dock_Top<T>(this DockPanel dockPanel, string title) where T : Control
        {
            return dockPanel.add_DockContent(title, DockState.DockTop).add_Control<T>();
        }
        public static T dock_Bottom<T>(this DockPanel dockPanel, string title) where T : Control
        {
            return dockPanel.add_DockContent(title, DockState.DockBottom).add_Control<T>();
        }
        public static T dock_Left<T>(this DockPanel dockPanel, string title) where T : Control
        {
            return dockPanel.add_DockContent(title, DockState.DockLeft).add_Control<T>();
        }
        public static T dock_Right<T>(this DockPanel dockPanel, string title) where T : Control
        {
            return dockPanel.add_DockContent(title, DockState.DockRight).add_Control<T>();
        }        

        public static T dock_Middle<T>(this DockPanel dockPanel, string title) where T : Control
        {
            return dockPanel.add_DockContent(title, DockState.Document).add_Control<T>();
        }
        public static TextBox add_DockContent_TextArea(this DockPanel dockPanel, string title)
        {
            return dockPanel.add_DockContent(title, DockState.Document).add_TextArea();
        }
        public static RichTextBox add_DockContent_RichTextBox(this DockPanel dockPanel, string title)
        {
            return dockPanel.add_DockContent(title, DockState.Document).add_RichTextBox();
        }
        public static ascx_SourceCodeEditor add_DockContent_CodeEditor(this DockPanel dockPanel, string title)
        {
            return dockPanel.add_DockContent(title, DockState.Document).add_SourceCodeEditor();
        }
        public static ascx_SourceCodeViewer add_DockContent_CodeViewer(this DockPanel dockPanel, string title)
        {
            return dockPanel.add_DockContent(title, DockState.Document).add_SourceCodeViewer();
        }
    }

    public static class WinFormsUI_ExtensionMethods_DockContent
    {
        public static DockContent dockContent(this Control control)
        {
            return control.parent<DockContent>();
        }
        public static T dock_Title<T>(this T control, string title) where T : Control
        {
            control.dockContent().title(title);
            return control;
        }
        public static DockContent dock_State(this DockContent dockContent)
        {
            dockContent.invokeOnThread(() => dockContent.ShowHint = DockState.Document);
            return dockContent;
        }
        public static DockContent title(this DockContent dockContent, string text)
        {
            return dockContent.invokeOnThread(
                () =>
                {
                    dockContent.Text = text;
                    return dockContent;
                });
        }
        public static DockContent dockTo(this DockContent dockContent, DockStyle dockStyle)
        {
            return dockContent.invokeOnThread(
                () =>
                {
                    dockContent.DockTo(dockContent.DockPanel, dockStyle);
                    return dockContent;
                });

        }
        public static DockContent dock_Fill(this DockContent dockContent)
        {
            return dockContent.dockTo(DockStyle.Fill);
        }
        public static DockContent dock_Top(this DockContent dockContent)
        {
            return dockContent.dockTo(DockStyle.Top);
        }
        public static DockContent dock_Bottom(this DockContent dockContent)
        {
            return dockContent.dockTo(DockStyle.Bottom);
        }
        public static DockContent dock_Left(this DockContent dockContent)
        {
            return dockContent.dockTo(DockStyle.Left);
        }
        public static DockContent dock_Right(this DockContent dockContent)
        {
            return dockContent.dockTo(DockStyle.Right);
        }
    }
}
