using FluentSharp.NUnit;
using NUnit.Framework;

namespace UnitTests.FluentSharp.NUnit
{
    [TestFixture]
    public class Test_NUnitTests_ExtensionMethods
    {
        [Test] public void assert_Is_Bigger()
        {
            20.assert_Is_Bigger(10);            
        }
        [Test] public void assert_Is_Greater()
        {
            20.assert_Is_Greater(10);
            20.assert_Is_Greater(default(int));
            Assert.Throws<AssertionException>(()=> 10          .assert_Is_Greater(20));
            Assert.Throws<AssertionException>(()=> default(int).assert_Is_Greater(20));
        }
        [Test] public void assert_Is_Smaller()
        {
            10.assert_Is_Smaller(20);            
        }
        [Test] public void assert_Is_Less()
        {
            10          .assert_Is_Less(20);
            default(int).assert_Is_Less(20);
            Assert.Throws<AssertionException>(()=> 20.assert_Is_Less(10));
            Assert.Throws<AssertionException>(()=> 20.assert_Is_Less(default(int)));
        }
    }
}