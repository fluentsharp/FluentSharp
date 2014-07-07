
using FluentSharp.CoreLib;

namespace FluentSharp.MsBuild
{
    public static class API_Create_Exe_ExtensionMethods
    {
        public static string path_FileWithStartupCode(this API_Create_Exe apiCreateExe)
        {
            return apiCreateExe.get_File_From_Embedded_Resources(apiCreateExe.RESOURCE_FILE_WITH_STARTUP_CODE);                        
        }        
        public static string path_O2Logo_Icon(this API_Create_Exe apiCreateExe)
        {
            return apiCreateExe.get_File_From_Embedded_Resources(apiCreateExe.RESOURCE_FILE_O2_LOGO);            
        }

        public static string get_File_From_Embedded_Resources(this API_Create_Exe apiCreateExe, string resourceName)
        {
            return (apiCreateExe.notNull() && resourceName.valid())
                        ?   apiCreateExe.type()
                                        .assembly()
                                        .resource_GetFile(resourceName)
                        : null;

        }
    }
}
