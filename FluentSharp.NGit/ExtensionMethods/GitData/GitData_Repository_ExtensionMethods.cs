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
        public static GitData_Repository gitData_Repository(this API_NGit nGit, bool mapCommitTrees = false)
        {
            if (nGit.isGitRepository())
            {
                var gitData_Repository = new GitData_Repository
                        {
                            LocalPath = nGit.Path_Local_Repository,
                            CurrentBranch = nGit.branch_Current(mapCommitTrees)
                        };
                return gitData_Repository;
            }
            return null;
        }        
    }
    public static class GitData_Commits_ExtensionMethods
    {       
        public static List<GitData_Commit> gitData_Commits(this API_NGit nGit, bool mapCommitTrees)
        {
            var gitData_Commits  = new List<GitData_Commit>();
            foreach(var commit in nGit.commits())
            {
                var gitData_Commit = new GitData_Commit
                    {
                        Author    = commit.author_Name(),
                        Committer = commit.committer_Name(),                        
                        Message   = commit.message(),
                        Sha1      = commit.sha1(),
                        When      = commit.when().toFileTimeUtc()
                        
                    };
                if (mapCommitTrees)
                    gitData_Commit.Tree = commit.gitData_Files(nGit);              
                gitData_Commits.add(gitData_Commit);
            }
            return gitData_Commits;
        }
    }
    public static class GitData_Branch_ExtensionMethods
    {            
        public static GitData_Branch        branch_Current(this API_NGit nGit, bool mapCommitTrees)
        {
            var gitBranch = new GitData_Branch
                {
                    Commits = nGit.gitData_Commits(mapCommitTrees),
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
        public static List<GitData_File> gitData_Files(this RevCommit revCommit, API_NGit nGit)
        {
            var gitData_Files = new List<GitData_File>();
            revCommit.commit_TreeWalk(nGit, treeWalk => gitData_Files.add_File(treeWalk));
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

    public static class GitData_File_Commits_ExtensionMethods
    {
        public static List<GitData_File_Commit> file_Commits(this string fileToMap, API_NGit nGit)
        {
            var file_Commits = new List<GitData_File_Commit>();
            var mappedSha1 = new List<string>();
            var gitRepository = nGit.gitData_Repository(true);           
            var results = new List<string>();
            foreach(var commit in gitRepository.CurrentBranch.Commits)
	            foreach(var file in commit.Tree)
	            {
		            if (file.FilePath == fileToMap)
                        if (mappedSha1.contains(file.Sha1).isFalse())
	                    {
                            var fileCommit = new GitData_File_Commit
                                {
                                    FilePath  = fileToMap,
                                    Sha1      = file.Sha1,
                                    CommitId  = commit.Sha1,
                                    Author    = commit.Author,
                                    Committer    = commit.Committer,
                                    When         = commit.When,
                                    FileContents = nGit.open_Object(file.Sha1).bytes().ascii()
                                };
                            file_Commits.Add(fileCommit);
                            mappedSha1.add(file.Sha1);
	                    }	            
                }
            return file_Commits;
        }
    }
}
