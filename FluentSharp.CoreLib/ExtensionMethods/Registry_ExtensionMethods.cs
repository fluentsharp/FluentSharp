using System;

using Microsoft.Win32;

namespace FluentSharp.CoreLib
{    
	public static class Registry_ExtensionMethods
    {    
    	public static string makeDomainTrusted(this string rootDomain, string subDomain)
    	{
			try
			{				
				var ieKeysLocation = @"Software\Microsoft\Windows\CurrentVersion\Internet Settings\ZoneMap\";
				//var domainsKeyLocation =  ieKeysLocation + "Domains";
				var domainsKeyLocation =  ieKeysLocation + "EscDomains";			    
				var trustedSiteZone = 0x2;
				RegistryKey currentUserKey = Registry.CurrentUser; 
				currentUserKey.getOrCreateSubKey(domainsKeyLocation, rootDomain, false); 
				currentUserKey.createSubDomainKeyAndValue(domainsKeyLocation, rootDomain, subDomain, "http",trustedSiteZone); 
				currentUserKey.createSubDomainKeyAndValue(domainsKeyLocation, rootDomain, subDomain, "https",trustedSiteZone); 
				var message = "Added as truted the domain: {1}.{0}".format(rootDomain,subDomain);
				return message;
			}
			catch(Exception ex)
			{
				ex.log("in makeDomainTrusted");
				return ex.Message;
			}
		}
    
        public static RegistryKey getOrCreateSubKey(this RegistryKey registryKey, string parentKeyLocation, string key, bool writable)
        {
            string keyLocation = string.Format(@"{0}\{1}", parentKeyLocation, key);
            RegistryKey foundRegistryKey = registryKey.OpenSubKey(keyLocation, writable);
            return foundRegistryKey ?? registryKey.createSubKey(parentKeyLocation, key);
        }

        public static RegistryKey createSubKey(this RegistryKey registryKey, string parentKeyLocation, string key)
        {
            RegistryKey parentKey = registryKey.OpenSubKey(parentKeyLocation, true); //must be writable == true
            if (parentKey == null) 
            	 throw new NullReferenceException(string.Format("Missing parent key: {0}", parentKeyLocation)); 
            RegistryKey createdKey = parentKey.CreateSubKey(key);
            if (createdKey == null) 
            	throw new Exception(string.Format("Key not created: {0}", key));
            return createdKey;
        }
        
        //IE Specific
        public static void createSubDomainKeyAndValue(this RegistryKey currentUserKey, string domainsKeyLocation, string domain, 
        											   string subDomainKey, string subDomainValue, int zone)
        {
            RegistryKey subdomainRegistryKey = currentUserKey.getOrCreateSubKey(string.Format(@"{0}\{1}", domainsKeyLocation, domain), subDomainKey, true);
            object objSubDomainValue = subdomainRegistryKey.GetValue(subDomainValue);
            if (objSubDomainValue == null || Convert.ToInt32(objSubDomainValue) != zone)            
                subdomainRegistryKey.SetValue(subDomainValue, zone, RegistryValueKind.DWord);           
        }
	}   
}
