using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentSharp.CoreLib;
using FluentSharp.Git;
using FluentSharp.Git.APIs;
using NGit;

namespace FluentSharp.Git
{
    public static class Branch_ExtensionMethods
    {
        public static bool         branch_Create (this API_NGit nGit, string name)
        {
            if (nGit.git().notNull())
            {
                try
                {
                    var createBranch = nGit.git().BranchCreate();
                    createBranch.SetName(name);
                    var @ref = createBranch.Call();
                    return @ref.simple_BranchName() == name;
                   
                }
                catch (Exception ex)
                {
                    ex.log("[API_NGit][Repository][branch_Create]");
                }
            }
            return false;
        }        
        public static bool         branch_Delete (this API_NGit nGit, string branchName)
        {
            if(nGit.git().notNull() && branchName.notNull())
            {
                try
                {
                    var deleteBranch = nGit.git().BranchDelete();
                    deleteBranch.SetBranchNames(new [] { branchName});
                    return deleteBranch.Call().first().simple_BranchName() == branchName;
                }
                catch(Exception ex)
                {
                    ex.log("[API_NGit][branch_Delete]");
                }
            }
            return false;
        }
        public static bool         branch_Rename (this API_NGit nGit, string branchName_Old, string branchName_New)
        {
            if(nGit.git().notNull())
            {
                try
                {
                    var renameBranch = nGit.git().BranchRename();
                    renameBranch.SetOldName(branchName_Old);
                    renameBranch.SetNewName(branchName_New);
                    return renameBranch.Call().simple_BranchName()==branchName_New;
                }
                catch(Exception ex)
                {
                    ex.log("[API_NGit][branch_Rename]");
                }
            }
            return false;
        }
        public static List<Ref>    branches_Raw  (this API_NGit nGit)
        {
            if(nGit.git().notNull())
            {
                try
                {
                    var listBranches = nGit.git().BranchList();
                    return listBranches.Call().toList();                    
                }
                catch(Exception ex)
                {
                    ex.log("[API_NGit][branches_Raw]");
                }
            }
            return new List<Ref>();
        }
        public static List<string> branches      (this API_NGit nGit)
        {
            return (from @ref in nGit.branches_Raw()
                    select @ref.GetName().subString_After("refs/heads/")).toList();
        }                
        public static string       branch_Current(this API_NGit nGit)
        {
            if (nGit.repository().notNull())
                return nGit.repository().GetBranch();
            return null;
        }
         public static bool         branch_Checkout (this API_NGit nGit, string name)
        {
            if (nGit.git().notNull())
            {
                try
                {
                    var checkoutBranch = nGit.git().Checkout();
                    checkoutBranch.SetName(name);                    
                    var @ref = checkoutBranch.Call();
                    return @ref.simple_BranchName() == name;
                   
                }
                catch (Exception ex)
                {
                    ex.log("[API_NGit][Repository][branch_Checkout]");
                }
            }
            return false;
        }  

        public static bool         checkout (this API_NGit nGit, string name)
        {
            return nGit.branch_Checkout(name);
        }
    }
}
