using System;
using System.Collections;
using System.Collections.Generic;
using FluentSharp.CoreLib;
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
        public static string     assert_Contains(this string source,params string[] targets)       
        {
            foreach(var target in targets)
                nUnitTests.assert_Contains(source,target);
            return source;
        }
        //T
        public static T     assert_Contains<T>(this T source, Func<string> callback, params string[] targets)
        {
            foreach(var target in targets)
                nUnitTests.assert_Contains(callback(), target);
            return source;
        }
        public static T     assert_Are_Equal<T,T1>(this T source, Func<T1> callback, T1 target)
        {
            nUnitTests.assert_Are_Equal(callback(), target);
            return source;
        }
        public static T     assert_Are_Equal<T,T1>(this T source, Func<T,T1> callback, T1 target)
        {
            nUnitTests.assert_Are_Equal(callback(source), target);
            return source;
        }
        public static T     assert_Is_Equal_To<T>(this T source, T target)       
        {
            return nUnitTests.assert_Are_Equal(source,target);
        }
        public static T     assert_Are_Not_Equal<T,T1>(this T source, Func<T,T1> callback, T1 target)
        {
            nUnitTests.assert_Are_Not_Equal(callback(source), target);
            return source;
        }
        public static T     assert_Is_Not_Equal_To<T>(this T source, T target)       
        {
            return nUnitTests.assert_Are_Not_Equal(source,target);
        }
        public static T     assert_Is_Null<T>(this T target) where T : class     
        {
            return nUnitTests.assert_Is_Null(target);            
        }
        public static T     assert_Not_Null<T>(this T target) where T : class 
        {
            return target.assert_Is_Not_Null();            
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
        public static T    assert_Empty<T>(this T target) where  T : IEnumerable
        {
            return target.assert_Is_Empty();
        }
        public static T    assert_Is_Empty<T>(this T target) where  T : IEnumerable
        {
            nUnitTests.assert_Is_Empty(target);
            return target;
        }
        public static T    assert_Not_Empty<T>(this T target) where  T : IEnumerable
        {
            nUnitTests.assert_Is_Not_Empty(target);
            return target;
        }
     
        //Lists and IEnumerable
        public static T  assert_Size_Is<T>(this T target, int size) where  T : IEnumerable
        {
            return nUnitTests.assert_Size_Is(target,size);
        }
        public static List<T>  assert_Not_Contains<T>(this List<T> target , T item)
        {
            if(target.contains(item))
            {                 
                throw new AssertionException("target list ( {0} )should not contain item: {1}".format(target, item));
            }
            return target;
        }
        public static List<T>  assert_Contains<T>(this List<T> target , params T[] items)
        {
            foreach(var item in items)
                Assert.Contains(item, target);
            return target;
        }
        public static List<T>  assert_Item_Is_Equal<T>(this List<T> target, int index, T expectedItem)
        {
            if (target.size() < index)
                nUnitTests.assert_Fail("in assert_Item_Is_Equal, the provided index value ({0}) is smaller than the size of target list ({1})".format(index, target.size()));

            var itemAtIndex = target.value(index);
            if (itemAtIndex.notNull())
                nUnitTests.assert_Are_Equal(itemAtIndex, expectedItem);
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
