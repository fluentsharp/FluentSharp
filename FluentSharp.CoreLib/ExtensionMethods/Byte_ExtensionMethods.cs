using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FluentSharp.CoreLib
{
    public static class Byte_ExtensionMethods
    {
        public static string        ascii(this byte value)
        {            
            return Encoding.ASCII.GetString(new[] { value });
        }        
        public static string        ascii(this byte[] value)
        {
            if (value.isNull())
                return null;
            value = value.fix_BOM();
            return Encoding.ASCII.GetString(value);
        }
        public static byte[]        bytes_Ascii(this string value)
        {            
            return Encoding.ASCII.GetBytes(value);
        }
        public static byte[]        bytes_From_HexString(this string hexString)
        {

            try
            {
                if (hexString.isNotNull())
                    if (hexString.size().mod(2) != 0)
                        "[bytes_From_HexString] size of provided string must be a multiple of 2, but it was {0}".error(
                            hexString.size());
                    else
                    {
                        var bytes = new List<byte>();
                        for (var i = 0; i < hexString.size(); i += 2)
                        {
                            var hex = hexString.subString(i, 2);
                            bytes.add(byte.Parse(hex, NumberStyles.HexNumber));
                        }
                        return bytes.toArray();
                    }
            }
            catch (Exception  ex)
            {
                ex.log("[string][bytes_From_HexString]");
            }
            return new byte[0];
        }        
        public static byte[]        bytes_Unicode(this string value)
        {            
            return Encoding.Unicode.GetBytes(value);
        }
        public static char          @char(this byte @byte)
        {
            return (char)@byte;            
        }
        public static byte[]        fix_BOM  (this byte[] bytesToFix)
        {
            //see BOM - Byte Order Mark : http://en.wikipedia.org/wiki/Byte_order_mark
            //    http://www.verious.com/qa/-239-187-191-characters-appended-to-the-beginning-of-each-file/
            //    http://social.msdn.microsoft.com/Forums/en-US/8956758d-9814-4bd4-9812-e82903640b2f/recieving-239187191-character-symbols-when-loading-text-files-not-containing-them
            if (bytesToFix.notNull())
                if(bytesToFix.size() > 3)
                    if(bytesToFix[0] == '\xEF' && bytesToFix[1] == '\xBB' && bytesToFix[2] == '\xBF')
                        return bytesToFix.remove_Bytes(2);                    
            return bytesToFix;
        }
        public static string        hex(this byte value)
        {
            return value.ToString("x2").caps();
            //return Convert.ToString(value, 16).caps();
        }
        public static string        hexString(this byte[] bytes)
        {
            if (bytes.isNull())
                return "";
            var stringBuilder = new StringBuilder();            
            for (int i = 0; i < bytes.Length; i++)            
                stringBuilder.Append(bytes[i].hex());                        
            return stringBuilder.str();
        }        
        
        public static byte[]        insert_Bytes(this byte[] originalBytes, params char[] bytesToInsert)
        {
            return originalBytes.insert_Bytes(bytesToInsert.bytes());
        }
        public static byte[]        insert_Bytes(this byte[] originalBytes, params byte[] bytesToInsert)
        {
         try
            {
                var bytes = new byte[originalBytes.Length + bytesToInsert.Length];
                originalBytes.CopyTo(bytes, bytesToInsert.Length);
                bytesToInsert.CopyTo(bytes, 0);
                return bytes;
            }
            catch(Exception ex)
            {
                ex.log("[byte][add_Bytes]");
                return null;
            }   
        }           
        public static byte[]        remove_Bytes(this byte[] originalBytes, uint removeFrom)
        {
         try
            {
                if (originalBytes.size() > removeFrom)
                {
                    var newSize = originalBytes.Length - removeFrom -1;
                    var bytes = new byte[newSize];
                    Array.Copy(originalBytes, removeFrom + 1 , bytes, 0,  newSize);                    
                    return bytes;
                }
            }
            catch(Exception ex)
            {
                ex.log("[byte][add_Bytes]");                
            }   
            return new byte[0];
        }        
        public static List<string>  strings_From_Bytes(this byte[] bytes)
        {
            return bytes.strings_From_Bytes(true, 2);
        }
        public static List<string>  strings_From_Bytes(this byte[] bytes, bool ignoreCharZeroAfterValidChar, int minimumStringSize)
        {
            //this method is only really good to find ASCII binary strings
            var extractedStrings = new List<string>();
            var stringBuilder = new StringBuilder();
            Action addString =
                ()=>{
                        if (stringBuilder.Length > 0)
                        {
                            if (minimumStringSize == -1 || stringBuilder.Length > minimumStringSize)
                                extractedStrings.Add(stringBuilder.ToString());
                            stringBuilder = new StringBuilder();
                        }
                    };
            for (int i = 0; i < bytes.Length ; i++)
            {
                var value = bytes[i];
                if (value > 31 && value < 127) // see http://www.asciitable.com/
                {
                    var str = value.ascii();
                    stringBuilder.Append(str);
                    if (ignoreCharZeroAfterValidChar && i + 1 < bytes.Length)
                        if (bytes[i + 1] == 0)
                            i++;
                }
                else
                    addString();
            }
            addString();
            return extractedStrings;
        }
        public static string        unicode(this byte[] value)
        {
            if (value.isNull())
                return null;
            return Encoding.Unicode.GetString(value);
        }                     
    }
}