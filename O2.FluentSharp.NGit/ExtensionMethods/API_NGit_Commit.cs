using System.Collections.Generic;
using System.Linq;

namespace FluentSharp.ExtensionMethods
{
    public static class API_NGit_Commit
    {
        public static API_NGit      commit              (this API_NGit nGit, string commitMessage)
        {            
            if (commitMessage.valid())
            {
                "[API_NGit] commit: {0}".debug(commitMessage);
                var commit_Command = nGit.Git.Commit();
                commit_Command.SetMessage(commitMessage);
                commit_Command.Call();
            }
            else
                "[API_NGit] commit was called with no commitMessage".error();
            return nGit;
        }
        public static API_NGit      commit_using_Status (this API_NGit nGit                      )
        {
            nGit.commit(nGit.status());
            return nGit;
        }
        public static List<string>  commits             (this API_NGit nGit                      )
        {
            return nGit.commits(-1);
        }
        public static List<string>  commits             (this API_NGit nGit, int maxCount        )
        {
            return nGit.log_Raw(maxCount)
                       .Select(logEntry => logEntry.Name)
                       .toList();

        }
        
    }
}
