// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using FluentSharp.ExtensionMethods;
using O2.Kernel;
using FluentSharp.ExtensionMethods;

namespace FluentSharp
{
    public class API_NGit_O2Platform : API_NGit
    {
        public string GitHub_O2_Repositories { get; set; }
        public string LocalGitRepositories { get; set; }

        public API_NGit_O2Platform()
        {
            GitHub_O2_Repositories = "git://github.com/o2platform/{0}.git";
            LocalGitRepositories = PublicDI.config.CurrentExecutableDirectory.pathCombine("..\\..").fullPath(); // by default it is two above the current one
        }
    }

    public static class API_NGit_O2Platform_ExtensionMethods
    {
        public static API_NGit_O2Platform cloneOrPull(this API_NGit_O2Platform nGit_O2, string repositoryName)
        {
            var repositoryUrl = nGit_O2.GitHub_O2_Repositories.format(repositoryName);
            var localPath = nGit_O2.LocalGitRepositories.pathCombine(repositoryName);
            if (localPath.isGitRepository())
                nGit_O2.open(localPath).pull();
            else
                nGit_O2.clone(repositoryUrl, localPath);

            return nGit_O2;
        }
    }
}