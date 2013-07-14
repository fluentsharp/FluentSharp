using System;
using System.Windows.Forms;
using FluentSharp.WinForms;
using FluentSharp.CoreLib;
using FluentSharp.REPL.Controls;
using WeifenLuo.WinFormsUI.Docking;

namespace FluentSharp.REPL
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
        public static SourceCodeEditor add_DockContent_CodeEditor(this DockPanel dockPanel, string title)
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
        public static T           dock_Left<T>(this T control) where T : Control
        {
            WinFormsUI_ExtensionMethods_DockContent.dock_Left(control.dockContent());
            return control;
        }
        public static T           dock_Right<T>(this T control) where T : Control
        {
            WinFormsUI_ExtensionMethods_DockContent.dock_Right(control.dockContent());
            return control;
        }
        public static T           dock_Top<T>(this T control) where T : Control
        {
            WinFormsUI_ExtensionMethods_DockContent.dock_Top(control.dockContent());
            return control;
        }
        public static T           dock_Bottom<T>(this T control) where T : Control
        {
            WinFormsUI_ExtensionMethods_DockContent.dock_Bottom(control.dockContent());
            return control;
        }
    }

    public static class WinFormsUI_ExtensionMethods_DockPanel
    {   
        public static DockPanel dockPanel(this Control control)
        {
            return control.dockContent().DockPanel;
        }
    }

    public static class WinFormsUI_ExtensionMethods_GUI_Helpers
    {
        public static WebBrowser                add_Document_WebBrowser(this DockPanel dockPanel)
        {
            return dockPanel.dock_Middle<WebBrowser>("Web Browser");
        }
        public static ascx_Simple_Script_Editor add_Document_Script(this DockPanel dockPanel)
        {
            return dockPanel.dock_Middle("C# REPL Script").add_Script();
        }
        public static ascx_Simple_Script_Editor script_Me_in_Dock(this Control control)
        {
            return control.dockPanel().dock_Bottom("Script").add_Script_Me(control);
        }
        public static DockPanel                 open_Script_Viewer_GUI(this string startFolder)
        {            
            var winFormsUI = "Script Viewer".winFormsUI(1000, 600);
            winFormsUI.add_DockContent("Test Script").add_Script();
            winFormsUI.dock_Bottom("Log Viewer").add_LogViewer();

            Action<string> openFile =
                (file) =>
                {
                    winFormsUI.add_DockContent(file.fileName())
                              .add_Script()
                              .openFile(file);
                };
            Action<string> openFolder = null;

            var treeView = winFormsUI.dock_Left("Available Scripts").add_FolderViewer_Simple((file) => { }, ref openFolder, false);
            treeView.onDoubleClick<string>(openFile)
                    .add_ContextMenu()
                    .add_MenuItem("Open in Script Editor", true, () => openFile(treeView.selected().tag<string>()))
                    .add_MenuItem("New Script File",
                        () =>
                        {
                            var name = "What is the name of he new Script?".askUser();
                            if (name.valid())
                            {
                                var fileName = startFolder.pathCombine(name + ".h2");
                                "return \"Hello World\";".h2().saveAs(fileName);
                                openFile(fileName);
                                openFolder(startFolder);
                            }
                        });                     
            winFormsUI.splitterDistance(400);
            openFolder(startFolder);   
            return winFormsUI;
        }
    }
}
