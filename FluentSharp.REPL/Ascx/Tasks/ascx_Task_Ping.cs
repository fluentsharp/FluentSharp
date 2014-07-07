using FluentSharp.CoreLib.API;

//O2File:ascx_SimpleTaskGui.cs
using FluentSharp.Web35.API;

namespace FluentSharp.WinForms.Controls
{
    public class ascx_Task_Ping : ascx_SimpleTaskGui
    {
        public ascx_Task_Ping()
        {
            TaskType = "Ping";
            TaskName = "...";
            TaskFunction = () =>
            {
                var ping = new Ping();
                return ping.ping(TaskName);
            };
        }

        public ascx_Task_Ping start(string target)
        {
            this.clear();
            TaskName = target;
            buildGui();
            start();
            return this;
        }

    }
}
