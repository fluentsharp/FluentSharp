using System.Windows;
using System.Windows.Controls;

namespace FluentSharp.WPF
{
    public static class StackPanel_ExtensionMethods
    {	
        #region StackPanel

        public static StackPanel add_StackPanel(this UIElement uiElement)
        {
            return uiElement.add_Control_Wpf<StackPanel>();
        }

        #endregion

    }
}