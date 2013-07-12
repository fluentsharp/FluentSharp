// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)

using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;

namespace FluentSharp.Git.APIs
{
    public class API_NGit_O2Platform : API_NGit
    {
        public string GitHub_O2_Repositories { get; set; }
        public string LocalGitRepositories { get; set; }

        public API_NGit_O2Platform()
        {
            LocalGitRepositories = PublicDI.config.CurrentExecutableDirectory.pathCombine("..\\..").fullPath(); // by default it is two above the current one
            GitHub_O2_Repositories = "https://github.com/o2platform/{0}.git";
        }

        public API_NGit_O2Platform(string targetPath) : this()
        {
            LocalGitRepositories = targetPath;
        }
    }
}