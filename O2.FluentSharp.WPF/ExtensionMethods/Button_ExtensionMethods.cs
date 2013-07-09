using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;

namespace FluentSharp.WPF
{
    public static class Button_ExtensionMethods
    {
	
        #region Button

        public static Button onClick_Wpf(this Button button, Action callback)
        {
            return (Button)button.wpfInvoke(
                () =>
                    {
                        if (button.notNull())
                            button.Click += (sender, e) => 
                                            O2Thread.mtaThread(()=> 
                                                {
                                                    try
                                                    {
                                                        callback();
                                                    }
                                                    catch(Exception ex)
                                                    {
                                                        ex.log("in WPF button onClick_Wpf");
                                                    }
                                                });
                        return button;
                    });
        }
        
        public static Button name(this List<Button> buttons, string text)
        {            
            foreach(var button in buttons)
                if (button.get_Text_Wpf() == text)
                    return button;
            return null;
        }
	
        public static Button click (this Button button)
        {		
            return (Button)button.wpfInvoke(
                ()=>{
                        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                        return button;
                });
        }
        #endregion

    }
}