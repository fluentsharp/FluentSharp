using System.Windows.Forms;
using FluentSharp.BCL;
using FluentSharp.REPL;
using FluentSharp.REPL.Utils;
using NUnit.Framework;
using FluentSharp.CoreLib;

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
            //script.inspector.enableCodeComplete();
            script.waitForClose();
        }

    }
}
