// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Windows.Forms;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.WinForms;
using FluentSharp.WinForms.Controls;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.SharpDevelop.Dom;
//O2File:Ast_Engine_ExtensionMethods.cs 

namespace FluentSharp.CSharpAST.Utils
{
	//to be called from AppDomain (must be first class in this file)
	public class CodeStrams_Engine
	{
		public string create_Saved_MethodStream(string serialized_Saved_MethodStream_FileCache)
        {   
        	//if (ShowLogViewer)
	        //	showLogViewer();
	        "Creating Saved_MethodStream object from file: {0}".info(serialized_Saved_MethodStream_FileCache);
	        var savedMethodStream = serialized_Saved_MethodStream_FileCache.load<Saved_MethodStream>();
	        "Creating CodeStreams".info();
	        savedMethodStream.createCodeStreams();	       
	        "Saving Saved_MethodStream".info();
	        savedMethodStream.saveAs(serialized_Saved_MethodStream_FileCache);
	        
	        PublicDI.config.gcCollect();
	        savedMethodStream = null;
        	return "Created: {0} CodeStreams".format(savedMethodStream.CodeStreams.size());        	        	
        }
        
        public void showLogViewer()
        {
			var LogViewer = O2Gui.open<Panel>("MethodMappings Engine Logs", 400,200).add_LogViewer();
        }
	}
	
	[Serializable]
    public class Saved_MethodStream
    {       	
        	
    	public MethodStream_Item RootMethod { get; set; }
    	    	
    	public List<MethodStream_Item> MethodStreamItems { get; set; }
    	public List<CodeStreamPath> CodeStreams { get; set;}
    	
    	public string MethodStream_FileCache {get;set;}
    	public string Serialized_Saved_MethodStream_FileCache {get;set;}
    	public string MethodStream {get;set;}
    	
    	[XmlIgnore]
    	public O2MethodStream o2MethodStream;
    	
    	public Saved_MethodStream()
    	{
			  MethodStreamItems = new List<MethodStream_Item>();	
    	}
    	
    	public Saved_MethodStream(O2MappedAstData astData, IMethod iMethod, bool createCodeStreams) : this()
    	{
    		this.RootMethod = this.methodStreamItem(astData, iMethod);
    		    		
    		this.createMethodStream(astData, iMethod);
    		
    		this.map_MethodStreamItems(astData);
    		if (createCodeStreams)    		
    			this.createCodeStreams();
    		
    	}
    	
    	public Saved_MethodStream(O2MappedAstData astData, IMethod iMethod) : this(astData, iMethod, false)
    	{
    		
    	}
    	
    	public static Saved_MethodStream Create(O2MappedAstData astData, IMethod iMethod)
    	{
    		return Create(astData, iMethod, null, false);
    	}
    	
    	public static Saved_MethodStream Create(O2MappedAstData astData, IMethod iMethod, string methodStreams_CacheLocation, bool forceCreate)
    	{
    		if (methodStreams_CacheLocation.isNull())
    			methodStreams_CacheLocation = "_methodStreams_CacheLocation".tempDir();
    		var safeFileName = 	iMethod.fullName().safeFileName();//240 - methodStreams_CacheLocation.size());
    		var pathToSaveSerializedObject = methodStreams_CacheLocation.pathCombine_MaxSize(safeFileName + ".methodStream.xml");    											
			if (forceCreate.isFalse() && pathToSaveSerializedObject.fileExists())												
			{
				"Skipping iMethod '{0}' since serialized object already exists ".debug(iMethod.Name);
				var loaded_SavedMethodStream = pathToSaveSerializedObject.load<Saved_MethodStream>();
				if (loaded_SavedMethodStream.notNull())
					return loaded_SavedMethodStream;
			}
																			
			var savedMethodStream = new Saved_MethodStream(astData, iMethod);
			if (savedMethodStream.isNull())
				return null;
			else
			{	
				savedMethodStream.Serialized_Saved_MethodStream_FileCache = pathToSaveSerializedObject;
				savedMethodStream.saveAs(pathToSaveSerializedObject);					
				"Serialized method stream object saved to: {0}".debug(pathToSaveSerializedObject);													
				return savedMethodStream;
			}					
    	}
    }
    
    [Serializable]
    public class Ast_Location
    {
    	[XmlAttribute] public string File {get;set;}
    	[XmlAttribute] public int Line {get;set;}
    	[XmlAttribute] public int Column {get;set;}
    	[XmlAttribute] public int Line_End {get;set;}
    	[XmlAttribute] public int Column_End {get;set;}
    	
    	public Ast_Location()
    	{}
    	public Ast_Location(string file, int line, int column, int line_End, int column_End)
    	{
    		File = file;
    		Line = line;
    		Column = column;
    		Line_End = line_End;
    		Column_End = column_End;
    	}
    	
    	public Ast_Location(O2MappedAstData astData, IMethod iMethod)
    	{			
			File = astData.file(iMethod);
			
			var methodDeclaration = astData.methodDeclaration(iMethod);    					
			if (methodDeclaration.notNull())
			{
				Line = methodDeclaration.StartLocation.Line;
				Column = methodDeclaration.StartLocation.Column;			
				Line_End = methodDeclaration.EndLocation.Line;
				Column_End = methodDeclaration.EndLocation.Column;
			}
    	}
    	
    	public Ast_Location(INode iNode)
    	{
    		Line = iNode.StartLocation.Line;
			Column = iNode.StartLocation.Column;			
			Line_End = iNode.EndLocation.Line;
			Column_End = iNode.EndLocation.Column;
    	}
    }
    
    public class MethodStream_Item
    {
    	[XmlAttribute] public MethodStream_ItemType ItemType { get; set; }
    	[XmlAttribute] public string Name {get;set;}
    	[XmlAttribute] public string Class {get;set;}
    	[XmlAttribute] public string Namespace {get;set;}    	
    	[XmlAttribute] public string ReturnType {get;set;}    	
    	[XmlAttribute] public string Signature {get;set;}    			    	
    	[XmlAttribute] public string DotNetName {get;set;}
    	
    	[XmlElement("Parameter")]
    	public NameValueItems Parameters {get;set;}
    	[XmlElement("Attribute")]
    	public NameValueItems Attributes {get;set;}
    	public Ast_Location Location {get;set;}    	    
    	public MethodStream_Item()
    	{
    		//Parameters = new NameValueItems();
    		//Attributes = new NameValueItems();
    	}    	    	
    }
    
    public enum MethodStream_ItemType
    {
    	NotMapped,
    	@Namespace,
    	ExternalClasses,    	
    	ExternalMethod, 
    	MappedMethod,
    	Method,
    	Field,
    	Property
    	
    }
    
    public class CodeStreamPath
    {	
    	[XmlAttribute]
    	public string Text { get; set; }
    	[XmlAttribute] public int Line {get;set;}
    	[XmlAttribute] public int Column {get;set;}
    	[XmlAttribute] public int Line_End {get;set;}
    	[XmlAttribute] public int Column_End {get;set;}
    	[XmlAttribute] public int HashCode {get;set;}
    	
    	public Ast_Location Location { get; set; }
    	
    	[XmlElement("CodeStreamPath")]
    	public List<CodeStreamPath> CodeStreamPaths { get; set; }
    	public CodeStreamPath()
    	{
    		CodeStreamPaths = new List<CodeStreamPath>();
    	}
    	
    	public override string ToString()
    	{
    		return this.Text;
    	}
    }
    
    public static class Saved_MethodStream_ExtensionMethods_MethodStream
    {
    	public static Saved_MethodStream createMethodStream(this Saved_MethodStream savedMethodStream,  O2MappedAstData astData, IMethod iMethod)
		{
			savedMethodStream.o2MethodStream = astData.createO2MethodStream(iMethod);
			savedMethodStream.MethodStream = savedMethodStream.o2MethodStream.csharpCode();
			
			var fileCachePath = "_methodStreams".tempDir(false).pathCombine_MaxSize(savedMethodStream.RootMethod.Signature.safeFileName() + ".cs");
			savedMethodStream.MethodStream_FileCache = savedMethodStream.MethodStream.saveAs(fileCachePath);
			return savedMethodStream;			
		}
		
		public static Saved_MethodStream map_MethodStreamItems(this Saved_MethodStream savedMethodStream,  O2MappedAstData astData)
    	{
    		var methodStreamItems = savedMethodStream.MethodStreamItems;
    		foreach(var externalClass in savedMethodStream.o2MethodStream.ExternalClasses.Values)			
				methodStreamItems.add(savedMethodStream.methodStreamItem(externalClass)); 
				
			foreach(var externalIMethod in savedMethodStream.o2MethodStream.ExternalIMethods.Values)						
				methodStreamItems.add(savedMethodStream.methodStreamItem(astData,externalIMethod, MethodStream_ItemType.ExternalMethod));
			 
			foreach(var externalIMethod in savedMethodStream.o2MethodStream.MappedIMethods.Values)						
				methodStreamItems.add(savedMethodStream.methodStreamItem(astData,externalIMethod, MethodStream_ItemType.MappedMethod));

			foreach(var iField in savedMethodStream.o2MethodStream.Fields.Values) 			
				methodStreamItems.add(savedMethodStream.methodStreamItem(astData,iField)); 
				
			foreach(var iProperty in savedMethodStream.o2MethodStream.Properties.Values) 			
				methodStreamItems.add(savedMethodStream.methodStreamItem(astData,iProperty));	
				//break;
			return savedMethodStream;
    	}
    } 
    
    public static class Saved_MethodStream_ExtensionMethods_MethodStreamItem
    {
    	
		public static MethodStream_Item methodStreamItem(this Saved_MethodStream savedMethodStream,  O2MappedAstData astData, IMethod iMethod)
		{
			return savedMethodStream.methodStreamItem(astData,iMethod, MethodStream_ItemType.Method);
		}
		
		public static MethodStream_Item methodStreamItem(this Saved_MethodStream savedMethodStream,  O2MappedAstData astData, IMethod iMethod, MethodStream_ItemType itemType)
 		{ 						
 			var methodStreamItem = new MethodStream_Item();		
 			
			methodStreamItem.ItemType = itemType;
			
			if (iMethod.Parameters.Count > 0)
			{
				methodStreamItem.Parameters = new NameValueItems();
				foreach(var parameter in iMethod.Parameters) 
					methodStreamItem.Parameters.add(parameter.Name.str(), parameter.ReturnType.FullyQualifiedName);  
			}
			if (iMethod.Attributes.Count > 0)
			{
				methodStreamItem.Attributes = new NameValueItems();
				foreach(var attribute in iMethod.Attributes)
					methodStreamItem.Attributes.add(attribute.AttributeTarget.str(), attribute.AttributeType.FullyQualifiedName);  
			}
			
			methodStreamItem.Name = iMethod.name();
			methodStreamItem.Class = iMethod.DeclaringType.Name;  
			if (astData.file(iMethod).notNull())
				methodStreamItem.Location = new Ast_Location(astData, iMethod);
			methodStreamItem.Namespace = iMethod.DeclaringType.Namespace;  
			methodStreamItem.Signature = iMethod.fullName(); 
			methodStreamItem.ReturnType = iMethod.ReturnType.FullyQualifiedName;			
			methodStreamItem.DotNetName = iMethod.DotNetName;			
						
			return methodStreamItem;
		}
		
		public static MethodStream_Item methodStreamItem(this Saved_MethodStream savedMethodStream,  IReturnType iReturnType)
		{
			var methodStreamItem = new MethodStream_Item();				
			methodStreamItem.ItemType = MethodStream_ItemType.ExternalClasses;
			methodStreamItem.Name = iReturnType.Name;
			methodStreamItem.Namespace = iReturnType.Namespace;
			methodStreamItem.DotNetName = iReturnType.DotNetName;
			methodStreamItem.Class = iReturnType.DotNetName;
			methodStreamItem.Signature = iReturnType.FullyQualifiedName;			
			return methodStreamItem;
		}				
		
		public static MethodStream_Item methodStreamItem(this Saved_MethodStream savedMethodStream,  IEntity iEntity)
		{
			var methodStreamItem = new MethodStream_Item();							
			methodStreamItem.Name = iEntity.Name;
			methodStreamItem.Namespace = iEntity.Namespace;
			methodStreamItem.DotNetName = iEntity.DotNetName;
			methodStreamItem.Class = iEntity.DotNetName;
			methodStreamItem.Signature = iEntity.FullyQualifiedName;			
			return methodStreamItem;
		}
		
		public static MethodStream_Item methodStreamItem(this Saved_MethodStream savedMethodStream, O2MappedAstData astData,  IField iField)
		{
			var methodStreamItem = savedMethodStream.methodStreamItem(iField);
			methodStreamItem.ItemType = MethodStream_ItemType.Field;
			var fieldDeclaration = astData.fieldDeclaration(iField);
			if (fieldDeclaration.notNull())
				methodStreamItem.Location = new Ast_Location(fieldDeclaration);			
			return methodStreamItem;
		}
		
		public static MethodStream_Item methodStreamItem(this Saved_MethodStream savedMethodStream, O2MappedAstData astData,  IProperty iProperty)
		{                                           
			var methodStreamItem = savedMethodStream.methodStreamItem(iProperty);
			methodStreamItem.ItemType = MethodStream_ItemType.Property;
			var propertyDeclaration = astData.propertyDeclaration(iProperty);
			if (propertyDeclaration.notNull())
				methodStreamItem.Location = new Ast_Location(propertyDeclaration);
			//fieldDeclaration.details();
			return methodStreamItem;
		}
    }
    
    public static class Saved_MethodStream_ExtensionMethods_CodeStreams
    {
    	public static bool hasCodeStreamsMapped(this Saved_MethodStream savedMethodStream)
    	{
    		return savedMethodStream.CodeStreams.isNull().isFalse();
    	}
    		
    	public static Saved_MethodStream createCodeStreams(this Saved_MethodStream savedMethodStream)
		{
			var useAstCachedData  = true;  
			var methodStreamFile = savedMethodStream.MethodStream_FileCache;
			if (methodStreamFile.fileExists().isFalse())
			{
				"in createCodeStreams there was no MethodStream File to process".error();
				return savedMethodStream;
			}
			savedMethodStream.CodeStreams = new List<CodeStreamPath>();
			
			var AstData_MethodStream = methodStreamFile.getAstData(useAstCachedData);
			var methodDeclarations = AstData_MethodStream.methodDeclarations(); 
			if (methodDeclarations.size() > 0) 
			{
				var iNodes = methodDeclarations[0].iNodes();   
				
				foreach(var iNode in iNodes)			
					savedMethodStream.map_CodeStreams(AstData_MethodStream, methodStreamFile, iNode); 							
			}
			return savedMethodStream;
		}
		
		public static Saved_MethodStream map_CodeStreams(this Saved_MethodStream savedMethodStream, O2MappedAstData astData , String file, INode iNode)
		{									
		
			var o2CodeStream = astData.createO2CodeStream( file,iNode); 			
									
			//var uniqueStreamPaths = o2CodeStream.getUniqueStreamPaths(100);
			
			Func<O2CodeStreamNode, CodeStreamPath> map_O2CodeStreamNode = null; 
			map_O2CodeStreamNode = (o2CodeStreamNode) =>
				{
					var codeStreamPath = new CodeStreamPath(); 
					codeStreamPath.Text = o2CodeStreamNode.Text;
					codeStreamPath.Line = o2CodeStreamNode.INode.StartLocation.Line;
					codeStreamPath.Column = o2CodeStreamNode.INode.StartLocation.Column;
					codeStreamPath.Line_End = o2CodeStreamNode.INode.EndLocation.Line;
					codeStreamPath.Column_End = o2CodeStreamNode.INode.EndLocation.Column; 										
					foreach(var childNode in o2CodeStreamNode.ChildNodes)
						codeStreamPath.CodeStreamPaths.add(map_O2CodeStreamNode(childNode)); 
					return codeStreamPath;
				};
			foreach(var streamNode in o2CodeStream.StreamNode_First)
				savedMethodStream.CodeStreams.add(map_O2CodeStreamNode(streamNode));
			return savedMethodStream;	
		}
	}
	public static class Saved_MethodStream_ExtensionMethods_CodeStreams_AppDomain
	{
		public static string codeStreams_CreateInAppDomain(this Saved_MethodStream savedMethodStream)
	    {
	    	//var script = @"C:\O2\_XRules_Local\Ast_Test.cs";
	    	var script = "Saved_MethodStream.cs".local();
	    	"Creating new AppDomain".info();
			var appDomainName = 4.randomString();
			var o2AppDomain =  new O2AppDomainFactory(appDomainName);			
			o2AppDomain.appDomain().load("FluentSharp.CoreLib.dll"); 						
			var o2Proxy =  (O2Proxy)(o2AppDomain.appDomain().getProxyObject("O2Proxy"));
			var parameters = new object[]
					{ 
						savedMethodStream.Serialized_Saved_MethodStream_FileCache
//						sourceFolder,
//						resultsFolder,
//						methodFilter,
//						useCachedData,
//						references,
//						numberOfItemsToProcess                        
					};					
	
		    var result =(string)o2Proxy.staticInvocation("O2_External_SharpDevelop","FastCompiler_ExtensionMethods","executeFirstMethod",new object[]{script, parameters});	
		    "Result: {0}".info(result);
		    o2AppDomain.sleep(2000);
		    o2AppDomain.appDomain().unLoadAppDomain(); 
		    "AppDomain execution completed, Runing GCCollect".info();
		    PublicDI.config.gcCollect();
	    	"GCCollect completed, returning result data: {0}".info(result);
	    	return result;
	    }				
		}
	}
	