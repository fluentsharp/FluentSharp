using System;

namespace FluentSharp.NUnit
{
    public static class NUnitTests_ExtensionMethods_Bool
    {        
        public static NUnitTests nUnitTests = new NUnitTests();

        public static bool  assert_False        (this bool target, string message = NUnit_Messages.ASSERT_FALSE)
        {
            return target.assert_Is_False(message);
        }
        /// <summary>
        /// Asserts that value provided is false"
        /// </summary>
        /// <param name="target"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool  assert_Is_False     (this bool target, string message = NUnit_Messages.ASSERT_FALSE) 
        {
            return nUnitTests.assert_Is_False(target, message);            
        }        
        public static bool  assert_Is_Not_False (this bool target, string message = NUnit_Messages.ASSERT_TRUE) 
        {
            return nUnitTests.assert_Is_Not_False(target,message);            
        }        
        public static bool  assert_Is_Not_True  (this bool target, string message = NUnit_Messages.ASSERT_FALSE) 
        {
            return nUnitTests.assert_Is_Not_True(target,message);            
        }
        public static bool  assert_True         (this bool target, string message = NUnit_Messages.ASSERT_TRUE)
        {
            return target.assert_Is_True(message);
        }        
        public static bool  assert_Is_True      (this bool target, string message = NUnit_Messages.ASSERT_TRUE)
        {
            return nUnitTests.assert_Is_True(target, message);           
        }        
        public static T     assert_Is_True<T>   (this T target, Func<T,bool> callback , string message = NUnit_Messages.ASSERT_TRUE) 
        {
            nUnitTests.assert_Is_True(callback(target), message);            
            return target;
        }
    
        //With callbacks
        public static T  assert_True         <T>(this T target, Func<bool> callback, string message = NUnit_Messages.ASSERT_TRUE)
        {
            callback().assert_True(message);
            return target;
        }  
        public static T  assert_True         <T>(this T target, Func<T,bool> callback, string message = NUnit_Messages.ASSERT_TRUE)
        {
            callback(target).assert_True(message);
            return target;
        }
    }
}