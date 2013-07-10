using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentSharp.AspNet_MVC;
using FluentSharp.CoreLib;
using FluentSharp.REPL;
using NUnit.Framework;

namespace UnitTests.FluentSharp_AspNet_MVC
{
    [TestFixture]
    public class Test_Cassini_Host : Temp_Cassini_Site
    {
        [Test]
        public void Host_Object()
        {

            var host = server.invoke("GetHost");
            //Server.script_Me();
         //   Assert.IsNotNull(host);
            //
        }

        [Test]
        public void Get_File()
        {
            Action<string,string,string> checkFileViaHttp = 
                (fileName,fileContents, expectedResponse) =>
                    {
                        var filePath = webRoot.pathCombine(fileName);
                        Assert.IsFalse(filePath.fileExists());
                        if (fileContents.valid())
                        {
                            fileContents.saveAs(filePath);
                            Assert.IsTrue(filePath.fileExists());
                        }
                        var fileUrl = apiCassini.url() + fileName;
                        var html    = fileUrl.html();
                        Assert.AreEqual(expectedResponse, html);
                        filePath.file_Delete();
                        Assert.IsFalse(filePath.fileExists());                
                    };
            
            checkFileViaHttp("testFile.txt", "", "");
            checkFileViaHttp("testFile.txt", "Some contents", "Some contents");            
            checkFileViaHttp("testFile.aspx", "Some contents", "Some contents");
            checkFileViaHttp("testFile.aspx", 
                             "<%=\"Hello from ASPX\"%>", 
                             "Hello from ASPX");

        }
    }
}
