// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Daniel Grunwald" email="daniel@danielgrunwald.de"/>
//     <version>$Revision: 4955 $</version>
// </file>
//DC this is a variation of the code that was included in Sharpdevelop
//ICSharpCode.NRefactory.Visitors.NRefactoryASTConvertVisitor.cs file
//I started making so many changes to it that it was better to move it to a separate file so
// that I don't break Sharpdevelop's internal functionality
// Moving it to this namespace will also allow the use of a number of O2 helper classes


// created on 04.08.2003 at 17:49
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;

using ICSharpCode.NRefactory.Visitors;
using NRefactoryAST = ICSharpCode.NRefactory.Ast;
using RefParser = ICSharpCode.NRefactory;
using ICSharpCode.SharpDevelop.Dom;
using ICSharpCode.SharpDevelop.Dom.NRefactoryResolver;


using O2.DotNetWrappers.ExtensionMethods;
using O2.API.AST.ExtensionMethods;
using O2.API.AST.ExtensionMethods.CSharp;

namespace O2.API.AST.Visitors
{
	public class MapAstToNRefactory : AbstractAstVisitor
    {
        //public Properties with data collected
        public Dictionary<NRefactoryAST.CompilationUnit, ICompilationUnit> CompilationUnitToICompilationUnit { get; set; }        
        public Dictionary<NRefactoryAST.TypeDeclaration, IClass> TypeDeclarationToIClass { get; set; }
        public Dictionary<IClass, NRefactoryAST.TypeDeclaration> IClassToTypeDeclaration { get; set; }        
        public Dictionary<NRefactoryAST.MethodDeclaration, IMethod> MethodDeclarationToIMethod { get; set; }
        public Dictionary<IMethod, NRefactoryAST.MethodDeclaration> IMethodToMethodDeclaration { get; set; }
        public Dictionary<NRefactoryAST.ConstructorDeclaration, IMethod> ConstructorDeclarationToIMethod { get; set; } 
        public Dictionary<IMethod, NRefactoryAST.ConstructorDeclaration> IMethodToConstructorDeclaration  { get; set; }
        public Dictionary<IProperty, NRefactoryAST.PropertyDeclaration> IPropertyToPropertyDeclaration { get; set; }
        public Dictionary<IField, NRefactoryAST.FieldDeclaration> IFieldToFieldDeclaration { get; set; } 

        //internal variables
        IProjectContent defaultProjectContent;
		DefaultCompilationUnit cu;
		DefaultUsingScope currentNamespace;
		Stack<DefaultClass> currentClass = new Stack<DefaultClass>();
		public string VBRootNamespace { get; set; }
		
		public ICompilationUnit Cu {
			get {
				return cu;
			}
		}

        private MapAstToNRefactory()
        {                    
            CompilationUnitToICompilationUnit = new Dictionary<NRefactoryAST.CompilationUnit, ICompilationUnit>();            
            TypeDeclarationToIClass = new Dictionary<NRefactoryAST.TypeDeclaration, IClass>();
            IClassToTypeDeclaration = new Dictionary<IClass, NRefactoryAST.TypeDeclaration>();
            MethodDeclarationToIMethod = new Dictionary<NRefactoryAST.MethodDeclaration, IMethod>();
            IMethodToMethodDeclaration = new Dictionary<IMethod, NRefactoryAST.MethodDeclaration>();
            ConstructorDeclarationToIMethod = new Dictionary<NRefactoryAST.ConstructorDeclaration, IMethod>();
            IMethodToConstructorDeclaration = new Dictionary<IMethod, NRefactoryAST.ConstructorDeclaration>();
            IPropertyToPropertyDeclaration = new Dictionary<IProperty, NRefactoryAST.PropertyDeclaration>();
            IFieldToFieldDeclaration = new Dictionary<IField, NRefactoryAST.FieldDeclaration>();
            
        }
        
        public MapAstToNRefactory(IProjectContent projectContent): this()
		{
            defaultProjectContent = projectContent;
		}
		
        // DC

        //DC
        /*public void loadCode(string code)
        {
            code = (code.fileExists()) ? code.fileContents() : code;
            var parser = code.csharpAst();
            loadCompilationUnit(parser.CompilationUnit);
        }*/
        //DC
        public void loadCompilationUnit(NRefactoryAST.CompilationUnit compilationUnit)
        {
            cu = new DefaultCompilationUnit(defaultProjectContent);            
            compilationUnit.AcceptVisitor(this, null);
        }

        //DC
        public void mapCompilationUnit(NRefactoryAST.CompilationUnit compilationUnit, ICompilationUnit iCompilationUnit)
        {
            CompilationUnitToICompilationUnit.Add(compilationUnit, iCompilationUnit);
        }
        //DC

        public void mapConstructor(NRefactoryAST.ConstructorDeclaration constructorDeclaration, IMethod iMethod)
        {
            ConstructorDeclarationToIMethod.Add(constructorDeclaration, iMethod);
            IMethodToConstructorDeclaration.Add(iMethod, constructorDeclaration);
        }

        //DC
        public void mapMethod(NRefactoryAST.MethodDeclaration methodDeclaration, IMethod iMethod)
        {
            MethodDeclarationToIMethod.Add(methodDeclaration, iMethod);
            IMethodToMethodDeclaration.Add(iMethod, methodDeclaration);
        }
        //DC
        public void mapType(NRefactoryAST.TypeDeclaration typeDeclaration, IClass iClass)
        {
            TypeDeclarationToIClass.Add(typeDeclaration, iClass);
            IClassToTypeDeclaration.Add(iClass, typeDeclaration);
        }                

        public void mapProperty(NRefactoryAST.PropertyDeclaration propertyDeclaration, IProperty iProperty)
        {
            IPropertyToPropertyDeclaration.add(iProperty, propertyDeclaration);            
        }

        public void mapField(NRefactoryAST.FieldDeclaration fieldDeclaration, IField iField)
        {
            IFieldToFieldDeclaration.add(iField, fieldDeclaration);
        }

		DefaultClass GetCurrentClass()
		{
			return currentClass.Count == 0 ? null : currentClass.Peek();
		}
		
		ModifierEnum ConvertModifier(NRefactoryAST.Modifiers m)
		{
			if (this.IsVisualBasic)
				return ConvertModifier(m, ModifierEnum.Public);
			else if (currentClass.Count > 0 && currentClass.Peek().ClassType == ClassType.Interface)
				return ConvertModifier(m, ModifierEnum.Public);
			else
				return ConvertModifier(m, ModifierEnum.Private);
		}
		
		ModifierEnum ConvertTypeModifier(NRefactoryAST.Modifiers m)
		{
			if (this.IsVisualBasic)
				return ConvertModifier(m, ModifierEnum.Public);
			if (currentClass.Count > 0)
				return ConvertModifier(m, ModifierEnum.Private);
			else
				return ConvertModifier(m, ModifierEnum.Internal);
		}
		
		ModifierEnum ConvertModifier(NRefactoryAST.Modifiers m, ModifierEnum defaultVisibility)
		{            
			ModifierEnum r = (ModifierEnum)m;
			if ((r & ModifierEnum.VisibilityMask) == ModifierEnum.None)
				return r | defaultVisibility;
			else
				return r;
		}
		
		List<RefParser.ISpecial> specials;
		
		/// <summary>
		/// Gets/Sets the list of specials used to read the documentation.
		/// The list must be sorted by the start position of the specials!
		/// </summary>
		public List<RefParser.ISpecial> Specials {
			get {
				return specials;
			}
			set {
				specials = value;
			}
		}
		
		string GetDocumentation(int line, IList<NRefactoryAST.AttributeSection> attributes)
		{
			foreach (NRefactoryAST.AttributeSection att in attributes) {
				if (att.StartLocation.Y > 0 && att.StartLocation.Y < line)
					line = att.StartLocation.Y;
			}
			List<string> lines = new List<string>();
			int length = 0;
			while (line > 0) {
				line--;
				string doku = null;
				bool foundPreprocessing = false;
				var specialsOnLine = GetSpecialsFromLine(line);
				foreach (RefParser.ISpecial special in specialsOnLine) {
					RefParser.Comment comment = special as RefParser.Comment;
					if (comment != null && comment.CommentType == RefParser.CommentType.Documentation) {
						doku = comment.CommentText;
						break;
					} else if (special is RefParser.PreprocessingDirective) {
						foundPreprocessing = true;
					}
				}
				if (doku == null && !foundPreprocessing)
					break;
				if (doku != null) {
					length += 2 + doku.Length;
					lines.Add(doku);
				}
			}
			StringBuilder b = new StringBuilder(length);
			for (int i = lines.Count - 1; i >= 0; --i) {
				b.AppendLine(lines[i]);
			}
			return b.ToString();
		}
		
		string GetDocumentationFromLine(int line)
		{
			foreach (RefParser.ISpecial special in GetSpecialsFromLine(line)) {
				RefParser.Comment comment = special as RefParser.Comment;
				if (comment != null && comment.CommentType == RefParser.CommentType.Documentation) {
					return comment.CommentText;
				}
			}
			return null;
		}
		
		IEnumerable<RefParser.ISpecial> GetSpecialsFromLine(int line)
		{
			List<RefParser.ISpecial> result = new List<RefParser.ISpecial>();
			if (specials == null) return result;
			if (line < 0) return result;
			// specials is a sorted list: use interpolation search
			int left = 0;
			int right = specials.Count - 1;
			int m;
			
			while (left <= right) {
				int leftLine  = specials[left].StartPosition.Y;
				if (line < leftLine)
					break;
				int rightLine = specials[right].StartPosition.Y;
				if (line > rightLine)
					break;
				if (leftLine == rightLine) {
					if (leftLine == line)
						m = left;
					else
						break;
				} else {
					m = (int)(left + Math.BigMul((line - leftLine), (right - left)) / (rightLine - leftLine));
				}
				
				int mLine = specials[m].StartPosition.Y;
				if (mLine < line) { // found line smaller than line we are looking for
					left = m + 1;
				} else if (mLine > line) {
					right = m - 1;
				} else {
					// correct line found,
					// look for first special in that line
					while (--m >= 0 && specials[m].StartPosition.Y == line);
					// look at all specials in that line: find doku-comment
					while (++m < specials.Count && specials[m].StartPosition.Y == line) {
						result.Add(specials[m]);
					}
					break;
				}
			}
			return result;
		}
		
		public override object VisitCompilationUnit(NRefactoryAST.CompilationUnit compilationUnit, object data)
		{
			if (compilationUnit == null) {
				return null;
			}
            mapCompilationUnit(compilationUnit, cu);
			currentNamespace = new DefaultUsingScope();
			if (!string.IsNullOrEmpty(VBRootNamespace)) {
				foreach (string name in VBRootNamespace.Split('.')) 
				{
					currentNamespace = new DefaultUsingScope();
					currentNamespace.Parent = currentNamespace;
					currentNamespace.NamespaceName = PrependCurrentNamespace(name);
					currentNamespace.Parent.ChildScopes.Add(currentNamespace);
				}
			}
			cu.UsingScope = currentNamespace;
			compilationUnit.AcceptChildren(this, data);
			return cu;
		}
		
		public override object VisitUsingDeclaration(NRefactoryAST.UsingDeclaration usingDeclaration, object data)
		{
			DefaultUsing us = new DefaultUsing(cu.ProjectContent, GetRegion(usingDeclaration.StartLocation, usingDeclaration.EndLocation));
			foreach (NRefactoryAST.Using u in usingDeclaration.Usings) {
				u.AcceptVisitor(this, us);
			}
			currentNamespace.Usings.Add(us);
			return data;
		}
		
		public override object VisitUsing(NRefactoryAST.Using u, object data)
		{
			Debug.Assert(data is DefaultUsing);
			DefaultUsing us = (DefaultUsing)data;
			if (u.IsAlias) {
				IReturnType rt = CreateReturnType(u.Alias);
				if (rt != null) {
					us.AddAlias(u.Name, rt);
				}
			} else {
				us.Usings.Add(u.Name);
			}
			return data;
		}
		
		void ConvertAttributes(NRefactoryAST.AttributedNode from, AbstractEntity to)
		{
			if (from.Attributes.Count == 0) {
				to.Attributes = DefaultAttribute.EmptyAttributeList;
			} else {
				ICSharpCode.NRefactory.Location location = from.Attributes[0].StartLocation;
				ClassFinder context;
				if (to is IClass) {
					context = new ClassFinder((IClass)to, location.Line, location.Column);
				} else {
					context = new ClassFinder(to.DeclaringType, location.Line, location.Column);
				}
				to.Attributes = VisitAttributes(from.Attributes, context);
			}
		}
		
		List<IAttribute> VisitAttributes(IList<NRefactoryAST.AttributeSection> attributes, ClassFinder context)
		{
			// TODO Expressions???
			List<IAttribute> result = new List<IAttribute>();
			foreach (NRefactoryAST.AttributeSection section in attributes) {
				
				AttributeTarget target = AttributeTarget.None;
				if (section.AttributeTarget != null && section.AttributeTarget != "") {
					switch (section.AttributeTarget.ToUpperInvariant()) {
						case "ASSEMBLY":
							target = AttributeTarget.Assembly;
							break;
						case "FIELD":
							target = AttributeTarget.Field;
							break;
						case "EVENT":
							target = AttributeTarget.Event;
							break;
						case "METHOD":
							target = AttributeTarget.Method;
							break;
						case "MODULE":
							target = AttributeTarget.Module;
							break;
						case "PARAM":
							target = AttributeTarget.Param;
							break;
						case "PROPERTY":
							target = AttributeTarget.Property;
							break;
						case "RETURN":
							target = AttributeTarget.Return;
							break;
						case "TYPE":
							target = AttributeTarget.Type;
							break;
						default:
							target = AttributeTarget.None;
							break;
							
					}
				}
				
				foreach (NRefactoryAST.Attribute attribute in section.Attributes) 
				{
					List<object> positionalArguments = new List<object>();
					foreach (NRefactoryAST.Expression positionalArgument in attribute.PositionalArguments) 
					{
						positionalArguments.Add(ConvertAttributeArgument(positionalArgument));
					}
					Dictionary<string, object> namedArguments = new Dictionary<string, object>();
					foreach (NRefactoryAST.NamedArgumentExpression namedArgumentExpression in attribute.NamedArguments) 
					{
						namedArguments.Add(namedArgumentExpression.Name, ConvertAttributeArgument(namedArgumentExpression.Expression));
					}
					var defaultAttribue = new DefaultAttribute(new AttributeReturnType(context, attribute.Name),
					                                		target, positionalArguments, namedArguments);
					defaultAttribue.CompilationUnit = cu;
					defaultAttribue.Region = GetRegion(attribute.StartLocation, attribute.EndLocation);
					result.Add(defaultAttribue);
				}
			}
			return result;
		}
		
		static object ConvertAttributeArgument(NRefactoryAST.Expression expression)
		{
			NRefactoryAST.PrimitiveExpression pe = expression as NRefactoryAST.PrimitiveExpression;
			if (pe != null)
				return pe.Value;
			else
				return null;
		}
		
		public override object VisitAttributeSection(NRefactoryAST.AttributeSection attributeSection, object data)
		{
			if (GetCurrentClass() == null) {
				ClassFinder cf = new ClassFinder(new DefaultClass(cu, "DummyClass"), attributeSection.StartLocation.Line, attributeSection.StartLocation.Column);
				cu.Attributes.AddRange(VisitAttributes(new[] { attributeSection }, cf));
			}
			return null;
		}
		
		string PrependCurrentNamespace(string name)
		{
			if (string.IsNullOrEmpty(currentNamespace.NamespaceName))
				return name;
			else
				return currentNamespace.NamespaceName + "." + name;
		}
		
		public override object VisitNamespaceDeclaration(NRefactoryAST.NamespaceDeclaration namespaceDeclaration, object data)
		{
			DefaultUsingScope oldNamespace = currentNamespace;
			foreach (string name in namespaceDeclaration.Name.Split('.')) 
			{
				currentNamespace = new DefaultUsingScope();
				currentNamespace.Parent = currentNamespace;
				currentNamespace.NamespaceName = PrependCurrentNamespace(name);				
				currentNamespace.Parent.ChildScopes.Add(currentNamespace);
			}
			object ret = namespaceDeclaration.AcceptChildren(this, data);
			currentNamespace = oldNamespace;
			return ret;
		}
		
		ClassType TranslateClassType(NRefactoryAST.ClassType type)
		{
			switch (type) {
				case NRefactoryAST.ClassType.Enum:
					return ClassType.Enum;
				case NRefactoryAST.ClassType.Interface:
					return ClassType.Interface;
				case NRefactoryAST.ClassType.Struct:
					return ClassType.Struct;
				case NRefactoryAST.ClassType.Module:
					return ClassType.Module;
				default:
					return ClassType.Class;
			}
		}
		
		static DomRegion GetRegion(RefParser.Location start, RefParser.Location end)
		{
			return DomRegion.FromLocation(start, end);
		}
		
		public override object VisitTypeDeclaration(NRefactoryAST.TypeDeclaration typeDeclaration, object data)
		{
			DomRegion region = GetRegion(typeDeclaration.StartLocation, typeDeclaration.EndLocation);
			DomRegion bodyRegion = GetRegion(typeDeclaration.BodyStartLocation, typeDeclaration.EndLocation);
			
			DefaultClass c = new DefaultClass(cu, TranslateClassType(typeDeclaration.Type), ConvertTypeModifier(typeDeclaration.Modifier), region, GetCurrentClass());
			if (c.IsStatic) {
				// static classes are also abstract and sealed at the same time
				c.Modifiers |= ModifierEnum.Abstract | ModifierEnum.Sealed;
			}
			c.BodyRegion = bodyRegion;
			ConvertAttributes(typeDeclaration, c);
			c.Documentation = GetDocumentation(region.BeginLine, typeDeclaration.Attributes);
			
			DefaultClass outerClass = GetCurrentClass();
			if (outerClass != null) {
				outerClass.InnerClasses.Add(c);
				c.FullyQualifiedName = outerClass.FullyQualifiedName + '.' + typeDeclaration.Name;
			} else {
				c.FullyQualifiedName = PrependCurrentNamespace(typeDeclaration.Name);
				cu.Classes.Add(c);
			}
			c.UsingScope = currentNamespace;
			currentClass.Push(c);
			
			ConvertTemplates(outerClass, typeDeclaration.Templates, c); // resolve constrains in context of the class
			// templates must be converted before base types because base types may refer to generic types
			
			if (c.ClassType != ClassType.Enum && typeDeclaration.BaseTypes != null) {
				foreach (NRefactoryAST.TypeReference type in typeDeclaration.BaseTypes) {
					IReturnType rt = CreateReturnType(type, null, TypeVisitor.ReturnTypeOptions.BaseTypeReference);
					if (rt != null) {
						c.BaseTypes.Add(rt);
					}
				}
			}
			
			object ret = typeDeclaration.AcceptChildren(this, data);
			currentClass.Pop();
			
			if (c.ClassType == ClassType.Module) {
				foreach (DefaultField f in c.Fields) {
					f.Modifiers |= ModifierEnum.Static;
				}
				foreach (DefaultMethod m in c.Methods) {
					m.Modifiers |= ModifierEnum.Static;
				}
				foreach (DefaultProperty p in c.Properties) {
					p.Modifiers |= ModifierEnum.Static;
				}
				foreach (DefaultEvent e in c.Events) {
					e.Modifiers |= ModifierEnum.Static;
				}
			}

            mapType(typeDeclaration, c);

			return ret;
		}
		
		void ConvertTemplates(DefaultClass outerClass, IList<NRefactoryAST.TemplateDefinition> templateList, DefaultClass c)
		{
			int outerClassTypeParameterCount = outerClass != null ? outerClass.TypeParameters.Count : 0;
			if (templateList.Count == 0 && outerClassTypeParameterCount == 0) {
				c.TypeParameters = DefaultTypeParameter.EmptyTypeParameterList;
			} else {
				Debug.Assert(c.TypeParameters.Count == 0);
				
				int index = 0;
				if (outerClassTypeParameterCount > 0) {
					foreach (DefaultTypeParameter outerTypeParamter in outerClass.TypeParameters) {
						DefaultTypeParameter p = new DefaultTypeParameter(c, outerTypeParamter.Name, index++);
						p.HasConstructableConstraint = outerTypeParamter.HasConstructableConstraint;
						p.HasReferenceTypeConstraint = outerTypeParamter.HasReferenceTypeConstraint;
						p.HasValueTypeConstraint = outerTypeParamter.HasValueTypeConstraint;
						p.Attributes.AddRange(outerTypeParamter.Attributes);
						p.Constraints.AddRange(outerTypeParamter.Constraints);
						c.TypeParameters.Add(p);
					}
				}
				
				foreach (NRefactoryAST.TemplateDefinition template in templateList) {
					c.TypeParameters.Add(new DefaultTypeParameter(c, template.Name, index++));
				}
				// converting the constraints requires that the type parameters are already present
				for (int i = 0; i < templateList.Count; i++) {
					ConvertConstraints(templateList[i], (DefaultTypeParameter)c.TypeParameters[i + outerClassTypeParameterCount]);
				}
			}
		}
		
		void ConvertTemplates(List<NRefactoryAST.TemplateDefinition> templateList, DefaultMethod m)
		{
			int index = 0;
			if (templateList.Count == 0) {
				m.TypeParameters = DefaultTypeParameter.EmptyTypeParameterList;
			} else {
				Debug.Assert(m.TypeParameters.Count == 0);
				foreach (NRefactoryAST.TemplateDefinition template in templateList) {
					m.TypeParameters.Add(new DefaultTypeParameter(m, template.Name, index++));
				}
				// converting the constraints requires that the type parameters are already present
				for (int i = 0; i < templateList.Count; i++) {
					ConvertConstraints(templateList[i], (DefaultTypeParameter)m.TypeParameters[i]);
				}
			}
		}
		
		void ConvertConstraints(NRefactoryAST.TemplateDefinition template, DefaultTypeParameter typeParameter)
		{
			foreach (NRefactoryAST.TypeReference typeRef in template.Bases) {
				if (typeRef == NRefactoryAST.TypeReference.NewConstraint) {
					typeParameter.HasConstructableConstraint = true;
				} else if (typeRef == NRefactoryAST.TypeReference.ClassConstraint) {
					typeParameter.HasReferenceTypeConstraint = true;
				} else if (typeRef == NRefactoryAST.TypeReference.StructConstraint) {
					typeParameter.HasValueTypeConstraint = true;
				} else {
					IReturnType rt = CreateReturnType(typeRef, typeParameter.Method, TypeVisitor.ReturnTypeOptions.None);
					if (rt != null) {
						typeParameter.Constraints.Add(rt);
					}
				}
			}
		}
		
		public override object VisitDelegateDeclaration(NRefactoryAST.DelegateDeclaration delegateDeclaration, object data)
		{
			DomRegion region = GetRegion(delegateDeclaration.StartLocation, delegateDeclaration.EndLocation);
			DefaultClass c = new DefaultClass(cu, ClassType.Delegate, ConvertTypeModifier(delegateDeclaration.Modifier), region, GetCurrentClass());
			c.Documentation = GetDocumentation(region.BeginLine, delegateDeclaration.Attributes);
			ConvertAttributes(delegateDeclaration, c);
			CreateDelegate(c, delegateDeclaration.Name, delegateDeclaration.ReturnType,
			               delegateDeclaration.Templates, delegateDeclaration.Parameters);
			return c;
		}
		
		void CreateDelegate(DefaultClass c, string name, NRefactoryAST.TypeReference returnType, IList<NRefactoryAST.TemplateDefinition> templates, IList<NRefactoryAST.ParameterDeclarationExpression> parameters)
		{
			c.BaseTypes.Add(c.ProjectContent.SystemTypes.MulticastDelegate);
			DefaultClass outerClass = GetCurrentClass();
			if (outerClass != null) {
				outerClass.InnerClasses.Add(c);
				c.FullyQualifiedName = outerClass.FullyQualifiedName + '.' + name;
			} else {
				c.FullyQualifiedName = PrependCurrentNamespace(name);
				cu.Classes.Add(c);
			}
			c.UsingScope = currentNamespace;
			currentClass.Push(c); // necessary for CreateReturnType
			ConvertTemplates(outerClass, templates, c);
			
			List<IParameter> p = new List<IParameter>();
			if (parameters != null) {
				foreach (NRefactoryAST.ParameterDeclarationExpression param in parameters) {
					p.Add(CreateParameter(param));
				}
			}
			AnonymousMethodReturnType.AddDefaultDelegateMethod(c, CreateReturnType(returnType), p);
			
			currentClass.Pop();
		}
		
		IParameter CreateParameter(NRefactoryAST.ParameterDeclarationExpression par)
		{
			return CreateParameter(par, null);
		}
		
		IParameter CreateParameter(NRefactoryAST.ParameterDeclarationExpression par, IMethod method)
		{
			return CreateParameter(par, method, GetCurrentClass(), cu);
		}
		
		internal static IParameter CreateParameter(NRefactoryAST.ParameterDeclarationExpression par, IMethod method, IClass currentClass, ICompilationUnit cu)
		{
			IReturnType parType = CreateReturnType(par.TypeReference, method, currentClass, cu, TypeVisitor.ReturnTypeOptions.None);
			DefaultParameter p = new DefaultParameter(par.ParameterName, parType, GetRegion(par.StartLocation, par.EndLocation));
			p.Modifiers = (ParameterModifiers)par.ParamModifier;
			return p;
		}
		
		public override object VisitMethodDeclaration(NRefactoryAST.MethodDeclaration methodDeclaration, object data)
		{
			DomRegion region     = GetRegion(methodDeclaration.StartLocation, methodDeclaration.EndLocation);
			DomRegion bodyRegion = GetRegion(methodDeclaration.EndLocation, methodDeclaration.Body != null ? methodDeclaration.Body.EndLocation : RefParser.Location.Empty);
			DefaultClass currentClass = GetCurrentClass();
			
			DefaultMethod method = new DefaultMethod(methodDeclaration.Name, null, ConvertModifier(methodDeclaration.Modifier), region, bodyRegion, currentClass);
			method.IsExtensionMethod = methodDeclaration.IsExtensionMethod;
			method.Documentation = GetDocumentation(region.BeginLine, methodDeclaration.Attributes);
			ConvertTemplates(methodDeclaration.Templates, method);
			method.ReturnType = CreateReturnType(methodDeclaration.TypeReference, method, TypeVisitor.ReturnTypeOptions.None);
			ConvertAttributes(methodDeclaration, method);
			if (methodDeclaration.Parameters.Count > 0) {
				foreach (NRefactoryAST.ParameterDeclarationExpression par in methodDeclaration.Parameters) {
					method.Parameters.Add(CreateParameter(par, method));
				}
			} else {
				method.Parameters = DefaultParameter.EmptyParameterList;
			}
			if (methodDeclaration.HandlesClause.Count > 0) {
				foreach (string handlesClause in methodDeclaration.HandlesClause) {
					if (handlesClause.ToLowerInvariant().StartsWith("me."))
						method.HandlesClauses.Add(handlesClause.Substring(3));
					else if (handlesClause.ToLowerInvariant().StartsWith("mybase."))
						method.HandlesClauses.Add(handlesClause.Substring(7));
					else
						method.HandlesClauses.Add(handlesClause);
				}
			} else {
				method.HandlesClauses = EmptyList<string>.Instance;
			}
			
			currentClass.Methods.Add(method);
            mapMethod(methodDeclaration, method);
			return null;
		}
		
		public override object VisitDeclareDeclaration(NRefactoryAST.DeclareDeclaration declareDeclaration, object data)
		{
			DefaultClass currentClass = GetCurrentClass();
			
			DomRegion region = GetRegion(declareDeclaration.StartLocation, declareDeclaration.EndLocation);
			DefaultMethod method = new DefaultMethod(declareDeclaration.Name, null, ConvertModifier(declareDeclaration.Modifier), region, DomRegion.Empty, currentClass);
			method.Documentation = GetDocumentation(region.BeginLine, declareDeclaration.Attributes);
			method.Modifiers |= ModifierEnum.Extern | ModifierEnum.Static;
			
			method.ReturnType = CreateReturnType(declareDeclaration.TypeReference, method, TypeVisitor.ReturnTypeOptions.None);
			ConvertAttributes(declareDeclaration, method);
			
			foreach (NRefactoryAST.ParameterDeclarationExpression par in declareDeclaration.Parameters) {
				method.Parameters.Add(CreateParameter(par, method));
			}
			
			currentClass.Methods.Add(method);
			return null;
		}
		
		public override object VisitOperatorDeclaration(NRefactoryAST.OperatorDeclaration operatorDeclaration, object data)
		{
			DefaultClass c  = GetCurrentClass();
			DomRegion region     = GetRegion(operatorDeclaration.StartLocation, operatorDeclaration.EndLocation);
			DomRegion bodyRegion = GetRegion(operatorDeclaration.EndLocation, operatorDeclaration.Body != null ? operatorDeclaration.Body.EndLocation : RefParser.Location.Empty);
			
			DefaultMethod method = new DefaultMethod(operatorDeclaration.Name, CreateReturnType(operatorDeclaration.TypeReference), ConvertModifier(operatorDeclaration.Modifier), region, bodyRegion, c);
			ConvertAttributes(operatorDeclaration, method);
			if(operatorDeclaration.Parameters != null)
			{
				foreach (NRefactoryAST.ParameterDeclarationExpression par in operatorDeclaration.Parameters) {
					method.Parameters.Add(CreateParameter(par, method));
				}
			}
			c.Methods.Add(method);
			return null;
		}
		
		public override object VisitConstructorDeclaration(NRefactoryAST.ConstructorDeclaration constructorDeclaration, object data)
		{
			DomRegion region     = GetRegion(constructorDeclaration.StartLocation, constructorDeclaration.EndLocation);
			DomRegion bodyRegion = GetRegion(constructorDeclaration.EndLocation, constructorDeclaration.Body != null ? constructorDeclaration.Body.EndLocation : RefParser.Location.Empty);
			DefaultClass c = GetCurrentClass();
			
			Constructor constructor = new Constructor(ConvertModifier(constructorDeclaration.Modifier), region, bodyRegion, GetCurrentClass());
			constructor.Documentation = GetDocumentation(region.BeginLine, constructorDeclaration.Attributes);
			ConvertAttributes(constructorDeclaration, constructor);
			if (constructorDeclaration.Parameters != null) {
				foreach (NRefactoryAST.ParameterDeclarationExpression par in constructorDeclaration.Parameters) {
					constructor.Parameters.Add(CreateParameter(par));
				}
			}
			c.Methods.Add(constructor);
            mapConstructor(constructorDeclaration, constructor);
			return null;
		}
		
		public override object VisitDestructorDeclaration(NRefactoryAST.DestructorDeclaration destructorDeclaration, object data)
		{
			DomRegion region     = GetRegion(destructorDeclaration.StartLocation, destructorDeclaration.EndLocation);
			DomRegion bodyRegion = GetRegion(destructorDeclaration.EndLocation, destructorDeclaration.Body != null ? destructorDeclaration.Body.EndLocation : RefParser.Location.Empty);
			
			DefaultClass c = GetCurrentClass();
			
			Destructor destructor = new Destructor(region, bodyRegion, c);
			ConvertAttributes(destructorDeclaration, destructor);
			c.Methods.Add(destructor);
			return null;
		}
		
		bool IsVisualBasic {
			get {
				return cu.ProjectContent.Language == LanguageProperties.VBNet;
			}
		}
		
		public override object VisitFieldDeclaration(NRefactoryAST.FieldDeclaration fieldDeclaration, object data)
		{
			DomRegion region = GetRegion(fieldDeclaration.StartLocation, fieldDeclaration.EndLocation);
			DefaultClass c = GetCurrentClass();
			ModifierEnum modifier = ConvertModifier(fieldDeclaration.Modifier,
			                                        (c.ClassType == ClassType.Struct && this.IsVisualBasic)
			                                        ? ModifierEnum.Public : ModifierEnum.Private);
			string doku = GetDocumentation(region.BeginLine, fieldDeclaration.Attributes);
			if (currentClass.Count > 0) {
				for (int i = 0; i < fieldDeclaration.Fields.Count; ++i) {
					NRefactoryAST.VariableDeclaration field = (NRefactoryAST.VariableDeclaration)fieldDeclaration.Fields[i];
					
					IReturnType retType;
					if (c.ClassType == ClassType.Enum) {
						retType = c.DefaultReturnType;
					} else {
						retType = CreateReturnType(fieldDeclaration.GetTypeForField(i));
						if (!field.FixedArrayInitialization.IsNull)
							retType = new ArrayReturnType(cu.ProjectContent, retType, 1);
					}
					DefaultField f = new DefaultField(retType, field.Name, modifier, region, c);
					ConvertAttributes(fieldDeclaration, f);
					f.Documentation = doku;
					if (c.ClassType == ClassType.Enum) {
						f.Modifiers = ModifierEnum.Const | ModifierEnum.Public;
					}
					
					c.Fields.Add(f);
                    mapField(fieldDeclaration, f);
				}
			}
            
			return null;
		}
		
		public override object VisitPropertyDeclaration(NRefactoryAST.PropertyDeclaration propertyDeclaration, object data)
		{
			DomRegion region     = GetRegion(propertyDeclaration.StartLocation, propertyDeclaration.EndLocation);
			DomRegion bodyRegion = GetRegion(propertyDeclaration.BodyStart,     propertyDeclaration.BodyEnd);
			
			IReturnType type = CreateReturnType(propertyDeclaration.TypeReference);
			DefaultClass c = GetCurrentClass();
			
			DefaultProperty property = new DefaultProperty(propertyDeclaration.Name, type, ConvertModifier(propertyDeclaration.Modifier), region, bodyRegion, GetCurrentClass());
			if (propertyDeclaration.HasGetRegion) {
				property.GetterRegion = GetRegion(propertyDeclaration.GetRegion.StartLocation, propertyDeclaration.GetRegion.EndLocation);
				property.CanGet = true;
				property.GetterModifiers = ConvertModifier(propertyDeclaration.GetRegion.Modifier, ModifierEnum.None);
			}
			if (propertyDeclaration.HasSetRegion) {
				property.SetterRegion = GetRegion(propertyDeclaration.SetRegion.StartLocation, propertyDeclaration.SetRegion.EndLocation);
				property.CanSet = true;
				property.SetterModifiers = ConvertModifier(propertyDeclaration.SetRegion.Modifier, ModifierEnum.None);
			}
			property.Documentation = GetDocumentation(region.BeginLine, propertyDeclaration.Attributes);
			ConvertAttributes(propertyDeclaration, property);
			c.Properties.Add(property);
            mapProperty(propertyDeclaration, property);
			return null;
		}
		
		public override object VisitIndexerDeclaration(NRefactoryAST.IndexerDeclaration indexerDeclaration, object data)
		{
			DomRegion region     = GetRegion(indexerDeclaration.StartLocation, indexerDeclaration.EndLocation);
			DomRegion bodyRegion = GetRegion(indexerDeclaration.BodyStart,     indexerDeclaration.BodyEnd);
			DefaultProperty i = new DefaultProperty("Item", CreateReturnType(indexerDeclaration.TypeReference), ConvertModifier(indexerDeclaration.Modifier), region, bodyRegion, GetCurrentClass());
			i.IsIndexer = true;
			if (indexerDeclaration.HasGetRegion) {
				i.GetterRegion = GetRegion(indexerDeclaration.GetRegion.StartLocation, indexerDeclaration.GetRegion.EndLocation);
				i.CanGet = true;
				i.GetterModifiers = ConvertModifier(indexerDeclaration.GetRegion.Modifier, ModifierEnum.None);
			}
			if (indexerDeclaration.HasSetRegion) {
				i.SetterRegion = GetRegion(indexerDeclaration.SetRegion.StartLocation, indexerDeclaration.SetRegion.EndLocation);
				i.CanSet = true;
				i.SetterModifiers = ConvertModifier(indexerDeclaration.SetRegion.Modifier, ModifierEnum.None);
			}
			i.Documentation = GetDocumentation(region.BeginLine, indexerDeclaration.Attributes);
			ConvertAttributes(indexerDeclaration, i);
			if (indexerDeclaration.Parameters != null) {
				foreach (NRefactoryAST.ParameterDeclarationExpression par in indexerDeclaration.Parameters) {
					i.Parameters.Add(CreateParameter(par));
				}
			}
			// If an IndexerNameAttribute is specified, use the specified name
			// for the indexer instead of the default name.
			IAttribute indexerNameAttribute = i.Attributes.LastOrDefault(this.IsIndexerNameAttribute);
			if (indexerNameAttribute != null && indexerNameAttribute.PositionalArguments.Count > 0) {
				string name = indexerNameAttribute.PositionalArguments[0] as string;
				if (!String.IsNullOrEmpty(name)) {
					i.FullyQualifiedName = String.Concat(i.DeclaringType.FullyQualifiedName, ".", name);
				}
			}
			DefaultClass c = GetCurrentClass();
			c.Properties.Add(i);
			return null;
		}
		
		bool IsIndexerNameAttribute(IAttribute att)
		{
			if (att == null || att.AttributeType == null)
				return false;
			string indexerNameAttributeFullName = typeof(System.Runtime.CompilerServices.IndexerNameAttribute).FullName;
			IClass indexerNameAttributeClass = this.Cu.ProjectContent.GetClass(indexerNameAttributeFullName, 0, LanguageProperties.CSharp, GetClassOptions.Default | GetClassOptions.ExactMatch);
			if (indexerNameAttributeClass == null) {
				return String.Equals(att.AttributeType.FullyQualifiedName, indexerNameAttributeFullName, StringComparison.Ordinal);
			}
			return att.AttributeType.Equals(indexerNameAttributeClass.DefaultReturnType);
		}
		
		public override object VisitEventDeclaration(NRefactoryAST.EventDeclaration eventDeclaration, object data)
		{
			DomRegion region     = GetRegion(eventDeclaration.StartLocation, eventDeclaration.EndLocation);
			DomRegion bodyRegion = GetRegion(eventDeclaration.BodyStart,     eventDeclaration.BodyEnd);
			DefaultClass c = GetCurrentClass();
			
			IReturnType type;
			if (eventDeclaration.TypeReference.IsNull) {
				DefaultClass del = new DefaultClass(cu, ClassType.Delegate,
				                                    ConvertModifier(eventDeclaration.Modifier),
				                                    region, c);
				del.Modifiers |= ModifierEnum.Synthetic;
				CreateDelegate(del, eventDeclaration.Name + "EventHandler",
				               new NRefactoryAST.TypeReference("System.Void", true),
				               new NRefactoryAST.TemplateDefinition[0],
				               eventDeclaration.Parameters);
				type = del.DefaultReturnType;
			} else {
				type = CreateReturnType(eventDeclaration.TypeReference);
			}
			DefaultEvent e = new DefaultEvent(eventDeclaration.Name, type, ConvertModifier(eventDeclaration.Modifier), region, bodyRegion, c);
			ConvertAttributes(eventDeclaration, e);
			c.Events.Add(e);
			
			e.Documentation = GetDocumentation(region.BeginLine, eventDeclaration.Attributes);
			if (eventDeclaration.HasAddRegion) 
			{				
				var defaultMethod = new DefaultMethod(e.DeclaringType, "add_" + e.Name);
				var defaultParameters =  new List<IParameter>();
				defaultParameters.Add(new DefaultParameter("value", e.ReturnType, DomRegion.Empty));
				defaultMethod.Parameters = defaultParameters;
				defaultMethod.Region = GetRegion(eventDeclaration.AddRegion.StartLocation, eventDeclaration.AddRegion.EndLocation);
				defaultMethod.BodyRegion = GetRegion(eventDeclaration.AddRegion.Block.StartLocation, eventDeclaration.AddRegion.Block.EndLocation);
				e.AddMethod = defaultMethod;				
			}
			if (eventDeclaration.HasRemoveRegion) 
			{
				var defaultMethod = new DefaultMethod(e.DeclaringType, "remove_" + e.Name);
				var defaultParameters =  new List<IParameter>();
				defaultParameters.Add(new DefaultParameter("value", e.ReturnType, DomRegion.Empty));
				defaultMethod.Parameters = defaultParameters;
				defaultMethod.Region = GetRegion(eventDeclaration.RemoveRegion.StartLocation, eventDeclaration.RemoveRegion.EndLocation);
				defaultMethod.BodyRegion = GetRegion(eventDeclaration.RemoveRegion.Block.StartLocation, eventDeclaration.RemoveRegion.Block.EndLocation);
				e.RemoveMethod = defaultMethod;
			}
			return null;
		}
		
		IReturnType CreateReturnType(NRefactoryAST.TypeReference reference, IMethod method, TypeVisitor.ReturnTypeOptions options)
		{
			return CreateReturnType(reference, method, GetCurrentClass(), cu, options);
		}
		
		static IReturnType CreateReturnType(NRefactoryAST.TypeReference reference, IMethod method, IClass currentClass, ICompilationUnit cu, TypeVisitor.ReturnTypeOptions options)
		{
			if (currentClass == null) {
				return TypeVisitor.CreateReturnType(reference, new DefaultClass(cu, "___DummyClass"), method, 1, 1, cu.ProjectContent, options | TypeVisitor.ReturnTypeOptions.Lazy);
			} else {
				return TypeVisitor.CreateReturnType(reference, currentClass, method, currentClass.Region.BeginLine + 1, 1, cu.ProjectContent, options | TypeVisitor.ReturnTypeOptions.Lazy);
			}
		}
		
		IReturnType CreateReturnType(NRefactoryAST.TypeReference reference)
		{
			return CreateReturnType(reference, null, TypeVisitor.ReturnTypeOptions.None);
		}
	}
}

