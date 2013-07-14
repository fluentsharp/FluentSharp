using System;
using System.Windows.Forms;

namespace FluentSharp.WinForms
{
    public static class WinForms_ExtensionMethods_CheckBox
    {             
        public static CheckBox add_CheckBox(this Control control, string text, int top, int left, Action<bool> onChecked)
        {
            return control.invokeOnThread(
                () =>{
                         var checkBox = new CheckBox {Text = text};
                         checkBox.CheckedChanged += (sender, e) => onChecked(checkBox.Checked);
                         if (top > -1)
                             checkBox.Top = top;
                         if (left > -1)
                             checkBox.Left = left;
                         control.Controls.Add(checkBox);
                         return checkBox;
                });
        }
        public static CheckBox add_CheckBox(this Control control, int top, string checkBoxText)
        {
            return control.add_CheckBox(top, 0, checkBoxText);
        }
        public static CheckBox add_CheckBox(this Control control, int top, int left, string checkBoxText)
        {
            return control.add_CheckBox(checkBoxText, top, left, (value) => { })
                          .autoSize();
        }
        public static bool @checked(this CheckBox checkBox)
        {
            return checkBox.value();
        }
        public static bool value(this CheckBox checkBox)
        {
            return checkBox.invokeOnThread(
                () => checkBox.Checked);
        }
        public static CheckBox @checked(this CheckBox checkBox, bool value)
        {
            return checkBox.value(value);
        }
        public static CheckBox value(this CheckBox checkBox, bool value)
        {
            return checkBox.invokeOnThread(
                () =>
                    {
                        checkBox.Checked = value;
                        return checkBox;
                    });
        }
        public static CheckBox check(this CheckBox checkBox)
        {
            return checkBox.value(true);
        }
        public static CheckBox uncheck(this CheckBox checkBox)
        {
            return checkBox.value(false);
        }
        public static CheckBox tick(this CheckBox checkBox)
        {
            return checkBox.value(true);
        }
        public static CheckBox untick(this CheckBox checkBox)
        {
            return checkBox.value(false);
        }
        public static CheckBox autoSize(this CheckBox checkBox)
        {
            return checkBox.autoSize(true);
        }
        public static CheckBox autoSize(this CheckBox checkBox, bool value)
        {
            checkBox.invokeOnThread(() => checkBox.AutoSize = value);
            return checkBox;
        }
        public static CheckBox append_CheckBox(this Control control, string text, Action<bool> action)
        {
            return control.append_Control<CheckBox>()
                          .set_Text(text)
                          .autoSize()
                          .onChecked(action);
        }
        public static CheckBox onClick(this CheckBox checkBox, Action<bool> action)
        {
            return checkBox.onChecked(action);
        }		
        public static CheckBox onChecked(this CheckBox checkBox, Action<bool> action)
        {
            return checkBox.checkedChanged(action);
        }
        public static CheckBox checkedChanged(this CheckBox checkBox, Action<bool> action)
        {
            checkBox.invokeOnThread(
                ()=> checkBox.CheckedChanged+= (sender,e) => action(checkBox.value()));
            return checkBox;
        }

    }
}