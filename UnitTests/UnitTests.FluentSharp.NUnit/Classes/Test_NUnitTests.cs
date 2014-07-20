using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.NUnit;
using NUnit.Framework;

namespace UnitTests.FluentSharp_NUnit
{
    public class Test_NUnitTests : NUnitTests
    {
        [Test(Description = "Asserts that bool is false")]
        public void assert_Is_False()
        {            
            assert_Is_False    (false);
            assert_Is_True     (true);
            assert_Is_Not_False(true);
            assert_Is_Not_True (false);
            
        }

        [Test(Description = "Asserts that an object is Null")]
        public void assert_Is_Null()
        {            
            assert_Is_Null    ((null as string));            
            assert_Is_Not_Null(" aaa ");                            
        }

        [Test(Description = "Asserts that an object is Not Null")]
        public void assert_Is_Not_Null()
        {
            assert_Is_Null();
        }
    }
}