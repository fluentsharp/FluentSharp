using System.Web;
using FluentSharp.CoreLib;
using FluentSharp.Moq;
using FluentSharp.Web;
using NUnit.Framework;

namespace UnitTests.FluentSharp.Web_4_5
{
    [TestFixture]
    public class Test_Cookies_ExtensioMethods
    {
        public Test_Cookies_ExtensioMethods()
        {
            HttpContextFactory.Context.mock();
        }
        [Test]
        public void set_Cookie_HttpResponseBase()
        {
            var response = HttpContextFactory.Response;
            var cookieName = 10.randomLetters();
            var cookieValue = 10.randomLetters();

            Assert.IsNull  (response.cookie(cookieName));
            
            response.set_Cookie(cookieName, cookieValue);

            Assert.IsNotNull(response.cookie(cookieName));
            Assert.AreEqual (response.cookie(cookieName), cookieValue);

            //test nulls
            Assert.IsNull   (response.set_Cookie(null,null));
            Assert.IsNull   ((null as HttpResponseBase).set_Cookie(null,null));
        }

        [Test]
        public void set_Cookie_httpRequestBase()
        {
            var request = HttpContextFactory.Request;
            var cookieName = 10.randomLetters();
            var cookieValue = 10.randomLetters();

            Assert.IsNull  (request.cookie(cookieName));
            
            request.set_Cookie(cookieName, cookieValue);

            Assert.IsNotNull(request.cookie(cookieName));
            Assert.AreEqual (request.cookie(cookieName), cookieValue);

            //test nulls
            Assert.IsNull   (request.set_Cookie(null,null));
            Assert.IsNull   ((null as HttpRequestBase).set_Cookie(null,null));
        }
    }
}
