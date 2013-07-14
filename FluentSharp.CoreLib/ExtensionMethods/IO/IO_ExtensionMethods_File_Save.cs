using System;
using System.IO;
using FluentSharp.CoreLib.API;

namespace FluentSharp.CoreLib
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
}