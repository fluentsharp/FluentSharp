using FluentSharp;
using FluentSharp.CoreLib;
using NUnit.Framework;

namespace UnitTests.FluentSharp_NGit
{
    [TestFixture]
    public class Test_Files : Temp_Repo
    {
        [Test(Description = "Creates random file in repo root (returns filename)")]
        public void file_Create_RandomFile()
        {
            file_FullPath();
        }

        [Test(Description =  "Deleted virtual file in repo root")]
        public void file_Delete()
        {
            var refCommit = nGit.add_and_Commit_Random_File();
            var fileAdded = refCommit.commit_Files(nGit).first();
            var fullPath  = nGit.file_FullPath(fileAdded);
            
            Assert.IsTrue(fullPath.fileExists());
            var result1 = nGit.file_Delete(fileAdded);
            Assert.IsFalse(fullPath.fileExists());
            Assert.IsTrue (result1);
            nGit.add_and_Commit_using_Status();

            nGit.script_Me();

            //Check exeception code coverage
            var commitFiles = nGit.add_and_Commit_Random_File()
                                  .commit_Files_FullPath(nGit);
            Assert.AreEqual(1, commitFiles.size());
            var fileToDelete = commitFiles.first();
            fileToDelete.file_Attribute_ReadOnly_Add();
            nGit.file_Delete(fileToDelete.fileName());
            fileToDelete.file_Attribute_ReadOnly_Remove();

            //Check null error detection            
            var files_Before_Deletes = nGit.Path_Local_Repository.files(true);
            var result2 = (null as API_NGit).file_Delete("");            
            var result3 = nGit.file_Delete((null));
            var files_After_Deletes = nGit.Path_Local_Repository.files(true);

            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.AreEqual(files_Before_Deletes.size(), files_After_Deletes.size());            
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

        [Test(Description = "Creates file in repository root")]
        public void File_Write()
        {
            var fileName = "testFile.txt".add_RandomLetters(10);            
            var fileContents = "This is some Content";
            nGit.file_Write(fileName, fileContents);                                                       
            var firstFileFullPath = nGit.file_FullPath(fileName);
            Assert.AreEqual(fileName, firstFileFullPath.fileName());
            Assert.AreEqual(firstFileFullPath.fileContents(), fileContents);

        }

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

    }
}
