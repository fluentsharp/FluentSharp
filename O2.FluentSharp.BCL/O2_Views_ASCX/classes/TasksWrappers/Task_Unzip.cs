// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System.Collections.Generic;
using System.IO;
using O2.DotNetWrappers.Zip;
using O2.Views.ASCX;
using O2.Views.ASCX.classes;
using O2.Views.ASCX.classes.Tasks;
using O2.DotNetWrappers.Network;

namespace O2.Views.ASCX.classes.TasksWrappers
{
    public class Task_Unzip : BTask
    {
        public string folderToUnzipFiles;

        public Task_Unzip(string fileToUnzip)
        {
            taskName = "Unzip File";
            sourceObject = fileToUnzip;
        }

        public Task_Unzip(string fileToUnzip, string _folderToUnzipFiles) : this(fileToUnzip)
        {
            folderToUnzipFiles = _folderToUnzipFiles;
        }

        public override bool execute()
        {
            if (sourceObject == null)
                return false;
            var fileToUnzip = (string) sourceObject;
            if (fileToUnzip.IndexOf("http://") > -1)
                fileToUnzip = new Web().downloadBinaryFile(fileToUnzip);
            if (!File.Exists(fileToUnzip))
                return false;
            folderToUnzipFiles = folderToUnzipFiles ?? DI.config.TempFolderInTempDirectory;
            List<string> unzipedFiles = new zipUtils().unzipFileAndReturnListOfUnzipedFiles(fileToUnzip, folderToUnzipFiles);
            if (unzipedFiles.Count == 0)
                return false;
            resultsObject = unzipedFiles;
            return true;
        }
    }
}
