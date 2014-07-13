using System;

namespace FluentSharp.NUnit
{
    public static class NUnitTests_ExtensionMethods_Bool
    {        
        public static NUnitTests nUnitTests = new NUnitTests();

        public static bool  assert_False(this bool target)
        {
            return target.assert_Is_False();
        }
        /// <summary>
        /// Asserts that value provided is false"
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool  assert_Is_False(this bool target) 
        {
            return nUnitTests.assert_Is_False(target);            
        }        
        public static bool  assert_Is_Not_False(this bool target) 
        {
            return nUnitTests.assert_Is_Not_False(target);            
        }        
        public static bool  assert_Is_Not_True(this bool target) 
        {
            return nUnitTests.assert_Is_Not_True(target);            
        }
        public static bool  assert_True(this bool value, string message = "Assert.IsTrue")
        {
            return value.assert_Is_True(message);
        }        
        public static bool  assert_Is_True(this bool target, string message = "Assert.IsTrue")
        {
            return nUnitTests.assert_Is_True(target, message);           
        }        
        public static T     assert_Is_True<T>(this T target, Func<T,bool> callback) 
        {
            nUnitTests.assert_Is_True(callback(target));            
            return target;
        }
    }
}