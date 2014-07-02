using System.Security.Cryptography;
using NUnit.Framework;
using FluentSharp.CoreLib;

namespace TeamMentor.UnitTests.CoreLib
{
    [TestFixture]
    class Test_Crypto
    {
        [Test]
        public void EncryptAndDecrypt()
        {       
            var aes = Rijndael.Create();
            aes.GenerateIV();
            var key = aes.Key;
            var iv = aes.IV;
            var originalText = "the quick brown fox jumped over the lazy dog";
            var encryptedText  = EncyptDecrypt.EncryptString(originalText, key, iv);
            var decryptedText = EncyptDecrypt.DecryptString(encryptedText, key, iv);
            "key: {0}".info(key.base64Encode());
            "iv: {0}".info(iv.base64Encode());
            "original: {0}"      .info(originalText);
            "encryptedText: {0}" .info(encryptedText);
            "descryptedText: {0}".info(decryptedText);
            Assert.AreNotEqual(originalText,encryptedText);
            Assert.AreNotEqual(originalText,encryptedText.base64Decode());
            Assert.AreNotEqual(encryptedText.base64Decode(),decryptedText);
            Assert.AreEqual (originalText, decryptedText);
        }
    }
}
