using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.SharpDevelop.Dom;

using O2.DotNetWrappers.ExtensionMethods;
using O2.API.AST.ExtensionMethods;
using O2.API.AST;

namespace O2.API.AST.CSharp
{

	public class O2MethodStream
	{
		public O2MappedAstData O2MappedAstData { get; set;}		
		public Dictionary<string, IMethod> MappedIMethods {get;set;}
		public Dictionary<string,IMethod> ExternalIMethods {get;set;}
        public Dictionary<string, IReturnType> ExternalClasses { get; set; }
        public Dictionary<string, IField> Fields { get; set; }
        public Dictionary<string, IProperty> Properties { get; set; }

        public List<string> NamespaceReferences { get; set; }
		
		public O2MethodStream(O2MappedAstData o2MappedAstData)
		{
			O2MappedAstData = o2MappedAstData;
			MappedIMethods = new Dictionary<string,IMethod>();
			ExternalIMethods = new Dictionary<string,IMethod>(); 
			ExternalClasses = new Dictionary<string, IReturnType>();
            Fields = new Dictionary<string,IField>();
            Properties = new Dictionary<string, IProperty>();
            NamespaceReferences = new List<String>();
		}
	}
	
    /*public class O2MethodStream
    {
    	public O2MappedAstData O2MappedAstData { get; set;}
        public LinkedList<O2MethodNode> Path { get; set; }
        public List<O2MethodEdge> O2MethodEdges { get; set; }
        public List<O2MethodNode> O2MethodNodes { get; set; }
        public List<INode> INodes { get; set; }
        //public List<O2CodeStreamPath> O2CodeStreamPaths {get;set;}
        public Dictionary<IMethod, O2MethodNode> IMethodToO2MethodNode { get; set; }
        //Dictionary<MethodDeclaration, Path> Mapped_MethodDeclarations  {get;set;}
        
        public O2MethodStream(O2MappedAstData o2MappedAstData)
        {
        	O2MappedAstData = o2MappedAstData;
        	Path = new LinkedList<O2MethodNode>();
        	O2MethodEdges = new List<O2MethodEdge>();
        	O2MethodNodes = new List<O2MethodNode>();
        	INodes = new List<INode>();
        	IMethodToO2MethodNode = new Dictionary<IMethod, O2MethodNode>();
        }
    }

    public class O2MethodEdge
    {
        public O2MethodNode Source { get; set; }
        public MemberReferenceExpression MemberReferenceExpression {get;set;}        
        public O2MethodNode Target { get; set; }
    }

    public class O2MethodNode
    {
        public INode INode {get;set;}        
        public MethodDeclaration MethodDeclaration {get;set;}
        public IMethod IMethod {get;set;}
        public string File {get;set;}
        public string Signature {get;set;}
        public string SourceCode {get;set;}
      
        
        public O2MethodNode(O2MappedAstData o2MappedAstData, IMethod iMethod)
        {        	
        	populateData(o2MappedAstData,iMethod);
        }
      	
      	public O2MethodNode populateData(O2MappedAstData o2MappedAstData, IMethod iMethod)
      	{
      		IMethod = iMethod;        		        	
        	/ *MethodDeclaration = o2MappedAstData.methodDeclaration(iMethod);
        	INode = MethodDeclaration;
        	File = o2MappedAstData.file(iMethod);
        	SourceCode = o2MappedAstData.sourceCode(iMethod);
        	Signature = iMethod.fullName();* /
      		return this;
      	}
        
        public override string ToString()
        {
        	return Signature ?? base.ToString();
        }
        
    }
     * */
}
