using System.Security.Cryptography;
using System.Text;

namespace FluentSharp.CoreLib
{
    public static class SHA256_ExtensionMethods
    {
        public static string hash_SHA256(this string text, string salt)
        {
            var stringToHash = text + salt;
            var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(stringToHash.asciiBytes());
            var hashString = new StringBuilder();
            foreach (byte b in hashBytes)
                hashString.Append(b.ToString("x2"));
            return hashString.str();
        }
    }
}
