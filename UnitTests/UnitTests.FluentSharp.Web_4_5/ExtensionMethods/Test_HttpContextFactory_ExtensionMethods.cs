using System.Web;
using FluentSharp.Moq;
using FluentSharp.Web;
using NUnit.Framework;
using FluentSharp.CoreLib;

namespace TeamMentor.UnitTests
{
    [TestFixture]
    public class Test_HttpContextFactory_ExtensionMethods
    {
        public HttpContextBase       context        { get { return HttpContextFactory.Context ; } }
        public HttpRequestBase       request	    { get { return HttpContextFactory.Request ; } }
        public HttpResponseBase      response	    { get { return HttpContextFactory.Response; } }
        public HttpServerUtilityBase server         { get { return HttpContextFactory.Server  ; } }
        public HttpSessionStateBase  sessionState	{ get { return HttpContextFactory.Session ; } }

        [SetUp]
        public void setup()
        {
            context.mock();
            Assert.IsNotNull(context);
            Assert.IsNotNull(request);
            Assert.IsNotNull(response);
            Assert.IsNotNull(server);
            Assert.IsNotNull(sessionState);
        }

        [Test] public void addCookieFromResponseToRequest()
        {
            var cookieName = 10.randomLetters();
            var cookieValue = 10.randomLetters();
            var cookieValue2 = 10.randomLetters();
            var cookieValue3 = 10.randomLetters();

            Assert.IsFalse   (response.hasCookie(cookieName));
            Assert.IsNull    (response.cookie   (cookieName));
            Assert.IsFalse   (request .hasCookie(cookieName));
            Assert.IsNull    (request .cookie   (cookieName));
            
            //changing response cookie (should change request cookie)
            response.cookie(cookieName, cookieValue);

            Assert.IsTrue    (response.hasCookie(cookieName));
            Assert.IsNotNull (response.cookie   (cookieName));
            Assert.AreEqual  (response.cookie   (cookieName),cookieValue);
            Assert.IsFalse   (request .hasCookie(cookieName));
            Assert.IsNull    (request .cookie   (cookieName));

            context.addCookieFromResponseToRequest(cookieName);

            Assert.IsTrue    (request.hasCookie (cookieName));
            Assert.IsNotNull (request.cookie    (cookieName));
            Assert.AreEqual  (request.cookie    (cookieName),cookieValue);

            //changing response cookie  (should change request cookie)
            response.cookie(cookieName, cookieValue2);
            context.addCookieFromResponseToRequest(cookieName);

            Assert.AreEqual  (response.cookie   (cookieName),cookieValue2);
            Assert.AreEqual  (request.cookie    (cookieName),cookieValue2);

            //changing request cookie  (should NOt change response cookie)
            request.cookie(cookieName, cookieValue3);
            context.addCookieFromResponseToRequest(cookieName);

            Assert.AreEqual  (response.cookie   (cookieName),cookieValue2);
            Assert.AreEqual  (request.cookie    (cookieName),cookieValue2);

        }

        [Test]
        public void sessionId()
        {
            var newSessionId = 10.randomLetters();
            Assert.AreNotEqual(sessionState.sessionId(), "");
            Assert.AreEqual   (sessionState.sessionId(), sessionState.field("sessionId"));
            sessionState.field("sessionId", newSessionId);
            Assert.AreEqual   (sessionState.sessionId(), newSessionId);            
            sessionState.field("sessionId", null);
            Assert.AreEqual   (sessionState.sessionId(), null);
            Assert.AreEqual   ((null as HttpSessionStateBase).sessionId(), "");
        }

        [Test]
        public void ipAddress()
        {
            var newIpAddress = 10.randomLetters();
            Assert.AreEqual(request.ipAddress(), "127.0.0.1");
            request.field  ("_userHostAddress" , newIpAddress);
            Assert.AreEqual(request.ipAddress(), newIpAddress);
        }
    }
}
