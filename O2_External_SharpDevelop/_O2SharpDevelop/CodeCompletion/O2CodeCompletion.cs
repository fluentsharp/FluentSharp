// This code was based on the CSharp Editor Example with Code Completion created by Daniel Grunwald
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;

using O2.DotNetWrappers.ExtensionMethods;
using O2.DotNetWrappers.DotNet;
using O2.Views.ASCX.classes.MainGUI;

using ICSharpCode;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Gui;
using ICSharpCode.SharpDevelop.Dom.CSharp;
using ICSharpCode.TextEditor.Gui.CompletionWindow;
using ICSharpCode.SharpDevelop.Dom;
using ICSharpCode.SharpDevelop.Dom.NRefactoryResolver;

using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;
using CSharpEditor;

using O2.External.SharpDevelop.ExtensionMethods;
using O2.Kernel;

namespace O2.External.SharpDevelop.Ascx
{
    public class O2CodeCompletion : ICompletionDataProvider
    {        	
        public TextEditorControl textEditor;
        //public TextEditorControl textEditorToGrabCodeFrom;      // (Was not really working) to support behind the scenes AST building
        public List<string> extraSourceCodeToProcess;
        public LanguageProperties CurrentLanguageProperties = LanguageProperties.CSharp;
        public ProjectContentRegistry pcRegistry;
        public DefaultProjectContent myProjectContent;
        public ParseInformation parseInformation = new ParseInformation();
        public Dictionary<string, ICompilationUnit> mappedCompilationUnits { get; set; } // so that we support dynamic CodeCompletion from multiple files
        //public ICompilationUnit lastCompilationUnit;
        public string DummyFileName = "edited.cs";
        public int startOffset = 0;
        public ImageList SmallIcons;
        public Form HostForm { get; set; }
        public Location CodeCompleteCaretLocationOffset { get; set; }
        public string CodeCompleteTargetText { get; set; }
        public CodeCompletionWindow codeCompletionWindow;
        public Action<string> statusMessage;
        public bool OnlyShowCodeCompleteResulstFromO2Namespace { get; set; }
        public bool UseParseCodeThread { get; set; }
        public ExpressionResult currentExpression;
        public List<String> loadedReferences { get; set; }
        public List<String> gacAssemblies { get; set; }
		public ICompletionDataProvider completionDataProvider { get; set; }
		public Action<CodeCompletionWindow> After_CodeCompletionWindow_IsAvailable { get; set; }

        public O2CodeCompletion(TextEditorControl _textEditor)
            : this(_textEditor,(text)=>text.info())
        {
        }

        public O2CodeCompletion(TextEditorControl _textEditor, Action<string> status)
        {
            OnlyShowCodeCompleteResulstFromO2Namespace = false; //true;            
            UseParseCodeThread = true;
            extraSourceCodeToProcess = new List<string>();
            mappedCompilationUnits = new Dictionary<string, ICompilationUnit>();
            gacAssemblies = GacUtils.assemblyNames(true);
            loadedReferences = new List<string>();

            _textEditor.invokeOnThread(
                ()=>{
                        textEditor = _textEditor;
                        statusMessage = status;				    	
                        loadIcons();			    		
                        setupEnvironment();
                    });
        }
                
        
        public void setupEnvironment()
        {
            try
            {
                textEditor.ActiveTextAreaControl.TextArea.KeyEventHandler += TextAreaKeyEventHandler;
                textEditor.ActiveTextAreaControl.TextArea.Caret.PositionChanged += Caret_PositionChanged;
                textEditor.Disposed += CloseCodeCompletionWindow;  // When the editor is disposed, close the code completion window

                //set up the ToolTipRequest event
                textEditor.ActiveTextAreaControl.TextArea.ToolTipRequest += OnToolTipRequest;

                this.pcRegistry = new ProjectContentRegistry();
                this.myProjectContent = new DefaultProjectContent();
                this.myProjectContent.Language = this.CurrentLanguageProperties;
                var persistanceFolder = PublicDI.config.O2TempDir.pathCombine("..//_CSharpCodeCompletion").fullPath();  // Path.Combine(Path.GetTempPath(), "CSharpCodeCompletion");
                persistanceFolder.createFolder();
                pcRegistry.ActivatePersistence(persistanceFolder);
                // static parse current code thread
                //startParseCodeThread();
                // add references

                startAddReferencesThread();
                startParseCodeThread();
            }
            catch (Exception ex)
            {
                ex.log("in setupEnvironment");
            }

        }

        void Caret_PositionChanged(object sender, EventArgs e)
        {
            var caret = (Caret)sender;
           // "Current Offset: {0}".format(caret.Offset).info();
           // showLocationDetails(caret.Position);           
        }
        
        void loadIcons()    	
        {    	
            //System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(O2CodeCompletion));
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));    		
            SmallIcons = new ImageList();
            SmallIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));    			
        }
        /*public O2CodeCompletion(Form hostForm)
        {
            HostForm = hostForm;
        }*/
    
    
/*    	public void open(string fileToOpen)
        {
            textEditor.open(fileToOpen);
            //DummyFileName = fileToOpen;				// DC  - check if this makes the diference
        }
*/
        public void mapDotNetReferencesForCodeComplete()
        {
            statusMessage("Loading Code Complete Reference: MsCorlib");
            this.myProjectContent.AddReferencedContent(this.pcRegistry.Mscorlib);
            /*addReference("System.Windows.Forms");
            addReference("System");
            addReference("System.Data");
            addReference("System.Drawing");
            addReference("System.Xml");
            addReference("Microsoft.VisualBasic");
            addReference("PresentationCore");
            addReference("PresentationFramework");
            addReference("WindowsBase");
            addReference("WindowsFormsIntegration");*/
        }

        public void startAddReferencesThread()
        {
            O2Thread.mtaThread(
                () =>
                {
                    O2Thread.setPriority_Lowest();                    
                    mapDotNetReferencesForCodeComplete();

                    // for GraphSharp
                /*    addReference("ICSharpCode.AvalonEdit.dll");
                    addReference("GraphSharp.dll");
                    addReference("QuickGraph.dll");
                    addReference("GraphSharp.Controls.dll");
                    // for twitter
                    addReference("Newtonsoft.Json.dll");
                    addReference("Dimebrain.TweetSharp.dll");*/
                                        
                    //foreach (var reference in CompileEngine.getListOfO2AssembliesInExecutionDir())
                    //    addReference(reference);
                                   
                    //statusMessage("Dependencies loaded");
                });
        }
        
        public void addReference(string referenceAssembly)
        {
            try
            {
                if (loadedReferences.contains(referenceAssembly).isFalse())
                {
                    if (referenceAssembly.fileExists())
                        this.myProjectContent.add_Reference(this.pcRegistry, referenceAssembly, statusMessage);
                    else
                    {
                        var assembly = referenceAssembly.assembly();
                        if (assembly.notNull() && assembly.Location.fileExists())
                        {
                            this.myProjectContent.add_Reference(this.pcRegistry, assembly.Location, statusMessage);
                        }
                        else
                            "[addReference] could not find assembly for: {0}".error(referenceAssembly);
                        
                    }
 
                    //if (gacAssemblies.contains(assemblyWithoutExtension))
                    //    this.myProjectContent.add_Reference(this.pcRegistry, assemblyWithoutExtension, statusMessage);
                    //else
                    
                    loadedReferences.Add(referenceAssembly);
                }
            }
            catch (Exception ex)
            {
                ex.log("in O2CodeCompletion addReference:{0}".format(referenceAssembly));
            }
        }

        public void addReferences(List<string> referencesToAdd)
        {
            O2Thread.mtaThread(
                () =>{
                        try
                        {
                            var referencesTimer = new O2Timer("Added {0} references".format(referencesToAdd.size())).start(); ;
                            foreach (var referencedAssembly in referencesToAdd)
                                addReference(referencedAssembly);
                            referencesTimer.stop();
                        }
                        catch (Exception ex)
                        {
                            ex.log("in O2Completion addRefrences");
                        }
                    });
        }    
    
        // this will regularly parse the current source code so that we have code completion for its methods 
        public void startParseCodeThread()
        {
            O2Thread.mtaThread(
                ()=>{						
                        while (!textEditor.IsDisposed && UseParseCodeThread)
                            {			                    
                                this.parseSourceCode(DummyFileName, this.textEditor.get_Text());
                                foreach (var codeOrFile in extraSourceCodeToProcess)
                                    if (codeOrFile.isFile())
                                        this.parseSourceCode(codeOrFile,codeOrFile.contents());
                                    else
                                        this.parseSourceCode(codeOrFile);
                                this.sleep(2000,false);			             
                            }			              
                     });
        }        

        public void parseFile(string fileToParse)
        {
            parseSourceCode(fileToParse, fileToParse.fileContents());
        }

        public void parseSourceCode(string code)
        {
            parseSourceCode(DummyFileName, code);
        }


        public void parseSourceCode(string file, string code)
        {
            var text = textEditor.get_Text();
            //textEditor.set(")
            if (false == code.valid())
                return;

            if (false == mappedCompilationUnits.ContainsKey(file))
                mappedCompilationUnits.Add(file, null);

            var lastCompilationUnit = mappedCompilationUnits[file];

            var textReader = new StringReader(code);
            ICompilationUnit newCompilationUnit;
            var supportedLanguage = SupportedLanguage.CSharp;
            using (IParser p = ParserFactory.CreateParser(supportedLanguage, textReader))
            {
                // we only need to parse types and method definitions, no method bodies
                // so speed up the parser and make it more resistent to syntax
                // errors in methods
                p.ParseMethodBodies = false;
                p.Parse();
                newCompilationUnit = ConvertCompilationUnit(p.CompilationUnit);
            }            
            // Remove information from lastCompilationUnit and add information from newCompilationUnit.
            myProjectContent.UpdateCompilationUnit(lastCompilationUnit, newCompilationUnit, DummyFileName);
            mappedCompilationUnits[file] = newCompilationUnit;
            //            lastCompilationUnit = newCompilationUnit;
            parseInformation.SetCompilationUnit(newCompilationUnit);

            try {
            //    if (file.exists())
                    textEditor.Document.FoldingManager.UpdateFoldings(this.DummyFileName, parseInformation);
            } catch //(Exception ex)
            {
                //ex.log(ex);
            }

        }
        
        public ICompilationUnit ConvertCompilationUnit(CompilationUnit compilationUnit)
        {
            NRefactoryASTConvertVisitor converter;
            converter = new NRefactoryASTConvertVisitor(myProjectContent);
            compilationUnit.AcceptVisitor(converter, null);            
            return converter.Cu;
        }
        
        
        
        // Was part of the CodeCompletionKeyHandler file
        
        /// <summary>
        /// Return true to handle the keypress, return false to let the text area handle the keypress
        /// </summary>
        public bool TextAreaKeyEventHandler(char key)
        {            
            if (codeCompletionWindow != null)
            {
                // If completion window is open and wants to handle the key, don't let the text area
                // handle it

                if (codeCompletionWindow != null)
                    if (codeCompletionWindow.ProcessKeyEvent(key))
                        return true;
            }
   //         "key pressed:{0}".format(key).info();
         //   if (key == '.')// || key == ' ')
			if (key == '.')
            {
				showCodeCompleteWindow(key);
                 
                //});
//                return true;
            }			
            return false;
        }

        public void showCodeCompleteWindow(char key)
        { 
//            CodeCompleteTargetText = textEditor.get_Text(); // update this text so that we are working with the latest version of the code/snippet
            //var key = '.';
            //O2Thread.mtaThread(   //I really want to run this on a separate thread but It is causing a weird problem where the codecomplete only happens after the 2nd char
                //() =>
                //{

            currentExpression = FindExpression();
            //"[O2CodeComplete] Current Expression: {0}".info(currentExpression);
            //var o2Timer = new O2Timer("Code Completion").start();
            //textEditor.invokeOnThread(()=> textEditor.textArea().Caret.Column ++ );
            try
            {
                //startOffset = textEditor.currentOffset() + 1;   // it was +1 before we made this run on an mta thread
                completionDataProvider = this;//new CodeCompletionProvider(this);

                codeCompletionWindow = CodeCompletionWindow.ShowCompletionWindow(
                    textEditor.ParentForm,					// The parent window for the completion window
                    textEditor, 							// The text editor to show the window for
                    DummyFileName,							// Filename - will be passed back to the provider
                    completionDataProvider,					// Provider to get the list of possible completions
                    key										// Key pressed - will be passed to the provider
                );
                        
                if (codeCompletionWindow != null)
                {
                    // ShowCompletionWindow can return null when the provider returns an empty list
                    codeCompletionWindow.Closed += new EventHandler(CloseCodeCompletionWindow);
                }
                //textEditor.insertTextAtCurrentCaretLocation(".");
				codeCompletionWindow.AfterLoadGui = () =>
					{
						codeCompletionWindow.AfterLoadGui = null;			//so that we only invoke this once
						After_CodeCompletionWindow_IsAvailable.invoke(codeCompletionWindow);
					};
            }
            catch (Exception ex)
            {
                ex.log("in O2CodeCompletion.TextAreaKeyEventHandler");
            }
           // o2Timer.stop();
        }
        
        void CloseCodeCompletionWindow(object sender, EventArgs e)
        {
            if (codeCompletionWindow != null) {
                codeCompletionWindow.Closed -= new EventHandler(CloseCodeCompletionWindow);
                codeCompletionWindow.Dispose();
                codeCompletionWindow = null;
            }
        }
        
        
        // was part of Tool Tip Provider
        
        void OnToolTipRequest(object sender, ToolTipRequestEventArgs e)
        {
            O2Thread.mtaThread(
            () =>
            {
                try
                {
                    if (e.InDocument && !e.ToolTipShown)
                    {
                        var logicalPosition = e.LogicalPosition;

                        // parseSourceCode(CodeCompleteTargetText);
                        ResolveResult rr = resolveLocation(logicalPosition);
                        string toolTipText = GetText(rr);
                        if (toolTipText != null)
                        {
                            e.ShowToolTip(toolTipText);
                            //}
                            //if (toolTipText.valid())
                            "ToolTipText: {0}".format(toolTipText).info();
                        }

                    }
                }
                catch (Exception ex)
                {
                    ex.log("in OnToolTipRequest");
                }
            });
        }

        public void showLocationDetails(TextLocation location)
        {
            try
            {
                if (location == null)
                    return;
                "Line: {0}".format(textEditor.Text.lines()[location.Line]).debug();
                ResolveResult rr = resolveLocation(location);
                if (rr == null)
                    return;
                if (rr is MemberResolveResult)
                {
                    var memberResolved = (MemberResolveResult)rr;
                    if (memberResolved.CallingMember != null)
                        "   CallingMember: {0}".format(memberResolved.CallingMember.DotNetName).info();
                    if (memberResolved.ResolvedMember != null)
                        "   ResolvedMember: {0}".format(memberResolved.ResolvedMember.DotNetName).info();
                    if (memberResolved.ResolvedType != null)
                        "   ResolvedType: {0}".format(rr.ResolvedType.DotNetName).info();
                }
                else if (rr is UnknownIdentifierResolveResult)
                {
                    var unknonwResult = (UnknownIdentifierResolveResult)rr;
                    "   Identifier: {0}".format(unknonwResult.Identifier).info();
                    "   IsValid: {0}".format(unknonwResult.IsValid).info();
                }
                else
                {
                    string toolTipText = GetText(rr);
                    if (toolTipText.valid())
                        "    ToolTipText: {0}".format(toolTipText).info();
                    else
                    {
                        if (rr.CallingMember != null)
                            "   CallingMember: {0}".format(rr.CallingMember.DotNetName).info();
                        if (rr.ResolvedType != null)
                            "   ResolvedType: {0}".format(rr.ResolvedType.DotNetName).info();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.log("in showLocationDetails");
            }
        }
        private ResolveResult resolveLocation(TextLocation logicalPosition)
        {
           var textArea = textEditor.ActiveTextAreaControl.TextArea;
            var targetText = "";
            var targetOffset = 0;
            if (CodeCompleteCaretLocationOffset.Line == 0)
            {
                targetText = textEditor.get_Text();//.Text;
                targetOffset = textEditor.Document.PositionToOffset(logicalPosition);
            }
            else
            {
                var firstMethodOffset = calculateFirstMethodOffset();
                targetText = getAdjustedSnippetText(textArea, firstMethodOffset);
                targetOffset = firstMethodOffset + textArea.Caret.Offset;
            }
              ExpressionResult expression;
              
              //getExpressionFromTextArea(textArea);
              IExpressionFinder expressionFinder;
              expressionFinder = new CSharpExpressionFinder(this.parseInformation);
              expression = expressionFinder.FindFullExpression(targetText,targetOffset);              

            /*IExpressionFinder expressionFinder;
            expressionFinder = new CSharpExpressionFinder(this.parseInformation);
            ExpressionResult expression = expressionFinder.FindFullExpression(
                textEditor.Text,
                textEditor.Document.PositionToOffset(logicalPosition));
             */
            if (expression.Region.IsEmpty)
            {
                expression.Region = new DomRegion(logicalPosition.Line + 1, logicalPosition.Column + 1);
            }
            
            NRefactoryResolver resolver = new NRefactoryResolver(this.myProjectContent.Language);
            ResolveResult rr = resolver.Resolve(expression,
                                                this.parseInformation,
                                                targetText);
                                                //textArea.MotherTextEditorControl.Text);

            return rr;
        }
        
        static string GetText(ResolveResult result)
        {
            if (result == null) {
                return null;
            }
            if (result is MixedResolveResult)
                return GetText(((MixedResolveResult)result).PrimaryResult);
            IAmbience ambience = new CSharpAmbience();
            ambience.ConversionFlags = ConversionFlags.StandardConversionFlags | ConversionFlags.ShowAccessibility;
            if (result is MemberResolveResult) {
                return GetMemberText(ambience, ((MemberResolveResult)result).ResolvedMember);
            } else if (result is LocalResolveResult) {
                LocalResolveResult rr = (LocalResolveResult)result;
                ambience.ConversionFlags = ConversionFlags.UseFullyQualifiedTypeNames
                    | ConversionFlags.ShowReturnType;
                StringBuilder b = new StringBuilder();
                if (rr.IsParameter)
                    b.Append("parameter ");
                else
                    b.Append("local variable ");
                b.Append(ambience.Convert(rr.Field));
                return b.ToString();
            } else if (result is NamespaceResolveResult) {
                return "namespace " + ((NamespaceResolveResult)result).Name;
            } else if (result is TypeResolveResult) {
                IClass c = ((TypeResolveResult)result).ResolvedClass;
                if (c != null)
                    return GetMemberText(ambience, c);
                else
                    return ambience.Convert(result.ResolvedType);
            } else if (result is MethodGroupResolveResult) {
                MethodGroupResolveResult mrr = result as MethodGroupResolveResult;
                IMethod m = mrr.GetMethodIfSingleOverload();
                if (m != null)
                    return GetMemberText(ambience, m);
                else
                    return "Overload of " + ambience.Convert(mrr.ContainingType) + "." + mrr.Name;
            } else {
                return null;
            }
        }
        
        static string GetMemberText(IAmbience ambience, IEntity member)
        {
            StringBuilder text = new StringBuilder();
            if (member is IField) {
                text.Append(ambience.Convert(member as IField));
            } else if (member is IProperty) {
                text.Append(ambience.Convert(member as IProperty));
            } else if (member is IEvent) {
                text.Append(ambience.Convert(member as IEvent));
            } else if (member is IMethod) {
                text.Append(ambience.Convert(member as IMethod));
            } else if (member is IClass) {
                text.Append(ambience.Convert(member as IClass));
            } else {
                text.Append("unknown member ");
                text.Append(member.ToString());
            }
            string documentation = member.Documentation;
            if (documentation != null && documentation.Length > 0) {
                text.Append('\n');
                text.Append(CodeCompletionData.XmlDocumentationToText(documentation));
            }
            return text.ToString();
        }
        
        
        // part of the CodeCompletionProvider   (public class CodeCompletionProvider : ICompletionDataProvider)
        
        
        public ImageList ImageList {
            get {
                return this.SmallIcons;				
            }
        }
        
        public string PreSelection {
            get {
                return null;
            }
        }
        
        public int DefaultIndex {
            get {
                return -1;
            }
        }
        
        public CompletionDataProviderKeyResult ProcessKey(char key)
        {
            if (char.IsLetterOrDigit(key) || key == '_') {
                return CompletionDataProviderKeyResult.NormalKey;
            } else {
                // key triggers insertion of selected items
                return CompletionDataProviderKeyResult.InsertionKey;
            }
        }
        
        /// <summary>
        /// Called when entry should be inserted. Forward to the insertion action of the completion data.
        /// </summary>
        public bool InsertAction(ICompletionData data, TextArea textArea, int insertionOffset, char key)
        {
            textArea.Caret.Position = textArea.Document.OffsetToPosition(insertionOffset);
            return data.InsertAction(textArea, key);
        }
        
        public ICompletionData[] GenerateCompletionData(string fileName, TextArea textArea, char charTyped)
        {
            // We can return code-completion items like this:
            
            //return new ICompletionData[] {
            //	new DefaultCompletionData("Text", "Description", 1)
            //};
            var targetText = "";
            if (CodeCompleteCaretLocationOffset.Line == 0)
            {

                targetText = textArea.get_Text(); // textArea.MotherTextEditorControl.Text;                
            }
            else
            {
                var firstMethodOffset = calculateFirstMethodOffset();
                targetText = getAdjustedSnippetText(textArea, firstMethodOffset);
            }

            NRefactoryResolver resolver = new NRefactoryResolver(this.myProjectContent.Language);
            //ResolveResult rr = resolver.Resolve(FindExpression(textArea),
            ResolveResult rr = resolver.Resolve(currentExpression,
                                                    this.parseInformation,
                                                    targetText);
            List<ICompletionData> resultList = new List<ICompletionData>();
            if (rr.notNull())// && ) 
            {                
                ArrayList completionData = rr.GetCompletionData(this.myProjectContent);
                /*"[CodeComplete] expression '{0}' was resolved into type: {1} with {2} results".info(currentExpression.Expression, 
                                                                                                    rr.ResolvedType.FullyQualifiedName,
                                                                                                    completionData.isNull() ? -1 
                                                                                                                            : completionData.Count);*/
                "[CodeComplete] expression '{0}' was resolved into: {1} with {2} results".info(currentExpression.Expression,
                                                                                               rr.ResolvedType.notNull() 
                                                                                                    ? rr.ResolvedType.FullyQualifiedName
                                                                                                    : rr.prop("Name").str(),
                                                                                               completionData.isNull() 
                                                                                                    ? -1 
                                                                                                    : completionData.Count);
                if (completionData != null) {
                    AddCompletionData(resultList, completionData);
                }
            }
            else
                "[CodeComplete] expression '{0}' could not be resolved".error(currentExpression.Expression);
           // "In generate completion Data, There were {0} results found".format(resultList.Count).debug();
            return resultList.ToArray();
        }
        
        /// <summary>
        /// Find the expression the cursor is at.
        /// Also determines the context (using statement, "new"-expression etc.) the
        /// cursor is at.
        /// </summary>
        ExpressionResult FindExpression()//TextArea textArea)
        {
            var textArea = textEditor.ActiveTextAreaControl.TextArea;
            try
            {                
                var expression = (CodeCompleteCaretLocationOffset.Line == 0)
                    ? getExpressionFromTextArea(textArea)
                    : getExpressionFromCodeSnippet(textArea);
                                                
                if (expression.Region.IsEmpty)
                {
                    expression.Region = new DomRegion(textArea.Caret.Line + 1, textArea.Caret.Column + 1);
                }
                return expression;
            }
            catch (Exception ex)
            {
                ex.log("in FindExpression");
                return new ExpressionResult(); ;
            }
        }

        public ExpressionResult getExpressionFromTextArea(TextArea textArea)
        {
            IExpressionFinder finder = new CSharpExpressionFinder(this.parseInformation);
            return finder.FindExpression(textArea.Document.TextContent, textArea.Caret.Offset);            
        }

        public ExpressionResult getExpressionFromCodeSnippet(TextArea textArea)
        {
            IExpressionFinder finder = new CSharpExpressionFinder(this.parseInformation);
            var firstMethodOffset = calculateFirstMethodOffset();
            var adjustedSnippeetText = getAdjustedSnippetText(textArea, firstMethodOffset);

            var offset = firstMethodOffset + textArea.Caret.Offset;

            var expression = finder.FindExpression(adjustedSnippeetText, offset);
            return expression;
        }

        public int calculateFirstMethodOffset()
        {            
            //var offset = 0;                              
            var lines = CodeCompleteTargetText.lines();
             //"CodeCompleteTargetText:\n\n{0}".info(CodeCompleteTargetText);
            var linesToRemove = lines.size() - CodeCompleteCaretLocationOffset.Line +1;
            lines.RemoveRange(CodeCompleteCaretLocationOffset.Line -1, linesToRemove);            
            var topText = StringsAndLists.fromStringList_getText(lines);
            //for (int i = 0; i < CodeCompleteCaretLocationOffset.Line; i++)
            //    offset += lines[i].Length + 1;
        //    offset--;
           // var test = CodeCompleteTargetText.Substring(offset);
            return topText.Length;
        }

        public string getAdjustedSnippetText(TextArea textArea, int firstMethodOffset)
        {            
            var currentText = textArea.get_Text(); ;           
            var size = CodeCompleteTargetText.size();
            if (firstMethodOffset < size)
            {
                var adjustedSnippeetText = CodeCompleteTargetText.Substring(0, firstMethodOffset);
                adjustedSnippeetText += currentText.line();
                adjustedSnippeetText += "\t}".line() +
                                          "}".line();
                return adjustedSnippeetText;
            }
            else
                return currentText;
        }

        void AddCompletionData(List<ICompletionData> resultList, ArrayList completionData)
        {
         //   var currentCodeCompleteText = "";
         //   var lenght = textEditor.currentOffset() - startOffset;
         //   if (lenght >0)
         //       currentCodeCompleteText = textEditor.get_Text(startOffset,lenght);
         //   "{0}:{1}  = {2}".format(startOffset,lenght , currentCodeCompleteText).debug();

            // used to store the method names for grouping overloads
            Dictionary<string, CodeCompletionData> nameDictionary = new Dictionary<string, CodeCompletionData>();
            
            // Add the completion data as returned by SharpDevelop.Dom to the
            // list for the text editor
            foreach (object obj in completionData) {
                if (obj is string) {
                    // namespace names are returned as string
                    resultList.Add(new DefaultCompletionData((string)obj, "namespace " + obj, 5));
                } else if (obj is IClass) {
                    IClass c = (IClass)obj;
                    resultList.Add(new CodeCompletionData(c,this));
                } else if (obj is IMember) {
                    IMember m = (IMember)obj;
                    if (m is IMethod && ((m as IMethod).IsConstructor)) {
                        // Skip constructors
                        continue;
                    }
                    // if OnlyShowCodeCompleteResulstFromO2Namespace filter for only O2.* namepace
                    if (OnlyShowCodeCompleteResulstFromO2Namespace &&  m.DeclaringType.Namespace.starts("O2") == false)
                        continue;

                    // NOT WORKING only show items that match currentCodeCompleteText regex
            //        if (currentCodeCompleteText != "" && m.DotNetName.nregEx(currentCodeCompleteText))
            //            continue;
                    //if 
                    // Group results by name and add "(x Overloads)" to the
                    // description if there are multiple results with the same name.                    
                    CodeCompletionData data;
                    if (nameDictionary.TryGetValue(m.Name, out data)) 
					{						
						data.AddOverload(m);
                    } else {
                        nameDictionary[m.Name] = data = new CodeCompletionData(m,this);
                        resultList.Add(data);
                    }
                } else {
                    // Current ICSharpCode.SharpDevelop.Dom should never return anything else
                    throw new NotSupportedException();
                }
            }
        }
        
    }
    
    public class CodeCompletionData : DefaultCompletionData, ICompletionData
    {
        public O2CodeCompletion o2CodeCompletion;
        public IMember member;
        public IClass c;
        public CSharpAmbience csharpAmbience = new CSharpAmbience();
        
        public CodeCompletionData(IMember member, O2CodeCompletion _o2CodeCompletion)
            : base(member.Name, null, GetMemberImageIndex(member))
        {
            this.member = member;
            o2CodeCompletion =  _o2CodeCompletion;
        }
        
        public CodeCompletionData(IClass c, O2CodeCompletion _o2CodeCompletion)
            : base(c.Name, null, GetClassImageIndex(c))
        {
            this.c = c;
            o2CodeCompletion = _o2CodeCompletion;
        }
        
        //public int overloads = 0;
	    public List<IMember> overloads = new List<IMember>();
        //public void AddOverload()
		internal void AddOverload(IMember member)
		{
			//var name = (m as DefaultMethod).DotNetName;
			overloads.Add(member);
//            overloads++;
        }
        
        static int GetMemberImageIndex(IMember member)
        {
            // Missing: different icons for private/public member
            if (member is IMethod)
                return 1;
            if (member is IProperty)
                return 2;
            if (member is IField)
                return 3;
            if (member is IEvent)
                return 6;
            return 3;
        }
        
        static int GetClassImageIndex(IClass c)
        {
            switch (c.ClassType) {
                case ICSharpCode.SharpDevelop.Dom.ClassType.Enum:
                    return 4;
                default:
                    return 0;
            }
        }
        
        string description;
        
        // DefaultCompletionData.Description is not virtual, but we can reimplement
        // the interface to get the same effect as overriding.
        string ICompletionData.Description {
            get {
                if (description == null) {
                    IEntity entity = (IEntity)member ?? c;
                    description = GetText(entity);

                    if (overloads.size() > 1) 
					{
                        description += " (+" + overloads.size() + " overloads)";
						description += "\n\n-------------------------- \n\n";
						foreach (var overload in overloads)
						{
							if (overload is DefaultMethod)
								description += "{0}".line().format((overload as DefaultMethod).Signature());
							else
								description += "{0}".line().format(overload.str());
						}
					}
	                
                    //description += Environment.NewLine + XmlDocumentationToText(entity.Documentation);
                }
                return description;
            }
        }
        
        /// <summary>
        /// Converts a member to text.
        /// Returns the declaration of the member as C# or VB code, e.g.
        /// "public void MemberName(string parameter)"
        /// </summary>
        string GetText(IEntity entity)
        {
            return (string)o2CodeCompletion.textEditor.invokeOnThread(
                ()=> {					
                        IAmbience ambience = csharpAmbience;
                        if (entity is IMethod)
                            return ambience.Convert(entity as IMethod);
                        if (entity is IProperty)
                            return ambience.Convert(entity as IProperty);
                        if (entity is IEvent)
                            return ambience.Convert(entity as IEvent);
                        if (entity is IField)
                            return ambience.Convert(entity as IField);
                        if (entity is IClass)
                            return ambience.Convert(entity as IClass);
                        // unknown entity:
                        return entity.ToString();
                    });
        }
        
        static public string XmlDocumentationToText(string xmlDoc)
        {
            PublicDI.log.error(xmlDoc);
            StringBuilder b = new StringBuilder();
            try {
                using (XmlTextReader reader = new XmlTextReader(new StringReader("<root>" + xmlDoc + "</root>"))) {
                    reader.XmlResolver = null;
                    while (reader.Read()) {
                        switch (reader.NodeType) {
                            case XmlNodeType.Text:
                                b.Append(reader.Value);
                                break;
                            case XmlNodeType.Element:
                                switch (reader.Name) {
                                    case "filterpriority":
                                        reader.Skip();
                                        break;
                                    case "returns":
                                        b.AppendLine();
                                        b.Append("Returns: ");
                                        break;
                                    case "param":
                                        b.AppendLine();
                                        b.Append(reader.GetAttribute("name") + ": ");
                                        break;
                                    case "remarks":
                                        b.AppendLine();
                                        b.Append("Remarks: ");
                                        break;
                                    case "see":
                                        if (reader.IsEmptyElement) {
                                            b.Append(reader.GetAttribute("cref"));
                                        } else {
                                            reader.MoveToContent();
                                            if (reader.HasValue) {
                                                b.Append(reader.Value);
                                            } else {
                                                b.Append(reader.GetAttribute("cref"));
                                            }
                                        }
                                        break;
                                }
                                break;
                        }
                    }
                }
                return b.ToString();
            } catch (XmlException) {
                return xmlDoc;
            }
        }		
	}
    
    
    public static class SharpdevelopExtensionMethods
    {
        public static void add_Reference(this DefaultProjectContent projectContent, ProjectContentRegistry pcRegistry, string assemblyToLoad, Action<string> debugMessage)
        {
            try
            {
                debugMessage("Loading Code Completion Reference: {0}".format(assemblyToLoad));
                //if (!assemblyToLoad.fileExists())
                //	"file doesn't exist".error();    		
                IProjectContent referenceProjectContent = pcRegistry.GetProjectContentForReference(assemblyToLoad, assemblyToLoad);
                if (referenceProjectContent == null)
                    "referenceProjectContent was null".error();
                else
                {
                    projectContent.AddReferencedContent(referenceProjectContent);
                    if (referenceProjectContent is ReflectionProjectContent)
                        (referenceProjectContent as ReflectionProjectContent).InitializeReferences();
                    else
                        "something when wrong in DefaultProjectContent.add_Reference".error();
                }
            }
            catch (Exception ex)
            {
                ex.log("in DefaultProjectContent.add_Reference for assembly: {0}".format(assemblyToLoad));
            }
        }
    }
}
