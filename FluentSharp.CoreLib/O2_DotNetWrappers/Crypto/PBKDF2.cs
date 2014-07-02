//Code based on example from: Password Minder Internals http://msdn.microsoft.com/en-us/magazine/cc163913.aspx
using System;
using System.Security.Cryptography;
using System.Text;

// ReSharper disable InconsistentNaming
// ReSharper disable SuggestUseVarKeywordEvident
namespace FluentSharp.CoreLib
{
    // implementation of PKCS#5 v2.0
    // Password Based Key Derivation Function 2
    // http://www.rsasecurity.com/rsalabs/pkcs/pkcs-5/index.html
    // For the HMAC function, see RFC 2104
    // http://www.ietf.org/rfc/rfc2104.txt
    class PBKDF2
    {
        // SHA-256 has a 512-bit block size and gives a 256-bit output
        const int BLOCK_SIZE_IN_BYTES = 64;
        const int  HASH_SIZE_IN_BYTES = 32; 

        const byte IPAD = 0x36;
        const byte OPAD = 0x5C;

        public static byte[] GetBytes(string password, byte[] salt, int iterations, int howManyBytes)
        {
            return GetBytes(
                Encoding.UTF8.GetBytes(password),
                salt, iterations, howManyBytes);
        }
        public static byte[] GetBytes(byte[] password, byte[] salt, int iterations, int howManyBytes)
        {
            // round up

            uint cBlocks = (uint)((howManyBytes+ HASH_SIZE_IN_BYTES-1)/HASH_SIZE_IN_BYTES);

            // seed for the pseudo-random fcn: salt + block index
            byte[] saltAndIndex = new byte[salt.Length + 4];
            Array.Copy(salt, 0, saltAndIndex, 0, salt.Length);

            byte[] output = new byte[cBlocks*HASH_SIZE_IN_BYTES];
            int outputOffset = 0;

            SHA256Managed innerHash = new SHA256Managed();
            SHA256Managed outerHash = new SHA256Managed();

            // HMAC says the key must be hashed or padded with zeros
            // so it fits into a single block of the hash in use
            if (password.Length > BLOCK_SIZE_IN_BYTES) {
                password = innerHash.ComputeHash(password);
            }
            byte[] key = new byte[BLOCK_SIZE_IN_BYTES];
            Array.Copy(password, 0, key, 0, password.Length);

            byte[] InnerKey = new byte[BLOCK_SIZE_IN_BYTES];
            byte[] OuterKey = new byte[BLOCK_SIZE_IN_BYTES];
            for (int i = 0; i < BLOCK_SIZE_IN_BYTES; ++i) {
                InnerKey[i] = (byte)(key[i] ^ IPAD);
                OuterKey[i] = (byte)(key[i] ^ OPAD);
            }

            // for each block of desired output
            for (int iBlock = 0; iBlock < cBlocks; ++iBlock) {
                // seed HMAC with salt & block index
                _incrementBigEndianIndex(saltAndIndex, salt.Length);
                byte[] U = saltAndIndex;

                for (int i = 0; i < iterations; ++i) {
                    // simple implementation of HMAC-SHA-256
                    innerHash.Initialize();
                    innerHash.TransformBlock(InnerKey, 0,
                        BLOCK_SIZE_IN_BYTES, InnerKey, 0);
                    innerHash.TransformFinalBlock(U, 0, U.Length);

                    byte[] temp = innerHash.Hash;

                    outerHash.Initialize();
                    outerHash.TransformBlock(OuterKey, 0,
                        BLOCK_SIZE_IN_BYTES, OuterKey, 0);
                    outerHash.TransformFinalBlock(temp, 0, temp.Length);

                    U = outerHash.Hash; // U = result of HMAC

                    // xor result into output buffer
                    _xorByteArray(U, 0, HASH_SIZE_IN_BYTES,
                        output, outputOffset);
                }
                outputOffset += HASH_SIZE_IN_BYTES;
            }
            byte[] result = new byte[howManyBytes];
            Array.Copy(output, 0, result, 0, howManyBytes);
            return result;
        }        
        static void _incrementBigEndianIndex(byte[] buf, int offset)
        {
            // treat the four bytes starting at buf[offset]
            // as a big endian integer, and increment it
            unchecked {
                if (0 == ++buf[offset+3])
                if (0 == ++buf[offset+2])
                if (0 == ++buf[offset+1])
                if (0 == ++buf[offset+0])
                    throw new OverflowException();
            }
        }
        static void _xorByteArray(byte[] src,int srcOffset, int cb, byte[] dest, int destOffset)
        {
            int end = checked(srcOffset + cb);
            while (srcOffset != end) {
                dest[destOffset] ^= src[srcOffset];
                ++srcOffset;
                ++destOffset;
            }
        }
    }    
}
// ReSharper restore InconsistentNaming
// ReSharper restore SuggestUseVarKeywordEvident