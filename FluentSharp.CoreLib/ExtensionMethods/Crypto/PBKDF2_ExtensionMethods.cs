using System;
using FluentSharp.CoreLib;

namespace FluentSharp.CoreLib
{
    public static class PBKDF2_ExtensionMethods
    {
        public static int DEFAULT_PBKDF2_INTERACTIONS = 5000; // 20000;
        public static int DEFAULT_PBKDF2_BYTES = 64;

        public static string hash_PBKDF2(this string password, Guid salt)
        {
            return password.hash_PBKDF2(salt.str());
        }

        public static string hash_PBKDF2(this string password, string salt)
        {
            return password.hash_PBKDF2(salt, DEFAULT_PBKDF2_INTERACTIONS, DEFAULT_PBKDF2_BYTES);
        }

        public static string hash_PBKDF2(this string password, string salt, int iterations, int howManyBytes)
        {
            var bytes = PBKDF2.GetBytes(password.asciiBytes(), salt.asciiBytes(), iterations, howManyBytes);
            return bytes.base64Encode();
        }

        public static int set_DEFAULT_PBKDF2_INTERACTIONS(this int newValue)
        {
            "DEFAULT_PBKDF2_INTERACTIONS set to: {0}".debug(newValue);
            return DEFAULT_PBKDF2_INTERACTIONS = newValue;
        }
    }
}