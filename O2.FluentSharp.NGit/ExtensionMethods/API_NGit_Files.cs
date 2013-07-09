using System;
using System.Collections.Generic;
using FluentSharp.CoreLib;
using NGit.Revwalk;
using NGit.Treewalk;

namespace FluentSharp.ExtensionMethods
{
    public static class API_NGit_Files
    {
        public static API_NGit      file_Write(this API_NGit nGit, string virtualFileName, string fileContents)
        {
            var fileToWrite = nGit.Path_Local_Repository.pathCombine(virtualFileName);
            fileContents.saveAs(fileToWrite);
            return nGit;
        }
        public static API_NGit      file_Create(this API_NGit nGit, string virtualFileName, string fileContents)
        {
            return nGit.file_Write(virtualFileName, fileContents);
        }        
        public static string        file_FullPath(this API_NGit nGit, string virtualPath)
        {
            if (nGit.notNull() && nGit.Path_Local_Repository.valid() && nGit.Path_Local_Repository.dirExists() && virtualPath.notNull())
                return nGit.Path_Local_Repository.pathCombine(virtualPath);
            return null;
        }
        public static bool          file_Delete(this API_NGit nGit, string virtualPath)
        {            
            if (nGit.notNull())
            {
                var fullPath = nGit.file_FullPath(virtualPath);
                if (fullPath.fileExists())
                {
                    fullPath.file_Delete();
                    return true;
                }
            }
            return false;
        }
        public static string        file_Create_Random_File(this API_NGit nGit)
        {
            var fileName = "name".add_RandomLetters().add(".txt");
            var fileContent = "content".add_RandomLetters();
            nGit.file_Write(fileName, fileContent);
            return fileName;
        }
        public static string        files_Location(this API_NGit nGit)
        {
            if (nGit.notNull())
                return nGit.Path_Local_Repository;
            return null;
        }
        public static List<string>  files(this API_NGit nGit)        
        {            
            return nGit.files_HEAD();
        }

        public static List<string>  files_HEAD(this API_NGit nGit)
        {
            return nGit.files(nGit.head());
        }

        public static List<string>  files(this API_NGit nGit, string commitId) 
        {
            var repoFiles = new List<string>();
            try
            {
                var headCommit = nGit.Repository.Resolve(commitId);
                if (commitId.notNull())
                {
                    var revWalk = new RevWalk(nGit.Repository);
                    var commit = revWalk.ParseCommit(headCommit);
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
