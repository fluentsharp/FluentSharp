using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NGit.Storage.File;
using FluentSharp.ExtensionMethods;

namespace FluentSharp.ExtensionMethods
{
    public static class API_NGit_ExtMet_RefLogs
    {
        public static List<string> refLogs(this API_NGit nGit)
        {
            return nGit.refLogs(100);
        }
        /*public static List<string> refLogs(this API_NGit nGit)
        {
            return nGit.reflogs_Raw().Select(refLog => refLog.str()).toList();
        }*/
        public static List<string> refLogs(this API_NGit nGit, int maxCount)
        {
            return nGit.reflogs_Raw()
                       .take(maxCount)
                       .select(refLog => refLog.str()).toList();
        }
        public static ICollection<ReflogEntry> reflogs_Raw(this API_NGit nGit)
        {
            return nGit.Git.Reflog().Call();
        }
    }
}
