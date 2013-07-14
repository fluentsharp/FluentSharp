using System;
using System.Linq;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.WinForms.Controls;


namespace FluentSharp.WinForms
{
    public static class Download_ExtensionMethods        
	{		
		public static string download(this string fileToDownload)
		{
			return fileToDownload.uri().download();
		}
		
		
		public static string download(this Uri uri)
		{
			return uri.download(true);
		}
		
		public static string download(this Uri uri, bool deleteIfTargetAlreadyExists)
		{
			return uri.downloadFile(deleteIfTargetAlreadyExists);
		}				
		
		public static string downloadFile(this Uri uri)
		{
			return uri.downloadFile(true);
		}
		
		public static string downloadFile(this Uri uri, bool deleteIfTargetAlreadyExists)
		{
			if (uri.isNull())
				return null;
			var fileName = uri.Segments.Last();
			if (fileName.valid())
			{
				var targetFile = "".tempDir().pathCombine(fileName);
				if (deleteIfTargetAlreadyExists)                    
					Files.deleteFile(targetFile);
				return downloadFile(uri, targetFile);
			}
			else
				"Could not extract filename from provided uri: {0}".error(uri.str());
			return null;					
		}
		
		public static string download(this string fileToDownload, string targetFile)
		{
			return downloadFile(fileToDownload.uri(),targetFile);
		}
		
		public static string downloadFile(this Uri uri, string targetFile)
		{
			if (uri.isNull())
				return null;
			"Downloading file {0} to location:{1}".info(uri.str(), targetFile);
			if (targetFile.fileExists())		// don't download if file already exists
			{
				"File already existed, so skipping download".debug();
				return targetFile;
			}
			var sync = new System.Threading.AutoResetEvent(false); 
			var downloadControl = O2Gui.open<DownloadFile>("Downloading: {0}".format(uri.str()), 455  ,170 );							
			downloadControl.setAutoCloseOnDownload(true);							
			downloadControl.setCallBackWhenCompleted((file)=> downloadControl.parentForm().close());
			downloadControl.onClosed(()=>sync.Set());
			downloadControl.setDownloadDetails(uri.str(), targetFile);							
			downloadControl.downloadFile();
			sync.WaitOne();					 	// wait for download complete or form to be closed
            targetFile.file_WaitFor_CanOpen();
            if (targetFile.fileExists())		
				return targetFile;
			return null;
		}								
	}
}
