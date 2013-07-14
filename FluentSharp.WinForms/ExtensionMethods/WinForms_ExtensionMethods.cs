using System;
using System.Windows.Forms;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.WinForms.Controls;

namespace FluentSharp.WinForms
{
    public static class WinForms_ExtensionMethods
    {
        //add Ascx
        public static Control add_Ascx(this O2Gui o2Gui, Type controlType)
		{
			return (Control)(o2Gui.invokeOnThread(
				()=> {
						var control = (Control)PublicDI.reflection.createObjectUsingDefaultConstructor(controlType);
						if (control != null)
						{
							control.Dock = DockStyle.Fill;
							var hostForm = new Form();
                            hostForm.set_H2Icon();
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

        //show ascx in/as form
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
            return WinForms_Show.showAscxInForm(ascxType, name, width, height);
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

    }
}
