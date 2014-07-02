using System;
using System.IO;
using System.Web;
using FluentSharp.Moq;
using FluentSharp.Web;
using NUnit.Framework;
using FluentSharp.CoreLib;

namespace UnitTests.FluentSharp.Moq
{
    [TestFixture]
    public class Test_API_Moq_HttpContext
    {
        //public API_Moq_HttpContext  moqHttpContext;
        public HttpContextBase          context     { get { return HttpContextFactory.Context ; } }
        public HttpResponseBase         response    { get { return HttpContextFactory.Response; } }
        public HttpRequestBase          request     { get { return HttpContextFactory.Request ; } }
        public HttpServerUtilityBase    server      { get { return HttpContextFactory.Server ; } }

        [SetUp]
        public void Setup()
        {                     
            HttpContextFactory.Context.mock();            		
        }

        [Test] public void mock()
        {            
            HttpContextFactory.Context = null;
            Assert.IsNull(HttpContextFactory.Context);
            var mockedContext = HttpContextFactory.Context.mock();
            Assert.IsNotNull(HttpContextFactory.Context);            
            Assert.AreEqual(mockedContext, HttpContextFactory.Context);
        }
        [Test] public void HttpContext_Ctor()
        {
            Assert.IsNotNull(HttpContextFactory.Context					     , "Context");
            Assert.IsNotNull(HttpContextFactory.Context.Request			     , "Request");
            Assert.IsNotNull(HttpContextFactory.Context.Request.Headers      , "Request.Headers");
            Assert.IsNotNull(HttpContextFactory.Context.Response		     , "Response");
            Assert.IsNotNull(HttpContextFactory.Context.Response.Headers     , "Response.Headers");
            Assert.IsNotNull(HttpContextFactory.Context.Server		     	 , "Server");
            Assert.IsNotNull(HttpContextFactory.Context.Session			     , "Session");
            Assert.IsTrue   (WebUtils.runningOnLocalHost()                  , "runningOnLocalHost");
        }
        [Test] public void HttpRequest_Ctor()
        {
            Assert.IsNotNull(request.ContentType);
            Assert.IsNotNull(request.Url);
            Assert.IsNotNull(request.UserHostAddress);
            Assert.IsNull   (request.PhysicalPath);
            Assert.IsNotNull(request.InputStream);
            Assert.IsNotNull(request.Cookies);
            Assert.IsNotNull(request.Form);
            Assert.IsNotNull(request.Headers);
            Assert.IsNotNull(request.ServerVariables);            
        }
        
        [Test] public void HttpRequest_HttpResponse_Headers()
        {                        
            var request_Headers  = request.Headers;
            var response_Headers = response.Headers;

            Assert.AreEqual(0, request_Headers.size() , "request_Headers not empty");
            Assert.AreEqual(0, response_Headers.size(), "response_Headers not empty");

            var name            = "Name" .add_RandomLetters();
            var value           = "Value".add_RandomLetters();

            response.AddHeader(name, value);			

            Assert.AreEqual(1, response_Headers.size() , "response_Headers should had 1 key");

            var firstKey        = response_Headers.Keys.first().str();

            Assert.AreEqual(name, firstKey						, "first Name");			
            Assert.AreEqual(value, response_Headers[firstKey]	, "first Value");
        }
        [Test] public void HttpRequest_Variables()
        {            
            var testUri               = "http://1.1.1.1/test".uri();

            request.field("_url", testUri);
            
            var isLocal               = request.IsLocal;
            var isSecureConnection    = request.IsSecureConnection;

            Assert.AreEqual(testUri, context.Request.Url);
            Assert.IsFalse(isLocal           , "isLocal");
            Assert.IsFalse(isSecureConnection, "isSecureConnection");

            request.field("_url", "https://localhost/test".uri());
            
            isLocal                   = request.IsLocal;
            isSecureConnection        = request.IsSecureConnection;

            Assert.IsTrue(isLocal           , "isLocal 2");
            Assert.IsTrue(isSecureConnection, "isSecureConnection 2");

            var form_Key          = 10.randomLetters();
            var form_Value        = 10.randomLetters();
            var queryString_Key   = 10.randomLetters();
            var queryString_Value = 10.randomLetters();

            request.Form       [form_Key]        = form_Value;
            request.QueryString[queryString_Key] = queryString_Value;

            Assert.AreEqual(request            [form_Key]        , form_Value);
            Assert.AreEqual(request.Form       [form_Key]        , form_Value);
            Assert.AreEqual(request            [queryString_Key] , queryString_Value);
            Assert.AreEqual(request.QueryString[queryString_Key] , queryString_Value);
        }
        [Test] public void HttpRequest_InputStream()
        {
            var testWrite = 10.randomString();
            var inputStream = (MemoryStream)request.field("_inputStream");
            Assert.IsNotNull  (inputStream );
            Assert.AreEqual   (context.request_Read(), "");

            context.request_Write(testWrite);
            Assert.AreEqual   (context.request_Read(), testWrite);

            context.request_Write_Clear();

        }
        [Test] public void HttpResponse_Variables()
        {            
            
            // check ContentType
            Assert.IsEmpty(response.ContentType);
            var contentType = 10.randomLetters();
            response.ContentType = contentType;
            Assert.AreEqual(response.ContentType, contentType);
        }
        
        [Test] public void HttpResponse_InputStream()
        {
            var testWrite1 = 10.randomString();
            var testWrite2 = 10.randomString();
            var testWrite3 = 10.randomString();
            
            Assert.AreEqual   (context.response_Read(), "");

            context.response_Write(testWrite1);
            Assert.AreEqual   (context.response_Read()       , testWrite1);
            Assert.AreEqual   (response.OutputStream.Length  , 10);
            Assert.AreEqual   (response.OutputStream.Position, 0);
            
            context.response_Write(testWrite2);            

            Assert.AreEqual   (response.OutputStream.Length  , 20);            
            Assert.AreEqual   (response.OutputStream.Position, 0);
            Assert.AreEqual   (context.response_Read_All(), testWrite1 + testWrite2);            
        }
        [Test] public void HttpResponse_ServerTransfer()
        {
            var transferTarget   = "/a/page.html";
            Assert.AreEqual         (string.Empty, response.RedirectLocation, "before Transfer");
            Assert.Throws<Exception>(()=> server.Transfer(transferTarget));                          //Server.Transfer throws exception after configuring the transfer
            Assert.IsNotNull        (context.Response.RedirectLocation, "after Transfer");
            Assert.AreEqual         (transferTarget, response.RedirectLocation);
        }
        [Test] public void HttpSession_Values()
        {            
            var session       = context.Session;
            var sessionId     = session.sessionId();
            var sessionKey1   = "SessionID";
            var sessionKey2   = "SessionID".add_RandomLetters(10);
            var sessionValue1 = Guid.NewGuid();
            var sessionValue2 = "A value".add_RandomLetters(10);

            Assert.IsNotNull  (session             , "session was null");
            Assert.AreEqual   (session.size(),0    , "session size");
            Assert.IsNull     (session[sessionKey1], "sessionKey1 value before set");
            Assert.IsNull     (session[sessionKey2], "sessionKey2 value before set");

            session[sessionKey1] = sessionValue1;
            session[sessionKey2] = sessionValue2;
            var realValue1       = session[sessionKey1]; 
            var realValue2       = session[sessionKey2];

            Assert.AreEqual   (session.size(), 2        , "session size after set");
            Assert.AreEqual   (sessionValue1, realValue1, "sessionKey1 value after set");
            Assert.AreEqual   (sessionValue2, realValue2, "sessionKey2 value after set");
            Assert.AreNotEqual(realValue1   , realValue2, "sessionKey2 value after set");
            Assert.AreNotEqual(realValue1   , sessionId);
            Assert.AreNotEqual(realValue2   , sessionId);

            Assert.IsInstanceOf<Guid>   (realValue1, "realValue1 should be a GUID");
            Assert.IsInstanceOf<string> (realValue2, "realValue2 should be a string");
        }
        [Test]
        public void serverUrl()
        {
            var request = context.Request;

            Action<string, string, string> checkResult =
                (server, port, expectedValue) =>
                    {
                        request.ServerVariables["Server_Name"] = server;
                        request.ServerVariables["Server_Port"] = port;
                        var serverUrl = context.serverUrl();
                        Assert.AreEqual(expectedValue, serverUrl);
                    };

            checkResult(null        , null , ""                  ); // no values
            checkResult("localhost" , null , ""                   ); // just server name
            checkResult(null        , "80" , ""                   ); // just server port
            checkResult("localhost" , "80" , "http://localhost"   ); 
            checkResult("localhost" , "443", "https://localhost"  ); 
            checkResult("localhost" , "88" , "http://localhost:88"); 
            checkResult("1.2.3.4"   , "80" , "http://1.2.3.4"     ); 
            checkResult("1.2.3.4"   , "443", "https://1.2.3.4"    ); 
        }        
        
    }
}
