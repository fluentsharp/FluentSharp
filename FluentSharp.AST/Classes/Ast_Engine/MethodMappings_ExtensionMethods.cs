using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.WinForms.Utils;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.SharpDevelop.Dom;

//O2File:Ast_Engine_ExtensionMethods.cs
//O2Ref:QuickGraph.dll

namespace FluentSharp.CSharpAST.Utils
{	
	[Serializable]
	public class MethodMappings
	{	
		public string SourceCodeFolder {get;set;}
		public string ResultsFolder {get;set;}
		public string MethodFilter {get;set;}		
		public List<string> References {get;set;}
		public List<MethodMappingItem> MethodMappingItems {get; set;}
		public MethodMappings()
		{
			SourceCodeFolder = "";
			ResultsFolder = "";
			MethodFilter = "";
			References = new List<String>();
			MethodMappingItems = new List<MethodMappingItem>();
			
			//MappedMethods = new List<string>();
		}				
	}
	[Serializable]
	public class MethodMappingItem 
	{
		[XmlAttribute]
		public string Key {get; set;}
		
		
		public List<MethodMapping> MethodMappings {get; set;}
		public MethodMappingItem()
		{
			MethodMappings = new List<MethodMapping>();
		}				
	}
	
	[Serializable]
	public class MethodMapping
	{	
		[XmlAttribute]
		public string INodeType {get; set;}		
		[XmlAttribute]
		public string Signature {get; set;}
		[XmlAttribute]
		public string Key {get; set;}
		[XmlAttribute]
		public bool IsMethodDeclaration {get; set;}
		[XmlAttribute]
		public string File {get; set;}
		[XmlAttribute]
		public int Start_Line {get; set;}
		[XmlAttribute]
		public int Start_Column {get; set;}
		[XmlAttribute]
		public int End_Line {get; set;}
		[XmlAttribute]
		public int End_Column {get; set;}
		public string ParentMethod {get;set;}
		public string ParentClass {get;set;}
		public string SourceCode {get;set;}
		
		public MethodMapping()
		{}

		public MethodMapping(string signature, string key, string file,  string parentMethod, string parentClass, INode iNode) : this()		
		{				
			INodeType = iNode.typeName();
			Signature = signature ??  "";
			Key = key ?? "";
			IsMethodDeclaration = iNode is MethodDeclaration;
			File = file;
			Start_Line = iNode.notNull() && iNode.StartLocation.notNull() ? iNode.StartLocation.Line : -1;
			Start_Column = iNode.notNull() && iNode.StartLocation.notNull() ? iNode.StartLocation.Column : -1;
			End_Line = iNode.notNull() && iNode.EndLocation.notNull() ? iNode.EndLocation.Line : -1;
			End_Column = iNode.notNull() && iNode.EndLocation.notNull() ? iNode.EndLocation.Column : -1;		
			ParentMethod = parentMethod;
			ParentClass = parentClass;
			SourceCode = iNode.notNull() ? iNode.csharpCode().trim() : "";
		}
		
		public override string ToString()
		{
			return Key; //"{0} : {1}".format(INodeType, Signature);
		}
	}			
    
	public static class MethodMappings_ExtensionMethods_LoadAndMerge
	{
		public static MethodMappings loadMethodMappings (this string targetFile)
		{
			try
			{
				return targetFile.deserialize<MethodMappings>();
			}
			catch(Exception ex)
			{
				ex.log("in loadMethodMappings");
				return null;
			}	
		}

	public static MethodMappings loadAndMergeMethodMappings (this List<string> targetFiles)
	{
		"Creating MethodMappings from {0} files".info(targetFiles.size());
		var mergedMethodMappings = new Dictionary<string, List<MethodMapping>>();
		foreach(var targetFile in targetFiles)
		{
		
			var tempMethodMappings = loadMethodMappings(targetFile); 
			var indexedMappings = tempMethodMappings.indexedByKey(""); 
			foreach(var item in indexedMappings)
				foreach(var methodMapping in item.Value)									
					mergedMethodMappings.add(item.Key, methodMapping);																	
		}
		var methodMappings = new MethodMappings();							
		foreach(var item in mergedMethodMappings)
		{
			var methodMappingItem = new MethodMappingItem() { Key = item.Key};
			methodMappingItem.MethodMappings = item.Value;
			methodMappings.MethodMappingItems.Add(methodMappingItem); 
		}
		return methodMappings;
	}
				

	}
	
	public static class MethodMappings_ExtensionMethods_SaveMappings
	{	
		//move to O2MappedAstData
		public static IClass iClass(this O2MappedAstData astData, TypeDeclaration typeDeclaration)
		{
			if (astData.MapAstToNRefactory.TypeDeclarationToIClass.ContainsKey(typeDeclaration))
				return astData.MapAstToNRefactory.TypeDeclarationToIClass[typeDeclaration];
			return null;
		}
		public static string parentMethodSignature(this O2MappedAstData astData, INode iNode)
		{
			var methodDeclaration = iNode.parent<MethodDeclaration>();
			if (methodDeclaration.notNull())
			{
				var iMethod = astData.iMethod(methodDeclaration);
				if (iMethod.notNull())
					return iMethod.fullName();
			}
			return "";
		}
		
		public static string parentClassSignature(this O2MappedAstData astData, INode iNode)
		{
			var typeDeclaration = iNode.parent<TypeDeclaration>();
			if (typeDeclaration.notNull())
			{				
				var iClass = astData.iClass(typeDeclaration);
				if (iClass.notNull())
					return iClass.DotNetName;
			}
			return "";
		}
		
	
		public static MethodMapping methodMapping(this O2MappedAstData astData, IMethodOrProperty iMethodOrProperty,  INode iNode, string key)
		{			
			
			string parentMethod = astData.parentMethodSignature(iNode);
			string parentClass = astData.parentClassSignature(iNode);
			return astData.methodMapping(iMethodOrProperty.fullName(),key, parentMethod, parentClass, iNode);
		}
		public static MethodMapping methodMapping(this O2MappedAstData astData, string signature, string key, string parentMethod , string parentClass, INode iNode)
		{
			return new MethodMapping(signature,key, astData.file(iNode), parentMethod, parentClass, iNode);
		}
						
		public static MethodMappings createSavedMethodMappings(this O2MappedAstData astData, Dictionary<string,List<KeyValuePair<INode,IMethodOrProperty>>> mappings)
		{
			//"creating SavedMethodMappings object from {0} mappings".info(mappings.size());			
			var savedMethodMappings = new MethodMappings();
			foreach(var mapping in mappings)			
			{
				try
				{										
					string key = mapping.Key;					
					var methodMappingItem = new MethodMappingItem() { Key =  key};
					List<KeyValuePair<INode,IMethodOrProperty>> items = mapping.Value; 
					foreach(var keyValuePair in items)
					{							
						var iNode = keyValuePair.Key;				
						var iMethodOrProperty = keyValuePair.Value;
						methodMappingItem.MethodMappings.Add(astData.methodMapping(iMethodOrProperty,iNode,key));
					}						
					savedMethodMappings.MethodMappingItems.Add(methodMappingItem);				
				}
				catch(Exception ex)
				{
					ex.log("in createSavedMethodMappings");
				}
			}			
			return savedMethodMappings;			
		}			
		
		public static string saveMappings(this O2MappedAstData astData, Dictionary<string,List<KeyValuePair<INode,IMethodOrProperty>>> mappings)
		{
			return astData.createSavedMethodMappings(mappings).saveMappings();
		}
		
		public static string saveMappings(this MethodMappings savedMethodMappings, string targetFile)
		{
			var savedFile = savedMethodMappings.saveMappings();
			if (savedFile.fileExists())
			{								
				Files.moveFile(savedFile,targetFile);
				if (targetFile.fileExists())
					return targetFile;
			}
			return "";
			
		}
		public static string saveMappings(this MethodMappings savedMethodMappings)
		{	
			if (savedMethodMappings.isNull())
				"in saveMappings savedMethodMappings was Null".error();
			//"__*****>>> in saveMappings: {0}".info(savedMethodMappings.MethodMappingItems.size());
			//show.info(savedMethodMappings);
			var savedMappingsFile = savedMethodMappings.save();
			//"*****>>> Saved Mappings File: {0}".info(savedMappingsFile);
			return savedMappingsFile;
		}
	}
	
	public static class O2MappedAstData_ExtensionMethods_ExternalMethodsAndProperties
    {    
        public static Dictionary<IMethod, Dictionary<string,List<KeyValuePair<INode,IMethodOrProperty>>>> externalMethodsAndProperties(this O2MappedAstData astData)
        {
        	var tempFolder = PublicDI.config.getTempFolderInTempDirectory("_AstEngine_ExternalMappings");        	
        	return astData.externalMethodsAndProperties("");
        }

		public static Dictionary<IMethod, Dictionary<string,List<KeyValuePair<INode,IMethodOrProperty>>>> externalMethodsAndProperties(this O2MappedAstData astData,string filter)
        {        	
        	return astData.externalMethodsAndProperties("", "", -1);
        }      
        
        public static Dictionary<IMethod, Dictionary<string,List<KeyValuePair<INode,IMethodOrProperty>>>> externalMethodsAndProperties(this O2MappedAstData astData, string filter, string targetFolder, int numberOfItemsToProcess)
        {
        	var MimimumAvailableMemoryRequired = "".availableMemory()/4; //50;
        	"Starting externalMethodsAndProperties calculations (with min memory required set to: {0}".info(MimimumAvailableMemoryRequired);
        	if (targetFolder.valid().isFalse())
        		targetFolder = PublicDI.config.getTempFolderInTempDirectory("_AstEngine_ExternalMappings"); 
        	
        	var methodMappingHashesFile = targetFolder.pathCombine("_methodMappingHashes.txt");
        	        
        	var o2Timer = new O2Timer("Calculated externalMethodsAndProperties").start();
        	var iMethodMappings = new Dictionary<IMethod, Dictionary<string,List<KeyValuePair<INode,IMethodOrProperty>>>>(); 					
        	var iMethods = astData.iMethods();
        	var itemsToMap = iMethods.size();// - targetFolder.files().size();
			var itemsMapped = 0;
								
			foreach(var iMethod in iMethods)
			{
				// check avaialble memory
				var availableMemory = new System.Diagnostics.PerformanceCounter("Memory", "Available MBytes").NextValue();
				if (availableMemory < MimimumAvailableMemoryRequired)
				{
					"In externalMethodsAndProperties, There is not enough free memory to continue (MimimumAvailableMemoryRequired = {0}, availableMemory = {1}. Stopping mappings".error(MimimumAvailableMemoryRequired,availableMemory);
					"There are {0} iMethodMappings".debug(iMethodMappings.size());
					break;
				}
				//"Available Memory: {0}".debug(availableMemory);
				
				//convert method
				var fullName = iMethod.fullName();
                var md5Hash = fullName.safeFileName().md5Hash();
                var savedMethodMappingsFile = targetFolder.pathCombine(md5Hash) + ".xml";
				//var savedMethodMappingsFile = targetFolder.pathCombine(iMethod.fullName().safeFileName(100))+ ".xml";
				itemsMapped++;
				if (savedMethodMappingsFile.fileExists().isFalse()) // Skip if method mappings have already been calculated
				{
					//"Mapping :{0}".debug(iMethod.DotNetName);
					if (iMethod.Name.regEx(filter))	
					{
						var mappings = astData.externalMethodsAndProperties(iMethod);
						iMethodMappings.Add(iMethod, mappings);
						var savedMethodMappings = astData.saveMappings(mappings);
						if (savedMethodMappings.fileExists())
						{	
							Files.moveFile(savedMethodMappings, savedMethodMappingsFile);
							methodMappingHashesFile.fileInsertAt(0,"{0}\t{1}".format(md5Hash, fullName).line());
						}
						
					}
						//savedMethodMappingsFile	
					if (itemsMapped % 10 ==0)		// every 10 methods show a log message		
					{
						"In externalMethodsAndProperties, mapped [{0}/{1}] to folder: {2}".info(itemsMapped, itemsToMap, targetFolder);
						if (itemsMapped % 100 ==0)	
							PublicDI.config.gcCollect();	// every 100 methods release some memory				
					}
					if (numberOfItemsToProcess > 0 && numberOfItemsToProcess < iMethodMappings.size())
					{
						"In externalMethodsAndProperties, max number of Items To Process reached ({0}), so stopping mappings]".info(numberOfItemsToProcess);
						"There are {0} iMethodMappings".debug(iMethodMappings.size());
						break;
					}												
				}
			}
			o2Timer.stop();
			return iMethodMappings;
		}
                
        public static Dictionary<string,List<KeyValuePair<INode,IMethodOrProperty>>> externalMethodsAndProperties(this O2MappedAstData astData, IMethod iMethod)
        {        	
        	var externalMethods = new Dictionary<string,List<KeyValuePair<INode,IMethodOrProperty>>>();
        	// add the current method
        	
        	externalMethods.add(iMethod.DotNetName,  new KeyValuePair<INode,IMethodOrProperty>(astData.methodDeclaration(iMethod) ,iMethod));
        	
        	var iNodesAdded = new List<INode>();
        	
        	foreach (var methodCalled in astData.calledINodesReferences(iMethod))
			 	if (methodCalled is MemberReferenceExpression)
                {                             	
                	var memberRef = (MemberReferenceExpression)methodCalled;                 	
                	{                	
                		var methodOrProperty = astData.fromMemberReferenceExpressionGetIMethodOrProperty(memberRef);
                		if(methodOrProperty.notNull())                     	
                		{
                			externalMethods.add(methodOrProperty.DotNetName, new KeyValuePair<INode,IMethodOrProperty>(memberRef,methodOrProperty)); 
                			iNodesAdded.Add(memberRef);
                		}
                		else
                			externalMethods.add(astData.getTextForINode(memberRef),new KeyValuePair<INode,IMethodOrProperty>(memberRef,null));
                	}
				}
			
			
        	foreach(var mapping in astData.calledIMethods_getMappings(iMethod))
        	{                
                var iMethodMapping = mapping.Key;
                var iNodeMapping = mapping.Value;
                if (iNodesAdded.Contains(iNodeMapping).isFalse())
                	if (iNodeMapping is ObjectCreateExpression ||
                	   ((iNodeMapping is InvocationExpression &&
                	    (iNodeMapping as InvocationExpression).TargetObject.notNull() && 
                	    iNodesAdded.Contains((iNodeMapping as InvocationExpression).TargetObject).isFalse())))
                	 {                	 	
						var nodeText = (iMethodMapping.notNull())
												? iMethodMapping.DotNetName
												: astData.getTextForINode(iNodeMapping);
						externalMethods.add(nodeText, new KeyValuePair<INode,IMethodOrProperty>(iNodeMapping,iMethodMapping)); 				
					 }	
            }
        	
        	return externalMethods;
        }
        
        public static Dictionary<string,List<KeyValuePair<INode,IMethodOrProperty>>> indexedBy_ResolvedSignature( this Dictionary<IMethod, Dictionary<string,List<KeyValuePair<INode,IMethodOrProperty>>>> iMethodMappings)
        {
        	var o2Timer = new O2Timer("Calculated indexedBy_ResolvedSignature").start();
        	var results = new Dictionary<string,List<KeyValuePair<INode,IMethodOrProperty>>>();
        	foreach(var iMethodMapping in iMethodMappings) 
        		foreach(var mapping in iMethodMapping.Value)
        			foreach(var keyValuePair in mapping.Value)	        			        			
	        			results.add(mapping.Key, keyValuePair);	        		
			o2Timer.stop();
        	return results;        	
        }
        
        
        public static List<IMethod> calledIMethods(this O2MappedAstData o2MappedAstData, IMethod iMethod)
        {
        	return (from mapping in o2MappedAstData.calledIMethods_getMappings(iMethod)
        			select mapping.Key).toList();        	
        }
        
        public static KeyValuePair<IMethod, INode> iMethodMapping(this IMethod iMethod, INode iNode)
        {
        	return new KeyValuePair<IMethod, INode>(iMethod, iNode);
        }
        
        public static List<KeyValuePair<IMethod, INode>> calledIMethods_getMappings(this O2MappedAstData o2MappedAstData, IMethod iMethod)
        {
            //"in Called IMETHODS".debug();\
            var calledIMethodsMapping = new List<KeyValuePair<IMethod, INode>>();
            
            if (iMethod != null)
            {
              //  "-------------------".info();
                var methodDeclaration = o2MappedAstData.methodDeclaration(iMethod);
                if (methodDeclaration == null)
                    return calledIMethodsMapping;

                // handle invocations via MemberReferenceExpression
                var memberReferenceExpressions = methodDeclaration.iNodes<INode, MemberReferenceExpression>();
                foreach (var memberReferenceExpression in memberReferenceExpressions)                
                {
                	var iMethodFromRef = o2MappedAstData.iMethod(memberReferenceExpression);
                	if (iMethodFromRef.notNull())
                		calledIMethodsMapping.Add(iMethodFromRef.iMethodMapping(memberReferenceExpression));
                	else
                	{
                		var iMethodOrProperty = o2MappedAstData.fromMemberReferenceExpressionGetIMethodOrProperty(memberReferenceExpression);	
                		if (iMethodOrProperty is IMethod)
                			calledIMethodsMapping.Add((iMethodOrProperty as IMethod).iMethodMapping(memberReferenceExpression));
                		//else
                		//	"NOT Resolved for: {0}".error(memberReferenceExpression.MemberName);
                	}
                		
                }
//                    calledIMethods.add();

                // handle invocations via InvocationExpression
                var invocationExpressions = methodDeclaration.iNodes<INode, InvocationExpression>();
                foreach (var invocationExpression in invocationExpressions)
                    calledIMethodsMapping.add(o2MappedAstData.iMethod(invocationExpression).iMethodMapping(invocationExpression));

                // handle contructors
                var objectCreateExpressions = methodDeclaration.iNodes<INode, ObjectCreateExpression>();
                //"objectCreateExpressions: {0}".format(objectCreateExpressions.Count).info();
                foreach (var objectCreateExpression in objectCreateExpressions)
                    calledIMethodsMapping.add(o2MappedAstData.iMethod(objectCreateExpression).iMethodMapping(objectCreateExpression));

            }
            //return calledIMethods;
            return calledIMethodsMapping;
        }
        
        public static string add_Properties(this O2MappedAstData astData, string targetFolder)
        {
        	//return methodMappingsIndexedBySignature;
        	//handle Properties
			var propertyMappings = new Dictionary<string,List<KeyValuePair<INode,IMethodOrProperty>>>();
//			show.info(methodMappingsIndexedBySignature);

            var fileName = "_MethodMapings_For_Properties";
            //var md5Hash = fullName.safeFileName().md5Hash();
            var savedPropertiesMappingsFile = targetFolder.pathCombine(fileName) + ".xml";
            if (savedPropertiesMappingsFile.fileExists())
                return savedPropertiesMappingsFile;

			var propertyRefs = new List<INode>();
			foreach(var propertyDeclaration in astData.MapAstToNRefactory.IPropertyToPropertyDeclaration.Values)
			{
				"Mapping property: {0}".info(propertyDeclaration.Name);
				
				propertyRefs.add(propertyDeclaration.iNodes<INode, MemberReferenceExpression>());
                propertyRefs.add(propertyDeclaration.iNodes<INode, InvocationExpression>());
                propertyRefs.add(propertyDeclaration.iNodes<INode, ObjectCreateExpression>());
                
			}
//			show.info(propertyRefs);
			try
			{
				foreach(Expression propertyRef in propertyRefs)
				{
					var methodOrProperty = astData.fromExpressionGetIMethodOrProperty(propertyRef);				
					var nodeText =	astData.getTextForINode(propertyRef);// propertyRef.str();
					propertyMappings.add(nodeText, new KeyValuePair<INode,IMethodOrProperty>(propertyRef,methodOrProperty)); 
				}			
			}
			catch(Exception ex)
			{
				ex.log("in add_Property");
			}
            var tempFile = astData.saveMappings(propertyMappings);
            if (tempFile.fileExists())
				Files.moveFile(tempFile, savedPropertiesMappingsFile);            
            return savedPropertiesMappingsFile;
			/// PROPERTIES
        }
	}				
	
	public static class O2MappedAstData_ExtensionMethods_MethodMappings
	{
		public static int getINodePosition(this O2MappedAstData astData, MethodMapping methodMapping)
		{
			var position = -1;
			var methodSignature = methodMapping.ParentMethod;
			var iMethod = astData.iMethod_withSignature(methodSignature);
			var methodDeclaration = astData.methodDeclaration(iMethod);
			var iNodes = methodDeclaration.iNodes();
			if (iNodes.isNull())
				return position;
			var iNodeAtLocation = astData.iNode(methodMapping.File,  methodMapping.Start_Line, methodMapping.Start_Column );			
			for(int i=0; i < iNodes.size(); i++)
				if (iNodes[i] == iNodeAtLocation)
				{
					position = i;
					break; 															
				}
			if (position.eq(-1))
				return position;
			//Hack to try to find the exact INode we had before
			if (position.typeName() != methodMapping.INodeType)
			{
				if (position > 0 && iNodes[position-1].typeName() == methodMapping.INodeType)
					position--;
				else
					if (position < (iNodes.size()-1) &&iNodes[position+1].typeName() == methodMapping.INodeType)
						position++;
			}
			return position;
		}
	}
	
	// this part is almost redundant
	public static class O2MappedAstData_ExtensionMethods_MethodsCalledAndIsCalledBy
	{	        
        public static List<IMethod> iMethodsThatCallThisIMethod(this O2MappedAstData astData, IMethod targetIMethod)
        {
            var results = new List<IMethod>();
            if (astData != null && targetIMethod != null)
            {
                var targetIMethodName = targetIMethod.fullName();
                foreach (var iMethod in astData.iMethods())
                    if (iMethod != null && iMethod.DotNetName.valid())
                    {
                        try
                        {
                            foreach (var iMethodCalled in astData.calledIMethods(iMethod))
                            {
                                if (iMethodCalled != null && iMethodCalled.fullName() == targetIMethodName)
                                    if (results.Contains(iMethod).isFalse())
                                        results.add(iMethod);
                                //"{0} -> {1}".debug(iMethod.DotNetName,  iMethodCalled.DotNetName);
                                //results.add(iMethod);
                            }
                        }
                        catch (Exception ex)
                        {
                            ex.log("iMethodsThatCallThisIMethod");
                        }
                    }
            }
            return results;
        }		
		
        public static Dictionary<string, List<string>> calculateMappingsFor_MethodsCalled(this O2MappedAstData astData)
        {
            "stating: calculateMappingsFor_MethodsCalled".info();
            var mappings = new Dictionary<string, List<string>>();
            foreach (var iMethod in astData.iMethods())
                if (iMethod != null && iMethod.DotNetName.valid())
                {
                    var calledMethodsList = mappings.add_Key(iMethod.fullName());
                    try
                    {
                        foreach (var iMethodCalled in astData.calledIMethods(iMethod))
                            if (iMethodCalled != null)
                            {
                                var methodCalledName = iMethodCalled.fullName();
                                if (calledMethodsList.Contains(methodCalledName).isFalse())
                                    calledMethodsList.add(methodCalledName);
                            }
                    }
                    catch (Exception ex)
                    {
                        ex.log("iMethodsThatCallThisIMethod");
                    }
                    //if (mappings.Keys.size() == 25)
                    //	return mappings;
                }
            "completed: calculateMappingsFor_MethodsCalled".info();
            return mappings;
        }

        public static Dictionary<string, List<string>> calculateMappingsFor_MethodIsCalledBy(this O2MappedAstData astData, Dictionary<string, List<string>> methodsCalledMappings)
        {
            "stating: calculateMappingsFor_MethodIsCalledBy".info();
            var mappings = new Dictionary<string, List<string>>();
            foreach (var item in methodsCalledMappings)
            {
                var methodName = item.Key;
                var methodsCalled = item.Value;
                foreach (var methodCalled in methodsCalled)
                {
                    var isCalledBy = mappings.add_Key(methodCalled);
                    if (isCalledBy.Contains(methodName).isFalse())
                        isCalledBy.Add(methodName);
                }
            }
            "completed: calculateMappingsFor_MethodIsCalledBy".debug(); ;
            return mappings;
        }
	}

	public static class MethodMappings_ExtensionMethods_Indexing
	{
		
		public static Dictionary<string, List<MethodMapping>> indexedByKey(this MethodMappings methodMappings, string filter)
		{
			var result = new Dictionary<string, List<MethodMapping>>(); 
			if (methodMappings.MethodMappingItems.isNull())
				"In IndexedByKey there were no methodMappingsItems (filer = '{0}'".error(filter ?? "");
			else
				foreach(var mappingItem in methodMappings.MethodMappingItems)
				{
					var key = mappingItem.Key;   
					if (filter.valid().isFalse() || key.regEx(filter)) 
						foreach(var methodMapping in mappingItem.MethodMappings)
						{
							methodMapping.Key = key;					//Hack(26-Aug-2010): fix until we create all methodMappings with this Key valu	e
							result.add(key, methodMapping);
						}
				}
			return result;
		}
	}
	public static class MethodMappings_ExtensionMethods_MethodMapping
	{
		public static string sourceCodeLine(this MethodMapping methodMapping)
		{
			if (methodMapping.File.fileExists())
			{
				 var sourceCodeLine = Files_WinForms.getLineFromSourceCode(methodMapping.File, methodMapping.Start_Line.uInt());
				 if (methodMapping.SourceCode.contains(sourceCodeLine) || sourceCodeLine.contains(methodMapping.SourceCode))
					 return sourceCodeLine.trim();;				 
			}
			return methodMapping.SourceCode;
		}
	}
}