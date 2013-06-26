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
        public static Iterable<RevCommit>       log_Raw(this API_NGit nGit)
        {
            return nGit.log_Raw(-1);
        }
        public static Iterable<RevCommit>       log_Raw(this API_NGit nGit, int maxCount)
        {
            return nGit.Git.Log().SetMaxCount(maxCount).Call();
        }
    }
}
