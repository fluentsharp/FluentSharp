using System.Windows.Forms;
using FluentSharp.CoreLib;

namespace FluentSharp.WinForms
{
    public static class WinForms_ExtensionMethods_MenuStrip
    {
        public static MenuStrip         add_Menu(this Form form)
        {
            return form.invokeOnThread(
                () =>
                {
                    var menuStrip = new MenuStrip();
                    form.Controls.Add(menuStrip);
                    form.MainMenuStrip = menuStrip;
                    return menuStrip;
                });
        }
        public static MenuStrip         add_MenuStrip(this Control control)
        {
            return control.add_Control<MenuStrip>();
        }
        public static MenuStrip         insert_MenuStrip(this Control control)
        {
            return control.insert_Above(30).splitContainerFixed().add_MenuStrip();
        }
        public static ToolStripMenuItem add_Menu(this MenuStrip menuStrip, string name)
        {
            return menuStrip.add_MenuItem(name);
        }
        public static ToolStripMenuItem add_Menu(this ToolStripMenuItem toolStripMenuItem, string name)
        {
            if (toolStripMenuItem.notNull() && toolStripMenuItem.Owner is MenuStrip)
                return (toolStripMenuItem.Owner as MenuStrip).add_Menu(name);
            "[in add_Menu] toolStripMenuItem.Owner was not a MenuStrip, it was: {0}".error(toolStripMenuItem.typeName());
            return null;
        }
        public static ToolStripMenuItem add_MenuItem(this MenuStrip menuStrip, string text)
        {
            return menuStrip.add_MenuItem(text, null);
        }
        public static ToolStripMenuItem add_MenuItem(this MenuStrip menuStrip, string text, MethodInvoker callback)
        {
            return menuStrip.invokeOnThread(
                () =>
                {
                    var fileMenuItem = new ToolStripMenuItem {Text = text};
                    menuStrip.Items.Add(fileMenuItem);
                    if (callback != null)
                        fileMenuItem.Click += (sender, e) => callback();
                    return fileMenuItem;
                });
        }
        public static ContextMenuStrip  add_MenuItem(this ContextMenuStrip contextMenu, string text, bool dummyValue, MethodInvoker onClick)
        {
            // since we can't have two different return types the dummyValue is there for the cases where we want to get the reference to the 
            // ContextMenuStrip and not the menu item created

            if (dummyValue.isFalse())
                "invalid value in ContextMenuStrip add_MenuItem, only true creates the expected behaviour".error();
            contextMenu.add_MenuItem(text, onClick);
            return contextMenu;
        }
        
        public static ToolStripMenuItem click(this ToolStripMenuItem toolStripMenuItem)
        {
            toolStripMenuItem.toolStrip().invokeOnThread(toolStripMenuItem.PerformClick);
            return toolStripMenuItem;
        }

            
    }
}