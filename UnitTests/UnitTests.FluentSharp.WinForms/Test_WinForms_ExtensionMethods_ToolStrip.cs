using System.Windows.Forms;
using FluentSharp.CoreLib;
using FluentSharp.WinForms;
using FluentSharp.NUnit;
using NUnit.Framework;

namespace UnitTests.FluentSharp_WinForms
{
    [TestFixture]
    class Test_WinForms_ExtensionMethods_ToolStrip : NUnitTests
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

        [Test(Description="Add ToolStrip to provided control")]
        public void add_ToolStrip()
        {
            var toolStrip = topPanel.add_ToolStrip();
            var label     = toolStrip.add_Label("123");

            assert_Not_Null(toolStrip);            
            assert_Not_Null(label);
            assert_Size_Is (toolStrip.items(),1);
            assert_Are_Equal(toolStrip.items().first(),label);
            assert_Are_Equal(label.toolStrip(), toolStrip);
        }        
    }
}
