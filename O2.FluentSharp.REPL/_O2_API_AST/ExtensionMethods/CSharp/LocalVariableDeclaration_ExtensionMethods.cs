using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using O2.DotNetWrappers.ExtensionMethods;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory.Parser;

namespace O2.API.AST.ExtensionMethods.CSharp
{
    public static class LocalVariableDeclaration_ExtensionMethods
    {
    	#region name
    	
    	public static LocalVariableDeclaration variable(this List<LocalVariableDeclaration> localVariableDeclarations, string name)
        {
        	foreach(var localVariableDeclaration in localVariableDeclarations)
        		foreach(var variable in localVariableDeclaration.Variables)
        			if (variable.Name == name)
        				return localVariableDeclaration;
            return null;
        }
        
         public static List<string> names(this List<LocalVariableDeclaration> localVariableDeclarations)
        {
            var names = from localVariableDeclaration in localVariableDeclarations
                          select localVariableDeclaration.name();
            return names.ToList();
        }
        
	    public static LocalVariableDeclaration name(this List<LocalVariableDeclaration> localVariableDeclarations, string name)
        {
            return localVariableDeclarations.variable(name);
        }

        public static string name(this LocalVariableDeclaration localVariableDeclaration)
        {
            if (localVariableDeclaration.Variables.size()==1)
            	return localVariableDeclaration.Variables[0].Name;
            if (localVariableDeclaration.Variables.size() > 1)
            {
            	"In LocalVariableDeclaration.name there where more than one item in the Variables list (returning the first one)".error();
            	return localVariableDeclaration.Variables[0].Name;
            }
            "In LocalVariableDeclaration.name there where NO items in the Variables list (returning null)".error();
            return null;
        }
        
        #endregion
        
        #region type
                        
         public static List<TypeReference> types(this List<LocalVariableDeclaration> localVariableDeclarations)
        {
            var types = from localVariableDeclaration in localVariableDeclarations
                        select localVariableDeclaration.type();
            return types.ToList();
        }        	    

        public static TypeReference type(this LocalVariableDeclaration localVariableDeclaration)
        {
        	return localVariableDeclaration.TypeReference;        	
        }
        
        #endregion
        
        #region initializer
                        
        public static List<Expression> initializers(this List<LocalVariableDeclaration> localVariableDeclarations)
        {
            var initializers = from localVariableDeclaration in localVariableDeclarations
                        select localVariableDeclaration.initializer();
            return initializers.ToList();
        }        	    

        public static Expression initializer(this LocalVariableDeclaration localVariableDeclaration)
        {
        	if (localVariableDeclaration.Variables.size()==1)
            	return localVariableDeclaration.Variables[0].Initializer;
            if (localVariableDeclaration.Variables.size() > 1)
            {
            	"In LocalVariableDeclaration.initializer there where more than one item in the Variables list (returning the first one)".error();
            	return localVariableDeclaration.Variables[0].Initializer;
            }
            "In LocalVariableDeclaration.initializer there where NO items in the Variables list (returning null)".error();
            return null;
        }
        
        #endregion
        
        #region value
                        
        public static List<String> values(this List<LocalVariableDeclaration> localVariableDeclarations)
        {
        	var values = new List<String>();
        	foreach(var localVariableDeclaration in localVariableDeclarations)
        	{
        		var value = localVariableDeclaration.value();
        		if (value!=null)
        			values.Add(value);
        	}
        	return values;
        }        	    

        public static string value(this LocalVariableDeclaration localVariableDeclaration)
        {
        	var initializer = localVariableDeclaration.initializer();
        	if (initializer != null && initializer is PrimitiveExpression)
        		return ((PrimitiveExpression)initializer).StringValue;
        	return null;
        }
        
        #endregion
        
        #region LiteralFormat
                        
        public static List<String> literalFormats(this List<LocalVariableDeclaration> localVariableDeclarations)
        {
        	var literalFormats = new List<String>();
        	foreach(var localVariableDeclaration in localVariableDeclarations)
        	{
        		var literalFormat = localVariableDeclaration.literalFormat();
        		if (literalFormat!=null)
        			literalFormats.Add(literalFormat);
        	}
        	return literalFormats;
        }        	    

        public static String literalFormat(this LocalVariableDeclaration localVariableDeclaration)
        {
        	var initializer = localVariableDeclaration.initializer();
        	if (initializer != null && initializer is PrimitiveExpression)
        		return ((PrimitiveExpression)initializer).LiteralFormat.str();
        	return null;
        }
        
        #endregion
        
    }
}
