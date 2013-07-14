// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.WinForms.Controls;

namespace FluentSharp.WinForms.Utils
{
	public class API_ConsoleOut_Test
	{
		public void launch()
		{
			//new API_ConsoleOut().show_ConsoleOut_TestGui();
			"Testing O2 Console".show_ConsoleOut();
			Console.WriteLine("this is a test");
			Console.Write("Another ");
			Console.Write("test");
			
			for(int i = 0 ; i< 50 ; i++)
			{
				var newText = "asdas".add_RandomLetters(100.random());					
				foreach(var _char in newText)				
					Console.Write(_char);
				Console.WriteLine();
			};
			Console.WriteLine("");
			Console.WriteLine("All done :)");
		}
	}
    public class API_ConsoleOut : Control 
    {         	
    	public O2StreamWriter   o2StreamWriter;
    	public RichTextBox 		RichTextBox;
    	
    	public API_ConsoleOut()
    	{    	
			this.o2StreamWriter = new O2StreamWriter(); 	 
			this.o2StreamWriter.Name = "API_ConsoleOut";
			Console.SetOut(this.o2StreamWriter);  
		}
    }
    
    public class O2StreamWriter : TextWriter
	{
		public StringBuilder ConsoleOut 	{ get; set; }
		public StringBuilder CurrentLine	{ get; set; }
		public List<string> Lines			{ get; set; }
		public Action<char> On_NewChar		{ get; set; }	
		public Action<string> On_NewLine 	{ get; set; }
		public bool LogAllChars				{ get; set; }
		public bool LogAllLines				{ get; set; }
		public string Name					{ get; set; }
		public int PauseBetweenCharWrite	{ get; set; }				
		public O2StreamWriter()
		{
			ConsoleOut = new StringBuilder();
			CurrentLine = new StringBuilder();
			Lines = new List<string>();		
			LogAllChars = false; 
			LogAllLines = true;
			Name = "O2StreamWriter";
		}	
		public override void Write(char value)
        {
            base.Write(value);
          	this.ConsoleOut.Append(value.ToString());          	
          	
          	if (value == '\n')
          		handleLine();          		
          	else
          		this.CurrentLine.Append(value.ToString());
          		
          	if (this.LogAllChars)
            	"[{0}] char: {1}".info(this.Name, value);
            
            if (this.On_NewChar.notNull())
            	this.On_NewChar(value);
            
            if (PauseBetweenCharWrite > 0)
            	this.sleep(PauseBetweenCharWrite, false);
        }                		
		public void handleLine()
		{
			var line = this.CurrentLine.str();
			if (line.lastChar('\r'))
				line = line.removeLastChar();
			this.CurrentLine = new StringBuilder();
			this.Lines.add(line);
			
			if (this.LogAllLines)
            	"[{0}]: {1}".info(this.Name, line);
            	
			if (this.On_NewLine.notNull())
				this.On_NewLine(line);			
		}
        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
	}

	public static class API_ConsoleOut_ExtensionMethods
	{

		public static API_ConsoleOut	write(this API_ConsoleOut apiConsoleOut, string message)
		{
			Console.Write(message);
			return apiConsoleOut;
		}
		public static API_ConsoleOut	writeLine(this API_ConsoleOut apiConsoleOut)
		{
			return apiConsoleOut.writeLine("");
		}
		public static API_ConsoleOut	writeLine(this API_ConsoleOut apiConsoleOut, string message)
		{
			Console.WriteLine(message);
			return apiConsoleOut;
		}
		public static API_ConsoleOut	clear(this API_ConsoleOut apiConsoleOut, string message)
		{
			if (apiConsoleOut.RichTextBox.isNull())
				"[API_ConsoleOut] in clean() richTextBox was not set".error();
			else
				apiConsoleOut.RichTextBox.set_Text("");
			return apiConsoleOut;
		}
		public static string			console_WriteLine(this string text)
		{
			Console.WriteLine(text);
			return text;
		}
		public static T					console_WriteLine<T>(this T _object, string text)
		{
			Console.WriteLine(text);
			return _object;
		}
		public static T					console_WriteLine<T>(this T _object, Func<string> getTextToOutput)
		{
			Console.WriteLine(getTextToOutput.invoke());
			return _object;
		}
	}
    public static class API_ConsoleOut_ExtensionMethods_TestGuis
    {    	
    	public static API_ConsoleOut show_ConsoleOut_TestGui(this API_ConsoleOut apiConsoleOut)
    	{
    		var topPanel = O2Gui.open<Panel>("Console.Out capture Test",700,200); 
    		return apiConsoleOut.show_ConsoleOut_TestGui(topPanel);
    	}    	    	
    	public static API_ConsoleOut show_ConsoleOut_TestGui(this API_ConsoleOut apiConsoleOut, Control topPanel)
    	{
			var consoleIn = topPanel.insert_Above(40, "ConsoleIn");
			var lines_TextBox = topPanel.clear().add_GroupBox("Console.Out Lines").add_TreeView();
			var chars_TextBox = topPanel.insert_Right("Console.Out Chars")
										.add_RichTextBox()
										.showSelection()
										.wordWrap(false);
						
			consoleIn.add_TextBox("Write to send to Console Input ->","")
					 .onKeyPress_getChar( (_char)=>  Console.Write(_char))
					 .multiLine(true)
					 .focus();
					 			
			apiConsoleOut.o2StreamWriter.On_NewChar = (_char) => { if (_char != '\x0a') chars_TextBox.append_Text(_char.str()); };
			apiConsoleOut.o2StreamWriter.On_NewLine = (line) => lines_TextBox.add_Node(line); 
			
			Console.WriteLine("Welcome to show_ConsoleOut_TestGui");    
			Console.Write("Type something on the TextBox above to see it on the rigth".line());
			Console.WriteLine("Current Process: " + Processes.getCurrentProcess().Id);
			return apiConsoleOut;
		}		
		public static API_ConsoleOut open_ConsoleOut(this string message)
		{
			return message.show_ConsoleOut();
		}		
		public static API_ConsoleOut show_ConsoleOut	(this string message)
		{
			var apiConsoleOut = new API_ConsoleOut();
			apiConsoleOut.show_ConsoleOut();
			Console.WriteLine(message);
			return apiConsoleOut;
		}
		public static API_ConsoleOut insert_ConsoleOut	(this Control topPanel)
		{ 
			return topPanel.insert_ConsoleOut(true);
		}
		public static API_ConsoleOut insert_ConsoleOut	(this Control topPanel,  bool showO2Message)
		{
			return topPanel.insert_Below(100).add_ConsoleOut(showO2Message);
		}
		public static API_ConsoleOut add_ConsoleOut		(this Control topPanel) 
		{ 
			return topPanel.add_ConsoleOut(true); 
		}
		public static API_ConsoleOut add_ConsoleOut		(this Control topPanel,  bool showO2Message)
		{
			return topPanel.show_ConsoleOut(showO2Message);
		}
		public static API_ConsoleOut show_ConsoleOut<T>	(this T topPanel)					  where T : Control 
		{ 
			return topPanel.show_ConsoleOut(true); 
		}
		public static API_ConsoleOut show_ConsoleOut<T>	(this T topPanel, bool showO2Message) where T : Control
		{
			return topPanel.show_ConsoleOut(showO2Message ? "ConsoleOut for current process (Console WriteLine will show here)" : "");
		}		
		public static API_ConsoleOut show_ConsoleOut<T>	(this T topPanel, string message ) where T : Control
		{
			var apiConsoleOut = new API_ConsoleOut();
			apiConsoleOut.show_ConsoleOut(topPanel);
			if (message.valid())
				Console.WriteLine(message);
			return apiConsoleOut;
		}		
		public static API_ConsoleOut show_ConsoleOut	(this API_ConsoleOut apiConsoleOut)
    	{
    		var topPanel = O2Gui.open<Panel>("Console.Out",700,200); 
    		return apiConsoleOut.show_ConsoleOut(topPanel);
    	}    	    
    	public static API_ConsoleOut show_ConsoleOut	(this API_ConsoleOut apiConsoleOut, Control topPanel)
    	{    		
    		var richTextBox = apiConsoleOut.RichTextBox = topPanel.add_RichTextBox().showSelection().wordWrap(false);
    		var showData = true;
			richTextBox.backColor(Color.Black)
					   .foreColor(Color.White)
					   .font("Lucida Console");
			apiConsoleOut.o2StreamWriter.On_NewChar = (_char) => 
				{ 										
					if (showData)
						if (_char != '\x0d') 
							richTextBox.append_Text(_char.str()); 																
					
				};
			apiConsoleOut.o2StreamWriter.LogAllLines = false;
			
			richTextBox.add_ContextMenu()
					   .add_MenuItem("Clear", true, ()=> richTextBox.set_Text(""))
					   .add_MenuItem("Write to Console Input",true, ()=> Console.WriteLine("Text to send to Console.Input".askUser()))
					   .add_MenuItem("'Show' / 'Don't Show' received data", true, ()=> showData = !showData);
					   
			return apiConsoleOut;
    	}
    }
}