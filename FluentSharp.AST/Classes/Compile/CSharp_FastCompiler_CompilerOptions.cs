using System;
using System.Collections.Generic;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;

namespace FluentSharp.AST
{
    public class CSharp_FastCompiler_CompilerOptions
    {
        public string           CompilationVersion              { get; set; }
        public string           default_MethodName              { get; set;}
        public string           default_TypeName                { get; set; }
        public bool				ExecuteInStaThread		        { get; set; }
        public bool				ExecuteInMtaThread		        { get; set; }
        public List<string>     ExtraSourceCodeFilesToCompile   { get; set; }
        public List<string>     Extra_Using_Statements          { get; set; }
        public List<string>     FilesToDownload                 { get; set; }        
        public bool				generateDebugSymbols	        { get; set; }
        public bool             onlyAddReferencedAssemblies     { get; set; }
        public List<string>     NuGet_References                { get; set; }
        public List<string>     ReferencedAssemblies            { get; set; }               
        public bool             ResolveInvocationParametersType { get; set; }        
        public string           SourceCodeFile                  { get; set; }                
        public bool             UseCachedAssemblyIfAvailable    { get; set; }
        public bool				WorkOffline				        { get; set; }

        public CSharp_FastCompiler_CompilerOptions()
        {
            CompilationVersion              = (Environment.Version.Major.eq(4)) ? "v4.0" : "v3.5";                        
            default_MethodName              = "dynamicMethod";
            default_TypeName                = "DynamicType";                                    
            generateDebugSymbols            = false;            
            ExtraSourceCodeFilesToCompile   = new List<string>();
            Extra_Using_Statements          = new List<string>();
            FilesToDownload                 = new List<string>();
            NuGet_References                = new List<string>();
            ReferencedAssemblies            = CompileEngine.DefaultReferencedAssemblies;
            ResolveInvocationParametersType = true;
            UseCachedAssemblyIfAvailable    = true;

        }
    }
}