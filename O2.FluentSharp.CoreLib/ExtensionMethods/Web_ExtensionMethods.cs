using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using O2.DotNetWrappers.Network;
using O2.DotNetWrappers.Windows;

using System.Drawing;
using System.IO;


namespace FluentSharp.ExtensionMethods
{
    public static class Web_ExtensionMethods_Http_Requests
    {
        public static string    getHtml(this Uri uri, bool showDebugMessage)
        {
            showDebugMessage.ifInfo("Downloading html code for: {0}", uri.str());
            return uri.getHtml();
        }        
        public static string    getUrlContents(this Uri uri)
        {
            return uri.getUrlContents(null);
        }
        public static string    getUrlContents(this Uri uri, string cookies)
        {
            return new Web().getUrlContents(uri.str(), cookies, false);
        }
        public static string    getHtml(this Uri uri)
        {
            return uri.getUrlContents();
        }
        public static string    getHtmlAndSave(this Uri uri)
        {
            return uri.getHtmlAndSaveOnFolder("".o2Temp2Dir());
        }
        public static string    getHtmlAndSaveOnFolder(this Uri uri, string targetFolder)
        {
            var html = uri.getHtml();
            if (html.valid())
            {
                var targeFileName = targetFolder.pathCombine(uri.fileNameFriendly());
                return html.save(targeFileName);
            }
            return "";
        }
        public static Bitmap    getImageAsBitmap(this Uri uri)
        {
            var webClient = new WebClient();
            var imageBytes = webClient.DownloadData(uri);
            "image size :{0}".info(imageBytes.size());
            var memoryStream = new MemoryStream(imageBytes);
            var bitmap = new Bitmap(memoryStream);
            return bitmap;
        }


        //GET requests

        public static string GET(this Uri uri)
        {
            return uri.get_Html();
        }
        public static string GET(this string url)
        {
            return url.get_Html();
        }
        public static string html(this string url)
        {
            return url.get_Html();
        }
        public static string get_Html(this string url)
        {
            if (url.isUri())
                return url.uri().get_Html();
            "in get_Html (GET), url provided was not valid URI: {0}".error(url);
            return null;
        }
        public static string get_Html(this Uri url)	// this is a better way to represent it
        {
            return url.getHtml();
        }

        //POST requests

        public static string POST(this Uri uri, byte[] postData)
        {
            return uri.get_Html(postData);
        }
        public static string POST(this Uri uri, string postData)
        {
            return uri.get_Html(postData);
        }
        public static string POST(this string url, string postData)
        {
            return url.html(postData);
        }
        public static string html(this string url, string postData)
        {
            return url.get_Html(postData);
        }
        public static string get_Html(this string url, string postData)
        {
            if (url.isUri())
                return url.uri().get_Html(postData);
            "in get_Html (POST), url provided was not  valid URI: {0}".error(url);
            return null;
        }
        public static string get_Html(this Uri url, string postData)
        {
            return new Web().getUrlContents_POST(url.str(), postData);
        }
        public static string get_Html(this Uri url, byte[] postData)
        {
            return new Web().getUrlContents_POST(url.str(), postData);
        }

        //HEAD requests
        public static WebHeaderCollection HEAD_Headers(this Uri uri)
        {
            var request = (HttpWebRequest) WebRequest.Create(uri);
            request.Timeout = 1000;
            request.AllowAutoRedirect = false;            
            request.Method = "HEAD";
            try
            {
                return request.GetResponse().Headers;                                
            }
            catch (WebException)
            {                
                return null;
            }
        }
        public static bool                HEAD(this Uri uri)
        {
            return uri.HEAD_Headers().notNull();
        }
        public static bool                httpFileExists(this string url)
        {
            return url.httpFileExists(false);
        }
        public static bool                httpFileExists(this string url, bool showError)
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
            catch (Exception ex)
            {
                if (showError)
                    ex.log("in Web.httpFileExists");
                if (ex.Message.contains("SSL"))
                    ex.log("in Web.httpFileExists ({0}) got SSL error: {1}".format(url, ex.Message));
                return false;
            }
        }
    }
    
    public static class Web_ExtensionMethods_URI
    {
        public static Uri       web(this string _string)
        {
            return _string.uri();
        }
        public static Uri       link(this string _string)
        {
            return _string.uri();
        }
        public static Uri       url(this string _string)
        {
            return _string.uri();
        }
        public static Uri       uri(this string _string)
        {
            try
            {
                if (_string.starts("about:"))   // deal with the fact that Uri throws an error for urls like about:blank
                    return null;
                if (_string.isFile().isFalse() && _string.isFolder().isFalse())                    
                    _string = _string.StartsWith("http") ? _string : @"http://" + _string;
                
                return new Uri(_string);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static List<Uri> uris(this List<string> urls)
		{
			return (from url in urls
					where url.isUri()
					select url.uri()).toList();
		}	
        public static bool      isUri(this string _string)
        {
            return _string.validUri();
        }
        public static bool      validUri(this string _string)
        {
            try
            {
                if (_string.valid() && _string.contains("://"))
                {
                    var uri = new Uri(_string);
                    return (uri.IsAbsoluteUri && uri.IsFile.isFalse());
                }
            }
            catch (Exception ex)
            {
                ex.log();
            }
            return false;
        }
        public static string    fileNameFriendly(this Uri uri)
        {
            return Files.getSafeFileNameString(uri.str());
        }
        public static bool      exists(this Uri uri)
        {
            return uri.str().httpFileExists();
        }
        public static string    hostUrl(this Uri uri)
        {
            return "{0}://{1}".format(uri.Scheme, uri.Host);
        }
        public static string    pathNoQuery(this Uri uri)
        {
            return uri.Query.valid()
                        ? uri.AbsoluteUri.remove(uri.Query)
                        : uri.AbsoluteUri;
        }
        public static Uri       append(this Uri uri, string virtualPath)
		{
			try
			{
				return new Uri(uri, virtualPath);
			}
			catch
			{
				return null;
			}
		}
        public static bool      isWebRequest(this Uri uri)
        {
            if (uri.notNull())
                return uri.Scheme.lower() == "html" || uri.Scheme.lower() == "https";
            return false;
        }
        
    }

    public static class Web_ExtensionMethods_QueryString
    { 
        public static List<string>              queryParameters_Names(this List<Uri> uris)
		{			
			return (from uri in uris
					from name in uri.queryParameters_Indexed_ByName().Keys					
					select name).Distinct().toList();
		}		
		public static List<string>              queryParameters_Values(this List<Uri> uris, string parameterName)
		{
			return (from uri in uris
					let parameters = uri.queryParameters_Indexed_ByName()										
					where parameters.hasKey(parameterName)					
					select parameters[parameterName]).toList();					
		}		
		public static List<string>              queryParameters_Values(this List<Uri> uris)
		{
			var values = new List<string>();
			foreach(var uri in uris)
				values.AddRange(uri.queryParameters_Indexed_ByName().Values);								
			return values;				
		}		
		public static Dictionary<string,string> queryParameters_Indexed_ByName(this Uri uri)
		{		
			var queryParameters = new Dictionary<string,string>();
			if (uri.notNull())
			{
				var query = uri.Query;
				if (query.starts("?"))
					query = query.removeFirstChar();
				foreach(var parameter in query.split("&"))				
					if (parameter.valid())
					{
						var splitParameter = parameter.split("=");
						if (splitParameter.size()==2)
							if (queryParameters.hasKey(splitParameter[0]))
							{	
								"duplicate parameter key in property '{0}', adding extra parameter in a new line".info(splitParameter[0]);
								queryParameters.add(splitParameter[0], queryParameters[splitParameter[0]].line() + splitParameter[1]);
							}
							else
								queryParameters.add(splitParameter[0], splitParameter[1]);
						else						
							"Something's wrong with the parameter value, there should only be one = in there: '{0}' ".info(parameter);
					}					
			} 
		return queryParameters;
		}
    }
    public static class Web_ExtensionMethods_Encoding
    {
        public static List<List<string>> encode(this List<List<string>> data, Func<string, string> encodeCallback)
        {
            foreach (var list in data)
                list.encode(encodeCallback);
            return data;
        }


        public static List<string> encode(this List<string> list, Func<string, string> encodeCallback)
        {
            for (int i = 0; i < list.size(); i++)
                list[i] = encodeCallback(list[i]);
            return list;
        }	

        public static byte      asciiByte(this char charToConvert)
        {
            try
            {
                return Encoding.ASCII.GetBytes(new [] { charToConvert })[0];
            }
            catch
            {
                return default(byte);
            }            
        }
        public static byte[]    asciiBytes(this string stringToConvert)
        {
            return Encoding.ASCII.GetBytes(stringToConvert);
        }
        public static string    base64Encode(this string stringToEncode)
        {
            try
            {
                return Convert.ToBase64String(stringToEncode.asciiBytes());
            }
            catch (Exception ex)
            {
                ex.log("in base64Encode");
                return "";
            }
        }
        public static string    base64Encode(this byte[] bytesToEncode)
        {
            try
            {
                return Convert.ToBase64String(bytesToEncode);
            }
            catch (Exception ex)
            {
                ex.log("in base64Encode");
                return "";
            }
        }
        public static string    base64Decode(this string stringToDecode)
        {
            try
            {
                return Convert.FromBase64String(stringToDecode).ascii();
            }
            catch (Exception ex)
            {
                ex.log("in base64Decode");
                return "";
            }
        }
        public static byte[]    base64Decode_AsByteArray(this string stringToDecode)
        {
            try
            {
                return Convert.FromBase64String(stringToDecode);
            }
            catch (Exception ex)
            {
                ex.log("in base64Decode");
                return null;
            }
        }
    }

    public static class Web_ExtensionMethods_Ping
    {     
        public static bool      ping(this string address)
        {
            return new Ping().ping(address);
        }
        public static bool      online(this object _object)
        {
            return ping("www.google.com") ||                                // first ping, 
                   "https://www.google.com/favicon.ico".httpFileExists();    // then try making a HEAD connection            
        }        
    }
}
