using System.Windows.Forms;
using FluentSharp.WinForms;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.REPL.Controls;
using FluentSharp.WinForms.Controls;
using System.Threading;

//O2File:ExtensionMethods/Reflection_ExtensionMethods.cs
//O2File:ExtensionMethods/Views_ExtensionMethods.cs
//O2File:CodeUtils/O2Kernel_Files.cs
//O2File:show.cs

namespace FluentSharp.REPL.Utils
{
    public class open
    {
        public static SourceCodeEditor codeEditor()
        {
            return codeEditor("");
        }
        public static SourceCodeEditor codeEditor(string fileToOpen)
        {
            return fileToOpen.open_InCodeEditor();
        }
        public static Control               directory()
        {
            return "Directory Viewer".popupWindow(300,300)    
                                    .add_Control< DirectoryViewer>()
                                    .simpleMode_withAddressBar();
        }
        public static Control               directory(string startDir)
        {
            return directory(startDir, true);
        }
        public static Control               directory(string startDir, bool watchFolder)
        {
            var control = directory();
            control.invoke("openDirectory", startDir);
            control.prop("_WatchFolder", watchFolder);
            return control;
        }
        public static TextBox               file(string fileToView)
        {
            var title = "Text file: " + fileToView;
            return title.popupWindow(800, 400)
                        .add_TextBox()
                        .set_Text(O2Kernel_Files.getFileContents(fileToView));            
        }
        public static RichTextBox           document(string fileToView)
        {
            var title = "RTF file: " + fileToView;
            return title.popupWindow(800, 400)
                        .add_RichTextBox()
                        .set_Rtf(O2Kernel_Files.getFileContents(fileToView));
        }        
        public static PictureBox            image(string imageToLoad)
        {
            var title = "image: " + imageToLoad;
            return title.popupWindow(800, 400)
                        .add_PictureBox()
                        .load(imageToLoad);
        }
        public static WebBrowser            web()
        {
            return webBrowser("");
        }
        public static WebBrowser            web(string url)
        {
            return webBrowser(url);
        }
        public static object                link(string url)
        {
            return webBrowser(url);
        }
        public static WebBrowser            browser()
        {
            return webBrowser("");
        }
        public static WebBrowser            browser(string url)
        {
            return webBrowser(url);
        }
        public static WebBrowser            webBrowser()
        {
            return webBrowser("");
        }
        public static WebBrowser            webBrowser(string url)
        {
			var browser = "Web Browser for: {0}".format(url).popupWindow().add_WebBrowser_with_NavigationBar();
			browser.open_ASync(url);
			return browser;
        }
        public static O2ObjectModel    o2ObjectModel()
        {
            return "O2 Object Model".popupWindow(500, 400)
                                    .add_Control<O2ObjectModel>();        
        }
        public static ascx_Panel_With_Inspector csharpREPL()
        {
            return ascx_Panel_With_Inspector.runControl();
        }
        public static ascx_Panel_With_Inspector scriptEditor()
        {
            return ascx_Panel_With_Inspector.runControl();
        }
        public static ascx_Panel_With_Inspector scriptEditor(string file)
        {
            return ascx_Panel_With_Inspector.runControl(file);
        }        
        public static Thread scriptEditor_MtaThread()
        {
            return O2Thread.mtaThread(() => scriptEditor());
        }
        public static ascx_XRules_Editor devEnvironment()
        {
            return O2Gui.open<ascx_XRules_Editor>("O2 Development Environment",1024, 600);
        }
        public static Thread devEnvironment_MtaThread()
        {
            return O2Thread.mtaThread(() => devEnvironment());
        }
        public static ascx_LogViewer logViewer()
        {

            return O2Gui.open<ascx_LogViewer>();

        }
    }
}
