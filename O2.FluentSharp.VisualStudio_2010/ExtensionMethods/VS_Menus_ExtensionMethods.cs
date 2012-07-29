using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using O2.FluentSharp;
using Microsoft.VisualStudio.CommandBars;
using EnvDTE80;
//O2Ref:Microsoft.CSharp.dll
//O2Ref:Microsoft.VisualStudio.CommandBars.dll
//O2File:VisualStudio_2010_ExtensionMethods.cs

namespace O2.DotNetWrappers.ExtensionMethods
{
    public static class VS_Menus_ExtensionMethods
    {
        public static CommandBarPopup add_TopMenu(this DTE2 dte, string text = "New Top Menu", string addAfterMenu = "Help")
        {            
            dynamic commandBars = dte.CommandBars;
            var menuCommandBar = commandBars["MenuBar"];
            var position = (commandBars[addAfterMenu].Parent as CommandBarControl).Index;            
            var newMenu = (CommandBarPopup)menuCommandBar.Controls.Add(MsoControlType.msoControlPopup, 
                                                                       System.Type.Missing, 
                                                                       System.Type.Missing, 
                                                                       ++position, 
                                                                       true);
            newMenu.Caption = text;
            newMenu.Enabled = true;
            return newMenu;            
        }
        
        public static CommandBarButton add(this CommandBarPopup topMenu, string text, Action onClick)
        {
        	return topMenu.add_Button(text, onClick);
        }
        public static CommandBarButton add_Button(this CommandBarPopup topMenu, string text = "New Button", Action onClick = null , int before = 1)
        {    	
			var  button = (CommandBarButton)topMenu.Controls.Add(MsoControlType.msoControlButton, // type
																 System.Type.Missing, 			  // id
																 System.Type.Missing, 			  // parameter
																 before,								  // before
																 false);						  // temporary

			button.Click += (CommandBarButton sender, ref bool CancelDefault) => onClick.invoke();
			button.Caption = text;
			button.Enabled = true;
			return button;
		}
        
               	
        public static CommandBar get_CommandBar(this DTE2 dte2, string commandBarName)
		{
			var commandBars = (CommandBars)dte2.CommandBars;			
			return commandBars[commandBarName];			
		}				
       	
		public static CommandBarControl get_CommandBarMenu(this DTE2 dte2, string menuName)
		{
			return dte2.get_CommandBarMenu<CommandBarControl>(menuName);
		}

		public static T get_CommandBarMenu<T>(this DTE2 dte2, string menuName)
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
			return dte2.get_CommandBarMenu<CommandBarPopup>(menuName);
		}
		
		public static CommandBarPopup get_Menu(this DTE2 dte2, string menuName)
       	{
       		return dte2.get_CommandBarPopup(menuName);
       	}
    }
}