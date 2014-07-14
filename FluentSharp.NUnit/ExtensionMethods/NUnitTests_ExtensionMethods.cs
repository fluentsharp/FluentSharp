using NUnit.Framework;

namespace FluentSharp.NUnit
{
    public static class NUnitTests_ExtensionMethods
    {
        public static void assert_Fail(this string message)
        {
            Assert.Fail(message);
        }

        public static void assert_Ignore(this string message)
        {
            Assert.Ignore(message);
        }
    }
}
