// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System.Reflection;
using FluentSharp.WinForms.Interfaces;

namespace FluentSharp.WinForms.Utils
{
    class KM_O2MdbgAction : KO2Message, IM_O2MdbgAction
    {
        public IM_O2MdbgActions o2MdbgAction { get; set; }
        public string filename { get; set; }
        public MethodInfo method { get; set; }
        public int line { get; set; }
        public string loadDllsFrom { get; set; }
        public string lastCommandExecutionMessage { get; set; }                
    }
}
