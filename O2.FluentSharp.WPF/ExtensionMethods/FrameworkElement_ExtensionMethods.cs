using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using FluentSharp.CoreLib.API;

namespace FluentSharp.WPF
{
    public static class FrameworkElement_ExtensionMethods
    {               
        #region FrameworkElement

        // this is the generic one
        public static T prop<T>(this T frameworkElement, string propertyName, object value) where T : FrameworkElement
        {    		
            frameworkElement.wpfInvoke(
                ()=>{ PublicDI.reflection.setProperty(propertyName, frameworkElement, value); });    				
            return frameworkElement;
        }
    	
    	
        // specific to frameworkElement

        public static double width_Wpf<T>(this T frameworkElement)
            where T : FrameworkElement
        {
            return (double)frameworkElement.wpfInvoke(
                () =>
                    {
                        return frameworkElement.Width;
                    });
        }

        public static double height_Wpf<T>(this T frameworkElement)
            where T : FrameworkElement
        {
            return (double)frameworkElement.wpfInvoke(
                () =>
                    {
                        return frameworkElement.Height;
                    });

        }
        public static T width_Wpf<T>(this T frameworkElement, double width)
            where T : FrameworkElement
        {
            return (T)frameworkElement.wpfInvoke(
                () =>
                    {
                        if (width > -1)
                            frameworkElement.Width = width;
                        return frameworkElement;
                    });
        }

        public static T height_Wpf<T>(this T frameworkElement, double height)
            where T : FrameworkElement
        {
            return (T)frameworkElement.wpfInvoke(
                () =>
                    {
                        if (height > -1)
                            frameworkElement.Height = height;
                        return frameworkElement;
                    });
        }

        public static T left_Wpf<T>(this T frameworkElement, double left)
            where T : FrameworkElement
        {
            return (T)frameworkElement.wpfInvoke(
                () =>
                    {
                        if (left > -1)
                            frameworkElement.SetValue(Canvas.LeftProperty, (double)left);
                        return frameworkElement;
                    });
        }

        public static T top_Wpf<T>(this T frameworkElement, double top)
            where T : FrameworkElement
        {
            return (T)frameworkElement.wpfInvoke(
                () =>
                    {
                        if (top > -1)
                            frameworkElement.SetValue(Canvas.TopProperty, (double)top);
                        return frameworkElement;
                    });
        }

        /*public static T width<T>(this T frameworkElement, double value) where T : FrameworkElement
    	{
    		return frameworkElement.prop("Width",value); 
    	}
    	
    	public static T height<T>(this T frameworkElement, double value) where T : FrameworkElement
    	{
    		return frameworkElement.prop("Height",value); 
    	}
        */
        /*public static double width(this FrameworkElement frameworkElement)
        {
            return (double)frameworkElement.wpfInvoke(() => { return frameworkElement.Width; });
        }

        public static double height(this FrameworkElement frameworkElement)
        {
            return (double)frameworkElement.wpfInvoke(() => { return frameworkElement.Height; });
        }*/
    	
        public static T tag<T>(this T frameworkElement, object value) where T : FrameworkElement
        {
            return frameworkElement.prop("Tag",value); 
        }
    	
        // specific to Panel (Ideally i should be able to merge this with the Control ones, but it doesn't seem to be possible    	
        public static Canvas background(this Canvas canvas, Brush value) 
        {
            return canvas.prop("Background",value); 
        }
        // specific to Control    	
        public static T background<T>(this T frameworkElement, Brush value) 
            where T : Control
        {
            return frameworkElement.prop("Background",value); 
        }

        public static T backColor<T>(this T frameworkElement, Brush value)
            where T : FrameworkElement
        {
            return frameworkElement.prop<T>("Background", value);
        }

        public static T fontSize<T>(this T frameworkElement, double value) where T : Control
        {
            return frameworkElement.prop("FontSize",value); 
        }
    	
        public static T fontColor<T>(this T frameworkElement, Brush value) where T : Control
        {
            return frameworkElement.prop("Foreground",value); 
        }
    	
        public static T foreground<T>(this T frameworkElement, Brush value) where T : Control
        {
            return frameworkElement.prop("Foreground",value); 
        }
    	
        public static T borderThickness<T>(this T frameworkElement, double value) where T : Control
        {
            return frameworkElement.borderThickness(new Thickness(value));    		
        }
    	
        public static T borderThickness<T>(this T frameworkElement, Thickness value) where T : Control
        {
            return frameworkElement.prop("BorderThickness",value); 
        }
    	
        public static T padding<T>(this T frameworkElement, double value) where T : Control
        {
            return frameworkElement.padding(new Thickness(value));    		
        }
    	
        public static T padding<T>(this T frameworkElement, Thickness value) where T : Control
        {
            return frameworkElement.prop("Padding",value); 
        }
    	
        public static T borderBrush<T>(this T frameworkElement, Brush value) where T : Control
        {
            return frameworkElement.prop("BorderBrush",value); 
        }
    	
        public static T vertical<T>(this T frameworkElement, VerticalAlignment value) where T : Control
        {
            return frameworkElement.verticalContentAlignment(value);
        }
        public static T verticalContentAlignment<T>(this T frameworkElement, VerticalAlignment value) where T : Control
        {
            return frameworkElement.prop("VerticalContentAlignment",value); 
        }
    	
        public static T horizontal<T>(this T frameworkElement, HorizontalAlignment value) where T : Control
        {
            return frameworkElement.horizontalContentAlignment(value);
        }
        public static T horizontalContentAlignment<T>(this T frameworkElement, HorizontalAlignment value) where T : Control
        {
            return frameworkElement.prop("HorizontalContentAlignment",value); 
        }
    	
        // specific to TextBoxBase
    	
        public static T acceptsReturn<T>(this T frameworkElement, bool value) where T : TextBoxBase
        {
            return frameworkElement.prop("AcceptsReturn",value); 
        }

        public static T acceptsTab<T>(this T frameworkElement, bool value) where T : TextBoxBase
        {
            return frameworkElement.prop("AcceptsTab",value); 
        }    	
    	
        /*
    	
textBox1.prop("VerticalContentAlignment",VerticalAlignment.Center);  
textBox1.prop("HorizontalContentAlignment",System.Windows.HorizontalAlignment.Center);  
textBox1.prop("",true);

    	*/
    	
    	
    	
        #endregion

    }
}