using System;
using System.Web;
using FluentSharp;
using FluentSharp.CoreLib;

namespace FluentSharp.Web
{
    public class HttpContextFactory
    {
        public static HttpContextBase _context; 		

        public static HttpContextBase       Current
        {
            get
            {
                if (_context.notNull())                                 // context has been set
                    return _context;
                if (HttpContext.Current.isNull())                       // context has not been set and we are not inside ASP.NET
                    return null;                
                return new HttpContextWrapper(HttpContext.Current);     // return current asp.net Context			    
            }
        }
        public static HttpContextBase       Context     { 	get { return Current;          } set { _context = value;  }	}
        public static HttpRequestBase       Request		{	get { return Current.notNull() ? Current.Request : null;  } }
        public static HttpResponseBase      Response	{	get { return Current.notNull() ? Current.Response: null;  } }
        public static HttpServerUtilityBase Server      {	get { return Current.notNull() ? Current.Server  : null;  } }
        public static HttpSessionStateBase  Session		{   get { return Current.notNull() ? Current.Session : null;  } }
        
        public static DateTime              LastModified_HeaderDate	{ get; set; } //used to calculate if we will send a '302 Not Modified' to the user (this could be better if the value used was the 'TM Startup Date')

        static HttpContextFactory()
        {
            LastModified_HeaderDate = DateTime.Now;			        
        }
    }
}