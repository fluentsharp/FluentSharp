using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using O2.DotNetWrappers.ExtensionMethods;
using O2.External.SharpDevelop.ExtensionMethods;
using O2.Kernel.CodeUtils;
using System.Drawing;
using O2.Views.ASCX.CoreControls;
using O2.XRules.Database.Utils;
using O2.Core.XRules.Ascx;
using O2.Views.ASCX.classes.MainGUI;
using O2.DotNetWrappers.DotNet;
using System.Threading;

//O2File:ExtensionMethods/Reflection_ExtensionMethods.cs
//O2File:ExtensionMethods/Views_ExtensionMethods.cs
//O2File:CodeUtils/O2Kernel_Files.cs
//O2File:show.cs

namespace O2.Kernel
{
    public class open
    {
        public static Control directory()
        {
            return "Directory Viewer".popupWindow(300,300)    
                                    .add_Control< ascx_Directory>()
                                    .simpleMode_withAddressBar();
            /*var directory = viewAscx("ascx_Directory");
            directory.invoke("simpleMode_withAddressBar");
            return directory;*/
        }

        public static Control directory(string startDir)
        {
            return directory(startDir, true);
        }

        public static Control directory(string startDir, bool watchFolder)
        {
            var control = directory();
            control.invoke("openDirectory", startDir);
            control.prop("_WatchFolder", watchFolder);
            return control;
        }
        public static TextBox file(string fileToView)
        {
            var title = "Text file: " + fileToView;
            return title.popupWindow(800, 400)
                        .add_TextBox()
                        .set_Text(O2Kernel_Files.getFileContents(fileToView));            
        }

        public static RichTextBox document(string fileToView)
        {
            var title = "RTF file: " + fileToView;
            return title.popupWindow(800, 400)
                        .add_RichTextBox()
                        .set_Rtf(O2Kernel_Files.getFileContents(fileToView));
        }        


        public static PictureBox image(string imageToLoad)
        {
            var title = "image: " + imageToLoad;
            return title.popupWindow(800, 400)
                        .add_PictureBox()
                        .load(imageToLoad);
        }

        public static object web()
        {
            return webBrowser("");
        }

        public static object web(string url)
        {
            return webBrowser(url);
        }

        public static object link(string url)
        {
            return webBrowser(url);
        }

        public static object browser()
        {
            return webBrowser("");
        }

        public static object browser(string url)
        {
            return webBrowser(url);
        }

        public static object webBrowser()
        {
            return webBrowser("");
        }

        public static object webBrowser(string url)
        {
            var browser = "O2_External_IE.dll".type("O2BrowserIE").invokeStatic("openAsForm");
            if (url.valid())
                return browser.invoke("openSync", url);
            return browser;
        }

        public static ascx_O2ObjectModel o2ObjectModel()
        {
            return "O2 Object Model".popupWindow(500, 400)
                                    .add_Control<ascx_O2ObjectModel>();

			//var graphControlType = "O2_FluentSharp_BCL.dll".type("ascx_O2ObjectModel");
            //return graphControlType.openControlAsForm("O2 Object Model", 500, 400);
        
        }

        public static ascx_Panel_With_Inspector scriptEditor()
        {
            return ascx_Panel_With_Inspector.runControl();
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

        /*
        public static Control viewAscx(string controlName)
        {
            return controlName.viewsAscxControl();
        }

        public static Control viewAscx(string controlName, string title, int width, int height)
        {
            return controlName.viewsAscxControl(title, width, height);
        }

        public static T asForm<T>() where T : Control
        {
            return typeof(T).openControlAsForm<T>(typeof(T).Name,500,400);
        }*/
    }
}
