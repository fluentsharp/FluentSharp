using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//O2File:ascx_SimpleTaskGui.cs

namespace O2.XRules.Database._Rules.APIs.Tasks
{
    public static class ascx_SimpleTaskGui_ExtensionMethods
    {
        public static T start_ExecuteOnlyOnce<T>(this T taskGui)
            where T : ascx_SimpleTaskGui
        {
            return taskGui.start(true);
        }

        public static T start<T>(this T taskGui, bool executeOnlyOnce)
            where T : ascx_SimpleTaskGui
        {
            taskGui.ExecuteOnlyOnce = executeOnlyOnce;
            taskGui.start();
            return taskGui;
        }

        public static T once<T>(this T taskGui)
            where T : ascx_SimpleTaskGui
        {
            taskGui.ExecuteOnlyOnce = true;
            return taskGui;
        }

        public static T sleepPeriod<T>(this T taskGui, int sleepPeriod)
            where T : ascx_SimpleTaskGui
        {
            taskGui.SleepPeriod = sleepPeriod;
            return taskGui;
        }

        public static List<T> sleepPeriod<T>(this List<T> tasks, int sleepPeriod)
            where T : ascx_SimpleTaskGui
        {
            foreach (var task in tasks)
                task.sleepPeriod(sleepPeriod);
            return tasks;
        }

        public static List<T> start<T>(this List<T> tasks)
            where T : ascx_SimpleTaskGui
        {
            foreach (var task in tasks)
                task.start();
            return tasks;
        }

        public static List<T> stop<T>(this List<T> tasks)
            where T : ascx_SimpleTaskGui
        {
            foreach (var task in tasks)
                task.stop();
            return tasks;
        }

        public static List<T> executeOnce<T>(this List<T> tasks)
            where T : ascx_SimpleTaskGui
        {
            foreach (var task in tasks)
                task.ExecuteOnlyOnce = true; ;
            return tasks;
        }

        public static List<T> loopExecution<T>(this List<T> tasks)
            where T : ascx_SimpleTaskGui
        {
            foreach (var task in tasks)
                task.ExecuteOnlyOnce = false;
            return tasks;
        }

    }
}
