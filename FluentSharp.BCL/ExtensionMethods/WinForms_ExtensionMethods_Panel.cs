using System.Windows.Forms;

namespace FluentSharp.WinForms
{
    public static class WinForms_ExtensionMethods_Panel
    {
        public static Panel add_Panel(this Control control, bool clear)
        {
            if (clear)
                control.clear();
            return control.add_Panel();
        }

        public static Panel add_Panel(this Control control)
        {
            return control.add_Control<Panel>();
        }
    }
}