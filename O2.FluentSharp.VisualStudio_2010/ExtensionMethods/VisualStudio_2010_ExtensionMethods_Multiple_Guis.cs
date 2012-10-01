using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinForms = System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell.Interop;
using O2.DotNetWrappers.DotNet;
using O2.FluentSharp.VisualStudio;
using O2.DotNetWrappers.ExtensionMethods;

namespace O2.FluentSharp.VisualStudio.ExtensionMethods
{
	public static class VisualStudio_2010_ExtensionMethods_Multiple_Guis
	{
		//WinForms Guis
		public static T open_Control<T>(this VisualStudio_2010 visualStudio) where T : WinForms.Control
		{
			return visualStudio.open_Control<T>("{0}".format(typeof(T).Name));
		}
		public static T open_Control<T>(this VisualStudio_2010 visualStudio, string title) where T : WinForms.Control
		{
			var panel = title.create_WinForms_Window_Float();
			return panel.add_Control<T>();
		}
		public static WinForms.Panel open_Panel(this VisualStudio_2010 visualStudio, string title = "Title")
		{
			return visualStudio.open_Control<WinForms.Panel>(title);
		}
		public static WinForms.TreeView open_TreeView(this VisualStudio_2010 visualStudio)
		{
			return visualStudio.open_Control<WinForms.TreeView>();
		}

		//VisualStudio Guis
		public static IVsWindowFrame open_Document(this VisualStudio_2010 visualStudio, string file)
		{
			return file.open_Document();
		}
		public static Window open_WebBrowser(this VisualStudio_2010 visualStudio, string url)
		{
			return visualStudio.dte()
							  .ItemOperations
							  .Navigate(url, EnvDTE.vsNavigateOptions.vsNavigateOptionsNewWindow);		
		}
		
		//O2 Guis
		public static O2.Views.ASCX.Ascx.MainGUI.ascx_LogViewer open_LogViewer(this VisualStudio_2010 visualStudio)
		{
			var logViewer = "O2 LogViewer".create_WinForms_Window_Float().add_LogViewer();
			return logViewer;
		}
		public static O2.XRules.Database.Utils.ascx_Simple_Script_Editor open_ScriptEditor(this VisualStudio_2010 visualStudio)
		{
			return "C# REPL Script".create_WinForms_Window_Float(800, 400).add_Script(true);
		}
		public static O2.XRules.Database.Utils.ascx_Simple_Script_Editor open_ScriptEditor_With_VisualStudio_API(this VisualStudio_2010 visualStudio)
		{ 
			var defaultCode = 
@"var visualStudio = new VisualStudio_2010();
return visualStudio.dte();";
			return visualStudio.open_ScriptEditor().set_Code(defaultCode);
		}
		public static O2.XRules.Database.Utils.ascx_Simple_Script_Editor open_ScriptEditor_With_VisualStudio_CodeSample(this VisualStudio_2010 visualStudio)
		{
			var defaultCode =
@"//get a reference to the VisualStudio API
var visualStudio = new VisualStudio_2010();

//write an Error and Warning messages to the 'Error List' VisualStudio Window
visualStudio.errorList().add_Error(""I'm an Error"");
visualStudio.errorList().add_Warning(""I'm an Warning"");					    

//open a text file
visualStudio.open_Document(""a text file"".saveWithExtension("".exe""));

//open a C# file
visualStudio.open_Document(@""VS_Scripts\O2_Platform_Gui.cs"".local());

//open a WebBrowser
visualStudio.open_WebBrowser(@""http://www.google.com"");

//add a top Menu
visualStudio.dte().add_TopMenu(""A new Menu"")
				  .add_Menu_Button(""Ask me a question"", ()=> ""Hi {0}"".alert(""What is your name?"".askUser()));

//change the main title				  
visualStudio.mainWindow().title(visualStudio.mainWindow().title()  + "" - Now with REPL"");

//change the status bar
visualStudio.statusBar(""C# script example complete"");

//return the EnvDTE object
return visualStudio.dte();";
			return visualStudio.open_ScriptEditor().set_Code(defaultCode);
		}



	} 
}
