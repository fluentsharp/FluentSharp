using NUnit.Framework;
using O2.DotNetWrappers.ExtensionMethods;

namespace UnitTests.FluentSharp.CoreLib
{
    [TestFixture]
    class Test_Ex_Objects
    {
        [Test]
        public void isNull()
        {
            // ReSharper disable HeuristicUnreachableCode  
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            // ReSharper disable UnusedVariable            
            string aString = null;
            
            if (aString.isNull().isFalse()) 
            {
                var a = aString.Length;
            }
            if (aString.notNull().isTrue()) 
            {
                var a = aString.Length;
            }
            Assert.IsTrue   (aString.isNull());
            Assert.IsFalse  (aString.isNotNull());
            Assert.IsFalse  (aString.notNull());
            aString = "value";
            Assert.IsFalse   (aString.isNull());
            Assert.IsTrue  (aString.isNotNull());
            Assert.IsTrue  (aString.notNull());
            
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            // ReSharper restore HeuristicUnreachableCode
            // ReSharper restore UnusedVariable
        }
    }
}
