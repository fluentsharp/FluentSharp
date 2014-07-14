using FluentSharp.CoreLib;
using NUnit.Framework;

namespace FluentSharp.NUnit
{
    public static class NUnitTests_ExtensionMethods_String
    {
        public static NUnitTests nUnitTests = new NUnitTests();
        
        //string
        public static string and_Contains(this string source, string target)
        {
            return source.assert_Contains(target);
        }
        public static string    assert_Contains(this string source,params string[] targets)       
        {
            foreach(var target in targets)
                nUnitTests.assert_Contains(source,target);
            return source;
        }
        public static string    assert_Not_Contains(this string target, params string[] values)
        {
            foreach(var value in values)
                Assert.False(target.contains(value), "value '{0}' was not expected to exist in string: \n\n {1}".format(value,target));
            return target;
        }
        public static string    assert_Valid(this string target)
        {
            return target.assert_Is_Valid();
        }
        public static string    assert_Is_Valid(this string target)
        {
            Assert.True(target.valid(), "string value provided was either null or empty");
            return target;
        }
        public static string    assert_Not_Valid(this string target)
        {
            return target.assert_Is_Not_Valid();
        }
        public static string    assert_Is_Not_Valid(this string target)
        {
            Assert.False(target.valid(), "string value provided was expected to be either null or empty, but it was: {0}".format(target));
            return target;
        }
    }
}