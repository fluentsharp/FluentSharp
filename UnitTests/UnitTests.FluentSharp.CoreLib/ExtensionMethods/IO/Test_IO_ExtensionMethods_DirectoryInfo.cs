using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FluentSharp.CoreLib;
using NUnit.Framework;
using FluentSharp.CoreLib.API;
using FluentSharp.NUnit;

namespace UnitTests.FluentSharp.CoreLib
{
    [TestFixture]
    public class Test_IO_ExtensionMethods_DirectoryInfo
    {
		[TestFixtureSetUp]
		public void setup()
		{
			if (clr.mono())		
				"ignoring tests since we are on mono".assert_Ignore();
		}

        [Test(Description="Adds a Deny Write ACL for the User's Security group (to the provided folder)")]
        public void deny_Write_Users()
        {
            var tempDir       = "setAccessControl".tempDir();
            var directoryInfo = tempDir.directoryInfo();

            tempDir      .assert_Folder_Exists()
                         .canWriteToPath().assert_True();

            directoryInfo.deny_Write_Users().assert_True();
            tempDir      .canWriteToPath().assert_False();

            directoryInfo.allow_Write_Users().assert_True();
            tempDir      .canWriteToPath()   .assert_True();

            tempDir.delete_Folder().assert_True();
            tempDir                .assert_Folder_Not_Exists();

            //check nulls and bad values
            Assert.IsFalse(directoryInfo.allow_Write(null));
            Assert.IsFalse(directoryInfo.deny_Write(null));

            var badDirectoryInfo = 10.randomLetters().directoryInfo();          //directoryInfo returns null if the folder doesn't exist
            badDirectoryInfo.assert_Is_Null();
            Assert.IsFalse(badDirectoryInfo.allow_Write_Users());
            Assert.IsFalse(badDirectoryInfo.deny_Write_Users());            
        }

        [Test(Description="Adds a Deny CreateDirectories ACL for the User's Security group (to the provided folder)")] 
        public void deny_CreateDirectories_Users()
        {
            var tempDir       = "setAccessControl".tempDir();                               // Creates temp Directory
            
            var dirToCreate   = tempDir.pathCombine(10.randomLetters());                    // sub folders inside tempDir       
            var dirToFail     = tempDir.pathCombine(10.randomLetters());

            tempDir    .assert_Folder_Exists();                                             // confirmt that tempDir exists and the others don't 
            dirToCreate.assert_Folder_Not_Exists();
            dirToFail  .assert_Folder_Not_Exists();

            dirToCreate.assert_Is(dirToCreate.createDir());                                 // create dirToCreate
            dirToCreate.assert_Folder_Exists();

            tempDir    .directoryInfo().deny_CreateDirectories_Users().assert_Is_True();    // change CreateDirectories priv on tempDir

            dirToFail  .createDir().assert_Is_Null();                                       // try to create dirToFail
            dirToFail  .assert_Folder_Not_Exists();                                         // confirm it was not created

            tempDir    .directoryInfo().allow_Write_Users();                                // reset CreateDirectories priv

            dirToCreate.assert_Folder_Deleted();                                            // delete folders created
            tempDir    .assert_Folder_Deleted();
        }
    }
}
