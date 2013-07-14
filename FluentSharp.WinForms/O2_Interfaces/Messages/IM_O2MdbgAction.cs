// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System.Reflection;

namespace FluentSharp.WinForms.Interfaces
{
    public enum IM_O2MdbgActions 
    {
        startDebugSession,
        endDebugSession,
        breakEvent,
        debugProcessRequest,
        debugMethodInfoRequest,
        commandExecutionMessage, 
        setBreakpointOnFile
    }

    public interface IM_O2MdbgAction : IO2Message
    {
        IM_O2MdbgActions o2MdbgAction { get; set; }
        string filename { get; set; }
        MethodInfo method { get; set; }
        int line { get; set; }
        string loadDllsFrom { get; set; }
        string lastCommandExecutionMessage { get; set; }
            
    }
}