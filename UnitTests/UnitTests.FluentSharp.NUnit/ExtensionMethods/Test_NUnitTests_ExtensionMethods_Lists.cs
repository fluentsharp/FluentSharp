using FluentSharp.CoreLib;
using FluentSharp.NUnit;
using NUnit.Framework;

namespace UnitTests.FluentSharp_NUnit
{
    public class Test_NUnitTests_ExtensionMethods_Lists : NUnitTests
    {
        [Test]
        public void assert_Item_Is_Equal()
        {
            var item1 = "1".add_5_RandomLetters();
            var item2 = "1".add_5_RandomLetters();
            var items = new [] {item1, item2}.toList();
            items.assert_Item_Is_Equal(0, item1);
            items.assert_Item_Is_Equal(1, item2);            
            assert_Throws(()=>items.assert_Item_Is_Equal(0 , item2));
            assert_Throws(()=>items.assert_Item_Is_Equal(1 , item1));
            assert_Throws(()=>items.assert_Item_Is_Equal(2 , item2));
            assert_Throws(()=>items.assert_Item_Is_Equal(-1, item2));
            assert_Throws(()=>items.assert_Item_Is_Equal(99, item2));
            assert_Throws(()=>items.assert_Item_Is_Equal(99, null));

            items.assert_Item_Not_Equal(0, item2);
            items.assert_Item_Not_Equal(1, item1);
            items.assert_Item_Not_Equal(2, item1);
            items.assert_Item_Not_Equal(99, null);

            assert_Throws(()=>items.assert_Item_Not_Equal(0 , item1));
            assert_Throws(()=>items.assert_Item_Not_Equal(1 , item2));
            
        }
    }
}