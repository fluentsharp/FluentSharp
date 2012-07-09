// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using ICSharpCode.NRefactory.Ast;

namespace O2.API.AST.CSharp
{
    public class O2CodeStream
    {        	            	    	
    	public O2MappedAstData O2MappedAstData { get; set; }
    	public O2CodeStreamTaintRules TaintRules { get; set; }
    	public Dictionary<INode, O2CodeStreamNode> O2CodeStreamNodes { get; set; }
    	public string SourceFile { get; set; }
    	public Stack<INode> INodeStack { get; set; }    	
    	public List<O2CodeStreamNode> StreamNode_First { get; set; }        	
    	    	    	    
		public O2CodeStream(O2MappedAstData o2MappedAstData , O2CodeStreamTaintRules taintRules , string sourceFile )
		{
			 O2MappedAstData = o2MappedAstData;
			 TaintRules = taintRules;
			 O2CodeStreamNodes  = new Dictionary<INode,O2CodeStreamNode>();
			 StreamNode_First = new List<O2CodeStreamNode>();
			 INodeStack = new Stack<INode>();
			 SourceFile = sourceFile;			
		}				
    }
    
    public class O2CodeStreamNode
    {
    	public string Text {get;set;}	
		public INode INode {get; set;}
		public List<O2CodeStreamNode> ChildNodes { get; set;}
		
		public O2CodeStreamNode()
		{
			ChildNodes = new List<O2CodeStreamNode>();
		}
		
    	public O2CodeStreamNode(string text, INode iNode ) : this()
		{
			Text = text;
			INode = iNode;			
		}
		
		public override string ToString()
		{
			return Text;
		}
	}
	
	public class O2CodeStreamTaintRules
	{
		public List<String> TaintPropagators { get; set; }
		
		public O2CodeStreamTaintRules()
		{
			TaintPropagators = new List<String>();
		}
	}
}
