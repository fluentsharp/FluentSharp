using System.Text;
using NGit.Transport;
using NGit.Util;
using NSch;
using Sharpen;

namespace FluentSharp.Git.Utils
{
    //based on example from http://stackoverflow.com/questions/13764435/ngit-making-a-connection-with-a-private-key-file/
    public class CustomConfigSessionFactory : JschConfigSessionFactory
    {
        public string PrivateKey { get; set; }
        public string PublicKey  { get; set; }

        public CustomConfigSessionFactory(string publicKey, string privateKey)
        {
            PublicKey  = publicKey;
            PrivateKey = privateKey;
        }

        protected override void Configure(OpenSshConfig.Host hc, Session session)
        {
            var config = new Properties();
            config["StrictHostKeyChecking"] = "no";
            config["PreferredAuthentications"] = "publickey";
            session.SetConfig(config);

            var jsch = this.GetJSch(hc, FS.DETECTED);
            jsch.AddIdentity("KeyPair", Encoding.UTF8.GetBytes(PrivateKey), Encoding.UTF8.GetBytes(PublicKey), null);
        }
    }
}
