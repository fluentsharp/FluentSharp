using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using O2.DotNetWrappers.Network;

namespace O2.DotNetWrappers.ExtensionMethods
{
    public static class Web_Encoding_ExtensionMethods
    {
        #region encode/decode

        public static string urlEncode(this String stringToEncode)
        {
            return WebEncoding.urlEncode(stringToEncode);
        }

        public static string urlDecode(this String stringToEncode)
        {
            return WebEncoding.urlDecode(stringToEncode);
        }

        public static string htmlEncode(this String stringToEncode)
        {
            return WebEncoding.htmlEncode(stringToEncode);
        }

        public static string htmlDecode(this String stringToEncode)
        {
            return WebEncoding.htmlDecode(stringToEncode);
        }
        #endregion
    }
}
