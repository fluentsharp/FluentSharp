using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.NUnit;
using FluentSharp.REPL;
using FluentSharp.REPL.Utils;
using FluentSharp.WinForms;
using NUnit.Framework;

namespace UnitTests.FluentSharp.O2Platform
{
    //This class checks that the main O2 Platform scripts still compile OK
    [TestFixture]
    public class Workflow_Tests_Key_Script_Compilation : NUnitTests
    {
        [Test]
        public void Main_O2_Gui_h2()
        {

            //CompileEngine.clearLocalScriptFileMappings();

            var scriptName = "Main O2 Gui.h2"; // this is the script used in ascx_Execute_Scripts.NEW_GUI_SCRIPT
            var file       = scriptName.local();
            var h2Code     = file.h2_SourceCode();


            //compile using internal methods

            var csharpCode = new CSharp_FastCompiler().createCSharpCodeWith_Class_Method_WithMethodText(h2Code);
                   
            assert_Not_Null(scriptName);
            assert_Not_Null(file);
            assert_Not_Null(h2Code);
            assert_Not_Null(csharpCode);

            csharpCode.assert_Contains("public class", "DynamicType", "{", "}")
                      .assert_Contains("using System;","System.Linq;")
                      .assert_Contains("FluentSharp.REPL.Utils");             
            
            var compileEngine = new CompileEngine();
            var assembly = compileEngine.compileSourceCode(csharpCode);

            assert_Is_Null(compileEngine.sbErrorMessage);
            assert_Not_Null(assembly);

            //check that we can also compile using the main wrapper methods
            assert_Not_Null(file.compile_H2Script());                                 
        }
    }
}
