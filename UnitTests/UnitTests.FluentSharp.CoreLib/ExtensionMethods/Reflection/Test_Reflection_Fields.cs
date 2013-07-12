using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FluentSharp.CoreLib;
using NUnit.Framework;

namespace UnitTests.FluentSharp_CoreLib.ExtensionMethods.Reflection
{
    [TestFixture]
    class Test_Reflection_Fields
    {
        [Test(Description = "Returns the FieldInfo object of: object, Type ")]
        public void fieldInfo()
        {
            var typeDelegator = (Type)new TypeDelegator(this.type());            

            var fieldInfo  = typeDelegator.type().fieldInfos().first();
            var fieldInfo2 = typeDelegator.obj().fieldInfo("typeImpl");
            var fieldValue1      = typeDelegator.obj().field("typeImpl");
            Assert.IsNotNull(fieldInfo);
            Assert.IsNotNull(fieldInfo2);
            Assert.IsNotNull(fieldValue1);

            Assert.AreEqual(fieldInfo, fieldInfo2);

            typeDelegator.obj().field("typeImpl",null);

            var fieldValue2      = typeDelegator.obj().field("typeImpl");

            Assert.IsNull(fieldValue2);
        }
    }
}
