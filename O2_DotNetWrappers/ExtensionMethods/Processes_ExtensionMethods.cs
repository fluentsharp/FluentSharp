using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using O2.Kernel.ExtensionMethods;
using O2.DotNetWrappers.Windows;
using System.IO;

namespace O2.DotNetWrappers.ExtensionMethods
{

    public static class Processes_ExtensionMethods
    {

        public static Process getProcessWithWindowTitle(this string processName, string windowTitle)
        {
            foreach (var process in Processes.getProcessesCalled(processName))
                if (process.MainWindowTitle == windowTitle)
                    return process;
            return null;
        }

        public static Process write(this Process process, string textToSendToStandardInput)
        {
            if (process.StandardInput != null)
                process.StandardInput.WriteLine(textToSendToStandardInput.line());
            return process;
        }

        public static Process stop(this Process process)
        {
            if (process != null)
                if (process.HasExited.isFalse())
                    process.Kill();
            return process;
        }

        // add to main Processes.cs file
        public static Process startConsoleApp(this string processToStart, string arguments, Action<string> consoleOut)
        {
            var pProcess = new Process
            {
                StartInfo =
                {
                    Arguments = arguments,
                    FileName = processToStart,
                    UseShellExecute = false,
                    //RedirectStandardInput  = true,
                    //RedirectStandardOutput = true,
                    //RedirectStandardError = true,
                    //CreateNoWindow = false
                }
            };
            pProcess.EnableRaisingEvents = true;
            pProcess.Exited += (sender, e) => "Process exited".error();

            pProcess.ErrorDataReceived +=
                (sender, e) =>
                {
                    if (e.Data.valid())
                        consoleOut(e.Data);
                };


            pProcess.OutputDataReceived +=
                    (sender, e) =>
                    {
                        if (e.Data.valid())
                            consoleOut(e.Data);
                    };

            pProcess.Start();
            //pProcess.BeginErrorReadLine();
            //pProcess.BeginOutputReadLine();
            //            pProcess.be();
            return pProcess;

            /*return Processes.startProcessAsConsoleApplication(
                processToStart,
                arguments,
                (sender, e) => {
                                    if(e.Data.valid())
                                        consoleOut(e.Data);
                               });	    						   */
        }

        public static Process startConsoleAppAndRedirectInput(this string processToStart, string arguments, Action<string> consoleOut, Action<string> consoleError)
        {
            var memoryStream = new MemoryStream();
            var streamWriter = new StreamWriter(memoryStream);

            var process = Processes.startProcessAndRedirectIO(
                                        processToStart, arguments, ref streamWriter,
                                        (sender, e) =>
                                        {
                                            if (e.Data.valid())
                                                consoleOut(e.Data);
                                        },
                                        (sender, e) =>
                                        {
                                            if (e.Data.valid())
                                                consoleError(e.Data);
                                        });
            //return streamWriter; // not using this since the use the extension method procecess.write(...)
            return process;
        }

        public static List<Process> stop(this List<Process> processesToStop)
        {
            foreach (var process in processesToStop)
                if (process.HasExited == false)
                    process.Kill();
            return processesToStop;
        }	
    }
}
