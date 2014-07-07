using System;
using System.Collections;
using System.Collections.Generic;
using FluentSharp.CoreLib;
using NUnit.Framework;

namespace FluentSharp.NUnit
{    
    public class Tests : NUnitTests
    {
        
    }
    [TestFixture]
    public class NUnitTests
    {   

        //string
        public string      assert_Contains(string source, string target)
        {            
            Assert.IsTrue(source.contains(target));
            return source;
        }                     
        //Equality
        public T      assert_Are_Equal<T>(T source, T target )
        {            
            Assert.AreEqual(source,target);
            return source;
        }             
        public T      assert_Not_Equal<T>(T source, T target )
        {
            return assert_Are_Not_Equal(source,target);     
        }
        public T      assert_Are_Not_Equal<T>(T source, T target )
        {            
            Assert.AreNotEqual(source,target);
            return source;
        }     
        
        //Bool
        public bool   assert_False     (bool target)
        {
            return assert_Is_False(target);
        }
        public bool   assert_Is_False    (bool target)
        {            
            Assert.IsFalse(target);
            return true;
        }             
        public bool   assert_Is_Not_True (bool target)
        {            
            Assert.IsFalse(target);
            return true;
        }    
        public bool   assert_Is_Not_False(bool target)
        {            
            Assert.IsTrue(target);
            return true;
        }
        public bool   assert_True     (bool target)
        {
            return assert_Is_True(target);
        }
        public bool   assert_Is_True     (bool target, string message = "Assert.IsTrue")
        {
            Assert.IsTrue(target, message);
            return true;
        }            

        //ints
        public bool   assert_Is_Bigger   (int source, int target)
        {
            return assert_Is_Greater(source,target);
        }
        public bool   assert_Is_Greater  (int source, int target)
        {            
            Assert.Greater(source,target);
            return true;
        } 
        public bool   assert_Is_Smaller  (int source, int target)
        {
            return assert_Is_Less(source,target);
        }    
        public bool   assert_Is_Less     (int source, int target)
        {            
            Assert.Less(source,target);
            return true;
        }             
        
        //Object
        public T      assert_Null<T>    (T target) where  T : class
        {
            return assert_Is_Null(target);
        }
        public T      assert_Is_Null<T>    (T target) where  T : class
        {                        
            Assert.IsNull(target, "Target was null");
            return null;
        }        
        public T      assert_Not_Null<T>(T target) where  T : class
        {
            return assert_Is_Not_Null(target);
        }
        public T      assert_Is_Not_Null<T>(T target) where  T : class
        {                        
            Assert.IsNotNull(target, "Target was not null {0}".format(target));
            return target;
        }                
        public T      assert_Default<T> (T target)
        {
            return assert_Is_Default(target);
        }
        public T      assert_Is_Default<T> (T target)
        {                        
            Assert.AreEqual(target, default(T), "provided target was not default(T). Target={0}   T={1}".format(target, typeof(T)));
            return target;
        }        
        public T      assert_Not_Default<T> (T target)
        {
            return assert_Is_Not_Default(target);
        }
        public T      assert_Is_Not_Default<T> (T target)
        {                        
            Assert.AreNotEqual(target, default(T), "provided target was default(T). Target={0}   T={1}".format(target, typeof(T)));
            return target;
        }
        
        //Lists
        public T  assert_Is_Empty<T>(T target) where  T : IEnumerable
        {
            Assert.IsEmpty(target, "Target was Not Empty");            
            return target;
        }
        public T  assert_Not_Empty<T>(T target) where  T : IEnumerable
        {
            return assert_Is_Not_Empty(target);
        }
        public T  assert_Is_Not_Empty<T>(T target) where  T : IEnumerable
        {
            Assert.IsNotEmpty(target, "Target was Empty");            
            return target;
        }
        public T  assert_Size_Is<T>(T target, int size) where  T : IEnumerable
        {
            Assert.AreEqual(target.size(), size);
            return target;
        }

        //IO

        public string assert_Dir_Exists(string folderPath)
        {                        
            Assert.IsTrue(folderPath.dirExists());
            return folderPath;
        }
        public string assert_Dir_Not_Exists(string folderPath)
        {                        
            Assert.IsFalse(folderPath.dirExists());
            return folderPath;
        }
        public List<string> assert_Files_Not_Exists(List<string> filesPath)
        {
            foreach(var filePath in filesPath)
                assert_File_Not_Exists(filePath);
            return filesPath;
        }
        public string assert_File_Not_Exists(string filePath)
        {                        
            Assert.IsFalse(filePath.fileExists(), "Not Expected file was found: {0}".format(filePath));
            return filePath;
        }
        public List<string> assert_Files_Exists(List<string> filesPath)
        {
            foreach(var filePath in filesPath)
                assert_File_Exists(filePath);
            return filesPath;
        }
        public string assert_File_Exists(string filePath)
        {                        
            if(filePath.notValid())
                assert_Fail("[assert_File_Exists] provided filePath was null or empty");
            Assert.IsTrue(filePath.fileExists(),"Expected File was not found: {0}".format(filePath));
            return filePath;
        }
        public string assert_Folder_Exists(string folderPath) 
        {
            return assert_Dir_Exists(folderPath);
        }
        public string assert_Folder_Not_Exists(string folderPath) 
        {
            return assert_Dir_Not_Exists(folderPath);
        }        
    
        //Exceptions
        public Action assert_Does_Throw<T>(Action action) where  T : Exception
        {
            return assert_Throws<T>(action);
        }
        /// <summary>
        /// Checks that the provided <paramref name="action"/>action throws an exception (no mater what type)
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public Action assert_Throws(Action action)
        {
            try
            {
                action();                
            }
            catch
            {
                return action;
            }
            throw new AssertionException("action didn't throw an exception");
        }
        /// <summary>
        /// Checks that the provided <paramref name="action"/> throws an exception of the provided type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        public Action assert_Throws<T>(Action action) where  T : Exception
        {                        
            Assert.Throws<T>(()=>action(), "action didn't throw expected execption {0}".format(typeof(T)));
            return action;
        }
        public Action assert_Not_Throws(Action action)
        {
            return assert_Does_Not_Throw(action);
        }
        public Action assert_Does_Not_Throw(Action action)
        {                        
            Assert.DoesNotThrow(()=>action(), "action throw an unexpected exception");
            return action;
        }

        //Fail
        public void assert_Fail(string failMessage)
        {                        
            Assert.Fail(failMessage);            
        }
        public void assert_Ignore(string ignoreMessage)
        {                        
            Assert.Ignore(ignoreMessage);            
        }

        //Type
        public bool   assert_Is_Not_Type     <T>(object target)
        {
            if (target.isNull())
                return false;
            Assert.AreNotEqual(typeof(T),target.type());
            return true;
        }
        public bool   assert_Is_Type     <T>(object target)
        {            
            if (target.isNull())
                return false;            
            Assert.AreEqual(typeof(T),target.type());            
            return true;
        }
    }
}