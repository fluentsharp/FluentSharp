using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using FluentSharp.CoreLib;

namespace FluentSharp.WinForms
{
    public static class Processes_ExtensionMethods_WinForms
    {
        public static Process exeConsoleOut(this TextBox textBox, string processToStart)
        {
            return textBox.exeConsoleOut(processToStart, "");
        }
        public static Process exeConsoleOut(this TextBox textBox, string processToStart, string arguments)
        {
            return textBox.startProcessAndShowConsoleOut(processToStart, arguments);
        }
        public static Process startProcessAndShowConsoleOut(this TextBox textBox, string processToStart)
        {
            return textBox.startProcessAndShowConsoleOut(processToStart, "");
        }
        public static Process startProcessAndShowConsoleOut(this TextBox textBox, string processToStart, string arguments)
        {
            textBox.set_Text("");
            return processToStart.startConsoleApp(arguments, (text) => textBox.append_Line(text));
        }
        public static Process exeConsoleOutWithConsoleIn(this TextBox textBox, string processToStart)
        {
            return textBox.exeConsoleOutWithConsoleIn(processToStart, "");
        }
        public static Process exeConsoleOutWithConsoleIn(this TextBox textBox, string processToStart, string arguments)
        {
            return textBox.startProcessMapConsoleOutAndReturnConsoleIn(processToStart, arguments);
        }
        public static Process startProcessMapConsoleOutAndReturnConsoleIn(this TextBox textBox, string processToStart, string arguments)
        {
            return processToStart.startConsoleAppAndRedirectInput(arguments,
                                                                  (text) => textBox.append_Line(text),
                                                                  (text) => textBox.append_Line(text));
        }

        public static Process startH2(this string scriptFile)
        {
            return scriptFile.executeH2_or_O2_in_new_Process();
        }
        public static Process executeH2_or_O2_in_new_Process(this string scriptFile)
        {
            "[executeH2_or_O2_in_new_Process] executing: {0}".info(scriptFile);
            if (scriptFile.fileExists())
                return scriptFile.startProcess();
            else
            {
                scriptFile = scriptFile.local();
                if (scriptFile.fileExists())
                    return scriptFile.startProcess();
            }
            "[executeH2_or_O2_in_new_Process] could not find O2 or H2 script to execute: {0}".error(scriptFile);
            return null;
        }
        public static Process executeH2_as_Admin_askUserBefore(this string scriptName)
        {
            if ("It looks like your current account doesn't have the rights to run this script, do you want to try running this script with full priviledges?".askUserQuestion())
                return scriptName.executeH2_as_Admin();
            return null;
        }
        public static Process executeH2_as_Admin(this string scriptToExecute)
        {
            var process = new Process();
            process.StartInfo.FileName = System.Reflection.Assembly.GetEntryAssembly().location(); //PublicDI.config.CurrentExecutableDirectory.pathCombine("O2 Platform.exe");            
            process.StartInfo.Arguments = "\"{0}\"".format(scriptToExecute);
            process.StartInfo.Verb = "runas";
            process.Start();
            return process;

        }

        public static TreeView add_ProcessModules(this TreeView treeView, Process process)  
        {
            treeView.clear();
            if (process.doWeHaveAccess())
                treeView.add_Nodes(process.Modules.toList<ProcessModule>().Select((m)=>m.ModuleName))
                        .white();
            else
                treeView.pink().add_Node("No Access to Process modules");
            return treeView;
        }
    }
}