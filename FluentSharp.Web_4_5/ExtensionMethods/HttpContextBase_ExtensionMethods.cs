using System.IO;
using System.Web;

namespace FluentSharp.Moq
{
    public static class HttpContextBase_ExtensionMethods
    { 
        public static HttpContextBase request_Write(this HttpContextBase httpContextBase,string text)
        {														
            httpContextBase.stream_Write(httpContextBase.Request.InputStream, text);			
            return httpContextBase;
        }
                
        public static string request_Read(this HttpContextBase httpContextBase)
        {					
            return httpContextBase.stream_Read(httpContextBase.Request.InputStream);
        }
        
        public static HttpContextBase response_Write(this HttpContextBase httpContextBase,string text)
        {														
            httpContextBase.stream_Write(httpContextBase.Response.OutputStream, text);			
            return httpContextBase;
        }
        
        public static string response_Read(this HttpContextBase httpContextBase)
        {					
            return httpContextBase.stream_Read(httpContextBase.Response.OutputStream);						
        }
        
        public static string response_Read_All(this HttpContextBase httpContextBase)
        {
            httpContextBase.Response.OutputStream.Flush();
            httpContextBase.Response.OutputStream.Position = 0;
            return httpContextBase.response_Read();
        }
        
        public static HttpContextBase stream_Write(this HttpContextBase httpContextBase, Stream inputStream, string text)
        {														
            var streamWriter = new StreamWriter(inputStream);
            
            //inputStream.Position = inputStream.property("Length").str().toInt();  
            inputStream.Position = (int)inputStream.Length; // the line above can also be this
            streamWriter.Write(text);    
            streamWriter.Flush(); 			
            inputStream.Position = 0; 			
            
            return httpContextBase;
        }
        
        public static string stream_Read(this HttpContextBase httpContextBase, Stream inputStream)
        {								
            var originalPosition = inputStream.Position;
            var streamReader = new StreamReader(inputStream);
            var requestData = streamReader.ReadToEnd();	
            inputStream.Position = originalPosition;
            return requestData;
        }
    }
}