using FluentSharp.CoreLib;
using FluentSharp.Git.APIs;

namespace FluentSharp.Git
{
    public static class API_NGit_ExtensionMethods
    {
        public static bool isGitRepository(this API_NGit nGit)
        {
            return  nGit.notNull()     &&
                    nGit.Git.notNull() &&
                    nGit.Repository.notNull() && 
                    nGit.Path_Local_Repository.dirExists() && 
                    nGit.Path_Local_Repository.isGitRepository();
        }
    }
}
