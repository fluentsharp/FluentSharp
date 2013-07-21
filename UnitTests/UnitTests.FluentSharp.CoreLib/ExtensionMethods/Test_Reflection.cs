using System.Reflection;
using System.Text;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.NUnit;
using NUnit.Framework;

namespace UnitTests.FluentSharp_CoreLib.ExtensionMethods
{    
    public class Test_Reflection : NUnitTests
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

        [Test(Description = "Appends to provided string the Assembly version from from GetCallingAssembly, in format x.x.x.x (Major.MajorRevision.Minor.MinorRevision)")]
        public void append_Version_Calling_Assembly()
        {
            var originalString  = "aaaa".add_RandomLetters(10);
            var assembly        = Assembly.GetExecutingAssembly();
            var version         = assembly.version();
            var expected        = originalString + "1.0.0.0";
            var result          = originalString.append_Version_Calling_Assembly();

            assert_Are_Equal(result, expected);
            assert_Are_Equal(result, originalString + version);
            assert_Are_Equal((null as string).append_Version_Calling_Assembly(), version);
        }

        [Test(Description = "Appends to provided string the Assembly version from FluentSharp.CoreLib, in format x.x.x.x (Major.MajorRevision.Minor.MinorRevision)")]
        public void append_Version_FluentSharp()
        {
            var originalString  = "aaaa".add_RandomLetters(10);
            var assembly        = typeof(Files).assembly();
            var version         = assembly.version();            
            var result          = originalString.append_Version_FluentSharp();
            
            assert_Are_Equal(result, originalString + version);
            assert_Are_Equal((null as string).append_Version_FluentSharp(), version);
        }

        [Test(Description = "Appends to provided string the Assembly version from FluentSharp.CoreLib, in format x.x (Major.MajorRevision)")]
        public void append_Version_FluentSharp_Short()
        {
            var originalString  = "aaaa".add_RandomLetters(10);
            var assembly        = typeof(Files).assembly();
            var version         = assembly.version_Short();            
            var result          = originalString.append_Version_FluentSharp_Short();
            
            assert_Are_Equal(result, originalString + version);
            assert_Are_Equal((null as string).append_Version_FluentSharp_Short(), version);
        }

        [Test(Description = "returns the version number of the provided assembly in format x.x.x.x (Major.MajorRevision.Minor.MinorRevision)")]
        public void version()
        {
            var currentAssembly = Assembly.GetExecutingAssembly();            
            assert_Are_Equal(currentAssembly   .version(), "1.0.0.0");
            assert_Are_Equal((null as Assembly).version(), "");
        }

        [Test(Description = "returns the version number of the provided assembly in format x.x.x.x (Major.MajorRevision")]
        public void version_Short()
        {
            var currentAssembly = Assembly.GetExecutingAssembly();            
            assert_Are_Equal(currentAssembly   .version_Short(), "1.0");
            assert_Are_Equal((null as Assembly).version_Short(), "");
        }
    }
}
