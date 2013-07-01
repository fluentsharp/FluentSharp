using FluentSharp.ExtensionMethods;
using NUnit.Framework;

namespace UnitTests.FluentSharp_NGit
{
    [TestFixture]
    public class Test_Files : Temp_Repo
    {
        [Test(Description = "Gets list of repository files")]
        public void files()
        {            
            nGit.add_and_Commit_using_Status();                     // in case there are files in there that were not commited
            
            var fileName1 = "1st file.txt".add_RandomLetters();
            var fileName2 = "2nd file.txt".add_RandomLetters();
            nGit.file_Write(fileName1, "With Some content");
            nGit.file_Write(fileName2, "With Some content");

            var filesBeforeCommit = nGit.files().size();

            nGit.add_and_Commit_using_Status();
            
            var repoFiles = nGit.files();            

            Assert.IsNotEmpty(repoFiles);
            Assert.AreEqual  (repoFiles.size(), filesBeforeCommit + 2);
            Assert.IsTrue    (repoFiles.contains(fileName1));            
            Assert.IsTrue    (repoFiles.contains(fileName2));            
        }

        [Test(Description = "Returns the full path to a repo's virtualPath")]
        public void file_FullPath()
        {
            var fileName = nGit.file_Create_Random_File();
            var fullPath = nGit.file_FullPath(fileName);
            Assert.IsNotNull(fileName);
            Assert.IsNotNull(fullPath);
            Assert.IsTrue   (fullPath.fileExists());
            Assert.AreEqual (fileName.extension(), ".txt");
            Assert.AreEqual (fullPath.extension(), ".txt");
            Assert.AreEqual (fullPath, nGit.Path_Local_Repository.pathCombine(fileName));
            Assert.IsNull   (nGit.file_FullPath(null));
        }

        [Test(Description = "Creates random file in repo root (returns filename)")]
        public void file_Create_RandomFile()
        {
            file_FullPath();
        }

        [Test(Description = "Creates file in repository root")]
        public void write_File()
        {
            var fileName = "testFile.txt".add_RandomLetters(10);            
            var fileContents = "This is some Content";
            nGit.file_Write(fileName, fileContents);                                                       
            var firstFileFullPath = nGit.file_FullPath(fileName);
            Assert.AreEqual(fileName, firstFileFullPath.fileName());
            Assert.AreEqual(firstFileFullPath.fileContents(), fileContents);

        }


    }
}
