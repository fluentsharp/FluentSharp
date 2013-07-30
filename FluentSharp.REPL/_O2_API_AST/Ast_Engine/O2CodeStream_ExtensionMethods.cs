// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using FluentSharp.CoreLib;
using FluentSharp.WinForms;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.SharpDevelop.Dom;

//O2File:Ast_Engine_ExtensionMethods.cs

namespace FluentSharp.CSharpAST.Utils
{
    public static class O2CodeStream_ExtensionMethods
    {    
    	public static int maxLoop = 50000;
    	public static int currentCount =0;
        #region create stream

        public static INode createStream(this O2CodeStream o2CodeStream, INode iNode, O2CodeStreamNode parentStreamNode)
        {        	
        	if (currentCount > maxLoop)
        	{        		
        		"MaxLoop reached: {0}".error(maxLoop);
        		"]]]]]]]]]]]]]] {0} {1}".error(iNode, o2CodeStream.getTextForINode(iNode));        		        		
        		return iNode;
        	}
            return o2CodeStream.createStream(iNode, null, parentStreamNode);
        }

        public static INode createStream(this O2CodeStream o2CodeStream, INode iNode, IdentifierExpression identifier, O2CodeStreamNode parentStreamNode)
        {
            if (iNode != null)
                if (o2CodeStream.stackContains(iNode).isFalse())
                    switch (iNode.typeName())
                    {
                        case "ParameterDeclarationExpression":
                            var parametedNode = o2CodeStream.add_INode(iNode, parentStreamNode);
                            o2CodeStream.expandTaint(iNode as ParameterDeclarationExpression, parametedNode);
                            break;
                        case "IdentifierExpression":
                            var identifierNode = o2CodeStream.add_INode(iNode, parentStreamNode);
                            o2CodeStream.expandTaint(iNode as Expression, identifier, identifierNode);
                            break;
                        case "VariableDeclaration":
                            // handle special case of global vars 
                            if (iNode.Parent is FieldDeclaration && iNode.Parent.Parent is TypeDeclaration)
                            {
                                var fieldDeclaration = iNode.Parent as FieldDeclaration;
                                var fieldNode = o2CodeStream.add_INode(fieldDeclaration, parentStreamNode);
                                var fieldType = (iNode.Parent.Parent as TypeDeclaration);
                                foreach(var fieldUsage in fieldType.iNodes<IdentifierExpression>())
                                    foreach(var fieldName in fieldDeclaration.Fields)
                                        if (fieldUsage.Identifier == fieldName.Name)
                                            o2CodeStream.createStream(fieldUsage, fieldNode);
                            }
                            else                            
                            {
                                var variableNode = o2CodeStream.add_INode(iNode, parentStreamNode);
                                o2CodeStream.expandTaint(iNode as VariableDeclaration, variableNode);
                            }
                            break;
                            
						case "PropertyDeclaration":							
                            // handle special case of global property 
                            if (iNode.Parent is TypeDeclaration)
                            {
                                var propertyDeclaration = iNode as PropertyDeclaration;
                                var propertyNode = o2CodeStream.add_INode(propertyDeclaration, parentStreamNode);
                                var parentType = (iNode.Parent as TypeDeclaration);
                                foreach(var propertyUsage in parentType.iNodes<IdentifierExpression>())
									if (propertyUsage.Identifier == propertyDeclaration.Name)
                                    	o2CodeStream.createStream(propertyUsage, propertyNode);
                            }                            
                            break;
                        /*case "PropertyDeclaration":
                   		var propertyDeclaration = (PropertyDeclaration)iNode;
                   		return taintAllIdentifiers(codeStream, propertyDeclaration, propertyDeclaration.Name, propertyDeclaration.StartLocation);
                   		*/
                        case "ReturnStatement":
                            var returnStatement = o2CodeStream.add_INode(iNode, parentStreamNode);
                            o2CodeStream.expandTaint(iNode as ReturnStatement, returnStatement);
                            break;
                        case "InvocationExpression":
                            var invocationExpressionNode = o2CodeStream.add_INode(iNode, parentStreamNode);
                            o2CodeStream.expandTaint(iNode as Expression, identifier, invocationExpressionNode);
                            break;
                        case "ObjectCreateExpression":
                            var objectCreateExpression = o2CodeStream.add_INode(iNode, parentStreamNode);
                            o2CodeStream.expandTaint(iNode as Expression, identifier, objectCreateExpression);
                            break;
                        case "ForeachStatement":
                            var foreachStatement = (ForeachStatement)iNode;
                            var variableName = foreachStatement.VariableName;
                            foreach (var iExpression in iNode.iNodes<IdentifierExpression>())                            
                                if (iExpression.Identifier == variableName)
                                {
                                    o2CodeStream.createStream(iExpression, parentStreamNode);        
                                }                            
                            break;
                        case "ForStatement":
                            var forStatement = (ForStatement)iNode;                            
                            foreach (var initializer in forStatement.Initializers)
                            {
                                if (initializer is LocalVariableDeclaration)
                                {
                                    var parentNode = o2CodeStream.peekStack();
                                    foreach (var forVariableDeclaration in (initializer as LocalVariableDeclaration).Variables)
                                        o2CodeStream.createStream(forVariableDeclaration, parentNode);
                                }
                            }
                            break;
                        case "MemberReferenceExpression":
                            var memberReferenceExpression = (MemberReferenceExpression)iNode;                            
                            o2CodeStream.createStream(iNode.Parent, identifier, parentStreamNode);                            // handle the case were we got here via an invocation to this method

                            //this has quite a number of side effects
                            if (memberReferenceExpression.TargetObject != identifier && 
                                (memberReferenceExpression.TargetObject is MemberReferenceExpression).isFalse())  // to avoid circular loops                            
                                if (memberReferenceExpression.Parent is AssignmentExpression)
                                {
                                    o2CodeStream.createStream(memberReferenceExpression.TargetObject, null, o2CodeStream.peekStack()); // handle the case were we got here via an assignment
                                    if (memberReferenceExpression.TargetObject is IdentifierExpression)
                                    {
                                        var identifierExpression = (IdentifierExpression)memberReferenceExpression.TargetObject;
                                        taintAllIdentifiersInParentMethod(o2CodeStream, identifierExpression, identifierExpression.Identifier, identifierExpression.StartLocation);
                                    }
                                }
                                
                            break;
                        case "IndexerExpression":                            
                            var indexerExpression = (IndexerExpression)iNode;
                            parentStreamNode = o2CodeStream.add_INode(iNode, parentStreamNode);
                            o2CodeStream.createStream(indexerExpression.TargetObject,identifier,  parentStreamNode);
							break;
                        default:
                            if (o2CodeStream.debugMode())
                                "Unsupported stream Node type (tying its parent):{0}".error(iNode.typeName());
                            o2CodeStream.createStream(iNode.Parent, identifier, parentStreamNode);
                            // get an iNode that we have not added (getParentINodeThatIsNotAdded will first check the provided iNode)
                            /*iNode = o2CodeStream.getParentINodeThatIsNotAdded(iNode);
                            if (iNode != null)
                            {
                                parentStreamNode = o2CodeStream.add_INode(iNode, parentStreamNode);
                                // get its parent
                                var parentToTaint = iNode.Parent; //o2CodeStream.getParentINodeThatIsNotAdded(iNode.Parent);
                                // create stream for Parent
                                o2CodeStream.createStream(parentToTaint, identifier, parentStreamNode);
                            }*/
                            break;
                    }
            return iNode;
        }

        public static O2CodeStream createO2CodeStream(this O2MappedAstData astData, string methodStreamFile, INode iNode)
        {            
            return astData.createO2CodeStream(null, methodStreamFile, iNode);
        }

        public static O2CodeStream createO2CodeStream(this O2MappedAstData astData, O2CodeStreamTaintRules taintRules, string methodStreamFile, INode iNode)
        {
            if (iNode == null)
                return null;
            if (taintRules.isNull())
            	taintRules = new O2CodeStreamTaintRules();
            	            
            var codeStream = new O2CodeStream(astData, taintRules, methodStreamFile);
                        
            //O2CodeStreamNode parentNode = null;
            // start with the first node and keep going up (via its parent) until we find a iNode type that is currently handled
            var originalINode = iNode;
            IdentifierExpression identifier = null;            
            
            while (iNode != null)
            {
                //"> iNode:{0}           :      {1}".debug(iNode.typeName(), iNode);
                switch (iNode.typeName())
                {
                    // these trigger the createStream process
                    case "ParameterDeclarationExpression":
                        var parameterDeclaration = (ParameterDeclarationExpression)iNode;
                        "Creating Stream for ParameterDeclarationExpression Declaration: {0}".info(parameterDeclaration.ParameterName);
                        // start with the host method
                        if (parameterDeclaration.Parent is MethodDeclaration)                        
                            codeStream.add_INode(parameterDeclaration.Parent, codeStream.peekStack());                        
                        
                        codeStream.createStream(iNode, codeStream.peekStack());
                        return codeStream;
                        
                    case "VariableDeclaration":                                               
                        var variableDeclaration = (VariableDeclaration)iNode;
                        "Creating Stream for VariableDeclaration: {0}".info(variableDeclaration.Name);
                        codeStream.createStream(variableDeclaration, codeStream.peekStack());
                        return codeStream;
                        
                    case "PropertyDeclaration":    
						var propertyDeclaration = (PropertyDeclaration)iNode;
                        "Creating Stream for PropertyDeclaration: {0}".info(propertyDeclaration.Name);
                        codeStream.createStream(propertyDeclaration, codeStream.peekStack()); 
                        return codeStream;
                        
                    case "InvocationExpression":
                        var invocationExpression = (InvocationExpression)iNode;
                        "Creating Stream for InvocationExpression: {0}".info(invocationExpression.str());                        
                        codeStream.createStream(invocationExpression, identifier, codeStream.peekStack());

                        //check if the current invocationExpression is a taint propagator
                        if (codeStream.TaintRules.isTaintPropagator(invocationExpression))
                            break;
                        return codeStream;                        

                    // these create an INode but let the upwards iNode.Parent path to continue
                    case "MemberReferenceExpression":
                        var memberReferenceExpression = (MemberReferenceExpression)iNode;
                        "Adding reference to MemberReferenceExpression: {0}".info(memberReferenceExpression.MemberName);
                        codeStream.add_INode(memberReferenceExpression, codeStream.peekStack());
                        if (memberReferenceExpression.TargetObject is IdentifierExpression)
                            identifier = (IdentifierExpression)memberReferenceExpression.TargetObject;
                        break;
                    
                    case "IdentifierExpression":                        
                        identifier = iNode as IdentifierExpression;
                        //codeStream.add_INode(identifier, codeStream.peekStack());
                        codeStream.createStream(identifier, codeStream.peekStack());
                        break;

                    case "ForeachStatement":
                    case "ForStatement":
                        codeStream.createStream(iNode, codeStream.peekStack());
                        
                        //var variableName = forStatement.I
                        //var identifiers = iNode.iNodes<IdentifierExpression>();

                        return codeStream;
                    case "MethodDeclaration":
                    case "TypeDeclaration":      
                    	if (codeStream.debugMode())
                        	"**** in createO2CodeStream, not supported INode type:{0}".error(iNode.typeName());
                        return codeStream;

                    case "BlockStatement":
                    case "IfElseStatement":
                    case "TryCatchStatement":
                        break;

                    default:
                        codeStream.add_INode(iNode, codeStream.peekStack());
                        break; 
                }
                // figure out where to go next
                switch (iNode.typeName())
                {

                    case "IndexerExpression":
                        if (codeStream.has_INode(iNode))
                            iNode = iNode.Parent;
                        else
                        {
                            var indexerExpression = (IndexerExpression)iNode;
                            //"Adding reference to IndexerExpression.TargetObject: {0}".info(indexerExpression.Tar.MemberName);
                            codeStream.add_INode(indexerExpression, codeStream.peekStack());
                            iNode = indexerExpression.TargetObject;
                        }
                        break;

                    case "IdentifierExpression":                                                            
                    	// this will taint all IdentifierExpression in the current method
                        var identifierINode = (IdentifierExpression)iNode;
                        return taintAllIdentifiersInParentMethod(codeStream, identifierINode, identifierINode.Identifier, identifierINode.StartLocation);                        					
                    	
                    case "AssignmentExpression":
                        var assignmentExpression = (AssignmentExpression)iNode;
                        if (codeStream.has_INode(assignmentExpression.Left).isFalse())  // check if we have already added this expresion
                            iNode = assignmentExpression.Left;
                        else
                            iNode = iNode.Parent;
                        break;
                        
                    default:
                        iNode = iNode.Parent;
                        break;
                }                
            }
            return codeStream;
        }
			
		public static O2CodeStream taintAllIdentifiersInParentMethod(O2CodeStream codeStream, INode iNode, string nameToMatch, ICSharpCode.NRefactory.Location startLocation)
		{
			var methodDeclaration = iNode.parent<MethodDeclaration>();
            var parentStream = codeStream.peekStack();
            if (methodDeclaration.notNull())
                foreach (var taintedIdentifier in methodDeclaration.iNodes<IdentifierExpression>())
                    if (taintedIdentifier.Identifier == nameToMatch)
                        if (taintedIdentifier.StartLocation > startLocation)    // so that we don't taint backwards                                        
                            codeStream.createStream(taintedIdentifier, parentStream);
            return codeStream;
        }

        public static void createUniqueStreamPath(this List<List<O2CodeStreamNode>> uniqueStreamPaths, int maxDepth, List<O2CodeStreamNode> streamPath, O2CodeStreamNode streamNode)
        {
            if (streamNode != null)
            {
                streamPath.add(streamNode);
                if (streamPath.size() >= maxDepth)
                {
                    "in createUniqueStreamPath, maxDepth reached: {0}".error(maxDepth);
                    uniqueStreamPaths.Add(streamPath);
                }
                else if (streamNode.ChildNodes.size() == 0)
                    uniqueStreamPaths.Add(streamPath);
                else
                {
                    var streamPaths = new Dictionary<List<O2CodeStreamNode>, O2CodeStreamNode>();
                    for (int i = 0; i < streamNode.ChildNodes.size(); i++)
                    {
                        var childStream = streamNode.ChildNodes[i];
                        if (childStream != streamNode)
                        {
                            if (i > 0)								//create a copy of the current stream for branches																				
                                streamPaths.Add(new List<O2CodeStreamNode>(streamPath), childStream);
                            else
                                streamPaths.Add(streamPath, childStream);
                        }
                        else
                            "in createUniqueStreamPath: childStream = streamNode.ChildNodes[{0}]".error(i);
                    }
                    foreach (var item in streamPaths)                    
                        if (uniqueStreamPaths.size() < maxDepth)
                            uniqueStreamPaths.createUniqueStreamPath(maxDepth, item.Key, item.Value);                    
                }
            }
        }

        public static List<O2CodeStreamNode> streamNodes(this O2CodeStream o2CodeStream)
        {
            return o2CodeStream.O2CodeStreamNodes.Values.ToList();
        }

        #endregion 

        #region expand taint

        public static O2CodeStream expandTaint(this O2CodeStream o2CodeStream, ReturnStatement returnStatement, O2CodeStreamNode parentStreamNode)
        {
            //"Tainting the return data".error();
            //var lastMethodDeclaration = o2CodeStream.popStack<MethodDeclaration>();
            
            var lastMethodDeclaration = o2CodeStream.peekStack<MethodDeclaration>();
            
            if (lastMethodDeclaration != default(MethodDeclaration))
            {
                // option to add this method as a tain propagator
                /*
                // and add to TaintRules as taint propagator
                var lastMethodDeclaration_IMethod = o2CodeStream.O2MappedAstData.iMethod(lastMethodDeclaration);
                o2CodeStream.TaintRules.add_TaintPropagator(lastMethodDeclaration_IMethod.fullName());
                o2CodeStream.TaintRules.add_TaintPropagator(lastMethodDeclaration_IMethod.DotNetName);*/
                //"lastMethodDeclaration: {0}".debug(lastMethodDeclaration);    		

                // find who calls this
                //var iNode = o2CodeStream.popStack();
                var iNode = o2CodeStream.peekStack(lastMethodDeclaration);

                o2CodeStream.expandTaint_of_ParentINode(iNode, parentStreamNode);

                /*var iNodeToTaint = o2CodeStream.getParentINodeThatIsNotAdded(iNode);
                if (iNodeToTaint != null)
                {
                    if (o2CodeStream.debugMode())
                    {
                        "FOUND iNodeToTaint:{0}".info(iNodeToTaint);
                        "FOUND iNodeToTaint.Parent:{0}".info(iNodeToTaint.Parent);
                    }
                    
                    //var invocationExpression = iNodeToTaint.parent<InvocationExpression>();
                    //if (invocationExpression != null)
                    //    o2CodeStream.createStream(invocationExpression, parentStreamNode);
                    //else
                    //{
 
                    //}
                    //o2CodeStream.createStream(iNodeToTaint.Parent, parentStreamNode);
                    o2CodeStream.createStream(iNodeToTaint.Parent, parentStreamNode);
                }*/                
            }
            return o2CodeStream;
        }

        public static O2CodeStream expandTaint(this O2CodeStream o2CodeStream, ParameterDeclarationExpression parameter, O2CodeStreamNode parentStreamNode)
        {
            if (parameter.Parent is MethodDeclaration)
            {
                var parameterName = parameter.name();
                var methodDeclaration = parameter.Parent as MethodDeclaration;

                //var currentStackINode = o2CodeStream.peekStack();       // after each o2CodeStream.createStream below, we need to restore the stack to this position
                foreach (var identifier in methodDeclaration.iNodes<IdentifierExpression>())
                    if (identifier.Identifier == parameterName)
                    {
                        o2CodeStream.createStream(identifier, parentStreamNode);    // create new stream from here
                        o2CodeStream.popStack(identifier);                   // restore stack                        
                    }
                
            }
            return o2CodeStream;
        }

        public static O2CodeStream expandTaint(this O2CodeStream o2CodeStream, VariableDeclaration variable, O2CodeStreamNode parentStreamNode)
        {
            if (variable == null)
                return o2CodeStream;
            if (o2CodeStream.debugMode())
                "expandTaint for VariableDeclaration:{0}".info(variable.Name);
            var parentMethod = variable.parent<MethodDeclaration>();
            if (parentMethod != null)
            {
                foreach (var identifier in parentMethod.iNodes<IdentifierExpression>())
                    if (identifier.Identifier == variable.Name)
                        o2CodeStream.createStream(identifier, parentStreamNode);
                //"identifier: {0}".error(identifier.Identifier);
                //"got method".error();
            }
            else
                if (o2CodeStream.debugMode())
                    "in VariableDeclaration.expandTaint could not find parent method for provided variable: {0}".error(variable.Name);

            return o2CodeStream;
        }

        public static O2CodeStream expandTaint(this O2CodeStream o2CodeStream, Expression expression, IdentifierExpression identifier, O2CodeStreamNode parentStreamNode)
        {
            if (expression == null)
            {
                if (o2CodeStream.debugMode())
                    "in expression.expandTaint, expression was NULL".error();
                return o2CodeStream;
            }
            if (o2CodeStream.debugMode())
                "MAPPING FOR: {0}   parent : {1}   identifier: {2}".info(
                    expression.typeName(),
                    expression.Parent.typeName(),
                    (identifier != null) ? identifier.Identifier : "[NULL]");


            switch (expression.typeName())
            {
                case "IdentifierExpression":

                    var identifierExpression = expression as IdentifierExpression; 		

                    var parent = expression.Parent;
                    if (parent == expression)		// just in case so that we don't have a non-ending recursive loop
                        return o2CodeStream;

                    switch (parent.typeName())
                    {
                        case "IndexerExpression":
                            taintAllIdentifiersInParentMethod(o2CodeStream, identifierExpression, identifierExpression.Identifier, identifierExpression.StartLocation);
                            break;
                        case "InvocationExpression":
                        case "BinaryOperatorExpression":
                        case "ObjectCreateExpression":
                        case "CollectionInitializerExpression":
                        case "ArrayCreateExpression":                        
                        case "ParenthesizedExpression":
                        case "CastExpression":
                            o2CodeStream.expandTaint(parent as Expression, identifierExpression, parentStreamNode);
                            break;
                        case "ReturnStatement":
                        case "VariableDeclaration":
                            o2CodeStream.createStream(parent, parentStreamNode);
                            break;
                        case "ForStatement":
                        case "ForeachStatement":
                            o2CodeStream.createStream(parent,parentStreamNode);
                            break;
                        case "MemberReferenceExpression":
                            o2CodeStream.createStream(parent, identifierExpression, parentStreamNode);
                            break;
                        case "AssignmentExpression":
                            var assignmentExpression = (AssignmentExpression)parent;
                            if (o2CodeStream.has_INode(assignmentExpression.Left).isFalse())  // check if we have already added this expresion
                                o2CodeStream.createStream(assignmentExpression.Left, null, parentStreamNode);
                            break;
                                //iNode = assignmentExpression.Left;
                            //else
                              //  iNode = iNode.Parent;
                        default:
                            //if (o2CodeStream.debugMode())
                                "in Expression.IdentifierExpression.expandTaint unsupported INode parent type: {0}".error(parent.typeName());
                            break;
                    }
                   

                    break;                
                case "InvocationExpression":
                    var invocationExpression = (InvocationExpression)expression;
                    var argumentPosition = invocationExpression.argumentPosition(identifier);
                    if (o2CodeStream.debugMode())
                        "argument Position:{0}".info(argumentPosition);

                    var calledIMethod = o2CodeStream.O2MappedAstData.iMethod(invocationExpression);

                    if (calledIMethod != null)
                    {
                        // check for taint propagators
                        if (o2CodeStream.TaintRules.isTaintPropagator(calledIMethod.DotNetName))
                        {
                            if (o2CodeStream.debugMode())
                                "Handling Taint Propagator:{0}".info(calledIMethod.DotNetName);
                            parentStreamNode = o2CodeStream.add_INode(invocationExpression, parentStreamNode);
                            o2CodeStream.expandTaint_of_ParentINode(invocationExpression, parentStreamNode);
                        }
                        else
                        {
                            var methodDeclaration = o2CodeStream.O2MappedAstData.methodDeclaration(calledIMethod);
                            if (methodDeclaration != null)
                            {
                                if (argumentPosition > -1 && methodDeclaration.parameters().Count > argumentPosition)
                                {
                                    var invocationNode = o2CodeStream.add_INode(methodDeclaration, parentStreamNode);
                                    o2CodeStream.createStream(methodDeclaration.parameters()[argumentPosition], invocationNode);
                                }
                                else
                                {
                                    if (o2CodeStream.debugMode())
                                        "in IdentifierExpression.InvocationExpression.expandTaint, methodDeclaration.parameters().Count > parameterPosition (adding the method as iNode)".error();
                                    var currentStreamNode = o2CodeStream.add_INode(methodDeclaration, parentStreamNode);

                                    //handleAutoTaintOfAutoGeneratedMethods
                                    o2CodeStream.handleAutoTaintOfAutoGeneratedMethods(methodDeclaration.csharpCode(), invocationExpression, currentStreamNode);
                                }
                            }
                            else
                                o2CodeStream.add_INode(expression, parentStreamNode);
                        }
                    }
                    else
                        o2CodeStream.add_INode(invocationExpression, parentStreamNode);
                    break;

                case "ObjectCreateExpression":
                    var objectCreateExpression = (ObjectCreateExpression)expression;
                    var parameterPosition = objectCreateExpression.parameterPosition(identifier);
                    if (o2CodeStream.debugMode())
                        "parameter Position:{0}".info(parameterPosition);
                    var ctorIMethod = o2CodeStream.O2MappedAstData.iMethod(objectCreateExpression);
                    if (ctorIMethod != null)
                    {
                        // check for taint propagators
                        if (o2CodeStream.TaintRules.isTaintPropagator(ctorIMethod.DotNetName))
                        {
                            if (o2CodeStream.debugMode())
                                "Handling Taint Propagator:{0}".info(ctorIMethod.DotNetName);
                            parentStreamNode = o2CodeStream.add_INode(objectCreateExpression, parentStreamNode);
                            o2CodeStream.expandTaint_of_ParentINode(objectCreateExpression, parentStreamNode);
                        }
                        else
                        {
                            var constructorDeclaration = o2CodeStream.O2MappedAstData.constructorDeclaration(ctorIMethod);
                            if (constructorDeclaration != null)
                            {
                                if (parameterPosition > -1 && constructorDeclaration.parameters().Count > parameterPosition)
                                    o2CodeStream.createStream(constructorDeclaration.parameters()[parameterPosition], parentStreamNode);
                                else
                                {
                                    if (o2CodeStream.debugMode())
                                        "in IdentifierExpression.ObjectCreateExpression.expandTaint, constructorDeclaration.parameters().Count > parameterPosition (adding the constructor as iNode)".error();
                                    var currentStreamNode = o2CodeStream.add_INode(constructorDeclaration, parentStreamNode);
                                    //handleAutoTaintOfAutoGeneratedMethods
                                    o2CodeStream.handleAutoTaintOfAutoGeneratedMethods(constructorDeclaration.csharpCode(), objectCreateExpression, currentStreamNode);
                                }
                            }
                        }
                    }
                    break;

                case "BinaryOperatorExpression":
                case "CollectionInitializerExpression":
                case "ArrayCreateExpression":
                case "IndexerExpression":
                case "ParenthesizedExpression":
                case "CastExpression":
                    if (expression.Parent is Expression)
                        o2CodeStream.expandTaint(expression.Parent as Expression, identifier, parentStreamNode);
                    else
                        o2CodeStream.createStream(expression.Parent, parentStreamNode);
                    break;

                default:
                    if (o2CodeStream.debugMode())
                        "in Expression.expandTaint unsupported INode type: {0}".error(expression.typeName());
                    break;
            }
            return o2CodeStream;
        }

        public static O2CodeStream expandTaint(this O2CodeStream o2CodeStream, O2CodeStreamNode streamNode, string identifierWithTaint, O2CodeStreamNode parentStreamNode)
        {
            if (o2CodeStream.debugMode())
            {
                "identifierWithTaint:{0}".info(identifierWithTaint);
                "type {0}".info(streamNode.INode.typeName());
                "size {0}".info(streamNode.INode.iNodes().size());
            }
            foreach (var iNode in streamNode.INode.iNodes())
                if (iNode != null && iNode is IdentifierExpression)
                    if (((IdentifierExpression)iNode).Identifier == identifierWithTaint)
                        o2CodeStream.add_INode(iNode, parentStreamNode);
            return o2CodeStream;
        }


        public static O2CodeStream expandTaint_of_ParentINode(this O2CodeStream o2CodeStream, INode iNode, O2CodeStreamNode parentStreamNode)
        {
            // find a taint target
            while (iNode != null)
            {
                //if (o2CodeStream.has_INode(iNode).isFalse())

                if (o2CodeStream.stackContains(iNode).isFalse())
                {
                    switch (iNode.typeName())
                    {
                        case "VariableDeclaration":
                        case "InvocationDeclaration":
                        case "ObjectCreateExpression":
                        case "ReturnStatement":                        
                            o2CodeStream.createStream(iNode, parentStreamNode);
                            return o2CodeStream;
                        case "AssignmentExpression":
                            var assignmentExpression = (AssignmentExpression)iNode;                            
                            o2CodeStream.createStream(assignmentExpression.Left, parentStreamNode);
                            return o2CodeStream;
                    }
                }
                //"INODE TO Taint: {0}".info(iNode);
                iNode = iNode.Parent;
            }
            return o2CodeStream;
        }

        public static void handleAutoTaintOfAutoGeneratedMethods(this O2CodeStream o2CodeStream, string methodSourceCode, Expression expression, O2CodeStreamNode parentStreamNode)
        {        	        	
            if (methodSourceCode.Contains("throw new System.Exception(\"O2 Auto Generated Method\")") ||
                methodSourceCode.Contains("throw new NotImplementedException();") ||
                methodSourceCode.Contains("{".line() + "}") )
            {   
            //	"Expanding Taing of Parent node: {0}".debug(methodSourceCode);
                o2CodeStream.expandTaint_of_ParentINode(expression,  parentStreamNode);
            }
        }
                                    

        #endregion
            	    	

        #region add

        public static bool has_INode(this O2CodeStream o2CodeStream, INode iNode)
    	{
    		return o2CodeStream.O2CodeStreamNodes.ContainsKey(iNode);
    	}
    	
    	public static O2CodeStreamNode add_INode(this O2CodeStream o2CodeStream, INode iNode, O2CodeStreamNode parentStreamNode)
    	{            
    		//if (o2CodeStream.has_INode(iNode).isFalse())
            if (o2CodeStream.stackContains(iNode).isFalse())
    		{
                if (o2CodeStream.debugMode())
    			    "adding INode: {0}".info(iNode.typeName());    			
    			o2CodeStream.INodeStack.Push(iNode);
    			var text = o2CodeStream.getTextForINode(iNode);;
    			    			
    			var streamNode = new O2CodeStreamNode(text,iNode);

				o2CodeStream.O2CodeStreamNodes.add(iNode, streamNode);
				if (parentStreamNode == null)
					o2CodeStream.StreamNode_First.Add(streamNode);
				if (parentStreamNode!= null)
					parentStreamNode.ChildNodes.Add(streamNode);				
				return streamNode;
    		}
    		else
    			if(o2CodeStream.debugMode())
    				"o2CodeStream contained INode:".debug();
            
    		var existingStreamNode = o2CodeStream.O2CodeStreamNodes[iNode];
            if (o2CodeStream.stackContains(iNode).isFalse())            // to avoid circular references                    
                if (parentStreamNode != null && parentStreamNode != existingStreamNode)
//                 if (existingStreamNode.ChildNodes.Contains(parentStreamNode).isFalse()) 
                        parentStreamNode.ChildNodes.Add(existingStreamNode);					
                else
                	if(o2CodeStream.debugMode())
	                	"parentStreamNode == null || parentStreamNode == existingStreamNode".error();
    		return existingStreamNode;
        }

        #endregion

        #region map values

        public static O2CodeStreamNode map_IMethod(this O2CodeStream o2CodeStream, IMethod iMethod)
        {
            var methodDeclaration = o2CodeStream.O2MappedAstData.methodDeclaration(iMethod);
            return o2CodeStream.map_MethodDeclaration(methodDeclaration);
        }

        public static O2CodeStreamNode map_MethodDeclaration(this O2CodeStream o2CodeStream, MethodDeclaration methodDeclaration)
        {
            var methodNode = o2CodeStream.add_INode(methodDeclaration, null);
            foreach (var parameter in methodDeclaration.parameters())
                o2CodeStream.createStream(parameter, methodNode);
            return methodNode;
        }

        #endregion

        #region get

        public static string getTextForINode(this O2CodeStream o2CodeStream, INode iNode)
        {
        	return o2CodeStream.O2MappedAstData.getTextForINode(iNode);
        }
        
        public static string getTextForINode(this O2MappedAstData o2MappedAstData, INode iNode)
    	{
	    	var typeName = iNode.typeName();
	    	var text = typeName;
            try
            {
                switch (typeName)
                {
                    case "ParameterDeclarationExpression":
                        text = "parameter -> {0}".format((iNode as ParameterDeclarationExpression).name());
                        break;
                    case "MethodDeclaration":
                        var iMethod = o2MappedAstData.iMethod(iNode as MethodDeclaration);
                        text = "method -> {0}".format(iMethod.fullName());
                        // o2CodeStream.O2MappedAstData.iMethod(iNode as MethodDeclaration).name());
                        break;
                    case "InvocationExpression":
                        var invocationExpression = iNode as InvocationExpression;
                        var invocationMethod = o2MappedAstData.iMethod(invocationExpression);                                               

                        if (invocationMethod != null)
                            text = "invocation -> {0}".format(invocationMethod.fullName());
                        else if (invocationExpression.TargetObject is MemberReferenceExpression)
                        {
                            var memberReferenceExpressionTarget = (MemberReferenceExpression)invocationExpression.TargetObject;
                            var iMethodOrIProperty = o2MappedAstData.fromMemberReferenceExpressionGetIMethodOrProperty(memberReferenceExpressionTarget);
                            if (iMethodOrIProperty != null)
                                text = "invocation -> {0}".format(iMethodOrIProperty.fullName());
                            else
                                text = "[not resolved: invocation] {0}".format(memberReferenceExpressionTarget.MemberName);
                        }
                        else
                            text = "invocation: COULD NOT FIND TARGET";

                        //text = "invocation: {0}".format(invocationExpression.TargetObject);
                        //elseelse

                        break;
                    case "IdentifierExpression":
                        var identifier = (iNode as IdentifierExpression);
                        var resolved = o2MappedAstData.resolveExpression(identifier);
                        var identifierType = (resolved.ResolvedType != null) ? resolved.ResolvedType.FullyQualifiedName : "[type not resolved]";
                        text = "identifier  -> {0} : {1}".format(identifier.Identifier, identifierType);
                        break;
                    case "VariableDeclaration":
                        text = "variable  -> {0}".format((iNode as VariableDeclaration).Name);
                        break;
                    case "PropertyDeclaration":
                        text = "property  -> {0}".format((iNode as PropertyDeclaration).Name);
                        break;                            
                    case "ConstructorDeclaration":
                        var ctorIMethod = o2MappedAstData.iMethod(iNode as ConstructorDeclaration);
                        text = "constructor -> {0}".format(ctorIMethod.fullName());// (iNode as ConstructorDeclaration).Name);
                        break;
                    case "MemberReferenceExpression":
                        var memberReferenceExpression = (MemberReferenceExpression)iNode;
                        var iMethodOrProperty = o2MappedAstData.fromMemberReferenceExpressionGetIMethodOrProperty(memberReferenceExpression);
                        if (iMethodOrProperty != null)
                        {
                            var signature = iMethodOrProperty.fullName();
                            text = "memberRef -> {0}: {1}".format(iMethodOrProperty.typeName(), signature);
                        }
                        else
                        {
                        	var memberName = ".{0}".format(memberReferenceExpression.MemberName); 
                        	var memberIdentifier = "";
                        	if (memberReferenceExpression.TargetObject is IdentifierExpression)                        	
                        		memberIdentifier = "{0}".format((memberReferenceExpression.TargetObject as IdentifierExpression).Identifier);
                            var memberReferenceResolved = o2MappedAstData.resolveExpression(memberReferenceExpression.TargetObject);
                            if (memberReferenceResolved.notNull() && memberReferenceResolved.ResolvedType.notNull() && memberReferenceResolved.ResolvedType.FullyQualifiedName.valid())
                            {                            		
                                text = "{0}{1} : {2}".format(memberReferenceResolved.ResolvedType.FullyQualifiedName, memberName, memberIdentifier);
							}
                            else
                                text = "[not resolved: target] {0} : {1}".format(memberName, memberIdentifier);
                        }
                        break;

                    case "ObjectCreateExpression":
                        var objectCreateExpression = (ObjectCreateExpression)iNode;
                        var objectCreateMapping = o2MappedAstData.fromExpressionGetIMethodOrProperty(iNode as Expression);
                        if (objectCreateMapping.notNull())
                        
                       	text = "constructor -> {0}".format(objectCreateMapping.fullName());// objectCreateExpression.CreateType.str());
                       
                        //var objectCreateSignature = (objectCreateMapping.notNull())
                        			//					? objectCreateMapping.fullName()
                        					//			: ;
                        else                        
                        	text = "[not resolved: objectCreate] {0}".format(objectCreateExpression.CreateType.str());
                        break;

                    case "FieldDeclaration":
                        var fieldDeclaration = (FieldDeclaration)iNode;
                        var variableName = "";
                        foreach (var field in fieldDeclaration.Fields)
                            variableName += field.Name + " ";
                        text = "field -> {0} : {1}".format(variableName.trim(), fieldDeclaration.TypeReference.Type);
                        break;
                    default:
                    	if (o2MappedAstData.debugMode)
	                        "in O2CodeStream.getTextForNode: not supported INode type: {0}".error(text);
                        break;

                }
            }
            catch (Exception ex)
            {
                //ex.log("in getTextForINode",true);
                //ex.StackTrace.info();
                text += "     (error calculating text: {0})".format(ex.Message);
            }
			return text;
        }

        public static INode getParentINodeThatIsNotAdded(this O2CodeStream o2CodeStream, INode iNode)
        {
            while (iNode != null && iNode.Parent != null)
            {
                if (o2CodeStream.has_INode(iNode).isFalse())
                    if ((iNode is BinaryOperatorExpression).isFalse())          // returning to a BinaryOperatorExpression was creating a recursive loop
                        return iNode;
                iNode = iNode.Parent;
            }
            return null;
        }

        public static List<List<O2CodeStreamNode>> getUniqueStreamPaths(this O2CodeStream o2CodeStream, int maxDepth)
        {
            var uniqueStreamPaths = new List<List<O2CodeStreamNode>>();
            if (o2CodeStream.notNull())
            	foreach (var streamNode in o2CodeStream.StreamNode_First)
                	uniqueStreamPaths.createUniqueStreamPath(maxDepth, new List<O2CodeStreamNode>(), streamNode);
            return uniqueStreamPaths;
        }

        public static List<INode> iNodes(this O2CodeStream o2CodeStream)
        {
            return o2CodeStream.O2CodeStreamNodes.Keys.ToList();
        }

        #endregion

        #region INode stack

        public static INode popStack(this O2CodeStream o2CodeStream)
        {
            if (o2CodeStream.INodeStack.Count > 0)
                return o2CodeStream.INodeStack.Pop();
            return null;
        }

        public static T popStack<T>(this O2CodeStream o2CodeStream)
            where T : INode
        {
            while (o2CodeStream.INodeStack.Count > 0)
            {
                var iNode = o2CodeStream.INodeStack.Pop();
                if (iNode is T)
                    return (T)iNode;
            }
            return default(T);
        }

        public static INode popStack(this O2CodeStream o2CodeStream, INode iNodeToPopTo)            
        {
            while (o2CodeStream.INodeStack.Count > 0)
            {
                var iNode = o2CodeStream.INodeStack.Pop();
                if (iNode == iNodeToPopTo)
                    return iNode;
            }
            return default(INode);
        }

        public static O2CodeStreamNode peekStack(this O2CodeStream codeStream)
        {
            if (codeStream != null && codeStream.INodeStack != null)
                if (codeStream.INodeStack.Count > 0)
                {
                    var peekINode = codeStream.INodeStack.Peek();
                    if (peekINode != null)
                        if (codeStream.O2CodeStreamNodes.ContainsKey(peekINode))
                            return codeStream.O2CodeStreamNodes[peekINode];
                }
            return null;
        }

        public static T peekStack<T>(this O2CodeStream o2CodeStream)
            where T : INode
        {
        	var stackNodes = o2CodeStream.INodeStack.ToArray();
            for(var i=0 ; i< stackNodes.Length ; i ++)
            {
                var iNode = stackNodes[i];
                if (iNode is T)
                    return (T)iNode;
            }
            return default(T);
        }

        public static INode peekStack(this O2CodeStream o2CodeStream, INode iNodeToFind)
            //where T : INode
        {
        	var stackNodes = o2CodeStream.INodeStack.ToArray();
            for (var i = 0; i < stackNodes.Length -1; i++)
            {
                var iNode = stackNodes[i];
                if (iNode == iNodeToFind)
                    return stackNodes[i+1];
            }
            return default(INode);
        }

        public static bool stackContains(this O2CodeStream o2CodeStream, INode iNodeToFind)
        {        	
        	return o2CodeStream.INodeStack.ToArray().Contains(iNodeToFind);
        	/*var stackNodes = o2CodeStream.INodeStack.ToArray();
        	if(stackNodes.Contains(iNodeToFind))
        		return true;
        	if (iNodeToFind is MethodDeclaration)
        	{        	
        		"************* PPPPP earching for MethodDeclaration".error();
        		foreach(var stackNode in stackNodes)
        		{
        			if(stackNode == iNodeToFind)
        				"MATCH on ==".error();
        			if (stackNode.str() == iNodeToFind.str())
        				"Match on str: {0}".error(stackNode.str());
        			if(stackNode is MethodDeclaration)
        			{
        				var stackNode_Signature = o2CodeStream.O2MappedAstData.iMethod(stackNode as MethodDeclaration);
        				var iNodeToFind_Signature = o2CodeStream.O2MappedAstData.iMethod(iNodeToFind as MethodDeclaration);
        				"{0} == {1}".info(stackNode_Signature, iNodeToFind_Signature);
        				if (stackNode_Signature == iNodeToFind_Signature)
        					"Match on Signature".error();        				
        			}
        			else
        				"stackNode is not MethodDeclaration is it: {0}".debug(stackNode.typeName());
        		}	
        		
        	}
        	return false;*/            
            //o2CodeStream.O2MappedAstData.iMethod(nodeStack as MethodDeclaration)
        }

        #endregion            	  	    	    	 

        #region misc

        public static bool debugMode(this O2CodeStream o2CodeStream)
        {
        	if (o2CodeStream.O2MappedAstData.notNull())
	            return o2CodeStream.O2MappedAstData.debugMode;
	        return false;
        }

        public static bool hasPaths(this O2CodeStream o2CodeStream)
        {
            return (o2CodeStream != null && o2CodeStream.O2CodeStreamNodes.size() > 0);
        }

        #endregion

        #region show Windows Forms

        public static O2CodeStream show(this O2CodeStream o2CodeStream, TreeView treeView)
        {
            if (o2CodeStream != null)
            {
                treeView.clear();
                // show all INodes
                var allINodes = treeView.add_Node("All INodes");
                foreach (var node in o2CodeStream.streamNodes())
                    allINodes.add_Node(node.Text, node);
                //allINodes.expand();

                // show tree (starting on parent)
                treeView.removeEventHandlers_BeforeExpand();

                var parentNodes = treeView.add_Node("Parent(s)");
                foreach (var streamNode in o2CodeStream.StreamNode_First)
                    parentNodes.add_Node(streamNode.Text,
                                         streamNode,
                                         streamNode.ChildNodes.size() > 0);

                treeView.beforeExpand<O2CodeStreamNode>(
                    (streamNode) =>
                    {
                        var currentNode = treeView.current();
                        currentNode.clear();
                        foreach (var childStreamNode in streamNode.ChildNodes)
                            currentNode.add_Node(childStreamNode.Text, childStreamNode, childStreamNode.ChildNodes.size() > 0);
                        //currentNode.add_Node("There are {0} childNodes".format(streamNode.ChildNodes.size()));
                    });


                // show unique paths

                var uniqueStreamPaths = o2CodeStream.getUniqueStreamPaths(100);

                var uniquePaths = treeView.add_Node("UniquePaths");
                for (int i = 0; i < uniqueStreamPaths.size(); i++)
                {
                    var uniquePath = uniquePaths.add_Node("path #" + (i + 1));
                    foreach (var streamNode in uniqueStreamPaths[i])
                        uniquePath.add_Node(streamNode.str(), streamNode);
                }
                uniquePaths.expand();


                //treeView.expandAll();
                treeView.selectFirst();
            }
            return o2CodeStream;
        }

        #endregion

    }
}
