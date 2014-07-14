using System.Collections;
using System.Collections.Generic;
using FluentSharp.CoreLib;
using NUnit.Framework;

namespace FluentSharp.NUnit
{
    public static class NUnitTests_ExtensionMethods_Lists
    {
        public static NUnitTests nUnitTests = new NUnitTests();

        public static T    assert_Empty<T>(this T target, string message = "Target was Not Empty") where  T : IEnumerable
        {
            return nUnitTests.assert_Is_Empty(target, message);
        }
        
        public static T    assert_Is_Empty<T>(this T target, string message = "Target was Not Empty") where  T : IEnumerable
        {
            nUnitTests.assert_Is_Empty(target);
            return target;
        }
        
        public static T    assert_Not_Empty<T>(this T target, string message = "Target was empty") where  T : IEnumerable
        {
            nUnitTests.assert_Is_Not_Empty(target);
            return target;
        }
     
        //Lists and IEnumerable
        public static T  assert_Size_Is<T>(this T target, int size, string message = "target size didn't match expected value") where  T : IEnumerable
        {
            return nUnitTests.assert_Size_Is(target,size, message);
        }

        public static T assert_Size_Is_Bigger_Than<T>(this T target, int value) where  T : IEnumerable
        {
            return nUnitTests.assert_Size_Is_Bigger_Than(target,value);
        }
        public static T assert_Bigger_Than<T>(this T target, int value)  where  T : IEnumerable
        {
            return target.assert_Size_Is_Bigger_Than(value);
        }
        public static T assert_Size_Is_Smaller_Than<T>(this T target, int value)  where  T : IEnumerable
        {
            return nUnitTests.assert_Size_Is_Smaller_Than(target,value);
        }
        public static T assert_Smaller_Than<T>(this T target, int value)  where  T : IEnumerable
        {
            return target.assert_Size_Is_Smaller_Than(value);
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
        /// <summary>
        /// Checks if the List Item in the index provided matches the provided expectedItem
        /// 
        /// Assert Fail is thrown if index is bigger than the size of the list provided
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="index"></param>
        /// <param name="expectedItem"></param>
        /// <returns></returns>
        public static List<T>  assert_Item_Is_Equal<T>(this List<T> target, int index, T expectedItem)
        {
            if (target.size() <= index)
                nUnitTests.assert_Fail("in assert_Item_Is_Equal, the provided index value ({0}) is smaller than the size of target list ({1})".format(index, target.size()));

            var itemAtIndex = target.value(index);
            if (itemAtIndex.notNull())
                nUnitTests.assert_Are_Equal(itemAtIndex, expectedItem);
            return target;
        }
        /// <summary>
        /// Checks if the List Item in the index provided does NOT matches the provided expectedItem        
        /// 
        /// If index is bigger than the size of the list provided it is assumed that they are not equal
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="index"></param>
        /// <param name="expectedItem"></param>
        /// <returns></returns>
        public static List<T>  assert_Item_Not_Equal<T>(this List<T> target, int index, T expectedItem)
        {
            if (target.size() > index)
            { 
                var itemAtIndex = target.value(index);
                if (itemAtIndex.notNull())
                    nUnitTests.assert_Not_Equal(itemAtIndex, expectedItem);
            }
            return target;
        }
    }
}