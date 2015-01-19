using System;
using FluentSharp.CoreLib;
using NUnit.Framework;

namespace FluentSharp.NUnit
{
    public static class NUnitTests_ExtensionMethods_T_Comparisson
    {
        public static NUnitTests nUnitTests = new NUnitTests();
        //T
        public static T     assert_Contains<T>        (this T source, Func<string> callback, params string[] targets)
        {
            foreach(var target in targets)
                nUnitTests.assert_Contains(callback(), target);
            return source;
        }                
        public static T     assert_Are_Equal<T,T1>    (this T source, Func<T1> callback, T1 target)
        {
            nUnitTests.assert_Are_Equal(callback(), target);
            return source;
        }
        public static T     assert_Are_Equal<T,T1>    (this T source, Func<T,T1> callback, T1 target)
        {
            nUnitTests.assert_Are_Equal(callback(source), target);
            return source;
        }
        public static T     assert_Equal<T>           (this T target, T value, string message = NUnit_Messages.ASSERT_ARE_EQUAL)
        {
            Assert.AreEqual(target, value, message);
            return target;
        }        
        public static T     assert_Equals<T>          (this T source, T target, string message = NUnit_Messages.ASSERT_ARE_EQUAL)
        {
            return assert_Equal_To(source, target, message);
        }
        public static T     assert_Equal_To<T>        (this T source, T target, string message = NUnit_Messages.ASSERT_ARE_EQUAL)
        {            
            return source.assert_Is_Equal_To(target, message);
        }
        public static T     assert_Is<T>              (this T source, T target, string message = NUnit_Messages.ASSERT_ARE_EQUAL)    
        {
            return source.assert_Is_Equal_To(target, message);
        }
        public static T     assert_Is_Not<T>          (this T source, T target, string message = NUnit_Messages.ASSERT_ARE_NOT_EQUAL)
        {
            return source.assert_Is_Not_Equal_To(target, message);
        }
        public static T     assert_Is_Equal_To<T>     (this T source, T target, string message = NUnit_Messages.ASSERT_ARE_EQUAL)    
        {
            return nUnitTests.assert_Are_Equal(source,target, message);
        }
        public static T     assert_Are_Not_Equal<T,T1>(this T source, Func<T,T1> callback, T1 target)
        {
            nUnitTests.assert_Are_Not_Equal(callback(source), target);
            return source;
        }        
        public static T     assert_Not_Equal<T>       (this T target, T value, string message = NUnit_Messages.ASSERT_ARE_NOT_EQUAL)
        {
            return target.assert_Not_Equals(value);            
        }
        public static T     assert_Not_Equal_To<T>    (this T target, T value, string message = NUnit_Messages.ASSERT_ARE_NOT_EQUAL)
        {
            return target.assert_Not_Equals(value,message);
        }        
        public static T     assert_Not_Equals<T>      (this T source, T target, string message = NUnit_Messages.ASSERT_ARE_NOT_EQUAL)
        {
            return source.assert_Is_Not_Equal_To(target, message);
        }
        public static T     assert_Is_Not_Equal_To<T> (this T source, T target, string message = NUnit_Messages.ASSERT_ARE_NOT_EQUAL)       
        {
            return nUnitTests.assert_Are_Not_Equal(source,target, message);
        }
        public static T     assert_Null<T>            (this T target, string message = NUnit_Messages.ASSERT_NULL) where T : class
        {
            Assert.True(target.isNull(), message.format(target));
            return target;
        }        
        public static T     assert_Is_Null<T>         (this T target, string message = NUnit_Messages.ASSERT_NULL) where T : class     
        {
            return target.assert_Null(message);            
        }        
        public static T     assert_Not_Null<T>        (this T target, string message = NUnit_Messages.ASSERT_NOT_NULL) where T : class
        {
            Assert.True(target.notNull(), message);
            return target;
        }                     
        public static T     assert_Not_Null<T>        (this T target, Func<T> action) where T : class
        {
            action().assert_Not_Null();
            return target;
        }
        public static T     assert_Not_Null<T,T1>     (this T target, Func<T,T1> action) where T : class where T1 : class
        {
            action(target).assert_Not_Null();
            return target;
        }        
        public static T     assert_Is_Not_Null<T>     (this T target) where T : class 
        {
            return nUnitTests.assert_Is_Not_Null(target);            
        }        
        public static T     assert_Instance_Of<T>     (this object target)
        {
            return target.assert_Is_Instance_Of<T>();
        }
        public static T     assert_Is_Instance_Of<T>  (this object target)
        {
            Assert.IsInstanceOf<T>(target);
            return (T)target;
        }
        public static T     assert_Default<T>         (this T target)
        {
            return target.assert_Is_Default();
        }
        public static T     assert_Is_Default<T>      (this T target)
        {
            return nUnitTests.assert_Is_Default(target);            
        }
        public static T     assert_Not_Default<T>     (this T target)
        {
            return target.assert_Is_Not_Default();
        }
        public static T     assert_Is_Not_Default<T>  (this T target)
        {
            return nUnitTests.assert_Is_Not_Default(target);            
        }        
    }
}