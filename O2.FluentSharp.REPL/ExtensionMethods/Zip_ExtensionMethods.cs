using System;
using System.Collections.Generic;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.REPL.Utils;
using Ionic.Zip;

namespace FluentSharp.REPL
{
    public static class Zip_ExtensionMethods
    {
        public static string        zip_File(this string strFileToZip, string strTargetZipFileName)
        {
            return new zipUtils().zipFile(strFileToZip, strTargetZipFileName);
        }
        public static string        zip_Folder(this string strPathOfFolderToZip, string strTargetZipFileName)
        {
            return new zipUtils().zipFolder(strPathOfFolderToZip, strTargetZipFileName);
        }
        public static List<string>  zip_getListOfFilesInZip(this string sZipFileToLoad)
        {
            return new zipUtils().getListOfFilesInZip(sZipFileToLoad);
        }
        public static string        unzip_File(this string fileToUnzip)
        {
            return new zipUtils().unzipFile(fileToUnzip);
        }
        public static string        unzip_File(this string fileToUnzip, string targetFolder)
        {
            return new zipUtils().unzipFile(fileToUnzip,targetFolder);
        }
        public static string        unzip_FirstFile(this string zipFile)
		{
			return zipFile.unzip_FirstFile(false);
		}
		public static string        unzip_FirstFile(this string zipFile, bool overwrite)
		{
			try
			{
				if (zipFile.fileExists().isFalse())
				{
					"[in unzip_FirstFile] provided zipFile was e was not found {0}".info(zipFile);
					return null;
				}
				var targetDir = "_UnzippedSingleFiles".tempDir(false);	
				var zpZipFile = new ZipFile(zipFile);
				var zipEntry = zpZipFile.Entries.first();				
				var targetFile = targetDir.pathCombine(zipEntry.FileName);
				if (targetFile.fileExists())
					if (overwrite.isFalse())
						return targetFile;
					else
						"[in unzip_FirstFile] target file already exists (will be overwriten): {0}".info(targetFile);
				zipEntry.Extract(targetDir,ExtractExistingFileAction.OverwriteSilently);			
				if (targetFile.fileExists())
					return targetFile;
				"[in unzip_FirstFile] after unzip target file was not found {0}".info(targetFile);
			}
			catch(Exception ex)
			{
				ex.log("[in unzip_FirstFile");		
			}
			return null;
		}
        public static List<string>  unzip_FileAndReturnListOfUnzipedFiles(this string fileToUnzip)
        {
            return new zipUtils().unzipFileAndReturnListOfUnzipedFiles(fileToUnzip);
        }
        public static List<string>  unzip_FileAndReturnListOfUnzipedFiles(this string fileToUnzip, string targetFolder)
        {
            return new zipUtils().unzipFileAndReturnListOfUnzipedFiles(fileToUnzip, targetFolder);
        }
		public static string        zip_File(this string filesToZip)
		{
			return filesToZip.zip_File(".zip".tempFile());
		}		
		public static string        zip_Folder(this string filesToZip)
		{
			return filesToZip.zip_Folder(".zip".tempFile());
		}		
		public static string        zip_Files(this List<string> filesToZip)
		{
			return filesToZip.zip_Files(".zip".tempFile());
		}		
		public static string        zip_Files(this List<string> filesToZip, string targetZipFile)//, string baseFolder)
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
		public static string        unzip(this string zipFile, string targetFolder)
		{
			return new zipUtils().unzipFile(zipFile, targetFolder);
		}
        
    }
}
