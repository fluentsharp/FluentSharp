using System;
using FluentSharp.CoreLib;
using FluentSharp.Git.APIs;
using FluentSharp.Git.Utils;
using NGit;

namespace FluentSharp.Git
{
    public static class Misc_ExtensionMethods
    {
        public static string       git_Folder(this API_NGit nGit)
        {
            return nGit.files_Location().pathCombine(".git");
        }        
        public static NGit.Api.Git git(this API_NGit nGit)
        {
            if (nGit.notNull())
                return nGit.Git;
            return null;
        }        
        public static ObjectId    objectId(this String objectId)
        {
            try
            {
                if (objectId.notValid() || objectId == "0")
                    return ObjectId.FromString(NGit_Consts.EMPTY_SHA1);
                return ObjectId.FromString(objectId);
            }
            catch (Exception ex)
            {
                ex.log("String.objectId");
                return null;
            }            
        }        
        public static PersonIdent personIdent(this string name, string email)
        {
            return new PersonIdent(name,email);
        }
        public static string      name(this PersonIdent personIdent)
        {
            if (personIdent.notNull())
                return personIdent.GetName();
            return null;
        }
        public static string      email(this PersonIdent personIdent)
        {
            if (personIdent.notNull())
                return personIdent.GetEmailAddress();
            return null;
        }
        public static DateTime    when(this PersonIdent personIdent)
        {
            if (personIdent.notNull())
                return personIdent.GetWhen();
            return default(DateTime);
        }
        public static string      changeBackslashWithForwardSlash(this string targetString)
        {
            return targetString.replace(@"\\", @"/");
        }
        public static string      fixDoubleForwardSlash(this string targetString)
        {
            return targetString.replace(@"//", @"/");
        }
        public static string      simple_BranchName    (this string refName)
        {
            if(refName.notNull())                
                return refName.starts("refs/heads/")
                            ? refName.subString_After("refs/heads/")
                            : refName;
            return null;
        }
        public static string      simple_BranchName    (this Ref @ref)
        {
            if (@ref.notNull())            
                return @ref.GetName().simple_BranchName();                
            return null;
        }
    }
}
