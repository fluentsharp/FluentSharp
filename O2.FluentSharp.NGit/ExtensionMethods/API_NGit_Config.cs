using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NGit;

namespace FluentSharp.ExtensionMethods
{
    public static class API_NGit_Config
    {
        public static StoredConfig config(this API_NGit nGit)
        {
            if (nGit.repository().notNull())
                return nGit.repository().GetConfig();
            return null;
        }
        public static List<string> config_Sections(this API_NGit nGit)
        {
            if (nGit.repository().notNull())
                return nGit.config().GetSections().toList();
            return new List<string>();
        }
        public static List<string> config_SubSections(this API_NGit nGit, string sectionName)
        {
            if (nGit.repository().notNull())
                try
                {
                    return nGit.config().GetSubsections(sectionName).toList();
                }
                catch (Exception ex)
                {
                    ex.log("[API_NGit][config_SubSections]");
                }
            return new List<string>();
        }

        public static List<string> remotes(this API_NGit nGit)
        {
            return nGit.config_SubSections("remote");
        }
        
        public static bool remote_Add(this API_NGit nGit, string remoteName, string url)
        {
            if (nGit.repository().notNull() && remoteName.valid() && url.valid())
                //try   // can't trigger from UnitTest
            {
                nGit.config().SetString("remote", remoteName, "url", url);
                nGit.config().Save();
                return true;
            }
                /*catch (Exception ex)
                {
                    ex.log("[API_NGit][remote_Add]");
                }*/
            return false;
        }
        public static bool remote_Delete(this API_NGit nGit, string remoteName)
        {
            if (nGit.repository().notNull() && remoteName.valid())
                //try   // can't trigger from UnitTest
            {
                nGit.config().UnsetSection("remote", remoteName);
                return true;
            }
                /*catch (Exception ex)
                {
                    ex.log("[API_NGit][remote_Add]");
                }*/
            return false;
        }
        public static string remote_Url(this API_NGit nGit, string remoteName)
        {
            if (nGit.repository().notNull())
                //try   // can't trigger from UnitTest
                {                    
                    return nGit.config().GetString("remote", remoteName, "url");                    
                }
                /*catch (Exception ex)
                {
                    ex.log("[API_NGit][remote_Url]");
                }*/
            return null;
        }
    }
}
