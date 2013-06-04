using NUnit.Framework;
using O2.DotNetWrappers.DotNet;
using FluentSharp.ExtensionMethods;

namespace UnitTests.FluentSharp_CoreLib
{
    [TestFixture]
    public class Test_O2Threads
    {        
        [Test]
        public void Check_If_MtaThreadName_Contains_StackTrace()
        {            
            var threadCount_BeforeWait = O2Thread.ThreadsCreated.size();
            this.sleep(250,              () => "after sleep".info());
            var threadCount_AfterWait  = O2Thread.ThreadsCreated.size();            
            var lastThread             = O2Thread.ThreadsCreated.last();
            var lastThread_Name        = lastThread.name();

            Assert.AreNotEqual(threadCount_BeforeWait, threadCount_AfterWait, "threadCount");
            Assert.NotNull(lastThread       , "lastThread");
            Assert.NotNull(lastThread_Name  , "lastThread_Name");
            Assert.IsTrue(lastThread_Name.contains("Check_If_MtaThreadName_Contains_StackTrace"));
            lastThread_Name.info();
        }
    }
}
