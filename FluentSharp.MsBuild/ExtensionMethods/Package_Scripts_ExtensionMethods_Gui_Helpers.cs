using System;
using System.Drawing;
using System.Windows.Forms;
using FluentSharp.CoreLib;
using FluentSharp.WinForms;

namespace FluentSharp.MsBuild
{

    public static class Package_Scripts_ExtensionMethods_Gui_Helpers
    {
        public static TextBox showProjectBuildResult(this Control panel , string projectName , string targetDir, bool buildOk)
        {
            var logFile = targetDir.pathCombine(projectName + ".csproj.log");			
            return panel.clear().add_TextArea().wordWrap(false).set_Text(logFile.fileContents())
                .backColor(buildOk ? Color.LightGreen 
                    : Color.LightPink); 										      
        }
		
        public static T add_MenuItem_with_TestScripts<T>(this T control, Action<string> onItemSelected)
            where T : Control
        {
            control .add_ContextMenu()
                .add_MenuItem("Test with: LogViewer"								, true, ()=> onItemSelected("Util - LogViewer.h2"))
                .add_MenuItem("Test with: C# REPL Editor"							, true, ()=> onItemSelected("Util - C# REPL Script [4.0].h2"))
                .add_MenuItem("Test with: Package O2 Script into separate Folder"	, true, ()=> onItemSelected("Util - Package O2 Script into separate Folder.h2"))		
                .add_MenuItem("Test with: HtmlAgilityPack - Filter Html Code"		, true, ()=> onItemSelected("HtmlAgilityPack - Filter Html Code.h2"));							
                //.add_MenuItem("View available Scripts"								, true, ()=> "Util - O2 Available scripts.h2".executeFirstMethod());
            return control;					
        }
		
    }
}