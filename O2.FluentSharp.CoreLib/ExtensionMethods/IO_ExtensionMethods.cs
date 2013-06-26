using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using O2.DotNetWrappers.Windows;
using O2.Kernel;
using O2.DotNetWrappers.DotNet;

namespace FluentSharp.ExtensionMethods
{
    public static class IO_ExtensionMethods_File_Save
    {        
        public static bool      fileWrite(this string file, string fileContents)
        {
            return Files.WriteFileContent(file, fileContents);
        }
        public static void      create(this string file, string fileContents)
        {
            if (file.valid())
                Files.WriteFileContent(file, fileContents);            
        }
        public static string    save(this string contents)
        {
            return contents.saveAs(PublicDI.config.TempFileNameInTempDirectory);
        }
        public static string    save(this string fileContents, string targeFileName)
        {
            return fileContents.saveAs(targeFileName);
        }        
        public static string    saveWithExtension(this string contents, string extension)
        {
            if (extension.starts("."))
                extension = extension.removeFirstChar();
            return contents.saveAs(PublicDI.config.getTempFileInTempDirectory(extension));
        }
        public static string    saveWithName(this string contents, string fileName)
        {
            return contents.saveAs(PublicDI.config.O2TempDir.pathCombine(fileName));
        }        
        public static string    saveAs(this string contents, string targetFileName)
        {
            Files.WriteFileContent(targetFileName, contents);
            if (targetFileName.fileExists())
                return targetFileName;
            return "";
        }
        public static string    save(this byte[] contents)
        {
            return contents.saveAs(PublicDI.config.TempFileNameInTempDirectory);
        }
        public static string    saveAs(this byte[] contents, string targetFileName)
        {
            Files.WriteFileContent(targetFileName, contents);
            if (targetFileName.fileExists())
                return targetFileName;
            return "";
        }
        public static bool      createEmptyFile(this string targetFileName)
        {
            Files.WriteFileContent(targetFileName, new byte[] { });
            if (targetFileName.fileExists() && targetFileName.fileContents().empty())
                return true;
            return false;
        }
        public static bool      canSaveToFile(this string targetFileName)
        {
            var fileExisted = targetFileName.fileExists();
            var originalFileContents = (fileExisted)
                                            ? targetFileName.fileContents()
                                            : null;
            try
            {
                var testContent = "This is a test content";
                testContent.saveAs(targetFileName);
                if (testContent != targetFileName.fileContents())
                    return false;
                if (fileExisted)
                    originalFileContents.saveAs(targetFileName);
                else
                    File.Delete(targetFileName);
                return true;
            }
            catch
            {
                return false;
            }

        }
        public static bool      canWriteToPath(this string path)
        {
            try
            {
                //var files = path.files();
                var tempFile = path.pathCombine("tempFile".add_RandomLetters());
                "test content".saveAs(tempFile);
                if (tempFile.fileExists())
                {
                    File.Delete(tempFile);
                    if (tempFile.fileExists().isFalse())
                        return true;
                }
                "[in canWriteToPath] test failed for for path: {0}".error(path);
            }
            catch (Exception ex)
            {
                ex.log("[in canWriteToPath] for path: {0} : {1}", path, ex.Message);
            }
            return false;
        }        
        public static bool      canNotWriteToPath(this string path)
        {
            return path.canWriteToPath().isFalse();
        }
        
        public static string    fileTrimContents(this string filePath)
        {
            var fileContents = filePath.fileContents();
            if (fileContents.valid())
                fileContents.trim().save(filePath);
            return filePath;
        }
        public static string    inTempDir(this string fileName)
        {
            return PublicDI.config.O2TempDir.pathCombine(fileName);
        }
    }

    public static class IO_ExtensionMethods_FileInfo
    {
        public static FileInfo      fileInfo(this string filePath)
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
        public static string        file_Attribute_ReadOnly_Remove(this string filePath)
        {
            var fileInfo = filePath.fileInfo();
            if (fileInfo.notNull())
            {
                fileInfo.Attributes = fileInfo.Attributes & ~FileAttributes.ReadOnly;
                return filePath;
            }
            return null;
        }
        public static long          size(this FileInfo fileInfo)
        {
            if (fileInfo.notNull())
                return fileInfo.Length;
            return -1;
        }
        public static string        safeFileName(this DateTime dateTime)
        {
            return Files.getSafeFileNameString(dateTime.str());
        }
        public static string        safeFileName(this string _string)
        {
            return _string.safeFileName(false);
        }
        public static string        safeFileName(this string _string, bool prependBase64EncodedString)
        {
            return Files.getSafeFileNameString(_string,prependBase64EncodedString);
        }
        public static string        safeFileName(this string stringToConvert, int maxLength)
        {
            var safeName = stringToConvert.safeFileName();
            if (maxLength > 10 && safeName.size() > maxLength)
                return "{0} ({1}){2}".format(
                            safeName.Substring(0, maxLength - 10),
                            3.randomNumbers(),
                            stringToConvert.Substring(stringToConvert.size() - 9).extension());
            return safeName;
        }
        public static string        fileName(this string file)
        {
            try
            {
                if (file.valid())
                    return Path.GetFileName(file);
            }
            catch (Exception ex)
            {
                ex.log("[in fileName] for file: {0}".format(file));
            }
            return "";
        }
        public static List<string>  fileNames(this List<string> files)
        {
            var fileNames = from file in files
                            select file.fileName();
            return fileNames.toList();
        }   
        public static string        fileName_WithoutExtension(this string filePath)
        {
            return Path.GetFileNameWithoutExtension(filePath);
        }
        public static string        extension(this string file)
        {
            try
            {
                if (file.valid() && file.size() < 256)
                    return Path.GetExtension(file).lower();
            }
            catch
            {
                //return "";
            }
            return "";
        }
        public static bool          extension(this string file, string extension)
        {
            if (file.valid())
                return file.extension() == extension;
            return false;
        }
        public static string        extensionChange(this string file, string newExtension)
        {
            if (file.isFile())
                return Path.ChangeExtension(file, newExtension);
            return file;
        }
        public static bool          exists(this string file)
        {
            if (file.valid())
                return file.fileExists() || file.dirExists();
            return false;
        }
        public static bool          isFile(this string path)
        {            
            return path.fileExists();
        }
        public static bool          isImage(this string path)
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
        public static bool          isText(this string path)
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
        public static bool          isDocument(this string path)
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
        public static bool          fileExists(this string file)
        {
            try
            {
                if (file.valid() && file.size() < 256)
                    return File.Exists(file);
            }            
            catch            
            { }
            return false;
        }        
        public static bool          isBinaryFormat(this string file)
        {
            try
            {
                return file.fileContents_AsByteArray().Contains((byte)0);
            }
            catch (Exception ex)
            {
                ex.log("in file.isBinaryFormat for: {0}", file);
                return false;
            }
            
        }
        public static bool          fileName_Is(this string file, params string[] values)
        {
            var fileName = file.fileName();
            return fileName.eq(values);
        }
        public static bool          fileName_Contains(this string file, params string[] values)
        {
            var fileName = file.fileName();
            return fileName.contains(values);
        }
    }

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

    public static class IO_ExtensionMethods_File_Open
    {        
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
            if (file.valid())
            {
                if (file.extension(".h2"))
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
            return text.fixCRLF()
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

    public static class IO_ExtensionMethods_Delete_or_Copy
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
                if (waitForCanOpen)
                    path.file_WaitFor_CanOpen();
                File.Delete(path);
                "Deleted file: {0}".info(path);
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

    public static class IO_ExtensionMethods_AskUser
    {
        public static string        askUser(this string question)
        {
            return question.askUser("O2 Question", "");
        }
        public static string        askUser(this string question, string title, string defaultValue)
        {
            var assembly = "Microsoft.VisualBasic".assembly();
            var intercation = assembly.type("Interaction");

            var parameters = new object[] { question, title, defaultValue, -1, -1 };
            return intercation.invokeStatic("InputBox", parameters).str();
        }
    }

    public static class IO_ExtensionMethods_MemoryStream
    {
        public static MemoryStream  stream(this string initialValue)
        {
            return initialValue.memoryStream();
        }
        public static MemoryStream  memoryStream(this string initialValue)
        {
            return new MemoryStream().write(initialValue);
        }
        public static MemoryStream  writeLine(this MemoryStream memoryStream)
        {
            return memoryStream.writeLine("");
        }
        public static MemoryStream  writeLine(this MemoryStream memoryStream, string text)
        {
            return memoryStream.write(text.line());
        }
        public static MemoryStream  write(this MemoryStream memoryStream)
        {
            return memoryStream.write("");
        }
        public static MemoryStream  write(this MemoryStream memoryStream, string text)
        {
            var bytes = text.asciiBytes();
            "write size: {0} - {1} : {2}".debug(bytes.size(), text.size(), text);
            memoryStream.Write(bytes, 0, bytes.size());
            return memoryStream;
        }
        public static string        ascii(this MemoryStream memoryStream)
        {
            return memoryStream.ToArray().ascii();
        }
        public static MemoryStream  stream_UFT8(this string text)
		{
			var memoryStream = text.valid()
				       ? new MemoryStream(Encoding.UTF8.GetBytes(text))
				       : new MemoryStream();
			memoryStream.Flush();
			return memoryStream;
		}
    }
    
}