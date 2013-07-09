using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentSharp.CoreLib;
using NUnit.Framework;

namespace UnitTests.FluentSharp_CoreLib.ExtensionMethods
{
    [TestFixture]
    public class Test_String
    {
        [Test(Description = "Returns a string after the provided string")]
        public void subString_After()
        {
            var value = "123456";
            var test1 = "1";
            var test2 = "2";
            var test3 = "5";
            var test4 = "6";
            var test5 = "0";
            var test6 = "";            
            var expected1 = "23456";
            var expected2 = "3456";
            var expected3 = "6";
            var expected4 = "";
            var expected5 = "";
            var expected6 = "123456";            

            Assert.AreEqual(value.subString_After(test1), expected1);
            Assert.AreEqual(value.subString_After(test2), expected2);
            Assert.AreEqual(value.subString_After(test3), expected3);
            Assert.AreEqual(value.subString_After(test4), expected4);
            Assert.AreEqual(value.subString_After(test5), expected5);
            Assert.AreEqual(value.subString_After(test6), expected6);

            Assert.AreEqual("".subString_After(test1), "");            
            Assert.AreEqual((null as string).subString_After(""), "");
            Assert.AreEqual(value.subString_After((null as string)), "");
        }

    }
}
