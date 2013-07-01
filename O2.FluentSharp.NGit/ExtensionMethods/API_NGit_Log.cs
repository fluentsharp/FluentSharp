using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NGit.Revwalk;
using Sharpen;

namespace FluentSharp.ExtensionMethods
{
    public static class API_NGit_Log
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
