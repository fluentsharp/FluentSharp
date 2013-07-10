using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using CassiniDev;
using FluentSharp.AspNet_MVC;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using NUnit.Framework;

namespace UnitTests.FluentSharp_AspNet_MVC
{
    public class Temp_Cassini_Site
    {
        public API_Cassini apiCassini;
        public Server      server;
        public String      webRoot;

        [SetUp]
        public void SetUp()
        {
            webRoot = "_temp_CassiniSite".tempDir();
            apiCassini = new API_Cassini(webRoot).start();
            server  = apiCassini.CassiniServer;            

            Assert.IsNotNull(apiCassini);
            Assert.IsNotNull(server);
            Assert.Less     (32767, apiCassini.port());
            Assert.IsNotNull(apiCassini.port().tcpClient());
            Assert.IsTrue   (webRoot.dirExists());
            Assert.AreEqual (webRoot, apiCassini.PhysicalPath);

            //var httpContent = HttpContext.Current;
            //
        }
        
        [TearDown]
        public void TearDown()
        {
            apiCassini.stop();
            Assert.IsNull (apiCassini.port().tcpClient());
            Files.deleteFolder(webRoot, true);
            Assert.IsFalse(webRoot.dirExists());
        }
    }
}
