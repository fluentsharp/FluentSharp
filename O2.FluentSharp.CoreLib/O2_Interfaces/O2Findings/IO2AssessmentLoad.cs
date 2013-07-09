// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
namespace FluentSharp.CoreLib.Interfaces
{
    public interface IO2AssessmentLoad
    {
        string engineName { get; set; }
        bool canLoadFile(string fileToTryToLoad);
        IO2Assessment loadFile(string fileToLoad);
        bool importFile(string fileToLoad, IO2Assessment o2Assessment);
    }
}