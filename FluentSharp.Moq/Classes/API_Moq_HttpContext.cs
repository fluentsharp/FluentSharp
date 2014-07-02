// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.IO;
using System.Security.Principal;
using System.Collections.Specialized;
using System.Web;
using FluentSharp.CoreLib;
using Moq;


//O2Ref:Moq.dll
//O2Ref:System.Web.Abstractions.dll

namespace FluentSharp.Moq
{
    public class API_Moq_HttpContext 
    {        	
        public Mock<HttpContextBase>        MockContext  { get; set; }
        public MockHttpRequest              MockRequest  { get; set; }
        public Mock<HttpResponseBase>       MockResponse { get; set; }
        public MockHttpSession              MockSession  { get; set; }
        public Mock<HttpServerUtilityBase>  MockServer   { get; set; }		
        
        public HttpContextBase HttpContextBase  	{ get; set; }
        public HttpRequestBase HttpRequestBase  	{ get; set; }
        public HttpResponseBase HttpResponseBase  	{ get; set; }  
        
        public String BaseDir						{ get; set; }
        //public Uri    RequestUrl                    { get; set; }   

        public API_Moq_HttpContext() : this(null)
        {			
        }
        
        public API_Moq_HttpContext(string baseDir)
        {
            BaseDir = baseDir;
            //RequestUrl = "http://localhost".uri();
            createBaseMocks();
            setupNormalRequestValues();
        }
        
        public API_Moq_HttpContext createBaseMocks()
        {
            MockContext  = new Mock<HttpContextBase>();
            MockRequest  = new MockHttpRequest(); // new Mock<HttpRequestBase>();
            MockResponse = new Mock<HttpResponseBase>();
            MockSession  = new MockHttpSession();
            MockServer   = new Mock<HttpServerUtilityBase>();
            
    
            MockContext.Setup(ctx => ctx.Request).Returns(MockRequest);
            MockContext.Setup(ctx => ctx.Response).Returns(MockResponse.Object);
            MockContext.Setup(ctx => ctx.Session).Returns(MockSession);
            MockContext.Setup(ctx => ctx.Server).Returns(MockServer.Object);
            
            
            HttpContextBase  = MockContext.Object; 
            HttpRequestBase  = MockRequest;
            HttpResponseBase = MockResponse.Object;
                        
            return this;
        }
        
        public Func<string,string> context_Server_MapPath {get;set;} 
         
        public API_Moq_HttpContext setupNormalRequestValues()		
        {							        
            var genericIdentity = new GenericIdentity("genericIdentity");
            IPrincipal genericPrincipal = new GenericPrincipal(genericIdentity, new string[] {});
            MockContext.Setup(context => context.User).Returns(()=>
                    {
                        return genericPrincipal;
                    });	     	            
            MockContext.SetupSet(context => context.User).Callback((IPrincipal principal)=>
                    {
                        genericPrincipal = principal;
                    });
            MockContext.Setup(context => context.Cache).Returns(HttpRuntime.Cache);            
                 
            //Response
            var outputStream = new MemoryStream();
            var redirectTarget = "";
            var contentType = "";
            MockResponse.SetupGet(response => response.ContentType  ).Returns(()=>
            {
                return contentType;
            });
            MockResponse.SetupSet(response => response.ContentType  ).Callback((string value)=>
            {
                contentType = value;
            });
            MockResponse.SetupGet(response => response.Cache        ).Returns(new Mock<HttpCachePolicyBase>().Object);
            MockResponse.Setup   (response => response.Cookies      ).Returns(new HttpCookieCollection()); 	     	
            MockResponse.Setup   (response => response.Headers      ).Returns(new NameValueCollection());
            MockResponse.Setup   (response => response.OutputStream ).Returns(outputStream);
            MockResponse.Setup   (response => response.Write        (It.IsAny<string>())                    ).Callback((string code)
                =>
                {
                    HttpContextBase.response_Write(code);
                    //outputStream.Write(code.asciiBytes(), 0, code.size());
                });
            MockResponse.Setup   (response => response.AddHeader    (It.IsAny<string>(), It.IsAny<string>())).Callback((string name,string value) => MockResponse.Object.Headers.Add(name,value));
            MockResponse.Setup   (response => response.Redirect     (It.IsAny<string>())                    ).Callback((string target)            =>{ redirectTarget = target; throw new Exception("Thread was being aborted.");});            
            
            MockResponse.Setup   (response => response.IsRequestBeingRedirected ).Returns(() => redirectTarget.valid());
            MockResponse.Setup   (response => response.RedirectLocation         ).Returns(() => redirectTarget);
            
            //Server
            MockServer.Setup(server => server.MapPath (It.IsAny<string>()))                 .Returns ((string path)                      =>  BaseDir.pathCombine(path));
            MockServer.Setup(server => server.Transfer(It.IsAny<string>()))                 .Callback((string target)                    =>  { redirectTarget = target; throw new Exception("Thread was being aborted.");}  );   // use the redirectTarget to hold this value
            MockServer.Setup(server => server.Transfer(It.IsAny<string>(),It.IsAny<bool>())).Callback((string target, bool preserveForm) =>  { redirectTarget = target; throw new Exception("Thread was being aborted.");}  );   // use the redirectTarget to hold this value            
            return this;
        }
    }
}        
