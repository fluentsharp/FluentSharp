// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System.Reflection;

namespace FluentSharp.CoreLib.API
{
    public class ShellCmdLet
    {
        public MethodInfo MethodToExecute { get; set; }
        public string CmdInstruction { get; set; }
        public object[] CmdParameters { get; set; }
        
        //public VoidFunc { get; set; }
        public ShellCmdLet(MethodInfo methodToExecute, string cmdInstruction, object[] cmdParameters)
        {
            MethodToExecute = methodToExecute;
            CmdInstruction = cmdInstruction;
            CmdParameters = cmdParameters;
        }
    }
}
