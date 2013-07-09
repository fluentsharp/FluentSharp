using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FluentSharp.WPF
{
    public static class Events_ExtensionMethods
    {
        #region generic events
    	
        public static T onKeyPress_Wpf<T>(this T control, Key keyWanted,  Action callback)    		
            where T : UIElement
        {    		
            return control.onKeyPress_Wpf(
                (keyPressed)=>{
                                  if (keyPressed == keyWanted)
                                      callback();
                });
        }
    	
        public static T onKeyPress_Wpf<T>(this T control, Action<Key> callback)
            where T : UIElement
        {
            control.wpfInvoke(    		
                ()=>{
                        control.KeyUp += (sender,e)=> callback(e.Key);
                });
            return control;
        }    	

        public static T onMouseDoubleClick<T1, T>(this T control, Action<T1> callback)
            where T : Control
        {
            control.MouseDoubleClick += (sender, e) =>
                {
                    var tag = control.get_Tag();
                    if (tag != null && tag is T1)
                        callback((T1)tag);
                };
            return control;
        }

        #endregion	
		
    }
}