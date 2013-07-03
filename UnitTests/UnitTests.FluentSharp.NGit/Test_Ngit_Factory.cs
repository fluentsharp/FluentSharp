using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FluentSharp.ExtensionMethods;
using FluentSharp.Support_Classes;
using NUnit.Framework;
using Sharpen;

namespace UnitTests.FluentSharp_NGit
{
    [TestFixture]
    public class Test_Ngit_Factory
    {
        [Test(Description = "returns the Sharpen.dll assembly")]
        public void Dll_Sharpen()
        {
            var assembly = NGit_Factory.Dll_Sharpen();
            Assert.IsNotNull(assembly);
            Assert.IsInstanceOf<Assembly>(assembly);
            Assert.AreEqual(assembly.name(),"Sharpen");
        }

        [Test(Description = "returns the Type object of the private class Sharpen.ByteArrayOutputStream")]
        public void Type_ByteArrayOutputStream()
        {
            var type = NGit_Factory.Type_ByteArrayOutputStream();
            Assert.IsNotNull(type);
            Assert.IsInstanceOf<Type>(type);
            Assert.AreEqual(type.name(),"ByteArrayOutputStream");
            Assert.AreEqual(type.fullName(),"Sharpen.ByteArrayOutputStream");
        }

        [Test(Description = "returns a new instance of the OutputStream object")]
        public void New_OutputStream()
        {
            var outputStream = NGit_Factory.New_OutputStream();
            Assert.IsNotNull(outputStream);
            Assert.IsInstanceOf<OutputStream>(outputStream);            
        }
    }
}
