using System.Windows.Forms;

namespace FluentSharp.WinForms
{
    public static class WinForms_ExtensionMethods_GroupBox
    {     
        public static GroupBox add_GroupBox(this Control control, string groupBoxText)
        {
            return (GroupBox) control.invokeOnThread(
                ()=>{
                        var groupBox = new GroupBox();
                        groupBox.Dock = DockStyle.Fill;
                        groupBox.Text = groupBoxText;
                        control.Controls.Add(groupBox);
                        return groupBox;
                });
        }
        public static GroupBox title(this Control control, string title)
        {
            return control.add_GroupBox(title);
        }
    }
}