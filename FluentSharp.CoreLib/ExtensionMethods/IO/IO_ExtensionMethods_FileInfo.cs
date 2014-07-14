using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using FluentSharp.CoreLib.API;

namespace FluentSharp.CoreLib
{
    public static class IO_ExtensionMethods_FileInfo
    {
        public static FileAttributes    attributes(this FileInfo fileInfo)
        {
            if (fileInfo.valid())
                return fileInfo.Attributes;
            return default(FileAttributes);
        }

        public static FileInfo          fileInfo(this string filePath)
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
        public static bool              valid(this FileInfo fileInfo)
        {
            return fileInfo.notNull() && fileInfo.Exists;
        }
        public static FileInfo          readOnly_Remove(this FileInfo fileInfo)
        {
            if (fileInfo.valid())            
                fileInfo.Attributes = fileInfo.Attributes & ~FileAttributes.ReadOnly;                
            return fileInfo;
        }
        public static FileInfo          readOnly_Add(this FileInfo fileInfo)
        {
            if (fileInfo.valid())
                fileInfo.Attributes = fileInfo.Attributes | FileAttributes.ReadOnly;
            return fileInfo;
        }

        public static bool file_Has_Attribute(this string filePath, FileAttributes attribute)
        {
            var fileInfo = filePath.fileInfo();
            if (fileInfo.valid())
                return (fileInfo.Attributes == (fileInfo.Attributes | attribute));
            return false;
        }

        public static FileAttributes    file_Attributes(this string filePath)
        {
            var fileInfo = filePath.fileInfo();
            if (fileInfo.valid())
                return fileInfo.Attributes;
            return default(FileAttributes);
        }
        public static string            file_Attribute_ReadOnly_Add(this string filePath)
        {
            filePath.fileInfo().readOnly_Add();
            return filePath;
        }
        public static string            file_Attribute_ReadOnly_Remove(this string filePath)
        {
            filePath.fileInfo().readOnly_Remove();
            return filePath;
        }        

        public static List<string>      files_Attribute_ReadOnly_Remove(this List<string> files)
        {
            foreach (var file in files)
                file.file_Attribute_ReadOnly_Remove();
            return files;
        }
        public static string        path(this FileInfo fileInfo)
        {
            if (fileInfo.valid())
                return fileInfo.FullName;
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
        public static List<string>  fileNames(this string folder)
		{
			return folder.files().fileNames();
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
        /// <summary>
        /// Returns true if the file extension matches the provided value
        /// 
        /// extension value can be provided with our without the . (for example '.txt' or 'txt')
        /// 
        /// comparison is not case sensitive (i.e. both values are converter using <code>.lower()</code> before comparision is made
        /// </summary>
        /// <param name="file"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static bool          extension(this string file, string extension)
        {            
            if (file.valid() && extension.valid())
            {
                extension = extension.lower();
                var fileExtension = file.extension();

                if (extension.firstChar().equals("."))
                    return fileExtension.equals(extension);
                
                return fileExtension.equals(".".append(extension));                                
            }
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
        public static bool          files_Exist(this List<string> files)
        {
            if(files.empty())
                return false;
            foreach(var file in files)
                if (file.file_Not_Exists())
                    return false;
            return true;

        }
        public static bool          files_Not_Exist(this List<string> files)
        {
            if(files.empty())
                return true;
            foreach(var file in files)
                if (file.file_Exists())
                    return false;
            return true;
        }
        public static bool          file_Doesnt_Exist(this string file)
        {
            return file.file_Not_Exists();
        }
        public static bool          file_Not_Exists(this string file)
        {
            return file.fileExists().isFalse();
        }
        public static bool          file_Exists(this string file)
        {
            return file.fileExists();
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
    
        public static bool setAccessControl(this FileInfo fileInfo, string targetUser, FileSystemRights fileSystemRights, AccessControlType accessControlType)
        {
            if (fileInfo.notNull() && targetUser.notNull())
            {
                try
                {
                    var fileSystemAccessRule = new FileSystemAccessRule(targetUser, fileSystemRights, accessControlType);
                    var fileSecurity = new FileSecurity();
                    fileSecurity.AddAccessRule(fileSystemAccessRule);
                    fileInfo.SetAccessControl(fileSecurity);
                    return true;
                }
                catch (Exception ex)
                {
                    ex.log();
                }
            }
            return false;
        }
    }
}