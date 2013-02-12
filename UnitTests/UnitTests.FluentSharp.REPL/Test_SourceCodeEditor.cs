using System;
using System.Reflection;
using NUnit.Framework;
using O2.DotNetWrappers.ExtensionMethods;
using O2.External.SharpDevelop.Ascx;
using O2.External.SharpDevelop.ExtensionMethods;

namespace UnitTests.FluentSharp_REPL
{
    [TestFixture]
    public class Test_SourceCodeEditor
    {
        public ascx_SourceCodeEditor codeEditor;
        [SetUp]
        public void SetUp()
        {
            var codeSnippet = @"return ""Hello World"";";
            var testFile    = codeSnippet.createCSharpFileFromCodeSnippet();

            codeEditor  = "SourceCodeEditor".popupWindow()
                                            .add_SourceCodeEditor()
                                            .open(testFile);            
        }

        [TearDown]
        public void tearDown()
        {
            codeEditor.closeForm_InNSeconds(0);
            codeEditor.waitForClose();
        }

        [Test]
        public void OpenAndCompileTestFile()
        {            
            var assembly = codeEditor.compileCSSharpFile();
            var type     = assembly.type("DynamicType");
            var method   = type.method("dynamicMethod");
            var result   = method.invoke();
            
            Assert.IsInstanceOf<Assembly>   (assembly);
            Assert.IsInstanceOf<Type>       (type);
            Assert.IsInstanceOf<MethodInfo> (method);
            Assert.IsInstanceOf<string>     (result);

            Assert.AreEqual("DynamicType"  , type.Name);
            Assert.AreEqual("dynamicMethod" , method.Name);
            Assert.AreEqual("Hello World"    , result);         
        }

        [Test]
        public void SearchForText()
        {            
            var textToSearch             = "Hello";
            var selectedTextBeforeSearch = codeEditor.selectedText();            
            codeEditor.searchForTextInTextEditor(textToSearch);
            var selectedTextAfterSearch  = codeEditor.selectedText();

            Assert.AreNotEqual(textToSearch, selectedTextBeforeSearch);
            Assert.AreEqual   (textToSearch, selectedTextAfterSearch);

            "textToSearch            : {0}".info(textToSearch);
            "selectedTextBeforeSearch: {0}".info(selectedTextBeforeSearch);
            "selectedTextAfterSearch : {0}".info(selectedTextAfterSearch);            
        }

        //codeEditor.script_Me("codeEditor").waitForClose();
    }
}
