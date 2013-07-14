using System.Drawing;
using System.Windows.Forms;

namespace FluentSharp.WinForms
{
    public static class WinForms_ExtensionMethods_Label
    { 
        public static Label add_Label		(this Control control, string text, int top)
        {
            return control.add_Label(text, top, -1);
        }
        public static Label add_Label		(this Control control, string text, int top, int left)
        {
            Label label = control.add_Label(text);
            label.invokeOnThread(() =>
                {
                    if (top > -1)
                        label.Top = top;
                    if (left > -1)
                        label.Left = left;
                });
            return label;
        }
        public static Label add_Label		(this Control control, string labelText)
        {
            return (Label) control.invokeOnThread(
                ()=>{
                        var label = new Label();
                        label.AutoSize = true;
                        label.Text = labelText;
                        control.Controls.Add(label);
                        return label;
                });
        }
        public static Label add_Label		(this Control control, string text, ref Label label)
        {
            return label = control.add_Label(text);
        }
        public static Label append_Label<T> (this T control, string text, ref Label label) where T : Control
        {
            return label = control.append_Label(text);
        }
        public static Label append_Label<T>	(this T control, string text) where T : Control
        {
            return control.append_Control<System.Windows.Forms.Label>()
                          .set_Text(text)
                          .autoSize();
        }
        public static Label append_Label    (this Control control, string text, int top)
        {
            return control.append_Label(text).top(top);
        }		
        public static Label append_Label    (this Control control, string text, int top, ref Label label)
        {
            return label = control.append_Label(text).top(top);
        }
        public static Label append_Label    (this Control control, ref Label label)
        {
            return label = control.append_Control<Label>().autoSize();
        }
        public static Label set_Text		(this Label label, string text)
        {
            return (Label)label.invokeOnThread(
                () =>
                    {
                        label.Text = text;
                        return label;
                    });
        }
        public static Label append_Text		(this Label label, string text)
        {
            return (System.Windows.Forms.Label)label.invokeOnThread(
                () =>
                    {
                        label.Text += text;
                        return label;
                    });

        }
        public static string get_Text		(this Label label)
        {
            return (string)label.invokeOnThread(
                () =>
                    {
                        return label.Text;
                    });
        }
        public static Label textColor		(this Label label, Color color)
        {
            return (Label)label.invokeOnThread(
                () =>
                    {
                        label.ForeColor = color;
                        return label;
                    });
        }
        public static Label autoSize		(this Label label)
        {
            label.invokeOnThread(() => label.AutoSize = true);
            return label;
        }        
        public static Label autoSize		(this Label label, bool value)
        {
            label.invokeOnThread(
                ()=>{						
                        label.AutoSize = value;
                });
            return label;
        }		
        public static Label text_Center		(this Label label)			
        {			
            label.invokeOnThread(
                ()=>{						
                        label.autoSize(false);
                        label.TextAlign = ContentAlignment.MiddleCenter;
                });
            return label;
        }	        		
        public static Label bold(this Label label)
        {
            return label.font_bold();
        }

        public static Label add_Message(this Control control, string messageToShow)
        {
            return control.clear().white().add_Label(messageToShow)
                          .fill().text_Center().font_bold().size(20);
        }
    }
}