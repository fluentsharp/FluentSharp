// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)

using System.Collections.Generic;
using FluentSharp.CoreLib;

namespace FluentSharp.CSharpAST.Utils
{
    public class Ast_Engine_Cache
    {    
    	public static Dictionary<string, O2MappedAstData> Cached_O2MappedAstData { get; set;}
    	public static bool CacheEnabled { get; set; }	
 		static Ast_Engine_Cache()
 		{ 			
 			Cached_O2MappedAstData = new Dictionary<string, O2MappedAstData>();
 			CacheEnabled = true;
 		}
 		
 		public static Dictionary<string, O2MappedAstData> cache_AstData()
 		{
 			return Cached_O2MappedAstData;
 		}
 		
 		public static Dictionary<string, O2MappedAstData> clear()
 		{
 			"[Ast_Engine_Cache] clearing cache".info();
 			Cached_O2MappedAstData.Clear();
 			return Cached_O2MappedAstData;
 		}
 		
 		public static O2MappedAstData get(string file)
 		{
 			if (CacheEnabled && Cached_O2MappedAstData.hasKey(file))
 			{
// 				"[Ast_Engine_Cache]  using O2MappedAstData cached version of file: {0}".debug(file);
 				return Cached_O2MappedAstData[file];
 			}
// 			"[Ast_Engine_Cache]  creating O2MappedAstData for file: {0}".debug(file);
			var astData = new O2MappedAstData();
			astData.loadFile(file);
			if (CacheEnabled)
				Cached_O2MappedAstData.add(file, astData);
			return astData;	
 		}
    }
}
