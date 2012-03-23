using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;
using EnvDTE80;
using System.Diagnostics;
using O2.Kernel.ExtensionMethods;
using O2.DotNetWrappers.ExtensionMethods;
using O2.FluentSharp.VisualStudio.ExtensionMethods;
using O2.Views.ASCX.ExtensionMethods;
using O2.Views.ASCX.Ascx.MainGUI;
using O2.Views.ASCX.classes.MainGUI;
using O2.DotNetWrappers.DotNet;
using O2.FluentSharp.VisualStudio.Commands;
using Microsoft.VisualStudio.CommandBars;
using O2.Platform;
using O2.Kernel;
using System.Windows.Forms;

//O2Ref:EnvDTE.dll
//O2Ref:EnvDTE80.dll
//O2Ref:Extensibility.dll
//O2Ref:Microsoft.VisualStudio.CommandBars.dll
//O2Ref:O2_FluentSharp_VisualStudio.dll

namespace O2.FluentSharp.VisualStudio
{
	public class O2_VS_AddIn
	{
		public DTE2		VS_Dte							 { get; set; }
		public AddIn	VS_AddIn						 { get; set; }

		public string	VS_Type							 { get; set; }

		public Dictionary<string, CommandBase>	Commands { get; set; }

		public O2_VS_AddIn()
		{
			Commands = new Dictionary<string, CommandBase>();


	//		CompileEngine.LocalReferenceFolders.Add(@"C:\_WorkDir\Git_O2OPlatform\O2.Platform.ReferenceAssemblies\O2_Assemblies");
		}

		public O2_VS_AddIn setup(DTE2 dte, AddIn addin, string vsType)
		{
			try
			{
				this.VS_Dte = dte;
				this.VS_AddIn = addin;
				this.VS_Type = vsType;

				//PublicDI.config.setLocalScriptsFolder(PublicDI.config.CurrentExecutableDirectory.pathCombine("O2.Platform.Scripts"));
				openO2LogViewer();

				//syncO2Repositories();
                "Testing Log viewer...".info();
				
				this.add_TopMenu("O2 Platform");


				this.add_Command<O2_ScriptWithPanel>()
					.add_Command<O2_ScriptGui>()
					.add_Command<O2_LogViewer>();
			}
			catch (Exception ex)
			{
				ex.log();
			}
			return this;
		}

		public void syncO2Repositories()
		{
			try
			{
/*				GitHub_Actions.TargetDir = PublicDI.config.CurrentExecutableDirectory;
				GitHub_Actions.LogMessage = (message) => "[GitHub Actions] {0}".info(message);

				GitHub_Actions.LogMessage("Target Dir: {0}".format(GitHub_Actions.TargetDir));
				GitHub_Actions.CloneRepository("O2.Platform.Scripts");*/
			}
			catch (Exception ex)
			{
				ex.log();
			}
		}

		public void openO2LogViewer()
		{
			O2Gui.open<Panel>("O2 Log Viewer", 400, 300).add_LogViewer();
		}

		public void add_Button()
		{
			try
			{
				Debug.Write("in AddButton");
				
				Debug.WriteLine("[O2.VisualStudio][OnConnection] uiSetUp ");
				
			}
			catch (Exception ex)
			{
				ex.log();
			}
		}		
	}

	public static class O2_VS_AddIn_ExtensionMethods_Commands
	{
		public static CommandBarPopup add_TopMenu(this O2_VS_AddIn o2AddIn, string caption)
		{
			var addAfterMenu = "Help"; //"Tools"
			var commandBars = (CommandBars)o2AddIn.VS_Dte.CommandBars;
			var menuCommandBar = commandBars["MenuBar"];
			var position = (commandBars[addAfterMenu].Parent as CommandBarControl).Index;
			position++;
			var newMenu = (CommandBarPopup)menuCommandBar.Controls.Add(MsoControlType.msoControlPopup, System.Type.Missing, System.Type.Missing, position, true);
			newMenu.Caption = caption;
			newMenu.Enabled = true;
			return newMenu;
		}

		public static O2_VS_AddIn add_Command<T>(this O2_VS_AddIn o2Addin)
			where T : CommandBase
		{
			var command = (T)typeof(T).ctor(o2Addin);		
			return o2Addin;
		}

		public static Panel add_WinForm_Panel(this O2_VS_AddIn o2Addin,string title, int width = -1 , int height = -1)
		{
			return o2Addin.VS_Dte.createWindowWithPanel(o2Addin.VS_AddIn, title, width, height);
		}

		public static Panel add_WinForm_Control_from_O2Script(this O2_VS_AddIn o2Addin, string title, string o2Script, string type, int width = -1, int height = -1)
		{
			var assembly = new CompileEngine().compileSourceFile(o2Script.local());
			var editorType = assembly.type(type);

			var panel = o2Addin.add_WinForm_Panel(title, width, height);
			panel.add_Control(editorType);
			return panel;
		}
	}

	public static class O2_VS_AddIn_ExtensionMethods_for_Connect_Calls
	{
		public static bool showCommand(this O2_VS_AddIn o2Addin, string commandName)
		{
	//		Debug.WriteLine("in showCommand: {0}".format(commandName));			
			return o2Addin.Commands.hasKey(commandName);
			//return (commandName == o2Addin.FullName);
		}

		public static bool executeCommand(this O2_VS_AddIn o2Addin, string commandName)
		{
			
			//var result = (commandName == o2Addin.FullName);
			if (o2Addin.Commands.hasKey(commandName))		
			{
				O2Thread.mtaThread(() => o2Addin.Commands[commandName].Execute()); ;
				return true;
				

			//	O2Gui.open<ascx_LogViewer>("Asp.Net O2 LogViewer", 400, 400);
		/*		O2Thread.mtaThread(
						()=>{
								CompileEngine.LocalReferenceFolders.Add(@"C:\_WorkDir\Git_O2OPlatform\O2.Platform\O2.VisualStudio.AddIn\bin\Debug");
								CompileEngine.LocalReferenceFolders.Add(@"C:\_WorkDir\Git_O2OPlatform\O2.Platform.ReferenceAssemblies\O2_Assemblies");
								"ascx_Simple_Script_Editor.cs.o2".local().compile().executeFirstMethod();


								"ascx_Quick_Development_GUI.cs.o2".local().compile().executeFirstMethod();   
						});*/
			}
			return false;
		}

	}

}
