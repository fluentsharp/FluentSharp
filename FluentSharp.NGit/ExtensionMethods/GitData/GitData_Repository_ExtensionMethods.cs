using System;
using System.Collections.Generic;
using FluentSharp.CoreLib;
using FluentSharp.Git;
using FluentSharp.Git.APIs;
using NGit.Revwalk;
using NGit.Treewalk;

namespace FluentSharp.ExtensionMethods
{
    public static class GitData_Repository_ExtensionMethods
    {
        public static GitData_Repository gitData_Repository(this API_NGit nGit)
        {
            if (nGit.isGitRepository())
            {
                var gitData_Repository = new GitData_Repository
                        {
                            LocalPath = nGit.Path_Local_Repository,
                            CurrentBranch = nGit.branch_Current()
                        };
                return gitData_Repository;
            }
            return null;
        }        
    }
    public static class GitData_Commits_ExtensionMethods
    {       
        public static List<GitData_Commit> gitData_Commits(this API_NGit nGit)
        {
            var gitData_Commits  = new List<GitData_Commit>();
            foreach(var commit in nGit.commits())
            {
                var gitData_Commit = new GitData_Commit
                    {
                        Author    = commit.author_Name(),
                        Committer = commit.committer_Name(),
                        Sha1      = commit.sha1(),
                        Message   = commit.message(),
                        When      = commit.when().ToShortDateString().fromFiletime
                    };
                gitData_Commits.add(gitData_Commit);
            }
            return gitData_Commits;
        }
    }
    public static class GitData_Branch_ExtensionMethods
    {            
        public static GitData_Branch        branch_Current(this API_NGit nGit)
        {
            var gitBranch = new GitData_Branch
                {
                    Commits = nGit.gitData_Commits(),
                    Files = nGit.gitData_Files(nGit.head()),
                    HEAD  = nGit.head() 
                };
            return gitBranch;                
        }
        public static List<GitData_File>    files         (this GitData_Branch gitData_Branch)
        {
            if(gitData_Branch.notNull())
                return gitData_Branch.Files;
            return new List<GitData_File>();
        }
        public static List<GitData_Commit>  commits       (this GitData_Branch gitData_Branch)
        {
            if(gitData_Branch.notNull())
                return gitData_Branch.Commits;
            return new List<GitData_Commit>();
        }
    }

    public static class GitData_File_ExtensionMethods
    {   
        public static List<GitData_File> gitData_Files(this API_NGit nGit)
        {
            return nGit.gitData_Files(nGit.head());
        }
        public static List<GitData_File> gitData_Files(this API_NGit nGit, string commitSha1)
        {
            var gitData_Files = new  List<GitData_File>();
            try
            {            
                var headCommit = nGit.Repository.Resolve(commitSha1);
                if (commitSha1.notNull())
                {
                    var revWalk = new RevWalk(nGit.Repository);
                    var commit = revWalk.ParseCommit(headCommit);
                    var treeWalk = new TreeWalk(nGit.Repository);
                    var tree = commit.Tree;
                    treeWalk.AddTree(tree);
                    treeWalk.Recursive = true;

                    while (treeWalk.Next())                
                        gitData_Files.add_File(treeWalk);
                        //repoFiles.Add(treeWalk.PathString);                
                }
            }
            catch(Exception ex)
            {
                ex.log("[API_NGit][gitData_Files]");
            }
            return gitData_Files;
        }
        public static GitData_File       add_File(this List<GitData_File> gitData_Files, TreeWalk treeWalk)
        {
            var gitData_File = new GitData_File
                {
                    FilePath = treeWalk.PathString,
                    Sha1     = treeWalk.GetObjectId(0).sha1()
                };
            gitData_Files.add(gitData_File);
            return gitData_File;
        }
    }
}
