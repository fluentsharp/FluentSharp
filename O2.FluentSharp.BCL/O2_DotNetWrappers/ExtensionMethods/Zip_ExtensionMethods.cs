using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using O2.DotNetWrappers.Zip;
using O2.DotNetWrappers.Windows;
using Ionic.Zip;

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
		public static string zip_File(this string filesToZip)
		{
			return filesToZip.zip_File(".zip".tempFile());
		}		
		public static string zip_Folder(this string filesToZip)
		{
			return filesToZip.zip_Folder(".zip".tempFile());
		}		
		public static string zip_Files(this List<string> filesToZip)
		{
			return filesToZip.zip_Files(".zip".tempFile());
		}		
		public static string zip_Files(this List<string> filesToZip, string targetZipFile)//, string baseFolder)
		{		
			"Creating ZipFile with {0} files to {1}".info(filesToZip.size(), targetZipFile);
			if (targetZipFile.fileExists())
				Files.deleteFile(targetZipFile);
            var zpZipFile = new ZipFile(targetZipFile);            
            foreach(var fileToZip in filesToZip)
            {            	
            	{
            		zpZipFile.AddFile(fileToZip);            
            	}
            	//catch(Exception ex)
            	{
            	//	"[zip_Files] {0} in file {1}".error(ex.Message, fileToZip);
            	}
            }
            zpZipFile.Save();
            zpZipFile.Dispose();
            return targetZipFile;        
		}		
		public static string unzip(this string zipFile, string targetFolder)
		{
			return new zipUtils().unzipFile(zipFile, targetFolder);
		}
    }
}
