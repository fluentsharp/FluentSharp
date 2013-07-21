using System;
using FluentSharp.CoreLib;
using FluentSharp.Git.APIs;
using FluentSharp.Git.Utils;
using NGit.Api;

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
                "[API_NGit] push completed".debug();
                return true;
            }
            catch (Exception ex)
            {
                if (nGit.notNull())
                    nGit.Last_Exception = ex;
                ex.log("[API_NGit][push]");
            }                            
            return false;
        }       
        public static bool  pull    (this API_NGit nGit, string remote = "origin")
        {
            try
            {
                var originalHead = nGit.head();
                nGit.Last_Exception = null;
                var pull_Command = nGit.Git.Pull();                
                nGit.LastGitProgress = new GitProgress();
                pull_Command.SetProgressMonitor(nGit.LastGitProgress);                
                nGit.Last_PullResult = pull_Command.Call();
                if (nGit.Last_PullResult.GetMergeResult().GetMergeStatus() == MergeStatus.CONFLICTING)
                    return nGit.reset_on_MergeConflicts(nGit.Last_PullResult);
                "[API_NGit] pull completed ok (with no conflicts)".debug();
                return true;
            }
            catch (Exception ex)
            {
                if (nGit.notNull())                        
                    nGit.Last_Exception = ex;
                ex.log("[API_NGit][pull]");
                return false;
            }
        }
        public static bool  reset_Hard(this API_NGit nGit)
        {
            try
            {
                var resetCommand = nGit.git().Reset();
                resetCommand.SetMode(ResetCommand.ResetType.HARD);
                nGit.Last_ResetResult = resetCommand.Call();                        
                "[API_NGit][reset_Hard] {0}".error(nGit.Last_ResetResult);
                return true;      
            }
            catch (Exception ex)
            {
                ex.log("[API_NGit][reset_Hard]");
                return false;
            }
        }
        public static bool  reset_on_MergeConflicts(this API_NGit nGit, PullResult pullResult)
        {
            if (nGit.notNull() && pullResult.notNull())
            {
                try
                {
                    if (pullResult.GetMergeResult().GetMergeStatus() == MergeStatus.CONFLICTING)
                    {
                        "[API_NGit][revert_on_MergeConflicts] pull result had a conflict so going to triger a hard reset".error();
                        return nGit.reset_Hard();                                          
                    }
                }
                catch (Exception ex)
                {
                    ex.log("[API_NGit][reset_on_MergeConflicts]");
                }            
            }
            return false;
        }

        public static bool  git_Pull    (this string repository)
        {
            return repository.git_Open()
                             .pull();
        }
    }
}
