using System;
using System.Net;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.NUnit;
using FluentSharp.Web35;
using FluentSharp.Web35.API;
using NUnit.Framework;

namespace UnitTests.FluentSharp.Web.ExtensionMethods
{
    [TestFixture]
    public class Test_O2GitHub_ExtensionMethods_CompileEngine : NUnitTests
    {
        public string tempDownloadDir;

        [SetUp]
        public void setup()
        {
            this.ignore_If_Offline();
            tempDownloadDir = "O2GitHub_Temp_DownloadDir".tempDir();            
            assert_Dir_Exists(tempDownloadDir);
        }
        [TearDown]
        public void teardown()
        {
            Files.deleteAllFilesFromDir(tempDownloadDir);
            
            Files.deleteFolder         (tempDownloadDir);
            assert_Dir_Not_Exists(tempDownloadDir);
        }
        /// <summary>
        /// Thread used to download a file from GitHub (will try a couple locations)
        /// </summary>
        [Test] public void downloadThread()
        {
            // *********************************************
            // first try to do a direct download of the 3 possible locations

            // with url mapped in PublicDI.config.O2GitHub_ExternalDlls
            Action<string,string> test_DirectDownload = (targetAssembly, downloadLocation)=>
                {
                    PublicDI.config.O2GitHub_ExternalDlls.info();
                    assert_Are_Equal(PublicDI.config.O2GitHub_ExternalDlls.HEAD_StatusCode(), HttpStatusCode.BadRequest);            
                                
                    var localFilePath = tempDownloadDir.pathCombine(targetAssembly);
                    var webLocation = "{0}{1}".format(downloadLocation, targetAssembly).trim();

                    assert_File_Not_Exists(localFilePath);                              // file shouln't be there before download
                    assert_True           (webLocation.httpFileExists());               // check that we can ping it

                    new O2Kernel_Web().downloadBinaryFile(webLocation, localFilePath);  // download file

                    assert_File_Exists    (localFilePath);                              // file should be there now        
                    
                    localFilePath.file_Delete();                                        // delete in order to not affect test_downloadThread tests 
                    assert_File_Not_Exists(localFilePath);                              
                };
            
            Action<string, bool> test_downloadThread = (file, shouldExist) =>
                    {
                        var localFilePath = "";

                        var result = O2GitHub.downloadThread(file, ref localFilePath, useCacheInfo : false);
                        
                        assert_Not_Null   (localFilePath);   // this is always set
                        
                        assert_Are_Equal  (shouldExist  , result);
                        assert_Are_Equal  (shouldExist, localFilePath.fileExists() );                        
                    };
            
            test_DirectDownload("GithubSharp.Plugins.LogProviders.SimpleLogProvider.dll", PublicDI.config.O2GitHub_ExternalDlls   ); // 6k - Moq.dll can also be used but it is bigger (400k)
            test_DirectDownload("O2_Kernel_WCF.dll"                                     , PublicDI.config.O2GitHub_Binaries       ); // 20k
            test_DirectDownload("ActiveUp.Net.Dns.dll"                                  , PublicDI.config.O2GitHub_FilesWithNoCode); // 40k            

            test_downloadThread("O2_Kernel_WCF.dll"     , true);
            test_downloadThread("O2_Kernel_AAA.dll"     , false);            
            test_downloadThread("ActiveUp.Net.Dns.dll"  , true);
            test_downloadThread("ActiveUp.AAA.BBB.dll"  , false);
            test_downloadThread("GithubSharp.Plugins.LogProviders.SimpleLogProvider.dll" , true);
            test_downloadThread("GithubSharp.Plugins.LogProviders.AAAAAAAAAAAAAAAAA.dll" , false);
            
            
            
            // *********************************************
            // Then use the download Thread to confirm that we get the same files
        }
        [Test] public void download_Assembly_From_O2_GitHub()      
        {
            var targetAssembly = "GithubSharp.Plugins.LogProviders.SimpleLogProvider"; //"Moq.dll";

            var currentLocation = AssemblyResolver.NameResolver(targetAssembly);       // check if it has been downloaded before and delete it if it had
            if (currentLocation.fileExists())
                currentLocation.file_Delete();
            assert_File_Not_Exists(currentLocation);

            if(targetAssembly.assembly().isNull())  // this test will only run once per appdomain (since after that the assembly is already going to exist in memory)
            {
                var assembly = targetAssembly.download_Assembly_From_O2_GitHub();
                assert_Not_Null(assembly);
                assert_Are_Equal(assembly.name(), targetAssembly);
            }
            var moqAssembly = targetAssembly.assembly();
            assert_Are_Equal(moqAssembly.name() , targetAssembly);
        } 
        [Test] public void register_GitHub_As_ExternalAssemblerResolver()
        {        
            var targetAssembly = "BasicCacher.dll";
            var currentLocation = AssemblyResolver.NameResolver(targetAssembly);       // check if it has been downloaded before and delete it if it had
            if (currentLocation.fileExists())
                currentLocation.file_Delete();
            assert_File_Not_Exists(currentLocation);


            assert_Not_Null(AssemblyResolver.ExternalAssemblerResolver);
            AssemblyResolver.ExternalAssemblerResolver.clear();     

            assert_Is_Empty    (AssemblyResolver.ExternalAssemblerResolver);

            O2GitHub.register_GitHub_As_ExternalAssemblerResolver();

            assert_Is_Not_Empty(AssemblyResolver.ExternalAssemblerResolver);
                   
            assert_Null("System.dll".resolve_Assembly_Using_ExternalAssemblerResolver());
            assert_Not_Null(targetAssembly.resolve_Assembly_Using_ExternalAssemblerResolver());

            assert_Size_Is(AssemblyResolver.ExternalAssemblerResolver,1);

            //calling it again, should not add a duplicate resolver
            O2GitHub.register_GitHub_As_ExternalAssemblerResolver();
            assert_Size_Is(AssemblyResolver.ExternalAssemblerResolver,1);
        }
   }
}
