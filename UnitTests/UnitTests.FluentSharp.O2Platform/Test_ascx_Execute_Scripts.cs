using FluentSharp.CoreLib;
using FluentSharp.NUnit;
using FluentSharp.O2Platform.Controls;
using FluentSharp.REPL;
using NUnit.Framework;

namespace UnitTests.FluentSharp.O2Platform
{
    [TestFixture]
    public class Test_ascx_Execute_Scripts : NUnitTests
    {
        //WorkFlows 
        [Test]
        public void Check_That_NEW_GUI_SCRIPT_Exists_And_Can_Be_Compiled()
        {            
            var newGuiScript_FileName  = ascx_Execute_Scripts.NEW_GUI_SCRIPT;
            var newGuiScript_LocalPath = newGuiScript_FileName.local();

            newGuiScript_FileName .assert_Not_Null   ();
            newGuiScript_LocalPath.assert_Not_Null   ();
            newGuiScript_LocalPath.assert_File_Exists();

            var newGui = newGuiScript_LocalPath.compile_H2Script();

            assert_Not_Null(newGui);
        }
    }
}
