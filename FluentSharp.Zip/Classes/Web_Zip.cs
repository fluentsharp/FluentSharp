using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using FluentSharp.CoreLib.API;
using System.IO;
using FluentSharp.CoreLib;
using FluentSharp.Web35.API;

namespace FluentSharp.Zip
{
    public class Web_Zip : Web
    {
        public List<String> downloadZipFileAndExtractFiles(string urlOfFileToFetch)
        {
            var webClient = new WebClient();
            try
            {
                //setup headers
                foreach (var header in Headers_Request)
                    webClient.Headers.Add(header.Key, header.Value);

                string tempFileName = String.Format("{0}_{1}.zip", PublicDI.config.TempFileNameInTempDirectory,
                                                    Path.GetFileNameWithoutExtension(urlOfFileToFetch));
                byte[] pageData = webClient.DownloadData(urlOfFileToFetch);
                Files.WriteFileContent(tempFileName, pageData);
                List<string> extractedFiles = new zipUtils().unzipFileAndReturnListOfUnzipedFiles(tempFileName);
                File.Delete(tempFileName);
                return extractedFiles;
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex);
            }
            return null;
        }


        public string checkIfFileExistsAndDownloadIfNot(string urlToDownloadFile)
        {
            return checkIfFileExistsAndDownloadIfNot(urlToDownloadFile.fileName(), urlToDownloadFile);
        }

        public string checkIfFileExistsAndDownloadIfNot(string file , string urlToDownloadFile)
        {
        	if (File.Exists(file))
        		return file;
            if (file.valid().isFalse())
                file = urlToDownloadFile.fileName();
            var localTempFile = urlToDownloadFile.extension(".zip") 
                ? PublicDI.config.getTempFileInTempDirectory(".zip")
                : Path.Combine(PublicDI.config.O2TempDir, file);
        	if (File.Exists(localTempFile))
        		return localTempFile;
            downloadBinaryFile(urlToDownloadFile, localTempFile);
        	//var downloadedFile = downloadBinaryFile(urlToDownloadFile, false /*saveUsingTempFileName*/);
            if (File.Exists(localTempFile))
        	{
                if (Path.GetExtension(localTempFile) != ".zip" && urlToDownloadFile.fileName().extension(".zip").isFalse())
                    return localTempFile;

                List<string> extractedFiles = new zipUtils().unzipFileAndReturnListOfUnzipedFiles(localTempFile, PublicDI.config.O2TempDir);
        		if (extractedFiles != null)
        			foreach(var extractedFile in  extractedFiles)
        				if (Path.GetFileName(extractedFile) == file)
        					return extractedFile;        					        		        		        		
        	}
        	return "";
        }

    }
}
