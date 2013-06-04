using System.Collections.Generic;
using NGit.Diff;
using O2.DotNetWrappers.ExtensionMethods;
using Sharpen;

namespace O2.FluentSharp.ExtensionMethods
{
    public static class API_NGit_ExtMet_Diff
    {
        public static string            diff    (this API_NGit nGit)
        {
            var outputStream = "Sharpen.dll".assembly()
                                            .type("ByteArrayOutputStream")
                                            .ctor()
                                            .cast<OutputStream>();
            nGit.diff_Raw(outputStream);
            return outputStream.str();
        }
        public static IList<DiffEntry>  diff_Raw(this API_NGit nGit)
        {
            return nGit.diff_Raw(null);
        }
        public static IList<DiffEntry>  diff_Raw(this API_NGit nGit, OutputStream outputStream)
        {
            var diff = nGit.Git.Diff();
            if (outputStream.notNull())
                diff.SetOutputStream(outputStream);
            return diff.Call();
        }
    }
}
