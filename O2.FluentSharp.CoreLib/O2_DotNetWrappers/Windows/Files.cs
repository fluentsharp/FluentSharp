// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using O2.DotNetWrappers.DotNet;
using O2.DotNetWrappers.O2Misc;

using O2.DotNetWrappers.ExtensionMethods;
using O2.Kernel;

namespace O2.DotNetWrappers.Windows
{
    public class Files
    {
        // code to use binaryreader:
        /*FileStream fileStream = File.Open(pathToProxyDll, FileMode.Open, FileAccess.Read);
                 
                BinaryReader  binaryReader = new BinaryReader(fileStream);
                Files.WriteFileContent(file, binaryReader);
                 */

        public static String copy(String sourceFile, String targetFileOrFolder)
        {
            return copy(sourceFile, targetFileOrFolder, false);
        }
        public static String copy(String sSourceFile, String sTargetFileOrFolder, bool overrideFile)
        {
            string sTargetFile = sTargetFileOrFolder;            
            if (Directory.Exists(sTargetFile))
                sTargetFile = Path.Combine(sTargetFile, Path.GetFileName(sSourceFile));
            try
            {
                if (sSourceFile != sTargetFile || overrideFile)
                    File.Copy(sSourceFile, sTargetFile, true);
                
                return sTargetFile;
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex, "in Files.Copy");
                return null;
            }            
        }
        public static string copyVerbose(string fileToCopy, string targetFolder, bool dontCopyIfTargetFileAlreadyExists)
        {
            var fileName = Path.GetFileName(fileToCopy);
            var targetFileLocation = Path.Combine(targetFolder, Path.GetFileName(fileToCopy));
            if (false == File.Exists(targetFileLocation))
            {
                PublicDI.log.write("copying file {0} to folder {1}", fileName, targetFolder);
                copy(fileToCopy, targetFolder);
            }
            else
                if (dontCopyIfTargetFileAlreadyExists)
                    PublicDI.log.write("skipping file: {0}", fileName);
                else
                {
                    PublicDI.log.write("over-writing file: {0}", fileName);
                    copy(fileToCopy, targetFolder);
                }
            return fileToCopy;
        }
        public static String moveFile(String sSourceFile, String sTargetFileOrDirectory)
        {
            try
            {
                string copiedFile = null;
                if (sSourceFile != sTargetFileOrDirectory)
                {
                    copiedFile = copy(sSourceFile, sTargetFileOrDirectory);
                    if (copiedFile != null)
                        deleteFile(sSourceFile);
                    //File.Move(sSourceFile, sTargetFile);
                }
                return copiedFile;
            }
            catch (Exception ex)
            {
                PublicDI.log.error("in MoveFile: {0}", ex.Message);
            }
            return "";
        }
        public static void copyDllAndPdb(String sSourceDll, String sTargetDll, bool bOveride)
        {
            try
            {
                if (File.Exists(sSourceDll))
                {
                    if (File.Exists(sTargetDll)) // copy the latest one
                    {
                    }
                    File.Copy(sSourceDll, sTargetDll, bOveride);
                    sSourceDll = Path.Combine(Path.GetDirectoryName(sSourceDll),
                                              Path.GetFileNameWithoutExtension(sSourceDll) + ".pdb");
                    if (File.Exists(sSourceDll))
                    {
                        sTargetDll = Path.Combine(Path.GetDirectoryName(sTargetDll),
                                                  Path.GetFileNameWithoutExtension(sTargetDll) + ".pdb");
                        File.Copy(sSourceDll, sTargetDll, bOveride);
                    }
                }
            }
            catch (Exception ex)
            {
                PublicDI.log.error("in copyDllAndPdb:{0}", ex.Message);
            }
        }
        public static bool copyFilesFromDirectoryToDirectory(String sSourceDirectory, String sTargetDirectory)
        {
            return copyFolder(sSourceDirectory, sTargetDirectory);
            /*
            if (false == Directory.Exists(sSourceDirectory))
            {
                PublicDI.log.error("Source Directory doesn't exist:{0}", sSourceDirectory);
                return false;
            }
            if (false == Directory.Exists(sTargetDirectory))
            {
                Directory.CreateDirectory(sTargetDirectory);
                if (false == Directory.Exists(sTargetDirectory))
                {
                    PublicDI.log.error("Target Directory doesn't exist:{0}", sTargetDirectory);
                    return false;
                }
            }
            foreach (String sFile in Directory.GetFiles(sSourceDirectory))
                copy(sFile, Path.Combine(sTargetDirectory, Path.GetFileName(sFile)));
            return true;*/
        }
        public static string getTempFolderName()
        {
            String sTempFileName = Path.GetTempFileName();
            File.Delete(sTempFileName);
            return Path.GetFileNameWithoutExtension(sTempFileName);
        }
        public static string getTempFileName()
        {
            String sTempFileName = Path.GetTempFileName();
            File.Delete(sTempFileName);
            return Path.GetFileName(sTempFileName);
        }
        public static bool deleteFile(String fileToDelete)
        {            
            return deleteFile(fileToDelete, false);
        }
        public static bool deleteFile(String fileToDelete, bool logFileDeletion)
        {
            try
            {
                if (File.Exists(fileToDelete))
                {
                    if (fileToDelete.file_WaitFor_CanOpen().notNull())
                    {
                        File.Delete(fileToDelete);
                        if (logFileDeletion)
                            PublicDI.log.error("Deleted File :{0}:", fileToDelete);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                PublicDI.log.error("In deleteFile:{0}:", ex.Message);
            }
            return false;
        }
        public static void deleteFolder(String sFolderToDelete)
        {
            deleteFolder(sFolderToDelete, false);
        }
        public static bool deleteFolder(String sFolderToDelete, bool bRecursive)
        {
            try
            {
                if (Directory.Exists(sFolderToDelete))
                {
                    deleteAllFilesFromDir(sFolderToDelete);
                    Directory.Delete(sFolderToDelete, bRecursive);
                    return true;
                }
            }
            catch (Exception ex)
            {
                PublicDI.log.error("In deleteFolder:{0}:", ex.Message);                
            }
            return false;
        }
        public static void deleteAllFilesFromDir(String targetDir)
        {
            deleteFilesFromDirThatMatchPattern(targetDir, "*.*");
        }
        public static void deleteFilesFromDirThatMatchPattern(String targetDir, String searchPattern)
        {
            List<String> lsFiles = getFilesFromDir_returnFullPath(targetDir, searchPattern);
            foreach (String sFile in lsFiles)
                try
                {
                    File.Delete(sFile);
                }
                catch (Exception ex)
                {
                    PublicDI.log.error("In deleteFilesFromDirThatMatchPattern:{0}:", ex.Message);
                }
        }
        public static String getFirstFileFromDirThatMatchesPattern_returnFulPath(String sTargetDir,String sSearchPattern)
        {
            List<String> lsFiles = getFilesFromDir_returnFullPath(sTargetDir, sSearchPattern, true);
            if (lsFiles.Count > 0)
                return lsFiles[0];
            return "";
        }
        public static List<String> getFilesFromDir(String sTargetDir)
        {
            return getFilesFromDir(sTargetDir, "*.*");
        }
        public static List<String> getFilesFromDir(String sTargetDir, String sSearchPattern)
        {
            var lsFiles = new List<string>();
            if (Directory.Exists(sTargetDir))
                foreach (String sFile in Directory.GetFiles(sTargetDir, sSearchPattern))
                    lsFiles.Add(Path.GetFileName(sFile));
            else
                PublicDI.log.error("in getFilesFromDir, sTargetDir doesn't exist: {0}", sTargetDir);
            return lsFiles;
        }
        public static List<String> getFilesFromDir_returnFullPath(String sPath)
        {
            return getFilesFromDir_returnFullPath(sPath, "*.*", false);
        }
        public static List<String> getFilesFromDir_returnFullPath(String path, String searchPattern)
        {
            return getFilesFromDir_returnFullPath(path, searchPattern, false);
        }
        public static List<String> getFilesFromDir_returnFullPath(String path, List<String> searchPatterns,bool searchRecursively)        
        {
            var results = new List<String>();
            try
            {
                foreach (var searchPattern in searchPatterns)
                    results.AddRange(getFilesFromDir_returnFullPath(path, searchPattern, searchRecursively));
            }
            catch (Exception ex)
            {
                ex.log("[getFilesFromDir_returnFullPath] for path : {0}".format(path));
            }
            return results;
        }
        // returns full paths instead of just the file names
        public static List<String> getFilesFromDir_returnFullPath(String path, String searchPattern,bool searchRecursively)
        {
            var results = new List<String>();
            getListOfAllFilesFromDirectory(results, path, searchRecursively, searchPattern, false /*verbose*/);
            ///  foreach (String sFile in lsFiles)
            //      lsFiles.Add(Path.Combine(sPath, sFile));

            return results;
        }
        public static List<String> getListOfAllFilesFromDirectory(String sStartDirectory, bool bSearchRecursively, O2Thread.FuncVoidT1<List<String>> onComplete)
        {
            var lsFiles = new List<string>();
            O2Thread.mtaThread(
                () =>
                    {
                        getListOfAllFilesFromDirectory(lsFiles, sStartDirectory, bSearchRecursively, "*.*", false);
                        onComplete(lsFiles);
                    });
            return lsFiles;
        }
        public static List<String> getListOfAllFilesFromDirectory(String sStartDirectory, bool bSearchRecursively)
        {
            return getListOfAllFilesFromDirectory(sStartDirectory, bSearchRecursively, "*.*");
        }
        public static List<String> getListOfAllFilesFromDirectory(String sStartDirectory, bool bSearchRecursively,String sSearchPattern)
        {
            var lsFiles = new List<string>();
            //bool bVerbose = false;
            getListOfAllFilesFromDirectory(lsFiles, sStartDirectory, bSearchRecursively, sSearchPattern, false
                /*bVerbose*/);
            return lsFiles;
        }        
        public static void getListOfAllFilesFromDirectory(List<String> lsFiles, String sStartDirectory,bool bSearchRecursively, String sSearchPattern, bool bVerbose)
        {
        try
            {
                sStartDirectory = sStartDirectory.Trim();
                if (Directory.Exists(sStartDirectory))
                {
                    if (sSearchPattern == "")
                        lsFiles.AddRange(Directory.GetFiles(sStartDirectory));
                    else
                    {
                        var searchOptions = (bSearchRecursively) ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
                        String[] sFileMatches = Directory.GetFiles(sStartDirectory, sSearchPattern, searchOptions);
                        if (bVerbose)
                            foreach (String sFile in sFileMatches)
                                PublicDI.log.debug("File matched filter: {0}", sFile);
                        lsFiles.AddRange(sFileMatches);
                    }
                    /*if (bSearchRecursively)
                        foreach (String sDirectory in Directory.GetDirectories(sStartDirectory))
                            getListOfAllFilesFromDirectory(lsFiles, sDirectory, true,
                                                           sSearchPattern,
                                                           bVerbose);*/
                }
            }
            catch (Exception ex)
            {
                PublicDI.log.debug("Error in getListOfAllFilesFromDirectory {0}", ex.Message);
            }
        }
        public static List<String> getListOfAllDirectoriesFromDirectory(String startDirectory, bool searchRecursively)
        {
            return getListOfAllDirectoriesFromDirectory(startDirectory, searchRecursively, "");
        }
        public static List<String> getListOfAllDirectoriesFromDirectory(String sStartDirectory, bool bSearchRecursively,String sSearchPattern)
        {
            var lsDirectories = new List<string>();
            getListOfAllDirectoriesFromDirectory(lsDirectories, sStartDirectory, bSearchRecursively, sSearchPattern,
                                                 false /*verbose*/);
            return lsDirectories;
        }
        public static void getListOfAllDirectoriesFromDirectory(List<String> lsDirectories, String sStartDirectory,bool bSearchRecursively, String sSearchPattern,bool bVerbose)
        {
            try
            {
                sStartDirectory = sStartDirectory.Trim();
                if (Directory.Exists(sStartDirectory))
                {
                    if (sSearchPattern == "")
                        lsDirectories.AddRange(Directory.GetDirectories(sStartDirectory));
                    else
                    {
                        String[] asDirectoryMatches = Directory.GetDirectories(sStartDirectory, sSearchPattern);
                        if (bVerbose)
                            foreach (String sDirectory in asDirectoryMatches)
                                PublicDI.log.debug("File matched filter: {0}", sDirectory);
                        lsDirectories.AddRange(asDirectoryMatches);
                    }
                    if (bSearchRecursively)
                        foreach (String sDirectory in Directory.GetDirectories(sStartDirectory))
                            getListOfAllDirectoriesFromDirectory(lsDirectories, sDirectory, true /*bSearchRecursively*/,
                                                                 sSearchPattern, bVerbose);
                }
                else
                    PublicDI.log.debug("Directory does not exist: {0}", sStartDirectory);
            }
            catch (Exception ex)
            {
                PublicDI.log.error("Error in getListOfAllDirectoriesFromDirectory {0}", ex.Message);
            }
        }
        public static List<String> findFiles(String sPathToFolder, String sFilter)
        {
            PublicDI.log.debug("Discovering all files that match the pattern: {0} in the directory: {1}", sFilter,
                         sPathToFolder);
            List<String> lsFiles = getFiles(sPathToFolder, sFilter);
            PublicDI.log.debug("{0} Files Found", lsFiles.Count);
            foreach (String sFile in lsFiles)
                PublicDI.log.info(sFile);
            return lsFiles;
        }
        public static void saveAsFile_StringList(String sFileToSave, List<String> lsFileContents)
        {
            try
            {
                var swStreamWriter = new StreamWriter(sFileToSave);
                foreach (String sLine in lsFileContents)
                    swStreamWriter.WriteLine(sLine);
                swStreamWriter.Close();
            }
            catch (Exception ex)
            {
                PublicDI.log.error("Error in saveAsFile_StringList {0}", ex.Message);
            }
        }
        public static List<String> getFiles(String sPath, String sFilter)
        {
            var lsFiles = new List<String>();
            getListOfAllFilesFromDirectory(lsFiles, sPath, true, sFilter, false);
            return lsFiles;
        }
        public static List<String> getFileLines(String sFileToOpen)
        {
            FileStream fs = null;
            StreamReader sr = null;
            var lsFileLines = new List<string>();
            //var sbFileContents = new StringBuilder();
            try
            {
                if (sFileToOpen.fileExists())
                {
                    fs = File.OpenRead(sFileToOpen);
                    sr = new StreamReader(fs);
                    while (false == sr.EndOfStream)
                        lsFileLines.Add(sr.ReadLine());
                }
            }
            catch (Exception ex)
            {
                PublicDI.log.error("Error in getFileLines {0}", ex.Message);
            }
            if (sr != null)
                sr.Close();
            if (fs != null)
                fs.Close();
            return lsFileLines;
        }
        public static byte[] getFileContentsAsByteArray(string sFileToOpen)
        {
            if (false == File.Exists(sFileToOpen))
                return null;            
            //StreamReader sr = null;            
            try
            {
                var fileSize = sFileToOpen.fileInfo().size();
                var fileContents = new byte[fileSize];
                using (FileStream fs = File.OpenRead(sFileToOpen))
                {                    
                    fs.Read(fileContents, 0, fileContents.Length);
                }
                return fileContents;                
            }
            catch(Exception ex)
            {
                PublicDI.log.ex(ex, "in getFileContentsAsByteArray");
                return null;
            }
        }       
        public static string getFileContents(string sFileToOpen)
        {
            if (false == File.Exists(sFileToOpen))
                return "";
            FileStream fs = null;
            StreamReader sr = null;
            string strContent = "";
            try
            {
                fs = File.OpenRead(sFileToOpen);
                sr = new StreamReader(fs);
                strContent = sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                PublicDI.log.error("Error GetFileContent {0}", ex.Message);
            }
            if (sr != null)
                sr.Close();
            if (fs != null)
                fs.Close();
            return strContent;
        }
        public static bool WriteFileContent(string targetFile, string newFileContent)
        {
            return WriteFileContent(targetFile, newFileContent, false);
        }
        public static bool WriteFileContent(string targetFile, string newFileContent, bool dontWriteIfTargetFileIsTheSameAsString)
        {
           if (newFileContent.empty())
                return false;
            if (File.Exists(targetFile) && dontWriteIfTargetFileIsTheSameAsString)
            {
                var existingFileContents = getFileContents(targetFile);
                if (existingFileContents == newFileContent)
                    return true;
            }
            return WriteFileContent(targetFile, new UTF8Encoding(true).GetBytes(newFileContent));
        }
        public static bool WriteFileContent(string strFile, Byte[] abBytes)
        {
            try
            {
                if (File.Exists(strFile))
                    deleteFile(strFile);

                using (FileStream fs = File.Create(strFile))
                {
                    fs.Write(abBytes, 0, abBytes.Length);
                    fs.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                PublicDI.log.error("Error WriteFileContent {0}", ex.Message);
            }
            return false;
        }
        public static String checkIfDirectoryExistsAndCreateIfNot(String directory, bool deleteFiles)
        {
            checkIfDirectoryExistsAndCreateIfNot(directory);
            if (deleteFiles)
                deleteAllFilesFromDir(directory);
            return directory;
        }
        public static String checkIfDirectoryExistsAndCreateIfNot(String directory)
        {
            try
            {                
                if (Directory.Exists(directory))
                    return directory;
                Directory.CreateDirectory(directory);
                if (Directory.Exists(directory))
                    return directory;
            }
            catch (Exception e)
            {
                PublicDI.log.error("Could not create directory: {0} ({1})", directory, e.Message);
            }
            return "";
        }
        /*   public static List<String> loadSourceFileIntoList(String sPathToSourceCodeFile)
        {
            var lsSourceCode = new List<string>();
            String sFileContents = getFileContents(sPathToSourceCodeFile);
            if (sFileContents != "")
            {
                sFileContents = sFileContents.Replace("\r\n", "\n").Replace("\n", Environment.NewLine);
                // fix the files only use \n for line breaks
                lsSourceCode.AddRange(sFileContents.Split(new[] {Environment.NewLine}, StringSplitOptions.None));
            }
            return lsSourceCode;
        }*/

        public static String getLineFromSourceCode(UInt32 uLineNumber, List<String> lsSourceCode)
        {
            if (uLineNumber > 0 && uLineNumber < lsSourceCode.Count)
                return lsSourceCode[(int) uLineNumber - 1];
            if (lsSourceCode.Count >0)
                PublicDI.log.error("In getLineFromSourceCode uLineNumeber==0 || uLineNumber >= lsSourceCode.Count");
            return "";
        }
        public static bool copyFolder(string sourceFolder, string targetFolder)
        {
            return copyFolder(sourceFolder, targetFolder, true /*copyRecursively*/ , false /*dontCreateSourceFolderInTarget */, "" /*ignoreFolderWith*/);
        }
        public static bool copyFolder(string sourceFolder, string targetFolder, bool copyRecursively, bool dontCreateSourceFolderInTarget, string ignoreFolderWith)
        {
            try
            {
                "Copying folder {0} to  {1}   (copyRecursively: {2} dontCreateSourceFolderInTarget : {3})".format(sourceFolder, targetFolder, copyRecursively, dontCreateSourceFolderInTarget).info();
                if (false == Directory.Exists(sourceFolder))
                    PublicDI.log.error("in copyFolder , sourceFolder doesn't exist: {0}", sourceFolder);
                else if (false == Directory.Exists(targetFolder))
                    PublicDI.log.error("in copyFolder , targetFolder doesn't exist: {0}", targetFolder);
                else
                {
                    List<string> foldersToCreate = getListOfAllDirectoriesFromDirectory(sourceFolder, copyRecursively);
                    foldersToCreate.Add(sourceFolder);
                    //var filesToCopy = getListOfAllFilesFromDirectory(sourceFolder, copyRecursively);
                    var pathReplaceString = (dontCreateSourceFolderInTarget)
                                                ? sourceFolder
                                                : Path.GetDirectoryName(sourceFolder);
                    foreach (string folder in foldersToCreate)
                    {
                        if (ignoreFolderWith.valid().isFalse() || folder.contains(ignoreFolderWith).isFalse())
                        {
                            string folderToCopyFiles = targetFolder.pathCombine(folder.Replace(pathReplaceString, ""));
                            if (false == Directory.Exists(folderToCopyFiles))
                                Directory.CreateDirectory(folderToCopyFiles);
                            if (folderToCopyFiles.dirExists())
                            {
                                List<string> filesToCopy = getListOfAllFilesFromDirectory(folder, false /*searchRecursively*/);
                                foreach (string file in filesToCopy)
                                    copy(file, folderToCopyFiles);
                            }
                            else
                                "in Files.copyFolder, it was not possible to created folder to copy files: {0}".error(folderToCopyFiles);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                ex.log("Copy Folder] {0} -> {1}".format(sourceFolder, targetFolder));
                return false;
            }
        }
        public static String getFileSaveDateTime_Now()
        {
            //String sFileSafeNowDateTime = " (" + DateTime.Now.ToShortDateString() + "," +
            //                              DateTime.Now.ToShortTimeString() + ") ";
            //sFileSafeNowDateTime = sFileSafeNowDateTime.Replace("/", "_").Replace(":", "_");
            var sFileSafeNowDateTime = DateTime.Now.ToString(" (HH\\h mm\\s ss\\m\\s , dd-MM-yy)");
            return sFileSafeNowDateTime;
        }
        /*public static long getFileSize(string assessmentRunFileToImport)
        {
            if (!File.Exists(assessmentRunFileToImport))
                return -1;
            var fileInfo = new FileInfo(assessmentRunFileToImport);
            return fileInfo.Length;
        }*/
        public static string getSafeFileNameString(string stringToParse)
        {
            return getSafeFileNameString(stringToParse, false);
        }
        public static string getSafeFileNameString(string stringToParse, bool prependBase64EncodedString)
        
        {
            var validCharsRegEx = @"[A-Z]|[a-z]|[0-9]|[\.]";   //|[\(\)\s]";

            var safeString = new StringBuilder(stringToParse);
            for(int i=0; i<safeString.Length;i++)
            {
                try
                {
                    if (false == RegEx.findStringInString(safeString[i].ToString(), validCharsRegEx))
                    {
                        var cc = safeString[i];
                        safeString[i] = '_';
                    }
                }
                catch
                {
                    safeString[i] = '_';
                }
            }
            if (prependBase64EncodedString)
                return "{1} - {0}".format(stringToParse.base64Encode(), safeString.ToString());
            return safeString.ToString();
        }
        public static bool createFile_IfItDoesntExist(string pathToFileToCreate, object fileContents)
        {
            try
            {
                if (false == File.Exists(pathToFileToCreate))
                {
                    if (fileContents.GetType() == typeof(byte[]))
                        WriteFileContent(pathToFileToCreate, (byte[])fileContents);
                    else
                        WriteFileContent(pathToFileToCreate, fileContents.ToString());                    
                }
                return File.Exists(pathToFileToCreate);

            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex);
                return false;
            }            
        }
        public static bool IsExtension(string pathToFile, string extentionToCheck)
        {
            if (String.IsNullOrEmpty(extentionToCheck))
                return false;
            if (extentionToCheck[0] != '.')   // check if the first char is a dot (.)
                extentionToCheck = "." + extentionToCheck;
            return Path.GetExtension(pathToFile) == extentionToCheck;            
        }
        public static void setCurrentDirectoryToExecutableDirectory()
        {
            Environment.CurrentDirectory = PublicDI.config.CurrentExecutableDirectory;
        }
        public static void renameFolder(string oldName, string newName)
        {
            try
            {
                System.IO.Directory.Move(oldName, newName);
            }
            catch (Exception ex)
            {
                ex.log("in Files.renameFolder");
            }            
        }
    }
}
