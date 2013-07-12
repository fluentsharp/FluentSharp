// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;

namespace FluentSharp.CoreLib.Interfaces
{
    public interface IZipUtils
    {
        string zipFile(string strFileToZip, string strTargetZipFileName);
        string zipFolder(string strPathOfFolderToZip, string strTargetZipFileName);
        List<String> getListOfFilesInZip(String sZipFileToLoad);
        string unzipFile(string fileToUnzip);
        string unzipFile(string fileToUnzip, string targetFolder);
        List<string> unzipFileAndReturnListOfUnzipedFiles(string fileToUnzip);
        List<string> unzipFileAndReturnListOfUnzipedFiles(string fileToUnzip, string targetFolder);
    }
}