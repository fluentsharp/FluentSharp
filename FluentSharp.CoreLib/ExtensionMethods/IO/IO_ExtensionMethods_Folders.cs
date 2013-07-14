using System;
using System.Collections.Generic;
using System.IO;
using FluentSharp.CoreLib.API;

namespace FluentSharp.CoreLib
{
    public static class IO_ExtensionMethods_Folders
    {
        public static string        folderName(this string folder)
        {
            if (folder.isFolder())
                return folder.fileName();
            return null;
        }		
        public static string        parentFolder(this string path)
        {
            if (path.valid())
                return path.directoryName();
            return null;
        }
        public static List<string>  folders(this string path)
        {
            return path.folders(false);
        }
        public static List<string>  folders(this string path, bool recursive)
        {
            return (path.isFolder())
                       ? Files.getListOfAllDirectoriesFromDirectory(path, recursive)
                       : new List<string>();
        }
        public static string        createDir(this string directory)
        {            
            return Files.checkIfDirectoryExistsAndCreateIfNot(directory);            
        }
        public static string        createFolder(this string folder)
        {            
            return folder.createDir();
        }
        public static string        directoryName(this string file)
        {
            if (file.valid())
            {
                try
                {
                    return Path.GetDirectoryName(file);
                }
                catch(Exception ex)
                {
                    ex.log("in directoryName for: {0}".info(file));                    
                }
            }
            return ""; 
        }
        public static bool          isFolder(this string path)
        {
            return path.dirExists();
        }
        public static bool          folderExists(this string path)
        {
            return path.dirExists();
        }
        public static bool          dirExists(this string path)
        {
            if (path.valid())
                return Directory.Exists(path);
            return false;
        }        
        public static List<string>  dirs(this string path)
        {
            return path.folders(false);
        }

        public static DirectoryInfo directoryInfo(this string directoryPath)
        {
            return new DirectoryInfo(directoryPath);
        }
    }
}