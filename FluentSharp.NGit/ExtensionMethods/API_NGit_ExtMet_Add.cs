using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentSharp.ExtensionMethods;

namespace O2.FluentSharp.ExtensionMethods
{
    public static class API_NGit_ExtMet_Add
    {
        public static API_NGit  add     (this API_NGit nGit                                               )
        {
            return nGit.add(".");
        }
        public static API_NGit  add     (this API_NGit nGit, string filePattern                           )
        {
            return nGit.add(filePattern, true);
        }
        public static API_NGit  add     (this API_NGit nGit, string filePattern, bool  handleMissingFiles )
        {
            "[API_NGit] add: {0}".debug(filePattern);

            nGit.Git.Add().AddFilepattern(filePattern).Call();
            if (handleMissingFiles)
                nGit.Git.Add().AddFilepattern(filePattern).SetUpdate(true).Call();
            return nGit;
        }

        public static API_NGit  add_and_Commit_using_Status (this API_NGit nGit                           )
        {
            nGit.add();
            nGit.commit_using_Status();
            return nGit;
        }
    }
}
