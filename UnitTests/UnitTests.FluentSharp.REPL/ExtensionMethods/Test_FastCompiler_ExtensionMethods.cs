using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.NUnit;
using FluentSharp.REPL;
using FluentSharp.REPL.Utils;
using NUnit.Framework;

namespace UnitTests.FluentSharp_REPL.ExtensionMethods
{
    [TestFixture]
    public class Test_CSharp_FastCompiler_ExtensionMethods : NUnitTests
    {
        [Test] public void CSharp_FastCompiler_ExtensionMethods_Ctor_Static()
        {
            //note: this test is currently checking for the values set in CSharp_FastCompiler.setDefaultReferencedAssemblies()

            assert_Not_Null(CompileEngine.DefaultReferencedAssemblies);
            
            //after CompileEngine static invocation we should have 9 references including the FluentSharp.CoreLib.dll
            assert_True(typeof(CompileEngine).invoke_Ctor_Static());

            CompileEngine.DefaultReferencedAssemblies.assert_Not_Null    ()
                                                     .assert_Size_Is     (9)
                                                     .assert_Contains    ("System.Drawing.dll")
                                                     .assert_Contains    ("FluentSharp.CoreLib.dll")
                                                     .assert_Not_Contains("FluentSharp.Web.dll");

            CompileEngine.DefaultUsingStatements     .assert_Not_Null    ()
                                                     .assert_Size_Is     (10)
                                                     .assert_Contains    ("System.Drawing", 
                                                                          "System.Linq", 
                                                                          "System.Windows.Forms")
                                                     .assert_Contains    ("FluentSharp.CoreLib", 
                                                                          "FluentSharp.CoreLib.API", 
                                                                          "FluentSharp.CoreLib.Interfaces");

            //after FastCompiler_ExtensionMethods static invocation we should have 13 references including several more FluentSharp dlls
            assert_True(typeof(CSharp_FastCompiler_ExtensionMethods).invoke_Ctor_Static());

            CompileEngine.DefaultReferencedAssemblies.assert_Not_Null    ()
                                                     .assert_Size_Is     (13)
                                                     .assert_Contains    ("System.Drawing.dll")                         // these should still be there
                                                     .assert_Contains    ("FluentSharp.CoreLib.dll")
                                                     .assert_Contains    ("FluentSharp.REPL.exe")                       // with these being added by CSharp_FastCompiler_ExtensionMethods..ctor()
                                                     .assert_Contains    ("FluentSharp.WinForms.dll")
                                                     .assert_Contains    ("FluentSharp.SharpDevelopEditor.dll")                                                    
                                                     .assert_Contains    ("FluentSharp.Web_3_5.dll")
                                                     .assert_Not_Contains("WeifenLuo.WinFormsUI.Docking.dll");          // removed since this is now part of the FluentSharp.WinFormsUI assembly
            
            CompileEngine.DefaultUsingStatements     .assert_Not_Null    ()
                                                     .assert_Size_Is     (19)
                                                     .assert_Contains    ("System.Drawing",                             // these should still be there
                                                                          "FluentSharp.CoreLib.API") 
                                                     .assert_Contains    ("FluentSharp.WinForms",                       // with these being added by CSharp_FastCompiler_ExtensionMethods..ctor()
                                                                          "FluentSharp.WinForms.Controls",
                                                                          "FluentSharp.WinForms.Interfaces",
                                                                          "FluentSharp.WinForms.Utils",
                                                                          "FluentSharp.Web35",
                                                                          "FluentSharp.Web35.API",
                                                                          "FluentSharp.REPL",
                                                                          "FluentSharp.REPL.Controls",
                                                                          "FluentSharp.REPL.Utils");
        //FluentSharp.Web35.API
        
        }
        [Test] public void register_GitHub_As_ExternalAssemblerResolver()
        {
            AssemblyResolver.ExternalAssemblerResolver.clear();
            assert_Is_Empty(AssemblyResolver.ExternalAssemblerResolver);

            assert_True(typeof(CSharp_FastCompiler).invoke_Ctor_Static());        //this should call register_GitHub_As_ExternalAssemblerResolver
            
            assert_Is_Not_Empty(AssemblyResolver.ExternalAssemblerResolver);            
        }
    
    }
}
