using System.Collections.Generic;
using System.Linq;
using FluentSharp.CoreLib.API;


namespace FluentSharp.CoreLib.Utils
{

	public static class SecretData_ExtensionMethods
    {
        //#region credential

        public static List<Credential> credentialTypes(this SecretData secretData, string credentialType)
        {                    
            var credentials = new List<Credential>();
            if (secretData.isNull())
            	return credentials;
            if (credentialType.valid().isFalse())
                if (secretData.Credentials != null)
                    return secretData.Credentials;
                else
                    return credentials;
            
            foreach (var credential in secretData.Credentials)
                if (credential.CredentialType == credentialType)
                    credentials.add(credential);
            return credentials;
        }
                
		public static Credential credential(this string fileWithSecretData, string credentialTypeOrName)
        {
        	if (fileWithSecretData.fileExists().isFalse())
    			fileWithSecretData = PublicDI.config.UserData.pathCombine(fileWithSecretData);
            if (fileWithSecretData.fileExists())
            {
                var secretData = fileWithSecretData.deserialize<SecretData>();
                return secretData.credential(credentialTypeOrName);
            }
            return null;
        }

        public static Credential credential(this SecretData secretData, string credentialTypeOrName)
        {        	
            if (secretData != null)
            {
                var credentials = secretData.credentialTypes(credentialTypeOrName);
                if (credentials != null && credentials.size() > 0)
                    return credentials[0];
                "Finding by Username".debug();
                foreach(var credential in secretData.Credentials)
                	if (credential.CredentialType ==credentialTypeOrName || credential.UserName == credentialTypeOrName)
                	{                		
                		"found credential.UserName: {0} with type: {1}".info(credential.UserName, credential.CredentialType);
                		return credential;
                	}
            }
            return null;
        }
        
        public static List<Credential> credentials(this string fileWithSecretData)
    	{
    		if (fileWithSecretData.fileExists().isFalse())
                fileWithSecretData = PublicDI.config.UserData.pathCombine(fileWithSecretData);
    		if (fileWithSecretData.fileExists())
            {
            	var secretData = fileWithSecretData.deserialize<SecretData>();
    			return secretData.Credentials;
            }
           	return new List<Credential>();
    	}
    	
    	public static List<Credential> credentialTypes(this string fileWithSecretData, string credentialType)
    	{
    		if (fileWithSecretData.fileExists())
            {
    			var secretData = fileWithSecretData.deserialize<SecretData>();
    			return secretData.credentialTypes(credentialType);
    		}
    		return new List<Credential>();
    	}
        
                
        //#endregion
        
       // #region username

        public static string username(this SecretData secretData)
        {
            return secretData.username("", 0);
        }

        public static string username(this SecretData secretData, string credentialType)
        {
            return secretData.username(credentialType,0);
        }
        
        public static string username(this SecretData secretData, int index)
        {
            return secretData.username("", index);
        }

        public static string username(this SecretData secretData, string credentialType, int index)
        {
            var credentials = secretData.credentialTypes(credentialType);
            if (index < credentials.size())
                return credentials[index].UserName;
            return "";
        }

        public static List<Credential> usernames(this SecretData secretData)
        {
            return secretData.usernames("");
        }
        public static List<Credential> usernames(this SecretData secretData, string credentialType)
        {
            var usernames = from credential in secretData.credentialTypes(credentialType)
                            select credential;
            return usernames.ToList();
        }
        
        public static string username(this Credential credential)
        {
        	if (credential.notNull())
            	return credential.UserName;
            return "";
        }
        
        public static List<string> names(this List<Credential> credentials)
        {
        	return credentials.usernames();
        }
        public static List<string> usernames(this List<Credential> credentials)
        {
        	return (from credential in credentials
        		    select credential.UserName).toList();
        }

        //#region get_User

        public static Credential get_User(this SecretData secretData, string userName)
        {
            return secretData.get_User("", userName);
        }

        public static Credential get_User(this SecretData secretData, string credentialType, string userName)
        {
            if (secretData != null && secretData.Credentials != null)
                foreach (var credential in secretData.Credentials)
                    if (credential.UserName == userName)
                        if (credentialType.valid().isFalse() || credentialType == credential.CredentialType)
                            return credential;
            return null;
        }
        
        //#region password

        public static string password(this SecretData secretData)
        {
            return secretData.password("", 0);
        }

        public static string password(this SecretData secretData, string credentialType)
        {
            return secretData.password(credentialType, 0);
        }

        public static string password(this SecretData secretData, int index)
        {
            return secretData.password("", index);
        }

        public static string password(this SecretData secretData, string credentialType, int index)
        {
            var credentials = secretData.credentialTypes(credentialType);
            if (index < credentials.size())
                return credentials[index].Password;
            return "";
        }

        public static string password(this SecretData secretData, string credentialType, string username)
        {
            foreach (var credential in secretData.Credentials)
                if (credential.UserName == username && credential.CredentialType == credentialType)
                    return credential.Password;
            return "";
        }
                
        public static string password(this Credential credential)
        {
            return credential.Password;
        }
       // #endregion

    }   
    
    
}
