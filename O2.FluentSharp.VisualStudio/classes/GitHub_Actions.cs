// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
/*
 * using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using NGit.Api;
using O2.Kernel;
//O2Ref:Ngit.dll
//O2Ref:Sharpen.dll

namespace O2.Platform
{  
	public class GitHub_Actions
	{
		public static string 			TargetDir 		{ get; set; }
		public static string 			GitUrl_Template { get; set; }
		public static Action<string>   	LogMessage   	{ get;set;}
		
		
		static GitHub_Actions()
		{
			LogMessage = (message) => Console.WriteLine("* " + message);
			var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
			if (baseDirectory.Contains("O2") == false)
				baseDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			TargetDir = Path.GetFullPath(Path.Combine(baseDirectory, "..\\GitHub.Repositories"));
			//TargetDir = Path.GetFullPath(PublicDI.config.defaultO2LocalTempFolder.pathCombine( "..\\GitHub.Repositories"));
			GitUrl_Template = "git://github.com/o2platform/{0}.git";
		}
		
		public static void start()
		{
			LogMessage(" in GitHub_Actions");
			CloneRepository("O2.Platform.Exe");
			CloneRepository("O2.Platform.Scripts");		
		}
		
		public static void CloneRepository(string repositoryName )
		{
			Git git = null;
			var repositoryUrl	= String.Format(GitUrl_Template	, repositoryName);
			var repositoryPath 	= Path.Combine (TargetDir		, repositoryName);
			
			if (Directory.Exists(repositoryPath))
			{
				git = Git.Open(repositoryPath);
				LogMessage(" Pulling Repository: " + repositoryUrl);
				LogMessage(" .. starting pull");
				var pullResponse = git.Pull().Call();
				LogMessage("    " + pullResponse.ToString());
				LogMessage(" .. pull completed");
			}
			else
			{
				LogMessage(" Cloning Repository: " + repositoryUrl);
				LogMessage("  into Folder: " + repositoryPath);
				
				var cloneCommand = Git.CloneRepository();
				
				cloneCommand.SetDirectory(repositoryPath);			
				cloneCommand.SetURI(repositoryUrl);
				
				LogMessage(" .. starting clone");
				git = cloneCommand.Call(); 
				
				LogMessage(" .. clone completed");
			}
			var repository = git.GetRepository();
			
			LogMessage("  local repository parh: " + repository);
		}
		
	}
}*/