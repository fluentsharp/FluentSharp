using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FluentSharp.CoreLib;

namespace FluentSharp.WPF
{
    public static class Child_Controls_ExtensionMethods
    {
        #region WPF child controls

        public static List<T> allUIElements<T>(this UIElement uiElement)
            where T : UIElement
        {
            return (from element in uiElement.allUIElements()
                    where element is T
                    select (T)element).toList();
        }
       	
        public static List<UIElement> allUIElements(this UIElement uiElement)
        {        	
            return (List<UIElement>)uiElement.wpfInvoke(
                ()=>{
                        var uiElements = new List<UIElement>();        				        				
        				
                        uiElements.Add(uiElement);
        				
                        if (uiElement is ContentControl)
                        {
                            var content = (uiElement as ContentControl).Content;
                            if (content is UIElement)
                                uiElements.AddRange((content as UIElement).allUIElements());
                        }
                        if (uiElement is Panel)
                        {
                            var children = (uiElement as Panel).Children;
                            foreach(var child in children)
                                if (child is UIElement)
                                    uiElements.AddRange((child as UIElement).allUIElements()); 
                        }
                        return uiElements;
                });        			        	                	
        }
        
        public static List<T> controls_Wpf<T>(this UIElement uiElement)
            where T : UIElement
        {
            return uiElement.allUIElements<T>();
        }    
    
        public static List<UIElement> controls_Wpf(this UIElement uiElement)
        {
            return uiElement.allUIElements();
        }

        public static List<UIElement> children(this UIElement uiElement)
        {
            return uiElement.allUIElements();
        }
        
        public static T control_Wpf<T>(this UIElement uiElement)
            where T : UIElement
        {
            foreach(var control in uiElement.allUIElements())
                if (control is T)
                    return (T)control;
            return null;
        }


        #endregion       
    }
}