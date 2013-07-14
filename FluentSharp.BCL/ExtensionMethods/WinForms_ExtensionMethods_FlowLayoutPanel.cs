using System.Windows.Forms;

namespace FluentSharp.WinForms
{
    public static class WinForms_ExtensionMethods_FlowLayoutPanel
    {             
        public static FlowLayoutPanel add_FlowLayoutPanel(this Control control)
        {
            return control.add_Control<FlowLayoutPanel>();
        }
        public static FlowLayoutPanel clear(this FlowLayoutPanel flowLayoutPanel)
        {
            return (FlowLayoutPanel)flowLayoutPanel.invokeOnThread(
                () =>
                    {
                        flowLayoutPanel.Controls.Clear();
                        return flowLayoutPanel;
                    });
        }
    }
}