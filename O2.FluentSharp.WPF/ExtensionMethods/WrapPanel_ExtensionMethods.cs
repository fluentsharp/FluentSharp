// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)

using System.Windows;
using System.Windows.Controls;

//O2File:WinFormIntegration_ExtensionMethods.cs

//O2Ref:O2_API_Visualization.dll

//O2Ref:PresentationCore.dll
//O2Ref:PresentationFramework.dll
//O2Ref:WindowsBase.dll
//O2Ref:System.Xaml.dll

namespace FluentSharp.WPF
{
    public static class WrapPanel_ExtensionMethods
	{	
        //#region WrapPanel
        public static WrapPanel add_WrapPanel(this UIElement uiElement)
        {
            return uiElement.add_Control_Wpf<WrapPanel>();
        }

 	}
}
