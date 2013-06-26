using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

using FluentSharp.ExtensionMethods;
using O2.DotNetWrappers.Windows;
using O2.Kernel;

namespace O2.DotNetWrappers.Network
{
    public class Web
    {
        public static int DefaultHttpWebRequestTimeout { get; set; }

        public Dictionary<string, string> Headers_Request { get; set; }
        public Dictionary<string, string> Headers_Response { get; set; }
        //public List<string> RequestHistory { get; set; }
        //public string LastRequest { get; set; }
        //public string LastResponse { get; set; }

        static Web()
        {
            DefaultHttpWebRequestTimeout = 1000 * 30;     //(30 seconds)
        }

        public Web()
        {
            Headers_Request = new Dictionary<string, string>();
            Headers_Response = new Dictionary<string, string>();
            //RequestHistory = new List<string>();
            //LastRequest = "";
            //LastResponse = "";
        }

        public Dictionary<string,string> set_Cookies(string value)
        {
            Headers_Request.add("Cookie", value);
            return Headers_Request;
        }
        public string get_Cookies()
        {
            return Headers_Request.value("Cookie");            
        }
        public Dictionary<string,string> add_Header(string name, string value)
        {
            Headers_Request.add(name, value);
            return Headers_Request;
        }
        public string get_Header(string name)
        {
            return Headers_Request.value(name);            
        }
        public Dictionary<string, string> delete_Header(string name)
        {
            Headers_Request.delete(name);
            return Headers_Request;
        }
        public void updateHeadersResponse(WebHeaderCollection webHeaders)
        {            
            Headers_Response = new Dictionary<string, string>();
            if (webHeaders.notNull())
                foreach (string key in webHeaders.Keys)
                    Headers_Response.Add(key, webHeaders[key]);

        }
        public string saveUrlContents(string urlToFetch)
        {
            if (urlToFetch.validUri())
            {
                var uri = urlToFetch.uri();
                // first try to save the file using the original name
                urlToFetch = urlToFetch.Replace(uri.Query, "");
                string targetFile;
                if (uri.Segments.Length >0 && uri.Segments[uri.Segments.Length -1 ] == "/")
                    targetFile = PublicDI.config.TempFileNameInTempDirectory + ".html";
                else
                {
                    targetFile = PublicDI.config.O2TempDir.pathCombine(urlToFetch.fileName());
                    if (File.Exists(targetFile)) // but give it a unique name if that file alredy exists
                        targetFile = string.Format("{0}_{1}", PublicDI.config.TempFileNameInTempDirectory,
                                                   Path.GetFileName(urlToFetch));
                }
                //PublicDI.config.getTempFileInTempDirectory(Path.GetExtension(urlToFetch));                
                return saveUrlContents(urlToFetch, targetFile);
            }
            return "";
        }
        public string saveUrlContents(string urlToFetch, string targetFile)
        {
            var urlContents = getUrlContents(urlToFetch);
            if (urlContents != "")
            {
                if (Files.WriteFileContent(targetFile, urlContents))
                    return targetFile;
            }
            return "";

        }
        public String getUrlContents(String urlToFetch)
        {
            return getUrlContents(urlToFetch, false);
        }
        public String getUrlContents(String urlToFetch, bool verbose)
        {
            return getUrlContents(urlToFetch, null, verbose);
        }
        public String getUrlContents(String urlToFetch, string cookies, bool verbose)
        {        
            try
            {
                if (verbose)
                    PublicDI.log.info("Fetching url: {0}", urlToFetch);
                var webRequest = WebRequest.Create(urlToFetch) as HttpWebRequest;
                if (webRequest.isNull())
                    return null;

                webRequest.Timeout = Web.DefaultHttpWebRequestTimeout;
                webRequest.ReadWriteTimeout = Web.DefaultHttpWebRequestTimeout;

                //setup headers (& cookies)
                if (cookies != null && cookies.valid())
                    Headers_Request.add("Cookie", cookies);
                foreach (var header in Headers_Request)
                    webRequest.Headers.Add(header.Key, header.Value);

                var rResponse = webRequest.GetResponse();                

                updateHeadersResponse(rResponse.Headers);               // store response headers

                Stream sStream = rResponse.GetResponseStream();
                if (sStream.isNull())
                    return null;
                var srStreamReader = new StreamReader(sStream);
                string sHtml = srStreamReader.ReadToEnd();
                sStream.Close();
                srStreamReader.Close();
                rResponse.Close();
                return sHtml;
            }
            catch (Exception ex)
            {
                PublicDI.log.error("Error in getUrlContents: {0}", ex.Message);
                return "";
            }
        }
        public String getUrlContents_POST(String urlToFetch, string postData)
        {
            return getUrlContents_POST(urlToFetch, "application/x-www-form-urlencoded", postData);
        }
        public String getUrlContents_POST(String urlToFetch, string cookies, string postData)
        {
            return getUrlContents_POST(urlToFetch, "application/x-www-form-urlencoded", cookies, postData);
        }
        public String getUrlContents_POST(String urlToFetch, string contentType, string cookies, string postData)    
        {
            return getUrlContents_POST(urlToFetch, contentType, cookies, Encoding.ASCII.GetBytes(postData));
        }
        public String getUrlContents_POST(String urlToFetch, byte[] postData)
        {
            return getUrlContents_POST(urlToFetch, null, postData);
        }
        public String getUrlContents_POST(String urlToFetch, string cookies, byte[] postData)
        {
            return getUrlContents_POST(urlToFetch, "application/x-www-form-urlencoded", cookies, postData);
        }
        public String getUrlContents_POST(String urlToFetch,string contentType, string cookies, byte[] postData)
        {            
            try
            {
                // the Timeout and GC calls below were introduced due to GetResponseStream() hangs                
                var webRequest = (HttpWebRequest)WebRequest.Create(urlToFetch);
                webRequest.Timeout = DefaultHttpWebRequestTimeout;
                webRequest.ReadWriteTimeout = Web.DefaultHttpWebRequestTimeout;

                //setup headers (& cookies)   
                if (cookies != null && cookies.valid())
                    Headers_Request.add("Cookie", cookies);
                foreach (var header in Headers_Request)
                    webRequest.Headers.Add(header.Key, header.Value);

                // setup POST details:
                webRequest.Method = "POST";
                webRequest.ContentLength = postData.Length;
                webRequest.ContentType = contentType;
                using (Stream dataStream = webRequest.GetRequestStream())
                {
                    dataStream.Write(postData, 0, postData.Length);
                    dataStream.Close();
                    System.GC.Collect();
                }

                using (WebResponse rResponse = webRequest.GetResponse())
                {
                    updateHeadersResponse(rResponse.Headers);               // store response headers
                    using (Stream sStream = rResponse.GetResponseStream())
                    {
                        if (sStream.isNull())
                            return null;
                        var srStreamReader = new StreamReader(sStream);
                        string sHtml = srStreamReader.ReadToEnd();
                        sStream.Close();
                        srStreamReader.Close();
                        rResponse.Close();
                        return sHtml;
                    }
                }
            }
            catch (WebException webEx)
            {
                if (webEx.Response.notNull())
                    updateHeadersResponse(webEx.Response.Headers);                  // store response headers 
            }
            catch (Exception ex)
            {
                PublicDI.log.error("Error in getUrlContents: {0}", ex.Message);                
            }
            return "";
             //   });
            //thread.
        }                 
		public string downloadBinaryFile(string urlOfFileToFetch)
		{
			return downloadBinaryFile(urlOfFileToFetch, true);
		}						
        public string downloadBinaryFile(string urlOfFileToFetch, bool saveUsingTempFileName)
        {
        	string targetFile = String.Format("{0}.{1}", 
                									(saveUsingTempFileName) ? PublicDI.config.TempFileNameInTempDirectory + "_" : PublicDI.config.O2TempDir,
                                                    Path.GetFileName(urlOfFileToFetch));
            return downloadBinaryFile(urlOfFileToFetch,targetFile);
        }        
        public string downloadBinaryFile(string urlOfFileToFetch, string targetFileOrFolder)
        {
        	var targetFile = targetFileOrFolder;
            if (Directory.Exists(targetFileOrFolder))
                targetFile = targetFileOrFolder.pathCombine(urlOfFileToFetch.fileName());
        		
        	PublicDI.log.debug("Downloading Binary File {0}", urlOfFileToFetch);
            var webClient = new WebClient();
            try
            {
                //setup headers
                foreach (var header in Headers_Request)
                    webClient.Headers.Add(header.Key, header.Value);

                byte[] pageData = webClient.DownloadData(urlOfFileToFetch);
                Files.WriteFileContent(targetFile, pageData);
                PublicDI.log.debug("Downloaded File saved to: {0}", targetFile);

                updateHeadersResponse(webClient.ResponseHeaders);               // store response headers

                return targetFile;
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex);
            }
            return null;
        }
                
        // static methods
        public static bool Online
        {
            get { return "".online(); }
        }
        
        public class Https
        {
            public static void ignoreServerSslErrors()
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                        (obj, cert, chain, errors) => true;
            }
        }
    }
}
