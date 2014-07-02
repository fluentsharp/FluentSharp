using System;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using FluentSharp.CoreLib;

namespace FluentSharp.Moq
{
    public class MockHttpRequest : HttpRequestBase
    {
        public string _userHostAddress = "127.0.0.1";        
        public string _contentType     = "";        
        public Uri    _url             = "http://localhost".uri();
        public Stream _inputStream     =  new MemoryStream();  
        public string _physicalPath    = null;

        public HttpCookieCollection _cookies           = new HttpCookieCollection();
        public NameValueCollection  _queryString       = new NameValueCollection();
        public NameValueCollection  _form              = new NameValueCollection();
        public NameValueCollection  _headers           = new NameValueCollection();
        public NameValueCollection  _serverVariables   = new NameValueCollection();

        public override string  ContentType         {  get {   return _contentType;           } set { _contentType = value; } }
        public override bool    IsLocal             {  get {   return _url.Host == "localhost" || _url.Host=="127.0.0.1";   } }
        public override bool    IsSecureConnection  {  get {   return _url.Scheme.lower() == "https";                       } }
        public override Uri     Url                 {  get {   return _url;                                                 } }
        public override string  UserHostAddress     {  get {   return _userHostAddress;                                     } }
        public override string  PhysicalPath        {  get {   return _physicalPath;                                                 } }

        public override Stream                InputStream     {  get {   return _inputStream;       } }
        public override HttpCookieCollection  Cookies         {  get {   return _cookies;           } }
        public override NameValueCollection   QueryString     {  get {   return _queryString;       } }
        public override NameValueCollection   Form            {  get {   return _form;              } }
        public override NameValueCollection   Headers         {  get {   return _headers;           } }
        public override NameValueCollection   ServerVariables {  get {   return _serverVariables;   } }

        public override string this[string key]
        {
            get
            {          
                if (_form.Get(key).notNull())
                    return _form.Get(key);
                if (_queryString.Get(key).notNull())
                    return _queryString.Get(key);
                return null;                
            }            
        }             
    }
}