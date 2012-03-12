// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.IO;
using Ionic.Zip;
using O2.Kernel.ExtensionMethods;
using O2.DotNetWrappers;
using O2.DotNetWrappers.Windows;
using O2.Interfaces.Utils;

namespace O2.DotNetWrappers.Zip
{    

    public class zipUtils : IZipUtils
    {
        public string zipFile(string strFileToZip, string strTargetZipFileName)
        {
            var zpZipFile = new ZipFile(strTargetZipFileName);
            zpZipFile.AddFile(strFileToZip, "");
            //zpZipFile.TrimVolumeFromFullyQualifiedPaths = true;            
            zpZipFile.Save();
            zpZipFile.Dispose();
            return strTargetZipFileName;
        }

        public string zipFolder(string strPathOfFolderToZip, string strTargetZipFileName)
        {
            var zpZipFile = new ZipFile(strTargetZipFileName);
            zpZipFile.AddDirectory(strPathOfFolderToZip, "");
            //zpZipFile.TrimVolumeFromFullyQualifiedPaths = true;
            zpZipFile.Save();
            zpZipFile.Dispose();
            return strTargetZipFileName;
        }

        public List<String> getListOfFilesInZip(String sZipFileToLoad)
        {
            var lsFilesInZip = new List<string>();
            var zpZipFile = new ZipFile(sZipFileToLoad);

            Object oZipEntries = DI.reflection.getFieldValue("_entries", zpZipFile);
            if (oZipEntries != null)
            {
                var lzeZipEntries = (List<ZipEntry>) oZipEntries;
                //foreach (Object asd in zpZipFile.GetEnumerator())
                foreach (ZipEntry zpZipEntry in lzeZipEntries)
                {
                    lsFilesInZip.Add(zpZipEntry.FileName);
                }
            }
            return lsFilesInZip;
        }


        /*    // this code snippet was based on the code from http://www.eggheadcafe.com/articles/20050821.asp
        public static void zipFolder(string strPathOfFolderToZip, string strTargetZipFileName)
        {            			
            ArrayList ar = files.returnPathToAllFilesInFolder_Recursively(strPathOfFolderToZip);
            int TrimLength = (Directory.GetParent(strPathOfFolderToZip)).ToString().Length;
            TrimLength += 1; //remove '\'			
            FileStream ostream;
            byte[] obuffer;
            ZipOutputStream oZipStream = new ZipOutputStream(System.IO.File.Create(strTargetZipFileName));	// create zip stream
            oZipStream.SetLevel(9);	// 9 = maximum compression level

            ZipEntry oZipEntry;
            foreach (string Fil in ar) // for each file, generate a zipentry
            {
                oZipEntry = new ZipEntry(Fil.Remove(0, TrimLength));
                oZipStream.PutNextEntry(oZipEntry);
                if (!Fil.EndsWith(@"/")) // if a file ends with '/' its a directory
                {
                    ostream = File.OpenRead(Fil);
                    obuffer = new byte[ostream.Length];
                    // byte buffer
                    ostream.Read(obuffer, 0, obuffer.Length);
                    oZipStream.Write(obuffer, 0, obuffer.Length);
                    Console.Write(".");
                    ostream.Close();
                }
            }
            oZipStream.Finish();
            oZipStream.Close();
        }
*/        

        public string unzipFile(string fileToUnzip)
        {
            string tempFolder = DI.config.TempFolderInTempDirectory + "_" +
                                Path.GetFileNameWithoutExtension(fileToUnzip);
            return unzipFile(fileToUnzip, tempFolder);
        }

        public string unzipFile(string fileToUnzip, string targetFolder)
        {
           // targetFolder = "C:\\O2\\_tempDir\\tmp10BA_aa";
            try
            {
                DI.log.info("UnZiping file {0} into folder {1}", fileToUnzip, targetFolder);
                Files.checkIfDirectoryExistsAndCreateIfNot(targetFolder);
                var zpZipFile = new ZipFile(fileToUnzip);
                //zpZipFile.ExtractAll(targetFolder, true);
                zpZipFile.ExtractExistingFile = ExtractExistingFileAction.OverwriteSilently;
                //var test = zpZipFile.OutputUsedZip64;            
                zpZipFile.ExtractAll(targetFolder);
                zpZipFile.Dispose();
                return targetFolder;
            }
            catch (Exception ex)
            { 
                ex.log("in unzipFile: {0} -> {1}".format(fileToUnzip, targetFolder));
                return "";
            }
        }

        public List<string> unzipFileAndReturnListOfUnzipedFiles(string fileToUnzip)
        {
            return Files.getFilesFromDir_returnFullPath(unzipFile(fileToUnzip));
        }

        public List<string> unzipFileAndReturnListOfUnzipedFiles(string fileToUnzip, string targetFolder)
        {
            if ("" != Files.checkIfDirectoryExistsAndCreateIfNot(targetFolder))
                return Files.getFilesFromDir_returnFullPath(unzipFile(fileToUnzip, targetFolder),"*.*",true);
            return null;
        }


       
    }
}
