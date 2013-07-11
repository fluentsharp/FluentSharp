using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentSharp.BCL;
using FluentSharp.CoreLib;
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

        [Test(Description = "Returns a compilable C# file from and Snippet")]
        public void tryToCreateCSharpCodeWith_Class_Method_WithMethodText()
        {
            Assert.IsEmpty(csharpFastCompiler.InvocationParameters);
            Assert.IsTrue(csharpFastCompiler.ResolveInvocationParametersType);

            var codeSnippet1 = "return 2+2;";
            var codeSnippet2 = "return 2+2; BAD CODE";
            var codeSnippet3 = "return parameterName;";

            //Compile with no Invocation parameters
            var cSharpCode1 = csharpFastCompiler.tryToCreateCSharpCodeWith_Class_Method_WithMethodText(codeSnippet1);
            var cSharpCode2 = csharpFastCompiler.tryToCreateCSharpCodeWith_Class_Method_WithMethodText(codeSnippet2);
            var cSharpCode3 = csharpFastCompiler.tryToCreateCSharpCodeWith_Class_Method_WithMethodText(codeSnippet3);
            var assembly1 = csharpFastCompiler.compileSourceCode_Sync(cSharpCode1);
            var assembly2 = csharpFastCompiler.compileSourceCode_Sync(cSharpCode2);
            var assembly3 = csharpFastCompiler.compileSourceCode_Sync(cSharpCode3);
            Assert.IsTrue(cSharpCode1.valid());
            Assert.IsFalse(cSharpCode2.valid());
            Assert.IsTrue(cSharpCode3.valid());
            Assert.IsNotNull(assembly1);
            Assert.IsNull(assembly2);
            Assert.IsNull(assembly3);

            //Check invocation parameters
            csharpFastCompiler.InvocationParameters.Add("parameterName", "parameterValue");
            var cSharpCode4      = csharpFastCompiler.tryToCreateCSharpCodeWith_Class_Method_WithMethodText(codeSnippet3);
            var assembly4        = csharpFastCompiler.compileSourceCode_Sync(cSharpCode4);
            var firstMethod      = assembly4.firstMethod();
            var methodParameters = firstMethod.parameters();
            var firstParameter   = methodParameters.first();
            var parameterName    = firstParameter.Name;
            var parameterType    = firstParameter.ParameterType.fullName();

            Assert.IsTrue(cSharpCode4.valid());
            Assert.IsNotNull(assembly4);
            Assert.IsNotNull(firstMethod);
            Assert.AreEqual(methodParameters.size(), 1);
            Assert.IsNotNull(firstParameter);
            Assert.AreEqual(parameterName, "parameterName");
            Assert.AreEqual(parameterType, "System.String");
            
            //todo: add code and test for extra return null; (which shouldn't be there when the last statement is a return
        }


        [Test]
        public void Check_Dynamic_InvocationParameters()
        {
            Assert.IsEmpty(csharpFastCompiler.InvocationParameters);
            csharpFastCompiler.InvocationParameters.Add("parameterName", "parameterValue");
            csharpFastCompiler.ResolveInvocationParametersType = false;
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
