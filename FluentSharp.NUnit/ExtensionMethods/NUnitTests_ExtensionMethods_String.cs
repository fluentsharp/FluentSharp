using FluentSharp.CoreLib;
using NUnit.Framework;

namespace FluentSharp.NUnit
{
    public static class NUnitTests_ExtensionMethods_String
    {
        public static NUnitTests nUnitTests = new NUnitTests();
        
        //string
        public static string    assert_Contains(this string source, params string[] targets)       
        {
            foreach(var target in targets)
                nUnitTests.assert_Contains(source,target);
            return source;
        }
        public static string    assert_Not_Contains(this string target, params string[] values)
        {
            foreach(var value in values)
                Assert.False(target.contains(value), NUnit_Messages.ASSERT_NOT_CONTAINS.format(value,target));
            return target;
        }
        public static string    assert_Valid(this string target, string message = NUnit_Messages.ASSERT_IS_VALID)
        {
            return target.assert_Is_Valid(message);
        }
        public static string    assert_Is_Valid(this string target, string message = NUnit_Messages.ASSERT_IS_VALID)
        {
            Assert.True(target.valid(), message);
            return target;
        }
        public static string    assert_Not_Valid(this string target, string message = NUnit_Messages.ASSERT_IS_NOT_VALID)
        {
            return target.assert_Is_Not_Valid(message);
        }
        public static string    assert_Is_Not_Valid(this string target, string message = NUnit_Messages.ASSERT_IS_NOT_VALID)
        {
            Assert.False(target.valid(), NUnit_Messages.ASSERT_IS_NOT_VALID.format(target));
            return target;
        }
    }
}