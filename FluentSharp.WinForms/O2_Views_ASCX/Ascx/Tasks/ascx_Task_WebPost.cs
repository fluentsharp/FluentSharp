using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;

namespace FluentSharp.WinForms.Controls
{
    public class ascx_Task_WebPost : ascx_SimpleTaskGui
    {

        public ascx_Task_WebPost()
        {
            this.TaskType = "Web (Post request)";
            this.TaskName = "...";
            this.TaskFunction = () =>
            {
                var response = new Web().getUrlContents_POST(TaskName, "");
                messageLabel.set_Text("response size: {0}".format(response.size()));
                return response != "";
            };
        }

        public ascx_Task_WebPost start(string target)
        {
            this.clear();
            this.TaskName = target.uri().str();
            this.buildGui();
            this.start();
            return this;
        }
    }
}
