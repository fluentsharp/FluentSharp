using System.Collections.Generic;
using FluentSharp.CoreLib;
using NUnit.Framework;

namespace FluentSharp.NUnit
{
    public static class NUnitTests_ExtensionMethods_IO
    {
        public static NUnitTests nUnitTests = new NUnitTests();
        
        //Files
        public static string        assert_File_Contains(this string file, params string[] values)
        {
            var fileContents = file.assert_File_Exists().fileContents()
                                                        .assert_Not_Empty();
            fileContents.assert_Contains(values);
            return file;
        }
        public static string        assert_File_Not_Contains(this string file, params string[] values)
        {
            var fileContents = file.assert_File_Exists().fileContents()
                                                        .assert_Not_Empty();
            fileContents.assert_Not_Contains(values);
            return file;
        }
        /// <summary>
        /// Asserts that an File exists
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string        assert_File_Exists(this string filePath,string message = NUnit_Messages.ASSERT_FILE_EXISTS) 
        {            
            Assert.IsTrue(filePath.file_Exists(),message.format(filePath));
            return filePath;            
        }
        /// <summary>
        /// Asserts that an File doesn't exists
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string        assert_File_Not_Exists(this string filePath, string message = NUnit_Messages.ASSERT_FILE_NOT_EXISTS)
        {            
            Assert.IsTrue(filePath.file_Not_Exists(), message.format(filePath));
            return filePath;            
        }
        public static List<string>  assert_Files_Not_Exist(this List<string> filesPath) 
        {
            foreach(var filePath in filesPath)
                filePath.assert_File_Not_Exists();
            return filesPath;            
        }
        public static List<string>  assert_Files_Exist(this List<string> filesPath) 
        {
            foreach(var filePath in filesPath)
                filePath.assert_File_Exists();
            return filesPath;          
        }        
        public static string        assert_File_Deleted(this string filePath)
        {
            filePath.assert_File_Exists();
            filePath.file_Delete().assert_Is_True("Failed to deleted file: {0}".format(filePath));
            filePath.assert_File_Not_Exists();
            return filePath;
        }
        public static List<string>  assert_Files_Deleted(this List<string> filesPath)
        {
                //filesPath.files_Exist().assert_True();
            filesPath.assert_Files_Exist();
            filesPath.files_Delete().str().fileExists();
                filesPath.files_Not_Exist().assert_True();
            filesPath.assert_Files_Not_Exist();
            return filesPath;
        }
        public static string        assert_File_Extension_Is(this string filePath, string expectedValue)
        {
            filePath.extension(expectedValue).assert_True(NUnit_Messages.ASSERT_FILE_EXTENSION_IS.format(filePath, expectedValue));
            return filePath;
        }
        public static string        assert_File_Extension_Is_Not(this string filePath, string expectedValue)
        {
            filePath.extension(expectedValue).assert_False(NUnit_Messages.ASSERT_FILE_EXTENSION_IS_NOT.format(filePath, expectedValue));
            return filePath;
        }
        public static string        assert_File_Parent_Folder_Is(this string filePath, string folder, string message = NUnit_Messages.ASSERT_FILE_PARENT_FOLDER_IS)
        {
            filePath.parentFolder().assert_Equal(folder, message);
            return filePath;
        }
        
        
        //Folders
        /// <summary>
        /// Checks that a folder exists and has no files
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static string assert_Folder_Empty(this string folder)
        {
            folder.assert_Folder_Exists();
            var files = folder.files();
            files.assert_Empty("on Folder {0} there where {1} files".format(folder, files.size()));
            return folder;
        }
        /// <summary>
        /// Asserts that an Directory exists (same as assert_Folder_Exists)
        /// </summary>
        /// <param name="dirPath"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string assert_Dir_Exists(this string dirPath, string message = NUnit_Messages.ASSERT_FOLDER_EXISTS)
        {
            return dirPath.assert_Folder_Exists(message);
        }        
        /// <summary>
        /// Asserts that an Folder exists (same as assert_Dir_Exists)
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string assert_Folder_Exists(this string folderPath, string message = NUnit_Messages.ASSERT_FOLDER_EXISTS)
        {
            Assert.IsTrue(folderPath.dirExists(), message.format(folderPath));
            return folderPath;
        }        
        /// <summary>
        /// Asserts that an Directory doesn't exists (same as assert_Folder_Not_Exists)
        /// </summary>
        /// <param name="dirPath"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string assert_Dir_Not_Exists(this string dirPath, string message = NUnit_Messages.ASSERT_FOLDER_EXISTS)
        {
            return dirPath.assert_Folder_Doesnt_Exist(message);
        }
        public static string assert_Dir_Doesnt_Exist(this string dirPath, string message = NUnit_Messages.ASSERT_FOLDER_EXISTS)
        {
            return dirPath.assert_Folder_Doesnt_Exist(message);
        }
        public static string assert_Folder_Doesnt_Exist(this string folder, string message = NUnit_Messages.ASSERT_FOLDER_NOT_EXISTS)
        {
            return folder.assert_Folder_Not_Exists(message.format(folder));
        }                        
        public static string assert_Folder_Has_Files(this string folder, params string[] files)
        {
            foreach(var file in files)
                folder.assert_Folder_Has_File(file);
            return folder;
        }
        public static string assert_Folder_Has_File(this string folder, string file)
        {
            folder.assert_Folder_Exists();
            folder.mapPath(file).assert_File_Exists();
            return folder;
        }
        /// <summary>
        /// Asserts that an Folder doesn't exists (same as assert_Dir_Not_Exists)
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string  assert_Folder_Not_Exists(this string folderPath, string message = NUnit_Messages.ASSERT_FOLDER_NOT_EXISTS) 
        {
            Assert.IsTrue(folderPath.folder_Not_Exists(), message);            
            return folderPath;
        }
        /// <summary>
        /// Checks that a folder exists and has at least 1 files
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static string assert_Folder_Not_Empty(this string folder)
        {
            folder.assert_Folder_Exists();
            var files = folder.files();
            files.assert_Not_Empty("on Folder {0} there where no files".format(folder));
            return folder;
        }
        public static string assert_Folder_Deleted(this string folderPath)
        {
            folderPath.assert_Folder_Exists();
            folderPath.folder_Delete().assert_Is_True("Failed to deleted Folder: {0}".format(folderPath));
            folderPath.assert_Folder_Not_Exists();
            return folderPath;
        }
        public static string assert_Parent_Folder_Is(this string folderPath, string folder, string message = NUnit_Messages.ASSERT_PARENT_FOLDER_IS)
        {
            folderPath.parentFolder().assert_Equal(folder, message);
            return folderPath;
        }        
        public static string assert_Is_Folder(this string folderPath)
        {
            return folderPath.assert_Folder_Exists();
        }
        public static string assert_Folder_File_Count_Is(this string folderPath, int value, string message = NUnit_Messages.ASSERT_FOLDER_FILE_COUNT_IS)
        {
            var fileCount = folderPath.assert_Is_Folder().files().size();
            fileCount.assert_Is(value, message.format(folderPath , value, fileCount));
            return folderPath;
        }

    }
}