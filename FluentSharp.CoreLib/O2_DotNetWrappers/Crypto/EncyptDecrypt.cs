using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace FluentSharp.CoreLib
{
    public class EncyptDecrypt
    {
        
        //based on code sample from https://teammentor.net/article/244d7faf-c64e-4df8-88e1-1d72228392bf (Decrypt a String via a Block Cipher Using AES)
        public static string DecryptString(string ciphertext, byte[] sKey, byte[] sIV)
        {
            // The default AES key size under the .NET framework is 256.  The following
            // call will create an AES crypto provider and create a random initialization
            // vector and key. The crypto mode defaults to CBC ensuring the proper chaining
            // of data to mitigate repetition of cipher text blocks.
            var rijndaelAlg = Rijndael.Create();            //Set secret key For AES algorithm.
            rijndaelAlg.Key = sKey;                         //Set initialization vector.
            rijndaelAlg.IV = sIV;
            //Create a memorystream to which we'll decrypt our input string
            var ms = new MemoryStream();
            var ecs = new CryptoStream(ms, rijndaelAlg.CreateDecryptor(), CryptoStreamMode.Write);
            //Because the input string is passed in as a Base64 encoded value we decode prior writing to
            //the decryptor stream.
            ecs.Write(Convert.FromBase64String(ciphertext), 0, Convert.FromBase64String(ciphertext).Length);
            ecs.Close();
            return Encoding.ASCII.GetString(ms.ToArray());
        }
        
        public static string EncryptString(string ciphertext, byte[] sKey, byte[] sIV)
        {			
            var rijndaelAlg = Rijndael.Create();    //Set secret key For AES algorithm.
            rijndaelAlg.Key = sKey;                 //Set initialization vector.
            rijndaelAlg.IV = sIV;                   //Create a memorystream to which we'll decrypt our input string
            var ms = new MemoryStream();
            var ecs = new CryptoStream(ms, rijndaelAlg.CreateEncryptor(), CryptoStreamMode.Write);            
            ecs.Write(Encoding.ASCII.GetBytes(ciphertext), 0, Encoding.ASCII.GetBytes(ciphertext).Length);
            ecs.Close();
            return Convert.ToBase64String(ms.ToArray());
        }

    }
}