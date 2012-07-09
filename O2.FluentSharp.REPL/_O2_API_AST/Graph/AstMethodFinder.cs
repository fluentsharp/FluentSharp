using System;
using System.Collections.Generic;
using ICSharpCode.NRefactory.Ast;
using O2.API.AST.CSharp;

namespace O2.API.AST.Graph
{
    public class AstMethodFinder
    {
        public O2AstResolver O2AstResolver{ get; set; }
        public String MethodToFind { get; set; }
        public List<MethodDeclaration> foundMethods { get; set;}
        public O2AstVisitor O2AstVisitor { get; set; }

        public AstMethodFinder(O2AstResolver o2AstResolver)
        {
            O2AstResolver = o2AstResolver;            
            foundMethods = new List<MethodDeclaration>();
            setupAstVisitor();
        }

        public void setupAstVisitor()
        {
            O2AstVisitor = new O2AstVisitor();
            O2AstVisitor.methodDeclarationVisit =
                (methodDeclaration, data)=> { 
                                                onMethodDeclaration(methodDeclaration);
                                                return data;
                                            }; 
        }


        public void onMethodDeclaration(MethodDeclaration methodDeclaration)
        {
            var mappedMethod = O2AstResolver.getMappedIMethod(methodDeclaration);
            if (mappedMethod != null && mappedMethod.DotNetName == MethodToFind)
                foundMethods.Add(methodDeclaration);            
        }

        public List<MethodDeclaration> find(string methodToFind)
        {
            MethodToFind = methodToFind;
            foreach (var compilationUnit in O2AstResolver.parsedCompilationUnits.Values)
                {
                    //graphAstVisitor.parseInformation.SetCompilationUnit(compilationUnit);                    
                    O2AstResolver.setCurrentCompilationUnit(compilationUnit);
                    compilationUnit.AcceptVisitor(O2AstVisitor, null);
                   // var methods = compilationUnit.methods();
                   // foreach (var method in methods)
                   // { 
                   //      method.
                   // }
                    
                }
            return foundMethods;
        }
    }
}
