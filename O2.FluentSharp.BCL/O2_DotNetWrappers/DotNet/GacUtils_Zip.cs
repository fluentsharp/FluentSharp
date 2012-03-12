using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using O2.Interfaces.DotNet;
using O2.Kernel;
using O2.DotNetWrappers.Windows;
using O2.Kernel.InterfacesBaseImpl;
using O2.Kernel.ExtensionMethods;
using O2.DotNetWrappers.Zip;
using O2.DotNetWrappers.ExtensionMethods;

namespace O2.DotNetWrappers.DotNet
{
    public class GacUtils_Zip
    {            
        public static void backupGac()
        {
            backupGac(PublicDI.config.getTempFileInTempDirectory("zip"));
        }

        public static void backupGac(string zipFileToSaveGacContents)
        {
            O2Thread.mtaThread(
                () =>
                {
                    PublicDI.log.info("Started unzip process of Gac Folder");
                    var timer = new O2Timer("Gac Backup").start();
                    new zipUtils().zipFolder(DI.PathToGac, zipFileToSaveGacContents);
                    var logMessage = String.Format("Contents of \n\n\t{0}\n\n saved to \n\n\t{1}\n\n ", DI.PathToGac, zipFileToSaveGacContents);
                    timer.stop();
                    PublicDI.log.info(logMessage);
                    //PublicDI.log.showMessageBox(logMessage);
                });
        }

    }
}
