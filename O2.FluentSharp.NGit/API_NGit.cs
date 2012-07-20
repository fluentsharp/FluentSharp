// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.IO;
using System.Collections.Generic;
using O2.Kernel.ExtensionMethods;
using O2.DotNetWrappers.ExtensionMethods;
using NGit.Api;
using NGit;

//O2Ref:O2_FluentSharp_NGit.dll
//O2Ref:NGit.dll
//O2Ref:NSch.dll
//O2Ref:Mono.Security.dll
//O2Ref:Sharpen.dll


namespace O2.XRules.Database.APIs
{
    public class API_NGit
    {
        public string Path_Local_Repository { get; set; }
        public Git Git { get; set; }
        public Repository Repository { get; set; }
        public GitProgress LastGitProgress { get; set; }
    }

    public class GitProgress : TextProgressMonitor
    {
        public Action<string, string, int> onMessage { get; set; }
        public StringWriter FullMessage { get; set; }

        public GitProgress()
            : this(new StringWriter())
        {
        }
        public GitProgress(StringWriter stringWriter)
            : base(stringWriter)
        {
            FullMessage = stringWriter;

            onMessage = (type, taskName, workCurr) =>
            {
                "[GitProgress] {0} : {1} : {2}".info(type, taskName, workCurr);
            };
        }

        public override void Start(int totalTasks)
        {
            //onMessage("Start","",totalTasks);
            base.Start(totalTasks);
        }

        public override void BeginTask(string title, int work)
        {
            onMessage("BeginTask", title, work);
            base.BeginTask(title, work);
        }

        public override void Update(int completed)
        {
            //onMessage("Update", "", completed);
            base.Update(completed);
        }

        public override void EndTask()
        {
            //onMessage("EndTask", "", -1);
            base.EndTask();
        }
    }

    public static class API_NGit_ExtensionMethods
    {
        public static API_NGit init(this API_NGit nGit, string pathToLocalRepository)
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

        public static API_NGit git_Open(this string pathToLocalRepository)
        {
            return new API_NGit().open(pathToLocalRepository);
        }

        public static API_NGit open(this API_NGit nGit, string pathToLocalRepository)
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

        public static API_NGit git_Clone(this Uri sourceRepository, string targetFolder)
        {
            return sourceRepository.str().git_Clone(targetFolder);
        }

        public static API_NGit git_Clone(this string sourceRepository, string targetFolder)
        {
            return new API_NGit().clone(sourceRepository, targetFolder);
        }

        public static API_NGit clone(this API_NGit nGit, string sourceRepository, string targetFolder)
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

        public static API_NGit add(this API_NGit nGit, string filePattern)
        {
            "[API_NGit] add: {0}".debug(filePattern);

            var add_Command = nGit.Git.Add();
            add_Command.AddFilepattern(filePattern);
            add_Command.Call();
            return nGit;
        }
        public static API_NGit commit(this API_NGit nGit, string commitMessage)
        {
            "[API_NGit] commit: {0}".debug(commitMessage);

            if (commitMessage.valid())
            {
                var commit_Command = nGit.Git.Commit();
                commit_Command.SetMessage(commitMessage);
                commit_Command.Call();
            }
            else
                "[API_NGit] commit was called with no commitMessage".error();
            return nGit;
        }

        public static API_NGit push(this API_NGit nGit)
        {
            return nGit.push("origin");
        }

        public static API_NGit push(this API_NGit nGit, string remote)
        {
            "[API_NGit] push: {0}".debug(remote);

            var push_Command = nGit.Git.Push();
            push_Command.SetRemote(remote);
            nGit.LastGitProgress = new GitProgress();
            push_Command.SetProgressMonitor(nGit.LastGitProgress);
            push_Command.Call();

            "[API_NGit] push completed".debug();
            return nGit;
        }

        public static API_NGit git_Pull(this string repository)
        {
            return repository.git_Open()
                             .pull();
        }
        public static API_NGit pull(this API_NGit nGit) //, string remote)
        {
            //"[API_NGit] pull start".debug();
            var pull_Command = nGit.Git.Pull();
            //pull_Command.SetRemote(remote);	
            nGit.LastGitProgress = new GitProgress();
            pull_Command.SetProgressMonitor(nGit.LastGitProgress);
            pull_Command.Call();

            "[API_NGit] pull completed".debug();
            return nGit;
        }

        public static API_NGit add_and_commit_using_Status(this API_NGit nGit)
        {
            nGit.add(".");
            nGit.commit_using_Status();
            return nGit;
        }
        public static API_NGit commit_using_Status(this API_NGit nGit)
        {
            nGit.commit(nGit.status());
            return nGit;
        }

        public static string status(this API_NGit nGit)
        {
            var statusCommand = nGit.Git.Status();
            var status = statusCommand.Call();

            var added = status.GetAdded().toList();
            var changed = status.GetChanged().toList();
            var removed = status.GetRemoved().toList();
            var missing = status.GetMissing().toList();
            var modified = status.GetModified().toList();
            var untracked = status.GetUntracked().toList();
            var conflicting = status.GetConflicting().toList();


            var statusDetails = ((added.Count > 0) ? "Added: {0}\n".format(added.join(" , ")) : "") +
                                ((changed.Count > 0) ? "changed: {0}\n".format(changed.join(" , ")) : "") +
                                ((removed.Count > 0) ? "removed: {0}\n".format(removed.join(" , ")) : "") +
                                ((missing.Count > 0) ? "missing: {0}\n".format(missing.join(" , ")) : "") +
                                ((modified.Count > 0) ? "modified: {0}\n".format(modified.join(" , ")) : "") +
                                ((untracked.Count > 0) ? "untracked: {0}\n".format(untracked.join(" , ")) : "") +
                                ((conflicting.Count > 0) ? "conflicting: {0}\n".format(conflicting.join(" , ")) : "");
            return statusDetails;
        }


    }


    public static class API_NGit_ExtensionMethods_File_Utils
    {
        public static API_NGit writeFile(this API_NGit nGit, string virtualFileName, string fileContents)
        {
            var fileToWrite = nGit.Path_Local_Repository.pathCombine(virtualFileName);
            fileContents.saveAs(fileToWrite);
            return nGit;
        }

        public static bool isGitRepository(this string pathToFolder)
        {
            return pathToFolder.dirExists() && pathToFolder.pathCombine(".git").dirExists();
        }
    }
}
