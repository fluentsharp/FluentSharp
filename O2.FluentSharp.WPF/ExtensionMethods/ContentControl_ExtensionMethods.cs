using System.Windows;
using System.Windows.Controls;

namespace FluentSharp.WPF
{
    public static class ContentControl_ExtensionMethods
    {
		
        public static T bold<T>(this T contentControl)
            where T : ContentControl
        {
            return contentControl.bold(true);
        }
    	
        public static T bold<T>(this T contentControl, bool value)
            where T : ContentControl
        {
            contentControl.wpfInvoke(
                ()=>{
                        if (value)
                            contentControl.FontWeight = FontWeights.Bold;
                        else
                            contentControl.FontWeight = FontWeights.Normal;    					
                });
            return contentControl;
        }
    
    
    }
}