using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using O2.Kernel;
using O2.DotNetWrappers.DotNet;
using O2.DotNetWrappers.ExtensionMethods;
using O2.External.SharpDevelop.ExtensionMethods;
using O2.FluentSharp.REPL;
//O2File:../ExtensionMethods\VisualStudio_2010_ExtensionMethods.cs
namespace O2.FluentSharp
{
    public class O2_Menus_In_VisualStudio
    {
        public void buildMenus()
        {
        	var o2Timer = new O2Timer("CreatedMenus in").start();
            "[O2_Menus_In_VisualStudio] building menus".debug();
            var dte = VisualStudio_2010.DTE2;
            var o2Menu = dte.add_TopMenu("Tools");  //shouldn't need to create since it is usually already there           
            var o2PlatformMenu = o2Menu.add_Menu_Button("O2 Platform");

            /*o2Menu.add_Menu_Button("C# REPL Script", () => this.openScriptEditor());
            o2Menu.add_Menu_Button("Log Viewer", () => this.openLogViewer());
            o2Menu.add_Menu_Button("Development Environment", () => open.devEnvironment());
            o2Menu.add_Menu_Button("Code Editor", () => this.openCodeEditor());
            o2Menu.add_Menu_Button("Create Dock Window", () => this.createDocWindow());                        
            o2Menu.add_Menu_Button("O2Script: Util - O2 Available scripts", ()=> "Util - O2 Available scripts.h2".local().executeH2Script());
            o2Menu.add_Menu_Button("O2Script: Main O2 Gui", () => "Main O2 Gui.h2".local().executeH2Script());
            o2Menu.add_Menu_Button("Update O2 Scripts (download from GitHub)", () => O2Scripts.downloadO2Scripts());
           */ 
            o2Timer.stop();
        }
        public void openLogViewer()
        {
            "O2 LogViewer".create_WinForms_Window_Float().add_LogViewer();                          
        }
        public void openCodeEditor()
        {
            "O2 Script Editor".create_WinForms_Window_Float().add_SourceCodeEditor();                          
        }
        public void openScriptEditor()
        {
            var script = "C# REPL Script".create_WinForms_Window_Float().add_Script(true);
            script.Code = 
@"var visualStudio = new VisualStudio_2010();

visualStudio.mainWindow().title(""Hello from O2""); 
visualStudio.create_WinForms_Window_Float(""Web Browser"")
			.add_WebBrowser().open(""http://www.google.com"");
			
return visualStudio; 

//O2"+@"File:ExtensionMethods\VisualStudio_2010_ExtensionMethods.cs";
        }
        public void createDocWindow()
        {
            var panel = "O2 Platform".create_WinForms_Window();
            panel.add_Panel().add_Link("C# REPL Script", 10, 10, () => this.openScriptEditor())
                             .append_Below_Link("Log Viewer", () => this.openLogViewer())
                             .append_Below_Link("Development Environment", () => open.devEnvironment())
                             .append_Below_Link("Code Editor", () => this.openCodeEditor())
                             .append_Below_Link("Add O2 Menu", () => this.buildMenus());                             
        }
    }
}
