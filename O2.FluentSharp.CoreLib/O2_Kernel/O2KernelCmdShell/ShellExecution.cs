// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System.Collections.Generic;

namespace FluentSharp.CoreLib.API
{
    public class ShellExecution
    {
        private ShellIO _shellIO;
        //private readonly ShellCommands shellCommands;
        public static bool cmdExecutionEnabled; 

        public ShellExecution(ShellIO shellIO)
        {
            _shellIO = shellIO;
        }

        public bool execute(string cmdToExecute)
        {
           // _shellIO.write("\nExecuting: {0} \n\n", cmdToExecute);
            var shellCmdLet = resolveCmdToExecuteIntoShellCmdLet(cmdToExecute);
            if (shellCmdLet == null)
                _shellIO.writeLine("Error: Could not resolve command: {0}", cmdToExecute);
            else
                execute(shellCmdLet);
            return true;
        }

        private void execute(ShellCmdLet shellCmdLet)
        {            
            var returnData = PublicDI.reflection.invoke(null,shellCmdLet.MethodToExecute, shellCmdLet.CmdParameters);
            _shellIO.lastExecutionResult = (returnData!=null) ? returnData.str() : "";
            if (returnData != null)
                _shellIO.writeLineWithPreAndPostNewLine(returnData.str());
        }

        public ShellCmdLet resolveCmdToExecuteIntoShellCmdLet(string cmdToExecute)
        {
            if (cmdToExecute.Length >0)
            {
                var cmdInstruction = cmdToExecute;
                var cmdParameters = new object[0];                
                if (cmdInstruction.IndexOf(' ') > -1)
                {
                    var splittedCommand = new List<object>();
                    splittedCommand.AddRange(cmdToExecute.Split(' '));
                    cmdInstruction = splittedCommand[0].ToString();
                    splittedCommand.RemoveAt(0);
                    cmdParameters = splittedCommand.ToArray();
                }
                switch (cmdInstruction[0])
                {
                    case ';': // means we are going to pass this directly to cmd.exe                        
                        if (cmdExecutionEnabled)
                        {
                            cmdToExecute = cmdToExecute.Substring(1);
                            var cmdExeParams = new object[] {"cmd.exe", "/c " + cmdToExecute};
                            var o2MethodThatWillStartTheProcess = PublicDI.reflection.getMethod(typeof (O2Kernel_Processes),
                                                                                          "startProcessAsConsoleApplicationAndReturnConsoleOutput",
                                                                                          cmdExeParams);
                            return new ShellCmdLet(o2MethodThatWillStartTheProcess, cmdToExecute, cmdExeParams);
                        }
                        _shellIO.writeLine("Cmd execution is disabled!");
                        break;
                    default:
                        // if it not one of the above first try to get this from the ShellCommands class
                        var methodInfo = PublicDI.reflection.getMethod(typeof(ShellCommands), cmdInstruction, cmdParameters);
                        if (methodInfo != null)
                            return new ShellCmdLet(methodInfo, cmdInstruction, cmdParameters);                        

                        // NOT IMPLEMENTED YET
        /*                // if not try to find it it by type and method name
                        if (cmdInstruction.IndexOf('.') > -1)
                        {
                            var methodTypeName = cmdInstruction.Substring(0,cmdInstruction.LastIndexOf('.'));
                            var methodName = cmdInstruction.Replace(methodTypeName + ".", "");
                            var methodType = PublicDI.reflection.getType("O2_Kernel.dll", methodTypeName);
                            if (methodType != null)
                            {
                                methodInfo = PublicDI.reflection.getMethod(methodType, methodName, cmdParameters);
                            }
                        }
                        */
                        break;
                }
                

            }
            return null;
        }

    }


}
