using FluentSharp.CoreLib.API;
using NUnit.Framework;
using FluentSharp.CoreLib;

namespace UnitTests.FluentSharp_CoreLib
{
    [TestFixture]
    public class Test_H2_FileFormat
    {
        [Test]
        public void LoadAndSaveH2File()
        {
            var codeSnippet = "return 42;";
            var h2 = new H2(codeSnippet);
            var h2Saved = h2.save();
            "h2Saved: {0}".info(h2Saved);
            var h2Loaded = H2.load(h2Saved);

            Assert.IsNotNull(h2);
            Assert.IsTrue(h2Saved.fileExists());
            Assert.IsNotNull(h2Loaded);
            Assert.AreEqual(h2Loaded.SourceCode, codeSnippet);

            var tempH2 = "test.h2".tempFile();
            h2.save(tempH2);
            var tempH2Loaded = H2.load(tempH2);
            var h2SourceCode = tempH2.h2_SourceCode();

            Assert.IsTrue(tempH2.fileExists());
            Assert.IsNotNull(tempH2Loaded);
            Assert.AreEqual(tempH2Loaded.SourceCode, codeSnippet);
            Assert.IsNotNull(h2SourceCode);
            Assert.AreEqual(h2SourceCode, codeSnippet);
        }

        [Test]
        public void ExecuteH2File()
        {
            var codeSnippet = "return 42;";
            var h2 = new H2(codeSnippet);
            var h2File = "test.h2".tempFile();
            h2.save(h2File);

            var h2SourceCode = h2File.h2_SourceCode();
            var assembly = h2SourceCode.compileCodeSnippet();
            var result = assembly.firstMethod().invoke();

            Assert.NotNull(h2SourceCode);
            Assert.NotNull(assembly);
            Assert.NotNull(result);
            Assert.AreEqual(result, 42);
        }

        [Test]
        public void LoadAndSaveH2File_AsFlatFiles()
        {
            var codeSnippet = "return 42;";
            var h2File = "test.h2".tempFile();
            codeSnippet.saveAs(h2File);

            var h2SourceCode = h2File.h2_SourceCode();
            var assembly = h2SourceCode.compileCodeSnippet();
            var result = assembly.firstMethod().invoke();

            Assert.NotNull(h2SourceCode);
            Assert.NotNull(assembly);
            Assert.NotNull(result);
            Assert.AreEqual(result, 42);

            var h2FileSave = "test.h2".tempFile();
            new H2(codeSnippet).save(h2FileSave);

            Assert.IsTrue(h2FileSave.fileExists());
            Assert.AreNotEqual(h2File, h2FileSave);
            Assert.AreEqual   (h2File.fileContents_AsByteArray().ascii(), h2FileSave.fileContents_AsByteArray().ascii());
            Assert.AreEqual   (h2File.fileContents()                    , h2FileSave.fileContents());

        }
    }
}
