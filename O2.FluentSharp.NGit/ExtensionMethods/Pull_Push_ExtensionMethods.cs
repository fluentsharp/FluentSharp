using System;
using FluentSharp.CoreLib;
using FluentSharp.Git.APIs;
using FluentSharp.Git.Utils;

namespace FluentSharp.Git
{
    public static class Pull_Push_ExtensionMethods
    {
        public static bool  push    (this API_NGit nGit                                               )
        {
            return nGit.push("origin");
        }
        public static bool  push    (this API_NGit nGit, string remote                                )
        {
            "[API_NGit] push: {0}".debug(remote);
            try
            {
                nGit.Last_Exception = null;
                var push_Command = nGit.Git.Push();
                push_Command.SetRemote(remote);
                nGit.LastGitProgress = new GitProgress();
                push_Command.SetProgressMonitor(nGit.LastGitProgress);
                nGit.Last_PushResult = push_Command.Call().toList();
                return true;
            }
            catch (Exception ex)
            {
                nGit.Last_Exception = ex;
                ex.log("[API_NGit][push]");
            }
            "[API_NGit] push completed".debug();
            return false;
        }  
      
        public static bool  pull    (this API_NGit nGit, string remote = "origin")
        {
            try
            {
                nGit.Last_Exception = null;
                var pull_Command = nGit.Git.Pull();                
                nGit.LastGitProgress = new GitProgress();
                pull_Command.SetProgressMonitor(nGit.LastGitProgress);
                nGit.Last_PullResult = pull_Command.Call();

                "[API_NGit] pull completed".debug();
                return true;
            }
            catch (Exception ex)
            {
                nGit.Last_Exception = ex;
                ex.log("[API_NGit][pull]");
                return false;
            }
        }
        public static bool  git_Pull    (this string repository)
        {
            return repository.git_Open()
                             .pull();
        }
    }
}
