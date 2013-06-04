using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentSharp.ExtensionMethods;

namespace O2.FluentSharp.ExtensionMethods
{
    public static class API_NGit_ExtMet_Push
    {
        public static API_NGit  push    (this API_NGit nGit                                               )
        {
            return nGit.push("origin");
        }
        public static API_NGit  push    (this API_NGit nGit, string remote                                )
        {
            "[API_NGit] push: {0}".debug(remote);

            var push_Command = nGit.Git.Push();
            push_Command.SetRemote(remote);
            nGit.LastGitProgress = new GitProgress();
            push_Command.SetProgressMonitor(nGit.LastGitProgress);
            push_Command.Call();

            "[API_NGit] push completed".debug();
            return nGit;
        }
        public static API_NGit  pull    (this API_NGit nGit                                               )
        {
            //"[API_NGit] pull start".debug();
            var pull_Command = nGit.Git.Pull();
            //pull_Command.SetRemote(remote);	
            nGit.LastGitProgress = new GitProgress();
            pull_Command.SetProgressMonitor(nGit.LastGitProgress);
            pull_Command.Call();

            "[API_NGit] pull completed".debug();
            return nGit;
        }
    }
}
