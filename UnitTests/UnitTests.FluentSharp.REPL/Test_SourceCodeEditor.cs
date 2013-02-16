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
        public ascx_SourceCodeEditor CodeEditor { get; set; }

        [SetUp]
        public void SetUp()
        {            
            CodeEditor = Default_Helpers.CSharpFile_HelloWord
                                        .open_InCodeEditor();            
        }

        [TearDown]
        public void tearDown()
        {            
            CodeEditor.close()
                      .waitForClose();
        }

        [Test]
        public void OpenAndCompileTestFile()
        {            
            var assembly = CodeEditor.compileCSSharpFile();
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
            var selectedTextBeforeSearch = CodeEditor.selectedText();            
            CodeEditor.searchForTextInTextEditor(textToSearch);
            var selectedTextAfterSearch  = CodeEditor.selectedText();

            Assert.AreNotEqual(textToSearch, selectedTextBeforeSearch);
            Assert.AreEqual   (textToSearch, selectedTextAfterSearch);

            "textToSearch            : {0}".info(textToSearch);
            "selectedTextBeforeSearch: {0}".info(selectedTextBeforeSearch);
            "selectedTextAfterSearch : {0}".info(selectedTextAfterSearch);            
        }

        //codeEditor.script_Me("codeEditor").waitForClose();
    }
}
