using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.NUnit;
using NUnit.Framework;

namespace UnitTests.FluentSharp.CoreLib
{
    [TestFixture]
    public class Test_O2Proxy
    {
        //Workflows
        [Test] public void O2Proxy()
        {
            var o2Proxy = new O2Proxy();
            o2Proxy.ToString().assert_Is(o2Proxy.nameOfCurrentDomain());        
        }
    }
}
