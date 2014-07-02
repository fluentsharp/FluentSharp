using System.IO;
using System.Web;
using FluentSharp.CoreLib;
using FluentSharp.Web;

namespace FluentSharp.Moq
{
    public static class API_Moq_HttpContext_ExtensionMethods
    {
        public static HttpContextBase mock(this HttpContextBase contextBase)
        {
            HttpContextFactory.Context = new API_Moq_HttpContext().httpContext();
            return HttpContextFactory.Context;
        }
        public static HttpContextBase httpContext(this API_Moq_HttpContext moqHttpContext)
        {
            return moqHttpContext.HttpContextBase;
        }
        
        public static HttpContextBase request_Write_Clear(this HttpContextBase httpContextBase)
        {
            httpContextBase.Request.field("_inputStream",new MemoryStream());             
            return httpContextBase;
            //moqHttpContext.MockRequest.Setup(request =>request.InputStream).Returns(new MemoryStream()); 
            //return moqHttpContext.httpContext();
        }
    }
}