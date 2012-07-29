using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extensibility;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;
using O2.Kernel.ExtensionMethods;
using O2.DotNetWrappers.ExtensionMethods;
using EnvDTE;
using System.Windows.Forms;

//O2Ref:EnvDTE.dll
//O2Ref:EnvDTE80.dll
//O2Ref:Extensibility.dll
//O2Ref:Microsoft.VisualStudio.CommandBars.dll


namespace O2.FluentSharp.VisualStudio.ExtensionMethods
{
    /// <summary>
    /// 
    /// </summary>
    public static class Microsoft_VisualStudio_ExtensionMethods_Misc
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectMode"></param>
        /// <returns></returns>
        public static bool uiSetUp(this ext_ConnectMode connectMode)
        {
            return connectMode == ext_ConnectMode.ext_cm_UISetup;
        }		
    }

	public static class Microsoft_VisualStudio_ExtensionMethods_DTE
	{
		public static CommandBar get_CommandBar(this DTE2 dte2, string commandBarName)
		{
			var commandBars = (CommandBars)dte2.CommandBars;			
			return commandBars[commandBarName];			
		}

		public static CommandBarControl get_CommandBarControl(this DTE2 dte2, string menuName)
		{
			return dte2.get_CommandBarControl<CommandBarControl>(menuName);
		}

		public static T get_CommandBarControl<T>(this DTE2 dte2, string menuName)
			where T : CommandBarControl		
		{
			var menuBarCommandBar = dte2.get_CommandBar("MenuBar");
			var menu = menuBarCommandBar.Controls[menuName];
			if(menu is T)
				return (T)menu;
			return default(T);
		}

		public static CommandBarPopup get_CommandBarPopup(this DTE2 dte2, string menuName)
		{
			return dte2.get_CommandBarControl<CommandBarPopup>(menuName);
		}

		public static Command add_NamedCommand(this CommandBarPopup commandBarPopup, 
												    DTE2 dte2, AddIn addIn, string Name,
													string ButtonText, string ToolTip,
													bool MSOButton, object Bitmap)
		{
			object[] contextGUIDS = new object[] { };
			Commands2 commands = (Commands2)dte2.Commands;

			Command command = commands.AddNamedCommand2(addIn,
														Name,
														ButtonText,
														ToolTip,
														MSOButton,
														Bitmap,
														ref contextGUIDS,
														(int)vsCommandStatus.vsCommandStatusSupported + (int)vsCommandStatus.vsCommandStatusEnabled, (int)vsCommandStyle.vsCommandStylePictAndText,
														vsCommandControlType.vsCommandControlTypeButton);

			//Add a control for the command to the tools menu:
			if ((command != null) && (commandBarPopup != null))
			{
				command.AddControl(commandBarPopup.CommandBar, 1);
				return command;
			}
			return null;
		}

	}


	public static class Microsoft_VisualStudio_ExtensionMethods_DTE_WinForms
	{ 
		
		public static Panel createWindowWithPanel(this DTE2 dte, AddIn addIn, string title, int width = -1, int height = -1)
		{
			var windows = (Windows2)dte.Windows;
			object newControl = null;
			
			var window = windows.CreateToolWindow2(	addIn , 
													"System.Windows.Forms".assembly().Location,
													"System.Windows.Forms.Panel",
													title,
													Guid.NewGuid().str(), ref newControl);
			if (width > -1)
				window.Width = width;
			if (height > -1)
				window.Height = height;
			window.Visible = true;
			return (Panel)newControl;
		}
	}
}
