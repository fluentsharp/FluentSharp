using System.Windows;
using System.Windows.Controls;

namespace FluentSharp.WPF
{
    public static class Label_ExtensionMethods
    {	
        #region Label

        public static Label set_Text(this Label label, string value)
        {
            label.set_Text_Wpf(value);    		
            return label;
        }
    	
        public static Label add_Label_Wpf<T>(this  T uiElement, string text)
            where T : UIElement
        {
            return uiElement.add_Label_Wpf(text,false);
        }
    	
        public static Label add_Label_Wpf<T>(this  T uiElement, string text, bool bold)
            where T : UIElement
        {
            var label = uiElement.add_Control_Wpf<Label>();
            if (bold) 
                label.bold();
            label.set_Text_Wpf(text);
            return label;
        }                	
    	
        #endregion
 
    }
}