using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FluentSharp.NUnit;
using FluentSharp.WinForms;
using NUnit.Framework;

namespace UnitTests.FluentSharp_WinForms
{
    [TestFixture]
    public class Test_WinForms_ExtensionMethods_Control_Object : NUnitTests
    {
        [Test(Description="Returns provided control visible state")]
        public void isVisible()
        {
            var popupWindow = "test visibility".popupWindow();
            var textBox     = popupWindow.add_TextBox();
            assert_Is_True(popupWindow.isVisible());
            assert_Is_True(textBox.isVisible());

            popupWindow.visible(false);

            assert_Is_False(popupWindow.isVisible());
            assert_Is_False(textBox.isVisible());

            popupWindow.visible(true);

            assert_Is_True(popupWindow.isVisible());
            assert_Is_True(textBox.isVisible());

            textBox.visible(false);

            assert_Is_True(popupWindow.isVisible());
            assert_Is_False(textBox.isVisible());

            textBox.visible(true);

            assert_Is_True(popupWindow.isVisible());
            assert_Is_True(textBox.isVisible());

            popupWindow.closeForm();

            assert_Is_False(popupWindow.isVisible());
            assert_Is_False(textBox.isVisible());

            //test nulls
            assert_Is_False((null as Form).isVisible());
            assert_Is_False((null as Panel).isVisible());
        }
    }
}
