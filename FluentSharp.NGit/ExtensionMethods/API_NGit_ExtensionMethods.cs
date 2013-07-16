using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentSharp.CoreLib;
using FluentSharp.Git;
using FluentSharp.Git.APIs;

namespace FluentSharp.ExtensionMethods
{
    public static class API_NGit_ExtensionMethods
    {
        public static bool isGitRepository(this API_NGit nGit)
        {
            return  nGit.notNull()     &&
                    nGit.Git.notNull() &&
                    nGit.Repository.notNull() && 
                    nGit.Path_Local_Repository.dirExists() && 
                    nGit.Path_Local_Repository.isGitRepository();
        }
    }
}
