using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using O2.DotNetWrappers.ExtensionMethods;
using System.Security.Cryptography;
using System.IO;

namespace O2.DotNetWrappers.ExtensionMethods
{
    public static class Crypto_ExtensionMethods
    {
        public static Random randomObject = new Random((int)DateTime.Now.Ticks);
         
        public static int random(this int maxValue)
        {
            return randomObject.Next(maxValue);
        }

        // inspired from the accepted answer from http://stackoverflow.com/questions/1122483/c-random-string-generator
        public static string randomString(this int size)
        {
            var random = Crypto_ExtensionMethods.randomObject;

            var stringBuilder = new StringBuilder();
            for (int i = 0; i < size; i++)
            {
                var intValue = Convert.ToInt32(Math.Floor(93 * random.NextDouble() + 33));  // gets a ASCII value from 33 till 126		        	
                var ch = Convert.ToChar(intValue);
                stringBuilder.Append(ch);
            }
            return stringBuilder.ToString();
        }

        public static string randomNumbers(this int size)
        {
            var random = Crypto_ExtensionMethods.randomObject;

            var stringBuilder = new StringBuilder();
            for (int i = 0; i < size; i++)
            {
                var intValue = Convert.ToInt32(Math.Floor(10 * random.NextDouble() + 48));  // gets a ASCII value from 33 till 126		        	
                var ch = Convert.ToChar(intValue);
                stringBuilder.Append(ch);
            }
            return stringBuilder.ToString();
        }



        public static string randomLetters(this int size)
        {
            var random = Crypto_ExtensionMethods.randomObject;

            var stringBuilder = new StringBuilder();
            for (int i = 0; i < size; i++)
            {
                var intValue = Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65));  // gets a ASCII value from 33 till 126
                if (1000.random().isEven())			// if it is an even number
                    intValue += 32;				 	// make it lower case
                var ch = Convert.ToChar(intValue);
                stringBuilder.Append(ch);
            }
            return stringBuilder.ToString();
        }

        #region MD5 String
        public static string md5Hash(this string stringToHash)
        {
            if (stringToHash.isNull())
                return "";

            var md5 = MD5.Create();

            var data = md5.ComputeHash(stringToHash.asciiBytes());
            return data.hexString();            
        }

        public static string hexString(this byte[] bytes)
        {
            if (bytes.isNull())
                return "";
            var stringBuilder = new StringBuilder();
            
            for (int i = 0; i < bytes.Length; i++)            
                stringBuilder.Append(bytes[i].ToString("x2"));                        
            return stringBuilder.ToString();
        }
        #endregion

        #region MD5 Bitmap
        public static string md5Hash(this Bitmap bitmap)
        {
            try
            {
                if (bitmap.isNull())
                    return null;
                //based on code snippets from http://dotnet.itags.org/dotnet-c-sharp/85838/
                using (MemoryStream strm = new MemoryStream())
                {
                    var image = new Bitmap(bitmap);
                    bitmap.Save(strm, System.Drawing.Imaging.ImageFormat.Bmp);
                    strm.Seek(0, 0);
                    byte[] bytes = strm.ToArray();
                    MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                    byte[] hashed = md5.TransformFinalBlock(bytes, 0, bytes.Length);
                    string hash = BitConverter.ToString(hashed).ToLower();
                    md5.Clear();
                    image.Dispose();
                    return hash;
                }
            }
            catch (Exception ex)
            {
                ex.log("in bitmap.md5Hash");
                return "";
            }
        }
        public static bool isNotEqualTo(this Bitmap bitmap1, Bitmap bitmap2)
        {
            return bitmap1.isEqualTo(bitmap2).isFalse();
        }
        public static bool isEqualTo(this Bitmap bitmap1, Bitmap bitmap2)
        {
            var md5Hash1 = bitmap1.md5Hash();
            var md5Hash2 = bitmap2.md5Hash();
            if (md5Hash1.valid() && md5Hash2.valid())
                return md5Hash1 == md5Hash2;
            else
                "in Bitmap.isEqualTo at least one of the calculated MD5 Hashes was not valid".error();
            return false;
        }
        #endregion
    }
}
