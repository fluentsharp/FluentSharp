using System.Collections.Generic;
using FluentSharp;
using NGit;
using NGit.Revwalk;
using NGit.Treewalk;

namespace O2.FluentSharp.ExtensionMethods
{
    public static class API_NGit_Files
    {
        public static List<string> getRepoFiles(this API_NGit nGit)
        {
            var lastCommitId = nGit.Repository.Resolve(Constants.HEAD);
            var revWalk = new RevWalk(nGit.Repository);
            var commit = revWalk.ParseCommit(lastCommitId);
            var treeWalk = new TreeWalk(nGit.Repository);
            var tree = commit.Tree;
            treeWalk.AddTree(tree);
            treeWalk.Recursive = true;
            var repoFiles = new List<string>();
            while (treeWalk.Next())
            {
                repoFiles.Add(treeWalk.PathString);
            }
            return repoFiles;
        }
    }
}
