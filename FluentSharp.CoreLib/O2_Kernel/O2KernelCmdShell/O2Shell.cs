// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
//O2File:ShellExecution.cs
//O2File:ShellCommands.cs
//O2File:ShellIO.cs
//O2File:ShellCmdLet.cs

namespace FluentSharp.CoreLib.API
{
    public class O2Shell
    {
        public ShellExecution shellExecution;
        public ShellIO shellIO;

        public O2Shell()
        {
            shellIO = new ShellIO();
            shellExecution = new ShellExecution(shellIO);
            shellIO.writeLine("Welcome to O2's Kernel shell.\n");
            shellIO.writeLine("This is an interactive command prompt into O2's world\n");
            shellIO.writeLine("O2Kernel Process Name: {0}\n\n",PublicDI.O2KernelProcessName);
        }    

        public void startShell()
        {
            bool closeShell = false;
            int commandIndex = 1;
            while (!closeShell)
            {
                shellIO.write("[{0}m:{1}s.{2}ms]  #{3}> ", DateTime.Now.Minute, DateTime.Now.Second,
                              DateTime.Now.Millisecond, commandIndex++);
                string nextCmd = shellIO.readLine();
                if (nextCmd != "")
                {
                    closeShell = (nextCmd == "exit");
                    if (!closeShell && false == shellExecution.execute(nextCmd))
                        break;
                }
            }
        }
    }
}
