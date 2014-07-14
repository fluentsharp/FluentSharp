using NUnit.Framework;

namespace FluentSharp.NUnit
{
    public static class NUnitTests_ExtensionMethods
    {
        public static NUnitTests nUnitTests = new NUnitTests();
        public static void assert_Fail(this string message)
        {
            Assert.Fail(message);
        }

        public static void assert_Ignore(this string message)
        {
            Assert.Ignore(message);
        }

        public static int assert_Is_Bigger(this int target, int value)
        {
            return nUnitTests.assert_Is_Bigger(target,value);
        }
        public static int assert_Bigger_Than(this int target, int value)
        {
            return target.assert_Is_Bigger(value);
        }
        public static int assert_Is_Smaller(this int target, int value)
        {
            return nUnitTests.assert_Is_Smaller(target,value);
        }
        public static int assert_Smaller_Than(this int target, int value)
        {
            return target.assert_Is_Smaller(value);
        }
    }
}
