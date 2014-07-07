using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentSharp.CoreLib.API;

namespace FluentSharp.CoreLib
{
    public static class IO_ExtensionMethods_File_Open
    {        
        public static string        file_Contents(this string file)
        {
            return file.contents();
        }
        public static string        file_Contents(this string folder, string file)
		{
			return folder.pathCombine(file).fileContents();
		}
        public static string        filesContents(this List<string> files)
        {
            var filesContents = "";
            foreach (var file in files)
                filesContents += file.fileContents().line();
            return filesContents;
        }
        public static string        fileContents(this string file)
        {
            return file.contents();
        }
        public static byte[]        fileContents_AsByteArray(this string file)
        {
            return Files.getFileContentsAsByteArray(file);
        }
        public static string        fileSnippet(this string file, int startLine, int endLine)
        {
            if (file.fileExists().isFalse())
            {
                "in fileSnippet, request file didn't exist: {0}".format(file).error();
                return "";
            }
            if (startLine == endLine)
            {
                "in fileSnippet, provided start line is equal to end end line: {0}".format(startLine).error();
                return "";
            }
            var fileLines = file.fileContents().split_onLines();
            var numberOfLines = fileLines.size();
            if (startLine > endLine || numberOfLines < endLine)
            {
                "in fileSnippet, problem with the provided start line ({0}), end line ({1}) or file lines ({2}) values"
                    .format(startLine, endLine, numberOfLines).error();
                return "";
            }
            var snippet = fileLines.GetRange(startLine, endLine - startLine);
            "snippet size : {0}".format(snippet.size()).debug();
            return StringsAndLists.fromStringList_getText(snippet).trim();
        }
        public static string        contents(this string file)
        {
            return file.contents(true);
        }
        public static string        contents(this string file, bool autoExtractH2Files)
        {
            if (file.valid())
            {                
                if (autoExtractH2Files && file.extension(".h2"))
                    return file.h2_SourceCode();
                return Files.getFileContents(file);
            }
            return "";
        }
        public static byte[]        contentsAsBytes(this string file)
        {
            if (file.fileExists())
                return Files.getFileContentsAsByteArray(file);
            return null;
        }
        public static bool          file_CanOpen(this string file)
        {
            try
            {
                using (File.OpenWrite(file))
                {
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static string        file_WaitFor_CanOpen(this string file)
        {
            if (file.isFile())
            {
                int maxWait = 10;
                for (int i = 0; i < maxWait; i++)
                {
                    if (file.file_CanOpen())
                        return file;
                    "[file_WaitFor_CanOpen] Trying to get lock into file #{0}: {1} ".info(i, file);
                    file.wait(500, false);
                }
                "[file_WaitFor_CanOpen] After #{0} tried, Could not get lock into file: {1} ".info(maxWait, file);
            }
            return null;
        }
        public static string        file(this string folder, string virtualFilePath)
        {
            var mappedFile = folder.pathCombine(virtualFilePath);
            if (mappedFile.fileExists())
                return mappedFile; 
            return null;
        }	
        public static List<string>  files(this string path)
        {
            return path.files("", false);
        }
        public static List<string>  files(this string path, string  searchPattern)
        {
            return path.files(searchPattern, false);
        }
        public static List<string>  files(this string path, string searchPatterns, bool recursive)
        {
            return path.files(searchPatterns.wrapOnList(), recursive);
        }
        public static List<string>  files(this string path, List<string> searchPatterns)
        {
            return path.files(searchPatterns, false);
        }
        public static List<string>  files(this string path, bool recursive, params string[] searchPatterns)
        {
            if (searchPatterns.empty())
                searchPatterns = new[] {"*.*"};
            return path.files(searchPatterns.toList(), recursive);
        }
        public static List<string>  files(this string path, List<string> searchPatterns, bool recursive)
        {            
            return (path.isFolder()) 
                       ? Files.getFilesFromDir_returnFullPath(path, searchPatterns, recursive)
                       : new List<string>();
        }        
        public static List<string>  files(this string folder, bool recursiveSearch, string filter)
        {
            return Files.getFilesFromDir_returnFullPath(folder, filter, recursiveSearch);
        }  
        
        public static List<string>  files(this List<string> folders)
        {
            return folders.files("*.*");
        }		
        public static List<string>  files(this List<string> folders, string filter)
        {
            return folders.files(filter,false);
        }		
        public static List<string>  files(this List<string> folders, string filter, bool recursive)
        {
            var files = new List<string>();
            foreach(var folder in folders)
                files.AddRange(folder.files(filter, recursive));
            return files;
        }		
        
        public static string        pathCombine(this string folder, string path)
        {            
            return pathCombine_MaxSize(folder, path);
        }
        public static string        pathCombine_MaxSize(this string folder, string path )
        {
            if (path.notValid())
                return folder;
            if (folder.notValid())
                return null;
            var maxLength = 256 - folder.size();
            if(maxLength < 10)
                throw new Exception("in pathCombine_MaxSize folder name is too large: {0}".format(folder.size()));

            // add the file hash if too big
            if (path.size() > maxLength)
                path = "{0} ({1}){2}".format(
                    path.Substring(0, maxLength - 20),
                    path.hash(),
                    path.Substring(path.size() - 9).extension());
            
            if (path.StartsWith("/") || path.StartsWith(@"\"))           // need to remove a leading '/' or '\' or the Path.Combine doesn't work properly
                path = path.Substring(1);

            return Path.Combine(folder, path).fullPath();
        }
        public static string        pathCombine_WithTempDir(this string fileOrPath)
        {
            return PublicDI.config.O2TempDir.pathCombine(fileOrPath.fileName());
        }
        /* public static string        pathCombine_With_ExecutingAssembly_Folder(this string path)
        {
            try
            {
                return Assembly.GetExecutingAssembly().location().parentFolder().pathCombine(path);
            }
            catch (Exception ex)
            {
                ex.log("[in pathCombine_With_ExecutingAssembly_Folder]")
            }
            return null;
        }*/
        public static string        fullPath(this string path)
        {
            try
            {
                return Path.GetFullPath(path);
            }
            catch (Exception ex)
            {
                ex.log("[in fullPath] for: {0}".format(path));
                return path;
            }
        }
        public static bool          fileContains(this string pathToFileToCompile, string textToFind)
        {
            var fileContents = pathToFileToCompile.fileContents();
            return fileContents.contains(textToFind);
        }
        public static string        fileInsertAt(this string filePath, int location, string textToInsert)
        {
            var fileContents = filePath.fileContents();
            return fileContents.Insert(location, textToInsert).saveAs(filePath);
        }
        public static List<string>  onlyValidFiles(this List<string> files)
        {
            return (from file in files
                    where file.fileExists()
                    select file).toList();
        }
        public static List<string> onlyValidFolders(this List<string> folders)
        {
            return (from folder in folders
                    where folder.folderExists()
                    select folder).toList();
        }
        public static List<string>  lines(this string text, bool removeEmptyLines)
        {
            if (removeEmptyLines)
                return text.lines();
            return text.fix_CRLF()
                       .Split(new [] { Environment.NewLine }, StringSplitOptions.None )
                       .toList();
        }
        public static List<string>  filesContains(this List<string> files, string textToSearch)
        {
            return (from file in files
                    where file.fileContents().contains(textToSearch)
                    select file).toList();
        }		
        public static List<string>  filesContains_RegEx(this List<string> files, string regExToSearch)
        {
            return (from file in files
                    where file.fileContents().regEx(regExToSearch)
                    select file).toList();
        }		
        public static string        fromLines_getText(this List<string> lines)
        {
            return StringsAndLists.fromStringList_getText(lines);
        }
        
        public static Dictionary<string,string>         files_Indexed_by_FileName(this string path)
        {
            return	 path.files().files_Indexed_by_FileName();
        }		
        public static Dictionary<string,string>         add_Files_Indexed_by_FileName(this Dictionary<string,string> mappedFiles, string path)
        {
            foreach(var item in path.files_Indexed_by_FileName())
                mappedFiles.add(item.Key, item.Value);
            return mappedFiles;
        }		
        public static Dictionary<string,string>         files_Indexed_by_FileName(this List<string> files)
        {
            var files_Indexed_By_FileName = new Dictionary<string,string>();
            foreach(var file in files)			
                files_Indexed_By_FileName.add(file.fileName(), file);
            return files_Indexed_By_FileName;
        }		
        public static Dictionary<string,List<string>>   files_Mapped_by_Extension(this List<string> files)
        {
            var files_Indexed_By_FileName = new Dictionary<string,List<string>>();
            foreach(var file in files)			
                files_Indexed_By_FileName.add(file.extension(), file);
            return files_Indexed_By_FileName;
        }

        public static string        find_File_in_List(this List<string> files, params string[] fileNames)
        {
            foreach(var file in files)
                foreach(var fileName in fileNames)		
                    if (file.fileName() == fileName)
                        return file;
            return null;
        }		
        public static string        findFilesInFolder(this string folder, params string[] fileNames)
        {
            foreach(var fileName in fileNames)
            {
                var resolvedPath = folder.pathCombine(fileName);
                if (resolvedPath.fileExists())
                    return resolvedPath;
            }
            return null;
        }		
        public static string        findParentFolderCalled(this string fullPath, string folderToFind)
        {
            var parentFolder = fullPath.directoryName();
            if (folderToFind.valid() && parentFolder.notNull())
            {
                if (parentFolder.fileName() == folderToFind)
                    return parentFolder;
                return findParentFolderCalled(parentFolder,folderToFind);
            }
            return null;
        }
        public static string        file_Copy(this string file, string folder)
        {
            return file.file_CopyToFolder(folder);
        }
        public static List<string>  files_Copy(this List<string> files, string targetFolder)
        {
            foreach (var file in files)
                Files.copy(file, targetFolder);
            return files;
        }

    }
}