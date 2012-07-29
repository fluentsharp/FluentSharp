using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using O2.FluentSharp.VisualStudio.ExtensionMethods;
using O2.Kernel.ExtensionMethods;
using O2.DotNetWrappers.ExtensionMethods;
using O2.FluentSharp.VisualStudio;
using EnvDTE;

//O2File:../classes/O2_VS_AddIn.cs
//O2File:../ExtensionMethods/Microsoft_VisualStudio_ExtensionMethods.cs
//O2Ref:EnvDTE.dll

namespace O2.FluentSharp.VisualStudio.Commands
{
	public class CommandBase
	{		
		public O2_VS_AddIn O2AddIn;

		public string	ButtonText	{ get; set; }
		public string	ToolTip		{ get; set; }
		public string	TargetMenu	{ get; set; }

		public string	Name		{ get; set; }		
		public bool		MSOButton	{ get; set; }
		public int		icon		{ get; set; }
		public string	FullName	{ get; set; }


		public Action	Execute		{ get; set; }

		public Command  Command		{ get; set; }

		public CommandBase() {}

		public CommandBase(O2_VS_AddIn o2AddIn)
		{
			O2AddIn = o2AddIn;
			Execute = () => { "no execution set for this command".info(); };
		}

		public virtual void create()
		{
			this.Name = "VS_AddIn_{0}_{1}".format(this.ButtonText, "".appendGuid());
			this.ToolTip = this.ToolTip ?? "";
			this.MSOButton = true;
			this.icon = 212; //59
			this.createMenuItem(this.TargetMenu);			
		}
		

		public void createMenuItem(string targetMenu)
		{
			try
			{
				this.Name =  this.Name.replace("-", "_")
									  .replace(" ", "_")
									  .trim();
				this.FullName = "{0}.{1}".format(this.O2AddIn.VS_Type, this.Name);
										 

				var toolsMenu = this.O2AddIn.VS_Dte.get_CommandBarPopup(targetMenu);

				this.Command = toolsMenu.add_NamedCommand(  this.O2AddIn.VS_Dte, this.O2AddIn.VS_AddIn,
															this.Name,
															this.ButtonText,
															this.ToolTip,
															this.MSOButton,
															this.icon);
				this.O2AddIn.Commands.Add(this.FullName, this);
			}
			catch (Exception ex)
			{
				ex.log();
			}
		}
	}


	public static class CommandBase_ExtensionMethods
	{ 
		public static CommandBase add_MenuItem(this O2_VS_AddIn o2AddIn, string buttonText, string targetMenu, Action onExecute) 
		{
			var newCommand = new CommandBase(o2AddIn)
									{
										ButtonText = buttonText,
										TargetMenu = targetMenu,
										Execute = onExecute
									};
			newCommand.create();
			return newCommand;
		}
	}
}
