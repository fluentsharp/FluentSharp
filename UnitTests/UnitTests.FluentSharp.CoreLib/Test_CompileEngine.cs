using System;
using FluentSharp.CoreLib.API;
using FluentSharp.NUnit;
using NUnit.Framework;
using FluentSharp.CoreLib;

namespace UnitTests.FluentSharp_CoreLib
{
    [TestFixture]
    public class Test_CompileEngine : NUnitTests
    {
        [Test] public void CompileEngine_Ctor()
        {
            CompileEngine.clearCompilationCache();
            // call the static Ctor to reset the values
            typeof(CompileEngine).invoke_Ctor_Static();
            
            var compileEngine = new CompileEngine();
            assert_Is_True  (compileEngine.useCachedAssemblyIfAvailable);
            assert_Are_Equal(compileEngine.compilationVersion, "v4.0");
            assert_Not_Empty(CompileEngine.LocalReferenceFolders);
            
            
            assert_Not_Empty(CompileEngine.DefaultReferencedAssemblies);
            assert_Not_Empty(CompileEngine.DefaultUsingStatements);                        
            assert_Not_Empty(CompileEngine.CachedCompiledAssembliesMappingsFile);
            assert_Not_Empty(CompileEngine._specialO2Tag_ExtraReferences);

            assert_Is_Empty(CompileEngine.CachedCompiledAssemblies);            
            assert_Is_Empty (CompileEngine.CompilationPathMappings);
            assert_Is_Empty (CompileEngine.LocalScriptFileMappings);
            assert_Is_Empty (CompileEngine.LocalFoldersToSearchForCodeFiles);
            assert_Is_Empty (CompileEngine.LocalFoldersToAddToFileMappings); 

        }
        [Test] public void CompileAndExecuteCodeSnippet() 
        {            
            //try with good script
            var snippet = "return 12;";
            var compileError = "";
            Action<string> onCompileOk = (msg) => { };
            Action<string> onCompileFail = (msg) => { compileError = msg; };
            var result = snippet.fix_CRLF().compileAndExecuteCodeSnippet(onCompileOk, onCompileFail);
            Assert.AreEqual("", compileError, "there were compile errors");
            Assert.That(result is Int32, "result should be an int");
            Assert.AreEqual(result, 12, "result should be 12");

            //try with bad script
            snippet = "AAAA";
            result = snippet.fix_CRLF().compileAndExecuteCodeSnippet(onCompileOk, onCompileFail);
            Assert.AreNotEqual("", compileError, "compile errors were expected");
            Assert.IsNull(result, "result should be null");
        }
        [Test] public void LocalScriptFolders()
        {
            var localScripts      = CompileEngine.LocalScriptFileMappings;
            var localScriptFolder = PublicDI.config.LocalScriptsFolder.createDir();
            var fileName          = "testFile{0}.cs".format(4.randomLetters());                                  
            var fileContents      = "some code";
            var testFile          = localScriptFolder.pathCombine(fileName);            

            Assert.IsNotNull(localScripts);
            //Assert.IsEmpty  (localScripts);
            Assert.AreEqual (fileName.local() , "");
            Assert.IsNotNull(localScriptFolder);            
            
            fileContents.saveAs(testFile);
            CompileEngine.resetLocalScriptsFileMappings();

            Assert.IsTrue    (testFile.fileExists() , "File didn't exist: {0}".format(testFile));
            Assert.AreEqual  (fileContents, testFile.fileContents());
            Assert.IsNotEmpty(localScripts);
            Assert.IsNotNull (fileName.local());
            Assert.AreEqual  (fileName.local(), testFile);
            Assert.IsFalse   (localScripts.hasKey(fileName)         , "localscripts");
            Assert.IsTrue    (localScripts.hasKey(fileName.lower()) , "localscripts (lowercase search)");
            Assert.AreEqual  (localScripts[fileName.lower()], testFile);

            //ExtraLocalScriptFolders
            
            var tempDir = "ExtraScripts".tempDir();
            var extraFileName = "extraFile.cs";
            var extraFile = tempDir.pathCombine("extraFile.cs");
            var extraFileContents = "some more code";
            extraFileContents.saveAs(extraFile);
            
            Assert.IsFalse   (localScripts.hasKey(fileName) );
            Assert.AreEqual  (extraFileName.local(),"");

            CompileEngine.LocalFoldersToAddToFileMappings.add(tempDir);
            CompileEngine.resetLocalScriptsFileMappings();

            Assert.IsFalse  (localScripts.hasKey(extraFileName));
            Assert.IsTrue   (localScripts.hasKey(extraFileName.lower()));
            Assert.AreEqual (extraFileName.local(),extraFile);
            Assert.AreEqual (extraFileName.local().fileContents(),extraFileContents);
        }
    }
}
