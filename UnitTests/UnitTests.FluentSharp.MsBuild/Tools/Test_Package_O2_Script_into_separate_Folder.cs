using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FluentSharp.CoreLib;
using FluentSharp.MsBuild;
using FluentSharp.NUnit;
using FluentSharp.REPL;
using FluentSharp.WinForms;
using NUnit.Framework;

namespace UnitTests.FluentSharp.MsBuild.Tools
{
    [TestFixture]
    public class Test_Package_O2_Script_into_separate_Folder : NUnitTests
    {
        [Test] public void Main()
        {
            var main = Package_O2_Script_into_separate_Folder.Main(startHidden: true);
            main.assert_Not_Null()
                .assert_Is_Instance_Of<Panel>();

            var parentForm = main.parentForm();
            parentForm.buttons()  .assert_Size_Is(4);
            parentForm.controls<WebBrowser>(true).assert_Size_Is(1);
            parentForm.controls<ComboBox>(true).assert_Size_Is(1);

            //parentForm.waitForClose();
            parentForm.close();
        }

        //Workflows

        [Test][Ignore("TO FINISH")] public void Invoke_Using_Reflection_Package_O2_Script_into_separate_Folder()
        {
            var assembly = "FluentSharp.MsBuild".assembly().assert_Not_Null();
            var type     = assembly.type("Package_O2_Script_into_separate_Folder").assert_Not_Null();
            var topPanel = type.invokeStatic("Main",  true, default(string)).assert_Not_Null().assert_Instance_Of<Panel>();
            topPanel.parentForm().close();
        }
    }
}
