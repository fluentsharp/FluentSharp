// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using Microsoft.Win32;


namespace FluentSharp.CoreLib.API
{
    public class WinRegistry
    {
        public static String getKeyValue_LocalMachine(String sKeyToOpen, String sValueToFetch)
        {
            return getKeyValue(Registry.LocalMachine.OpenSubKey(sKeyToOpen), sValueToFetch);
        }

        public static String getKeyValue_CurrentUser(String sKeyToOpen, String sValueToFetch)
        {
            return getKeyValue(Registry.CurrentUser.OpenSubKey(sKeyToOpen), sValueToFetch);
        }

        public static String getKeyValue_Users(String sKeyToOpen, String sValueToFetch)
        {
            return getKeyValue(Registry.Users.OpenSubKey(sKeyToOpen), sValueToFetch);
        }

        public static String getKeyValue(RegistryKey rkRegistryKeyToOpen, String sValueToFetch)
        {
            try
            {
                if (rkRegistryKeyToOpen != null)
                {
                    Object oValue = rkRegistryKeyToOpen.GetValue(sValueToFetch);
                    if (oValue != null)
                        return oValue.ToString();
                }
            }
            catch (Exception ex)
            {
                PublicDI.log.error("in getKeyValue :{0}", ex.Message);
            }
            return "";
        }
    }
}
