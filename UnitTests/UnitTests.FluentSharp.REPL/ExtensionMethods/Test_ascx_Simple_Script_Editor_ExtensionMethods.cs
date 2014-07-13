using FluentSharp.CoreLib;
using FluentSharp.NUnit;
using FluentSharp.REPL;
using FluentSharp.REPL.Controls;
using NUnit.Framework;

namespace UnitTests.FluentSharp_REPL.ExtensionMethods
{
    [TestFixture]
    public class Test_ascx_Simple_Script_Editor_ExtensionMethods : NUnitTests
    {
        [Test] public void packageCurrentScriptAsStandAloneExe()
        {            
            // see test  Invoke_Using_Reflection_Package_O2_Script_into_separate_Folder (in UnitTests.FluentSharp.MsBuild.Tools.Test_Package_O2_Script_into_separate_Folder)
            // for the check reflection invocation requirements
            
            "FluentSharp.MsBuild".assembly().assert_Null();                         // when this is null
            new ascx_Simple_Script_Editor().assert_Not_Null()
                                           .packageCurrentScriptAsStandAloneExe()   // this should also be null
                                           .assert_Null();
            
        }
    }
}
