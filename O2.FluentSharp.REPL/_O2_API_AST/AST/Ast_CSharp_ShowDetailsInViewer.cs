using System.Windows.Forms;
using ICSharpCode.TextEditor;
using O2.DotNetWrappers.DotNet;
using O2.DotNetWrappers.ExtensionMethods;
using O2.External.SharpDevelop.ExtensionMethods;
//using O2.Views.ASCX.MerlinWizard.O2Wizard_ExtensionMethods;

namespace O2.External.SharpDevelop.AST
{
    public class Ast_CSharp_ShowDetailsInViewer
    {
        public readonly TextEditorControl textEditorControl;
        public readonly TabControl tabControl;                
        public TreeView ast_TreeView;
        public TreeView usingDeclarations_TreeView;
        public TreeView types_TreeView;
        public TreeView methods_TreeView;
        public TreeView fields_TreeView;
        public TreeView properties_TreeView;
        public TreeView comments_TreeView;
        public TextBox errors_TextBox;
        public bool enabled = true;

        public Ast_CSharp_ShowDetailsInViewer(TextEditorControl _textEditorControl, TabControl _tabControl)
        {
            /*sourceCodeEditor = _sourceCodeEditor;            
            tabControl = (TabControl)sourceCodeEditor.field("tcSourceInfo");
             * */
            tabControl = _tabControl;
            textEditorControl = _textEditorControl;
            createGUI();
        }

        public void createGUI()
        {                                    
            methods_TreeView = tabControl.add_Tab("Methods").add_TreeView();
            properties_TreeView = tabControl.add_Tab("Properties").add_TreeView();
            fields_TreeView = tabControl.add_Tab("Fields").add_TreeView();
            types_TreeView = tabControl.add_Tab("Types").add_TreeView();                        
            comments_TreeView = tabControl.add_Tab("Comments").add_TreeView();
            usingDeclarations_TreeView = tabControl.add_Tab("Using Declarations").add_TreeView();
            ast_TreeView = tabControl.add_Tab("AST").add_TreeView();
            errors_TextBox = tabControl.add_Tab("AST Errors").add_TextBox(true);

            textEditorControl.Document.DocumentChanged += (sender, e) => update();

            usingDeclarations_TreeView.AfterSelect += showInSourceCode;
            types_TreeView.AfterSelect += showInSourceCode;
            methods_TreeView.AfterSelect += showInSourceCode;
            fields_TreeView.AfterSelect += showInSourceCode;
            properties_TreeView.AfterSelect += showInSourceCode;
            comments_TreeView.AfterSelect += showInSourceCode;
        }
        
        public void update()
        {
            textEditorControl.invokeOnThread(
                () =>
                    {
                        var documentText = textEditorControl.Text;
                        O2Thread.mtaThread(() => update(documentText));
                    });            
        }

        public void updateSync()
        {
            update(textEditorControl.Text);
        }

        public void update(string sourceCode)
        {
            if (enabled)
            {
                var ast = new Ast_CSharp(sourceCode);
                ast_TreeView.show_Ast(ast);
                types_TreeView.show_List(ast.AstDetails.Types, "Text");
                usingDeclarations_TreeView.show_List(ast.AstDetails.UsingDeclarations, "Text");
                methods_TreeView.show_List(ast.AstDetails.Methods, "Text");
                fields_TreeView.show_List(ast.AstDetails.Fields, "Text");
                properties_TreeView.show_List(ast.AstDetails.Properties, "Text");
                comments_TreeView.show_List(ast.AstDetails.Comments, "Text");
                errors_TextBox.set_Text(ast.Errors);
            }
        }

        private void showInSourceCode(object sender, TreeViewEventArgs e)
        {
            var treeNoteTag = e.Node.Tag;
            var endLocation = (ICSharpCode.NRefactory.Location)O2.Kernel.PublicDI.reflection.getProperty("EndLocation",treeNoteTag);
            var startLocation = (ICSharpCode.NRefactory.Location)O2.Kernel.PublicDI.reflection.getProperty("StartLocation", treeNoteTag);
            var originalObject = O2.Kernel.PublicDI.reflection.getProperty("OriginalObject", treeNoteTag);
            var text = (string)O2.Kernel.PublicDI.reflection.getProperty("Text", treeNoteTag);
            var astValue = new AstValue<object>(text,originalObject,startLocation,endLocation);
            textEditorControl.showAstValueInSourceCode(astValue);
            
            //astValue.EndLocation = 

            /*if (treeNoteTag is AstValue<ICSharpCode.NRefactory.Ast.MethodDeclaration>)
                textEditorControl.showAstValueInSourceCode((AstValue<ICSharpCode.NRefactory.Ast.MethodDeclaration>)treeNoteTag);

            else if (treeNoteTag is AstValue<object>)
            {
                var astValue = (AstValue<object>)treeNoteTag;
                //var textEditorControl = sourceCodeEditor.getObject_TextEditorControl();
                textEditorControl.showAstValueInSourceCode(astValue);
            }*/
        }
    }
}
