using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FluentSharp.WPF
{
    public static class Control_ExtensionMethods
    {
        public static T opacity<T>(this T uiElement, double value) where T : UIElement
        {
            return (T)uiElement.wpfInvoke(
                ()=>{
                        uiElement.Opacity = value;
                        return uiElement;
                });
        }	      
        public static T color<T>(this T control, string colorName) where T : Control
        {
            var color = new BrushConverter().ConvertFromString(colorName);
            if (color is Brush)
                control.fontColor((Brush)color);
            return control;
        }
        public static T black<T>(this T control) where T : Control
        {
            return control.fontColor(Brushes.Black);
        }
        public static T blue<T>(this T control) where T : Control
        {
            return control.fontColor(Brushes.Blue);
        }
        public static T red<T>(this T control) where T : Control
        {
            return control.fontColor(Brushes.Red);            
        }
    }
}