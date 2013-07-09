using System;
using System.Collections.Generic;
using FluentSharp.Git.APIs;
using FluentSharp.Git.Utils;
using NGit.Diff;
using FluentSharp.CoreLib;
using NGit.Revwalk;
using Sharpen;

namespace FluentSharp.Git
{
    public static class Diff_ExtensionMethods
    {
        public static string            diff    (this API_NGit nGit)        
        {
            var outputStream = NGit_Factory.New_OutputStream();
            nGit.diff_Raw(outputStream);
            return outputStream.str();
        }
        public static string            diff(this API_NGit nGit, RevCommit from_RevCommit, RevCommit to_RevCommit)
        {
            return nGit.diff(from_RevCommit.sha1(), to_RevCommit.sha1());
        }
        public static string            diff(this API_NGit nGit, string from_CommitId, string to_CommitId)
        {
            if (nGit.repository().notNull())
                try
                {
                    var fromCommit = nGit.resolve(from_CommitId);
                    var toCommit = nGit.resolve(to_CommitId);
                    var outputStream = NGit_Factory.New_OutputStream();
                    var diffFormater = new DiffFormatter(outputStream);
                    diffFormater.SetRepository(nGit.repository());
                    diffFormater.Format(fromCommit, toCommit);
                    return outputStream.str();
                }
                catch (Exception ex)
                {
                    ex.log("[API_NGit][diff]");
                }
            return null;
        }
        public static List<DiffEntry>   diff_Commits(this API_NGit nGit, RevCommit from_RevCommit, RevCommit to_RevCommit)
        {
            if (nGit.repository().notNull())
                try
                {                    
                    var outputStream = NGit_Factory.New_OutputStream();
                    var diffFormater = new DiffFormatter(outputStream);
                    diffFormater.SetRepository(nGit.repository());
                    return diffFormater.Scan(from_RevCommit, to_RevCommit).toList();                    
                }
                catch (Exception ex)
                {
                    ex.log("[API_NGit][diff]");
                }
            return new List<DiffEntry>();
        }
        public static List<DiffEntry>   diff_Raw(this API_NGit nGit)        
        {
            return nGit.diff_Raw(null);
        }
        public static List<DiffEntry>   diff_Raw(this API_NGit nGit, OutputStream outputStream)
        {
            var diff = nGit.Git.Diff();
            if (outputStream.notNull())
                diff.SetOutputStream(outputStream);
            return diff.Call().toList();
        }
        public static string            changeType(this DiffEntry diffEntry)
        {
            if (diffEntry.notNull())
                return diffEntry.GetChangeType().str();
            return null;
        }
        public static string            path(this DiffEntry diffEntry)      
        {
            if (diffEntry.notNull())
                return diffEntry.GetNewPath();
            return null;
        }
        public static string            sha1(this DiffEntry diffEntry)      
        {
            if (diffEntry.notNull())
                return diffEntry.GetNewId().Name;
            return null;
        }
    }
}
