using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;
using FluentSharp.CoreLib;

namespace FluentSharp.REPL
{
    
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
	        var ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
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

        /// <summary>
        /// Assign the return value to a dynamic object to access the data as it was a string dictionary
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static object json_Deserialize(this string jsonString)
        {
            return new JavaScriptSerializer().Deserialize<object>(jsonString);
        }
    }
}
