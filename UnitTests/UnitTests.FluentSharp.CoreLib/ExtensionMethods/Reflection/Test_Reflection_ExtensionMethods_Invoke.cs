using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.NUnit;
using NUnit.Framework;

namespace UnitTests.FluentSharp_CoreLib.ExtensionMethods.Reflection
{
    [TestFixture]
    public class Test_Reflection_ExtensionMethods_Invoke : NUnitTests
    {
        [Test]
        public void invoke_Ctor_Static()
        {
            var targetType = typeof(CompileEngine);
            CompileEngine._specialO2Tag_DontCompile = null;
            assert_Is_Null (CompileEngine._specialO2Tag_DontCompile);
            assert_Is_True (targetType.invoke_Ctor_Static());
            assert_Not_Null(CompileEngine._specialO2Tag_DontCompile);

            
            assert_Is_False (typeof(Test_Reflection_ExtensionMethods_Invoke).invoke_Ctor_Static()); //this unittest class does not have a static ctor
            assert_Is_False (typeof(Object).invoke_Ctor_Static());
            assert_Is_False ((null as Type).invoke_Ctor_Static());
        }
    }
}
