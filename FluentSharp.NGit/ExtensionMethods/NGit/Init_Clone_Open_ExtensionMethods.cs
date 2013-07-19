using System;
using System.Net;
using FluentSharp.CoreLib.API;
using FluentSharp.Git.APIs;
using FluentSharp.CoreLib;
using FluentSharp.Git.Utils;


namespace FluentSharp.Git
{
    public static class Init_Clone_Open_ExtensionMethods
    {
        public static API_NGit init(this API_NGit nGit, string targetFolder)                                
        {
            nGit.Last_Exception = null;

            if (targetFolder.isNull())
            {
                "[API_NGit][git_Init] targetFolder value provided was null".error();                    
                return null;
            }
            if (targetFolder.isGitRepository())
            {
                "[API_NGit][git_Init] tried to init a repository in a folder that already has a git repository: {0}"
                    .error(targetFolder);
                return null;
            }            
            try
            {
                nGit.close();
                "[API_NGit] init: {0}".debug(targetFolder);
                var init_Command = NGit.Api.Git.Init();

                init_Command.SetDirectory(targetFolder);
                nGit.Git = init_Command.Call();
                nGit.Repository = nGit.Git.GetRepository();
                nGit.Path_Local_Repository = targetFolder;                
                return nGit;
            }
            catch (Exception ex)
            {
                nGit.Last_Exception = ex;
                ex.log("[API_NGit] ");
            }
            return null;
        }        
        public static API_NGit open         (this API_NGit nGit, string pathToLocalRepository)                  
        {
            nGit.Last_Exception = null;
            try
            {
                nGit.close();
                "[API_NGit] open: {0}".debug(pathToLocalRepository);                
                nGit.Git = NGit.Api.Git.Open(pathToLocalRepository);                
                nGit.Repository = nGit.Git.GetRepository();
                nGit.Path_Local_Repository = pathToLocalRepository;                
                return nGit;
            }
            catch (Exception ex)
            {
                nGit.Last_Exception = ex;
                ex.log("[API_NGit] ");
            }
            return null;
        }
        public static API_NGit clone        (this API_NGit nGit, string sourceRepository, string targetFolder)  
        {
            nGit.Last_Exception = null;
            "[API_NGit] cloning: {0} into {1}".debug(sourceRepository, targetFolder);
            try
            {                
                if (targetFolder.dirExists())
                {
                    "[API_NGit] provided target folder already exists,please delete it or provide a difference one: {0}".error(targetFolder);
                    return null;
                }
                nGit.close();
                var start = DateTime.Now;
                var clone_Command = NGit.Api.Git.CloneRepository();
                
                clone_Command.SetDirectory(targetFolder);
                clone_Command.SetURI(sourceRepository);
                clone_Command.SetCredentialsProvider(nGit.Credentials);
                nGit.LastGitProgress = new GitProgress();
                clone_Command.SetProgressMonitor(nGit.LastGitProgress);

                nGit.Git = clone_Command.Call();
                nGit.Repository = nGit.Git.GetRepository();
                nGit.Path_Local_Repository = targetFolder;

                "[API_NGit] clone completed in: {0}".debug(start.timeSpan_ToString());
                                
                return nGit;
            }                
            catch (Exception ex)
            {
                nGit.Last_Exception = ex;
                ex.log("[API_NGit] ");
                if(ex.isNotInstanceOf<WebException>() )
                    Files.deleteFolder(targetFolder, true);
            }
            return null;
        }
        public static API_NGit open_or_Clone(this API_NGit nGit, string sourceRepository, string targetFolder)  
        {
            if (targetFolder.isGitRepository())
                return nGit.open(targetFolder);
            
            return nGit.clone(sourceRepository, targetFolder);
        }
        public static API_NGit git_Init     (this string targetFolder)                                          
        {            
            return new API_NGit().init(targetFolder);
        }
        public static API_NGit git_Open     (this string targetFolder)                                          
        {
            return new API_NGit().open(targetFolder);
        }
        public static API_NGit git_Clone    (this Uri sourceRepository, string targetFolder)                    
        {
            return sourceRepository.str().git_Clone(targetFolder);
        }
        public static API_NGit git_Clone    (this string sourceRepository, string targetFolder)                 
        {
            return new API_NGit().clone(sourceRepository, targetFolder);
        }
        public static API_NGit close        (this API_NGit nGit)                                                
        {
            if (nGit.notNull() && nGit.Git.notNull())
            {                
                nGit.Git.GetRepository().Close();
                NGit.RepositoryCache.Clear();
                nGit.Git.Clean();
                nGit.Repository = null;
                nGit.Git = null;
                GC.Collect();
            }
            return nGit;
        }
        public static bool     delete_Repository(this API_NGit nGit)                                            
        {
            nGit.close();
            if (nGit.files_Location().valid())
            {
                var folderToDelete = nGit.git_Folder();
                if (folderToDelete.dirExists())
                {
                    //need to make the repo files read/write before deleting the repo folder
                    foreach (var file in folderToDelete.files(true))
                        file.file_Attribute_ReadOnly_Remove();
                    return Files.deleteFolder(folderToDelete, true);
                }
            }
            return false;
        }
        public static bool     delete_Repository_And_Files(this API_NGit nGit)                                  
        {
            if (nGit.delete_Repository())
                return Files.deleteFolder(nGit.Path_Local_Repository, true);
            return false;
        }
    }
}
