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

        [SetUp]     public void SetUp()
        {            
            CodeEditor = Default_Helpers.CSharpFile_HelloWord
                                        .open_InCodeEditor();            
        }
        [TearDown]  public void tearDown()
        {            
            CodeEditor.close()
                      .waitForClose();
        }

        [Test] public void OpenAndCompileTestFile()
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
        [Test] public void SearchForText()
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
        [Test] public void LoadH2FileInCodeEditor()
        {
            var h2File = "aaa.h2".tempFile();
            "var a = 12;\nreturn a;".saveAs(h2File);
            var h2     = h2File.h2();
            Assert.IsTrue   (h2File.fileExists());
            Assert.IsNotNull(h2);
            CodeEditor.open(h2File);
            Assert.AreEqual(CodeEditor.get_Code().fixCRLF(), h2File.fileContents());
        }
        [Test,Ignore("Fails in TeamCity")] public void CheckCodeCompleteReferences()
        {
            //var referenceToLoad = "FluentSharp.REPL.exe";
            CodeEditor.compileCSSharpFile();
            var o2CodeComplete = CodeEditor.o2CodeCompletion;
            Assert.IsNotNull(o2CodeComplete);

            var sync = false.sync();
            o2CodeComplete.onCompleted_AddReferences += ()=> sync.Set();
                
            if (sync.waitOne(30 * 1000).isFalse())
                Assert.Fail("Timeout on first sync");
            sync.reset();
            var loadedReferences = o2CodeComplete.loadedReferences.fileNames();
            
            Assert.Less(5, loadedReferences.size());

            var testAssembly = "return {0};".format(1000000.randomNumber()).compileCodeSnippet();
            Assert.IsTrue(testAssembly.Location.valid());
            var referenceToLoad = testAssembly.Location.fileName();

            Assert.IsFalse(loadedReferences.contains(referenceToLoad));
            var code = CodeEditor.get_Text();
            Assert.IsNotNullOrEmpty(code);

            //add new code reference and compile again
            var textToInsert = "//O2Ref:{0}".format(referenceToLoad).line();
            CodeEditor.insert_Text(textToInsert);
            CodeEditor.saveSourceCode()
                      .compileCSSharpFile();         
   
            if (sync.waitOne(30 * 1000).isFalse())
                Assert.Fail("Timeout on 2nd sync");
            var newLoadedReferences = o2CodeComplete.loadedReferences.fileNames();

            Assert.AreNotEqual(newLoadedReferences.size(), loadedReferences.size());
            Assert.IsTrue(newLoadedReferences.lower().contains(referenceToLoad.lower()));                   
        }
        
        
        //codeEditor.script_Me("codeEditor").waitForClose();
    }
}
