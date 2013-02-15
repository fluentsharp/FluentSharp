using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpEditor;

namespace O2SharpDevelop.CodeCompletion
{
    public class form_O2EditorWithCodeComplete : MainForm
    {
        public form_O2EditorWithCodeComplete(string fileToOpen)
        {
            //this();
            openFile(fileToOpen);
        }

        public void openFile(string fileToOpen)
        { 
            this.textEditorControl1.LoadFile(fileToOpen);
        }
    }
}
