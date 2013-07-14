// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using FluentSharp.CoreLib.API;

namespace FluentSharp.CoreLib.Utils
{
    public class FileCache
    {    
    	public string PathLocalCache {get;set;}
    	public bool UseBase64EncodedStringInFileName {get;set;}
    	public string defaultCacheExtension = ".o2Cache";
    	public bool DisableCache { get; set; }
    	
    	public FileCache(string pathLocalCache)
    	{            
    		if (pathLocalCache.contains("\\","/").isFalse())
    			pathLocalCache = pathLocalCache.tempDir(false);
    		PathLocalCache = pathLocalCache;
    		Files.checkIfDirectoryExistsAndCreateIfNot(PathLocalCache);
    	}
    	
    	public string getCacheAddress(string itemPath)
		{
			return getCacheAddress(itemPath,defaultCacheExtension);
		}
		
    	public string getCacheAddress(string itemPath, string cacheSaveExtension)
		{
			var fileName = itemPath.safeFileName(UseBase64EncodedStringInFileName);
			if (PathLocalCache.size() + fileName.size() > 250)
			{
				"in getCacheAddress, the combined filename was too big: {0} + {1}".error(PathLocalCache, fileName);
				fileName = fileName.Substring(0, PathLocalCache.size() + fileName.size()  - 250);
				"in getCacheAddress, new fileCacheName (with size {0}): {1}".error(fileName.size(), fileName);
			}
			return PathLocalCache.pathCombine(fileName + cacheSaveExtension);
		}
		
		public string cacheGet(string uri)
		{
			return cacheGet(uri,defaultCacheExtension, null);
		}
		
		public string cacheGet(string itemPath, string cacheSaveExtension)
		{
			return cacheGet(itemPath,cacheSaveExtension, null);
		}
		
		public string cacheGet(string itemPath,  Func<string,string> getFunction)
		{
			return cacheGet(itemPath , ()=> {return getFunction(itemPath);} );
		}
		
		public string cacheGet(string itemPath,  Func<string> getFunction)
		{
			return cacheGet(itemPath, defaultCacheExtension ,getFunction);
		}
		 		 
		public string cacheGet(string itemPath, string cacheSaveExtension, Func<string> getFunction)
		{
			if (DisableCache)
				return getFunction();
				
			var cacheAddress = getCacheAddress(itemPath,cacheSaveExtension);
			
			if (cacheAddress.fileExists())
			{
				"[FileCache] returning data from local cache: {0}".debug(cacheAddress);
				return cacheAddress.fileContents();
			}
			else if (getFunction.notNull())
				{
					var result = getFunction();
					if (result.valid())
					{
						result.saveAs(cacheAddress);
						return result;
					}
					else
						"[FileCache] getFunction returned not data".error();
				}			
			return "";
			
		}
		public string cacheGet_File(string itemPath)
		{
			return 	cacheGet_File(itemPath, defaultCacheExtension);
		}
		
		public string cacheGet_File(string itemPath, string cacheSaveExtension)
		{
			if (DisableCache)
				return "";
			var cacheAddress = getCacheAddress(itemPath, cacheSaveExtension);
			
			if (cacheAddress.fileExists())
			{
				"[FileCache] found data in local cache: {0}".debug(cacheAddress);
				return cacheAddress;
			}
			return "";
		}
		
		public string cachePut(string itemPath, string cacheValue)
		{
			return 	cachePut(itemPath, defaultCacheExtension,cacheValue);
		}
		
		public string cachePut(string itemPath, string cacheSaveExtension, string cacheValue)
		{
			if (DisableCache)
				return "";
			cacheValue = cacheValue ?? "";
			var cacheAddress = getCacheAddress(itemPath,cacheSaveExtension);//  PathLocalCache.pathCombine(itemPath.safeFileName(UseBase64EncodedStringInFileName) + cacheSaveExtension);
			cacheValue.saveAs(cacheAddress);
			return cacheAddress;
		}
		
		public void cacheRemove(string itemPath)
		{
			var localFile = getCacheAddress(itemPath);
			if (localFile.fileExists())
			{
				"deleting caceh file:{0}".info(itemPath);
				Files.deleteFile(localFile);
			}
			
		}
		public void clearCache()
		{
			if (PathLocalCache.dirExists())
				foreach(var file in PathLocalCache.files())
				{
					"deleting cache file:{0}".info(file);
					Files.deleteFile(file);
				}
		}
    }
}
