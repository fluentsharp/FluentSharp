using System;
using System.IO;
using FluentSharp.CoreLib.API;

namespace FluentSharp.REPL.Utils
{
    public class GacUtils_Zip
    {            
        public static void backupGac()
        {
            backupGac(PublicDI.config.getTempFileInTempDirectory("zip"));
        }

        public static void backupGac(string zipFileToSaveGacContents)
        {
            var pathToGac = Path.Combine(Environment.GetEnvironmentVariable("windir") ?? "", "Assembly");//\\GAC_MSIL");                        
            O2Thread.mtaThread(
                () =>
                {
                    PublicDI.log.info("Started unzip process of Gac Folder");
                    var timer = new O2Timer("Gac Backup").start();
                    //new zipUtils().zipFolder(PublicDI.PathToGac, zipFileToSaveGacContents);
                    new zipUtils().zipFolder(pathToGac, zipFileToSaveGacContents);
                    var logMessage = String.Format("Contents of \n\n\t{0}\n\n saved to \n\n\t{1}\n\n ", pathToGac, zipFileToSaveGacContents);
                    timer.stop();
                    PublicDI.log.info(logMessage);
                    //PublicDI.log.showMessageBox(logMessage);
                });
        }

    }
}
