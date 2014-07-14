using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentSharp.CoreLib;
using FluentSharp.NUnit;
using NUnit.Framework;

namespace UnitTests.FluentSharp.CoreLib
{    
    public class Test_String : NUnitTests
    {
        [Test(Description = "Returns true if the provided string(s) were found in the provided target")]
        public void contains()
        {
            var target = "abc12345";
            assert_Is_True (target.contains("a"));
            assert_Is_True (target.contains("1"));
            assert_Is_True (target.contains("abc"));
            assert_Is_True (target.contains("123"));
            assert_Is_True (target.contains("abc12345"));

            assert_Is_False(target.contains("d"));
            assert_Is_False(target.contains("6"));
            assert_Is_False(target.contains("abcd"));
            assert_Is_False(target.contains("0123"));
            assert_Is_False(target.contains("abc123456"));

            assert_Is_True (target.contains("a","XXX"));
            assert_Is_True (target.contains("XXX","2"));
            assert_Is_True (target.contains("Y","XXX","123"));
            assert_Is_True (target.contains("XXX","Y","12345"));

            assert_Is_False(target.contains("Y","XXX"));
            assert_Is_False(target.contains("XXX","Y"));
            
            assert_Is_True (target.contains("1",null));
            assert_Is_True (target.contains(null,"1"));
            assert_Is_False(target.contains("XXX",null));
            assert_Is_False(target.contains(null,"XXX"));
            assert_Is_False(target.contains(null as string));            
            assert_Is_False(target.contains(null as string []));            
            
            assert_Is_False((null as string).contains("a"));
            assert_Is_False((null as string).contains("a","1"));
            assert_Is_False((null as string).contains(null as string));
            assert_Is_False((null as string).contains(null as string[]));        
            
            //test List<string>
            var testList = new List<string>();
            assert_Is_False(target.contains(testList));
            assert_Is_True (target.contains(testList.clear().add("a")));
            assert_Is_True (target.contains(testList.clear().add("1")));
            assert_Is_True (target.contains(testList.clear().add("abc")));
            assert_Is_True (target.contains(testList.clear().add("123")));
            assert_Is_False(target.contains(testList.clear().add("z")));
            assert_Is_False(target.contains(testList.clear().add("0")));
            assert_Is_False(target.contains(testList.clear().add("abcd")));
            assert_Is_False(target.contains(testList.clear().add("0123")));
            assert_Is_True (target.contains(testList.clear().add("z")   .add("1")));
            assert_Is_True (target.contains(testList.clear().add("a")   .add("0")));
            assert_Is_True (target.contains(testList.clear().add("abcd").add("1")));
            assert_Is_True (target.contains(testList.clear().add("a")   .add("0123")));            
            assert_Is_True (target.contains(testList.clear().add("a")   .add(null as string)));
            assert_Is_True (target.contains(testList.clear().add(null as string).add("1")));            
            assert_Is_False(target.contains(testList.clear().add(null as string)));
        }
        
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
