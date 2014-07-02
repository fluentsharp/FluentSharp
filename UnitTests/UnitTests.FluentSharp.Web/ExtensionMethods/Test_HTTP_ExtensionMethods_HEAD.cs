using System.Net;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.NUnit;
using FluentSharp.Web35;
using NUnit.Framework;

namespace UnitTests.FluentSharp.Web.ExtensionMethods
{
    [TestFixture]
    public class Test_HTTP_ExtensionMethods_HEAD : NUnitTests
    {
        [SetUp]
        public void setup()
        {
            this.ignore_If_Offline();
        }

        [Test] public void HEAD()
        {
            assert_True ("http://o2platform.com"           .uri().HEAD());
            assert_True ("http://www.o2platform.com"       .uri().HEAD());
            assert_False("http://AAAABBBCCC.o2platform.com".uri().HEAD());
        }
        [Test] public void HEAD_StatusCode()
        {
            assert_Are_Equal("http://o2platform.com"           .HEAD_StatusCode()   , HttpStatusCode.OK);
            assert_Are_Equal("http://AAAABBBCCC.o2platform.com".HEAD_StatusCode()   , HttpStatusCode.NotFound);
            assert_Are_Equal(PublicDI.config.O2GitHub_ExternalDlls.HEAD_StatusCode(), HttpStatusCode.BadRequest);

            ////nulls and bad data            
            assert_Are_Equal(default(HttpStatusCode)                , (HttpStatusCode)0);
            assert_Are_Equal((null as string)  .HEAD_StatusCode()   , (HttpStatusCode)0);
            assert_Are_Equal(""                .HEAD_StatusCode()   , (HttpStatusCode)0);
            assert_Are_Equal(10.randomLetters().HEAD_StatusCode()   , (HttpStatusCode)0);
            assert_Are_Equal(10.randomChars()  .HEAD_StatusCode()   , (HttpStatusCode)0);
        }
        [Test] public void httpFileExists()
        {
            assert_True ("http://o2platform.com/"           .httpFileExists());

            //false negatives
            assert_False("http://o2platform.com"            .httpFileExists()); 
            assert_False("http://www.o2platform.com"        .httpFileExists());
            assert_False("http://www.o2platform.com/"       .httpFileExists());
            assert_False("http://AAAABBBCCC.o2platform.com" .httpFileExists());
            assert_False("http://AAAABBBCCC.o2platform.com/".httpFileExists());

            //nulls and bad data
            assert_False((null as string)                   .httpFileExists()); 
            assert_False(""                                 .httpFileExists()); 
            assert_False(10.randomLetters()                 .httpFileExists()); 
            assert_False(10.randomChars()                   .httpFileExists()); 
            
        }

    }
}
