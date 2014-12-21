using FluentSharp.CoreLib;
using FluentSharp.NUnit;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;

namespace UnitTests.FluentSharp.CoreLib
{



    [TestFixture]
    public class Test_IO_ExtensionMethods_Folders : NUnitTests
    {
        private string temporaryFolderPath = null;
        private string randomFilePath = null;
        private string driveLetter = "C";

        [TestFixtureSetUp]
        public void Initialize()
        {
            if (string.IsNullOrEmpty(temporaryFolderPath))
            {
                var randomPath = Path.GetRandomFileName().Replace(".", "");
                temporaryFolderPath = string.Format(@"{0}:\{1}", driveLetter, randomPath);
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
            TestContext.CurrentContext.TestDirectory.folders().count().assert_Is_Not_Equal_To(0);
            TestContext.CurrentContext.WorkDirectory.folders().count().assert_Is_Not_Equal_To(0);
            "".folders().count().assert_Is_Equal_To(0);
            (null as string).folders().count().assert_Is_Equal_To(0);

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
