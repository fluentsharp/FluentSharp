using System.Windows;
using System.Windows.Controls;

namespace FluentSharp.WPF
{
    public static class TextBlock_ExtensionMethods
    {	
        #region WrapPanel

        public static TextBlock add_TextBlock(this UIElement uiElement)
        {
            return (TextBlock)uiElement.wpfInvoke(
                ()=>{
                        var textBlock = uiElement.add_Control_Wpf<TextBlock>();
                        textBlock.TextWrapping = TextWrapping.Wrap;
                        textBlock.Padding = new Thickness(4);
                        return textBlock;
                });
        }
        
        public static TextBlock set_Text_Wpf(this TextBlock textBlock, string text)
        {
        	
            textBlock.wpfInvoke(()=> textBlock.Text = text);
            return textBlock;
        }

        #endregion

    }
}