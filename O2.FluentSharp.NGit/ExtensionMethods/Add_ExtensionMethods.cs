using FluentSharp.CoreLib;
using FluentSharp.Git.APIs;
using NGit.Revwalk;

namespace FluentSharp.Git
{
    public static class Add_ExtensionMethods
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

        public static RevCommit  add_and_Commit_using_Status (this API_NGit nGit                           )
        {
            nGit.add();
            return nGit.commit_using_Status();            
        }

        public static RevCommit add_and_Commit_Random_File(this API_NGit nGit)
        {
            var randomFile = nGit.file_Create_Random_File();
            nGit.add(randomFile);
            var commitMessage = "Adding random file: {0}".format(randomFile);
            return nGit.commit(commitMessage);            
        }
    }
}
