using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NGit;
using NGit.Api;
using FluentSharp.ExtensionMethods;

namespace FluentSharp.ExtensionMethods
{
    public static class API_NGit_ExtMet_Status
    {
        public static string        status  (this API_NGit nGit                                               )
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
        public static List<string>  status_Added        (this API_NGit nGit                               ) 
        {
            return nGit.status_Raw().GetAdded().toList();
        }
        public static List<string>  status_Changed      (this API_NGit nGit                               ) 
        {
            return nGit.status_Raw().GetChanged().toList();
        }
        public static List<string>  status_Conflicting  (this API_NGit nGit                               ) 
        {
            return nGit.status_Raw().GetConflicting().toList();
        }    
        public static List<string>  status_Missing      (this API_NGit nGit                               ) 
        {
            return nGit.status_Raw().GetMissing().toList();
        }                
        public static List<string>  status_Modified     (this API_NGit nGit                               ) 
        {
            return nGit.status_Raw().GetModified().toList();
        }    
        public static List<string>  status_Untracked    (this API_NGit nGit                               ) 
        {
            return nGit.status_Raw().GetUntracked().toList();
        }
        public static List<string>  status_Removed      (this API_NGit nGit                               ) 
        {
            return nGit.status_Raw().GetRemoved().toList();
        }

       public static Status         status_Raw          (this API_NGit nGit                               ) 
        {
            return nGit.Git.Status().Call();
        }   
    }
}
