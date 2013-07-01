using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentSharp;
using FluentSharp.ExtensionMethods;
using NGit;
using NUnit.Framework;

namespace UnitTests.FluentSharp_NGit
{
    [TestFixture]
    public class Test_Misc : Temp_Repo
    {
        [Test(Description = "Changes backslash \\ into a forward slash")]
        public void changeBackSlashWithForwardSlash()
        {
            var test1   = @"C:\\aa\\bb\\c.txt";
            var test2   = @"\\aa\\bb\\c.txt";
            var test3   = @"aa\\bb\\c.txt";
            var test4   = @"bb\\c.txt";
            var result1 = @"C:/aa/bb/c.txt";
            var result2 = @"/aa/bb/c.txt";
            var result3 = @"aa/bb/c.txt";
            var result4 = @"bb/c.txt";

            Assert.AreEqual(result1, test1.changeBackslashWithForwardSlash());
            Assert.AreEqual(result2, test2.changeBackslashWithForwardSlash());
            Assert.AreEqual(result3, test3.changeBackslashWithForwardSlash());
            Assert.AreEqual(result4, test4.changeBackslashWithForwardSlash());
        }

        [Test(Description = "Changes double forward slash //  into a single slash /")]
        public void fixDoubleForwardSlash()
        {
            var test1 = @"C://aa//bb//c.txt";
            var test2 = @"//aa//bb//c.txt";
            var test3 = @"aa//bb//c.txt";
            var test4 = @"bb//c.txt";
            var result1 = @"C:/aa/bb/c.txt";
            var result2 = @"/aa/bb/c.txt";
            var result3 = @"aa/bb/c.txt";
            var result4 = @"bb/c.txt";

            Assert.AreEqual(result1, test1.fixDoubleForwardSlash());
            Assert.AreEqual(result2, test2.fixDoubleForwardSlash());
            Assert.AreEqual(result3, test3.fixDoubleForwardSlash());
            Assert.AreEqual(result4, test4.fixDoubleForwardSlash());
        }        
        
        [Test(Description = "Returns the NGIt Git object")]
        public void git()
        {
            var git = nGit.git();
            Assert.IsNotNull(git);
            Assert.IsNull((null as API_NGit).git());
        }
        
        [Test(Description = "Returns the .git folder")]
        public void git_Folder()
        {
            var gitFolder       = nGit.git_Folder();
            var expectedFolder  = nGit.files_Location().pathCombine(".git");
            var headFile        = gitFolder.pathCombine("HEAD");

            Assert.AreEqual(gitFolder, expectedFolder);
            Assert.IsTrue  (headFile.fileExists());
            Assert.AreEqual(headFile.fileContents().trim(), "ref: refs/heads/master");
        }

        [Test(Description = "Returns the SHA1 of the current repo HEAD")]
        public void head()
        {
            var head_BeforeCommit = nGit.head();
            var revCommit = nGit.add_and_Commit_Random_File();
            var head_AfterCommit = nGit.head();
            Assert.IsNull   (head_BeforeCommit);
            Assert.IsNotNull(head_AfterCommit);
            Assert.AreEqual (revCommit.sha1(), head_AfterCommit);
            Assert.IsNull   ((null as API_NGit).head());
        }

        [Test(Description = "Retuns a true is the the provided folder is a Git repository")]
        public void isGitRepository()
        {
            Assert.IsTrue(this.nGit.files_Location().isGitRepository());
            Assert.IsTrue(this.nGit.Path_Local_Repository.isGitRepository());
            Assert.IsTrue(this.repoPath.isGitRepository());

            var tempDir = "another_test".tempDir();

            Assert.IsFalse(tempDir.isGitRepository());
            var tempGitRepo = tempDir.git_Init();
            Assert.IsTrue(tempDir.isGitRepository());
            tempGitRepo.delete_Repository_And_Files();
        }        

        [Test(Description = "Converts an string into an ObjectId (AnyObjectId)")]
        public void objectId()
        {
            var badSHA1 = "12345";
            var okSHA1  = "979fb673fa2c6e8e2d787b5f08d50855079330d9";            

            Assert.IsNull   (badSHA1.objectId());
            Assert.IsNotNull(okSHA1.objectId ());
            // all these should return an empty SAH1
            Assert.AreEqual (NGit_Consts.EMPTY_SHA1, "".objectId().Name);
            Assert.AreEqual (NGit_Consts.EMPTY_SHA1, (null as string).objectId().Name);
            Assert.AreEqual (NGit_Consts.EMPTY_SHA1, "0".objectId().Name);               //Not 100% if this will be useful, but it makes sence that 0 should return an empty SAH1
        }

        [Test(Description = "Returns the NGit repository object")]
        public void repository()
        {
            var repository = nGit.repository();
            Assert.IsNotNull(repository);
            Assert.IsNull((null as API_NGit).repository());
        }

        [Test(Description = "Sets the Global Credential to use on Pushes and Pulls")]
        public void use_Credential()
        {
            var username = "a name".add_RandomLetters();
            var password = "a password".add_RandomLetters();
            nGit.use_Credential(username, password);
            Assert.AreEqual(username, nGit.Credentials.field<string>("username")); 
            Assert.AreEqual(password, nGit.Credentials.field<char[]>("password").ascii());
        }        
    }
}
