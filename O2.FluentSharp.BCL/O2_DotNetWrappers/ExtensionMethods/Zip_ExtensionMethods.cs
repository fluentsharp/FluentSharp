using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using O2.DotNetWrappers.Zip;

namespace O2.DotNetWrappers.ExtensionMethods
{
    public static class Zip_ExtensionMethods
    {
        public static string zip_File(this string strFileToZip, string strTargetZipFileName)
        {
            return new zipUtils().zipFile(strFileToZip, strTargetZipFileName);
        }

        public static string zip_Folder(this string strPathOfFolderToZip, string strTargetZipFileName)
        {
            return new zipUtils().zipFolder(strPathOfFolderToZip, strTargetZipFileName);
        }

        public static List<string> zip_getListOfFilesInZip(this string sZipFileToLoad)
        {
            return new zipUtils().getListOfFilesInZip(sZipFileToLoad);
        }

        public static string unzip_File(this string fileToUnzip)
        {
            return new zipUtils().unzipFile(fileToUnzip);
        }

        public static string unzip_File(this string fileToUnzip, string targetFolder)
        {
            return new zipUtils().unzipFile(fileToUnzip,targetFolder);
        }

        public static List<string> unzip_FileAndReturnListOfUnzipedFiles(this string fileToUnzip)
        {
            return new zipUtils().unzipFileAndReturnListOfUnzipedFiles(fileToUnzip);
        }

        public static List<string> unzip_FileAndReturnListOfUnzipedFiles(this string fileToUnzip, string targetFolder)
        {
            return new zipUtils().unzipFileAndReturnListOfUnzipedFiles(fileToUnzip, targetFolder);
        }
    }
}
