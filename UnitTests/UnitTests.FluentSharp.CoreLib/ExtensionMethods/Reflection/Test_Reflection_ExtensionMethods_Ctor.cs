using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using NUnit.Framework;

namespace UnitTests.FluentSharp_CoreLib.ExtensionMethods.Reflection
{
    [TestFixture]
    public class Test_Reflection_ExtensionMethods_Ctor
    {
         [Test(Description = "Creates a new instance of an type")]
        public void ctor()
        {
            var type      = typeof(StringBuilder); 
            var typeName = "StringBuilder";
            var assembly  = "mscorlib.dll";
            var firstParam = "content";
            var secondParam = 20;

            var ctorObject1 = typeName.ctor(assembly, firstParam, secondParam);
            var ctorObject2 = type.ctor(firstParam, secondParam);
            var ctorObject3 = type.ctor<StringBuilder>(firstParam, secondParam);

            PublicDI.reflection.verbose = true;

            Assert.IsNotNull(ctorObject1);
            Assert.IsNotNull(ctorObject2);
            Assert.IsNotNull(ctorObject3);

            Assert.IsInstanceOf<StringBuilder>(ctorObject1);
            Assert.IsInstanceOf<StringBuilder>(ctorObject2);
            Assert.IsInstanceOf<StringBuilder>(ctorObject3);

            Assert.IsNotNull(typeName.ctor(assembly));
            Assert.IsNotNull(type.ctor());
            Assert.IsNotNull(type.ctor<StringBuilder>());
            

            //check exception handling            
            Assert.IsNull(type.ctor<Encoder>());
            Assert.IsNull(type.ctor<StringBuilder>(null,null));
            Assert.IsNull(typeName.ctor(""));
            Assert.IsNull(typeName.ctor(null));
            Assert.IsNull(typeName.ctor("system.dll"));
            Assert.IsNull("XmlDocument".ctor(assembly));
            Assert.IsNull((null as string).ctor(assembly));
            PublicDI.reflection.verbose = false;
        }
    }
}
