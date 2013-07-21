using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using FluentSharp.CoreLib;

namespace FluentSharp.WPF
{
    public static class Generic_ExtensionMethods
    {    	
        #region generic methods								
    	
        public static T createInThread_Wpf<T>(this UIElement uiElement)
        {
            return (T)uiElement.wpfInvoke(() => typeof(T).ctor());
        }

        public static T add_Control_Wpf<T>(this UIElement uiElement, T uiElementToAdd)
            where T : UIElement
        {
            if ((uiElement is IAddChild).isFalse())
            {
                "in add_Control_Wpf, the host control must implement the IAddChild interface, and '{0}' did not".error(typeof(T).name());
                return null;
            }
            return (T)uiElement.wpfInvoke(
                () =>
                    {
                        try
                        {
                            if (uiElementToAdd != null)
                                (uiElement as IAddChild).AddChild(uiElementToAdd);
                            return uiElementToAdd;
                        }
                        catch (Exception ex)
                        {
                            ex.log("in add_Wpf_Control");
                            return null;
                        }
                    });

        }

        public static T add_Control_Wpf<T>(this UIElement uiElement)
            where T : UIElement
        {
            if (uiElement.isNull())
            {
                "in add_Wpf_Control, the host control was null".error();
                return null;
            }
            if ((uiElement is IAddChild).isFalse())
            {
                "in add_Wpf_Control, the host control must implement the IAddChild interface, and '{0}' did not".error(typeof(T).name());
                return null;
            }
            return (T)uiElement.wpfInvoke(
                () =>
                    {
                        try
                        {
                            var newControl = typeof(T).ctor();
                            if (newControl.isNull())
                            {
                                "in add_Wpf_Control, could not create control of type: {0}".error(typeof(T).name());
                                return null;
                            }
                            (uiElement as IAddChild).AddChild(newControl);
                            return newControl;
                        }
                        catch (Exception ex)
                        {
                            ex.log("in add_Wpf_Control");
                            return null;
                        }
                    });
        }

        /*public static T add_Control<T>(this ContentControl uiElement) where T : UIElement
    	{
    		return (T)uiElement.wpfInvoke(
    			()=>{
    					try
			            {  
	    					var wpfControl = typeof(T).ctor();					    				
	    					if (wpfControl is UIElement)
	    					{
	    						uiElement.invoke("AddChild",((UIElement)wpfControl));
	    						return (T)wpfControl;			    								    					
	    					}
						}			
			            catch (System.Exception ex)
			            {
			                ex.log("in add_Control");
			            }
			            return null;			    				
    				});     		
    	}
        */        
        public static T set<T>(this ContentControl control) where T : UIElement
        {
            return (T)control.wpfInvoke(
                ()=>{ 
                        control.Content = (T)typeof(T).ctor();
                        return control.Content;
                });
        }  
		
        public static T set<T>(this T control, object value) where T : ContentControl
        {
            return (T)control.wpfInvoke(
                ()=>{ 
                        control.Content = value;
                        return control;
                });
        }  
		
        public static T newInThread<T>(this ContentControl control) where T : UIElement
        {
            return (T)control.wpfInvoke(
                ()=>{ 
                        return (T)typeof(T).ctor();    					
                });
        }
		
        public static ContentControl set_Value(this ContentControl control,UIElement uiElement)
        {
            return (ContentControl)control.wpfInvoke(
                ()=>{
                        control.Content = uiElement;
                        return control;
                });
        }
		
        public static T getFirst<T>(this List<object> list) where  T : UIElement
        {
            foreach(var item in list)
                if (item is T)
                    return (T)item;
            return null;
        }


        public static T set_Tag<T>(this T control, object tagObject)
            where T : Control
        {
            control.wpfInvoke(() => control.Tag = tagObject);
            return control;
        }

        public static object get_Tag<T>(this T frameworkElement)
            where T : FrameworkElement
        {
            return (object)frameworkElement.wpfInvoke(() => frameworkElement.Tag);
        }

        public static T get_Tag<T>(this Control control)
        {
            return (T)control.wpfInvoke(
                () =>
                    {
                        var tag = control.Tag;
                        if (tag is T)
                            return (T)tag;
                        return default(T);
                    });
        }

        /*public static string get_Text<T>(this T control)
            where T : ContentControl
        {
            return (string)control.wpfInvoke(() => control.Content);
        }
        */

        public static string get_Text_Wpf<T>(this T control)
            where T : ContentControl
        {
            return (string)control.wpfInvoke(
                () =>
                    {
                        return control.Content;
                    });
        }
        /* unfortunatly I can't seem to be call this set_Text able to do this since there are conflits when WPF and WinForms Extension methods are
         * used at the same time */
        public static T set_Text_Wpf<T>(this T control, string value)
            where T : ContentControl
        {
            return (T)control.wpfInvoke(() =>
                {
                    control.Content = value;
                    return control;
                });
        }

        public static T set_Content<T>(this T control, string value)
            where T : ContentControl
        {
            return control.set_Text_Wpf(value);
        }

        public static Brush get_Color<T>(this T control)
            where T : Control
        {
            return (Brush)control.wpfInvoke(() => control.Foreground);
        }
     
        public static T content<T>(this UIElement uiElement)
        {
            var content = uiElement.content();
            if (content is T)
                return (T)content;
            return default(T);
        }
		
        public static object content(this UIElement uiElement)
        {
            return uiElement.wpfInvoke(
                ()=>{
                        return uiElement.prop("Content");
                });
        }
        #endregion
    }
}