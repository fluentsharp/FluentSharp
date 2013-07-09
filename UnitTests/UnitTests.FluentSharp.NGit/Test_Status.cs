using FluentSharp;
using FluentSharp.CoreLib;
using NGit.Api;
using NGit.Revwalk;
using NUnit.Framework;

namespace UnitTests.FluentSharp_NGit
{
    [TestFixture]
    public class Test_Status : Temp_Repo
    {
        [Test(Description = "Gets string with lots of detais about the status of the current repo (not committed changes)")]
        public void status()
        {
            var file1            = "TestFile.txt";
            var content1         =  "aaaaa".add_RandomLetters(200);
            var content2         = content1.replace("aaaaa", "bbbbb");            

            var untrackedMessage = "untracked: TestFile.txt\n";
            var addedMessage     = "Added: TestFile.txt\n";
            var changedMessage   = "changed: TestFile.txt\n";
            var removedMessage   = "removed: TestFile.txt\n";
            var modifiedMessage  = "modified: TestFile.txt\n";
            var missingMessage   = "missing: TestFile.txt\n";

            //Untracked
            var status1 = nGit.status();
            nGit.file_Write(file1, content1);
            var status2  = nGit.status();

            //Added
            nGit.add();            
            var status3  = nGit.status();

            //Commit
            var revCommit1 = nGit.commit_using_Status();            
            var status4  = nGit.status();

            Assert.AreEqual(status1     , "");                        
            Assert.AreEqual(status2     , untrackedMessage);
            Assert.AreEqual(status3     , addedMessage);            
            Assert.AreEqual(addedMessage, revCommit1.message());
            Assert.AreEqual(status4     , "");
                                    
            //Modified
            nGit.file_Write(file1, content2);
            var status5 = nGit.status();
            Assert.AreEqual(status5, modifiedMessage);
            nGit.add();

            //Changed
            var status6 = nGit.status();
            Assert.AreEqual(status6, changedMessage);            
            nGit.commit_using_Status();

            //Missing
            nGit.file_Delete(file1);
            var status7 = nGit.status();
            Assert.AreEqual(status7, missingMessage);
            
            //Removed
            nGit.add();
            var status8 = nGit.status();
            Assert.AreEqual(status8, removedMessage);

            //Conflicting
            //TODO: Not sure how to trigger it from here without doing a pull from a clone
            
            //test nulls
            Assert.IsNull ((null as RevCommit).message    ());
            Assert.IsNull ((null as API_NGit ).status     ());
            Assert.IsEmpty((null as Status   ).added      ());
            Assert.IsEmpty((null as Status   ).changed    ());
            Assert.IsEmpty((null as Status   ).conflicting());
            Assert.IsEmpty((null as Status   ).missing    ());
            Assert.IsEmpty((null as Status   ).modified   ());
            Assert.IsEmpty((null as Status   ).untracked  ());
            Assert.IsEmpty((null as Status   ).removed    ());
        }
    }
}
