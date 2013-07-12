using FluentSharp.CoreLib;
using FluentSharp.Git.APIs;

namespace FluentSharp.Git
{
    public static class O2Platform_Git_ExtensionMethods
    {
        public static string repositoryUrl(this API_NGit_O2Platform nGit_O2, string repositoryName)
        {
            return nGit_O2.GitHub_O2_Repositories.format(repositoryName);
        }

        public static API_NGit_O2Platform cloneOrPull(this API_NGit_O2Platform nGit_O2, string repositoryName)
        {
            var repositoryUrl = nGit_O2.repositoryUrl(repositoryName);
            var localPath = nGit_O2.LocalGitRepositories.pathCombine(repositoryName);
            nGit_O2.open_or_Clone(repositoryUrl, localPath);
            return nGit_O2;
        }

        public static API_NGit nGit(this API_NGit_O2Platform nGit_O2)
        {
            return nGit_O2;
        }        
    }
}