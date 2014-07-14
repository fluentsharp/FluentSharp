using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.NUnit;
using NUnit.Framework;

namespace UnitTests.FluentSharp.CoreLib
{
    [TestFixture]
    public class Test_Object_ExtensionMethods : NUnitTests
    {
        [Test] public void cast()
        {
            object anString = "a string";
            object anInt    = 12;
            object anItem   = new Item();

            var castString = anString.cast<string>();
            var castInt    = anInt.cast   <int>();
            var castItem   = anItem.cast  <Item>();

            castString.assert_Equal_To(anString).assert_Instance_Of<string>();
            castInt   .assert_Equal_To(anInt   ).assert_Instance_Of<int   >();
            castItem  .assert_Equal_To(anItem)  .assert_Instance_Of<Item>();            

            anString.cast<Item>().assert_Null();
            anString.cast<int>().assert_Is_Default();

            (null as String ).cast<string>().assert_Is_Null();
            (default(string)).cast<string>().assert_Is_Null();
            (null as Item   ).cast<string>().assert_Is_Null();
            (default(Item  )).cast<string>().assert_Is_Null();
            (default(int   )).cast<string>().assert_Is_Null();
            (default(int   )).cast<int>().assert_Is_Default().assert_Is_Equal_To(0);
        }
        [Test] public void obj()
        {
            var obj1 = "a string";            
            var obj2 = (null as string);
            var obj3 = default(string);      
            var obj4 = this;    
  
            Assert.IsNotNull(obj1.obj());
            Assert.IsNull   (obj2.obj());
            Assert.IsNull   (obj3.obj());            
                        
            Assert.IsInstanceOf<object>(obj1);            
            Assert.IsInstanceOf<object>(obj4);            
        }
    }
}
