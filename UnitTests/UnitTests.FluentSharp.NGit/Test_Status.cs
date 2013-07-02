using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentSharp.ExtensionMethods;
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
            var modifiedMessage  = "modified: TestFile.txt\n";

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

            //Deleted
            nGit.file_Delete(file1);

            Assert.AreEqual(status5, modifiedMessage);

            //Assert.AreEqual(""              , status3);
            
            //test nulls
            Assert.IsNull((null as RevCommit).message());
        }
    }
}
