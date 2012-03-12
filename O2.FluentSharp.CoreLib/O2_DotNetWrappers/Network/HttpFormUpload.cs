// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using O2.Kernel;
using O2.Kernel.ExtensionMethods;
using O2.DotNetWrappers.ExtensionMethods;

namespace O2.DotNetWrappers.Network
{
	// this code was based on the code from from http://www.briangrinstead.com/blog/multipart-form-post-in-c
	// which was created following this StackOverflow thread http://stackoverflow.com/questions/219827/multipart-forms-from-c-client
	
	public class HttpMultiPartForm
	{
		/* example */
		/*public static HttpWebResponse uploadFile()
		{			
			string fileToUpload = "c:\\people.doc";
			string fileName =  "People.doc";
			string fileFormat =  "doc";
			string fileContentType = "application/msword";
			string postURL = "http://localhost";
			string userAgent = "Someone";
			string cookies = "";
			return uploadFile(fileToUpload ,fileName ,fileFormat ,fileContentType , postURL, userAgent, cookies);
		}*/
		//
        public Dictionary<string, string> Headers_Request { get; set; }
        public Dictionary<string, string> Headers_Response { get; set; }

        public HttpMultiPartForm()
        {
            Headers_Request = new Dictionary<string, string>();
            Headers_Response = new Dictionary<string, string>();
        }        

        #region uploadFile

        public HttpWebResponse uploadFile(string fileToUpload, string fileName, string fileFormat, string fileContentType , 
							   string postURL, string userAgent, string cookies)
		{		
			// Read file data
			FileStream fs = new FileStream(fileToUpload, FileMode.Open, FileAccess.Read);
			byte[] data = new byte[fs.Length];
			fs.Read(data, 0, data.Length);
			fs.Close();
			return uploadFile(data, fileName ,fileFormat ,fileContentType , postURL, userAgent, cookies);			
		}	
		
		public HttpWebResponse uploadFile(byte[] data, string fileName, string fileFormat, string fileContentType , 
							   string postURL, string userAgent, string cookies)
		{
			// Generate post objects
			Dictionary<string, object> postParameters = new Dictionary<string, object>();
			postParameters.Add("filename", fileName);
			postParameters.Add("fileformat", fileFormat);
			var postedFileHttpFieldName = "file";
			return uploadFile(data, postParameters, fileName, fileContentType, postURL, userAgent, postedFileHttpFieldName, cookies);
		}
		
		public HttpWebResponse uploadFile(byte[] data, Dictionary<string, object> postParameters, string fileName, string fileContentType , 
							   string postURL, string userAgent, string postedFileHttpFieldName, string cookies)
		{
			
			postParameters.Add(postedFileHttpFieldName, new HttpMultiPartForm.FileParameter(data, fileName, fileContentType));
			 			
			return MultipartFormDataPost(postURL, userAgent, postParameters, cookies);
			
			//return webResponse.ResponseUri;
			//show.info(webResponse);
			
			// Process response
			/*StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
			string fullResponse = responseReader.ReadToEnd();
			webResponse.Close();
			return fullResponse;			//DC: Modified so that we return the response as string
			*/
		}			
	    
		private readonly Encoding encoding = Encoding.UTF8;
		
		public HttpWebResponse MultipartFormDataPost(string postUrl, string userAgent, Dictionary<string, object> postParameters, string cookies)
		{
			string formDataBoundary = "-----------------------------28947758029299";
			string contentType = "multipart/form-data; boundary=" + formDataBoundary;
	 
			byte[] formData = GetMultipartFormData(postParameters, formDataBoundary);
	 
			var response = PostForm(postUrl, userAgent, contentType, formData, cookies);
            this.gcCollect();
            return response;
		}
        #endregion 

        #region PostForm
        private HttpWebResponse PostForm(string postUrl, string userAgent, string contentType, byte[] formData, string cookies)
		{            
            try
            {

                HttpWebRequest webRequest = WebRequest.Create(postUrl) as HttpWebRequest;

                webRequest.Timeout = Web.DefaultHttpWebRequestTimeout;
                webRequest.ReadWriteTimeout = Web.DefaultHttpWebRequestTimeout;
                if (webRequest == null)
                {
                    throw new NullReferenceException("request is not a http request");
                }

                // Set up the request properties
                webRequest.Method = "POST";
                webRequest.ContentType = contentType;
                webRequest.UserAgent = userAgent;
                //request.CookieContainer = new CookieContainer();            
                webRequest.Headers.Add("Cookie", cookies);
                foreach (var extraHeader in Headers_Request)
                    webRequest.Headers.Add(extraHeader.Key,extraHeader.Value);
                webRequest.ContentLength = formData.Length;  // We need to count how many bytes we're sending. 

                this.gcCollect();                            // Hack: need this or after a couple requests the webRequest.GetRequestStream() will hang
                
                using (Stream requestStream = webRequest.GetRequestStream())
                {
                    // Push it out there
                    requestStream.Write(formData, 0, formData.Length);
                    requestStream.Close();
                    System.GC.Collect();
                }

                var response = webRequest.GetResponse() as HttpWebResponse;
                updateHeadersResponse(response.Headers);               // store response headers
                return response;                       
            }
            catch (Exception ex)
            {
                ex.log("in HttpFormUpload.PostForm");
                this.gcCollect();                       //Hack: need this or after a couple requests the webRequest.GetRequestStream() will hang
                if (ex is WebException)
                {
                    var webResponse = (ex as WebException).Response as HttpWebResponse;
                    return webResponse;
                }
                return null;
            }
		}
	 
		private byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
		{
			Stream formDataStream = new System.IO.MemoryStream();
	 
			foreach (var param in postParameters)
			{
				if (param.Value is FileParameter)
				{
					FileParameter fileToUpload = (FileParameter)param.Value;
	 
					// Add just the first part of this param, since we will write the file data directly to the Stream
					string header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\";\r\nContent-Type: {3}\r\n\r\n", 
						boundary, 
						param.Key, 
						fileToUpload.FileName ?? param.Key, 
						fileToUpload.ContentType ?? "application/octet-stream");
	 
					formDataStream.Write(encoding.GetBytes(header), 0, header.Length);
	 
					// Write the file data directly to the Stream, rather than serializing it to a string.
					formDataStream.Write(fileToUpload.File, 0, fileToUpload.File.Length);
				}
				else
				{
					string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}\r\n", 
						boundary, 
						param.Key, 
						param.Value);
					formDataStream.Write(encoding.GetBytes(postData), 0, postData.Length);
				}
			}
	 
			// Add the end of the request
			string footer = "\r\n--" + boundary + "--\r\n";
			formDataStream.Write(encoding.GetBytes(footer), 0, footer.Length);
	 
			// Dump the Stream into a byte[]
			formDataStream.Position = 0;
			byte[] formData = new byte[formDataStream.Length];
			formDataStream.Read(formData, 0, formData.Length);
			formDataStream.Close();
	 
			return formData;
		}

        #endregion

        #region support for custom headers (needs to be merged with the code in Web.cs
        public Dictionary<string, string> add_Header(string name, string value)
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
        #endregion

        #region class FileParameter
        public class FileParameter
		{
			public byte[] File { get; set; }
			public string FileName { get; set; }
			public string ContentType { get; set; }
			public FileParameter(byte[] file) : this(file, null) { }
			public FileParameter(byte[] file, string filename) : this(file, filename, null) { }
			public FileParameter(byte[] file, string filename, string contenttype) 
			{
				File = file;
				FileName = filename;
				ContentType = contenttype;
			}
        }
        #endregion

    }
    
}
