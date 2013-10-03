using System.Collections;
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
        public T      assert_Are_Not_Equal<T>(T source, T target )
        {            
            Assert.AreNotEqual(source,target);
            return source;
        }     
        //Bool

        public bool   assert_Is_False(bool target)
        {            
            Assert.IsFalse(target);
            return true;
        }             
        public bool   assert_Is_Not_True(bool target)
        {            
            Assert.IsFalse(target);
            return true;
        }    
        public bool   assert_Is_Not_False(bool target)
        {            
            Assert.IsTrue(target);
            return true;
        }    
        public bool   assert_Is_True(bool target)
        {            
            Assert.IsTrue(target);
            return true;
        }    

        //Object
        public T      assert_Is_Null<T>(T target) where  T : class
        {                        
            Assert.IsNull(target, "Target was null");
            return null;
        }
        public T      assert_Is_Not_Null<T>(T target) where  T : class
        {                        
            Assert.IsNotNull(target, "Target was not null {0}".format(target));
            return target;
        }                

        //Lists
        public T  assert_Is_Empty<T>(T target) where  T : IEnumerable
        {
            Assert.IsEmpty(target, "Target was Not Empty");            
            return target;
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
        public string assert_File_Not_Exists(string filePath)
        {                        
            Assert.IsFalse(filePath.fileExists(), "Not Expected file was found: {0}".format(filePath));
            return filePath;
        }
        public string assert_File_Exists(string filePath)
        {                        
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
    }
}