using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentSharp.CoreLib;
using NUnit.Framework;

namespace UnitTests.FluentSharp_CoreLib.ExtensionMethods
{
    [TestFixture]
    public class Test_Object
    {
        [Test]
        public void obj()
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
