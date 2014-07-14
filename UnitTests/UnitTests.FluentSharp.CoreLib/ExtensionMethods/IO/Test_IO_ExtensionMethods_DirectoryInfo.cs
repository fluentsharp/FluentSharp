using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FluentSharp.CoreLib;
using NUnit.Framework;

namespace UnitTests.FluentSharp.CoreLib
{
    [TestFixture]
    public class Test_IO_ExtensionMethods_DirectoryInfo
    {
        [Test(Description="Adds a Deny Write ACL for the User's Security group (to the provided folder)")]
        public void deny_Write_Users()
        {
            var tempDir       = "setAccessControl".tempDir();
            var directoryInfo = tempDir.directoryInfo();

            Assert.IsTrue (tempDir.dirExists());
            Assert.IsTrue (tempDir.canWriteToPath());

            Assert.IsTrue (directoryInfo.deny_Write_Users());
            Assert.IsFalse(tempDir.canWriteToPath());

            Assert.IsTrue (directoryInfo.allow_Write_Users());
            Assert.IsTrue (tempDir.canWriteToPath());

            tempDir.delete_Folder();
            Assert.IsFalse(tempDir.dirExists());

            //check nulls and bad values
            var badDirectoryInfo = 10.randomLetters().directoryInfo();
            Assert.NotNull(badDirectoryInfo);
            Assert.IsFalse(badDirectoryInfo.allow_Write_Users());
            Assert.IsFalse(badDirectoryInfo.deny_Write_Users());
            Assert.IsFalse(badDirectoryInfo.allow_Write(null));
            Assert.IsFalse(badDirectoryInfo.deny_Write(null));
            Assert.IsFalse((null as DirectoryInfo).allow_Write_Users());
            Assert.IsFalse((null as DirectoryInfo).deny_Write_Users());
        }

        [Test(Description="Adds a Deny CreateDirectories ACL for the User's Security group (to the provided folder)")] 
        public void deny_CreateDirectories_Users()
        {
            var tempDir       = "setAccessControl".tempDir();            
            
            var dirToCreate   = tempDir.pathCombine(10.randomLetters());
            var dirToFail     = tempDir.pathCombine(10.randomLetters());

            Assert.IsTrue (tempDir.dirExists());
            Assert.IsFalse(dirToCreate.dirExists());
            Assert.IsFalse(dirToFail.dirExists());

            Assert.AreEqual(dirToCreate, dirToCreate.createDir());            
            Assert.IsTrue  (dirToCreate.dirExists());

            Assert.IsTrue  (tempDir.directoryInfo().deny_CreateDirectories_Users());

            Assert.IsEmpty (dirToFail.createDir());
            Assert.IsFalse (dirToFail.dirExists());

            tempDir    .delete_Folder();
            dirToCreate.delete_Folder();

            Assert.IsFalse(dirToCreate.dirExists());
            Assert.IsFalse(dirToFail.dirExists());

        }
    }
}
