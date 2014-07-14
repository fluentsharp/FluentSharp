using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public static Process       parentFolder_Open_in_Explorer(this string path)
        {
            var parentFolder = path.parentFolder();
            if(parentFolder.folder_Exists())
                return parentFolder.startProcess();
            return null;
        }
        public static string        folder(this string basePath, string folderName)
		{
			var targetFolder = basePath.pathCombine(folderName);
			if(targetFolder.dirExists())
				return targetFolder;
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
        
        /// <summary>
        /// checks if the provided directory exists and if not, calls <code>Files.checkIfDirectoryExistsAndCreateIfNot(directory);</code>
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Creates a file in the provided folder
        /// 
        /// Returns path to created file.
        /// 
        /// retuns null if: 
        ///   - folder doesn't exist
        ///   - file already exists
        ///   - resolved file path is located outside the provided folder
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="fileName"></param>
        /// <param name="fileContents"></param>
        /// <returns></returns>
        public static string folder_Create_File(this string folder, string fileName, string fileContents)
        {   
            if(folder.folder_Exists())
            {
                var path = folder.mapPath(fileName);
                if (path.valid() && path.file_Doesnt_Exist())
                { 
                    fileContents.saveAs(path);
                    if (path.file_Exists())
                        return path;
                }
            }
            return null;
        }
        public static bool          folder_Delete_Files(this string folder)
        {
            if (folder.isFolder() && folder.files().notEmpty())
            {                
                Files.deleteAllFilesFromDir(folder);
                return folder.files().empty();
            }
            return false;
        }
        public static bool          folder_Exists(this string path)
        {
            return path.folderExists();
        }
        public static bool          folder_Not_Exists(this string path)
        {
            return path.folderExists().isFalse();
        }
         /// <summary>
        /// Waits for a folder to be deleted from disk. 
        /// 
        /// The maxWait defaults to 2000 and there will be 10 checks (at maxWait / 10 intervals).
        /// 
        /// If the folder was still there after maxWait an error message will be logged 
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="maxWait"></param>
        /// <returns></returns>
        public static string folder_Wait_For_Deleted(this string folder, int maxWait = 2000)
        {
            if(folder.isFolder() && folder.folder_Exists())
            {
                var loopCount = 10;
                var sleepValue = maxWait/loopCount;
                for(int i=0;i< loopCount;i++)
                {
                    if (folder.folder_Not_Exists())
                        break;
                    sleepValue.sleep();
                }
                if (folder.folder_Exists())
                    "[string][folder_Wait_For_Deleted] after {0}ms the target folder still existed: {1}".error(maxWait, folder);
            }
            return folder;
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

        /// <summary>
        /// Combines both paths and normalizes the file path.
        /// 
        /// There is a check that the rootPath is still in the final (it is not, an exception is logged and null is returned)
        /// 
        /// This simulates the System.Web MapPath functionality
        /// </summary>
        /// <param name="rootPath"></param>
        /// <param name="virtualPath"></param>
        /// <returns></returns>
        public static string mapPath(this string rootPath, string virtualPath)
        {
            if (rootPath.empty() || virtualPath.empty())
                return null;
            var mappedPath = rootPath.pathCombine(virtualPath).fullPath();
            if (mappedPath.starts(rootPath).isFalse())
            {
                @"[string][mapPath] the mappedPath did not contains the root path. 

                    mappedPath : {0}
                    rootPath   : {1}
                    virtualPath: {2}".error(mappedPath, rootPath, virtualPath);
                return null;
            }
            return mappedPath;
        }
        public static string temp_Folder(this string name, bool appendRandomStringToFolderName = true)
        {
            return name.temp_Dir(appendRandomStringToFolderName);
        }
        public static string temp_Dir(this string name, bool appendRandomStringToFolderName = true)
        {
            return name.tempDir(appendRandomStringToFolderName);
        }
    }
}