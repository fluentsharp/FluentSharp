using System;
using System.Collections.Generic;
using System.IO;
using FluentSharp.CoreLib.API;

namespace FluentSharp.CoreLib
{
    public static class IO_ExtensionMethods_Delete_Or_Copy
    { 
        public static bool          deleteFile(this string file)
        {
            return Files.deleteFile(file);
        }		
        public static List<string>  deleteFiles(this List<string> files)
        {
            foreach(var file in files)
                Files.deleteFile(file);
            return files;
        }
        public static bool          deleteIfExists(this string file)
        {
            try
            {
                if (file.fileExists())
                    Files.deleteFile(file);
                return true;
            }
            catch(Exception ex)
            {
                "[deleteIfExists] : {0}".error(ex.Message);
                return false;
            }
            
        }
        public static string        file_CopyToFolder(this string fileToCopy, string targetFolderOrFile)
        {
            if (fileToCopy.fileExists().isFalse())
                "[file_CopyFileToFolder] fileToCopy doesn't exist: {0}".error(fileToCopy);
            else
                if (targetFolderOrFile.dirExists() ||  targetFolderOrFile.parentFolder().dirExists())
                    return Files.copy(fileToCopy,targetFolderOrFile);
                else
                    "[file_CopyFileToFolder]..targetFolder or its parent doesn't exist: {0}".error(targetFolderOrFile);					
            return null;			
        }

        public static bool file_Delete(this string path)
        {
            return path.delete_File();
        }
        public static bool delete_File(this string path)
        {
            return path.delete_File(true);
        }
        public static bool delete_File(this string path, bool waitForCanOpen)
        {
            try
            {
                var fileInfo = path.fileInfo();
                if (fileInfo.valid())
                {
                    if (fileInfo.IsReadOnly)
                        return false;                    
                    if (waitForCanOpen)
                        path.file_WaitFor_CanOpen();
                    File.Delete(path);
                    "Deleted file: {0}".info(path);
                }
            }
            catch (Exception ex)
            {
                ex.log();
            }
            return path.fileExists().isFalse();
        }
        public static bool folder_Delete(this string path)
        {
            return path.delete_Folder();
        }
        public static bool delete_Folder(this string path)
        {
            Files.deleteAllFilesFromDir(path);
            Files.deleteFolder(path);
            return path.dirExists();
        }
    }
}