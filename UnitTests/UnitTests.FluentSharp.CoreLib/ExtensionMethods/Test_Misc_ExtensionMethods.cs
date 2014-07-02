using FluentSharp.CoreLib;
using NUnit.Framework;

namespace UnitTests.FluentSharp_CoreLib.ExtensionMethods
{
    [TestFixture]
    public class Test_Misc_ExtensionMethods
    {
        [Test] public void isDouble()
        {
            Assert.IsTrue ("123".isDouble());
            Assert.IsTrue ("123123123123213".isDouble());
            Assert.IsTrue ("123123123123213123123".isDouble());
            Assert.IsTrue ("123123123123213123123123123213".isDouble());
            Assert.IsTrue ("123123123123213123123123123213123123123213123123123123211341412341234213421342134".isDouble());
            Assert.IsFalse("a".isDouble());
            Assert.IsFalse("a123".isDouble());
            Assert.IsFalse("a123".isDouble());
            Assert.IsFalse((null as string).isDouble());
        }
    }
}
