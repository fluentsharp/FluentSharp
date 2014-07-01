using System;
using System.Collections.Generic;
using System.Text;

namespace FluentSharp.CoreLib
{
    public static class Encoding_ExtensionMethods
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
}