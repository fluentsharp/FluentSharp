using System.Collections.Generic;
using FluentSharp.Git.APIs;
using NGit.Api;
using FluentSharp.CoreLib;

namespace FluentSharp.Git
{
    public static class Status_ExtensionMethods
    {
        public static string        status       (this API_NGit nGit)
        {
            var status = nGit.status_Raw();
            if (status.isNull())
                return null;
            var added = status.added();
            var changed = status.changed();
            var modified = status.modified();
            var removed = status.removed();
            var missing = status.missing();            
            var untracked = status.untracked();
            var conflicting = status.conflicting();

            var statusDetails = ((added.Count > 0) ? "Added: {0}\n".format(added.join(" , ")) : "") +
                                ((changed.Count > 0) ? "changed: {0}\n".format(changed.join(" , ")) : "") +
                                ((removed.Count > 0) ? "removed: {0}\n".format(removed.join(" , ")) : "") +
                                ((missing.Count > 0) ? "missing: {0}\n".format(missing.join(" , ")) : "") +
                                ((modified.Count > 0) ? "modified: {0}\n".format(modified.join(" , ")) : "") +
                                ((untracked.Count > 0) ? "untracked: {0}\n".format(untracked.join(" , ")) : "") +
                                ((conflicting.Count > 0) ? "conflicting: {0}\n".format(conflicting.join(" , ")) : "");
            return statusDetails;
        }
        public static List<string>  added        (this Status status) 
        {
            if (status.notNull())
                return status.GetAdded().toList();            
            return new List<string>();
        }
        public static List<string>  changed      (this Status status) 
        {
            if (status.notNull())
                return status.GetChanged().toList();
            return new List<string>();            
        }
        public static List<string>  conflicting  (this Status status) 
        {
            if (status.notNull())
                return status.GetConflicting().toList();
            return new List<string>();                
        }    
        public static List<string>  missing      (this Status status) 
        {
            if (status.notNull())
                return status.GetMissing().toList();
            return new List<string>();                                         
        }                
        public static List<string>  modified     (this Status status) 
        {
            if (status.notNull())
                return status.GetModified().toList();
            return new List<string>();                             
        }    
        public static List<string>  untracked    (this Status status) 
        {
            if (status.notNull())
                return status.GetUntracked().toList();
            return new List<string>();                 
        }
        public static List<string>  removed      (this Status status) 
        {
            if (status.notNull())
                return status.GetRemoved().toList();
            return new List<string>();                 
        }
        public static Status        status_Raw   (this API_NGit nGit) 
        {
            if (nGit.notNull())
                return nGit.Git.Status().Call();
            return null;
        }   
    }
}
