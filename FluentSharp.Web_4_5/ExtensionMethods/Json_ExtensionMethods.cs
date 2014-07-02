using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using FluentSharp.CoreLib;

namespace FluentSharp.Web
{
    public static class Json_ExtensionMethods
    {
                public static string json<T>(this T target)
		{
			return target.json_Serialize();
		}
		public static string json_Serialize<T>(this T target)
		{
            try
            {
			    var dataContractJsonSerializer = new DataContractJsonSerializer(target.type());
			    var memoryStream               = new MemoryStream();
			    dataContractJsonSerializer.WriteObject(memoryStream, target);
			    var json                       = Encoding.Default.GetString(memoryStream.ToArray());
			    memoryStream.Dispose();
			    return json;
            }
            catch(Exception ex)
            {
                ex.log("[json_Serialize]");
                return null;
            }
		}
        public static object json_Deserialize(this string json)
		{
            try
            {
			    return new JavaScriptSerializer().Deserialize<object>(json);
            }
            catch(Exception ex)
            {
                ex.log("[json_Deserialize]");
                return null;
            }
		}
		public static T json_Deserialize<T>(this string json)
		{
            try
            {
			    var memoryStream               = new MemoryStream(Encoding.Unicode.GetBytes(json));
			    var dataContractJsonSerializer = new DataContractJsonSerializer(typeof(T));
			    var result = (T)dataContractJsonSerializer.ReadObject(memoryStream);
			    memoryStream.Close();
			    memoryStream.Dispose();
			    return result;
            }
            catch(Exception ex)
            {
                ex.log("[json_Deserialize]");
                return default(T);
            }
		}
        
		public static string javascript_Serialize<T>(this T target)
		{
            try
            {
			    return new JavaScriptSerializer().Serialize(target);
            }
            catch(Exception ex)
            {
                ex.log("[javascript_Serialize]");
                return null;
            }
		}
        public static object javascript_Deserialize(this string json)
        {
            return javascript_Deserialize<object>(json);
        }
		public static T javascript_Deserialize<T>(this string json)
		{
            try
            {
			    return new JavaScriptSerializer().Deserialize<T>(json);
            }
            catch(Exception ex)
            {
                ex.log("[javascript_Deserialize]");
                return default(T);
            }
		}	
    }
}
