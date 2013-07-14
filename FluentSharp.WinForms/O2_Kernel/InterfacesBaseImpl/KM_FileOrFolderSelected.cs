// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using FluentSharp.WinForms.Interfaces;

namespace FluentSharp.WinForms.Utils
{
    public class KM_FileOrFolderSelected : KO2Message, IM_FileOrFolderSelected
    {        
        public string pathToFileOrFolder { get; set; }
        public int lineNumber { get; set; }
        public int columnNumber { get; set; }
        public bool showAsError { get; set; }

        public KM_FileOrFolderSelected(string _pathToFileOrFolder)
        {
            messageText = "KM_FileOrFolderSelected";
            pathToFileOrFolder = _pathToFileOrFolder;
        }        

        public KM_FileOrFolderSelected(string _pathToFileOrFolder, int _lineNumber)
        {
            messageText = "KM_OpenControlInGUI";
            pathToFileOrFolder = _pathToFileOrFolder;
            lineNumber = _lineNumber;
        }

        public KM_FileOrFolderSelected(string _pathToFileOrFolder, int _lineNumber, int _columnNumber, bool _showAsError)
        {
            messageText = "KM_Show_Selected_Text";
            pathToFileOrFolder = _pathToFileOrFolder;
            lineNumber = _lineNumber;
            columnNumber = _columnNumber;
            showAsError = _showAsError;
        }

    }
}
