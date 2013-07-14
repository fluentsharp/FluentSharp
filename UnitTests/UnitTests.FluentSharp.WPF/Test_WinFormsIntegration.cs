using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentSharp.CoreLib;
using FluentSharp.WPF;
using FluentSharp.WinForms;
using NUnit.Framework;

namespace UnitTests.FluentSharp.WPF
{
    [TestFixture]
    public class Test_WinFormsIntegration
    {
        [Test]
        public void add_Wpf()
        {
            var winFormControl = "popupWindow".popupWindow(); // returns a System.Windows.Forms.Panel
            var elementHost = winFormControl.add_Wpf();

            Assert.IsNotNull(winFormControl);
            Assert.IsNotNull(elementHost);
            
            
            var wpfGrid = elementHost.add_Grid_Wpf();
							
            wpfGrid.backColor(System.Windows.Media.Brushes.White);				
            var wpfLabel = wpfGrid.add_Label_Wpf("test 123");
            var text = wpfLabel.get_Text_Wpf();

            Assert.AreEqual(text, "test 123");

            wpfLabel.set_Text_Wpf("changed");
            text = wpfLabel.get_Text_Wpf();

            Assert.AreEqual(text, "changed");
            
            winFormControl.parentForm().close();
        }
    }
}
