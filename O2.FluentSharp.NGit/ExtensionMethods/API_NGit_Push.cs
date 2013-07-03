using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentSharp.ExtensionMethods;
using FluentSharp.NGit_Classes;

namespace FluentSharp.ExtensionMethods
{
    public static class API_NGit_Push
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
    }
}
