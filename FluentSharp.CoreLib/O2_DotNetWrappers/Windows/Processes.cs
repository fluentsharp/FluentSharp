// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;



namespace FluentSharp.CoreLib.API
{
    public class Processes
    {
        private static int iMaxProcessExecutionTimeOut = 120000;        

        public static void      killO2s()
        {
            int iCurrentProcess = Process.GetCurrentProcess().Id;
            foreach (Process pProcess in Process.GetProcesses())
            {
                Console.WriteLine(pProcess.ProcessName.Substring(0, 2));
                if (pProcess.ProcessName.Substring(0, 2) == "O2" && pProcess.Id != iCurrentProcess)
                    // find all O2s except the current instance
                {
                    Console.WriteLine("Killing Process {0}", pProcess.ProcessName);
                    pProcess.Kill();
                }
            }
            Process.GetCurrentProcess().Kill(); // end current process
        }
        public static Process   startProcess(String sProcessToStart)
        {
            return startProcess(sProcessToStart, "");
        }
        public static Process   startProcess(String sProcessToStart, String sArguments)
        {
            return startProcess(sProcessToStart, sArguments, false);
        }
        public static Process   startProcess(String sProcessToStart, String sArguments, bool createNoWindow)
        {
            if (sProcessToStart.notValid())
                return null;
            if (sArguments.isNull())
                sArguments = "";
            try
            {
                PublicDI.log.debug("Starting process {0} with arguments {1}", sProcessToStart, sArguments);
                var pProcess = new Process
                    {
                        StartInfo = new ProcessStartInfo
                            {
                                Arguments = sArguments,
                                FileName = sProcessToStart,
                                WorkingDirectory = sProcessToStart.directoryName()
                            }
                    };

                if (createNoWindow)
                {
                    pProcess.StartInfo.CreateNoWindow = true;
                    pProcess.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                }
                pProcess.Start();
                return pProcess;
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex, "in startProcess");
                return null;
            }
        }
        public static String    waitForProcessExitAndGetProcessExitCode(Process pProcess)
        {
            try
            {
                pProcess.WaitForExit(iMaxProcessExecutionTimeOut);
                return pProcess.ExitCode.str();
            }
            catch
            {
                return null;
            }
        }
        public static void      waitForProcessExitAndInvokeCallBankOncompletion(Process pProcess, Callbacks.dMethod_Object dProcessCompletionCallback,  bool bNoExecutionTimeLimit)
        {
            ThreadStart tsThreadStart =
                () => waitForProcessExitAndInvokeCallBankOncompletion_Thread(pProcess, dProcessCompletionCallback,
                                                                             bNoExecutionTimeLimit);
            new Thread(tsThreadStart).Start();
        }
        public static void waitForProcessExitAndInvokeCallBankOncompletion_Thread(Process pProcess,
                                                                                  Callbacks.dMethod_Object
                                                                                      dProcessCompletionCallback,
                                                                                  bool bNoExecutionTimeLimit)
        {
            try
            {
                if (pProcess != null)
                {
                    if (bNoExecutionTimeLimit)
                        pProcess.WaitForExit();
                    else
                        pProcess.WaitForExit(iMaxProcessExecutionTimeOut);
                }
                Callbacks.raiseRegistedCallbacks(dProcessCompletionCallback, new object[] {pProcess});
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex, "In waitForProcessExitAndInvokeCallBankOncompletion:", true);
            }
        }
        public static String startAsCmdExe(String processToStart, String arguments)
        {
            return startProcessAsConsoleApplicationAndReturnConsoleOutput(processToStart, arguments, Path.GetDirectoryName(processToStart), true);
        }
        public static String startAsCmdExe(String processToStart, String arguments, string workingDirectory)
        {
            return startProcessAsConsoleApplicationAndReturnConsoleOutput(processToStart, arguments, workingDirectory, true);
        }

        public static String startProcessAsConsoleApplicationAndReturnConsoleOutput(String processToStart, String arguments)
        {
            return startProcessAsConsoleApplicationAndReturnConsoleOutput(processToStart, arguments,Path.GetDirectoryName(processToStart),  true);
        }
        public static String startProcessAsConsoleApplicationAndReturnConsoleOutput(String processToStart, String arguments, string workingDirectory)
        {
            return startProcessAsConsoleApplicationAndReturnConsoleOutput(processToStart, arguments, workingDirectory, true);
        }
        public static String startProcessAsConsoleApplicationAndReturnConsoleOutput(String processToStart,
                                                                                    String arguments, string workingDirectory, bool showProcessDetails)
        {
            try
            {
                if (showProcessDetails)
                    PublicDI.log.debug("Starting process {0} as a console Application with arguments {1}", processToStart,
                                 arguments);
                var pProcess = new Process();
                pProcess.StartInfo = new ProcessStartInfo();
                pProcess.StartInfo.Arguments = arguments;
                pProcess.StartInfo.FileName = processToStart;
                pProcess.StartInfo.UseShellExecute = false;
                pProcess.StartInfo.WorkingDirectory = workingDirectory;
                pProcess.StartInfo.RedirectStandardOutput = true;
                pProcess.StartInfo.RedirectStandardError = true;
                pProcess.StartInfo.CreateNoWindow = true;

                pProcess.Start();
                // _note that the StandardOutput and StandardError might not show in the correct order in consoleData is just adding one the other othere
                var consoleData = pProcess.StandardOutput.ReadToEnd();
                consoleData += pProcess.StandardError.ReadToEnd();
                return consoleData;
            }
            catch(Exception ex)
            {
                PublicDI.log.error("in startProcessAsConsoleApplicationAndReturnConsoleOutput failed stating process {0} with parameters {1} . The error was: {1} ",processToStart, ex.Message);
                return "";
            }
        }
        public static Process startProcessAsConsoleApplication(String processToStart, String arguments)
        {
            return startProcessAsConsoleApplication(processToStart, arguments, null, null);
        }
        public static Process startProcessAsConsoleApplication(String processToStart, String arguments,
                                                               DataReceivedEventHandler callbackDataReceived)
        {
            return startProcessAsConsoleApplication(processToStart, arguments, callbackDataReceived,
                                                    callbackDataReceived);
        }
        public static Process startProcessAsConsoleApplication(String sProcessToStart, String sArguments,
                                                               DataReceivedEventHandler callbackOutputDataReceived,
                                                               DataReceivedEventHandler callbackErrorDataReceived)
        {
            PublicDI.log.debug("Starting process {0} as a console Application with arguments {1}", sProcessToStart,
                         sArguments);
            var pProcess = new Process();
            pProcess.StartInfo = new ProcessStartInfo();
            pProcess.StartInfo.Arguments = sArguments;
            pProcess.StartInfo.FileName = sProcessToStart;
            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.RedirectStandardOutput = true;
            pProcess.StartInfo.RedirectStandardError = true;
            pProcess.StartInfo.CreateNoWindow = true;

            if (callbackErrorDataReceived == null)
                pProcess.ErrorDataReceived += pProcess_ErrorDataReceived;
            else
                pProcess.ErrorDataReceived += callbackErrorDataReceived;

            if (callbackOutputDataReceived == null)
                pProcess.OutputDataReceived += pProcess_OutputDataReceived;
            else
                pProcess.OutputDataReceived += callbackOutputDataReceived;

            pProcess.Start();
            pProcess.BeginErrorReadLine();
            pProcess.BeginOutputReadLine();

            return pProcess;
        }
        public static Process startProcessAndRedirectIO(string processToStart , Action<string> onDataReceived)
        {
            return startProcessAndRedirectIO(processToStart, "", onDataReceived);
        }

        public static Process startProcessAndRedirectIO(string processToStart, string arguments, Action<string> onDataReceived)
        {
            return startProcessAndRedirectIO(processToStart, arguments, processToStart.directoryName(), onDataReceived, onDataReceived);
        }

        public static Process startProcessAndRedirectIO(string processToStart, string arguments, string workingDirectory,Action<string> onDataReceived)
        {
            return startProcessAndRedirectIO(processToStart, arguments, workingDirectory, onDataReceived, onDataReceived);
        }

        public static Process startProcessAndRedirectIO(string processToStart, string arguments, string workingDirectory,
                                                        Action<string> onOutputDataReceived,
                                                        Action<string> onErrorDataReceived)
        { 
            //var memoryStream = new MemoryStream();
            //var stringWriter = new StringWriter();
            var streamWriter = new StreamWriter(new MemoryStream());
            return startProcessAndRedirectIO(processToStart, 
                                             arguments, 
                                             workingDirectory,
                                             ref streamWriter, 
                                               (sender,e)=>{
                                                            if (e.Data != "")
                                                                onOutputDataReceived(e.Data);
                                                           },
                                               (sender,e)=>{
                                                            if (e.Data != "")
                                                                onOutputDataReceived(e.Data);
                                                           });
    

        }

        public static Process startProcessAndRedirectIO(string processToStart, string arguments, ref StreamWriter processStdInput,
                                                        DataReceivedEventHandler callbackOutputDataReceived,
                                                        DataReceivedEventHandler callbackErrorDataReceived)
        {
            return startProcessAndRedirectIO(processToStart, arguments, Path.GetDirectoryName(processToStart),
                                             ref processStdInput, callbackOutputDataReceived, callbackErrorDataReceived);
        }
        // ReSharper disable RedundantAssignment
        public static Process startProcessAndRedirectIO(string processToStart, string arguments,string workingDirectory, ref StreamWriter processStdInput, 
                                                        DataReceivedEventHandler callbackOutputDataReceived,
                                                        DataReceivedEventHandler callbackErrorDataReceived)
        {            
            PublicDI.log.debug("Starting process {0} as a console Application with IO redirected {1}", processToStart,
                         arguments);
            var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                                    {
                                        Arguments = arguments ?? "",
                                        FileName = processToStart,
                                        WorkingDirectory = workingDirectory,
                                        UseShellExecute = false,
                                        RedirectStandardInput = true,
                                        RedirectStandardOutput = true,
                                        RedirectStandardError = true,
                                        CreateNoWindow = true
                                    }
                };

            //process.StartInfo.EnvironmentVariables.Add("_NT_SYMBOL_PATH",@"srv*c:\symbols*http://msdl.microsoft.com/download/symbols");

            if (callbackErrorDataReceived == null)
                process.ErrorDataReceived += pProcess_ErrorDataReceived;
            else
                process.ErrorDataReceived += callbackErrorDataReceived;

            if (callbackOutputDataReceived == null)
                process.OutputDataReceived += pProcess_OutputDataReceived;
            else
                process.OutputDataReceived += callbackOutputDataReceived;

            //process.StartInfo.EnvironmentVariables["Path"] += ";" + workingDirectory;
            process.Start();
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();                        
            processStdInput = process.StandardInput;
            
            return process;
        }
        // ReSharper restore RedundantAssignment
        public static void pProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != "")
                PublicDI.log.info("\t:{0}:", e.Data);
            //throw new NotImplementedException();
        }
        public static void pProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
                PublicDI.log.error("\t:{0}:", e.Data);
            //throw new NotImplementedException();
        }
        public static void killProcess(String sProcessToKill)
        {
            killProcess(sProcessToKill, false);
        }
        public static void killProcess(String sProcessToKill, bool bVerbose)
        {
            //  foreach (Process pProcess in Process.GetProcesses())
            //      PublicDI.log.debug("Process name :{0}", pProcess.ProcessName);
            Process[] pProcessesToKill = Process.GetProcessesByName(sProcessToKill);
            if (pProcessesToKill.Length == 0 && bVerbose)
                PublicDI.log.error("in killProcess, could not find any proccess with the name: {0}", sProcessToKill);
            foreach (Process pProcess in pProcessesToKill)
            {
                PublicDI.log.debug("Killing Process {0}", pProcess.ProcessName);
                try
                {
                    pProcess.Kill();
                }
                catch (Exception ex)
                {
                    PublicDI.log.error("In killProcess: {0}", ex.Message);
                }
            }
        }
        public static void killProcess(Process processToKill)
        {
            if (processToKill.HasExited == false)
                processToKill.Kill();
        }
        public static bool doesProcessExist(String sProcess)
        {
            return Process.GetProcessesByName(sProcess).Length > 0;
        }
        public static void resetIIS()
        {
            resetIIS(false);
        }
        public static void resetIIS(bool bWaitForCompletion)
        {
            PublicDI.log.debug("Reseting IIS");
            Thread.Sleep(2000); // give the current request some time to complete
            var process = new Process();
            process.StartInfo = new ProcessStartInfo();
            process.StartInfo.Arguments = "/noforce /timeout:300 /restart";
            process.StartInfo.FileName = "c:\\windows\\system32\\iisreset.exe";
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.CreateNoWindow = true;

            process.Start();
            if (bWaitForCompletion)
            {
                process.WaitForExit();
                PublicDI.log.info("IIS service reset");
            }
        }        
        public static void startCurrentProcessInTempFolder()
        {

            String sTargetDirectory = PublicDI.config.TempFolderInTempDirectory + "_" + PublicDI.config.CurrentExecutableFileName;
            Directory.CreateDirectory(sTargetDirectory);
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                if (assembly.Location != "") 
                    Files.copy(assembly.Location, sTargetDirectory);                        
            //String sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            //Files.copyFilesFromDirectoryToDirectory(sCurrentDirectory, sTargetDirectory);
            var sCommandLine = Environment.CommandLine.Replace("\"", "").Replace(".vshost", "");

            var sExeName = sCommandLine.fileName();
            var sExeInTempDirectory = sTargetDirectory.pathCombine(sExeName);
            
            //if there is a .config file, copy it to the temp directory
            var configFile = Path.Combine(PublicDI.config.CurrentExecutableDirectory,
                                          PublicDI.config.CurrentExecutableFileName + ".exe.config");
            if (File.Exists(configFile))
                Files.copy(configFile, sTargetDirectory);

            Process.Start(sExeInTempDirectory);

            Process.GetCurrentProcess().Kill();
        }
        public static String getCurrentProcessName()
        {
            return Process.GetCurrentProcess().ProcessName;
        }
        public static int getCurrentProcessID()
        {
            return Process.GetCurrentProcess().Id;
        }
        public static Process getProcess(int processIdOfProcessToGet)
        {
            try
            {
                return Process.GetProcessById(processIdOfProcessToGet);
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        public static List<Process> getProcesses()
        {
            return new List<Process>(Process.GetProcesses());
        }

        // returns the first one that mathes the name
        public static Process getProcessCalled(string processName)
        {
            foreach (Process process in Process.GetProcessesByName(processName))
                return process;
            return null;
        }

        public static List<Process> getProcessesCalled(string processName)
        {
            return new List<Process>(Process.GetProcessesByName(processName));
        }

        public static string getProcessName(int processId)
        {
            try
            {
                return Process.GetProcessById(processId).ProcessName;
            }
            catch (Exception)
            {
                return null;
            }
        }

/*        public static void reLaunchCurrentProcess()
        {
            try
            {
                Application.Restart();

            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex);
                throw;
            }
        }*/

        public static void Sleep(int miliSecondsToSleep)
        {
            Sleep(miliSecondsToSleep,true);
        }

        public static void Sleep(int miliSecondsToSleep, bool verbose)
        {
            if (miliSecondsToSleep > 0)
            {
                if (verbose)
                    PublicDI.log.info("Sleeping for: {0} mili-seconds", miliSecondsToSleep);
                Thread.Sleep(miliSecondsToSleep);
            }
        }


        public static Process getCurrentProcess()
        {
            return Process.GetCurrentProcess();
        }

        public static List<ProcessThread> getCurrentProcessThreads()
        {
            return getProcessThreads(Process.GetCurrentProcess());
        }

        public static List<ProcessThread> getProcessThreads(Process process)
        {            
            var processThreads = new List<ProcessThread>();           
            foreach (ProcessThread processThread in process.Threads)            
                processThreads.Add(processThread);            
            return processThreads;
        }
    }
}
