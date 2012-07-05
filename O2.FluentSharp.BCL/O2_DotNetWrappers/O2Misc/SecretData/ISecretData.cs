using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using O2.Kernel.ExtensionMethods;
using O2.DotNetWrappers.ExtensionMethods;
//O2File:SecretData_ExtensionMethods.cs
//O2Ref:System.Xml.Linq.dll
//O2Ref:System.Xml.dll

namespace O2.XRules.Database.Utils
{    
    public interface ISecretData
    {        
        List<Credential> Credentials { get; set;}
    }
    
    public interface ICredential
    {
        string UserName { get; set; }
        string Password { get; set; }
        string Url { get; set; }
        string CredentialType { get; set; }
        string Description { get; set; }
    }

    [Serializable]
    public class SecretData : ISecretData
    {
        public List<Credential> Credentials { get; set; }

        public SecretData()
        {
            Credentials = new List<Credential>();
        }
    }

    [Serializable]
    public class Credential : ICredential
    {
        [XmlAttribute]
        public string UserName { get; set; }
        [XmlAttribute]
        public string Password { get; set; }
        [XmlAttribute]
        public string CredentialType { get; set; }
        [XmlAttribute]
        public string Url { get; set; }
        [XmlAttribute]
        public string Description { get; set; }

        public Credential() : this("","","")
        { }

        public Credential(string userName, string password, string credentialType) 
        { 
            UserName = userName;
            Password = password;
            CredentialType = credentialType;
            Url ="";
            Description = "";
        }

        public Credential(string userName, string password) : this(userName, password, "")
        {
            
        }

        public override string ToString()
        {
        	if (CredentialType.valid())
        		return "{0} : {1}".format(CredentialType, UserName);
            return UserName ?? base.ToString();
        }
    }    
}
