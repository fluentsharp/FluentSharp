using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.CoreLib.Interfaces;
using FluentSharp.REPL;
using FluentSharp.WinForms;
using FluentSharp.WinForms.O2Findings;
using ICSharpCode.TextEditor;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.SharpDevelop.Dom;
using ICSharpCode.NRefactory;

//O2File:Ast_Engine_ExtensionMethods.cs
//O2Ref:QuickGraph.dll

namespace FluentSharp.CSharpAST.Utils
{
	public class O2MappedAstData_Cache
	{
		public static Dictionary<Expression,ResolveResult> AstResolve_Cache_ResolveExpression = new Dictionary<Expression,ResolveResult>();
		public static Dictionary<Expression, IMethodOrProperty> AstResolve_Cache_FromExpressionGetIMethodOrProperty = new Dictionary<Expression, IMethodOrProperty>();
	}
	
    public static class O2MappedAstData_ExtensionMethods_INodes_IClass_IMethods
    {
		 
		public static O2MappedAstData dispose(this O2MappedAstData o2MappedAstData)
        {        	
        	if (o2MappedAstData.notNull())
        		o2MappedAstData.Dispose();        		
            return o2MappedAstData;
        }
        
        //this removes the references resolution database which usually takes about 40Mb and is not needed in
        //most method and code stream calculations
        public static O2MappedAstData dispose_unloadProjectContent(this O2MappedAstData astData)
        {        	
        	if (astData.notNull())
        	{
        		//o2MappedAstData.O2AstResolver.pcRegistry.Dispose();
        		astData.O2AstResolver.pcRegistry.UnloadProjectContent(astData.O2AstResolver.myProjectContent);
        		PublicDI.config.gcCollect();
        	}
            return astData;
        }
                        
        #region iNode(s) 

        public static INode iNode(this O2MappedAstData o2MappedAstData, string file, Caret caret)
        {
        	if (caret != null)
        	{
    		    var adjustedLine = caret.Line + 1;
                var adjustedColumn = caret.Column + 1;
                return o2MappedAstData.iNode(file, adjustedLine,adjustedColumn);
        	}
        	return null;
        }
        public static INode iNode(this O2MappedAstData o2MappedAstData, string file, int line, int column)
        {
            if (o2MappedAstData != null && file != null)
            {
                if (o2MappedAstData.FileToINodes.hasKey(file))
                {
                    var allINodes = o2MappedAstData.FileToINodes[file];                
                    var iNode = allINodes.getINodeAt(line, column);
                    if (iNode != null)
                        return iNode;
                    "Could not find iNode for position {0}:{1} in file:{2}".format(line, column, file).error();
                }
                "o2MappedAstData did not have INodes for file:{0}".format(file).error();
            }
            return null;
        }

        public static List<INode> iNodes(this O2MappedAstData o2MappedAstData)
        {
            var iNodes = new List<INode>();
            foreach (var item in o2MappedAstData.FileToINodes)
                iNodes.add(item.Value.AllNodes);
            return iNodes;
        }

        public static List<T> iNodes<T>(this O2MappedAstData o2MappedAstData)
            where T : INode
        {
            var results = from iNode in o2MappedAstData.iNodes() where iNode is T select (T)iNode;
            return results.ToList();
        }
        
        public static List<INode> iNodes(this O2MappedAstData o2MappedAstData, string file)
        {
            if (o2MappedAstData.FileToINodes.hasKey(file))
                return o2MappedAstData.FileToINodes[file].AllNodes;
            return null;
        }

        public static List<T> iNodes<T>(this O2MappedAstData o2MappedAstData, string file)
            where T : INode
        {
            if (o2MappedAstData.FileToINodes.hasKey(file))
                return o2MappedAstData.FileToINodes[file].allByType<T>();
            return null;
        }

        public static Dictionary<string, List<INode>> iNodes_By_Type(this O2MappedAstData astData)
        {
            return astData.iNodes_By_Type("");
        }

        public static Dictionary<string, List<INode>> iNodes_By_Type(this O2MappedAstData astData, string iNodeType_RegExFilter)
        {
            var iNodesByType = new Dictionary<string, List<INode>>();
            foreach (var iNode in astData.iNodes())
            {
                var typeName = iNode.typeName();
                if (iNodeType_RegExFilter.valid().isFalse() || typeName.regEx(iNodeType_RegExFilter))
                    iNodesByType.add(typeName, iNode);
            }
            return iNodesByType;
        }

        #endregion

        #region ISpecial

        public static Dictionary<string, List<ISpecial>> iSpecials_By_Type(this O2MappedAstData astData)
        {
            var iSpecialsByType = new Dictionary<string, List<ISpecial>>();
            foreach (var iSpecial in astData.iSpecials())
            {
                var typeName = iSpecial.typeName();
                iSpecialsByType.add(typeName, iSpecial);
            }

            return iSpecialsByType;
        }


        public static List<ISpecial> comments(this O2MappedAstData astData)
        {
            var iSpecialByType = astData.iSpecials_By_Type();
            if (iSpecialByType.hasKey("Comment"))
                return iSpecialByType["Comment"];
            return new List<ISpecial>();
        }

        public static Dictionary<string, List<ISpecial>> comments_IndexedByTextValue(this O2MappedAstData astData)
        {
            return astData.comments_IndexedByTextValue("");
        }

        public static Dictionary<string, List<ISpecial>> comments_IndexedByTextValue(this O2MappedAstData astData, string commentsFilter)
        {
            return astData.comments()
                          .indexOnProperty("CommentText", commentsFilter);
        }


        public static List<ISpecial> iSpecials(this O2MappedAstData astData)
        {
            var iSpecials = new List<ISpecial>();
            foreach (var item in astData.FileToSpecials)
                iSpecials.AddRange(item.Value);
            return iSpecials;

        }


        #endregion

		#region iClass(es)
		
		//returns a list since there could be more that one class with the same name (case of partial classes)
		public static List<IClass> iClass(this O2MappedAstData o2MappedAstData,string classToFind)
		{
			return (from iClass in o2MappedAstData.iClasses()
				    where iClass.FullyQualifiedName ==classToFind
				    select iClass).toList();
			//foreach(var iClass in o2MappedAstData.iClasses())
				//if (iClass.FullyQualifiedName ==classToFind)
//					return iClass;
//			return null;
		}
		
		#endregion

        #region iMethod(s)

        public static IMethod iMethod(this O2MappedAstData o2MappedAstData, ConstructorDeclaration constructorDeclaration)
        {
            if (constructorDeclaration != null)
                if (o2MappedAstData.MapAstToNRefactory.ConstructorDeclarationToIMethod.ContainsKey(constructorDeclaration))
                    return o2MappedAstData.MapAstToNRefactory.ConstructorDeclarationToIMethod[constructorDeclaration];
            return null;
        }

        public static ConstructorDeclaration constructorDeclaration(this O2MappedAstData o2MappedAstData, IMethod iMethod)
        {
            if (iMethod != null)
                if (o2MappedAstData.MapAstToNRefactory.IMethodToConstructorDeclaration.hasKey(iMethod))
                    return o2MappedAstData.MapAstToNRefactory.IMethodToConstructorDeclaration[iMethod];
            return null;
        }

        public static IMethod iMethod(this O2MappedAstData o2MappedAstData, MethodDeclaration methodDeclaration)
        {

            if (methodDeclaration != null)
                if (o2MappedAstData.MapAstToNRefactory.MethodDeclarationToIMethod.ContainsKey(methodDeclaration))
                    return o2MappedAstData.MapAstToNRefactory.MethodDeclarationToIMethod[methodDeclaration];
            return null;
        }

        public static IMethod iMethod(this O2MappedAstData o2MappedAstData, MemberReferenceExpression memberReferenceExpression)
        {
            return o2MappedAstData.fromMemberReferenceExpressionGetIMethod(memberReferenceExpression);            
        }

        public static IMethod iMethod(this O2MappedAstData o2MappedAstData, ObjectCreateExpression objectCreateExpression)
        {
            return o2MappedAstData.fromExpressionGetIMethod(objectCreateExpression as Expression);
        }

        public static IMethod iMethod(this O2MappedAstData o2MappedAstData, InvocationExpression invocationExpression)
        {
            return o2MappedAstData.fromExpressionGetIMethod(invocationExpression as Expression);
        }
		public static List<IMethod> iMethods(this IClass iClass)
		{
			return iClass.Methods.toList();
		}
		
        public static List<IMethod> iMethods(this O2MappedAstData o2MappedAstData, string file)
        {
        	if (o2MappedAstData.isNull() || file.isNull())
        		return new List<IMethod> ();
            var methodDeclarations = o2MappedAstData.iNodes<MethodDeclaration>(file);
            return o2MappedAstData.iMethods(methodDeclarations);
        }

        public static List<IMethod> iMethods(this O2MappedAstData o2MappedAstData, List<MethodDeclaration> methodDeclarations)
        {
            var iMethods = new List<IMethod>();
            if (methodDeclarations != null)
                foreach (var methodDeclaration in methodDeclarations)
                {
                    var iMethod = o2MappedAstData.iMethod(methodDeclaration);
                    if (iMethod != null && iMethod is IMethod)
                        iMethods.add(iMethod);
                }
            return iMethods;
        }	
        
        public static IMethod iMethod_withSignature(this O2MappedAstData astData, string iMethodSignature)
        {
        	if (astData.notNull())
        	{
            	foreach (var iMethod in astData.iMethods())
            	    if (iMethod.fullName() == iMethodSignature)
            	        return iMethod;
			}
            return null;
        }
		
		#endregion
        
        
        #region IField
        public static FieldDeclaration fieldDeclaration(this O2MappedAstData astData, IField iField)
        {
        	if( astData.MapAstToNRefactory.IFieldToFieldDeclaration.ContainsKey(iField))
				return astData.MapAstToNRefactory.IFieldToFieldDeclaration[iField];
			return null;
		}
        #endregion
        
        #region IProperty
        public static PropertyDeclaration propertyDeclaration(this O2MappedAstData astData, IProperty iProperty)
        {
        	if( astData.MapAstToNRefactory.IPropertyToPropertyDeclaration.ContainsKey(iProperty))
				return astData.MapAstToNRefactory.IPropertyToPropertyDeclaration[iProperty];
			return null;
		}
        #endregion

	}
	
	public static class O2MappedAstData_ExtensionMethods_Resolve
	{			
        public static ResolveResult resolveExpression(this O2MappedAstData o2MappedAstData, Expression expression)
        {
        	if (expression.isNull())
        		return null;
        		
        	if (O2MappedAstData_Cache.AstResolve_Cache_ResolveExpression.ContainsKey(expression))        	        		
        		return O2MappedAstData_Cache.AstResolve_Cache_ResolveExpression[expression];
        	
        	Func<ResolveResult> resolveExpression = 	
        	()=> {		        	
		            //resolve type  (move this into a method called typeResolve
		            var compilationUnit = expression.compilationUnit();
		            if (compilationUnit != null)
		            {                
		                o2MappedAstData.O2AstResolver.setCurrentCompilationUnit(compilationUnit);
		                var resolved = o2MappedAstData.O2AstResolver.resolve(expression);
		                if (resolved is UnknownIdentifierResolveResult)
		                {
		                    // this is a hack to deal with partial classes (which are located in different files (this could be heavily optimized
		                    var callingClassName = resolved.CallingClass.FullyQualifiedName;
		                    foreach (var item in o2MappedAstData.MapAstToNRefactory.IClassToTypeDeclaration)
		                    {
		                        var iClass = item.Key;
		                        var typeDeclaration = item.Value;
		                        if (iClass.FullyQualifiedName == callingClassName)
		                        {
		                            var otherCompilationUnit = typeDeclaration.compilationUnit();
		                            if (otherCompilationUnit != compilationUnit)
		                            {
		                                o2MappedAstData.O2AstResolver.setCurrentCompilationUnit(otherCompilationUnit);
		                                var resolved2 = o2MappedAstData.O2AstResolver.resolve(expression, iClass, null);
		                                if ((resolved2 is UnknownIdentifierResolveResult).isFalse())
		                                    return resolved2;
		                            }
		                        }
		                    }
		
		                    // if we got here it means that we were not able to resolve this field (even after looking in partial files)
		                    if (o2MappedAstData.debugMode)
			                    if (expression is IdentifierExpression)	                    
									"in resolved Expression, it was not possible to resolve: {0}".error((expression as IdentifierExpression).Identifier);							
			                    else		                    
			                        "in resolved Expression, it was not possible to resolve: {0}".error(expression.str());
		                    return resolved;
		                }
		                return resolved;
		            }
		            return null;
		        };
	        var result = resolveExpression();	        
	        O2MappedAstData_Cache.AstResolve_Cache_ResolveExpression.Add(expression, result);
	        return result;
        }

        public static IMethod fromExpressionGetIMethod(this O2MappedAstData o2MappedAstData, Expression expression)
        {        	
            var result = o2MappedAstData.fromExpressionGetIMethodOrProperty(expression);
            if (result is IMethod)
                return (IMethod)result;
            return null;
        }
                
        
        public static IMethodOrProperty fromExpressionGetIMethodOrProperty(this O2MappedAstData o2MappedAstData, Expression expression)
        {        	
        	//"In fromExpressionGetIMethodOrProperty for:{0}".error(expression.str());
            if (expression == null)
                return null;
                
            if (O2MappedAstData_Cache.AstResolve_Cache_FromExpressionGetIMethodOrProperty.ContainsKey(expression))                        	
            	return O2MappedAstData_Cache.AstResolve_Cache_FromExpressionGetIMethodOrProperty[expression];            	
            	
            Func<IMethodOrProperty> fromExpressionGetIMethodOrProperty = 
            	()=>{            	
			            var compilationUnit = expression.compilationUnit();
			            if (compilationUnit == null)
			                return null;
			            o2MappedAstData.O2AstResolver.setCurrentCompilationUnit(compilationUnit);
			            var resolved = o2MappedAstData.O2AstResolver.resolve(expression);
			            if (resolved is MethodGroupResolveResult)
			            {
			                var resolvedIMethods = new List<IMethod>();
			                foreach (var groupResult in (resolved as MethodGroupResolveResult).Methods)
			                    foreach (var method in groupResult)
			                    {
			                        resolvedIMethods.Add(method);
			                    }
			                if (resolvedIMethods.Count == 1)
			                    return resolvedIMethods[0];
			            }
			            if (resolved != null)
			                if (resolved is MemberResolveResult)
			                {
			                    var memberResolveResult = (MemberResolveResult)resolved;
			                    if (memberResolveResult.ResolvedMember is IMethodOrProperty)
			                        return memberResolveResult.ResolvedMember as IMethodOrProperty;
			                    //else
			                    //    "in fromExpressionGetIMethod, could not resolve Expression".error();
			                }
			
			            //return null;
			            return o2MappedAstData.loseFindIMethodFrom_MethodDeclaration(expression);
					};
			var result = fromExpressionGetIMethodOrProperty();
			O2MappedAstData_Cache.AstResolve_Cache_FromExpressionGetIMethodOrProperty.Add(expression,result);
			return result;
        }

        // this handle some special cases where we have the code of this type but the code complete resolver couldn't find it
        // for example in the cases where there are two classes with the same name
        public static IMethod loseFindIMethodFrom_MethodDeclaration(this O2MappedAstData o2MappedAstData, Expression expression)
        {
            if (expression is InvocationExpression)
            {
                var invocationExpression = (expression as InvocationExpression);
                if (invocationExpression.TargetObject is Expression)                
                    return o2MappedAstData.loseFindIMethodFrom_MethodDeclaration(invocationExpression.TargetObject as Expression);
            }
            // move to loseFindIMethodFrom_MethodDeclartion
            if (expression is MemberReferenceExpression)
            {
                var memberReferenceExpression = (MemberReferenceExpression)expression;                
                
               if (memberReferenceExpression.Parent is InvocationExpression)
               {
                   var methodName = memberReferenceExpression.MemberName;
                   var parameterCount = (memberReferenceExpression.Parent as InvocationExpression).Arguments.Count;
                   var className = "";

                   if (memberReferenceExpression.TargetObject is IdentifierExpression)
                       className = (memberReferenceExpression.TargetObject as IdentifierExpression).Identifier;
                   else if (memberReferenceExpression.TargetObject is MemberReferenceExpression)
                       className = (memberReferenceExpression.TargetObject as MemberReferenceExpression).MemberName;

                   return o2MappedAstData.loseFindIMethod(className, methodName, parameterCount);
              }
                //var parametersCount = memberReferenceExpression
            }                       
            return null;
        }

        public static IMethod loseFindIMethod(this O2MappedAstData o2MappedAstData, string className, string methodName, int parameterCount)
        {
            foreach (var iClass in o2MappedAstData.O2AstResolver.myProjectContent.Classes)
                if (iClass.Name == className)
                    foreach (var iMethod in iClass.Methods)
                        if (iMethod.Name == methodName)
                            if (iMethod.Parameters.Count == parameterCount)
                                return iMethod;
            return null;
        }


        public static IMethodOrProperty fromMemberReferenceExpressionGetIMethodOrProperty(this O2MappedAstData o2MappedAstData, MemberReferenceExpression memberReferenceExpression)
        {
            return o2MappedAstData.fromExpressionGetIMethodOrProperty(memberReferenceExpression as Expression);  
        }

        public static IMethod fromMemberReferenceExpressionGetIMethod(this O2MappedAstData o2MappedAstData, MemberReferenceExpression memberReferenceExpression)
        {
            return o2MappedAstData.fromExpressionGetIMethod(memberReferenceExpression as Expression);  
/*            var compilationUnit = memberReferenceExpression.compilationUnit();
            o2MappedAstData.O2AstResolver.setCurrentCompilationUnit(compilationUnit);            
            var resolved = o2MappedAstData.O2AstResolver.resolve(memberReferenceExpression);
            if (resolved != null)
            {                
                if (resolved is MethodGroupResolveResult)
                {
                    var resolvedIMethods = new List<IMethod>();                    
                    foreach (var groupResult in (resolved as MethodGroupResolveResult).Methods)
                        foreach (var method in groupResult)
                        {
                            resolvedIMethods.Add(method);                 
                        }
                    if (resolvedIMethods.Count == 1)
                        return resolvedIMethods[0];
                    else
                        "in fromMemberReferenceExpressionGetIMethod: could not find valid IMethod (resolvedIMethods.Count = {0}".format(resolvedIMethods.Count).error();
                }
                else
                    "in fromMemberReferenceExpressionGetIMethod: expected MethodGroupResolveResult and got: {0}".format(resolved.typeName()).error();
            }
            else
                "fromMemberReferenceExpressionGetIMethod: Resolved (for MemberReferenceExpression) WAS null".error();
            return null;                        
 * */
        }

        public static MethodDeclaration fromMemberReferenceExpressionGetMethodDeclaration(this O2MappedAstData o2MappedAstData, MemberReferenceExpression memberReferenceExpression)
        {
            var resolvedIMethod = o2MappedAstData.fromMemberReferenceExpressionGetIMethod(memberReferenceExpression);
            
            if (o2MappedAstData.MapAstToNRefactory.IMethodToMethodDeclaration.ContainsKey(resolvedIMethod))
            {
                var methodDeclaration = o2MappedAstData.MapAstToNRefactory.IMethodToMethodDeclaration[resolvedIMethod];
                //"found methodDeclaration: {0}".format(resolvedIMethod).debug(); ;
                return methodDeclaration;
                //"methodDeclaration : {0}".format(methodDeclaration).info();
                //show.info(methodDeclaration);
            }
            else
                "in fromMemberReferenceExpressionGetMethodDeclaration: no IMethod -> MethodDeclaration mapping".error();
            return null;
        }

        public static Dictionary<MethodDeclaration, IMethod> getMappedIMethods(this O2MappedAstData o2MappedAstData, List<MethodDeclaration> methodDeclarations)
        {
            var mappedIMethods = new Dictionary<MethodDeclaration, IMethod>();
            foreach (var methodDeclaration in methodDeclarations)
            {
                var iMethod = o2MappedAstData.MapAstToNRefactory.MethodDeclarationToIMethod[methodDeclaration];
                mappedIMethods.add(methodDeclaration, iMethod);
            }
            return mappedIMethods;
        }

        public static bool has_IMethod(this O2MappedAstData o2MappedAstData, IMethod iMethod)
        {
            // try to find by IMethod reference
            if (iMethod != null)
            {
                var directMapping = o2MappedAstData.MapAstToNRefactory.IMethodToMethodDeclaration.hasKey(iMethod) ||
                                    o2MappedAstData.MapAstToNRefactory.IMethodToConstructorDeclaration.hasKey(iMethod);
                if (directMapping)
                    return true;
                // try to find by signature (this shouldn't be needed but there must be a bug which is making the resolved calledIMethod (current for InvocationExpression) to not be mapped)				
                foreach (var item in o2MappedAstData.MapAstToNRefactory.IMethodToMethodDeclaration)
                    if (item.Key.fullName() == iMethod.fullName())
                    {
                        //"add extra mapping to IMethodToMethodDeclaration for: {0}".format(item.Key.fullName()).error();
                        //o2MappedAstData.MapAstToNRefactory.IMethodToMethodDeclaration.add(iMethod, item.Value);
                        return true;
                    }
            }
            return false;
        }                                		

        public static List<INode> calledINodesReferences(this O2MappedAstData o2MappedAstData, IMethod iMethod)
        {
            var calledIMethodsRefs = new List<INode>();
            if (iMethod != null)
            {                
                var methodDeclaration = o2MappedAstData.methodDeclaration(iMethod);
				if (methodDeclaration.notNull())
				{
                	// handle invocations via MemberReferenceExpression
                	calledIMethodsRefs.add(methodDeclaration.iNodes<INode, MemberReferenceExpression>());
                	calledIMethodsRefs.add(methodDeclaration.iNodes<INode, InvocationExpression>());
                	calledIMethodsRefs.add(methodDeclaration.iNodes<INode, ObjectCreateExpression>());
                }
            }
            return calledIMethodsRefs;
        }

        //#endregion
	}
	
	        					
	public static class O2MappedAstData_ExtensionMethods_MethodStreams
	{
        #region methodStreams

        public static Dictionary<IMethod, string> methodStreams(this O2MappedAstData astData, Action<string> statusMessage)
        {
            var methodStreams = new Dictionary<IMethod, string>();
            foreach (var iMethod in astData.iMethods())
                methodStreams.Add(iMethod, astData.createO2MethodStream(iMethod).csharpCode());
            return methodStreams;
        }

        public static string methodStream(this O2MappedAstData astData, IMethod iMethod)
        {
            return astData.createO2MethodStream(iMethod).csharpCode();
        }
                                                                                
        public static string methodStream_SharpCode(this O2MappedAstData o2MappedAstData, MethodDeclaration methodDeclaration)
        {
            var iMethod = o2MappedAstData.iMethod(methodDeclaration);
            var methodStream = o2MappedAstData.createO2MethodStream(iMethod);
            return methodStream.csharpCode();
        }

        public static Dictionary<IMethod, string> methodStreams(this O2MappedAstData astData)
        {
            return astData.methodStreams(null);
        }

        #endregion
        
        public static INode fromFirstMethod_getINodeAtPosition(this O2MappedAstData astData, int iNodePosition)
        {
        	if (astData.isNull())
        		return null;
        	try
        	{
        		var methodDeclarations =  astData.methodDeclarations();
	        	if (methodDeclarations.notNull() && methodDeclarations.size() > 0)
	        	{
	        		var firstMethod =methodDeclarations[0];  
					var iNodesInFirstMethod = firstMethod.iNodes();
					if (iNodesInFirstMethod.notNull() && iNodesInFirstMethod.size()>iNodePosition)
						return iNodesInFirstMethod[iNodePosition];
				}
			}
			catch(Exception ex)
			{
				ex.log("in fromFirstMethod_getINodeAtPosition");
			}
			return null;
        }
	}
	
	public static class O2MappedAstData_ExtensionMethods_CodeStreams
	{
        #region codeStreams
        
        public static TreeView add_CodeStreams(this O2MappedAstData astData, TreeView treeView, List<IMethod> iMethods)
        {
            foreach (var iMethod in iMethods)
                astData.add_CodeStreams(treeView, iMethod);
            return treeView;
        }

        public static TreeView add_CodeStreams(this O2MappedAstData astData, TreeView treeView, IMethod iMethod)
        {
            var methodName = iMethod.name();
            var methodStream = astData.methodStream(iMethod);
            var codeStreams = methodStream.codeStreams();
            return treeView.add_CodeStreams(methodName, methodStream, codeStreams);
        }

        public static TreeView add_CodeStreams(this TreeView treeView, string rootFunctionName, string methodStreamCode, List<O2CodeStream> codeStreams)
        {
            return treeView.add_CodeStreams(rootFunctionName, methodStreamCode, codeStreams.codeStreams_UniquePaths());
        }
        public static TreeView add_CodeStreams(this TreeView treeView, string rootFunctionName, string methodStreamCode, List<List<O2CodeStreamNode>> codeStreamsPaths)
        {
            var count = 1;
            var treeNode = treeView.add_Node(rootFunctionName, methodStreamCode);
            //foreach(var codeStream in codeStreams.codeStreams_UniquePaths())
            //show.info(codeStreams[0]);
            foreach (var uniquePath in codeStreamsPaths)
            {
                //foreach(var uniquePath in codeStream.
                //var codeStreamNodes = codeStream.O2CodeStreamNodes.Values.toList();


                var iNodes = uniquePath.iNodes();// codeStream.O2CodeStreamNodes.Keys.toList();
                var nodeText = "Path #{0}       ({1} steps)".format(count++, uniquePath.size());
                treeNode.add_Node(nodeText, iNodes).add_Nodes(uniquePath);
            }
            return treeView;
        }

        public static List<IO2Finding> createAndShowCodeStreams(this O2MappedAstData astData, TreeView codeStreamViewer)
        {
            return astData.createAndShowCodeStreams(astData.iMethods(), codeStreamViewer);
        }

        public static List<IO2Finding> createAndShowCodeStreams(this O2MappedAstData astData, List<IMethod> iMethods, TreeView codeStreamViewer)
        {
            var o2Findings = new List<IO2Finding>();
            var processedMethods = new List<string>();		// hack deal with the problem that in some cases new IMethods mappings have to be added to astData (for more info search for "add extra mapping to IMethodToMethodDeclaration for:")
            foreach (var iMethod in iMethods)
            {
                if (processedMethods.Contains(iMethod.fullName()).isFalse())
                {
                    o2Findings.add(astData.createAndShowCodeStreams(iMethod, codeStreamViewer));
                    processedMethods.add(iMethod.fullName());
                }
            }
            return o2Findings;
        }

        public static List<IO2Finding> createAndShowCodeStreams(this O2MappedAstData astData, IMethod iMethod, TreeView codeStreamViewer)
        {
            var o2Findings = new List<IO2Finding>();
            var methodName = iMethod.name();
            var methodStream = astData.methodStream(iMethod);

            var codeStreams = methodStream.codeStreams_UniquePaths();

            codeStreamViewer.add_CodeStreams(methodName, methodStream, codeStreams);
            o2Findings.add_CodeStreams(astData,methodStream, codeStreams);
            return o2Findings;
        }

        // add the abilty to define where the taint starts (method parameter, external caller, etc..)
        public static List<O2CodeStream> codeStreams(this string methodStreamFile)
        {
            if (methodStreamFile.fileExists().isFalse())
                methodStreamFile = methodStreamFile.saveWithExtension(".MethodStream.cs");

            var codeStreams = new List<O2CodeStream>();
            var AstData_MethodStream = new O2MappedAstData();
            AstData_MethodStream.loadFile(methodStreamFile);
            var iMethods = AstData_MethodStream.iMethods();
            if (iMethods.size() > 0)
            //if (AstData_MethodStream.iNodes().size() > 10)
            {
                var iMethod = iMethods[0];
                if (AstData_MethodStream.methodDeclaration(iMethod) != null)
                {
                    var parameters = AstData_MethodStream.methodDeclaration(iMethod).parameters();
                    var TaintRules = new O2CodeStreamTaintRules();
                    foreach (var parameter in parameters)
                    {
                        var codeStream = new O2CodeStream(AstData_MethodStream, TaintRules, methodStreamFile);
                        codeStream.createStream(parameter, null);
                        codeStreams.add(codeStream);
                    }
                }
            }
            return codeStreams;
        }

        public static List<List<O2CodeStreamNode>> codeStreams_UniquePaths(this List<O2CodeStream> codeStreams)
        {
            var uniqueCodeStreams = new List<List<O2CodeStreamNode>>();
            foreach (var codeStream in codeStreams)
                foreach (var uniquePath in codeStream.getUniqueStreamPaths(100))
                    uniqueCodeStreams.Add(uniquePath);
            return uniqueCodeStreams;
        }
        

        public static List<List<O2CodeStreamNode>> codeStreams_UniquePaths(this O2MappedAstData astData, IMethod iMethod)
    	{			
    		var methodStream =  astData.methodStream(iMethod);
    		return methodStream.codeStreams_UniquePaths();
    	}
    	
    	public static List<List<O2CodeStreamNode>> codeStreams_UniquePaths(this string methodStream)
    	{    		
    		var codeStreams = methodStream.codeStreams();
    		return codeStreams.codeStreams_UniquePaths();
    	}

        public static List<IO2Finding> add_CodeStreams(this List<IO2Finding> o2Findings, O2MappedAstData astData, string methodStream, List<List<O2CodeStreamNode>> codeStreams)
        {
            o2Findings.AddRange(codeStreams.o2Findings(astData,methodStream, "vulnName", "vulnType", "Source of Tainted Data"));
            return o2Findings;
        }

        public static List<INode> iNodes(this List<O2CodeStreamNode> codeStreamNodes)
    	{
    		var iNodes = from node in codeStreamNodes select node.INode;
    		return iNodes.toList();
    	}

        public static bool contains(this List<O2CodeStreamNode> codeStream, string stringToFind)
    	{
    		foreach(var streamNode in codeStream)
    		{    			
    			if (streamNode.str().contains(stringToFind))
    				return true;
    		}
    		return false;
    	}

        #endregion
	}
	
	public static class O2MappedAstData_ExtensionMethods_O2Findings
	{
        #region codeStreams & O2Findings
        public static List<IO2Finding> o2Findings(this O2CodeStream codeStream)
		{
			return codeStream.wrapOnList().o2Findings("vulnName", "vulnType", "Source of Tainted Data");
		}
		
		public static List<IO2Finding> o2Findings(this List<O2CodeStream> codeStreams)
		{
			return codeStreams.o2Findings("vulnName", "vulnType", "Source of Tainted Data");
		}
		
        public static List<IO2Finding> o2Findings(this List<O2CodeStream> codeStreams, string vulnName, string vulnType, string sourceNodeText)
        {
            var o2Findings = new List<IO2Finding>();
            foreach (var codeStream in codeStreams)                
                o2Findings.add(codeStream.o2Findings(vulnName, vulnType, sourceNodeText));
            return o2Findings;
        }

        public static List<IO2Finding> o2Findings(this O2CodeStream o2CodeStream, string vulnName, string vulnType, string sourceNodeText)
        {
        	if (o2CodeStream.isNull())
        		return new List<IO2Finding>();
            var uniqueStreamPaths = o2CodeStream.getUniqueStreamPaths(100);
            return uniqueStreamPaths.o2Findings(o2CodeStream.O2MappedAstData, o2CodeStream.SourceFile, vulnName, vulnType, sourceNodeText);
        }

        public static List<IO2Finding> o2Findings(this List<List<O2CodeStreamNode>> codeStreamPaths, O2MappedAstData astData, string methodStreamFile, string vulnName, string vulnType, string sourceNodeText)
        {        	
            if (methodStreamFile.fileExists().isFalse())
                methodStreamFile = methodStreamFile.saveWithExtension(".MethodStream.cs");
            var o2Findings = new List<IO2Finding>();
            foreach (var uniquePath in codeStreamPaths)
            {
                var o2Finding = new O2Finding();
                o2Finding.vulnName = vulnName;
                o2Finding.vulnType = vulnType;
                var o2Trace = o2Finding.addTrace(sourceNodeText);
                var rootTrace = o2Trace;  
                IO2Trace parentTrace = null;
                IO2Trace methodTrace = null;
                foreach (var streamNode in uniquePath)
                {
                	if (methodTrace.isNull())
                	{
                		var methodDeclaration = streamNode.INode.methodDeclaration();
                		if (methodDeclaration.notNull())
                		{
                			var iMethod = astData.iMethod(methodDeclaration);
                			methodTrace = new O2Trace(iMethod.DotNetName);                            	
                			methodTrace.context = iMethod.fullName();							
						}
                	}
                	parentTrace = o2Trace;
                    if (streamNode.INode is ParameterDeclarationExpression)
                    {
                        var parameter = (ParameterDeclarationExpression)streamNode.INode ;
                        if (parameter.Parent is MethodDeclaration)
                        {     
                        	
                        	var attributes = "";
                            foreach (var attribute in (parameter.Parent as MethodDeclaration).attributes())
                            {
                            	var traceText = "Attribute: {0}".format(attribute.Name);
                            	var attributesTrace = new O2Trace(traceText);
                            	attributesTrace.context = attribute.str();
                                //o2Trace.file = methodStreamFile;
                                //o2Trace.lineNumber = attribute.StartLocation.Line.uInt();
                                o2Trace.childTraces.Add(attributesTrace);
                                attributes += "{0} ".format(attribute.str());
                            }                            
                        }
                    }

                    var newTrace = new O2Trace(streamNode.str());
                    o2Trace.childTraces.Add(newTrace);
                    o2Trace = newTrace;
                    o2Trace.file = methodStreamFile;
                    o2Trace.lineNumber = streamNode.INode.StartLocation.Line.uInt();
                }
                //if (rootTrace.childTraces.size()>1)
	            //    rootTrace.childTraces[1].traceType = TraceType.Source;
	            //else
	           rootTrace.childTraces[0].traceType = TraceType.Source;
	           if (methodTrace.notNull())
	           		rootTrace.childTraces.Add(methodTrace);
	            	
                
                //fix the sink when it is not a method and its parent is
                if(parentTrace.notNull() && 
                   o2Trace.signature.starts("method ->").isFalse() &&
                   parentTrace.signature.starts("method ->"))
                {
                	parentTrace.traceType = TraceType.Known_Sink;                	
                }
                else
					o2Trace.traceType = TraceType.Known_Sink;

                o2Finding.lineNumber = o2Trace.lineNumber;
                o2Finding.file = o2Trace.file;
                o2Findings.Add(o2Finding);
            }
            return o2Findings;
        }

        #endregion
	}
	
	public static class O2MappedAstData_ExtensionMethods_Mics
	{
        #region mist info

		public static List<ICSharpCode.NRefactory.Ast.Attribute> attributes(this O2MappedAstData o2MappedAstData)
        {
            return o2MappedAstData.iNodes<ICSharpCode.NRefactory.Ast.Attribute>();
        }
        
        public static string fromINodeGetFile(this O2MappedAstData o2MappedAstData, INode iNode)
        {
            if (iNode != null)
            {
                var compilationUnit = iNode.compilationUnit();
                return o2MappedAstData.fromCompilationUnitGetFile(compilationUnit);
            }
            return "";
            //var methodDeclaration = o2MappedData.fromMemberReferenceExpressionGetMethodDeclaration(iNode as MemberReferenceExpression);
        }

        public static string fromCompilationUnitGetFile(this O2MappedAstData o2MappedAstData, CompilationUnit compilationUnit)
        {
            if (compilationUnit != null)
            {
                foreach (var item in o2MappedAstData.FileToCompilationUnit)
                    if (compilationUnit == item.Value)
                        return item.Key;
                "in fromCompilationUnitGetFile: could not map compilation unit to file".error();
            }
            return "";
        }

        public static string fullName(this O2MappedAstData o2MappedAstData, MethodDeclaration methodDeclaration)
        {
            var iMethod = o2MappedAstData.iMethod(methodDeclaration);
            return iMethod.fullName();
        }

        public static string fullName(this O2MappedAstData o2MappedAstData, MemberReferenceExpression memberReferenceExpression)
        {
            var iMethod = o2MappedAstData.iMethod(memberReferenceExpression);
            return iMethod.fullName();
        }
        
        

		public static List<IClass> inheritedIClasses(this O2MappedAstData o2MappedAstData, IClass iClassToFind)
		{
			var mappedByInheritClass = new Dictionary<IClass, List<IClass>>();
			foreach(var iClass in o2MappedAstData.iClasses())
				foreach(var inheritClass in iClass.ClassInheritanceTree)	
					if (iClass != inheritClass)			
						mappedByInheritClass.add(inheritClass,iClass); 
			if (mappedByInheritClass.hasKey(iClassToFind))
				return mappedByInheritClass[iClassToFind];
			return null;
		}
        
        public static CompilationUnit compilationUnit(this O2MappedAstData o2MappedAstData, string file)
        {
            if (o2MappedAstData.FileToCompilationUnit.hasKey(file))
                return o2MappedAstData.FileToCompilationUnit[file];
            return null;
        }
        
        public static TypeDeclaration typeDeclaration(this O2MappedAstData o2MappedAstData, IClass iClass)	
		{
			if (o2MappedAstData.MapAstToNRefactory.IClassToTypeDeclaration.hasKey(iClass))
				return o2MappedAstData.MapAstToNRefactory.IClassToTypeDeclaration[iClass];
			return null;
		}
		
        public static MethodDeclaration methodDeclaration(this O2MappedAstData o2MappedAstData, IMethod iMethod)
        {
        	if (o2MappedAstData.isNull())
        		return null;
            //try to find by Key
            if (iMethod != null)
                if (o2MappedAstData.MapAstToNRefactory.IMethodToMethodDeclaration.hasKey(iMethod))
                    return o2MappedAstData.MapAstToNRefactory.IMethodToMethodDeclaration[iMethod];
            //try to find by Signature
            foreach (var item in o2MappedAstData.MapAstToNRefactory.IMethodToMethodDeclaration)
                if (item.Key.fullName() == iMethod.fullName())
                    return item.Value;
            return null;
        }

        public static MethodDeclaration methodDeclaration_withSignature(this O2MappedAstData astData, string iMethodSignature)
        {
            var iMethod = astData.iMethod_withSignature(iMethodSignature);
            if (iMethod != null)
                return astData.methodDeclaration(iMethod);
            return null;
        }

        public static List<String> files(this O2MappedAstData astData)
        {
            return astData.FileToCompilationUnit.Keys.toList();
        }

        public static string file(this O2MappedAstData o2MappedAstData, CompilationUnit compilationUnit)
        {
        	try
        	{
        		if (o2MappedAstData.FileToCompilationUnit.notNull())
            		foreach (var file in o2MappedAstData.FileToCompilationUnit)
            		    if (file.Value == compilationUnit)
            		        return file.Key;
			}
			catch(Exception ex)
			{
				ex.log("in o2MappedAstData.file(...)");
			}
            return null;
        }

        /*public static string file(this O2MappedAstData o2MappedAstData, IMethod iMethod)
        {
            var methodDeclaration = o2MappedAstData.methodDeclaration(iMethod);
            return o2MappedAstData.file(methodDeclaration);
        }*/
        //replaced with the one below
        public static string file(this O2MappedAstData o2MappedAstData, IMethod iMethod)
        {
            var iNode = o2MappedAstData.methodDeclaration(iMethod) as INode;
            if (iNode == null)
                iNode = o2MappedAstData.constructorDeclaration(iMethod) as INode;
            return o2MappedAstData.file(iNode);
        }

        //not very optimized (see if it is an issue in big data sets
        public static string file(this O2MappedAstData astData, ISpecial iSpecialToMap)
        {
            foreach (var item in astData.FileToSpecials)
                foreach (var iSpecial in item.Value)
                    if (iSpecialToMap == iSpecial)
                        return item.Key;
            return "";
        }

        public static string file(this O2MappedAstData o2MappedAstData, INode iNode)
        {
            if (iNode != null)
            {
                var compilationUnit = iNode.compilationUnit();
                return o2MappedAstData.file(compilationUnit);
            }
            return null;
        }

        #endregion

        #region sourceCode

        public static string sourceCodeWrappedOnClass(this O2MappedAstData o2MappedAstData, IMethod iMethod)
        {
            var code = ("namespace {0}".line() +
                       "{{".line() +
                       "\tpartial class {1}".line() +
                       "\t{{".line() +
                       "\t\t{2}".line() +
                       "\t}}".line() +
                       @"}}".line() +
                //@"\}".line()
                       "")
                       .format(iMethod.DeclaringType.Namespace,
                               iMethod.DeclaringType.Name,
                               o2MappedAstData.sourceCode(iMethod));
            return code;

        }

        public static string sourceCode(this O2MappedAstData o2MappedAstData, IMethod iMethod)
        {
            var methodDeclaration = o2MappedAstData.methodDeclaration(iMethod);
            return o2MappedAstData.sourceCode(methodDeclaration);
        }

        public static string sourceCode(this O2MappedAstData o2MappedAstData, MethodDeclaration methodDeclaration)
        {
            var file = o2MappedAstData.file(methodDeclaration);
            if (file != null)
                return methodDeclaration.sourceCode(file);
            return "";
        }

        

        #endregion
	}		
	
	public static class O2MappedAstData_ExtensionMethods_GUI_WinForms
	{
        #region show in WindowsForms

        public static O2MappedAstData showMethodStreamDetailsInTreeViews(this O2MappedAstData astData, TreeView ParametersTreeView, TreeView MethodsCalledTreeView)
        {
        	if (astData == null)
        		return astData;
            ParametersTreeView.clear();
            MethodsCalledTreeView.clear();

            if (astData.iMethods().size() == 0)
                "AstData_MethodStream.iMethods().size() == 0".debug();
            else
            {
                var iCodeStreamMethod = astData.iMethods()[0];
                iCodeStreamMethod.str().info();

                var methodDeclaration = astData.methodDeclaration(iCodeStreamMethod);

                if (methodDeclaration != null)
                {
                    foreach (var parameter in methodDeclaration.parameters())
                        ParametersTreeView.add_Node("parameter: " + parameter.name(), parameter);

                    foreach (var methodCalled in astData.calledINodesReferences(iCodeStreamMethod))
                        if (methodCalled != null)
                            if (methodCalled is MemberReferenceExpression)
                            {
                                var memberReference = (MemberReferenceExpression)methodCalled;
                                if (memberReference.TargetObject.typeName() == "MemberReferenceExpression")
                                {
                                    MethodsCalledTreeView.add_Node("memberRef: " + memberReference.MemberName, methodCalled);
                                }
                                else
                                {                                	
			                        var iMethodOrProperty = astData.fromMemberReferenceExpressionGetIMethodOrProperty(memberReference);
			                         if (iMethodOrProperty != null)
                        				MethodsCalledTreeView.add_Node(iMethodOrProperty.DotNetName, methodCalled);
                                	else
                                    	MethodsCalledTreeView.add_Node(">" + astData.getTextForINode(memberReference), methodCalled);
                                }
                            }
                    var parentType = methodDeclaration.parent<TypeDeclaration>();
                    if (parentType != null)
                    {
                        foreach (var fieldDeclaration in parentType.iNodes<FieldDeclaration>())
                            foreach(var fieldVariable in fieldDeclaration.Fields)
                                ParametersTreeView.add_Node("field: " + fieldVariable.Name, fieldVariable);

                        foreach (var propertyDeclaration in parentType.iNodes<PropertyDeclaration>())
                            ParametersTreeView.add_Node("property: " + propertyDeclaration.Name, propertyDeclaration);
                    }

                    foreach (var identifierExpression in methodDeclaration.iNodes<IdentifierExpression>())                        
                        ParametersTreeView.add_Node("identifier: " + identifierExpression.Identifier, identifierExpression);                        
                        /*    if (methodCalled is MemberReferenceExpression)
                            {
                                var memberReference = (MemberReferenceExpression)methodCalled;
                                if (memberReference.TargetObject.typeName() == "MemberReferenceExpression")
                                {
                                    MethodsCalledTreeView.add_Node(memberReference.MemberName, methodCalled);
                                }
                                // else
                                //     MethodsCalledTreeView.add_Node(">" + memberReference, methodCalled);
                            }*/
                }
            }
            return astData;
        } 
        #endregion
    }

	public static class O2MappedAstData_ExtensionMethods_MiscData
	{
		public static IMethod firstIMethod(this O2MappedAstData o2MappedAstData)
		{
			var iMethods = o2MappedAstData.iMethods();
			if (iMethods.size() > 0)
				return iMethods[0];
			return null;
		}
		
		public static MethodDeclaration firstMethodDeclaration(this O2MappedAstData o2MappedAstData)
		{
			var iMethod = o2MappedAstData.firstIMethod();                
            return o2MappedAstData.methodDeclaration(iMethod);
		}
		
		public static TypeDeclaration parentTypeDeclaration(this MethodDeclaration methodDeclaration)
		{
			return methodDeclaration.parent<TypeDeclaration>();
		}		
		
		public static TypeDeclaration firstTypeDeclaration(this O2MappedAstData o2MappedAstData)
		{
			return o2MappedAstData.firstMethodDeclaration().parentTypeDeclaration();
		}				
		
		public static List<FieldDeclaration> ast_Fields(this INode iNode)
		{
			if (iNode.isNull())
				return new List<FieldDeclaration>();
			return iNode.iNodes<FieldDeclaration>();			
		}
		
		public static List<FieldDeclaration> ast_Fields(this INode iNode, string fieldType)
		{
			return (from fieldDeclaration in iNode.ast_Fields()
					where fieldDeclaration.ast_Field_Type() == fieldType
					select fieldDeclaration).toList();
		}
		
		public static List<VariableDeclaration> ast_Field_Variables(this List<FieldDeclaration> fieldDeclarations)
		{
			var variables = new List<VariableDeclaration> ();
			foreach(var fieldDeclaration in fieldDeclarations)
				variables.AddRange(fieldDeclaration.ast_Field_Variable());
			return variables;
		}
		
		public static List<VariableDeclaration> ast_Field_Variable(this FieldDeclaration fieldDeclaration)
		{
			return fieldDeclaration.Fields;			
		}
		
		public static List<string> ast_Field_Types(this List<FieldDeclaration> fieldDeclarations)
		{
			var fieldTypes = new List<string> ();
			foreach(var fieldDeclaration in fieldDeclarations)
			{
				var fieldType = fieldDeclaration.ast_Field_Type();
				if (fieldType.valid())
					fieldTypes.Add(fieldType);
			}
			return fieldTypes;
		}
		
		public static string ast_Field_Type(this FieldDeclaration fieldDeclaration)
		{
			if (fieldDeclaration.notNull() && fieldDeclaration.TypeReference.notNull())
				return fieldDeclaration.TypeReference.Type;
			return null;
		}
		
		public static List<PropertyDeclaration> ast_Properties(this INode iNode)
		{
			if (iNode.isNull())
				return new List<PropertyDeclaration>();
			return iNode.iNodes<PropertyDeclaration>();			
		}
		
		public static List<PropertyDeclaration> ast_Properties(this INode iNode, string fieldType)
		{
			return (from propertyDeclaration in iNode.ast_Properties()
					where propertyDeclaration.ast_Property_Type() == fieldType
					select propertyDeclaration).toList();
		}
		
		public static string ast_Property_Type(this PropertyDeclaration propertyDeclaration)
		{
			if (propertyDeclaration.notNull() && propertyDeclaration.TypeReference.notNull())
				return propertyDeclaration.TypeReference.Type;
			return null;
		}
	}

	public static class O2MappedAstData_ExtensionMethods_Load
	{
		#region load

		public static O2MappedAstData get_O2MappedAstData(this string sourceCodeFolder)
		{
			return sourceCodeFolder.get_O2MappedAstData_UsingCache(false);
		}
		
		public static O2MappedAstData get_O2MappedAstData_UsingCache(this string sourceCodeFolder)
		{
			return sourceCodeFolder.get_O2MappedAstData_UsingCache(true);
		}
		
		public static O2MappedAstData get_O2MappedAstData_UsingCache(this string sourceCodeFolder,bool useCache)
		{
			return sourceCodeFolder.get_O2MappedAstData_UsingCache(useCache,"AstData");
		}
		
		public static O2MappedAstData get_O2MappedAstData_UsingCache(this string sourceCodeFolder, bool useCache, string cacheID)
		{
			return sourceCodeFolder.get_O2MappedAstData_UsingCache(cacheID, useCache, true, "*.cs","*.vb");
		}
				
		public static O2MappedAstData get_O2MappedAstData_UsingCache(this string sourceCodeFolder, string cacheID ,bool useCache, bool recursive, params string[] sourceCodeFilters)
		{
			var cacheKey = "{0}  - {1}".format(sourceCodeFolder, cacheID);
			var astData = (O2MappedAstData)O2LiveObjects.get(cacheKey);
			if (useCache.isFalse() || astData.isNull())
			{
			    "loading AstData from: {0}".info(sourceCodeFolder);
			    astData = new O2MappedAstData();
			    astData.loadFiles(sourceCodeFolder.files(recursive, sourceCodeFilters));  
			    O2LiveObjects.set(cacheKey,astData);
			}  
			return astData;
		}
		
        public static O2MappedAstData loadFiles(this O2MappedAstData o2MappedAstData, List<string> filesToLoad)
        {
            return o2MappedAstData.loadFiles(filesToLoad, false);
        }
        
        public static O2MappedAstData loadFiles(this O2MappedAstData o2MappedAstData, List<string> filesToLoad, bool verbose)
        {
            //"Loading {0} files".format().debug();
            var totalFilesCount = filesToLoad.size();
            var filesLoaded = 0;
            foreach (var fileToLoad in filesToLoad)
            {
                if (verbose)
                    "loading file in O2MappedAstData object:{0}".format(fileToLoad).info();
                o2MappedAstData.loadFile(fileToLoad);
                if ((filesLoaded++) % 20 == 0)
                    " [{0}/{1}] Loading files into O2MappedAstData".format(filesLoaded, totalFilesCount).debug();
            }
            return o2MappedAstData;
        }
        
        #endregion 
	
		public static O2MappedAstData getAstData(this string fileOrFolder)
		{
			return fileOrFolder.getAstData(true);
		}
		
		public static O2MappedAstData getAstData(this string fileOrFolder, bool useCachedData)		
		{
			return fileOrFolder.getAstData(new List<string>(), useCachedData);
		}
		
		public static O2MappedAstData getAstData(this string fileOrFolder, List<string> referencesToLoad,  bool useCachedData)
		{
			var astDataCacheKey = "astData_" + fileOrFolder; 
			O2MappedAstData astData = null;
			if (useCachedData)
				astData = (O2MappedAstData)O2LiveObjects.get(astDataCacheKey);			
			if (astData == null)
			{
				"Loading AstData".info();
				astData = new O2MappedAstData();
				foreach(var referenceToLoad in referencesToLoad)
					astData.O2AstResolver.addReference(referenceToLoad); 
				if (fileOrFolder.fileExists())
					astData.loadFile(fileOrFolder);	
				else	
					astData.loadFiles(fileOrFolder.files("*.cs",true));	
				if (useCachedData)
					O2LiveObjects.set(astDataCacheKey,astData); 
			}
			return astData;
		}
	}
}
