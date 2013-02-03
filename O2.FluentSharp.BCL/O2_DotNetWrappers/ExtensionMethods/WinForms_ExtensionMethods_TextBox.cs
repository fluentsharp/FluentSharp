using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using O2.DotNetWrappers.ExtensionMethods;
using System.Drawing;
using System.Text.RegularExpressions;
//using O2.DotNetWrappers.DotNet;

namespace O2.DotNetWrappers.ExtensionMethods
{
    public static class WinForms_ExtensionMethods_TextBox
    {
        public static TextBox   add_TextArea(this Control control)
        {
            return control.add_TextBox(true)
                          .unlimitedSize();
        }
        public static TextBox   add_TextBox(this Control control)
        {
            return control.add_TextBox(false);
        }
        public static TextBox   add_TextBox(this Control control, bool multiLine)
        {
            return control.add_TextBox(-1, -1, multiLine);
        }
        public static TextBox   add_TextBox(this Control control,int top, int left, bool multiLine)
        {
            return (TextBox) control.invokeOnThread(
                                 () =>
                                     {
                                         var textBox = new TextBox();
                                         if (multiLine)
                                         {
                                             textBox.Dock = DockStyle.Fill;
                                             textBox.Multiline = true;
                                             textBox.ScrollBars = ScrollBars.Both;
                                         }
                                         if (top > 0)
                                             textBox.Top = top;
                                         if (left > 0)
                                             textBox.Left = left;
                                         control.Controls.Add(textBox);
                                         return textBox;
                                     });
        }
        public static TextBox   add_TextBox(this Control control, int top, int left)
        {
            return control.add_TextBox(top, left, -1, -1);
        }
        public static TextBox   add_TextBox(this Control control, int top, int left, int width)
        {
            return control.add_TextBox(top, left, width, -1);
        }
        public static TextBox   add_TextBox(this Control control, int top, int left, int width, int height)
        {
            var textBox = control.add_TextBox(false);
            textBox.top(top);
            textBox.left(left);
            textBox.width(width);
            if (height > -1)
            {
                textBox.multiLine(true);
                textBox.height(height);
            }
            return textBox;
        }
        public static TextBox   add_TextBox(this Control control, string labelText, string defaultTextBoxText)
        {
            return control.add_Label(labelText).top(3)
                          .append_TextBox(defaultTextBoxText)
                          .align_Right(control); ;
        }
        public static TextBox   add_TextBox(this Control control, int top, string labelText, string defaultTextBoxText)
        {
            return control.add_Label(labelText).top(top + 3)
                          .append_TextBox(defaultTextBoxText)
                          .align_Right(control); ;
        }
        public static TextBox   append_TextBox(this Control control, string text)
        {
            return control.append_TextBox(text, -1, -1);
        }
        public static TextBox   append_TextBox(this Control control, int width)
        {
            return control.append_TextBox("", width, -1);
        }
        public static TextBox   append_TextBox(this Control control, string text, int width)
        {
            return control.append_TextBox(text, width, -1);
        }
        public static TextBox   append_TextBox(this Control control, string text, int width, int height)
        {
            var textBox = control.Parent.add_TextBox(control.Top - 3, control.Left + control.Width + 5, width, height);
            textBox.set_Text(text);
            return textBox;
        }
		public static TextBox	append_TextBox(this Control hostControl, ref TextBox textBox)
		{
			return textBox = hostControl.append_TextBox("");
		}
        public static TreeNode  select(this TreeView treeView, string text)
        {
            foreach (var treeNode in treeView.nodes())
                if (treeNode.get_Text() == text)
                    return treeNode.selected();
            return null;
        }
        public static TextBox   select(this TextBox textBox, int start, int length)
        {
            textBox.invokeOnThread(() => textBox.Select(start, length));
            return textBox;
        }
        public static TextBox   set_Text(this TextBox textBox, string text)
        {
            textBox.invokeOnThread(
                () =>
                {
                    textBox.SuspendLayout();
                    textBox.Text = text;
                    textBox.ResumeLayout();
                });
            return textBox;
        }
        public static string    get_Text(this TextBox textBox)
        {
            return (string)textBox.invokeOnThread(
                () =>
                {
                    return textBox.Text;
                });
        }
        public static string    contents(this TextBox textBox)
        {
            return textBox.get_Text();
        }
        public static TextBox   contents(this TextBox textBox, string text)
        {
            return textBox.set_Text(text);
        }
        public static TextBox   insertText(this TextBox textBox, string textToInsert)
        {
            return (TextBox)textBox.invokeOnThread(
                () =>
                {
                    textBox.SelectionLength = 0;
                    textBox.SelectedText = textToInsert;
                    return textBox;
                });
        }
        public static TextBox   replaceText(this TextBox textBox, string textToFind, string textToInsert)
        {
            return (TextBox)textBox.invokeOnThread(
                () =>
                {
                    var selectionStart = textBox.SelectionStart;
                    textBox.Text = textBox.Text.Replace(textToFind, textToInsert);
                    textBox.SelectionStart = selectionStart;
                    return textBox;
                });
        }
        public static TextBox   append_Line(this TextBox textBox, string textFormat, params object[] parameters)
        {
            textBox.append_Line(textFormat.line().format(parameters));
            return textBox;
        }
        public static TextBox   append_Line(this TextBox textBox, string text)
        {
            return textBox.append_Text(text.line());            
        }
        public static TextBox   append_Text(this TextBox textBox, string text)
        {
            return textBox.invokeOnThread(
                () =>
                    {
                        textBox.Text += text;
                        textBox.goToEnd();
                        return textBox;
                    });
        }
        public static TextBox insert_Line(this TextBox textBox, string text)
        {
            return textBox.insert_Text(text.line());
        }
        public static TextBox insert_Text(this TextBox textBox, string text)
        {
            return textBox.invokeOnThread(
                () =>
                {
                    textBox.Text = text + textBox.Text;
                    textBox.goToStart();
                    return textBox;
                });
        }
        public static TextBox   goToEnd(this TextBox textBox)
        {
            return textBox.invokeOnThread(() =>
                                       {
                                           textBox.Select(textBox.Text.Length, 0);
                                           textBox.ScrollToCaret();
                                           return textBox;
                                       });
        }
        public static TextBox goToStart(this TextBox textBox)
        {
            return textBox.invokeOnThread(() =>
            {
                textBox.Select(0, 0);
                textBox.ScrollToCaret();
                return textBox;
            });
        }
        public static TextBox   onTextChange(this TextBox textBox, Action<string> callback)
        {
            return (TextBox)textBox.invokeOnThread(
                () =>
                {
                    textBox.TextChanged += (sender, e) => callback(textBox.Text);
                    return textBox;
                });
        }        
        public static TextBox   onTextChange_AlertOnRegExFail(this TextBox textBox)
        {
            textBox.onTextChange((text) =>
            {
                textBox.backColor(text.regExOk()
                                        ? Color.White
                                        : Color.Red
                                  );
            });
            return textBox;

        }
        public static TextBox   multiLine(this TextBox textBox)
        {
            return textBox.multiLine(true);
        }
        public static TextBox   multiLine(this TextBox textBox, bool value)
        {
            return (TextBox)textBox.invokeOnThread(() =>
            {
                textBox.Multiline = value;
                return textBox;
            });
        }
        public static TextBox   wordWrap(this TextBox textBox, bool value)
        {
            return (TextBox)textBox.invokeOnThread(() =>
            {
                textBox.WordWrap = value;
                return textBox;
            });
        }
        public static TextBox   scrollBars(this TextBox textBox)
        {
            return (TextBox)textBox.invokeOnThread(() =>
            {
                textBox.ScrollBars = ScrollBars.Both;
                return textBox;
            });
        }
        public static TextBox   allowTabs(this TextBox textBox)
        {
            return textBox.acceptsTab();
        }
        public static TextBox   acceptsTab(this TextBox textBox)
        {
            return textBox.acceptsTab(true);
        }
        public static TextBox   acceptsTab(this TextBox textBox, bool value)
        {
            textBox.invokeOnThread(() => textBox.AcceptsTab = value);
            return textBox;
        }
        public static TextBox   isPasswordField(this TextBox textBox)
        {
            textBox.invokeOnThread(() => textBox.PasswordChar = '*');
            return textBox;
        }

        public static TextBox   add_TextBox(this Control control, string text)
		{
			var textBox = control.add_TextBox();
			textBox.set_Text(text);
			return textBox;
		}		
		public static TextBox   add_TextArea(this Control control, string text)
		{
			var textBox = control.add_TextArea();
			textBox.set_Text(text);
			return textBox;
		}		
		public static TextBox   add_TextArea_With_SearchPanel(this Control control)
		{
			return control.add_TextBox(true).insert_Below_RegEx_SearchPanel();
		}				
		public static TextBox   scrollToSelection(this TextBox textBox)
		{
			return textBox.scrollToCaret();
		}		
		public static TextBox   scrollToCaret(this TextBox textBox)
		{
			return (TextBox)textBox.invokeOnThread(
				()=>{
						textBox.ScrollToCaret();
						return textBox;
					});
		}		
		public static TextBox   showSelection(this TextBox textBox, bool value = true)
		{
			return textBox.hideSelection(!value);
		}		
		public static TextBox   hideSelection(this TextBox textBox, bool value = true)
		{
			return (TextBox)textBox.invokeOnThread(
				()=>{
						textBox.HideSelection = value;
						return textBox;
					});
		}		
		public static TextBox   selectionLength(this TextBox textBox, int value)
		{
			return (TextBox)textBox.invokeOnThread(
				()=>{
						textBox.SelectionLength = value;
						return textBox;
					});
		}		
		public static TextBox   selectionStart(this TextBox textBox, int value)
		{
			return (TextBox)textBox.invokeOnThread(
				()=>{
						textBox.SelectionStart = value;
						return textBox;
					});
		}				
		public static TextBox   insert_Below_RegEx_SearchPanel(this TextBox targetTextBox)
		{
			//var targetTextBox 			= textBox.showSelection(); 
			targetTextBox.showSelection();
			var searchPanel 			= targetTextBox.insert_Below(45, "Search for: (RegEx)");
			var searchText_TextBox 		= searchPanel.add_TextBox();
			var searchResults_ComboBox 	= searchText_TextBox.append_Control<ComboBox>().dropDownList();
			var searchResults_Label 	= searchResults_ComboBox.append_Label("...").autoSize().top(3);
			
			Func<string> textToSearch = ()=> targetTextBox.get_Text();
			
			Action<string> runSearch = 
				(matchPattern)=>{	
									targetTextBox.selectionLength(0);
									searchResults_ComboBox.clear();
									if (matchPattern.valid())
									{
										var matches = textToSearch().regEx_Matches(matchPattern);						 						
										searchResults_ComboBox.add_Items(matches.ToArray());
										if (matches.size()>0)
											searchResults_ComboBox.select_Item(0);
									}
									searchResults_Label.set_Text("{0} results".format(searchResults_ComboBox.items().size()));									
								 };
			
			searchResults_ComboBox.onSelection<Match>(
				(match)=>{
							targetTextBox.select(match.Index, match.Length)
										 .scrollToSelection();
						 });
			 
			searchText_TextBox.onTextChange(runSearch); 
			return targetTextBox;
		}

        public static TextBox   unlimitedSize(this TextBox textBox)
        {
            return textBox.maxLength(0);
        }
        public static TextBox   noMaxLength(this TextBox textBox)
        {
            return textBox.maxLength(0);
        }
        public static TextBox   maxLength(this TextBox textBox, int value)
        {
            return textBox.invokeOnThread(() => { textBox.MaxLength = value; return textBox; });
        }

        public static TextBox   show_in_TextArea(this string text)
        {
            return "Viewing String with size: {0}".format(text.size())
                        .popupWindow()
                        .add_TextArea()
                        .set_Text(text.fixCRLF());
        }
    }
}
