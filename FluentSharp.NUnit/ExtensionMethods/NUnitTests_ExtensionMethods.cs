using System;
using NUnit.Framework;

namespace FluentSharp.NUnit
{
    public static class NUnitTests_ExtensionMethods
    {
        public static NUnitTests nUnitTests = new NUnitTests();

        public static string and_Contains(this string source, string target)
        {
            return source.assert_Contains(target);
        }
        public static T      and_Is_Equal_To<T>(this T source, T target)
        {
            return source.assert_Is_Equal_To(target);
        }
        public static T      and_Is_Not_Equal_To<T>(this T source, T target)
        {
            return source.assert_Is_Not_Equal_To(target);
        }
        public static T      and_Is_Instance_Of<T>(this object target)
        {
            return target.assert_Is_Instance_Of<T>();
        }

        //string
        public static string     assert_Contains(this string source,string target)       
        {
            return nUnitTests.assert_Contains(source,target);
        }
        //T
        public static T     assert_Is_Equal_To<T>(this T source, T target)       
        {
            return nUnitTests.assert_Are_Equal(source,target);
        }
        public static T     assert_Is_Not_Equal_To<T>(this T source, T target)       
        {
            return nUnitTests.assert_Are_Not_Equal(source,target);
        }
        public static T     assert_Is_Null<T>(this T target) where T : class     
        {
            return nUnitTests.assert_Is_Null(target);            
        }
        public static T     assert_Is_Not_Null<T>(this T target) where T : class 
        {
            return nUnitTests.assert_Is_Not_Null(target);            
        }        
        public static T     assert_Is_Instance_Of<T>(this object target)
        {
            Assert.IsInstanceOf<T>(target);
            return (T)target;
        }
        //bool
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
        public static bool  assert_Is_True(this bool target) 
        {
            return nUnitTests.assert_Is_True(target);            
        }
        public static T     assert_Is_True<T>(this T target, Func<T,bool> callback) 
        {
            nUnitTests.assert_Is_True(callback(target));            
            return target;
        }

        //IO
        public static string  assert_File_Exists(this string filePath) 
        {
            return nUnitTests.assert_File_Exists(filePath);            
        }
        public static string  assert_Folder_Exists(this string folderPath) 
        {
            return nUnitTests.assert_Folder_Exists(folderPath);            
        }
    }
}
