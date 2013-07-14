using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using FluentSharp.CoreLib.API;

namespace FluentSharp.WinForms
{
    public static class WinForms_ExtensionMethods_Button
    {     
        public static Button add_Button(this Control control, string text)
        {
            return control.add_Button(text, -1);
        }
        public static Button add_Button(this Control control, string text, int top)
        {
            return control.add_Button(text, top, -1);
        }
        public static Button add_Button(this Control control, string text, int top, int left)
        {
            return control.add_Button(text, top, left, -1, -1);
        }
        public static Button add_Button(this Control control, string text, int top, int left, int height)
        {
            return control.add_Button(text, top, left, height, -1);
        }
        public static Button add_Button(this Control control, string text, int top, int left, int height, int width)
        {
            return control.add_Button(text, top, left, height, width, null);
        }
        public static Button add_Button(this Control control, string text, int top, int left, int height, int width, MethodInvoker onClick)
        {
            return control.invokeOnThread(
                () =>
                    {
                        var button = new Button {Text = text};
                        if (top > -1)
                            button.Top = top;
                        if (left > -1)
                            button.Left = left;
                        if (width == -1 && height == -1)
                            button.AutoSize = true;
                        else
                        {
                            if (width > -1)
                                button.Width = width;
                            if (height > -1)
                                button.Height = height;
                        }
                        button.onClick(onClick);
                        /*if (onClick != null)
                                    button.Click += (sender, e) => onClick();*/
                        control.Controls.Add(button);
                        return button;
                    });
        }
        public static Button add_Button(this Control control, string text, int top, int left, MethodInvoker onClick)
        {
            return control.add_Button(text, top, left, -1, -1, onClick);
        }
        public static Button add_Button(this Control control, int top, string buttonText)
        {
            return control.add_Button(top, 0, buttonText);
        }

        public static Button add_Button(this Control control, int top, int left, string buttonText)
        {
            return control.add_Button(buttonText, top, left);
        }

        public static Button onClick(this Button button, MethodInvoker onClick)
        {
            if (onClick != null)
                button.Click += (sender, e) => O2Thread.mtaThread(() => onClick());
            return button;
        }
        public static Button click(this Button button)
        {            
            O2Thread.mtaThread(
                () => button.invokeOnThread(button.PerformClick));
            return button;
        }
        public static Button set_Text(this Button button, string text)
        {
            return button.invokeOnThread(
                () =>
                    {
                        button.Text = text;
                        return button;
                    });

        }
        public static List<Button> buttons(this Control control)
        {
            return control.controls<Button>(true);
        }
        public static Button button(this Control control, string text)
        {
            return control.buttons().FirstOrDefault(button => WinForms_ExtensionMethods_Control_Object.get_Text(button) == text);
        }
    }
}