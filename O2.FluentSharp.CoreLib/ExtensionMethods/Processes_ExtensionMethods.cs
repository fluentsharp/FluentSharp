using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using O2.DotNetWrappers.Windows;
using System.IO;
using O2.DotNetWrappers.DotNet;
using System.Runtime.InteropServices;

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
            if (process.StandardInput.notNull())
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
            var pProcess = new Process();
            pProcess.StartInfo = new ProcessStartInfo();
            pProcess.StartInfo.Arguments = arguments;
            pProcess.StartInfo.FileName = processToStart;
            pProcess.StartInfo.UseShellExecute = false;                             
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

        public static Process               with_Name(this List<Process> processes, string name)
        {
            return (from process in processes
                    where process.ProcessName == name
                    select process).first();
        }
        public static Process               with_Id(this List<Process> processes, int id)
        {
            return (from process in processes
                    where process.Id == id
                    select process).first();
        }        
        public static List<string>          names(this List<ProcessModule> modules)
        {
            return modules.Select((module)=> module.ModuleName).toList();
        }		
        public static Process               process_WithId(this int id)
        {
            return Processes.getProcess(id);
        }
        public static Process               process_WithName(this string name)
        {
            return Processes.getProcessCalled(name);
        }
        public static Process               process_MainWindow_BringToFront(this Process process)
        {
            if (process.MainWindowHandle != IntPtr.Zero)
                "WindowsBase.dll".assembly()
                                .type("UnsafeNativeMethods")					 
                                .invokeStatic("SetForegroundWindow",new HandleRef(null, process.MainWindowHandle)) ;
            else
                "[process_MainWindow_BringToFront] provided process has no main Window".error();
            return process;
        }

        public static bool                              doWeHaveAccess(this Process process)
        {
            try
            {
                return process.Modules.notNull();                
            }
            catch
            {
                return false;
            }
        }
        public static List<ProcessModule>               modules(this Process process)
        {
            var modules = new List<ProcessModule>();
            try
            {		
                foreach(ProcessModule module in process.Modules)
                    modules.Add(module);				
            }
            catch(Exception ex)
            {
                ex.log();				
            }
            return modules;
        }
        public static Dictionary<string,ProcessModule>  modules_Indexed_by_ModuleName(this Process process)
        {
            //return process.modules().ToDictionary((module)=> module.ModuleName.lower());;		 //doesn't handle duplicate names
            var modulesIndexed = new Dictionary<string,ProcessModule>();
            foreach(var module in process.modules())
                modulesIndexed.add(module.ModuleName.lower(), module);
            return modulesIndexed;
        }		
        public static Dictionary<string,ProcessModule>  modules_Indexed_by_FileName(this Process process)
        {			
            var modulesIndexed = new Dictionary<string,ProcessModule>();
            foreach(var module in process.modules())
                modulesIndexed.add(module.FileName.lower(), module);
            return modulesIndexed;
        }		
        public static bool                              processHasModule(this Process process, string moduleName)
        {
            if (process.doWeHaveAccess().isFalse())
            {
                "A call was made to processHasModule for the process {0} which the current user doesn't have access to".error(process.ProcessName);
                return false;
            }
            
            foreach (ProcessModule module in process.Modules)
                if (module.ModuleName.fileName().contains(moduleName))
                    return true;        
            return false;
        }       
        public static Process                           startProcess_AsAdmin(this string pathToExe)
        {
             
            var process = new Process();
            process.StartInfo.FileName  = pathToExe;          
            process.StartInfo.Verb = "runas";
            process.Start();
            return process;
        }

        public static string    process_Id_and_Name(this Process process)
        {
            return (process.notNull())
                ? "{0} : {1}".format(process.Id, process.ProcessName)
                : "";
        }	    
        public static Process   waitFor_MainWindowHandle(this Process process)
        {
            "Waiting for MainWindowHandle for process: {0}".info(process.process_Id_and_Name());
            250.sleep_While(()=> process.refresh().MainWindowHandle == IntPtr.Zero);
            
            "Got for MainWindowHandle: {0}".info(process.MainWindowHandle);
            return process;
        }	    
        public static Process   waitFor_2nd_MainWindowHandle(this Process process)
        {
            var firstMainWindowHandle = process.waitFor_MainWindowHandle()
                                                .MainWindowHandle;
            "Waiting for 2nd MainWindowHandle for process: {0}".info(process.process_Id_and_Name());
            250.sleep_While(()=>process.refresh().MainWindowHandle == firstMainWindowHandle);
            "Got 2nd MainWindowHandle: {0}".info(process.MainWindowHandle);
            return process;
        }
        public static Process   refresh(this Process process)
        {
            if (process.notNull())
                process.Refresh();
            return process;
        }
        public static Process   end(this Process process)
        {
            process.Kill();
            return process;
        }
        public static Process   stop_in_NSeconds(this Process process, int seconds)
        {
            O2Thread.mtaThread(
                ()=>{
                        "Stopping process {0} in {1} seconds".debug(process.process_Id_and_Name(), seconds);
                        (seconds * 1000).sleep();
                        process.stop();
                    });
            return process;
        }    	    	    	
        public static void      sleep_While(this int miliseconds, Func<bool> whileCondition)
        {
            while(whileCondition())
                miliseconds.sleep();
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
