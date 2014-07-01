using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentSharp.CoreLib.API;
using FluentSharp.NUnit;
using NUnit.Framework;

namespace UnitTests.FluentSharp_CoreLib.O2_Kernel.InterfaceBaseImpl
{
    [TestFixture]
    public class Test_KReflection : NUnitTests
    {
        [Test] public void invoke_Ctor_Static()
        {
            var kReflection = new KReflection();
            assert_Is_True(kReflection.invoke_Ctor_Static(typeof(CompileEngine)));
            assert_Not_Null(CompileEngine.LocalReferenceFolders);
            CompileEngine.LocalReferenceFolders = null;
            assert_Is_Null(CompileEngine.LocalReferenceFolders);
            assert_Is_True(kReflection.invoke_Ctor_Static(typeof(CompileEngine)));
            assert_Not_Null(CompileEngine.LocalReferenceFolders);
        }
    }
}
