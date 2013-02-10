using NUnit.Framework;
using O2.DotNetWrappers.ExtensionMethods;
using O2.Kernel;

namespace UnitTests.FluentSharp_CoreLib
{
    [TestFixture]
    public class Test_Reflection
    {
        public string property1 { get; set; }
        public int    property2 { get; set; }

        [Test]
        public void Assemblies()
        {
            var currentAssembly = this.type().Assembly;
            var assembly = currentAssembly.name().assembly();
            Assert.AreEqual(assembly,currentAssembly);
        }

        [Test]
        public void Types()
        {
            var currentType = this.type();
            Assert.AreEqual(currentType, this.GetType());
        }

        [Test]
        public void Properties()
        {
            property1           = "a value";
            property2           = 12;
            var properties      = this.type().properties();
            var propertyValues  = this.propertyValues();
            var stringValues    = this.propertyValues<string>();
            var intValues       = this.propertyValues<int>();
            Assert.AreEqual     (properties.first() .Name, "property1");
            Assert.AreEqual     (properties.second().Name, "property2");
            Assert.AreEqual     (this.prop("property1"),property1);
            Assert.AreEqual     (this.prop("property2"),property2);
            Assert.AreNotEqual  (this.prop("property1"), this.prop("property2"));            
            Assert.AreEqual     (2, propertyValues.size());
            Assert.AreEqual     (propertyValues.first(),property1);
            Assert.AreEqual     (propertyValues.second(),property2);
            Assert.AreEqual     (1, stringValues.size());
            Assert.AreEqual     (1, intValues.size());
            Assert.AreEqual     (stringValues.first(),property1);
            Assert.AreEqual     (intValues.first(),property2);
            Assert.IsNull       (stringValues.second());
            Assert.AreEqual     (stringValues.second()  , default(string));
            Assert.AreEqual     (intValues.second()     , default(int));
        }

        [Test]
        public void AssemblyAttributes()
        {
            var fluentSharpCoreLib = typeof (PublicDI).Assembly;
            var thisAssembly       = typeof (Test_Reflection).Assembly;
            var assemblyAttributes = fluentSharpCoreLib.attributes();            
            Assert.IsNotEmpty(assemblyAttributes);
            Assert.IsFalse(fluentSharpCoreLib.hasAttribute<SkipTempPathLengthVerification>());
            Assert.IsTrue (thisAssembly      .hasAttribute<SkipTempPathLengthVerification>());
            
        }

    }
}
