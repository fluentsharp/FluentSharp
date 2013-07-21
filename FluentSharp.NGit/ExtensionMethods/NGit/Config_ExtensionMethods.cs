using System;
using System.Collections.Generic;
using FluentSharp.CoreLib;
using FluentSharp.Git.APIs;
using NGit;
using NGit.Storage.File;

namespace FluentSharp.Git
{
    public static class Config_ExtensionMethods
    {
        public static StoredConfig      config(this API_NGit nGit)
        {
            if (nGit.repository().notNull())
                return nGit.repository().GetConfig();
            return null;
        }
        public static FileBasedConfig   config_Repo (this API_NGit nGit)
        {
            return nGit.config() as FileBasedConfig;
        }
        public static FileBasedConfig   config_Global(this API_NGit nGit)
        {
            var fs = NGit.Util.FS.DETECTED;
            var systemReader = NGit.Util.SystemReader.GetInstance();
            var userConfig = systemReader.OpenUserConfig(null, fs);
            userConfig.Load();
            return userConfig;
        }
        public static FileBasedConfig   config_System(this API_NGit nGit)
        {
            var fs = NGit.Util.FS.DETECTED;
            var systemReader = NGit.Util.SystemReader.GetInstance();
            var userConfig = systemReader.OpenSystemConfig(null, fs);
            userConfig.Load();
            return userConfig;
        }
                
        public static List<string>  config_Sections(this API_NGit nGit)
        {
            if (nGit.repository().notNull())
                return nGit.config().GetSections().toList();
            return new List<string>();
        }
        public static List<string>  config_SubSections(this API_NGit nGit, string sectionName)
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

        public static string        file_Path     (this FileBasedConfig fileBasedConfig)
        {
            if (fileBasedConfig.notNull())
                return fileBasedConfig.GetFile();
            return null;
        }
        public static string        file_Contents (this FileBasedConfig fileBasedConfig)
        {
            return fileBasedConfig.file_Path().fileContents();
        }

        public static List<string>  section_Names (this FileBasedConfig config, string section)
        {            
            if(config.notNull())
                return config.GetNames(section).toList();
            return new List<string>();
        }
        public static string  section_Get_Value_String(this FileBasedConfig config, string section, string name)
        {            
            if(config.notNull())
                return config.GetString(section, null, name);
            return null;
        }
        public static FileBasedConfig  section_Set_Value_String(this FileBasedConfig config, string section, string name, string value)
        {            
            if(config.notNull())
            {
                config.SetString(section, null, name, value);
                config.Save();
            }
            return config;
        }

        public static List<string>  remotes      (this API_NGit nGit)
        {
            return nGit.config_SubSections("remote");
        }        
        public static bool          remote_Add   (this API_NGit nGit, string remoteName, string url)
        {
            if (nGit.repository().notNull() && remoteName.valid() && url.valid())            
            {
                //no try-catch becasue can't trigger from UnitTest
                nGit.config().SetString("remote", remoteName, "url", url);
                nGit.config().Save();
                return true;
            }                
            return false;
        }
        public static bool          remote_Delete(this API_NGit nGit, string remoteName)
        {
            if (nGit.repository().notNull() && remoteName.valid())                
            {
                //no try-catch becasue can't trigger from UnitTest
                nGit.config().UnsetSection("remote", remoteName);
                return true;
            }
            return false;
        }
        public static string        remote_Url   (this API_NGit nGit, string remoteName)
        {
            if (nGit.repository().notNull())                
            {                    
                //no try-catch becasue can't trigger from UnitTest
                return nGit.config().GetString("remote", remoteName, "url");                    
            }            
            return null;
        }
    }
}
