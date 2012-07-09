﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using O2.DotNetWrappers.ExtensionMethods;
using System.Runtime.Serialization.Json;
using System.IO.Compression;
using System.Web.Script.Serialization;

namespace O2.DotNetWrappers.ExtensionMethods
{

    public static class GZip_ExtensionMethods
	{	
		public static byte[] gzip_Compress(this string _string)
		{
			var bytes = Encoding.ASCII.GetBytes (_string);
			var outputStream = new MemoryStream();
			using (var gzipStream = new GZipStream(outputStream,CompressionMode.Compress))			
				gzipStream.Write(bytes, 0, bytes.size());			
			return outputStream.ToArray();
		}
		
		public static string gzip_Decompress(this byte[] bytes)
		{
			var inputStream = new MemoryStream();
			inputStream.Write(bytes, 0, bytes.Length);
			inputStream.Position = 0;
			var outputStream = new MemoryStream();
			using (var gzipStream= new GZipStream(inputStream,CompressionMode.Decompress))
			{
			    byte[] buffer = new byte[4096];
			    int numRead;
			    while ((numRead = gzipStream.Read(buffer, 0, buffer.Length)) != 0)			    
			        outputStream.Write(buffer, 0, numRead);			    
			}	
			return outputStream.ToArray().ascii();
		}
	}

    public static class Json_ExtensionMethods
	{
		public static string json<T>(this T _object)
		{
			return _object.json_Serialize();
		}
		
		public static string json_Serialize<T>(this T _object)
	    {
	        var serializer = new DataContractJsonSerializer(_object.type());
	        var memoryStream = new MemoryStream();
	        serializer.WriteObject(memoryStream, _object);
	        var serializedObject = Encoding.Default.GetString(memoryStream.ToArray());
	        memoryStream.Dispose();
	        return serializedObject;
	    }

	    public static T json_Deserialize<T>(this string json)
	    {
	        //T obj = Activator.CreateInstance<T>();
	        MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
	        var serializer = new DataContractJsonSerializer(typeof(T));
	        var obj = (T)serializer.ReadObject(ms);
	        ms.Close();
	        ms.Dispose();
	        return obj;
	    }
	    
	    public static string javascript_Serialize<T>(this T _object)
	    {
	        return new JavaScriptSerializer().Serialize(_object);
	    }

	    public static T javascript_Deserialize<T>(this string json)
	    {
	        //T obj = Activator.CreateInstance<T>();
	        return new JavaScriptSerializer().Deserialize<T>(json);	        
	    }
    }
}