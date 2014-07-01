using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentSharp.CoreLib;
using FluentSharp.NUnit;
using NUnit.Framework;

namespace UnitTests.FluentSharp_CoreLib.ExtensionMethods.Reflection
{
    [TestFixture]
    public class Test_Reflection_ExtensionMethods_Types : NUnitTests

    {
        [Test(Description="Returns the base types of the provided objects (if addParent is set, the type provided will be included)")]
        public void baseTypes()
        {
            
            var type = this.type();
            
            type.baseTypes()     .assert_Not_Null()
                                 .assert_Not_Empty()
                                 .assert_Size_Is(2)
                                 .assert_Item_Is_Equal(0, typeof(NUnitTests))
                                 .assert_Item_Is_Equal(1, typeof(object));

            //with addParent = false, the result should be the same as above
            type.baseTypes(false).assert_Not_Null()
                                 .assert_Not_Empty()
                                 .assert_Size_Is(2)
                                 .assert_Item_Is_Equal(0, typeof(NUnitTests))
                                 .assert_Item_Is_Equal(1, typeof(object));
            
            //with addParent = true, the result should have one more class (the type used to map the base types)                
            type.baseTypes(true ).assert_Not_Null()
                                 .assert_Not_Empty()
                                 .assert_Size_Is(3)
                                 .assert_Item_Is_Equal(0, this.type())
                                 .assert_Item_Is_Equal(1, typeof(NUnitTests))
                                 .assert_Item_Is_Equal(2, typeof(object));
                                 

        }
    }
}
