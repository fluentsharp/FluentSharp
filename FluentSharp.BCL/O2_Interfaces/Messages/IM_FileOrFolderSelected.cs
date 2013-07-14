// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
namespace FluentSharp.WinForms.Interfaces
{
    public interface IM_FileOrFolderSelected : IO2Message
    {
        string pathToFileOrFolder { get; set; }
        int lineNumber { get; set; }
        int columnNumber { get; set; }
        bool showAsError { get; set; }
    }
}