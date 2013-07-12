// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Diagnostics;
using System.Threading;


namespace FluentSharp.CoreLib.API
{
    public class O2Kernel_Processes
    {
        public static String startProcessAsConsoleApplicationAndReturnConsoleOutput(String sProcessToStart,
                                                                                    String sArguments)
        {
            PublicDI.log.debug("Starting process {0} as a console Application with arguments {1}", sProcessToStart,
                         sArguments);
            try
            {
                var pProcess = new Process();
                pProcess.StartInfo = new ProcessStartInfo();
                pProcess.StartInfo.Arguments = sArguments;
                pProcess.StartInfo.FileName = sProcessToStart;
                pProcess.StartInfo.UseShellExecute = false;
                pProcess.StartInfo.RedirectStandardOutput = true;
                pProcess.StartInfo.RedirectStandardError = true;
                pProcess.StartInfo.CreateNoWindow = true;
                pProcess.Start();
                return pProcess.StandardOutput.ReadToEnd();

            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex, "in startProcessAsConsoleApplicationAndReturnConsoleOutput", true);
                return null;
            }
        }

        public static Thread KillCurrentO2Process(int delayBeforeProcessKill)
        {
            return O2Thread.mtaThread(() =>
            {
                Thread.Sleep(delayBeforeProcessKill);
                PublicDI.log.info("KillCurrentO2Process was invoked, so current process will be killed");                
                Process.GetCurrentProcess().Kill();    
            });            
        }

        public static int getCurrentProcessId()
        {
            return Process.GetCurrentProcess().Id;
        }
    }
}
