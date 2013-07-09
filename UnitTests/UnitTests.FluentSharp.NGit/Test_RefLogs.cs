using FluentSharp;
using NGit.Revwalk;
using NGit.Storage.File;
using NUnit.Framework;
using FluentSharp.CoreLib;

namespace UnitTests.FluentSharp_NGit
{
    [TestFixture]
    public class Test_RefLogs : Temp_Repo
    {
        public Test_RefLogs()
        {
        }

        public Test_RefLogs(Temp_Repo tempRepo) : base(tempRepo)
        {
            
        }

        [Test(Description = "Returns string lists of Reflog Entries")]
        public void reflogs()
        {
            nGit.add_and_Commit_Random_File();
            nGit.add_and_Commit_Random_File();
            var refLogs = nGit.refLogs();
            var refLog1 = refLogs.first();
            var refLog2 = refLogs.second();

            Assert.AreEqual(refLogs.size(), 2);
            Assert.IsTrue(refLog1.valid());
            Assert.IsTrue(refLog2.valid());
        }

        [Test(Description = "Returns string lists of Reflog Entries")]
        public void reflogs_Raw()
        {
            var revCommit1 = nGit.add_and_Commit_Random_File();
            //refCommit1.script_Me("refCommit").add_InvocationParameter("repository",nGit.Repository).waitForClose();
            var revCommit2  = nGit.add_and_Commit_Random_File();
            
            //nGit.add_and_Commit_Random_File();
            var refLogsRaw  = nGit.refLogs_Raw().toList();
            var refLog1     = refLogsRaw.first();
            var refLog2     = refLogsRaw.second();
            
            var ident1      = revCommit1.GetAuthorIdent(); 
            var who1        = refLog1.GetWho();


     /*       refLog1.script_Me("refLog").add_InvocationParameter("revCommit1", revCommit1)
                                     .add_InvocationParameter("repository",nGit.Repository)
                                     .waitForClose();*/

            Assert.AreEqual("commit: " + revCommit1.GetFullMessage(), refLog2.GetComment());
            Assert.AreEqual("commit: " + revCommit2.GetFullMessage(), refLog1.GetComment());

            var refLog1_Old_SHA1 = refLog1.sha1_Parent();
            var refLog1_New_SHA1 = refLog1.sha1();
            var refLog2_Old_SHA1 = refLog2.sha1_Parent();
            var refLog2_New_SHA1 = refLog2.sha1();
            var revCommit1_SHA1  = revCommit1.Name;
            var revCommit2_SHA1  = revCommit2.Name;

            Assert.AreEqual(refLog2_Old_SHA1, "0".objectId().Name);
            Assert.AreEqual(refLog2_New_SHA1, revCommit1_SHA1);
            Assert.AreEqual(refLog1_Old_SHA1, revCommit1_SHA1);
            Assert.AreEqual(refLog1_New_SHA1, revCommit2_SHA1);

            //These are weird, since I would expect them to be the same
            Assert.AreNotEqual(ident1.GetName()        , who1.GetName());
            Assert.AreNotEqual(ident1.GetEmailAddress(), who1.GetEmailAddress());            
            
        }

        [Test(Description = "Returns the string of the current SHA1 id (NGit's GetNewId)")]
        public void sha1()
        {
            var revCommit1 = nGit.add_and_Commit_Random_File();
                        
            var refLogsRaw1  = nGit.refLogs_Raw().toList();
            var refLog1      = refLogsRaw1.first();

            Assert.AreEqual(1, refLogsRaw1.size());
            Assert.AreEqual(refLog1.sha1_Parent(), NGit_Consts.EMPTY_SHA1);
            Assert.AreEqual(refLog1.sha1()       , revCommit1.sha1());

            var revCommit2 = nGit.add_and_Commit_Random_File();

            var refLogsRaw2  = nGit.refLogs_Raw().toList();
            var refLog2 = refLogsRaw2.first();
            Assert.AreEqual(2, refLogsRaw2.size());
            Assert.AreEqual(refLog2.sha1_Parent(), revCommit1.sha1());
            Assert.AreEqual(refLog2.sha1()       , revCommit2.sha1());

            //Check null return values
            Assert.IsNull((null as ReflogEntry).sha1());
            Assert.IsNull((null as ReflogEntry).sha1_Parent());
            Assert.IsNull((null as RevCommit  ).sha1());
        }

        [Test(Description = "Returns the string of the current SHA1 id (NGit's GetOldId)")]
        public void sha1_Parent()
        {
            sha1();
        }
    }
}
