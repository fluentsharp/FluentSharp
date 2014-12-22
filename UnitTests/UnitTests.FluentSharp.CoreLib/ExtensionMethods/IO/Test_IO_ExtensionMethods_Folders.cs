using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.NUnit;
using NUnit.Framework;
using System.IO;
using System.Security.AccessControl;

namespace UnitTests.FluentSharp.CoreLib
{
    [TestFixture]
    public class Test_IO_ExtensionMethods_Folders : NUnitTests
    {
        private string temporaryFolderPath = null;
        private string randomFilePath = null;
        private string driveLetter = "C";
        private string rootDrive = null;

        [TestFixtureSetUp]
        public void Initialize()
        {
            rootDrive = string.Format(@"{0}:\", driveLetter);
            if (string.IsNullOrEmpty(temporaryFolderPath))
            {
                var randomPath = Path.GetRandomFileName().Replace(".", "");
                temporaryFolderPath = string.Format(@"{0}{1}", rootDrive, randomPath);
            }

            if (!Directory.Exists(temporaryFolderPath))
            {
                // It needs to create directory with specific rights in order to able to delete it after test execution
                DirectorySecurity securityRules = new DirectorySecurity();
                securityRules.AddAccessRule(new FileSystemAccessRule(System.Security.Principal.WindowsIdentity.GetCurrent().Name, FileSystemRights.FullControl, AccessControlType.Allow));
                Directory.CreateDirectory(temporaryFolderPath, securityRules);
            }

            if (string.IsNullOrEmpty(randomFilePath))
            {
                randomFilePath = Path.GetRandomFileName();
            }
        }

        [Test]
        public void folderName()
        {
            temporaryFolderPath.folderName().assert_Not_Null();
            randomFilePath.folderName().assert_Is_Null();
            "".folderName().assert_Is_Null();
            (null as string).folderName().assert_Is_Null();
        }

        [Test]
        public void parentFolder()
        {
            temporaryFolderPath.parentFolder().assert_Is_Not_Null();
            randomFilePath.parentFolder().assert_Is_Empty();
            "".parentFolder().assert_Is_Null();
            (null as string).parentFolder().assert_Is_Null();
        }

        [Test]
        public void parentFolder_Open_in_Explorer()
        {
            temporaryFolderPath.parentFolder_Open_in_Explorer().assert_Is_Not_Null();
            "".parentFolder_Open_in_Explorer().assert_Is_Null();
            (null as string).parentFolder_Open_in_Explorer().assert_Is_Null();
        }

        [Test]
        public void folder()
        {
            temporaryFolderPath.folder("".add_5_RandomLetters()).assert_Is_Null();
            var parentFolder = temporaryFolderPath.parentFolder();
            var folderName = temporaryFolderPath.folderName();
            parentFolder.folder(folderName).assert_Is_Equal_To(temporaryFolderPath);
            (null as string).folder(folderName).assert_Is_Null();
        }

        [Test]
        public void folders()
        {
            temporaryFolderPath.folders().count().assert_Is_Equal_To(0);
            "".folders().count().assert_Is_Equal_To(0);
            (null as string).folders().count().assert_Is_Equal_To(0);

            temporaryFolderPath.folders(true).count().assert_Is_Equal_To(0);
            "".folders(true).count().assert_Is_Equal_To(0);
            (null as string).folders(true).count().assert_Is_Equal_To(0);

            rootDrive.folders(true).count().assert_Is_Not_Equal_To(0);
        }

        [Test]
        public void createDir()
        {
            (null as string).createDir().assert_Is_Null();
            "".createDir().assert_Is_Null();
            "test_createDir".createDir().assert_Is_Equal_To("test_createDir");
            "test_createDir".delete_Folder().assert_True();
        }

        [Test]
        public void createFolder()
        {
            (null as string).createFolder().assert_Is_Null();
            "".createFolder().assert_Is_Null();
            "test_createDir".createFolder().assert_Is_Equal_To("test_createDir");
            "test_createDir".delete_Folder().assert_True();
        }

        [Test]
        public void directoryName()
        {
            (null as string).directoryName().assert_Is_Empty();
            "".directoryName().assert_Is_Empty();
            temporaryFolderPath.directoryName().assert_Is_Equal_To(temporaryFolderPath.parentFolder());
            rootDrive.directoryName().assert_Is_Null();
        }

        [Test]
        public void isFolder()
        {
            (null as string).isFolder().assert_Is_Not_True();
            "".isFolder().assert_Is_Not_True();
            temporaryFolderPath.isFolder().assert_Is_True();
            rootDrive.isFolder().assert_Is_True();
            randomFilePath.isFolder().assert_Is_Not_True();
        }

        [Test]
        public void folder_Create_File()
        {
            var filename = "text.txt";
            var content = "Lorem ipsum dolor";
            temporaryFolderPath.folder_Create_File(filename, content).assert_Is_Not_Null();
            "".folder_Create_File(filename, content).assert_Is_Null();
            (null as string).folder_Create_File(filename, content).assert_Is_Null();
            randomFilePath.folder_Create_File(filename, content).assert_Is_Null();
            //cleanup
            temporaryFolderPath.folder_Delete_Files();
        }

        [Test]
        public void folder_Delete_Files()
        {
            var filename = "text.txt";
            var content = "Lorem ipsum dolor";

            (null as string).folder_Delete_Files().assert_Is_False();
            "".folder_Delete_Files().assert_Is_False();
            temporaryFolderPath.folder_Delete_Files().assert_Is_False();
            temporaryFolderPath.folder_Create_File(filename, content);
            temporaryFolderPath.folder_Delete_Files().assert_Is_True();
            randomFilePath.folder_Delete_Files().assert_Is_False();
        }

        [Test]
        public void folder_Exists()
        {
            (null as string).folder_Exists().assert_Is_False();
            "".folder_Exists().assert_Is_False();
            temporaryFolderPath.folder_Exists().assert_Is_True();
            randomFilePath.folder_Delete_Files().assert_Is_False();
            rootDrive.folder_Exists().assert_Is_True();
        }

        [Test]
        public void folder_Not_Exists()
        {
            (null as string).folder_Not_Exists().assert_Is_True();
            "".folder_Not_Exists().assert_Is_True();
            temporaryFolderPath.folder_Not_Exists().assert_Is_False();
            randomFilePath.folder_Delete_Files().assert_Is_False();
            rootDrive.folder_Not_Exists().assert_Is_False();
        }

        [Test]
        public void folderExists()
        {
            (null as string).folderExists().assert_Is_False();
            "".folderExists().assert_Is_False();
            temporaryFolderPath.folderExists().assert_Is_True();
            randomFilePath.folder_Delete_Files().assert_Is_False();
            rootDrive.folderExists().assert_Is_True();
        }

        [Test]
        public void dirExists()
        {
            (null as string).dirExists().assert_Is_False();
            "".dirExists().assert_Is_False();
            temporaryFolderPath.dirExists().assert_Is_True();
            randomFilePath.folder_Delete_Files().assert_Is_False();
            rootDrive.dirExists().assert_Is_True();
        }

        [Test]
        public void dirs()
        {
            temporaryFolderPath.dirs().count().assert_Is_Equal_To(0);
            "".dirs().count().assert_Is_Equal_To(0);
            (null as string).dirs().count().assert_Is_Equal_To(0);
            rootDrive.folders().count().assert_Is_Not_Equal_To(0);
        }

        [Test]
        public void directoryInfo()
        {
            "".directoryInfo().assert_Is_Null();
            (null as string).directoryInfo().assert_Is_Null();
            temporaryFolderPath.directoryInfo().assert_Is_Not_Null();
            rootDrive.directoryInfo().assert_Is_Not_Null();
            randomFilePath.directoryInfo().assert_Is_Null();
        }

        [Test]
        public void mapPath()
        {
            temporaryFolderPath.mapPath(randomFilePath).assert_Is_Not_Null();
            driveLetter.mapPath(randomFilePath).assert_Is_Not_Null();
            "".mapPath(randomFilePath).assert_Is_Null();
            (null as string).mapPath(randomFilePath).assert_Is_Null();
            randomFilePath.mapPath(randomFilePath).assert_Is_Null();
            randomFilePath.mapPath(driveLetter).assert_Is_Null();
        }

        [Test]
        public void temp_Folder()
        {
            temporaryFolderPath.temp_Folder().assert_Is_Not_Null().folder_Delete().assert_Is_True();
            rootDrive.temp_Folder().assert_Is_Not_Null().folder_Delete().assert_Is_True();
            (null as string).temp_Folder().assert_Equal(PublicDI.config.O2TempDir);
            ("").temp_Folder().assert_Equal(PublicDI.config.O2TempDir);
        }

        [Test]
        public void temp_Dir()
        {
            temporaryFolderPath.temp_Dir().assert_Is_Not_Null().folder_Delete().assert_Is_True();
            rootDrive.temp_Dir().assert_Is_Not_Null().folder_Delete().assert_Is_True();
            (null as string).temp_Dir().assert_Equal(PublicDI.config.O2TempDir);
            ("").temp_Dir().assert_Equal(PublicDI.config.O2TempDir);
        }

        [TestFixtureTearDown]
        public void CleanUp()
        {
            if (!string.IsNullOrEmpty(temporaryFolderPath))
            {
                if (Directory.Exists(temporaryFolderPath))
                {
                    Directory.Delete(temporaryFolderPath, true);
                }
            }
        }
    }
}