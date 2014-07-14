using FluentSharp.CoreLib;
using FluentSharp.Git;
using FluentSharp.Git.APIs;
using FluentSharp.Git.Utils;
using NGit;
using NUnit.Framework;

namespace UnitTests.FluentSharp.Git
{
    [TestFixture]
    class Test_Objects : Temp_Repo
    {
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

        [Test(Description = "Open an object from the current repository based on its SHA1")]
        public void open_Object()
        {
            var fileName        = "test.txt";
            var fileContents_1  = "someText";
            var fileContents_2  = "someText2";
            var expectedSha1_1  = "61447cbfe08f98ece3b0491e9e111bcb3188d79a";
            var expectedSha1_2  = "6b384b15fbeecdd9747f35ff3ce3db4de86c72a4";

            //Before writing fileContents_1
            Assert.IsNull   (nGit.open_Object(expectedSha1_1));
            Assert.IsNull   (nGit.open_Object(expectedSha1_2));

            nGit.file_Create(fileName, fileContents_1);
            nGit.add_and_Commit_using_Status();

            //After writing fileContents_1
            Assert.IsNotNull(nGit.open_Object(expectedSha1_1));
            Assert.IsNull   (nGit.open_Object(expectedSha1_2));

            var objectLoader = nGit.open_Object(expectedSha1_1);

            Assert.NotNull(objectLoader);
            
            //check content 
            var bytes1 = objectLoader.bytes();
            var bytes2 = objectLoader.stream().bytes();
            Assert.AreEqual(bytes1.ascii(), fileContents_1);
            Assert.AreEqual(bytes2.ascii(), fileContents_1);       

            //write fileContents_2
            nGit.file_Create(fileName, fileContents_2);
            nGit.add_and_Commit_using_Status();

            //After writing fileContents_2
            Assert.IsNotNull(nGit.open_Object(expectedSha1_1));
            Assert.IsNotNull(nGit.open_Object(expectedSha1_2));


            //Test exception handing
            Assert.IsNull(nGit.open_Object(null));
            Assert.IsNull(nGit.open_Object(expectedSha1_2.replace("6", "1")));
            Assert.IsNull(nGit.open_Object(expectedSha1_2.replace("6", "-")));
            Assert.IsNull ((null as ObjectLoader).stream());
            Assert.IsEmpty((null as ObjectLoader).bytes());
            Assert.IsEmpty((null as ObjectStream).bytes());

            var badObjectStream = new ObjectStream.SmallStream(0, null);
            Assert.IsEmpty(badObjectStream.bytes());

            var badObjectLoader = new ObjectLoader.SmallObject(0, null);
            Assert.IsEmpty(badObjectLoader.bytes());
        }

        [Test(Description = "Resolve a string sha1 into an ObjectId ")]
        public void resolve()
        {
            var objectId = nGit.repository().resolve(NGit_Consts.EMPTY_SHA1);
            Assert.AreEqual(NGit_Consts.EMPTY_SHA1, objectId.Name);

            Assert.IsNull((null as Repository).resolve(null));            
        }
    }
}
