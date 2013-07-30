// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Windows.Forms;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.WinForms;

//O2File:Ast_Engine_Cache.cs
//O2File:O2CodeStreamTaintRules_ExtensionMethods.cs
//O2File:O2CodeStream_ExtensionMethods.cs
//O2File:O2MethodStream_ExtensionMethods.cs
//O2File:O2MappedAstData_ExtensionMethods.cs


namespace FluentSharp.CSharpAST.Utils
{
    public static class Ast_Engine_ExtensionMethods
    {    
      
    	public static string createAllMethodsStreams(this O2MappedAstData astData)
		{
			var targetFolder = PublicDI.config.getTempFolderInTempDirectory("_MethodStream");
			return astData.createAllMethodsStreams(targetFolder);
		}
		
		public static string createAllMethodsStreams(this O2MappedAstData astData, string targetFolder)
		{
			return astData.createAllMethodsStreams(targetFolder, null,null);
		}
		public static string createAllMethodsStreams(this O2MappedAstData astData, string targetFolder, ProgressBar TopProgressBar, Func<string,bool> statusMessage)
		{				
			var iMethods = astData.iMethods(astData.methodDeclarations()); 
			TopProgressBar.maximum(iMethods.size());
			int count = 0;
			int total = iMethods.size();
			//this.MethodStreams = new Dictionary<IMethod, string>();    		
			foreach(var iMethod in iMethods)
			{
				TopProgressBar.increment(1);				
				astData.createO2MethodStreamFile(iMethod,targetFolder);
				
    			//MethodStreams.Add(iMethod,AstData.createO2MethodStream(iMethod).csharpCode());  
    			if (statusMessage.notNull())
					if (statusMessage("calculating methodStream: {0}/{1}".format(count++, total)).isFalse())
					{
						"Scan Cancel/Stop request received".info();
						return targetFolder;
					}	
			}
			return targetFolder;
		}
    	    	    	    	    
    }
}
