
using System;
using FluentSharp.CoreLib;
using NUnit.Framework;

namespace UnitTests.FluentSharp_CoreLib.ExtensionMethods
{
    [TestFixture]
    public class Test_Char
    {
        [Test(Description = "Returns an byte of the provided char")]
        public void @byte()
        {
            var char1 = 'a';
            var char2 = '1';
            var char3 = ')';
            var char4 = '%';
            var char5 = (char)0x10;
            var char6 = (char)0xFF;

            var byte1 = (byte) 'a';
            var byte2 = (byte) '1';
            var byte3 = (byte) ')';
            var byte4 = (byte) '%';
            var byte5 = (byte) 0x10;
            var byte6 = (byte) 0xFF;

            Assert.AreEqual(byte1, char1.@byte());
            Assert.AreEqual(byte2, char2.@byte());
            Assert.AreEqual(byte3, char3.@byte());
            Assert.AreEqual(byte4, char4.@byte());
            Assert.AreEqual(byte5, char5.@byte());
            Assert.AreEqual(byte6, char6.@byte());

            Assert.AreEqual(char1, char1.@byte().@char());
            Assert.AreEqual(char2, char2.@byte().@char());
            Assert.AreEqual(char3, char3.@byte().@char());
            Assert.AreEqual(char4, char4.@byte().@char());
            Assert.AreEqual(char5, char5.@byte().@char());
            Assert.AreEqual(char6, char6.@byte().@char());            
        }

        [Test(Description = "Returns a byte array from a char array")]
        public void bytes()
        {
            var chars = 256.randomString().chars();
            var bytes = chars.bytes();

            Assert.AreEqual(chars.size(), bytes.size());
            for (var i = 0; i < chars.size(); i++)
            {
                Assert.AreEqual(chars[i]        , bytes[i]);
                Assert.AreEqual(chars[i]        , bytes[i].@char());
                Assert.AreEqual(chars[i].@byte(), bytes[i]);
            }
        }

        [Test(Description = "Returns an ascii string from a char array")]
        public void ascii()
        {
            var value = "this is a string with some random chars: {0}".format(200.randomString());
            var chars = value.chars();
            Assert.AreEqual(value, chars.ascii());
        }   

        [Test(Description = "Returns an ascii byte array from a char array")]
        public void ascii_Bytes()
        {
            var value = "this is a string with some random chars: {0}".format(200.randomString());
            var chars = value.chars();
            var bytes = chars.ascii_Bytes();
            var asciiValue = "";
            for (var i = 0; i < value.size(); i++)
            {
                Assert.AreEqual(value[i], chars[i]);
                Assert.AreEqual(value[i], bytes[i]);
                asciiValue += bytes[i].ascii();
            }
            Assert.AreEqual(asciiValue, value);
        }

        [Test(Description = "Returns a string with n chars repeated")]
        public void repeat()
        {
            var char1 = 'a';
            var char2 = '(';
            var char3 = (char) 0x10;
            var string1 = "aaaaa";
            var string2 = "((((((((((";
            var string3 = new String(new[] { (char)0x10, (char)0x10, (char)0x10, (char)0x10, (char)0x10, (char)0x10, (char)0x10});
            Assert.AreEqual(char1.repeat(5) , string1);
            Assert.AreEqual(char2.repeat(10), string2);
            Assert.AreEqual(char3.repeat(7) , string3);            
        }       
    }
}
