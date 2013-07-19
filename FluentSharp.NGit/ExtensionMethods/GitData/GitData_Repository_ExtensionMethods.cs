using System;
using System.Collections.Generic;
using System.Linq;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.Git.APIs;
using NGit.Revwalk;
using NGit.Treewalk;

namespace FluentSharp.Git
{
    public static class GitData_Repository_ExtensionMethods
    {   
        public static GitData_Repository gitData_Repository (this string pathToRepository)
        {
            if (pathToRepository.dirExists())
                if(pathToRepository.isGitRepository())
                {
                    var nGit = pathToRepository.git_Open();
                    return nGit.gitData_Repository();
                }
            return null;
        }
        public static GitData_Repository gitData_Repository (this API_NGit nGit, bool loadData = true)
        {
            if (nGit.isGitRepository())
            {
                var gitData_Repository = new GitData_Repository
                    {
                        Config =
                            {
                                LocalPath = nGit.Path_Local_Repository
                            },
                        nGit = nGit

                    };
                if (loadData)
                    gitData_Repository.loadData();
                return gitData_Repository;
            }
            return null;
        }        
        public static GitData_Repository loadData           (this GitData_Repository gitData_Repository)
        {
            if (gitData_Repository.notNull())
            {
                var nGit = gitData_Repository.nGit;
                gitData_Repository.CurrentBranch = nGit.branch_Current();
                gitData_Repository.HEAD          = nGit.head();                

                gitData_Repository.Branches      = nGit.gitData_Branches();
                gitData_Repository.Commits       = nGit.gitData_Commits(gitData_Repository.Config.Max_CommitsToShow, gitData_Repository.Config.Load_CommitTrees);
                gitData_Repository.Files         = nGit.gitData_Files  (gitData_Repository.Config.Max_FilesToShow, gitData_Repository.nGit.head());
                
            }
            return null;
        }
    }
    public static class GitData_Commits_ExtensionMethods
    {       
        public static List<GitData_Commit> gitData_Commits(this API_NGit nGit, int max_CommitsToShow , bool mapCommitTrees)
        {
            var gitData_Commits  = new List<GitData_Commit>();
            foreach(var commit in nGit.commits().take(max_CommitsToShow))
            {
                var gitData_Commit = new GitData_Commit
                    {
                        Author    = commit.author_Name(),
                        Committer = commit.committer_Name(),                        
                        Message   = commit.message(),
                        Sha1      = commit.sha1(),
                        When      = commit.when().toFileTimeUtc()                        
                    };
                if (commit.ParentCount >0)
                    gitData_Commit.Parents = (from parent in commit.Parents select parent.Name).toList();

                if (mapCommitTrees)
                    gitData_Commit.Tree = commit.gitData_Files(nGit);              
                gitData_Commits.add(gitData_Commit);
            }
            return gitData_Commits;
        }
        public static List<GitData_Commit> commits        (this GitData_Repository gitData_Repository)
        {
            if(gitData_Repository.notNull())
                return gitData_Repository.Commits;
            return new List<GitData_Commit>();
        }
        public static List<GitData_File>    files         (this GitData_Repository gitData_Repository)
        {
            if(gitData_Repository.notNull())
                return gitData_Repository.Files;
            return new List<GitData_File>();
        }        
    }
    public static class GitData_Branch_ExtensionMethods
    {            
        
        public static List<GitData_Branch> gitData_Branches(this API_NGit nGit)
        {
            var gitData_Branches  = new List<GitData_Branch>();
            foreach(var @ref in nGit.branches_Raw())
            {
                var gitData_Branch = new GitData_Branch
                    {
                        Name    = @ref.GetName(),
                        Sha1    = @ref.GetObjectId().Name                        
                    };                              
                gitData_Branches.add(gitData_Branch);
            }
            return gitData_Branches;
        }
        public static List<GitData_Branch>    branches         (this GitData_Repository gitData_Repository)
        {
            if(gitData_Repository.notNull())
                return gitData_Repository.Branches;
            return new List<GitData_Branch>();
        }
    }

    public static class GitData_File_ExtensionMethods
    {   
        public static List<GitData_File> gitData_Files(this API_NGit nGit)
        {
            return nGit.gitData_Files(-1, nGit.head());
        }
        public static List<GitData_File> gitData_Files(this API_NGit nGit, int max_FilesToShow, string commitSha1)
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

                    while (treeWalk.Next() && (max_FilesToShow == -1) || gitData_Files.size() < max_FilesToShow)                
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
        public static GitData_Repository map_File_Commits(this GitData_Repository gitDataRepository)
        {
            
            var mappedSha1s = new List<string>();
            var nGit = gitDataRepository.nGit;
            var results = new List<string>();
            var mappedFiles = new Dictionary<string, GitData_File>();
            foreach(var file in gitDataRepository.files())
                mappedFiles.Add(file.FilePath, file);

            foreach(var commit in nGit.commits())
            {
                Action<TreeWalk> onTreeWalk = 
                    (treeWalk)=>
                        {
                            if (mappedFiles.hasKey(treeWalk.PathString))
	                        {
                                var mappedSha1 = "{0}:{1}".format(treeWalk.GetObjectId(0).sha1(),mappedFiles[treeWalk.PathString].Sha1);
                                if (mappedSha1s.contains(mappedSha1))
                                    return;
                                var fileCommit = new GitData_File_Commit
                                    {                                    
                                        File_Sha1    = treeWalk.GetObjectId(0).sha1(),
                                        Commit_Sha1  = commit.sha1(),
                                        Author       = commit.author_Name(),
                                        When         = commit.when().toFileTimeUtc()
                                    };
                                mappedFiles[treeWalk.PathString].Commits.add(fileCommit);                                
                                mappedSha1s.add(mappedSha1);
                                //mappedSha1.add(file.Sha1);
	                        }	            
                        };
                nGit.files(commit.sha1(), onTreeWalk);
            }	            
            return gitDataRepository;
        }

        public static List<GitData_File_Commit_Full> file_Commits(this string fileToMap, API_NGit nGit)
        {
            var file_Commits = new List<GitData_File_Commit_Full>();
            var mappedSha1 = new List<string>();
            var gitRepository = nGit.gitData_Repository(false);           
            gitRepository.Config.Load_CommitTrees = true;
            gitRepository.loadData();
            var results = new List<string>();
            foreach(var commit in gitRepository.Commits)
	            foreach(var file in commit.Tree)
	            {
		            if (file.FilePath == fileToMap)
                        if (mappedSha1.contains(file.Sha1).isFalse())
	                    {
                            var fileCommit = new GitData_File_Commit_Full
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
