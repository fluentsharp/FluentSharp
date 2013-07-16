using System;
using System.Collections.Generic;
using System.Linq;
using FluentSharp.CoreLib;
using FluentSharp.Git.APIs;
using NGit;
using NGit.Revwalk;
using NGit.Treewalk;


namespace FluentSharp.Git
{
    public static class Commit_ExtensionMethods
    {
        public static PersonIdent     author(this RevCommit revCommit)
        {
            if (revCommit.notNull())
                return revCommit.GetAuthorIdent();
            return null;
        }
        public static string          author_Name(this RevCommit revCommit)
        {
            return revCommit.author().name();
        }
        public static string          author_Email(this RevCommit revCommit)
        {
            return revCommit.author().email();
        }
        public static RevCommit       commit(this API_NGit nGit, string commitMessage)
        {
            return nGit.commit(commitMessage, nGit.Author, nGit.Committer);
        }
        public static RevCommit       commit(this API_NGit nGit, string commitMessage, string name, string email)
        {
            var person = name.personIdent(email);
            return nGit.commit(commitMessage, person, person);
        }
        public static RevCommit       commit(this API_NGit nGit, string commitMessage, PersonIdent author, PersonIdent committer)
        {            
            if (commitMessage.valid())
            {
                "[API_NGit] commit: {0}".debug(commitMessage);
                var commit_Command = nGit.Git.Commit();
                commit_Command.SetMessage  (commitMessage);
                commit_Command.SetAuthor   (author);
                commit_Command.SetCommitter(committer);
                return commit_Command.Call();
            }
            
            "[API_NGit] commit was called with no commitMessage".error();
            return null;
        }
        public static RevCommit       commit_using_Status(this API_NGit nGit)
        {
            return nGit.commit(nGit.status());            
        }        
        public static List<string>    commits_SHA1             (this API_NGit nGit, int maxCount = -1)
        {            
            return nGit.commits(maxCount)
                       .Select(logEntry => logEntry.Name)
                       .toList();
        }
        public static List<RevCommit> commits(this API_NGit nGit, int maxCount = -1)
        {            
            return nGit.revCommits_Raw(maxCount).toList();
        }        
        public static List<string>    commit_Files(this RevCommit revCommit, API_NGit nGit)
        {
            var repoFiles = new List<string>();
            revCommit.commit_TreeWalk(nGit, treeWalk => repoFiles.Add(treeWalk.PathString));
            return repoFiles;
        }  

        public static Dictionary<string,string>    commit_Files_IndexedBy_SHA1(this RevCommit revCommit, API_NGit nGit)
        {
            var repoFiles = new Dictionary<string,string>();
            revCommit.commit_TreeWalk(nGit, treeWalk =>
                {
                    var objectId = treeWalk.GetObjectId(0);
                    repoFiles.Add(objectId.Name, treeWalk.PathString);
                });
            return repoFiles;
        }  
                      
        public static RevCommit       commit_TreeWalk(this RevCommit revCommit, API_NGit nGit, Action<TreeWalk> onNext)
        {            
            var treeWalk = new TreeWalk(nGit.Repository);
            var tree = revCommit.Tree;
            treeWalk.AddTree(tree);
            treeWalk.Recursive = true;

            while (treeWalk.Next())
                onNext(treeWalk);
            return revCommit;
        }                        
        public static List<string>    commit_Files_FullPath(this RevCommit revCommit, API_NGit nGit)
        {
            return (from file in revCommit.commit_Files(nGit)
                    select nGit.file_FullPath(file)).toList();
        }
        public static PersonIdent     committer(this RevCommit revCommit)
        {
            if (revCommit.notNull())
                return revCommit.GetCommitterIdent();
            return null;
        }        
        public static string          committer_Name(this RevCommit revCommit)
        {
            return revCommit.committer().name();
        }
        public static string          committer_Email(this RevCommit revCommit)
        {
            return revCommit.committer().email();
        }
        public static string          message(this RevCommit revCommit)
        {
            if (revCommit.notNull())
                return revCommit.GetFullMessage();
            return null;
        }
        public static string          sha1(this RevCommit revCommit)
        {
            if (revCommit.notNull())
                return revCommit.Name;
            return null;
        }
        public static DateTime        when(this RevCommit revCommit)
        {            
            return revCommit.committer().when();            
        }
    }
}
