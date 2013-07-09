using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NGit.Transport;

namespace FluentSharp.ExtensionMethods
{
    public static class API_NGit_Fetch_Merge
    {
        public static bool fetch(this API_NGit nGit)
        {
            return nGit.fetch("origin", "master", "master");
        }

        public static bool fetch(this API_NGit nGit, string remote, string fromBranch, string toBranch)
        {
            if (nGit.git().notNull())
            {
                try
                {
                    nGit.Last_FetchResult = null;
                    nGit.Last_Exception = null;
                    var fetchCommand = nGit.git().Fetch();
                    fetchCommand.SetRemote(remote);
                    var spec = new RefSpec("refs/heads/{0}:refs/heads/{1}".format(fromBranch, toBranch));
                    fetchCommand.SetRefSpecs(spec);
                    nGit.Last_FetchResult = fetchCommand.Call();
                    return true;
                }
                catch (Exception ex)
                {
                    nGit.Last_Exception = ex;
                    ex.log("[API_NGit][fetch]");
                }
            }
            return false;
        }
    }
}
