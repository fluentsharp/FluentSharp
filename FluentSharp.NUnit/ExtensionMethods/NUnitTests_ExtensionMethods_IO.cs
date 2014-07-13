using System.Collections.Generic;
using FluentSharp.CoreLib;

namespace FluentSharp.NUnit
{
    public static class NUnitTests_ExtensionMethods_IO
    {
        public static NUnitTests nUnitTests = new NUnitTests();

        //Files
        public static string         assert_File_Contains(this string file, params string[] values)
        {
            var fileContents = file.assert_File_Exists().fileContents()
                                                        .assert_Not_Empty();
            fileContents.assert_Contains(values);
            return file;
        }
        public static string         assert_File_Not_Contains(this string file, params string[] values)
        {
            var fileContents = file.assert_File_Exists().fileContents()
                                                        .assert_Not_Empty();
            fileContents.assert_Not_Contains(values);
            return file;
        }
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
        public static string  assert_Folder_Exists(this string folderPath) 
        {
            return nUnitTests.assert_Folder_Exists(folderPath);            
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
        public static string  assert_Folder_Not_Exists(this string folderPath) 
        {
            return nUnitTests.assert_Folder_Not_Exists(folderPath);            
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

    }
}