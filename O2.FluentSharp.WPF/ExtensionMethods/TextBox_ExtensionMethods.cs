using System;
using System.Windows;
using System.Windows.Controls;

namespace FluentSharp.WPF
{
    public static class TextBox_ExtensionMethods
    {
        #region TextBox
		
        public static TextBox set_Text(this TextBox textBox, string value)
        {
            textBox.wpfInvoke(()=> textBox.Text = value);    		
            return textBox;
        }

        public static string get_Text_Wpf(this TextBox textBox)
        {
            return (string)textBox.wpfInvoke(
                ()=>{
                        return textBox.Text;
                });
        }

        public static TextBox set_Text_Wpf(this TextBox textBox, string text)
        {
            return (TextBox)textBox.wpfInvoke(
                ()=>{
                        textBox.Text = text;
                        return textBox;
                });
        }

        public static TextBox multiLine(this TextBox textBox)
        {
            return (TextBox)textBox.wpfInvoke(
                () =>
                    {
                        textBox.TextWrapping = TextWrapping.Wrap;
                        textBox.AcceptsReturn = true;
                        textBox.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                        return textBox;
                    });
        }
        
        public static TextBox onTextChange(this TextBox textBox, Action<string> onTextChange)
        {
            return (TextBox)textBox.wpfInvoke(
                ()=>{
                        textBox.TextChanged += (sender,e)=> onTextChange(textBox.get_Text_Wpf());
                        return textBox;
                });
        }
		
        // not working since the Enter is not captured
        /*public static TextBox onEnter_Wpf(this TextBox textBox, Action<string> callback)
        {        
			textBox.onKeyPress_Wpf(Key.Return, ()=> callback(textBox.get_Text_Wpf()));
			return textBox;
        }*/
        #endregion
    }
}