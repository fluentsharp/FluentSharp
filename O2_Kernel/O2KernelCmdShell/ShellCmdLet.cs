// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System.Reflection;

namespace O2.Kernel.O2CmdShell
{
    public class ShellCmdLet
    {
        public MethodInfo methodToExecute { get; set; }
        public string cmdInstruction { get; set; }
        public object[] cmdParameters { get; set; }
        
        //public VoidFunc { get; set; }
        public ShellCmdLet(MethodInfo _methodToExecute, string _cmdInstruction, object[] _cmdParameters)
        {
            methodToExecute = _methodToExecute;
            cmdInstruction = _cmdInstruction;
            cmdParameters = _cmdParameters;
        }
    }
}
