using System;
using System.Windows.Forms;
using O2.DotNetWrappers.ExtensionMethods;
using O2.Kernel;
using O2.Kernel.ExtensionMethods;
using O2.Views.ASCX.classes.MainGUI;

namespace O2.Views.ASCX.ExtensionMethods
{
    public static class WinForms_ExtensionMethods
    {
        /*
        #region MenuStrip and ToolStripMenuItem
        public static MenuStrip add_Menu(this Form form)
		{
			var menuStrip = new MenuStrip();
			form.Controls.Add(menuStrip);            
            form.MainMenuStrip  = menuStrip;  
            return menuStrip;
		}
		
		public static ToolStripMenuItem add_MenuItem(this MenuStrip menuStrip, string text)
		{
			var fileMenuItem = new ToolStripMenuItem {Text = text};
		    menuStrip.Items.Add(fileMenuItem);
            return fileMenuItem;            
		}
		
		public static ToolStripMenuItem add_MenuItem(this ToolStripMenuItem menuItem,  string text)
		{
		    var clildMenuItem = new ToolStripMenuItem {Text = text};
		    menuItem.DropDownItems.Add(clildMenuItem);
            return clildMenuItem;
        }
        
        #endregion
        */
        #region add Ascx

        public static Control add_Ascx(this O2Gui o2Gui, Type controlType)
		{
			return (Control)(o2Gui.invokeOnThread(
				()=> {
						var control = (Control)PublicDI.reflection.createObjectUsingDefaultConstructor(controlType);
						if (control != null)
						{
							control.Dock = DockStyle.Fill;
							var hostForm = new Form();
							hostForm.Controls.Add(control);
							hostForm.MdiParent = o2Gui;
							hostForm.WindowState=FormWindowState.Maximized;
							hostForm.Show();							
							return control;
						}
						return null;
					 }));
		}
		
		public static void add_Ascx(this O2Gui o2Gui, Control control)
		{
			o2Gui.Controls.Add(control);
        }

        #endregion

        #region show ascx in/as form

        public static Control showInForm(this string typeName, string name, int width, int height)
        {
            var ascxType = PublicDI.reflection.getType(typeName);
            if (ascxType != null)
                return ascxType.showInForm(name, width, height);

            PublicDI.log.error("in string.showInForm, coult not find type: {0}", typeName);
            return null;
        }

        public static Control showInForm(this Type ascxType, string name, int width, int height)
        {
            return WinForms.showAscxInForm(ascxType, name, width, height);
        }

        public static object showAsForm(this Type type)
        {
            return type.showAsForm(-1, -1);
        }

        public static T openAsForm<T>(this T control) where T : Control
        {
            var title = control.str();
            var panel = O2Gui.open<Panel>(title, 300, 400);
            control.fill();
            panel.add(control);
            return control;
        }

        public static object showAsForm(this Type type, string title)
        {
            return type.showAsForm(title, -1, -1);
        }

        public static object showAsForm(this Type type, int width, int height)
        {
            var title = type.Name.remove("ascx_").toSpace("_");
            return type.showAsForm(title, width, height);
        }

        public static object showAsForm(this Type type, string title, int width, int height)
        {
            return type.showInForm(title, width, height);
        }

        #endregion

/*        

        public static void viewProperties(this Object _object)
        {
            var propertyGrid = O2Gui.load<PropertyGrid>();
            propertyGrid.show(_object);                        
        }
  */    
    }
}
