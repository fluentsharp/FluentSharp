using System.Windows;
using System.Windows.Controls;

namespace FluentSharp.WPF
{
    public static class Grid_ExtensionMethods
    {
	
        #region Grid

        public static Grid add_Grid_Wpf(this UIElement uiElement)
        {
            return uiElement.add_Control_Wpf<Grid>();
        }

        #endregion

    }
}