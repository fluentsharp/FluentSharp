using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using FluentSharp.CoreLib.API;

namespace FluentSharp.CoreLib
{
    public static class IO_ExtensionMethods_Folders
    {
        /// <summary>
        /// Given a valid folder path returns top directory name by calling <code>folder.filename()</code>
        /// </summary>
        /// <param name="folder">
        /// Input is checked that it is not null or empty <code>string.IsNullOrEmpty(folder)</code> and <code>folder.isFolder()</code>
        /// </param>
        /// <returns></returns>
        public static string        folder_Name(this string folder)
        {
            return folder.isFolder() ? folder.fileName() : null;
           
        }		
        public static string        folderName(this string folder)
        {
            return folder.folder_Name();
        }
        /// <summary>
        /// Given a valid path returns the parent folder
        /// </summary>
        /// <param name="path"></param>
        /// Input is validated using <code>path.valid()</code> function
        /// <returns></returns>
        public static string        parent_Folder(this string path)
        {
            if (path.valid())
                return path.directoryName();
            return null;
        }
        /// <summary>
        /// Given a valid path returns the parent folder
        /// </summary>
        /// <param name="path"></param>
        /// Input is validated using <code>path.valid()</code> function
        /// <returns></returns>
        [Obsolete("parentFolder is deprecated.Please use parent_Folder instead")]
        public static string parentFolder(this string path)
        {
            return path.parent_Folder();
        }
        /// <summary>
        /// Given a file path or directory path opens the parent folder in windows explorer
        /// </summary>
        /// <param name="path"></param>
        /// Input parameters is validated using <code>path.parentFolder().folder_Exists()</code>
        /// <returns></returns>
        public static Process       parentFolder_Open_in_Explorer(this string path)
        {
            var parentFolder = path.parent_Folder();
            if(parentFolder.folder_Exists())
                return parentFolder.startProcess();
            return null;
        }
        /// <summary>
        /// Combine two paths and checks if a folder exists in the merged directory path.
        /// </summary>
        /// <param name="basePath"></param>
        /// <param name="folderName"></param>
        /// <returns>If <code>basePath.pathCombine(folderName)</code> is a valid path then return it otherwise returns null</returns>
        public static string        folder(this string basePath, string folderName)
		{
			var targetFolder = basePath.pathCombine(folderName);
			if(targetFolder.dirExists())
				return targetFolder;
			return null;
		}

        /// <summary>
        /// It is searching recursive or non recursive for a list of folders in current path.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        public static List<string>  folders(this string path, bool recursive = false)
        {
            return (path.isFolder())
                       ? Files.getListOfAllDirectoriesFromDirectory(path, recursive)
                       : new List<string>();
        }
        /// <summary>
        /// It is searching recursive or non recursive for a list of folders in current path by a specific search pattern.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="searchPattern"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        public static List<string>  folders(this string path, string searchPattern, bool recursive = false)
        {
            return (path.isFolder())
                       ? Files.getListOfAllDirectoriesFromDirectory(path, recursive, searchPattern)
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
        /// <summary>
        ///  checks if the provided directory exists and if not, calls <code>Files.checkIfDirectoryExistsAndCreateIfNot(directory);</code>
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static string        create_Folder(this string folder)
        {            
            return folder.createDir();
        }
        /// <summary>
        ///  checks if the provided directory exists and if not, calls <code>Files.checkIfDirectoryExistsAndCreateIfNot(directory);</code>
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        [Obsolete("createFolder is deprecated.Please use create_Folder instead")] 
        public static string createFolder(this string folder)
        {
            return folder.create_Folder();
        }
        /// <summary>
        ///  checks if the provided directory exists and if not, calls <code>Files.checkIfDirectoryExistsAndCreateIfNot(directory);</code>
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static string folder_Create(this string folder)
        {
            return folder.create_Folder();
        }
        /// <summary>
        /// Given a <code>file.valid()</code> path returns directory name
        /// </summary>
        /// <param name="file"></param>
        /// <returns>
        /// Returns directory name if the path is valid
        /// If filepath is root or empty then returns null
        /// Otherwise returns empty string.
        /// </returns>
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
        /// <summary>
        /// Checks if a path is folder or a file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool          isFolder(this string path)
        {
            return path.is_Folder();
        }
        /// <summary>
        /// Checks if a path is folder or a file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool is_Folder(this string path)
        {
            return path.dirExists();
        }
        /// <summary>
        /// Checks if a path is not folder or a file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool is_Not_Folder(this string path)
        {
            return path.dir_Not_Exists();
        }
        /// <summary>
        /// Creates a file in the provided folder.
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
        /// <summary>
        /// Delete all files in current directory.
        /// </summary>
        /// <param name="folder"></param>
        /// <returns>
        /// Returns true if folder is empty oterwhise returns false.
        /// </returns>
        public static bool          folder_Delete_Files(this string folder)
        {
            if (folder.isFolder() && folder.files().notEmpty())
            {                
                Files.deleteAllFilesFromDir(folder);
                return folder.files().empty();
            }
            return false;
        }
        /// <summary>
        /// Checks if folder does not exist
        /// </summary>
        /// <param name="path"></param>
        /// <returns>
        /// True if folder does not exist , false otherwise.
        /// </returns>
        public static bool          folder_Not_Exists(this string path)
        {
            return path.folder_Exists().isFalse();
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
        /// <summary>
        /// Checks if folder exists
        /// </summary>
        /// <param name="path"></param>
        /// <returns>
        /// Returns true if folder exists, false otherwise.
        /// </returns>
        public static bool          folder_Exists(this string path)
        {
            return path.dirExists();
        }
        /// <summary>
        /// Checks if folder exists
        /// </summary>
        /// <param name="path"></param>
        /// <returns>
        /// Returns true if folder exists, false otherwise.
        /// </returns>
        [Obsolete("folderExists is deprecated.Please use folder_Exists instead.")]
        public static bool folderExists(this string path)
        {
            return path.folder_Exists();
        }
        /// <summary>
        /// Checks if folder exists
        /// </summary>
        /// <param name="path"></param>
        /// <returns>
        /// Returns true if folder exists, false otherwise.
        /// </returns>
        public static bool          dirExists(this string path)
        {
            if (path.valid())
                return Directory.Exists(path);
            return false;
        }
        /// <summary>
        /// Checks if folder exists
        /// </summary>
        /// <param name="path"></param>
        /// <returns>
        /// Returns true if folder exists, false otherwise.
        /// </returns>
        public static bool dir_Not_Exists(this string path)
        {
            if (path.valid())
                return !Directory.Exists(path);
            return false;
        }   
        /// <summary>
        /// It is searching non recursive for a list of folders in current path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<string>  dirs(this string path)
        {
            return path.folders();
        }
        /// <summary>
        /// Returns a new instance of <code>DirectoryInfo</code> class for a valid directory path 
        /// Returns null of the provided folder doesn't exist
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public static DirectoryInfo directoryInfo(this string directoryPath)
        {
            return directoryPath.folder_Not_Exists() ? null : new DirectoryInfo(directoryPath);
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
        /// <summary>
        /// Creates a temporary folder 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="appendRandomStringToFolderName"></param>
        /// <returns>
        /// If the path is valid then it will return newly created folder appended with a random string
        /// Otherwise will return <code>PublicDI.config.O2TempDir</code> path
        /// </returns>
        public static string temp_Folder(this string name, bool appendRandomStringToFolderName = true)
        {
            return name.temp_Dir(appendRandomStringToFolderName);
        }
        /// <summary>
        /// Creates a temporary folder 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="appendRandomStringToFolderName"></param>
        /// <returns>If the path is valid then it will return newly created folder appended with a random string
        /// Otherwise will return <code>PublicDI.config.O2TempDir</code> path
        /// </returns>
        public static string temp_Dir(this string name, bool appendRandomStringToFolderName = true)
        {
            return name.tempDir(appendRandomStringToFolderName);
        }
    }
}