using System;
using NGit;
using NGit.Api;

namespace FluentSharp.ExtensionMethods
{
    public static class API_NGit_Misc
    {
        public static bool       isGitRepository(this string pathToFolder)
        {
            return pathToFolder.dirExists() && pathToFolder.pathCombine(".git").dirExists();
        }
        public static string     head(this API_NGit nGit)
        {
            try
            {
                var head = nGit.Repository.Resolve(Constants.HEAD);
                return head.notNull() ? head.Name : null;
            }
            catch (Exception ex)
            {
                ex.log();
                return null;
            }
        }
        public static string git_Folder(this API_NGit nGit)
        {
            return nGit.files_Location().pathCombine(".git");
        }
        public static Repository repository(this API_NGit nGit)
        {
            if (nGit.notNull())
                return nGit.Repository;
            return null;
        }
        public static Git        git(this API_NGit nGit)
        {
            if (nGit.notNull())
                return nGit.Git;
            return null;
        }
    }
}
