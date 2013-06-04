using NUnit.Framework;
using FluentSharp.ExtensionMethods;
using O2.Kernel;
using O2.Kernel.InterfacesBaseImpl;

namespace UnitTests.FluentSharp_CoreLib
{
    [TestFixture]
    public class Test_Logging
    {
        [Test]
        public void MemoryLogger()
        {
            var log = PublicDI.log;
            var logTarget = log.LogRedirectionTarget as Logger_DiagnosticsDebug;
            
            Assert.AreEqual  (log.typeName(), "KO2Log");
            Assert.AreEqual  (log.LogRedirectionTarget.typeName(), "Logger_DiagnosticsDebug");
            Assert.IsNotNull(logTarget);
            
            var logData   = logTarget.LogData;
            logData.Clear();

            Assert.AreEqual  (logData.str(), "");

            var msg1     =        "INFO: {0}" .line().format("Test info" .info());
            var logData1 = logData.str();
            var msg2     = msg1 + "DEBUG: {0}".line().format("Test debug".debug());
            var logData2 = logData.str();
            var msg3     = msg2 + "ERROR: {0}".line().format("Test error".error());
            var logData3 = logData.str();
            Assert.AreEqual  (logData1, msg1);
            Assert.AreEqual  (logData2, msg2);
            Assert.AreEqual  (logData3, msg3);
            
            //logData.str().info();
        }
    }
}
