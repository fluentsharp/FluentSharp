// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
namespace FluentSharp.CoreLib.Interfaces
{
    public enum TaskStatus
    {
        No_Task_Defined,
        Ready,
        Initializing_Objects,
        Executing,
        Waiting,
        Uploading_Data_To_Remote_Server,
        Waiting_For_Remote_Server_Response,
        Downloading_Data_From_Remote_Server,
        Completed_Ok,
        Completed_Failed
    }
}