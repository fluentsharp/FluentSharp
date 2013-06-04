using System;
using System.Net;
using System.IO;
using FluentSharp.ExtensionMethods;
using System.Net.NetworkInformation;

namespace O2.Kernel.CodeUtils
{
    public class O2Kernel_Web
    {
        public static bool SkipOnlineCheck { get; set; }

        static O2Kernel_Web()
        {
             SkipOnlineCheck = PublicDI.Offline;
        }

        //need these settings of the WebClient and GetResponseStream Http .NET clients methods will hang
        //for more references see:
        //  http://www.cnblogs.com/anders06/archive/2007/01/23/627698.html
        //  http://blogs.msdn.com/b/darrenj/archive/2005/03/07/386655.aspx
        //  http://stackoverflow.com/questions/1043234/strange-timeout-webexception-in-http-get-using-webclient 
        //  http://stackoverflow.com/questions/1485508/the-operation-has-timed-out-with-webclient-downloadfile-and-correct-urls 
        //   
        public static void ApplyNetworkConnectionHack()
        {
            ServicePointManager.DefaultConnectionLimit = 4096;
            ServicePointManager.CheckCertificateRevocationList = true;
        }

        public bool httpFileExists(string url)
        {
            return httpFileExists(url, false);
        }

        public bool httpFileExists(string url, bool showError)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Timeout = 10000;
            webRequest.Method = "HEAD";
            try
            {
                var webResponse = (HttpWebResponse)webRequest.GetResponse();
                return (webResponse.StatusCode == HttpStatusCode.OK && 
                        webResponse.ResponseUri.str() == url);
            }
            catch (Exception ex )
            {
                if (showError)
                    ex.log("in Web.httpFileExists");
				if (ex.Message.contains("SSL"))
					ex.log("in Web.httpFileExists ({0}) got SSL error: {1}".format(url, ex.Message));
                return false;
            }
        }

        public string downloadBinaryFile(string urlOfFileToFetch, string targetFileOrFolder)
        {
            var attempts = 5;
            while (attempts-- > 0)
            {
                var file = downloadBinaryFile_Action(urlOfFileToFetch, targetFileOrFolder);
                if (file.notNull())
                    return file;
                "[downloadBinaryFile] didn't so trying {0} more times".info(attempts);
            }
            return null;
        }
        public string downloadBinaryFile_Action(string urlOfFileToFetch, string targetFileOrFolder)
        {
            var targetFile = targetFileOrFolder;
            if (Directory.Exists(targetFileOrFolder))
                targetFile = targetFileOrFolder.pathCombine(urlOfFileToFetch.fileName());

            PublicDI.log.debug("Downloading Binary File {0}", urlOfFileToFetch);
            lock (this)
            {
                using (var webClient = new WebClient())
                {        
                    try
                    {                                                
                        byte[] pageData = webClient.DownloadData(urlOfFileToFetch);
                        O2Kernel_Files.WriteFileContent(targetFile, pageData);
                        PublicDI.log.debug("Downloaded File saved to: {0}", targetFile);

                        webClient.Dispose();

                        GC.Collect();       // because of WebClient().GetRequestStream prob
                        return targetFile;
                    }
                    catch (Exception ex)
                    {
                        PublicDI.log.ex(ex);
                    }
                }
            }
            GC.Collect();       // because of WebClient().GetRequestStream prob
            return null;
        }

        #region Ping
        public bool online()
        {
            return ping("www.google.com") ||                                // first ping, 
                   httpFileExists("https://www.google.com/favicon.ico");    // then try making a HEAD connection
        }

        public bool ping(string address)
        {         
            try
            {
                var pPing = new Ping();
                PingReply prPingReply = pPing.Send(address);
                return (prPingReply != null && prPingReply.Status == IPStatus.Success);
            }
            catch (Exception ex)
            {
                PublicDI.log.error("in Ping: {0}", ex.Message);
                return false;
            }
        }
        #endregion
    }
}
