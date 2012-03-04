using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using O2.DotNetWrappers.Windows;
using O2.Kernel.ExtensionMethods;
using O2.Kernel;
using O2.DotNetWrappers.DotNet;

namespace O2.DotNetWrappers.ExtensionMethods
{
    public static class IO_ExtensionMethods
    {
        #region save

        public static string safeFileName(this DateTime dateTime)
        {
            return Files.getSafeFileNameString(dateTime.str());
        }

        public static string safeFileName(this string _string)
        {
            return _string.safeFileName(false);
        }

        public static string safeFileName(this string _string, bool prependBase64EncodedString)
        {
            return Files.getSafeFileNameString(_string,prependBase64EncodedString);
        }

        public static string safeFileName(this string _stringToConvert, int maxLength)
        {
            var safeName = _stringToConvert.safeFileName();
            if (maxLength > 10 && safeName.size() > maxLength)
                return "{0} ({1}){2}".format(
                            safeName.Substring(0, maxLength - 10),
                            3.randomNumbers(),
                            _stringToConvert.Substring(_stringToConvert.size() - 9).extension());
            return safeName;
        }

        public static string save(this string contents)
        {
            return contents.saveAs(PublicDI.config.TempFileNameInTempDirectory);
        }


        public static string save(this string fileContents, string targeFileName)
        {
            return fileContents.saveAs(targeFileName);
        }        

        public static string saveWithExtension(this string contents, string extension)
        {
            if (extension.starts("."))
                extension = extension.removeFirstChar();
            return contents.saveAs(PublicDI.config.getTempFileInTempDirectory(extension));
        }

        public static string saveWithName(this string contents, string fileName)
        {
            return contents.saveAs(PublicDI.config.O2TempDir.pathCombine(fileName));
        }

        public static string saveAs(this string contents, string targetFileName)
        {
            Files.WriteFileContent(targetFileName, contents);
            if (targetFileName.fileExists())
                return targetFileName;
            return "";
        }
        
        public static string save(this byte[] contents)
        {
            return contents.saveAs(PublicDI.config.TempFileNameInTempDirectory);
        }

        public static string saveAs(this byte[] contents, string targetFileName)
        {
            Files.WriteFileContent(targetFileName, contents);
            if (targetFileName.fileExists())
                return targetFileName;
            return "";
        }

        public static string fileTrimContents(this string filePath)
        {
            var fileContents = filePath.fileContents();
            if (fileContents.valid())
                fileContents.trim().save(filePath);
            return filePath;
        }
        #endregion

        public static string fileName(this string file)
        {
            if (file.valid())
                return Path.GetFileName(file);
            return "";
        }

        public static List<string> fileNames(this List<string> files)
        {
            var fileNames = from file in files
                            select file.fileName();
            return fileNames.toList();
        }

        public static string directoryName(this string file)
        {
            if (file.valid())
                return Path.GetDirectoryName(file);
            return "";            
        }

        public static string extension(this string file)
        {
            try
            {
                if (file.valid() && file.size() < 256)
                    return Path.GetExtension(file).ToLower();
            }
            catch
            {
                //return "";
            }
            return "";
        }

        public static bool extension(this string file, string extension)
        {
            if (file.valid())
                return file.extension() == extension;
            return false;
        }

        public static string extensionChange(this string file, string newExtension)
        {
            if (file.isFile())
                return Path.ChangeExtension(file, newExtension);
            return file;
        }

        public static bool exists(this string file)
        {
            if (file.valid())
                return file.fileExists() || file.dirExists();
            return false;
        }

        public static bool isFile(this string path)
        {            
            return path.fileExists();
        }

        public static bool isImage(this string path)
        {
            if (path.isFile().isFalse())
                return false;
            switch (path.extension())
            { 
                case ".gif":
                case ".jpg":
                case ".jpeg":
                case ".bmp":
                case ".png":
                    return true;
                default:
                    return false;
            }
        }

        public static bool isText(this string path)
        {
            if (path.isFile().isFalse())
                return false;
            switch (path.extension())
            {
                case ".txt":                
                    return true;
                default:
                    return false;
            }
        }

        public static bool isDocument(this string path)
        {
            if (path.isFile().isFalse())
                return false;
            switch (path.extension())
            {
                case ".rtf":
                case ".doc":
                    return true;
                default:
                    return false;
            }
        }

        public static bool fileExists(this string file)
        {            
            if (file.valid() && file.size() < 256)
                return File.Exists(file);
            return false;
        }

        public static bool isFolder(this string path)
        {
            return path.dirExists();
        }

        public static bool isBinaryFormat(this string file)
        {
            return file.fileContents().Contains("\0");
        }

        public static bool dirExists(this string path)
        {
            if (path.valid())
                return Directory.Exists(path);
            return false;
        }

        public static void create(this string file, string fileContents)
        {
            if (file.valid())
                Files.WriteFileContent(file, fileContents);            
        }

        public static string filesContents(this List<string> files)
        {
            var filesContents = "";
            foreach (var file in files)
                filesContents += file.fileContents().line();
            return filesContents;
        }

        public static string fileContents(this string file)
        {
            return file.contents();
        }

        public static byte[] fileContents_AsByteArray(this string file)
        {
            return Files.getFileContentsAsByteArray(file);
        }

        public static string fileSnippet(this string file, int startLine, int endLine)
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

        public static string contents(this string file)
        {
            if (file.valid())
                return Files.getFileContents(file);
            return "";
        }

        public static byte[] contentsAsBytes(this string file)
        {
            if (file.fileExists())
                return Files.getFileContentsAsByteArray(file);
            return null;
        }

        public static bool fileWrite(this string file, string fileContents)
        {
            return Files.WriteFileContent(file, fileContents);
        }

        public static List<T> wrapOnList<T>(this T item)
        {
            var list = new List<T>();
            list.add(item);
            return list;
        }

        public static List<string> files(this string path)
        {
            return path.files("", false);
        }

        public static List<string> files(this string path, string  searchPattern)
        {
            return path.files(searchPattern, false);
        }

        public static List<string> files(this string path, string searchPatterns, bool recursive)
        {
            return path.files(searchPatterns.wrapOnList(), recursive);
        }

        public static List<string> files(this string path, List<string> searchPatterns)
        {
            return path.files(searchPatterns, false);
        }

        public static List<string> files(this string path, bool recursive, params string[] searchPatterns)
        {
            return path.files(searchPatterns.toList(), recursive);
        }

        public static List<string> files(this string path, List<string> searchPatterns, bool recursive)
        {
            return (path.isFolder()) 
                ? Files.getFilesFromDir_returnFullPath(path, searchPatterns, recursive)
                : new List<string>();
        }

        public static List<string> files(this string folder, bool recursiveSearch)
        {
            return folder.files(recursiveSearch, "*.*");
        }

        public static List<string> files(this string folder, bool recursiveSearch, string filter)
        {
            return Files.getFilesFromDir_returnFullPath(folder, filter, recursiveSearch);
        }

        public static List<string> dirs(this string path)
        {
            return path.folders(false);
        }

        public static List<string> folders(this string path)
        {
            return path.folders(false);
        }

        public static List<string> folders(this string path, bool recursive)
        {
            return (path.isFolder())
                ? Files.getListOfAllDirectoriesFromDirectory(path, recursive)
                : new List<string>();
        }

        public static string pathCombine(this string folder, string file)
        {
            if (file.StartsWith("/"))           // need to remove a leading '/' or the Path.Combine doesn't work properly
                file = file.Substring(1);
            if (file.StartsWith(@"\"))           // need to remove a leading '\' or the Path.Combine doesn't work properly
                file = file.Substring(1);
            return Path.Combine(folder, file).fullPath();
        }

        public static string fullPath(this string path)
        {
            return Path.GetFullPath(path);
        }

        public static string createDir(this string directory)
        {
            return Files.checkIfDirectoryExistsAndCreateIfNot(directory);
        }

        public static string createFolder(this string folder)
        {            
            return folder.createDir();
        }

        public static bool fileContains(this string pathToFileToCompile, string textToFind)
        {
            var fileContents = pathToFileToCompile.fileContents();
            return fileContents.contains(textToFind);
        }

        public static string fileInsertAt(this string filePath, int location, string textToInsert)
        {
            var fileContents = filePath.fileContents();
            return fileContents.Insert(location, textToInsert).saveAs(filePath);
        }

        public static string askUser(this string question)
        {
            return question.askUser("O2 Question", "");
        }

        public static string askUser(this string question, string title, string defaultValue)
        {
            var assembly = "Microsoft.VisualBasic".assembly();
            var intercation = assembly.type("Interaction");

            var parameters = new object[] { question, title, defaultValue, -1, -1 };
            return intercation.invokeStatic("InputBox", parameters).str();
        }

        public static List<string> onlyValidFiles(this List<string> files)
        {
            return (from file in files
                    where file.fileExists()
                    select file).toList();
        }

        public static FileInfo fileInfo(this string filePath)
        {
            try
            {
                return new FileInfo(filePath);
            }
            catch (Exception ex)
            {
                ex.log("in filePath.fileInfo");
                return null;
            }
        }

        public static long size(this FileInfo fileInfo)
        {
            if (fileInfo.notNull())
                return fileInfo.Length;
            return -1;
        }
    }
}