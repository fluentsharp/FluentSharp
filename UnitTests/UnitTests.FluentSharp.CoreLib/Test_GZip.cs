using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentSharp.CoreLib;
using NUnit.Framework;

namespace UnitTests.FluentSharp_CoreLib
{
    [TestFixture]
    public class Test_GZip
    {
        [Test(Description = "returns a byte array with a GZIP of the string provided")]
         public void gzip_Compress()
        {
            var value = "This is a test";            
            var bytes = value.gzip_Compress();
            Assert.IsNotNull(value);
            Assert.Greater(bytes.size(), value.size());

            //check error handing

            Assert.IsEmpty((null as string).gzip_Compress());
        }
        [Test(Description = "returns a byte[] array with deGZIPed bytes provided")]
         public void gzip_Decompress()
        {
            var value1 = "This is a test";            
            var bytes  = value1.gzip_Compress ();
            var value2 = bytes.gzip_Decompress().ascii();
            Assert.AreEqual(value1,value2);
        }
        [Test(Description = "returns a string array with deGZIPed bytes provided")]
         public void gzip_Decompress_toString()
        {
            var value1 = "This is a test";            
            var bytes  = value1.gzip_Compress ();
            var value2 = bytes.gzip_Decompress_toString();
            Assert.AreEqual(value1,value2);
        }
    }
}
