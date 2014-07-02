using System;
using System.Collections.Generic;
using System.Linq;
using FluentSharp.CoreLib.API;

namespace FluentSharp.CoreLib
{
    public static class Uri_ExtensionMethods
    {
        public static Uri       web(this string _string)
        {
            return _string.uri();
        }
        public static Uri       link(this string _string)
        {
            return _string.uri();
        }
        public static Uri       url(this string _string)
        {
            return _string.uri();
        }
        public static Uri       uri(this string _string)
        {
            try
            {
                if (_string.starts("about:"))   // deal with the fact that Uri throws an error for urls like about:blank
                    return null;
                if (_string.isFile().isFalse() && _string.isFolder().isFalse())                    
                    _string = _string.StartsWith("http") ? _string : @"http://" + _string;
                
                return new Uri(_string);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static List<Uri> uris(this List<string> urls)
        {
            return (from url in urls
                where url.isUri()
                select url.uri()).toList();
        }	
        /// <summary>
        /// Returns false if the provided string is not an Url (see isUri() for the logic used)       
        /// </summary>
        /// <param name="_string"></param>
        /// <returns></returns>
        public static bool      isNotUri(this string _string)
        {
            return _string.isUri().isFalse();
        }
        public static bool      isUri(this string _string)
        {
            return _string.validUri();
        }
        public static bool      validUri(this string _string)
        {
            try
            {
                if (_string.valid() && _string.contains("://"))
                {
                    var uri = new Uri(_string);
                    return (uri.IsAbsoluteUri && uri.IsFile.isFalse());
                }
            }
            catch (Exception ex)
            {
                ex.log();
            }
            return false;
        }
        public static string    fileNameFriendly(this Uri uri)
        {
            return Files.getSafeFileNameString(uri.str());
        }        
        public static string    hostUrl(this Uri uri)
        {
            return "{0}://{1}".format(uri.Scheme, uri.Host);
        }
        public static string    pathNoQuery(this Uri uri)
        {
            return uri.Query.valid()
                ? uri.AbsoluteUri.remove(uri.Query)
                : uri.AbsoluteUri;
        }
        public static Uri       append(this Uri uri, string virtualPath)
        {
            try
            {
                return new Uri(uri, virtualPath);
            }
            catch
            {
                return null;
            }
        }
        public static bool      isWebRequest(this Uri uri)
        {
            if (uri.notNull())
                return uri.Scheme.lower() == "html" || uri.Scheme.lower() == "https";
            return false;
        }
        
    }
}