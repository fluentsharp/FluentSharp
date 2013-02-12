using System.Windows.Forms;
using NUnit.Framework;
using O2.DotNetWrappers.ExtensionMethods;
using O2.Kernel;

namespace UnitTests.FluentSharp_REPL
{
    [TestFixture]
    class Test_ScriptEditor
    {
        [Test]
        public void OpenScriptEditor()
        {            
            var script = open.scriptEditor();                        
            script.inspector.onExecute =
                (result) => {
                                var panel = (Panel)script.inspector.InvocationParameters["panel"];
                                Assert.NotNull(panel);
                                var textBox = panel.control<TextBox>();
                                Assert.NotNull(textBox);
                                Assert.AreEqual("hello world",textBox.get_Text());
                                Assert.AreEqual("[null value]",result.str());
                                script.closeForm();
                };
            script.inspector.onCompileExecuteOnce();
            script.inspector.enableCodeComplete();
            script.waitForClose();
        }

    }
}
