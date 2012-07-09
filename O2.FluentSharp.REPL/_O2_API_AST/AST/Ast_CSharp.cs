// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.CodeDom;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using O2.Interfaces.O2Core;
using O2.Kernel;

using O2.DotNetWrappers.ExtensionMethods;
using O2.DotNetWrappers.DotNet;
using O2.DotNetWrappers.Windows;
using O2.Views.ASCX;

using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;
using System.Collections.Generic;   

namespace O2.External.SharpDevelop.AST
{            
    public class Ast_CSharp
    {    
        private static IO2Log log = PublicDI.log;
    	
        public CompilationUnit CompilationUnit {get; set;}
        public IParser Parser {get; set;}
        public string SourceCode {get ; set;}
        public AstDetails AstDetails {get; set;}
        public string Errors  {get; set;}
        public List<ISpecial> ExtraSpecials { get; set; }
		
        SupportedLanguage language = SupportedLanguage.CSharp;

        public Ast_CSharp()
        {
            ExtraSpecials = new List<ISpecial>();
        }

        public Ast_CSharp(string _sourceCode) 
            : this()
        {    	
            createAst(_sourceCode);
            mapAstDetails(Parser.CompilationUnit);   
        }
        

        public Ast_CSharp(CompilationUnit unit)
            : this(unit, new List<ISpecial>())
        {            
        }

        public Ast_CSharp(CompilationUnit unit, List<ISpecial> extraSpecials)
        {
            createAst("");
            ExtraSpecials = extraSpecials;
            mapAstDetails(unit);
            
            //mapAstDetails(Parser.CompilationUnit);   
        }
    
        public void createAst(string _sourceCode)
        {
            //if (.isFalse())
              //  return;
            SourceCode = (_sourceCode.fileExists()) ? _sourceCode.fileContents() : _sourceCode;
            if (_sourceCode.valid() && _sourceCode.extension(".vb"))
                Parser = ParserFactory.CreateParser(SupportedLanguage.VBNet, new StringReader(SourceCode));
            else
                Parser = ParserFactory.CreateParser(language, new StringReader(SourceCode));
            Parser.Parse();
            Errors = (Parser.Errors.Count > 0) ?  Parser.Errors.ErrorOutput : "";                                     
        }
        
	    public void mapAstDetails(CompilationUnit unit)
	    {
	    	try
	    	{	    		
	        	CompilationUnit = unit;

	            AstDetails = new AstDetails();	            
	            var specials = Parser.Lexer.SpecialTracker.RetrieveSpecials();
                specials.AddRange(ExtraSpecials);
                AstDetails.mapSpecials(specials);
                AstDetails.rewriteCode_CSharp(CompilationUnit, specials);
                AstDetails.rewriteCode_VBNet(CompilationUnit, specials);	            
	
	            CompilationUnit.AcceptVisitor(AstDetails, null);
			}
			catch(Exception ex)
			{
				PublicDI.log.error("in mapAstDetails: {0}", ex.Message);
			}
	    }

        /*public List<String> UsingDeclarations
    	{
    		return astDetails.UsingDeclarations;
    	}
    	
    	public List<String> Types
    	{
    		get
    		{    		
    			return astDetails.Types
    		
    		}
    	}*/


        public void mapAstDetails()
        {
            mapAstDetails(this.CompilationUnit);
        }
    }
}