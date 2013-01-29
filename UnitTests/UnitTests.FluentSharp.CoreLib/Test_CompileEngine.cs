using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using O2.DotNetWrappers.ExtensionMethods;

namespace UnitTests.FluentSharp.CoreLib
{
    [TestFixture]
    public class Test_CompileEngine
    {
        [Test]
        public void compileAndExecuteCodeSnippet()
        {
            //try with good script
            var snippet = "return 12;";            
			var compileError = "";
			Action<string> onCompileOk = (msg) => { };
			Action<string> onCompileFail = (msg) => { compileError = msg; };
			var result = snippet.fixCRLF().compileAndExecuteCodeSnippet(onCompileOk, onCompileFail);
            Assert.AreEqual("",compileError, "there were compile errors");
            Assert.That    (result is Int32, "result should be an int");
            Assert.AreEqual(result, 12, "result should be 12");
            
            //try with bad script
            snippet = "AAAA";
            result = snippet.fixCRLF().compileAndExecuteCodeSnippet(onCompileOk, onCompileFail);
			Assert.AreNotEqual("",compileError, "compile errors were expected");
            Assert.IsNull     (result, "result should be null");            
        }
    }
}
