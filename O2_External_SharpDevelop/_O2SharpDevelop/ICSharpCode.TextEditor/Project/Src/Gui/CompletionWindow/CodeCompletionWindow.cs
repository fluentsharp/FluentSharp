// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Mike Krüger" email="mike@icsharpcode.net"/>
//     <version>$Revision: 3515 $</version>
// </file>

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using ICSharpCode.TextEditor.Document;
using O2.DotNetWrappers.ExtensionMethods;
using O2.DotNetWrappers.DotNet;
using O2.External.SharpDevelop.Ascx;

namespace ICSharpCode.TextEditor.Gui.CompletionWindow
{
	public class CodeCompletionWindow : AbstractCompletionWindow
	{
		public ICompletionData[] completionData;
		public CodeCompletionListView codeCompletionListView;
		public VScrollBar vScrollBar = new VScrollBar();
		public ICompletionDataProvider dataProvider;
		public IDocument document;
		public bool showDeclarationWindow = true;
		public bool fixedListViewWidth = true;
		public const int ScrollbarWidth = 16;
		public const int MaxListLength = 10;

		public int startOffset;         //DC: Made these public
		public int endOffset;           //DC: Made these public
        public bool guiLoaded;           //DC
        public static bool busy;        //DC
		public CodeCompletionData lastCodeCompleteData;
		public DeclarationViewWindow declarationViewWindow = null;
		public new Rectangle workingScreen;

		//DC Couple events
		public Action										AfterWindowOpen { get; set; }	
		public Action										AfterLoadGui { get; set; }
		public Action<Keys>									AfterKeyPress { get; set; }
		public Func<ICompletionData[], ICompletionData[]>	FilterCompletionData { get; set; }		

		public static CodeCompletionWindow ShowCompletionWindow(Form parent, TextEditorControl control, string fileName, ICompletionDataProvider completionDataProvider, char firstChar)
		{
			return ShowCompletionWindow(parent, control, fileName, completionDataProvider, firstChar, true, true);
		}
		
		public static CodeCompletionWindow ShowCompletionWindow(Form parent, TextEditorControl control, string fileName, ICompletionDataProvider completionDataProvider, char firstChar, bool showDeclarationWindow, bool fixedListViewWidth)
		{
            if (busy)  // DC to prevent multiple calls
            {
                "CodeCompletionWindow.ShowCompletionWindow was busy, skipping ShowCompletionWindow calculation".info();
                return null;
            }

            busy = true;
            
            //return (CodeCompletionWindow)parent.invokeOnThread(
            return (CodeCompletionWindow)control.invokeOnThread(
                        ()=>{
                                return ShowCompletionWindow_Thread(parent, control, fileName,completionDataProvider, firstChar, showDeclarationWindow, fixedListViewWidth);
                            });
            
		}

		
		
		

        public static CodeCompletionWindow ShowCompletionWindow_Thread(Form parent,TextEditorControl control, string fileName, ICompletionDataProvider completionDataProvider, char firstChar, bool showDeclarationWindow, bool fixedListViewWidth)
        { 
            try
                {

                    var tempCompletionData = new ICompletionData[] { };
                    CodeCompletionWindow codeCompletionWindow = new CodeCompletionWindow(completionDataProvider, tempCompletionData, parent, control, showDeclarationWindow, fixedListViewWidth);
	                codeCompletionWindow.CloseWhenCaretAtBeginning = firstChar == '\0';
                    codeCompletionWindow.ShowCompletionWindow();

					codeCompletionWindow.AfterWindowOpen.invoke();

                    O2Thread.mtaThread(         // run in on a separate thread for performance reasons
                        () =>
                        {
                            try
                            {
                                ICompletionData[] completionData = completionDataProvider.GenerateCompletionData(fileName, control.ActiveTextAreaControl.TextArea, firstChar);
                                if (completionData == null || completionData.Length == 0)
                                {
                                    //"There was no CompleteData".error();
                                    //return null;
                                }
                                else
                                    codeCompletionWindow.setCodeCompletionData(completionData);                                
                            }
                            catch (Exception ex)
                            {
                                ex.log("in CodeCompletionWindow.ShowCompletionWindow ");
                            }
                            busy = false;
                        });

                    return codeCompletionWindow;
                }
                catch// (Exception ex)
                {
                    busy = false;
                    return null;
                }
        }

		
            
        CodeCompletionWindow(ICompletionDataProvider completionDataProvider, ICompletionData[] completionData, Form parentForm, TextEditorControl control, bool showDeclarationWindow, bool fixedListViewWidth) : base(parentForm, control)
		{
			this.dataProvider = completionDataProvider;
			this.completionData = completionData;
			this.document = control.Document;
			this.showDeclarationWindow = showDeclarationWindow;
			this.fixedListViewWidth = fixedListViewWidth;
            this.guiLoaded = false;
            startOffset = control.ActiveTextAreaControl.Caret.Offset +1;
            endOffset = startOffset; 
            //startOffset = -1;
            //endOffset = -1;
		}
		
		bool inScrollUpdate;

        //DC
        public void setCodeCompletionData(ICompletionData[] completionData)
        {
			this.completionData = FilterCompletionData.notNull()
									? FilterCompletionData.invoke(completionData)
									: completionData;

            //parentForm.invokeOnThread(() => loadGui());
            control.invokeOnThread(() => loadGui());
        }

        // DC (this code was in the constructor of this method which was not working on a multithread environment
        public void loadGui()
        {
			this.clear();
            var completionDataProvider = this.dataProvider;            

            workingScreen = Screen.GetWorkingArea(Location);
        //    startOffset = control.ActiveTextAreaControl.Caret.Offset;// +1;
        //    endOffset = startOffset;
            
			endOffset = control.ActiveTextAreaControl.Caret.Offset;

            if (completionDataProvider.PreSelection != null)
            {
                startOffset -= completionDataProvider.PreSelection.Length;
                endOffset--;
            }
            guiLoaded = true;
            codeCompletionListView = new CodeCompletionListView(completionData);
            codeCompletionListView.ImageList = completionDataProvider.ImageList;
            codeCompletionListView.Dock = DockStyle.Fill;
            codeCompletionListView.SelectedItemChanged += new EventHandler(CodeCompletionListViewSelectedItemChanged);
            codeCompletionListView.DoubleClick += new EventHandler(CodeCompletionListViewDoubleClick);
            codeCompletionListView.Click += new EventHandler(CodeCompletionListViewClick);
			codeCompletionListView.KeyPress += new KeyPressEventHandler(codeCompletionListView_KeyPress);
			codeCompletionListView.KeyUp += new System.Windows.Forms.KeyEventHandler(codeCompletionListView_KeyUp);
            Controls.Add(codeCompletionListView);

            if (completionData.Length > MaxListLength)
            {
                vScrollBar.Dock = DockStyle.Right;
                vScrollBar.Minimum = 0;
                vScrollBar.Maximum = completionData.Length - 1;
                vScrollBar.SmallChange = 1;
                vScrollBar.LargeChange = MaxListLength;
                codeCompletionListView.FirstItemChanged += new EventHandler(CodeCompletionListViewFirstItemChanged);
                Controls.Add(vScrollBar);
            }

            this.drawingSize = GetListViewSize();
            SetLocation();

            if (declarationViewWindow == null)
            {
                //declarationViewWindow = new DeclarationViewWindow(parentForm);
                declarationViewWindow = new DeclarationViewWindow();
            }
            SetDeclarationViewLocation();
            declarationViewWindow.ShowDeclarationViewWindow();
            declarationViewWindow.MouseMove += ControlMouseMove;
           // control.Focus();
            CodeCompletionListViewSelectedItemChanged(this, EventArgs.Empty);

            if (completionDataProvider.DefaultIndex >= 0)
            {
                codeCompletionListView.SelectIndex(completionDataProvider.DefaultIndex);
            }

            if (completionDataProvider.PreSelection != null)
            {
                CaretOffsetChanged(this, EventArgs.Empty);
            }

            vScrollBar.ValueChanged += VScrollBarValueChanged;
            document.DocumentAboutToBeChanged += DocumentAboutToBeChanged;
	        addToolStrip();
			this.AfterLoadGui.invoke();
        }		

		void CodeCompletionListViewFirstItemChanged(object sender, EventArgs e)
		{
			if (inScrollUpdate) 
				return;
			inScrollUpdate = true;
			var value = Math.Min(vScrollBar.Maximum, codeCompletionListView.FirstItem);
			if (value > -1 && value < vScrollBar.Maximum)
				vScrollBar.Value = value;
			inScrollUpdate = false;
		}
		
		void VScrollBarValueChanged(object sender, EventArgs e)
		{
			if (inScrollUpdate) return;
			inScrollUpdate = true;
			codeCompletionListView.FirstItem = vScrollBar.Value;
			codeCompletionListView.Refresh();
            //DC: 6/15/2012 this is still not working 100% since the up and down buttons down work well after the user clicks on the VSScroll bar
            //    but before it would disaper (i.e. go into background (which was worse :)  )
			//control.ActiveTextAreaControl.TextArea.Focus();
			inScrollUpdate = false;
		}
		
		void SetDeclarationViewLocation()
		{
			//  This method uses the side with more free space
			int leftSpace = Bounds.Left - workingScreen.Left;
			int rightSpace = workingScreen.Right - Bounds.Right;
			Point pos;
			// The declaration view window has better line break when used on
			// the right side, so prefer the right side to the left.
			if (rightSpace * 2 > leftSpace) {
				declarationViewWindow.FixedWidth = false;
				pos = new Point(Bounds.Right, Bounds.Top);
				if (declarationViewWindow.Location != pos) {
					declarationViewWindow.Location = pos;
				}
			} else {
				declarationViewWindow.Width = declarationViewWindow.GetRequiredLeftHandSideWidth(new Point(Bounds.Left, Bounds.Top));
				declarationViewWindow.FixedWidth = true;
				if (Bounds.Left < declarationViewWindow.Width) {
					pos = new Point(0, Bounds.Top);
				} else {
					pos = new Point(Bounds.Left - declarationViewWindow.Width, Bounds.Top);
				}
				if (declarationViewWindow.Location != pos) {
					declarationViewWindow.Location = pos;
				}
				declarationViewWindow.Refresh();
			}
		}
		
		protected override void SetLocation()
		{
            this.invokeOnThread(
                ()=>{

                    base.SetLocation();
			        if (declarationViewWindow != null)
				        SetDeclarationViewLocation();
			        });
		}
		
		Util.MouseWheelHandler mouseWheelHandler = new Util.MouseWheelHandler();
		
		public void HandleMouseWheel(MouseEventArgs e)
		{
			int scrollDistance = mouseWheelHandler.GetScrollAmount(e);
			if (scrollDistance == 0)
				return;
			if (control.TextEditorProperties.MouseWheelScrollDown)
				scrollDistance = -scrollDistance;
			int newValue = vScrollBar.Value + vScrollBar.SmallChange * scrollDistance;
			vScrollBar.Value = Math.Max(vScrollBar.Minimum, Math.Min(vScrollBar.Maximum - vScrollBar.LargeChange + 1, newValue));
		}

		void CodeCompletionListViewSelectedItemChanged(object sender, EventArgs e)
		{
            if (codeCompletionListView != null)
            {
                ICompletionData data = codeCompletionListView.SelectedCompletionData;
                if (showDeclarationWindow && data != null && data.Description != null && data.Description.Length > 0)
                {
                    declarationViewWindow.Description = data.Description;
                    if (data is O2.External.SharpDevelop.Ascx.CodeCompletionData)
					{						
                        lastCodeCompleteData = (O2.External.SharpDevelop.Ascx.CodeCompletionData)data;
						if (lastCodeCompleteData.member.notNull())
                        {
							var memberSignature = lastCodeCompleteData.member.str().remove("[DefaultMethod: ").removeLastChar();
                            declarationViewWindow.Description = (declarationViewWindow.Description + memberSignature).trim();
                        }
						if (lastCodeCompleteData.c.notNull())
                        {
							var classDescription = lastCodeCompleteData.c.str().remove("[DefaultClass: ").removeLastChar().line();
							foreach (var method in lastCodeCompleteData.c.Methods)
                                classDescription += " - {0}".info(method.str()).line();
                            declarationViewWindow.Description = (declarationViewWindow.Description + classDescription).trim();
                        }
                    }
                    SetDeclarationViewLocation();
                }
                else
                {
                    declarationViewWindow.Description = null;
                }
            }
		}
		
		public override bool ProcessKeyEvent(char ch)
		{
            //"[CodeCompletionWindow] ProcessKeyEvent: {0}".info(ch);
			switch (dataProvider.ProcessKey(ch)) {
				case CompletionDataProviderKeyResult.BeforeStartKey:
					// increment start+end, then process as normal char
					++startOffset;
					++endOffset;
					return base.ProcessKeyEvent(ch);
				case CompletionDataProviderKeyResult.NormalKey:
					// just process normally
					return base.ProcessKeyEvent(ch);
				case CompletionDataProviderKeyResult.InsertionKey:
					return InsertSelectedItem(ch);
				default:
					throw new InvalidOperationException("Invalid return value of dataProvider.ProcessKey");
			}
		}
		
		void DocumentAboutToBeChanged(object sender, DocumentEventArgs e)
		{
			// => startOffset test required so that this startOffset/endOffset are not incremented again
			//    for BeforeStartKey characters
			if (e.Offset >= startOffset && e.Offset <= endOffset) {
				if (e.Length > 0) { // length of removed region
					endOffset -= e.Length;
				}
				if (!string.IsNullOrEmpty(e.Text)) {
					endOffset += e.Text.Length;
				}
			}
		}
		
		/// <summary>
		/// When this flag is set, code completion closes if the caret moves to the
		/// beginning of the allowed range. This is useful in Ctrl+Space and "complete when typing",
		/// but not in dot-completion.
		/// </summary>
		public bool CloseWhenCaretAtBeginning { get; set; }
		
		public string TextSoFar
		{
			get
			{
				int offset = control.ActiveTextAreaControl.Caret.Offset - startOffset;				
				return (offset> 0) 
							? control.Document.GetText(startOffset, offset)
							: "";
			}
		}
		protected override void CaretOffsetChanged(object sender, EventArgs e)
		{            
			int offset = control.ActiveTextAreaControl.Caret.Offset;

            if (guiLoaded.isFalse()) //DC, means the Window is not loaded (i.e first pass)
                return;

			if (offset == startOffset) {
				if (CloseWhenCaretAtBeginning)
					Close();
				return;
			}
			if (offset < startOffset || offset > endOffset) {
				Close();
			} else
			{
				ShowEntriesThatMatchText(this.TextSoFar);
			}
		}

		public void ShowEntriesThatMatchText(string text)
		{
			if (text.contains("d1"))
			{
				
				"aa".o2Cache(this);
				//this.codeCompletionListView.insert_Below(20);
			}
			codeCompletionListView.pink();
			//if (text.contains("d"))
			if (text.valid())
			{
				var lowerText = text.lower();
				
				var newCompletionData = new List<ICompletionData>();
				foreach (var item in completionData)
					if (item.Text.lower().contains(lowerText))
						newCompletionData.add(item);
				//completionData.Take(20).ToArray();

				this.codeCompletionListView.completionData = newCompletionData.ToArray() ;
				this.vScrollBar.Maximum = newCompletionData.size();
				this.codeCompletionListView.ClearSelection();
				this.codeCompletionListView.Refresh();
				this.Refresh();
			}
			else
			{
				this.vScrollBar.Maximum = completionData.size();
				this.codeCompletionListView.completionData = completionData;
			}
			
			codeCompletionListView.SelectItemWithStart(text);
		}

		void codeCompletionListView_KeyUp(object sender, KeyEventArgs e)
		{
			"Keyup: {0}".info(e.KeyCode);
			//ProcessTextAreaKey(e.KeyCode);
		}		
		void codeCompletionListView_KeyPress(object sender, KeyPressEventArgs e)
		{
			//ProcessTextAreaKey(e.KeyChar);
		}

		protected override bool ProcessTextAreaKey(Keys keyData)
		{
			if (keyData == Keys.Back)
			{
				return false;			//DC: don't process backspace
			}
            if (!Visible || codeCompletionListView == null)
            {
				return false;
			}
			
			switch (keyData) {
				case Keys.Home:
					codeCompletionListView.SelectIndex(0);
					return true;
				case Keys.End:
					codeCompletionListView.SelectIndex(completionData.Length-1);
					return true;
				case Keys.PageDown:
					codeCompletionListView.PageDown();
					return true;
				case Keys.PageUp:
					codeCompletionListView.PageUp();
					return true;
				case Keys.Down:
					codeCompletionListView.SelectNextItem();
					return true;
				case Keys.Up:
					codeCompletionListView.SelectPrevItem();
					return true;
				case Keys.Tab:
					InsertSelectedItem('\t');
					return true;
				case Keys.Return:
					InsertSelectedItem('\n');
					return true;
			}
			this.AfterKeyPress.invoke(keyData);
			return base.ProcessTextAreaKey(keyData);
		}
		
		void CodeCompletionListViewDoubleClick(object sender, EventArgs e)
		{
			InsertSelectedItem('\0');
		}
		
		void CodeCompletionListViewClick(object sender, EventArgs e)
		{
			O2Thread.mtaThread(() => this.focus()); // handle weird case where the focus went to the ScrollBar 

			//control.ActiveTextAreaControl.TextArea.Focus();
		}
		
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				document.DocumentAboutToBeChanged -= DocumentAboutToBeChanged;
				if (codeCompletionListView != null) {
					codeCompletionListView.Dispose();
					codeCompletionListView = null;
				}
				if (declarationViewWindow != null) {
					declarationViewWindow.Dispose();
					declarationViewWindow = null;
				}
			}
			base.Dispose(disposing);
		}
		
		bool InsertSelectedItem(char ch)
		{
			document.DocumentAboutToBeChanged -= DocumentAboutToBeChanged;
			ICompletionData data = (codeCompletionListView!=null) ? codeCompletionListView.SelectedCompletionData : null;
			bool result = false;
			if (data != null) {
				control.BeginUpdate();
				
				try {
					if (endOffset - startOffset > 0) {
						control.Document.Remove(startOffset, endOffset - startOffset);
					}
					Debug.Assert(startOffset <= document.TextLength);
					result = dataProvider.InsertAction(data, control.ActiveTextAreaControl.TextArea, startOffset, ch);
				} finally {
					control.EndUpdate();
				}
			}
			Close();
			return result;
		}
		
		Size GetListViewSize()
		{
			int height = codeCompletionListView.ItemHeight * Math.Min(MaxListLength, completionData.Length);
			int width = codeCompletionListView.ItemHeight * 10;
			if (!fixedListViewWidth) {
				width = GetListViewWidth(width, height);
			}
			return new Size(width, height);
		}
		
		/// <summary>
		/// Gets the list view width large enough to handle the longest completion data
		/// text string.
		/// </summary>
		/// <param name="defaultWidth">The default width of the list view.</param>
		/// <param name="height">The height of the list view.  This is
		/// used to determine if the scrollbar is visible.</param>
		/// <returns>The list view width to accommodate the longest completion
		/// data text string; otherwise the default width.</returns>
		int GetListViewWidth(int defaultWidth, int height)
		{
			float width = defaultWidth;
			using (Graphics graphics = codeCompletionListView.CreateGraphics()) {
				for (int i = 0; i < completionData.Length; ++i) {
					float itemWidth = graphics.MeasureString(completionData[i].Text.ToString(), codeCompletionListView.Font).Width;
					if(itemWidth > width) {
						width = itemWidth;
					}
				}
			}
			
			float totalItemsHeight = codeCompletionListView.ItemHeight * completionData.Length;
			if (totalItemsHeight > height) {
				width += ScrollbarWidth; // Compensate for scroll bar.
			}
			return (int)width;
		}

		//
		public void addToolStrip()
		{
		//	this.insert_Below(20).add_ToolStrip();
		}
	}

}
