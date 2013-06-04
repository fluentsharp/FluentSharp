using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NGit;
using FluentSharp.ExtensionMethods;

namespace O2.FluentSharp.ExtensionMethods
{
    public static class API_NGit_ExtMet_Misc
    {
        public static bool      isGitRepository(this string pathToFolder)
        {
            return pathToFolder.dirExists() && pathToFolder.pathCombine(".git").dirExists();
        }
        public static string    head(this API_NGit nGit)
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
        public static API_NGit  write_File     (this API_NGit nGit, string virtualFileName, string fileContents)
        {
            var fileToWrite = nGit.Path_Local_Repository.pathCombine(virtualFileName);
            fileContents.saveAs(fileToWrite);
            return nGit;
        }
        public static API_NGit  create_File    (this API_NGit nGit, string virtualFileName, string fileContents)
        {
            return nGit.write_File(virtualFileName, fileContents);
        }
        
    }
}
