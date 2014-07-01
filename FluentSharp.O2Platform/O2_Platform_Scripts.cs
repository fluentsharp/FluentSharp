using FluentSharp.Git;
using FluentSharp.Git.APIs;

namespace FluentSharp.O2Platform
{
    public class O2_Platform_Scripts
    {
        public API_NGit nGit;
        
        public bool SetUp()
        {
            return Clone_Or_Open_O2_Platform_Scripts_Repository();
        }
        public bool Clone_Or_Open_O2_Platform_Scripts_Repository()
        {            
            var sourceRepository = O2_Platform_Consts.GIT_HUB_O2_PLATFORM_SCRIPTS;
            var targetFolder     = O2_Platform_Config.Current.Folder_Scripts;
            nGit = new API_NGit().open_or_Clone(sourceRepository, targetFolder);
            return nGit.isGitRepository();
        }
        public string ScriptsFolder()
        {
            return O2_Platform_Config.Current.Folder_Scripts;
        }

        

    }
}