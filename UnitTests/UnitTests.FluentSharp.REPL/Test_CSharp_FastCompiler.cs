using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.NUnit;
using FluentSharp.REPL;
using FluentSharp.REPL.Utils;
using NUnit.Framework;

namespace UnitTests.FluentSharp_REPL
{
    [TestFixture]
    internal class Test_CSharp_FastCompiler
    {
        public CSharp_FastCompiler csharpFastCompiler;

        [SetUp]
        public void SetUp()
        {
            csharpFastCompiler = new CSharp_FastCompiler();
        }

        [Test] public void CSharp_FastCompiler_Ctor_Static()
        {
            //note: this test is currently checking for the values set in CSharp_FastCompiler.setDefaultReferencedAssemblies()

            CompileEngine.DefaultReferencedAssemblies.assert_Not_Null();
            
            //after CompileEngine static invocation we should have 9 references including the FluentSharp.CoreLib.dll
            typeof(CompileEngine).invoke_Ctor_Static().assert_True();

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
            typeof(CSharp_FastCompiler).invoke_Ctor_Static().assert_True();

            CompileEngine.DefaultReferencedAssemblies.assert_Not_Null    ()
                                                     .assert_Size_Is     (14)
                                                     .assert_Contains    ("System.Drawing.dll",                         // these should still be there
                                                                          "FluentSharp.CoreLib.dll",
                                                                          "FluentSharp.REPL.exe",                       // with these being added by CSharp_FastCompiler_ExtensionMethods..ctor()
                                                                          "FluentSharp.WinForms.dll",
                                                                          "FluentSharp.SharpDevelopEditor.dll",                                                  
                                                                          "FluentSharp.Web_3_5.dll",
                                                                          "FluentSharp.Zip.dll")
                                                     .assert_Not_Contains("WeifenLuo.WinFormsUI.Docking.dll");          // removed since this is now part of the FluentSharp.WinFormsUI assembly
            
            CompileEngine.DefaultUsingStatements     .assert_Not_Null    ()
                                                     .assert_Size_Is     (20)
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
                                                                          "FluentSharp.REPL.Utils",
                                                                          "FluentSharp.Zip");
        //FluentSharp.Web35.API
        
        }
        //ExtensionMethods
        [Test]
        public void tryToCreateCSharpCodeWith_Class_Method_WithMethodText()
        {
            Assert.IsEmpty(csharpFastCompiler.invocationParameters());
            Assert.IsTrue (csharpFastCompiler.resolveInvocationParametersType());

            var codeSnippet1 = "return 2+2;";
            var codeSnippet2 = "return 2+2; BAD CODE";
            var codeSnippet3 = "return parameterName;";
            var codeSnippet4 = "var a = 2+2; a.str().info();";

            //Compile with no Invocation parameters
            var cSharpCode1 = csharpFastCompiler.tryToCreateCSharpCodeWith_Class_Method_WithMethodText(codeSnippet1).assert_Valid();
            var cSharpCode2 = csharpFastCompiler.tryToCreateCSharpCodeWith_Class_Method_WithMethodText(codeSnippet2).assert_Not_Valid();
            var cSharpCode3 = csharpFastCompiler.tryToCreateCSharpCodeWith_Class_Method_WithMethodText(codeSnippet3).assert_Valid();
            var cSharpCode4 = csharpFastCompiler.tryToCreateCSharpCodeWith_Class_Method_WithMethodText(codeSnippet4).assert_Valid();
                              
            csharpFastCompiler.compileSourceCode_Sync(cSharpCode1).assert_Not_Null();
            csharpFastCompiler.compileSourceCode_Sync(cSharpCode2).assert_Null();
            csharpFastCompiler.compileSourceCode_Sync(cSharpCode3).assert_Null();            
            csharpFastCompiler.compileSourceCode_Sync(cSharpCode4).assert_Not_Null();

            //Check invocation parameters
            csharpFastCompiler.invocationParameters().Add("parameterName", "parameterValue");
            var cSharpCode5      = csharpFastCompiler.tryToCreateCSharpCodeWith_Class_Method_WithMethodText(codeSnippet3);
            var assembly4        = csharpFastCompiler.compileSourceCode_Sync(cSharpCode5);
            var firstMethod      = assembly4.firstMethod();
            var methodParameters = firstMethod.parameters();
            var firstParameter   = methodParameters.first();
            var parameterName    = firstParameter.Name;
            var parameterType    = firstParameter.ParameterType.fullName();

            Assert.IsTrue(cSharpCode5.valid());
            Assert.IsNotNull(assembly4);
            Assert.IsNotNull(firstMethod);
            Assert.AreEqual(methodParameters.size(), 1);
            Assert.IsNotNull(firstParameter);
            Assert.AreEqual(parameterName, "parameterName");
            Assert.AreEqual(parameterType, "System.String");
            
            //Check that return doesn't exist when the last line is a return            
            
        }

        //Workflows                
        [Test]
        public void Check_Dynamic_InvocationParameters()
        {
            Assert.IsEmpty(csharpFastCompiler.invocationParameters());
            csharpFastCompiler.invocationParameters().Add("parameterName", "parameterValue");
            csharpFastCompiler.resolveInvocationParametersType(false);
            var codeSnippet     = "return parameterName;";            
            var cSharpCode      = csharpFastCompiler.tryToCreateCSharpCodeWith_Class_Method_WithMethodText(codeSnippet);
            var assembly        = csharpFastCompiler.compileSourceCode_Sync(cSharpCode);
            var firstParameter  = assembly.firstMethod().parameters().first();
            var parameterName   = firstParameter.Name;
            var parameterType   = firstParameter.ParameterType.fullName();

            Assert.IsTrue   (cSharpCode.valid());
            Assert.IsNotNull(assembly);                        
            Assert.IsNotNull(firstParameter);
            Assert.AreEqual (parameterName, "parameterName");
            Assert.AreEqual (parameterType, "System.Object");        // note: in the source code the parameter is dynamic    
        }
    }
}
