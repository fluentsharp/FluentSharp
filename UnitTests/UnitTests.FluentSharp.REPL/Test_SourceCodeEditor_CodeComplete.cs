using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;
using O2.DotNetWrappers.ExtensionMethods;
using O2.External.SharpDevelop.Ascx;
using O2.External.SharpDevelop.ExtensionMethods;

namespace UnitTests.FluentSharp_REPL
{
    [TestFixture]
    public class Test_SourceCodeEditor_CodeComplete
    {
        public string TestFile                  { get; set; }
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
        public void CheckCodeCompleteReferences()
        {
            var referenceToLoad = "FluentSharp.REPL.exe";
            CodeEditor.compileCSSharpFile();
            var o2CodeComplete = CodeEditor.o2CodeCompletion;
            Assert.IsNotNull(o2CodeComplete);

            var sync = false.sync();
            o2CodeComplete.onCompleted_AddReferences +=
                ()=>{
                        sync.Set();
                     };
                    

            sync.waitOne();
            var loadedReferences = o2CodeComplete.loadedReferences.fileNames();
            
            Assert.Less(5, loadedReferences.size());
            Assert.IsFalse(loadedReferences.contains(referenceToLoad));
            var code = CodeEditor.get_Text();
            Assert.IsNotNullOrEmpty(code);

            //add new code reference and compile again
            var textToInsert = "//O2Ref:{0}".format(referenceToLoad).lineBeforeAndAfter();
            CodeEditor.insert_Text(textToInsert);
            CodeEditor.saveSourceCode()
                      .compileCSSharpFile();
            sync.reset()
                .waitOne();
            var newLoadedReferences = o2CodeComplete.loadedReferences.fileNames();

            Assert.AreNotEqual(newLoadedReferences.size(), loadedReferences.size());
            Assert.IsTrue(newLoadedReferences.lower().contains(referenceToLoad.lower()));

            CodeEditor.waitForClose();
            //CodeEditor.script_Me("CodeEditor")
            //          .waitForClose();

            //return CodeEditor.o2CodeCompletion.loadedReferences.fileNames();

//O2Ref:FluentSharp.REPL.EXE
        }
    }
}
