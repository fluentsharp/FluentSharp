using System;
using System.Collections.Generic;
using System.Linq;
using FluentSharp.CoreLib;
using FluentSharp.Git.APIs;
using NGit;
using NGit.Transport;

namespace FluentSharp.Git
{
    public static class Repository_ExtensionMethods
    {        
        public static string       head(this API_NGit nGit)
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
        public static bool         isNotGitRepository(this string pathToFolder)
        {
            return pathToFolder.isGitRepository().isFalse();
        }
        public static bool         hasGitFolder(this string pathToFolder)
        {
            return pathToFolder.dirExists() && pathToFolder.pathCombine(".git").dirExists();            
        }
        
        public static bool         isGitRepository(this string pathToFolder)
        {
            if (pathToFolder.hasGitFolder())
            {
                var nGit = pathToFolder.git_Open();
                if (nGit.notNull())
                {
                    nGit.close();
                    return true;
                }
            }
            return false;
        }
        public static Repository   repository(this API_NGit nGit)
        {
            if (nGit.notNull())
                return nGit.Repository;
            return null;
        }
        public static API_NGit     use_Credential(this API_NGit nGit, string username, string password)
        {
            nGit.Credentials = new UsernamePasswordCredentialsProvider(username, password);
            return nGit;
        }
        public static List<string> refs(this API_NGit nGit)
        {
            return (from @ref in nGit.refs_Raw()
                    select @ref.GetName()).toList();
        }
        public static List<Ref>    refs_Raw(this API_NGit nGit)
        {
            if (nGit.repository().notNull())
                return nGit.repository().GetAllRefs().Values.toList();
            return new List<Ref>();
        }

    }
}
