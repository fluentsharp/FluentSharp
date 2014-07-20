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
        /// <summary>
        /// Assert that first value is biggger than second (same as assert_Is_Greater)
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int assert_Is_Bigger(this int target, int value)
        {
            return target.assert_Is_Greater(value);
        }
        /// <summary>
        /// Assert that first value is biggger than second
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int assert_Is_Greater(this int target, int value)            
        {
            Assert.Greater(target,value);
            return target;            
        }
        public static int assert_Bigger_Than(this int target, int value)
        {
            return target.assert_Is_Bigger(value);
        }
        /// <summary>
        /// Assert that first value is biggger than second (same as assert_Is_Less)
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int assert_Is_Smaller(this int target, int value)
        {
            return target.assert_Is_Less(value);
        }
        /// <summary>
        /// Assert that first value is biggger than second"
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int assert_Is_Less(this int target, int value)
        {
            Assert.Less(target,value);
            return target;
        }
        
        public static int assert_Smaller_Than(this int target, int value)
        {
            return target.assert_Is_Smaller(value);
        }
    }
}
