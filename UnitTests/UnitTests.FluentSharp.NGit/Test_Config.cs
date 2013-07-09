using FluentSharp.CoreLib;
using FluentSharp.Git;
using FluentSharp.Git.APIs;
using NGit;
using NUnit.Framework;

namespace UnitTests.FluentSharp_NGit
{
    [TestFixture]
    public class Test_Config : Temp_Repo
    {
        [Test(Description = "Returns the Repository's config object")]
        public void config()
        {
            var config = nGit.config();
            Assert.IsNotNull(config);
            Assert.IsInstanceOf<Config>(config);
            Assert.IsInstanceOf<StoredConfig>(config);

            Assert.IsNull((null as API_NGit).config());
        }

        [Test(Description = "Returns the Repository's config sections")]
        public void config_Sections()
        {
            var configSections = nGit.config_Sections();

            Assert.IsNotEmpty(configSections);
            Assert.AreEqual(configSections.size(), 11);
            Assert.Contains("branch", configSections);
            Assert.Contains("user", configSections);
            Assert.Contains("sendemail", configSections);
            Assert.IsEmpty((null as API_NGit).config_Sections());
        }

        [Test(Description = "Returns a particular Repository's config subsections")]
        public void config_SubSections()
        {
            nGit.add_and_Commit_Random_File();
            
            var subsection_Branch   = nGit.config_SubSections("branch");
            var subsection_Remote   = nGit.config_SubSections("remote");
            var subsection_User     = nGit.config_SubSections("user");
            var subsection_Abc      = nGit.config_SubSections("abc");
            var subsection_Null     = nGit.config_SubSections(null);
            
            Assert.IsNotEmpty(subsection_Branch);
            Assert.AreEqual  (subsection_Branch.size(),1);
            Assert.AreEqual  (subsection_Branch.first(),"master");
            Assert.IsEmpty   (subsection_Remote);
            Assert.IsEmpty   (subsection_User);
            Assert.IsEmpty   (subsection_Abc);
            Assert.IsEmpty   (subsection_Null);

            Assert.IsEmpty((null as API_NGit).config_SubSections(""));
        }

        [Test(Description = "Returns current remotes")]
        public void remotes()
        {
            //remote_add()
        }

        [Test(Description = "Adds a remote")]
        public void remote_add()
        {
            var remoteName           = "remote_".add_RandomLetters();
            var remoteUrl            = "https//github.com/o2platform/o2.platform.scripts.git";
            var remotes_Before_Add   = nGit.remotes();
            var url_Before_Add       = nGit.remote_Url(remoteName);
            var result_Add           = nGit.remote_Add(remoteName, remoteUrl);
            var url_After_Add        = nGit.remote_Url(remoteName);
            var remotes_After_Add    = nGit.remotes();
            var result_Delete        = nGit.remote_Delete(remoteName);
            var remotes_After_Delete = nGit.remotes();
            var url_After_Delete     = nGit.remote_Url(remoteName);

            Assert.IsTrue(result_Add);
            Assert.IsTrue(result_Delete);

            Assert.AreEqual(url_Before_Add    ,null);
            Assert.AreEqual(url_After_Add     ,remoteUrl);
            Assert.AreEqual(url_After_Delete ,null);

            Assert.AreEqual(remotes_Before_Add.size()  , 0);
            Assert.AreEqual(remotes_After_Add.size()   , 1);
            Assert.AreEqual(remotes_After_Delete.size(), 0);
            
            Assert.Contains(remoteName, remotes_After_Add);

            //check exception handling
            Assert.IsFalse(nGit.remote_Add(null, null));
            Assert.IsFalse(nGit.remote_Add(remoteName, null));
            Assert.IsFalse(nGit.remote_Add(null, remoteUrl));

            Assert.IsFalse(nGit.remote_Delete(null));            

            Assert.IsNull(nGit.remote_Url(null));            
            Assert.IsNull((null as API_NGit).remote_Url(null));            
            Assert.IsEmpty(nGit.remotes());

        }

        
        [Test(Description = "Removes a remote")]
        public void remote_Delete()
        {
            //remote_add() 
        }
        [Test(Description = "returns the url of a remote")]
        public void remote_url()
        {
            //remote_add() 
        }
    }
}
