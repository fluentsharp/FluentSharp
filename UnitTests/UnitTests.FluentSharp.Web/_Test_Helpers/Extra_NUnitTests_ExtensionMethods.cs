using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentSharp.CoreLib;
using FluentSharp.NUnit;
using FluentSharp.Web35;
using NUnit.Framework;

namespace FluentSharp.NUnit
{
    public static class Extra_NUnitTests_ExtensionMethods
    {
                //misc Utils

        public static NUnitTests ignore_If_Offline(this NUnitTests nUnitTests)
        {
            if("".offline())
                nUnitTests.assert_Ignore("Ignoring Test(s) because the test runner is currently offline");
            return nUnitTests;


        }
    }
}
