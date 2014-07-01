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

            CompileEngine.DefaultReferencedAssemblies.assert_Not_Null()
                                                     .assert_Size_Is(9)
                                                     .assert_Contains    ("System.Drawing.dll")
                                                     .assert_Contains    ("FluentSharp.CoreLib.dll")
                                                     .assert_Not_Contains("FluentSharp.Web.dll");

            //after FastCompiler_ExtensionMethods static invocation we should have 13 references including several more FluentSharp dlls
            assert_True(typeof(CSharp_FastCompiler_ExtensionMethods).invoke_Ctor_Static());

            CompileEngine.DefaultReferencedAssemblies.assert_Not_Null()
                                                     .assert_Size_Is(13)
                                                     .assert_Contains    ("System.Drawing.dll")
                                                     .assert_Contains    ("FluentSharp.CoreLib.dll")
                                                     .assert_Contains    ("FluentSharp.REPL.exe")
                                                     .assert_Contains    ("FluentSharp.WinForms.dll")
                                                     .assert_Contains    ("FluentSharp.SharpDevelopEditor.dll")                                                    
                                                     .assert_Contains    ("FluentSharp.Web.dll")
                                                     .assert_Not_Contains("WeifenLuo.WinFormsUI.Docking.dll");          // removed since this is now part of the FluentSharp.WinFormsUI assembly
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
