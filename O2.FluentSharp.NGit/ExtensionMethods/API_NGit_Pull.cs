using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentSharp.NGit_Classes;

namespace FluentSharp.ExtensionMethods
{
    public static class API_NGit_Pull
    {
        public static bool  pull    (this API_NGit nGit)
        {
            try
            {
                var pull_Command = nGit.Git.Pull();
                nGit.LastGitProgress = new GitProgress();
                pull_Command.SetProgressMonitor(nGit.LastGitProgress);
                pull_Command.Call();

                "[API_NGit] pull completed".debug();
                return true;
            }
            catch (Exception ex)
            {
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
