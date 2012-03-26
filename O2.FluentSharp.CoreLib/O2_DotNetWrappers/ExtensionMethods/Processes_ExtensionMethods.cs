using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using O2.DotNetWrappers.Windows;
using System.IO;
using O2.DotNetWrappers.DotNet;
using O2.Kernel;

namespace O2.DotNetWrappers.ExtensionMethods
{

    public static class Processes_ExtensionMethods
    {
        public static Process       getProcessWithWindowTitle(this string processName, string windowTitle)
        {
            foreach (var process in Processes.getProcessesCalled(processName))
                if (process.MainWindowTitle == windowTitle)
                    return process;
            return null;
        }
        public static Process       write(this Process process, string textToSendToStandardInput)
        {
            if (process.StandardInput != null)
                process.StandardInput.WriteLine(textToSendToStandardInput.line());
            return process;
        }
        public static Process       stop(this Process process)
        {
            if (process != null)
                if (process.HasExited.isFalse())
                    process.Kill();
            return process;
        }        
        public static Process       startConsoleApp(this string processToStart, string arguments, Action<string> consoleOut)
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
        public static Process       startConsoleAppAndRedirectInput(this string processToStart, string arguments, Action<string> consoleOut, Action<string> consoleError)
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
        public static string        startProcess_getConsoleOut(this string processExe)
		{
			return processExe.startProcess_getConsoleOut("");
		}		
		public static string        startProcess_getConsoleOut(this string processExe, string arguments)
		{
			return Processes.startProcessAsConsoleApplicationAndReturnConsoleOutput(processExe, arguments);
		}		
		public static string        startProcess_getConsoleOut(this string processExe, string arguments, string workingDirectory)
		{
			return Processes.startAsCmdExe(processExe, arguments, workingDirectory);
		}				
		public static Process       startProcess(this string processExe, Action<string> onDataReceived)
		{
			return processExe.startProcess("", onDataReceived);
		}		
		public static Process       startProcess(this string processExe, string arguments, Action<string> onDataReceived)
		{
			return Processes.startProcessAndRedirectIO(processExe, arguments, onDataReceived);			
		}		
		public static Process       startProcess(this string processExe, string arguments)
		{
			return Processes.startProcess(processExe, arguments);
		}		
		public static Process       startProcess(this string processExe)
		{			
			return Processes.startProcess(processExe);
		}		
		public static Process       close(this Process process)
		{
			return process.stop();
		}		
		public static Process       closeInNSeconds(this Process process, int seconds)
		{
			O2Thread.mtaThread(
				()=>{
						process.sleep(seconds*1000);
						"Closing Process:{0}".info(process.ProcessName);
						process.stop();
					});
			return process;
		}		
    }

    public static class Console_ExtensionMethods
    { 
    	public static MemoryStream capture_Console(this string firstLine)
		{
			var memoryStream = new MemoryStream();
			memoryStream.capture_Console();  
			Console.WriteLine(firstLine);
			return memoryStream;
		}
		public static MemoryStream capture_Console(this MemoryStream memoryStream)
		{
			return memoryStream.capture_ConsoleOut()
							   .capture_ConsoleError(); 
		}
		public static MemoryStream capture_ConsoleOut(this MemoryStream memoryStream)
		{
			var streamWriter = new StreamWriter(memoryStream);
			System.Console.SetOut(streamWriter);
			streamWriter.AutoFlush = true;
			return memoryStream;
		}		
		public static MemoryStream capture_ConsoleError(this MemoryStream memoryStream)
		{
			var streamWriter = new StreamWriter(memoryStream);
			System.Console.SetError(streamWriter);
			streamWriter.AutoFlush = true;
			return memoryStream;
		}		
		public static string readToEnd(this MemoryStream memoryStream)
		{
			memoryStream.Position =0;
			return new StreamReader(memoryStream).ReadToEnd();
		}
    }
}
