using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentSharp.CoreLib;
using FluentSharp.NUnit;
using NUnit.Framework;

namespace UnitTests.FluentSharp.CoreLib
{    
    public class Test_String_ExtensionMethods : NUnitTests
    {
        [Test] public void contains()
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
        [Test] public void equal()
        {
            "a"  .equal("a"  ).assert_True ();            
            "1"  .equal("1"  ).assert_True ();
            "abc".equal("abc").assert_True ();
            "a"  .equal("1"  ).assert_False();            
            "1"  .equal("a"  ).assert_False();
            "abc".equal("1bc").assert_False();
            
            (null as string).equal(null) .assert_True();
            (null as string).equal("123").assert_False();
            "123"           .equal(null ).assert_False();
        }
        [Test] public void equals()
        {
            "a"  .equals("a"  ).assert_True ();            
            "1"  .equals("1"  ).assert_True ();
            "abc".equals("abc").assert_True ();
            "a"  .equals("1"  ).assert_False();            
            "1"  .equals("a"  ).assert_False();
            "abc".equals("1bc").assert_False();
            "a"  .equals("a"  , "b"  ).assert_True ();            
            "a"  .equals("b"  , "a"  ).assert_True ();            
            "1"  .equals("1"  , "1"  ).assert_True ();
            "1"  .equals("2"  , "1"  ).assert_True ();
            "abc".equals("abc", "123").assert_True ();
            "abc".equals("123", "abc").assert_True();
            "a"  .equals("1"  , "2"  ).assert_False();            
            "1"  .equals("a"  , "b"  ).assert_False();
            "abc".equals("1bc", "2bc").assert_False();

            "abc".equals(null , "2bc").assert_False();
            "abc".equals("1bc", null ).assert_False();
            "abc".equals(null , "abc").assert_True ();
            "abc".equals("abc", null ).assert_True ();
            "abc".equals( null       ).assert_False();

            (null as string).equals("1bc", "2bc").assert_False();
            (null as string).equals(null, "2bc" ).assert_True();
            (null as string).equals(null        ).assert_True();
        } 
        [Test] public void not_Equal()
        {
            "a"  .not_Equal("a"  ).assert_False();            
            "1"  .not_Equal("1"  ).assert_False();
            "abc".not_Equal("abc").assert_False();
            "a"  .not_Equal("1"  ).assert_True ();            
            "1"  .not_Equal("a"  ).assert_True ();
            "abc".not_Equal("1bc").assert_True ();

            (null as string).not_Equal(null) .assert_False();
            (null as string).not_Equal("123").assert_True();
            "123"           .not_Equal(null ).assert_True();
        }
        [Test] public void not_Equals()
        {
            "a"  .not_Equals("a"  ).assert_False();            
            "1"  .not_Equals("1"  ).assert_False();
            "abc".not_Equals("abc").assert_False();
            "a"  .not_Equals("1"  ).assert_True ();            
            "1"  .not_Equals("a"  ).assert_True ();
            "abc".not_Equals("1bc").assert_True ();
            "a"  .not_Equals("a"  , "b"  ).assert_False();            
            "a"  .not_Equals("b"  , "a"  ).assert_False();            
            "1"  .not_Equals("1"  , "1"  ).assert_False();
            "1"  .not_Equals("2"  , "1"  ).assert_False();
            "abc".not_Equals("abc", "123").assert_False();
            "abc".not_Equals("123", "abc").assert_False();
            "a"  .not_Equals("1"  , "2"  ).assert_True ();            
            "1"  .not_Equals("a"  , "b"  ).assert_True ();
            "abc".not_Equals("1bc", "2bc").assert_True ();

            "abc".not_Equals(null , "2bc").assert_True ();
            "abc".not_Equals("1bc", null ).assert_True ();
            "abc".not_Equals(null , "abc").assert_False();
            "abc".not_Equals("abc", null ).assert_False();
            "abc".not_Equals( null       ).assert_True ();

            (null as string).not_Equals("1bc", "2bc").assert_True ();
            (null as string).not_Equals(null, "2bc" ).assert_False();
            (null as string).not_Equals(null        ).assert_False();
        }
        [Test] public void remove()
        {
            "123456".remove("1"  ).assert_Is("23456" );
            "123456".remove("6"  ).assert_Is("12345" );
            "123456".remove("123").assert_Is("456"   );

            "123456".remove(""   ).assert_Is("123456");
            "123456".remove(null ).assert_Is("123456");
        }
        [Test] public void starts()
        {
            "a  ".starts("a"  ).assert_True ();
            "aaa".starts("a"  ).assert_True ();
            "123".starts("12" ).assert_True ();
            "123".starts("123").assert_True ();
            "a  ".starts("1"  ).assert_False();
            "aaa".starts("1"  ).assert_False();
            "123".starts("ab" ).assert_False();
            "123".starts("abc").assert_False();

            "a  ".starts("a"  ,"b"  ).assert_True();
            "aaa".starts("a"  ,"b"  ).assert_True();
            "123".starts("12" ,"b"  ).assert_True();
            "123".starts("123","b"  ).assert_True();
            "a  ".starts("b"  ,"a"  ).assert_True();
            "aaa".starts("b"  ,"a"  ).assert_True();
            "123".starts("b"  ,"12" ).assert_True();
            "123".starts("b"  ,"123").assert_True();

            "aaa".starts("1"  ,"b").assert_False();
            "123".starts("ab" ,"2").assert_False();
            "123".starts("abc","3").assert_False();
        }
        [Test] public void subString_After()
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
        [Test] public void replaceAllWith()
        {
            "123456".replaceAllWith("abc","1"     ).assert_Is("abc23456"  );
            "123456".replaceAllWith("abc","6"     ).assert_Is("12345abc"  );
            "123456".replaceAllWith("abc","123"   ).assert_Is("abc456"    );            
            "123456".replaceAllWith("abc","1", "2").assert_Is("abcabc3456");
            "123456".replaceAllWith("abc",""      ).assert_Is("123456"    );

            "123456".replaceAllWith(""   ,"abc"   ).assert_Is("123456"    );
            "123456".replaceAllWith(null ,"abc"   ).assert_Is("123456"    );            
            "123456".replaceAllWith("abc",null    ) .assert_Is("123456"   );
        }
        
    }
}
