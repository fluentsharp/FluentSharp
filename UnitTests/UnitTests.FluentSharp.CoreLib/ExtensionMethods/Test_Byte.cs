using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentSharp.CoreLib;
using NUnit.Framework;

namespace UnitTests.FluentSharp_CoreLib.ExtensionMethods
{
    [TestFixture]
    public class Test_Byte
    {
        [Test(Description = "Returns an ascii string from a byte of bytes")]
        public void ascii()
        {
            //ascii(this byte value)
            var value0    = (byte)00;
            var value1    = (byte)10;
            var value2    = (byte)13;
            var value3    = (byte)45;
            var value4    = (byte)60;
            var value5    = (byte)100;
            var value6    = (byte)255;
            var expected0 = "\0";
            var expected1 = "\n";
            var expected2 = "\r";
            var expected3 = "-";
            var expected4 = "<";
            var expected5 = "d";
            var expected6 = "?";

            Assert.AreEqual(value0.ascii(), expected0);
            Assert.AreEqual(value1.ascii(), expected1);
            Assert.AreEqual(value2.ascii(), expected2);
            Assert.AreEqual(value3.ascii(), expected3);
            Assert.AreEqual(value4.ascii(), expected4);
            Assert.AreEqual(value5.ascii(), expected5);
            Assert.AreEqual(value6.ascii(), expected6);

            //ascii(this byte[] value)

            var value7    = new byte[] {0,10,13,45,60,100,255};
            var expected7 = "\0\n\r-<d?";
            Assert.AreEqual(value7.ascii(), expected7);

            Assert.IsNull((null as byte[]).ascii());

            //byte with BOM
            var value8 = "a string value";
            var value9 = "a string value".bytes_Ascii().insert_Bytes('\xEF','\xBB','\xBF');
            
            Assert.AreEqual(value8, value8.bytes_Ascii().ascii());
            Assert.AreEqual(value8, value9.ascii());
        }

        [Test(Description = "Returns the ascii bytes from an string")]
        public void bytes_Ascii()
        {
            var string_En = 20.randomString();
            var bytes_En = string_En.bytes_Ascii();
            var ascii_En = bytes_En.ascii();
            
            Assert.AreEqual(ascii_En, ascii_En);

            //the convertion of Japanese chars only works for the Unicode methods (see below)
            var string_JP   = "適用されたフィルター";
            var bytes_JP    = string_JP.bytes_Ascii();
            var ascii_JP    = bytes_JP.unicode();
            var expected_JP = "㼿㼿㼿㼿㼿";         

            Assert.AreNotEqual(ascii_JP, string_JP);
            Assert.AreEqual   (ascii_JP, expected_JP);

            Assert.AreEqual   (ascii_JP[0], '㼿');
            Assert.AreEqual   (ascii_JP[0], 16191);
            Assert.AreEqual   (ascii_JP[1], 16191);
            Assert.AreEqual   (ascii_JP[2], 16191);
            Assert.AreEqual   (ascii_JP[3], 16191);
            Assert.AreEqual   (ascii_JP[4], 16191);
            Assert.Throws<IndexOutOfRangeException>(()=> ascii_JP[5].str());
            Assert.AreEqual   (string_JP[0], '適');
            Assert.AreEqual   (string_JP[1], '用');
            Assert.AreNotEqual(string_JP[1], '適');
            Assert.AreEqual   (string_JP[9], 'ー');
            Assert.AreEqual   (string_JP[0], 36969);
            Assert.AreEqual   (string_JP[1], 29992);
            Assert.AreEqual   (string_JP[9], 12540);
            Assert.AreEqual   (string_JP[9], 12540);
            
        }

        [Test(Description = "Returns the unicode bytes from an string")]
        public void bytes_Unicode()
        {
            var string_En    = 20.randomString() + "А б в г д е ё ж з и й к";
            var bytes_En     = string_En.bytes_Unicode();
            var ascii_En     = bytes_En.ascii();
            var unicode_En   = bytes_En.unicode();

            Assert.AreNotEqual(string_En, ascii_En);
            Assert.AreEqual(string_En, unicode_En);

            var string_JP   = "適用されたフィルター";
            var bytes_JP    = string_JP.bytes_Unicode();
            var unicode_JP  = bytes_JP.unicode();
            var ascii_JP    = bytes_En.ascii();            

            Assert.AreNotEqual(ascii_JP  , string_JP);
            Assert.AreEqual   (unicode_JP, string_JP);

            Assert.AreEqual   (unicode_JP[0], '適');
            Assert.AreEqual   (unicode_JP[1], '用');
            Assert.AreNotEqual(unicode_JP[1], '適');
            Assert.AreEqual   (unicode_JP[9], 'ー');
            Assert.AreEqual   (unicode_JP[0], 36969);
            Assert.AreEqual   (unicode_JP[1], 29992);
            Assert.AreEqual   (unicode_JP[9], 12540);
            Assert.Throws<IndexOutOfRangeException>(()=> unicode_JP[10].str());
        }

        [Test(Description = "Returns a Hex string of a byte (with two chars per byte)")]
        public void hex()
        {
            var value0    = (byte)00;
            var value1    = (byte)10;
            var value2    = (byte)13;
            var value3    = (byte)45;
            var value4    = (byte)60;
            var value5    = (byte)100;
            var value6    = (byte)255;
            var expected0 = "00";
            var expected1 = "0A";
            var expected2 = "0D";
            var expected3 = "2D";
            var expected4 = "3C";
            var expected5 = "64";
            var expected6 = "FF";

            Assert.AreEqual(value0.hex(), expected0);
            Assert.AreEqual(value1.hex(), expected1);
            Assert.AreEqual(value2.hex(), expected2);
            Assert.AreEqual(value3.hex(), expected3);
            Assert.AreEqual(value4.hex(), expected4);
            Assert.AreEqual(value5.hex(), expected5);
            Assert.AreEqual(value6.hex(), expected6);
        }

        [Test(Description = "Returns a Hex string of a byte array (with two chars per byte)")]
        public void hexString()
        {
            var value = new byte[] {0, 10, 13, 45, 60, 100, 255};
            var expected = "000A0D2D3C64FF";
            Assert.AreEqual(value.hexString(), expected);

            Assert.AreEqual("", (null as byte[]).hexString());
        }

        [Test(Description = "Inserts one byte[] into another byte[], the result is a byte[] with  bytesToInsert[] + originalBytes[]")]
        public void insert_Bytes()
        {
            var originalBytes = "originalBytes".asciiBytes();
            var bytesToInsert = "bytesToInsert".asciiBytes();
            var bytes1         = "bytesToInsertoriginalBytes".asciiBytes();
            var bytes2         = bytesToInsert.insert_Bytes('\x41','\x42','\x43','\x44'); 
            var expected1      = originalBytes.insert_Bytes(bytesToInsert);
            var expected2      = "ABCDbytesToInsert".asciiBytes(); 

            Assert.AreEqual(bytes1, expected1);
            Assert.AreEqual(bytes2.ascii(), expected2.ascii());
            Assert.AreEqual(bytes2, expected2);

            //test expception handing
            Assert.IsNull((null as byte[]).insert_Bytes(bytesToInsert));
            Assert.IsNull(originalBytes.insert_Bytes(null as byte[])); 
        }

        [Test(Description = "Returns the bytes from Hex formated string (with two chars per byte)")]
        public void bytes_From_HexString()
        {
            var value     = "000A0D2D3C64FF";
            var expected  = new byte[] {0, 10, 13, 45, 60, 100, 255};
            var result    = value.bytes_From_HexString();
            var roundTrip = result.hexString();

            Assert.IsNotEmpty(result);
            Assert.AreEqual(result, expected);
            Assert.AreEqual(value, roundTrip);



            //Check exception handing (and bad data)
            Assert.AreEqual(new byte[0], (null as string).bytes_From_HexString());
            Assert.IsEmpty("0"  .bytes_From_HexString());
            Assert.IsEmpty("012".bytes_From_HexString());
            Assert.IsEmpty("012ZZ".bytes_From_HexString());
            Assert.IsEmpty("*&^123".bytes_From_HexString());
            Assert.IsEmpty("*BADHEX".bytes_From_HexString());
            Assert.IsNotEmpty("0124".bytes_From_HexString());                        
        }

        [Test(Description = "Removes n bytes from an byte[]")]
        public void remove_Bytes()
        {
            var originalBytes = "1234".asciiBytes();
            var position1      = (uint)0; 
            var position2      = (uint)1; 
            var position3      = (uint)2; 
            var position4      = (uint)3; 
            var position5      = (uint)4; 
            var expected1      = "234".asciiBytes();
            var expected2      = "34" .asciiBytes();
            var expected3      = "4"  .asciiBytes();
            var expected4      = ""   .asciiBytes();
            var expected5      = new byte[0];

            Assert.AreEqual(originalBytes.remove_Bytes(position1).ascii(), expected1.ascii());
            Assert.AreEqual(originalBytes.remove_Bytes(position2).ascii(), expected2.ascii());
            Assert.AreEqual(originalBytes.remove_Bytes(position3).ascii(), expected3.ascii());
            Assert.AreEqual(originalBytes.remove_Bytes(position4).ascii(), expected4.ascii());
            Assert.AreEqual(originalBytes.remove_Bytes(position5).ascii(), expected5.ascii());

            Assert.AreEqual(originalBytes.remove_Bytes(position1)        , expected1);           
            Assert.AreEqual(originalBytes.remove_Bytes(position2)        , expected2);           
            Assert.AreEqual(originalBytes.remove_Bytes(position3)        , expected3);           
            Assert.AreEqual(originalBytes.remove_Bytes(position4)        , expected4);           
            Assert.AreEqual(originalBytes.remove_Bytes(position5)        , expected5);           

            //test expception handing
            Assert.IsEmpty((null as byte[]).remove_Bytes(0));            
        }

        [Test(Description = "Returns an unicode string from unicode bytes")]
        public void unicode()
        {
            //see bytes_Unicode()

            Assert.IsNull((null as byte[]).unicode());
        }

        [Test(Description = "Returns the strings found in a byte array (a string are bytes between 30 and 127")]
        public void strings_From_Bytes()
        {
            var testString      = "aaa\0" + 20.randomChars() + "bbb";
            var bytes_Ascii     = testString.bytes_Ascii();
            var bytes_Unicode   = testString.bytes_Unicode();
            var strings_Ascii   = bytes_Ascii.strings_From_Bytes();
            var strings_Unicode = bytes_Unicode.strings_From_Bytes();

            Assert.IsNotEmpty(bytes_Ascii);
            Assert.IsNotEmpty(bytes_Unicode);
            Assert.AreEqual(bytes_Ascii.size(),27);
            Assert.AreEqual(bytes_Unicode.size(),54);

            Assert.IsNotEmpty(strings_Ascii);
            Assert.IsNotEmpty(strings_Unicode);            
            Assert.Less(1, strings_Ascii.size());
            Assert.Less(1, strings_Unicode.size() , "For value:" + strings_Unicode);            

            Assert.IsTrue(strings_Ascii.first().contains("aaa"));
            Assert.IsTrue(strings_Ascii.last().error().contains("bbb"));
            Assert.IsTrue(strings_Unicode.first().contains("aaa"));
            Assert.IsTrue(strings_Unicode.last().contains("bbb"));

            Assert.IsEmpty(bytes_Unicode.strings_From_Bytes(false, 2));
        }

        
    }
}
