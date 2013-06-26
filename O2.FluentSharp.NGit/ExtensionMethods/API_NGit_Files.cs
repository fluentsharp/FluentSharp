using System;
using System.Collections.Generic;
using NGit;
using NGit.Revwalk;
using NGit.Treewalk;

namespace FluentSharp.ExtensionMethods
{
    public static class API_NGit_Files
    {
        public static API_NGit write_File(this API_NGit nGit, string virtualFileName, string fileContents)
        {
            var fileToWrite = nGit.Path_Local_Repository.pathCombine(virtualFileName);
            fileContents.saveAs(fileToWrite);
            return nGit;
        }
        public static API_NGit create_File(this API_NGit nGit, string virtualFileName, string fileContents)
        {
            return nGit.write_File(virtualFileName, fileContents);
        }        
        public static string file_FullPath(this API_NGit nGit, string virtualPath)
        {
            if (nGit.notNull() && nGit.Path_Local_Repository.valid() && nGit.Path_Local_Repository.dirExists() && virtualPath.notNull())
                return nGit.Path_Local_Repository.pathCombine(virtualPath);
            return null;
        }
        public static string file_Create_Random_File(this API_NGit nGit)
        {
            var fileName = "name".add_RandomLetters().add(".txt");
            var fileContent = "content".add_RandomLetters();
            nGit.write_File(fileName, fileContent);
            return fileName;
        }
        public static string files_Location(this API_NGit nGit)
        {
            if (nGit.notNull())
                return nGit.Path_Local_Repository;
            return null;
        }
        public static List<string> files(this API_NGit nGit)        
        {
            return nGit.getRepoFiles();
        }
        public static List<string> getRepoFiles(this API_NGit nGit) 
        {
            var repoFiles = new List<string>();
            try
            {
                var lastCommitId = nGit.Repository.Resolve(Constants.HEAD);
                if (lastCommitId.notNull())
                {
                    var revWalk = new RevWalk(nGit.Repository);
                    var commit = revWalk.ParseCommit(lastCommitId);
                    var treeWalk = new TreeWalk(nGit.Repository);
                    var tree = commit.Tree;
                    treeWalk.AddTree(tree);
                    treeWalk.Recursive = true;

                    while (treeWalk.Next())
                    {
                        repoFiles.Add(treeWalk.PathString);
                    }
                }
            }
            //ncrunch: no coverage start
            catch (Exception ex) 
            {
                ex.log("[API_NGit][getRepoFiles]"); 
            }
            //ncrunch: no coverage end
            return repoFiles;            
        }
    }
}
