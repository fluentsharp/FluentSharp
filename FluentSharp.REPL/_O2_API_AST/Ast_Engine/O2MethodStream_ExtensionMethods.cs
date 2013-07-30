using System;
using System.Collections.Generic;
using System.Linq;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.Utils;
using ICSharpCode.SharpDevelop.Dom;
using ICSharpCode.NRefactory.Ast;

//O2File:Ast_Engine_ExtensionMethods.cs

namespace FluentSharp.CSharpAST.Utils
{
    public static class O2MethodStream_ExtensionMethods
    {
    	public static Dictionary<IMethod, O2MethodStream> O2MethodStreamCache = new Dictionary<IMethod, O2MethodStream>();
    
		public static O2MappedAstData clearO2MethodStreamCache(this O2MappedAstData o2MappedAstData)
		{
			O2MethodStreamCache.Clear();
			return o2MappedAstData;
		}
    
        public static List<string> createO2MethodStreamFiles(this O2MappedAstData o2MappedAstData, List<string> sourceFiles, string targetFolder)
        {
            var createdFiles = new List<String>();
            foreach (var file in sourceFiles)
                createdFiles.AddRange(o2MappedAstData.createO2MethodStreamFiles(file, targetFolder));
            return createdFiles;
        }

        public static List<string> createO2MethodStreamFiles(this O2MappedAstData o2MappedAstData, string sourceFile, string targetFolder)
        {
            var iMethods = o2MappedAstData.iMethods(sourceFile);
            return o2MappedAstData.createO2MethodStreamFiles(iMethods, targetFolder);
        }

        public static List<string> createO2MethodStreamFiles(this O2MappedAstData o2MappedAstData, List<IMethod> iMethods, string targetFolder)
        {
            var files = new List<string>();
            foreach (var iMethod in iMethods)
                files.add(o2MappedAstData.createO2MethodStreamFile(iMethod, targetFolder));
            return files;
        }

        public static string createO2MethodStreamFile(this O2MappedAstData o2MappedAstData, IMethod iMethod, string targetFolder)
        {
            var targetFile = targetFolder.pathCombine(iMethod.DotNetName + ".cs");
            if (targetFile.fileExists())
                "in O2MappedAstData.createO2MethodStreamFile, target method stream already exists: {0}".debug(iMethod.DotNetName);
            else
                o2MappedAstData.createO2MethodStream(iMethod).csharpFile(targetFile);
            return targetFile;
        }

		public static O2MethodStream createO2MethodStream(this O2MappedAstData o2MappedAstData, IMethod iMethod)
		{
			return o2MappedAstData.createO2MethodStream(iMethod,true);
		}
        public static O2MethodStream createO2MethodStream(this O2MappedAstData o2MappedAstData, IMethod iMethod, bool mapInterfacesCalls)
        {
            var o2MethodStream = new O2MethodStream(o2MappedAstData);
            o2MethodStream.add_IMethod(iMethod);
            if (mapInterfacesCalls)
	            o2MethodStream.resolveInterfaceCalls();  
            //add to O2MethodStreamCache
            O2MethodStreamCache.add(iMethod, o2MethodStream);
            
            return o2MethodStream;
        }
        
        public static List<string> createO2MethodStreams_UseCache_ReturnFile(this O2MappedAstData o2MappedAstData, List<IMethod> iMethods, FileCache fileCache, string cacheExtension)
        {
        	var methodStreams = new List<string>();
        	foreach(var iMethod in iMethods)
        		methodStreams.Add(o2MappedAstData.createO2MethodStream_UseCache_ReturnFile(iMethod, fileCache, cacheExtension));
        	return methodStreams;
        }
        
        public static string methodStream_UseCache(this O2MappedAstData o2MappedAstData, IMethod iMethod)
        {
        	return o2MappedAstData.createO2MethodStream_UseCache_ReturnFile(iMethod);
        }
        
        public static string methodStream_UseCache(this O2MappedAstData o2MappedAstData, IMethod iMethod, string fileCacheLocation)
        {
        	return o2MappedAstData.createO2MethodStream_UseCache_ReturnFile(iMethod, fileCacheLocation);
        }
        
        public static string createO2MethodStream_UseCache_ReturnFile(this O2MappedAstData o2MappedAstData, IMethod iMethod)
        {
        	var fileCacheLocation = "".tempDir().pathCombine("_fileCache_MethodStreams");
        	
        	return o2MappedAstData.createO2MethodStream_UseCache_ReturnFile(iMethod, fileCacheLocation);
        }
        
        public static string createO2MethodStream_UseCache_ReturnFile(this O2MappedAstData o2MappedAstData, IMethod iMethod, string fileCacheLocation)
        {
        	var cacheExtension = ".cs";
        	var fileCache = new FileCache(fileCacheLocation);        	
        	return o2MappedAstData.createO2MethodStream_UseCache_ReturnFile(iMethod, fileCache, cacheExtension);
        }
        
		public static string createO2MethodStream_UseCache_ReturnFile(this O2MappedAstData o2MappedAstData, IMethod iMethod, FileCache fileCache, string cacheExtension)
		{
			if (iMethod.isNull())
				return null;
			var csharpCodeFile = fileCache.cacheGet_File(iMethod.fullName(),cacheExtension);
			if (csharpCodeFile.valid().isFalse())																		
			{			 
				"Creating MethodStream for iMethod: {0}".debug(iMethod.DotNetName); 
				var methodStream = o2MappedAstData.createO2MethodStream(iMethod);						
				var cSharpCode = methodStream.csharpCode();
				csharpCodeFile = cSharpCode.saveWithExtension(cacheExtension);				
				return fileCache.cachePut(iMethod.fullName(),".cs",cSharpCode);
			}
			return csharpCodeFile;	
		}	

        /*
        public static List<string> createO2MethodStreamFiles(this O2MappedAstData o2MappedAstData, List<string> sourceFiles, string targetFolder)
        {
            var createdFiles = new List<String>();
            foreach (var file in sourceFiles)
                createdFiles.AddRange(o2MappedAstData.createO2MethodStreamFiles(file, targetFolder));
            return createdFiles;
        }

        public static List<string> createO2MethodStreamFiles(this O2MappedAstData o2MappedAstData, string sourceFile, string targetFolder)
        {
            var iMethods = o2MappedAstData.iMethods(sourceFile);
            return o2MappedAstData.createO2MethodStreamFiles(iMethods, targetFolder);
        }

        public static List<string> createO2MethodStreamFiles(this O2MappedAstData o2MappedAstData, List<IMethod> iMethods, string targetFolder)
        {
            var files = new List<string>();
            foreach (var iMethod in iMethods)
                files.add(o2MappedAstData.createO2MethodStreamFile(iMethod, targetFolder));
            return files;
        }

        public static string createO2MethodStreamFile(this O2MappedAstData o2MappedAstData, IMethod iMethod, string targetFolder)
        {

            var targetFile = targetFolder.pathCombine(iMethod.DotNetName + ".cs");
            o2MappedAstData.createO2MethodStream(iMethod).csharpFile(targetFile);
            return targetFile;
        }

        public static O2MethodStream createO2MethodStream(this O2MappedAstData o2MappedAstData, IMethod iMethod)
        {
            var o2MethodStream = new O2MethodStream(o2MappedAstData);
            o2MethodStream.add_IMethod(iMethod);
            return o2MethodStream;
        }


        */
        public static bool has_ExternalIMethod(this O2MethodStream o2MethodStream, IMethod iMethod)
        {
            if (iMethod != null)
                return o2MethodStream.has_ExternalIMethod(iMethod.fullName());
            return false;
        }

        public static bool has_ExternalIMethod(this O2MethodStream o2MethodStream, string fullName)
        {
            if (fullName.valid())
                return o2MethodStream.ExternalIMethods.hasKey(fullName);
            return false;
        }

        public static O2MethodStream add_ExternalIMethod(this O2MethodStream o2MethodStream, IMethod iMethod)
        {
            if (iMethod != null)
            {
                string fullName = iMethod.fullName();
                if (o2MethodStream.has_ExternalIMethod(iMethod).isFalse())
                {
                    o2MethodStream.ExternalIMethods.add(fullName, iMethod);
                }
            }
            return o2MethodStream;
        }

        public static bool has_MappedIMethod(this O2MethodStream o2MethodStream, IMethod iMethod)
        {
            if (iMethod != null)
                return o2MethodStream.has_MappedIMethod(iMethod.fullName());
            return false;
        }

        public static bool has_MappedIMethod(this O2MethodStream o2MethodStream, string fullName)
        {
            if (fullName.valid())
                return o2MethodStream.MappedIMethods.hasKey(fullName);
            return false;
        }

        #region add/map IMethod

        public static O2MethodStream add_IMethods(this O2MethodStream o2MethodStream, List<IMethod> iMethods)
        {
            foreach (var iMethod in iMethods)
                o2MethodStream.add_IMethod(iMethod);
            return o2MethodStream;
        }

        public static O2MethodStream add_IMethod(this O2MethodStream o2MethodStream, IMethod iMethod)
        {
            return o2MethodStream.add_MappedIMethod(iMethod);
        }

        public static O2MethodStream add_MappedIMethod(this O2MethodStream o2MethodStream, IMethod iMethod)
        {            
            if (iMethod != null)
            {
                var fullName = iMethod.fullName();                
                // add IMethod
                if (o2MethodStream.has_MappedIMethod(fullName).isFalse())
                {
                    "adding MappedIMethod:{0}".format(fullName).info();
                    o2MethodStream.MappedIMethods.add(fullName, iMethod);

                    // remove from ExternalIMethod list (if there)								
                    if (o2MethodStream.has_ExternalIMethod(fullName))
                        o2MethodStream.ExternalIMethods.Remove(fullName);
                    // map its dependencies
                    o2MethodStream.map_IMethod_Dependencies(iMethod);
                }
            }
            return o2MethodStream;
        }		
		
        public static O2MethodStream map_IMethod_Dependencies(this O2MethodStream o2MethodStream, IMethod iMethod)
        {
        	if (O2MethodStream_ExtensionMethods.O2MethodStreamCache.hasKey(iMethod))
        	{
        		"Found IMethod in O2MethodStreamCache(merging its data with the current o2MethodStream): {0}".debug(iMethod.DotNetName);
        		o2MethodStream.add_O2MethodStreamMappings(O2MethodStream_ExtensionMethods.O2MethodStreamCache[iMethod]);
        		return o2MethodStream;
        	}        	
            // map the methods called
            INode methodOrCtorINode = o2MethodStream.O2MappedAstData.methodDeclaration(iMethod);
            if (methodOrCtorINode.isNull())
               methodOrCtorINode = o2MethodStream.O2MappedAstData.constructorDeclaration(iMethod);

			if (methodOrCtorINode.notNull())
			{
				o2MethodStream.map_MemberReferenceExpressions(methodOrCtorINode)
                              .map_ObjectCreateExpressions(methodOrCtorINode)
                              .map_InvocationExpressions(methodOrCtorINode)
                              .map_Expressions(methodOrCtorINode.iNodes<INode, IdentifierExpression>());

                // this is not working (the idea is to map the chained ctors
                /*
                 
                if (methodOrCtorINode is ConstructorDeclaration)
                {
                    var constructorDeclaration = (ConstructorDeclaration)methodOrCtorINode;
                    if (constructorDeclaration.ConstructorInitializer.notNull())
                    {
                        //var iMethodOfConstructorInitializer = o2MethodStream.fromExpressionGetIMethod(constructorDeclaration.ConstructorInitializer);
                        //if (iMethodOfConstructorInitializer.notNull())
                        //    o2MethodStream.add_IMethod(iMethodOfConstructorInitializer);
                    }
                }
                 */ 

			}                             
/*            var methodDeclaration = o2MethodStream.O2MappedAstData.methodDeclaration(iMethod);
            if (methodDeclaration != null)
            {            
				o2MethodStream
                              .map_MemberReferenceExpressions(methodDeclaration)
                              .map_ObjectCreateExpressions(methodDeclaration)
                              .map_InvocationExpressions(methodDeclaration)
                              .map_Expressions(methodDeclaration.iNodes<INode, IdentifierExpression>());                                               
            }*/
            // map the external classes
            o2MethodStream.map_ExternalClasses(iMethod);
            return o2MethodStream;
        }
        
		public static O2MethodStream add_O2MethodStreamMappings(this O2MethodStream targetO2MethodStream, O2MethodStream sourceO2MethodStream)
		{
			//MappedIMethods
			foreach(var iMethod in sourceO2MethodStream.MappedIMethods)
				targetO2MethodStream.MappedIMethods.add(iMethod.Key,iMethod.Value);
			//ExternalIMethods
			foreach(var iMethod in sourceO2MethodStream.ExternalIMethods)
				targetO2MethodStream.ExternalIMethods.add(iMethod.Key,iMethod.Value);
			//ExternalClasses	
			foreach(var iReturnType in sourceO2MethodStream.ExternalClasses)
				targetO2MethodStream.ExternalClasses.add(iReturnType.Key,iReturnType.Value);
			//Fields	
			foreach(var field in sourceO2MethodStream.Fields)
				targetO2MethodStream.Fields.add(field.Key,field.Value);
			//Properties	
			foreach(var property in sourceO2MethodStream.Properties)
				targetO2MethodStream.Properties.add(property.Key,property.Value);			
			return targetO2MethodStream;
		}


        public static O2MethodStream map_Expression<T>(this O2MethodStream o2MethodStream, T expression)
            where T : Expression
        {
            return o2MethodStream.map_Expressions(expression.wrapOnList());
        }

        public static O2MethodStream map_Expressions<T>(this O2MethodStream o2MethodStream, List<T> expressions)
            where T : Expression
        {             
            foreach (var identifierExpression in expressions)
            {
                var resolved = o2MethodStream.O2MappedAstData.resolveExpression(identifierExpression);
                switch (resolved.typeName())
                { 
                	
                    case "MemberResolveResult":
                        var resolvedMember = (resolved as MemberResolveResult).ResolvedMember;
                        if (resolvedMember is IField)
                        {
                            var iField = (resolvedMember as IField);
                            o2MethodStream.Fields.add(iField.FullyQualifiedName, iField);
                        }
                        else
                        if (resolvedMember is IProperty)
                        {
                            var iProperty = (resolvedMember as IProperty);
                            o2MethodStream.Properties.add(iProperty.FullyQualifiedName, iProperty);
                        }
                        break;

                    case "TypeResolveResult":
                        //var typeResolveResult = (resolved as TypeResolveResult);
                        //var resolvedClass = typeResolveResult.ResolvedType;
                        //o2MethodStream.compilationUnit().add_Type(typeResolveResult.ResolvedType);
                        break;

                    case "LocalResolveResult":
                        var localResolveResutlt = (resolved as LocalResolveResult);
                        break;

                    case "NamespaceResolveResult":
                        var namespaceResolveResult = (resolved as NamespaceResolveResult);
                        
                        break;

                    default:                        
                        if (o2MethodStream.O2MappedAstData.debugMode)
                        {
                            var unsupportedType = resolved.typeName();                            
                            "in map_IdentifierExpressions, unsupported ResolvedResult: {0}".error(unsupportedType);
                        }
                        break;
                }                
            }
            return o2MethodStream;
        }

        //public static O2MethodStream map_MemberReferenceExpressions(this O2MethodStream o2MethodStream, MethodDeclaration methodDeclaration)
        public static O2MethodStream map_MemberReferenceExpressions(this O2MethodStream o2MethodStream, INode methodDeclaration)
        {            
            var memberReferenceExpressions = methodDeclaration.iNodes<INode, MemberReferenceExpression>();
            foreach (var memberReferenceExpression in memberReferenceExpressions)
            {
                var calledIMethod = o2MethodStream.O2MappedAstData.iMethod(memberReferenceExpression);
                if (calledIMethod != null)
                    if (o2MethodStream.O2MappedAstData.has_IMethod(calledIMethod))
                        o2MethodStream.add_IMethod(calledIMethod);
                    else
                        o2MethodStream.add_ExternalIMethod(calledIMethod);
                else
                {
                    o2MethodStream.map_Expression(memberReferenceExpression);                    
                }
                //else
                //    "Could not resolve iMethod for memberReferenceExpression".error();
            }
            return o2MethodStream;
        }

        //public static O2MethodStream map_ObjectCreateExpressions(this O2MethodStream o2MethodStream, MethodDeclaration methodDeclaration)
        public static O2MethodStream map_ObjectCreateExpressions(this O2MethodStream o2MethodStream, INode methodDeclaration)
        {
            // map ObjectCreateExpression (just about the same process as for memberReferenceExpressions
            var objectCreateExpressions = methodDeclaration.iNodes<INode, ObjectCreateExpression>();
            foreach (var objectCreateExpression in objectCreateExpressions)
            {
                var calledIMethod = o2MethodStream.O2MappedAstData.iMethod(objectCreateExpression);
                if (calledIMethod != null)
                    if (o2MethodStream.O2MappedAstData.has_IMethod(calledIMethod))
                        o2MethodStream.add_IMethod(calledIMethod);
                    else
                        o2MethodStream.add_ExternalIMethod(calledIMethod);
                //else
                //    "Could not resolve iMethod for ObjectCreateExpression".error();
            }

            return o2MethodStream;
        }
        
		public static O2MethodStream map_InvocationExpressions(this O2MethodStream o2MethodStream, INode methodDeclaration)
        //public static O2MethodStream map_InvocationExpressions(this O2MethodStream o2MethodStream, MethodDeclaration methodDeclaration)
        {            
            var invocationExpressions = methodDeclaration.iNodes<INode, InvocationExpression>();
            foreach (var invocationExpression in invocationExpressions)
            {
                var calledIMethod = o2MethodStream.O2MappedAstData.iMethod(invocationExpression);
                if (calledIMethod != null)
                    if (o2MethodStream.O2MappedAstData.has_IMethod(calledIMethod))
                        o2MethodStream.add_IMethod(calledIMethod);
                    else
                        o2MethodStream.add_ExternalIMethod(calledIMethod);         
            }
            return o2MethodStream;
        }

        #endregion

        public static bool has_ExternalClass(this O2MethodStream o2MethodStream, string fullName)
        {
            return o2MethodStream.ExternalClasses.hasKey(fullName);
        }

        public static O2MethodStream map_ExternalClasses(this O2MethodStream o2MethodStream, IMethod iMethod)
        {
            var fullName = iMethod.ReturnType.FullyQualifiedName;
            var iReturnType = (IReturnType)iMethod.ReturnType;
            if (o2MethodStream.has_ExternalClass(fullName).isFalse())
                o2MethodStream.ExternalClasses.add(fullName, iReturnType);
            return o2MethodStream;
        }
        

        // CREATE Compilation Unit from data in O2MethodStream (NEED COMPLETE REWRITE)
        public static CompilationUnit compilationUnit(this O2MethodStream o2MethodStream)
        {
            var compilationUnit = new CompilationUnit();
            
            // this has to be the first since we want the first method to be the one that created the MethodStream

            // add methods that we have the code for (and want to include in the current compilationUnit			
            foreach (var iMethod in o2MethodStream.MappedIMethods.Values)
            {
                //var @namespace = iMethod.DeclaringType.Namespace;
                //var typeName = iMethod.DeclaringType.Name;
                if (iMethod.IsConstructor)
                {
                    var constructorDeclaration = o2MethodStream.O2MappedAstData.constructorDeclaration(iMethod);
                    compilationUnit.add_Ctor(iMethod.DeclaringType, constructorDeclaration);
                }
                else
                {
                    var methodDeclaration = o2MethodStream.O2MappedAstData.methodDeclaration(iMethod);
                    compilationUnit.add_Method(iMethod.DeclaringType, methodDeclaration);
                }
                compilationUnit.add_Using(iMethod.DeclaringType.Namespace);
            }

            // add external methods
            foreach (var iMethod in o2MethodStream.ExternalIMethods.Values)
            {
                //var @namespace = iMethod.DeclaringType.Namespace;
                //var typeName = iMethod.DeclaringType.Name;
                var declaringType = iMethod.DeclaringType;
                var csharpAst = iMethod.csharpCode().csharpAst();
                if (csharpAst.methods().size() == 1)
                {
                    var methodDeclaration = csharpAst.methods()[0];
                    //compilationUnit.add_Method(@namespace, typeName, methodDeclaration);
                    compilationUnit.add_Method(declaringType, methodDeclaration);                    
                    compilationUnit.add_Using(declaringType.Namespace);
                }
                else if (csharpAst.constructors().size() == 1)
                {
                    var constructor = csharpAst.constructors()[0];
                    compilationUnit.add_Ctor(declaringType, constructor);
                    compilationUnit.add_Using(declaringType.Namespace);
                    //compilationUnit.add_Ctor(@namespace, typeName, constructor);
                    //compilationUnit.add_Using(@namespace);
                }
                else
                    "*****: could not add to source code:{0}".format(iMethod.fullName()).error();
            }


            //o2MethodStream.UsingDeclarations.add("test", new UsingDeclaration("System.Collections"));
            // add Using Statements		
            var usingsAdded = compilationUnit.usings().values();
            foreach (var iReturnType in o2MethodStream.ExternalClasses.Values)
            {
                if (usingsAdded.Contains(iReturnType.Namespace).isFalse())
                {
                    usingsAdded.add(iReturnType.Namespace);
                    compilationUnit.add_Using(iReturnType.Namespace);
                }
            }


            // add fields
            foreach (var iField in o2MethodStream.Fields.Values)
            {
                if (o2MethodStream.O2MappedAstData.MapAstToNRefactory.IFieldToFieldDeclaration.ContainsKey(iField))
                    compilationUnit.add_Field(iField, o2MethodStream.O2MappedAstData.MapAstToNRefactory.IFieldToFieldDeclaration[iField]);
                else
                // try to find by name
                {
                    var foundMatch = false;
                    foreach (var item in o2MethodStream.O2MappedAstData.MapAstToNRefactory.IFieldToFieldDeclaration)
                        if (item.Key.FullyQualifiedName == iField.FullyQualifiedName)
                        {
                            compilationUnit.add_Field(iField, item.Value);
                            foundMatch = true;
                            break;
                        }
                    if (foundMatch.isFalse())
                        compilationUnit.add_Field(iField);
                }

                if (iField.ReturnType is DefaultReturnType)
                {
                	var iClass = (iField.ReturnType as DefaultReturnType).field("c");
                	if (iClass is IClass)
                		compilationUnit.add_Type(iClass as IClass);
                }
                if (iField.ReturnType is SearchClassReturnType)
                {
                    var iClass = (iField.ReturnType as SearchClassReturnType).BaseType.field("c");
                    if (iClass is IClass)
                        compilationUnit.add_Type(iClass as IClass);
                }

                
            }

            foreach (var iProperty in o2MethodStream.Properties.Values)
            {
                //try to find directly
                if (o2MethodStream.O2MappedAstData.MapAstToNRefactory.IPropertyToPropertyDeclaration.ContainsKey(iProperty))
                    compilationUnit.add_Property(iProperty, o2MethodStream.O2MappedAstData.MapAstToNRefactory.IPropertyToPropertyDeclaration[iProperty]);
                else
                // try to find by name
                {
                    var foundMatch = false;
                    foreach (var item in o2MethodStream.O2MappedAstData.MapAstToNRefactory.IPropertyToPropertyDeclaration)
                        if (item.Key.FullyQualifiedName == iProperty.FullyQualifiedName)
                        {
                            compilationUnit.add_Property(iProperty,item.Value);
                            foundMatch = true;
                            break;
                        }                    
                    if (foundMatch.isFalse())
                        compilationUnit.add_Property(iProperty);
                }
            }

            return compilationUnit;
        }

        public static string csharpCode(this O2MethodStream o2MethodStream)
        {
            var compilationUnit = o2MethodStream.compilationUnit();
            var csharpCode = compilationUnit.csharpCode();            
            //fix this which will prevent further AST mappings
            csharpCode = csharpCode.replace("using ?;".line(),"using NOT.RESOLVED.NAMESPACE;".line());
            return csharpCode;
        }

        public static O2MethodStream csharpFile(this O2MethodStream o2MethodStream)
        {
            o2MethodStream.csharpCode().saveWithExtension(".cs");
            return o2MethodStream;
        }

        public static O2MethodStream csharpFile(this O2MethodStream o2MethodStream, string targetFile)
        {
            "saving O2MethodStream as csharpFile to: {0}".format(targetFile).debug();
            o2MethodStream.csharpCode().save(targetFile);
            return o2MethodStream;
        }	
        
        public static string postCSharpCreationCodeFixes(this string csharpCode)
        {
        	// handle properties
        	var fixedCode = csharpCode.replace("get {".line() +
											   "				throw new NotImplementedException();".line() + 
											   "			}", "get;")
									  .replace("set {".line() +
											   "				throw new NotImplementedException();".line() + 
											   "			}", "set;");
			//handle autocreated exceptions
			fixedCode = fixedCode.replace("{".line() + 
										  "			throw new System.Exception(\"O2 Auto Generated Method\");".line() +
										  "		}", " { /*....*/ }");
			
			//"throw new NotImplementedException();", "XXXXXXXXXXX");
        	return fixedCode;        	
        }
    }

	public static class O2MethodStream_ExtensionMethods_Interfaces_Support
	{
	
		public static O2MethodStream resolveInterfaceCalls(this O2MethodStream methodStream)
		{
			var methodsToMap = methodStream.MappedIMethods.Values.ToArray();
			foreach(var interfaceMethod in methodsToMap) 
				methodStream.resolveInterfaceCalls(interfaceMethod);   
			return methodStream;
		}				

		public static O2MethodStream resolveInterfaceCalls(this O2MethodStream methodStream,IMethod iMethodToResolve)
		{
			var astData = methodStream.O2MappedAstData;
			try
			{
			//Action<O2MethodStream , IMethod> resolveInterfaceCalls = 
	 
				"Resolving Interface calls for: {0}".info(iMethodToResolve);
				var declaringType = iMethodToResolve.DeclaringType; 
				if (declaringType.ClassType.str() == "Interface") 
				{
					"Is interface".debug();
					
					foreach(var inheritedClass in astData.inheritedIClasses(declaringType))
					{
						var iMethods = inheritedClass.iMethods();
						var signatureToFind = iMethodToResolve.fullName().remove(iMethodToResolve.Namespace);
						var methodDeclaration = astData.methodDeclaration(iMethodToResolve);
						
						foreach(var iMethod in iMethods)
						{
							var filteredSignature =  iMethod.fullName().remove(iMethod.Namespace);										
							if (filteredSignature == signatureToFind) 
							{ 
								"Adding Interface Mapping: {0} -> {1}".debug(iMethodToResolve.fullName(), iMethod.fullName());     
								if (methodStream.MappedIMethods.ContainsKey(iMethod.fullName()).isFalse())												
									methodStream.add_IMethod(iMethod);
											
								// add call to this IMethod on its interface
								"Body: {0}".info(methodDeclaration.Body.typeName());
								"{0} = : {1}".debug(methodDeclaration.Body.str(), (methodDeclaration.Body.str() != "[NullBlockStatement]"));
								var body = (methodDeclaration.Body.str() != "[NullBlockStatement]") 
											? methodDeclaration.Body 
											: methodDeclaration.add_Body();																						
								var parameters = new List<IdentifierExpression>();			
								foreach(var parameter in iMethod.Parameters)	
									parameters.Add(new IdentifierExpression(parameter.Name)); 
								var tempBlockStatement = new BlockStatement();
								var tempInvocation = tempBlockStatement.add_Invocation(iMethod.Namespace,iMethod.name(), parameters.ToArray());			
								//make sure we don't add this more than once 
								if (body.csharpCode().contains(tempBlockStatement.csharpCode()).isFalse()) 
								{											
									"   Adding Interface Method call: {0}".info(tempBlockStatement.csharpCode().trim());     
									body.add_Invocation(iMethod.Namespace,iMethod.name(), parameters.ToArray());												
								}									 											 
							}
						}
					}
				}
			}
			catch(Exception ex)
			{
				"[O2MethodStream.resolveInterfaceCalls]: {0}".error(ex.Message);
			}
			return methodStream;
	   }	
	}
}
