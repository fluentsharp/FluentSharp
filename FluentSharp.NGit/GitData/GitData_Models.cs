using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using FluentSharp.Git.Utils;

namespace FluentSharp.Git.APIs
{
    public class GitData_Repository
    {
        [XmlAttribute] public string               LocalPath           { get; set; }        
        [XmlElement]   public GitData_Branch       CurrentBranch       { get; set; }        
        
        [XmlAttribute] public bool                 ShowingCommitTrees  { get; set;}
        public GitData_Repository()
        {
            CurrentBranch = new GitData_Branch();           
        }
    }
    
    public class GitData_Branch
    {   
        [XmlAttribute] public string                HEAD                { get; set; }
        [XmlElement]   public List<GitData_Commit>  Commits             { get; set; }
        [XmlElement]   public List<GitData_File>    Files               { get; set; }
        

        public GitData_Branch()
        {
            HEAD = NGit_Consts.EMPTY_SHA1;

            Commits       = new List<GitData_Commit>();
            Files = new List<GitData_File>();            
        }
    }
    public class GitData_File
    {
        [XmlAttribute] public string                FilePath            { get; set; }
        [XmlAttribute] public string                Sha1                { get; set; }
    }
    public class GitData_File_Commit
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
        [XmlElement]   public List<GitData_File>    Tree               { get; set; }

        public GitData_Commit()
        {            
            Tree = new List<GitData_File>();            
        }
    }
}
