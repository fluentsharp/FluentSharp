using NUnit.Framework;
using O2.DotNetWrappers.ExtensionMethods;
using O2.XRules.Database.Utils;

namespace UnitTests.FluentSharp_REPL
{
    [TestFixture]
    public class Test_ScriptMe
    {
        public string TestValue                  { get; set; }
        public ascx_Simple_Script_Editor CodeEditor { get; set; }

        [SetUp]
        public void SetUp()
        {
            TestValue = "a test Value";
            CodeEditor = TestValue.script_Me("testValue");
        }

        [TearDown]
        public void tearDown()
        {            
            CodeEditor.close()
                      .waitForClose();
        }

        [Test]
        public void CheckExecution()
        {
            var sync = false.sync();
            CodeEditor.onExecute = (result) => sync.set();
            CodeEditor.execute();

            sync.waitOne();
            
            Assert.AreEqual(CodeEditor.LastExecutionResult,TestValue);                     
        }

    }
}
