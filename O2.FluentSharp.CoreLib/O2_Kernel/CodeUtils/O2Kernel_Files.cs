// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.IO;

using System.Text;

//O2File:../PublicDI.cs
//O2File:../ExtensionMethods/String_ExtensionMethods.cs

namespace FluentSharp.CoreLib.API
{
    public class O2Kernel_Files
    {        

        // also on O2_DotNetWrappers.Windows.Files
        public static String Copy(String sSourceFile, String sTargetFileOrFolder)
        {
            string sTargetFile = sTargetFileOrFolder;
            if (Directory.Exists(sTargetFile))
                sTargetFile = sTargetFile.pathCombine(sSourceFile.fileName());
            try
            {
                File.Copy(sSourceFile, sTargetFile, true);
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex, "in O2Kernel_Files.Copy");
            }
            return sTargetFile;
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
                    File.Delete(fileToDelete);
                    if (logFileDeletion)
                        PublicDI.log.error("Deleted File :{0}:", fileToDelete);
                }
                return true;
            }
            catch (Exception ex)
            {
                PublicDI.log.error("In deleteFile:{0}:", ex.Message);
            }
            return false;
        }

    }
}
