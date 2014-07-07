using System.Collections.Generic;
using FluentSharp.CoreLib;

namespace FluentSharp.NUnit
{
    public static class NUnitTests_ExtensionMethods_IO
    {
        public static NUnitTests nUnitTests = new NUnitTests();

        //Files
        public static string  assert_File_Exists(this string filePath) 
        {
            return nUnitTests.assert_File_Exists(filePath);            
        }
        public static string  assert_File_Not_Exists(this string filePath) 
        {
            return nUnitTests.assert_File_Not_Exists(filePath);            
        }
        public static List<string>  assert_Files_Not_Exist(this List<string> filePath) 
        {
            return nUnitTests.assert_Files_Not_Exists(filePath);            
        }
        public static List<string>  assert_Files_Exist(this List<string> filesPath) 
        {
            return nUnitTests.assert_Files_Exists(filesPath);            
        }        
        public static string assert_File_Deleted(this string filePath)
        {
            filePath.assert_File_Exists();
            filePath.file_Delete().assert_Is_True("Failed to deleted file: {0}".format(filePath));
            filePath.assert_File_Not_Exists();
            return filePath;
        }
        public static List<string> assert_Files_Deleted(this List<string> filesPath)
        {
                //filesPath.files_Exist().assert_True();
            filesPath.assert_Files_Exist();
            filesPath.files_Delete().str().fileExists();
                filesPath.files_Not_Exist().assert_True();
            filesPath.assert_Files_Not_Exist();
            return filesPath;
        }

        //Folders
        public static string  assert_Folder_Exists(this string folderPath) 
        {
            return nUnitTests.assert_Folder_Exists(folderPath);            
        }
        public static string  assert_Folder_Not_Exists(this string folderPath) 
        {
            return nUnitTests.assert_Folder_Not_Exists(folderPath);            
        }
        public static string assert_Folder_Deleted(this string folderPath)
        {
            folderPath.assert_Folder_Exists();
            folderPath.folder_Delete().assert_Is_True("Failed to deleted Folder: {0}".format(folderPath));
            folderPath.assert_Folder_Not_Exists();
            return folderPath;
        }

    }
}