using System;
using System.IO;
using System.Reflection;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.NUnit;
using NUnit.Framework;
using UnitTests.FluentSharp_CoreLib;

namespace UnitTests.FluentSharp.CoreLib
{
    [TestFixture]
    public class Test_Reflection_Assembly : NUnitTests
    {
        [Test(Description = "Returns an Assembly object from assemblyName, bytes, assemblyName")]
        public void assembly()
        {
            var currentAssembly = this.type().assembly();
            var assembly_Name = this.type().assembly_Name();
            var assembly_AssemblyName = this.type().assembly_AssemblyName();
            var assembly_Location = currentAssembly.location();
            var assembly_Via_Name = assembly_Name.assembly();
            var assembly_Via_AssemblyName = assembly_AssemblyName.assembly();
            var assembly_Via_Bytes = assembly_Location.fileContents_AsByteArray().assembly();

            Assert.AreEqual(currentAssembly, assembly_Via_Name);
            Assert.AreEqual(currentAssembly, assembly_Via_AssemblyName);
            Assert.AreEqual(currentAssembly.str(), assembly_Via_Bytes.str());

            //Test Exception Handling
            Assert.IsNull((null as Type  ).assembly());
            Assert.IsNull((null as byte[]).assembly());
        }

        [Test(Description = "Returns the assembly Name from an Type or String")]
        public void assembly_Name()
        {
            var type            = this.type();
            var currentAssembly = type.assembly();
            var name            = currentAssembly.name();
            var fullName        = currentAssembly.fullName();
            var assembly_Name_Via_Type = this.type().assembly_Name();
            var assembly_Name_Via_String = fullName.assembly_Name();

            Assert.AreEqual(name, assembly_Name_Via_Type);
            Assert.AreEqual(name, assembly_Name_Via_String);            

            //Test Exception Handling                                 
            var badType = Tests_Utils.Bad_Type_Object();            
            Assert.IsNull(badType.assembly_AssemblyName());
            Assert.IsInstanceOf<NullReferenceException>(KO2Log.Last_Exception);
            KO2Log.Last_Exception = null;
            Assert.IsNull((null as string).assembly_AssemblyName());
			if (clr.not_Mono ()) 
			{
				Assert.IsNull (@"aaab\50\r\n".assembly_AssemblyName ());
				Assert.IsInstanceOf<FileLoadException> (KO2Log.Last_Exception);
			}
        }

        [Test (Description = "Returns a PortableExecutableKind with the type of .Net assembly that the provided string is")]
        public void assembly_PortableExecutableKind()
        {
            //var location1 = this.type().assembly_Location();
            //var location2 = typeof(string).assembly_Location();
            var location3 = @"C:\noFileIshere.dll";
            var location4 = 40.randomChars();                        
            Assert.AreEqual(location3       .assembly_PortableExecutableKind(), PortableExecutableKinds.NotAPortableExecutableImage);
            Assert.AreEqual(location4       .assembly_PortableExecutableKind(), PortableExecutableKinds.NotAPortableExecutableImage);
            Assert.AreEqual((null as string).assembly_PortableExecutableKind(), PortableExecutableKinds.NotAPortableExecutableImage);

            /* todo: change to a check if assembly_PortableExecutableKind() contains  PortableExecutableKinds.ILOnly (since teamcity has differnt results
             * 
            Assert.AreEqual(location1       .assembly_PortableExecutableKind(), PortableExecutableKinds.ILOnly | PortableExecutableKinds.Required32Bit);                        
            Assert.AreEqual(location2   .assembly_PortableExecutableKind(), PortableExecutableKinds.ILOnly | PortableExecutableKinds.Required32Bit);
             *
             */
        }

        [Test (Description = "Returns an Assembly full name  (equivalent to Assembly.GetName().ToString()")]
        public void fullName()
        {
            var assembly1 = this.type().assembly();
            var assembly2 = typeof(string).assembly();
            var expected1 = "UnitTests.FluentSharp.CoreLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            var expected2 = "mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
            Assert.AreEqual(assembly1.fullName(),expected1);
            Assert.AreEqual(assembly2.fullName(),expected2);
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
  
        [Test]
        public void resolve_Assembly_Using_ExternalAssemblerResolver()
        {
            assert_Not_Null(AssemblyResolver.ExternalAssemblerResolver);
            AssemblyResolver.ExternalAssemblerResolver.clear();            
            assert_Null("System.dll".resolve_Assembly_Using_ExternalAssemblerResolver());
            assert_Null("O2_Kernel_WCF.dll".resolve_Assembly_Using_ExternalAssemblerResolver());        // this should be resolved when tryToFetchAssemblyFromO2GitHub is set
            
            Func<string,Assembly> testResolver = (originalReference)=>
                {
                    if(originalReference == "System.dll")
                        return "System.dll".assembly();
                    return null;
                };

            AssemblyResolver.ExternalAssemblerResolver.add(testResolver);

            assert_Not_Null("System.dll".resolve_Assembly_Using_ExternalAssemblerResolver());
            assert_Null("O2_Kernel_WCF.dll".resolve_Assembly_Using_ExternalAssemblerResolver());



        }
        
    }
}
