using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FluentSharp.CoreLib;
using FluentSharp.NUnit;
using FluentSharp.WinForms;
using NUnit.Framework;

namespace UnitTests.FluentSharp_WinForms
{
    [TestFixture]
    public class Test_WinForms_ExtensionMethods_MenuStrip : NUnitTests
    {
        Form form;
        Panel topPanel;

        [SetUp]
        public void setup()
        {
            var hidden = true;
            topPanel = "Test_WinForms_ExtensionMethods".popupWindow(hidden);
            form = topPanel.parentForm();            
            assert_Not_Null(form);            
            assert_Not_Null(topPanel);            
        }
        [TearDown]
        public void teardown()
        {
            form.close();
        }
        [Test(Description="Adds an MenuStrip to an Form")]
        public void add_Menu()
        {
            var menu = form.add_Menu();
            assert_Not_Null(menu);
            var menuItem =  menu.add_MenuItem("asd");             
            assert_Size_Is  (menu.items(),1);
            assert_Are_Equal(menu.items().first(), menuItem);
        }
        [Test]
        public void add_MenuItem()
        {            
            //NOTE: this test covers the normal add_MenuItem funcionality and the regresssion test for the 
            //      issue described at:  https://github.com/o2platform/FluentSharp/issues/4
            var menu = form.add_Menu();
            var textbox = topPanel.add_TextArea();
            var menuItem1_Clicked = false;
            var menuItem2_Clicked = false;

            var menuItem1 =  menu.add_MenuItem("menuItem1", ()=> menuItem1_Clicked = true );            
            var menuItem2 =  menu.add_MenuItem("menuItem2", ()=> menuItem2_Clicked = true ); 

            assert_Not_Null (menuItem1);
            assert_Not_Null (menuItem2);
            assert_Are_Equal(menuItem1.Text, "menuItem1");
            assert_Are_Equal(menuItem2.Text, "menuItem2");
            assert_Is_False (menuItem1_Clicked);
            assert_Is_False (menuItem2_Clicked);
            
            menuItem1.click();

            assert_Is_True  (menuItem1_Clicked);
            assert_Is_False (menuItem2_Clicked);

            menuItem2.click();

            assert_Is_True  (menuItem1_Clicked);
            assert_Is_True  (menuItem2_Clicked);

        }
    }
}
