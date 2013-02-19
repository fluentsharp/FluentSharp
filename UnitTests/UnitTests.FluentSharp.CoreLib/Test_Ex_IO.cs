
using O2.DotNetWrappers.ExtensionMethods;
using NUnit.Framework;

namespace UnitTests.FluentSharp_CoreLib
{
    [TestFixture]
    public class Test_Ex_IO
    {
        [Test]
        public void isBinaryFormat()
        {
            var result_TextFile      = "test".save().isBinaryFormat();
            var result_TextWithChar0 = "aaa\0aaa".save().isBinaryFormat();
            var result_Assembly      = typeof (string).assemblyLocation().isBinaryFormat();

            Assert.IsFalse(result_TextFile);
            Assert.IsTrue (result_TextWithChar0);
            Assert.IsTrue (result_Assembly);
        }
    }
}
