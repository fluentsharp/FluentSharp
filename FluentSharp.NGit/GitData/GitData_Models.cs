using System.Collections.Generic;
using System.Xml.Serialization;
using FluentSharp.Git.Utils;

namespace FluentSharp.Git.APIs
{
    public class GitData_Repository : GitData_API_NGit
    {                                                
        
        [XmlAttribute]             public string                CurrentBranch       { get; set; }                
        [XmlAttribute]             public string                HEAD                { get; set; }
        [XmlElement]               public GitData_Config        Config              { get; set; }        
        [XmlArrayItem("Branch")]   public List<GitData_Branch>  Branches            { get; set; }
        [XmlArrayItem("Commit")]   public List<GitData_Commit>  Commits             { get; set; }
        [XmlArrayItem("File")]     public List<GitData_File>    Files               { get; set; }
        
        public GitData_Repository()
        {
            Config        = new GitData_Config();
            HEAD          = NGit_Consts.EMPTY_SHA1;
            Branches      = new List<GitData_Branch>();
            Commits       = new List<GitData_Commit>();
            Files         = new List<GitData_File>();             
        }
    }

    public class GitData_API_NGit
    {
        internal API_NGit nGit;
    }

    public class GitData_Config 
    {
        [XmlAttribute] public string               LocalPath           { get; set; }
        [XmlAttribute] public bool                 Load_CommitTrees    { get; set;}
        [XmlAttribute] public int                  Max_CommitsToShow   { get; set;}
        [XmlAttribute] public int                  Max_FilesToShow     { get; set;}

        public GitData_Config()
        {
            LocalPath         = null;  
            Load_CommitTrees  = false;
            Max_CommitsToShow = -1;
            Max_FilesToShow   = -1;

        }
    }
     
    public class GitData_Branch : GitData_API_NGit
    {   
        [XmlAttribute] public string                Name                { get; set; }
        [XmlAttribute] public string                Sha1                { get; set; }    
    }
    public class GitData_File
    {
        [XmlAttribute] public string                    FilePath            { get; set; }
        [XmlAttribute] public string                    Sha1                { get; set; }
        [XmlElement]   public List<GitData_File_Commit> Commits             { get; set; }

        public GitData_File()
        {
            Commits = new List<GitData_File_Commit>();
        }
    }


    public class GitData_File_Commit
    {        
        [XmlAttribute] public string                File_Sha1           { get; set; }
        [XmlAttribute] public string                Commit_Sha1         { get; set; }
        [XmlAttribute] public string                Author              { get; set; }        
        [XmlAttribute] public long                  When                { get; set; }        
    }
    public class GitData_File_Commit_Full
    {
        [XmlAttribute] public string                FilePath            { get; set; }
        [XmlAttribute] public string                Sha1                { get; set; }
        [XmlAttribute] public string                CommitId            { get; set; }
        [XmlAttribute] public string                Author              { get; set; }
        [XmlAttribute] public string                Committer           { get; set; }
        [XmlAttribute] public long                  When                { get; set; }
        [XmlAttribute] public string                FileContents        { get; set; }
    }

    public class GitData_Commit
    {
        [XmlAttribute] public string                Author              { get; set; }
        [XmlAttribute] public string                Committer           { get; set; }        
        [XmlAttribute] public string                Sha1                { get; set; }
        [XmlAttribute] public long                  When                { get; set; }        
        [XmlElement]   public string                Message             { get; set; }
        [XmlElement]   public List<string>          Parents             { get; set; }
        [XmlElement]   public List<GitData_File>    Tree                { get; set; }

        public GitData_Commit()
        {            
            Parents = new List<string>();
            Tree = new List<GitData_File>();            
        }
    }
}
