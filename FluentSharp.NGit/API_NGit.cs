// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using FluentSharp.Git.Utils;
using NGit.Api;
using NGit;
using NGit.Transport;


namespace FluentSharp.Git.APIs
{
    public class API_NGit
    {
        public string               Path_Local_Repository { get; set; }
        public NGit.Api.Git         Git                   { get; set; }        
        public Repository           Repository            { get; set; }
        public GitProgress          LastGitProgress       { get; set; }
        public CredentialsProvider  Credentials           { get; set; }        
        public PersonIdent          Author                { get; set; }
        public PersonIdent          Committer             { get; set; }
        public Exception            Last_Exception        { get; set; }
        public List<PushResult>     Last_PushResult       { get; set; }
        public FetchResult          Last_FetchResult      { get; set; }
        public PullResult           Last_PullResult       { get; set; }
        public Ref                  Last_ResetResult      { get; set; }

        public API_NGit()
        {
            Author    = new PersonIdent(NGit_Consts.DEFAULT_COMMIT_NAME, NGit_Consts.DEFAULT_COMMIT_EMAIL);
            Committer = new PersonIdent("FluentSharp NGit", "FluentSharp@o2platform.com");
        }
    }    
}
