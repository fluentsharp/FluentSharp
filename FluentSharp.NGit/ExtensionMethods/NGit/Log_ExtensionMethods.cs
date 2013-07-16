using FluentSharp.CoreLib;
using FluentSharp.Git.APIs;
using NGit.Revwalk;

namespace FluentSharp.Git
{
    public static class Log_ExtensionMethods
    {
        public static RevWalk revCommits_Raw(this API_NGit nGit)
        {
            return nGit.revCommits_Raw(-1);
        }
        public static RevWalk revCommits_Raw(this API_NGit nGit, int maxCount)
        {
            if (nGit.head().isNull())
                return nGit.revWalk();
            return (RevWalk)nGit.Git.Log().SetMaxCount(maxCount).Call();
        }        
    }
}
