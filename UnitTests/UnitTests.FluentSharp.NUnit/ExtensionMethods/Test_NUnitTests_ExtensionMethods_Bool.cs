using FluentSharp.NUnit;
using NUnit.Framework;

namespace UnitTests.FluentSharp_NUnit
{
    public class Test_NUnitTests_ExtensionMethods_Bool
    {
        [Test]
        public void assert_Is_False()
        {            
            false.assert_Is_False    ();
            true .assert_Is_True     ();
            true .assert_Is_Not_False();
            false.assert_Is_Not_True ();            
        }
    }
}
