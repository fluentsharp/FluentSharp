using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NGit.Api;
using FluentSharp.ExtensionMethods;

namespace O2.FluentSharp.ExtensionMethods
{
    public static class API_NGit_ExtMet_Init_Clone_Open
    {
        public static API_NGit  init    (this API_NGit nGit, string pathToLocalRepository                 )
        {
            try
            {
                "[API_NGit] init: {0}".debug(pathToLocalRepository);
                var init_Command = Git.Init();

                init_Command.SetDirectory(pathToLocalRepository);
                nGit.Git = init_Command.Call();
                nGit.Repository = nGit.Git.GetRepository();
                nGit.Path_Local_Repository = pathToLocalRepository;
                return nGit;
            }
            catch (Exception ex)
            {
                ex.log("[API_NGit] ");
            }
            return null;
        }        
        public static API_NGit  open    (this API_NGit nGit, string pathToLocalRepository                 )
        {
            try
            {
                "[API_NGit] open: {0}".debug(pathToLocalRepository);

                nGit.Git = Git.Open(pathToLocalRepository);
                nGit.Repository = nGit.Git.GetRepository();
                nGit.Path_Local_Repository = pathToLocalRepository;
                return nGit;
            }
            catch (Exception ex)    
            {
                ex.log("[API_NGit] ");
            }
            return null;
        }
        public static API_NGit  clone   (this API_NGit nGit, string sourceRepository, string targetFolder )
        {
            "[API_NGit] cloning: {0} into {1}".debug(sourceRepository, targetFolder);
            try
            {
                //need to find a better way to do this
                /*if (sourceRepository.str().ping().isFalse())
                {
                    "[API_NGit] it looks like we are offline, or the provided Uri cannot be reached: {0}".error(sourceRepository);
                    return null;
                }*/
                if (targetFolder.dirExists())
                {
                    "[API_NGit] provided target folder already exists,please delete it or provide a difference one: {0}".error(targetFolder);
                    return null;
                }
                var start = DateTime.Now;
                var clone_Command = Git.CloneRepository();
                clone_Command.SetDirectory(targetFolder);
                clone_Command.SetURI(sourceRepository);
                nGit.LastGitProgress = new GitProgress();
                clone_Command.SetProgressMonitor(nGit.LastGitProgress);
                clone_Command.Call();
                "[API_NGit] clone completed in: {0}".debug(start.timeSpan_ToString());
                return nGit;
            }
            catch (Exception ex)
            {
                ex.log("[API_NGit] ");
            }
            return null;
        }

        public static API_NGit  git_Init    (this string targetFolder                                     )
        {
            if (targetFolder.isGitRepository())
            {
                "[API_NGit][git_Init] tried to init a repository in a folder that already has a git repository: {0}"
                    .error(targetFolder);
                return null;
            }
            return new API_NGit().init(targetFolder);
        }
        public static API_NGit  git_Open    (this string targetFolder                                     )
        {
            return new API_NGit().open(targetFolder);
        }
        public static API_NGit  git_Clone   (this Uri sourceRepository, string targetFolder               )
        {
            return sourceRepository.str().git_Clone(targetFolder);
        }
        public static API_NGit  git_Clone   (this string sourceRepository, string targetFolder            )
        {
            return new API_NGit().clone(sourceRepository, targetFolder);
        }            
    }
}
