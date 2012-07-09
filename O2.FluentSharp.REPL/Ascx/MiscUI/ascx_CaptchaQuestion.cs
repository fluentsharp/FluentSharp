// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Text;
using O2.Kernel;
using O2.DotNetWrappers.ExtensionMethods;
using O2.Views.ASCX.ExtensionMethods;
using O2.Views.ASCX.classes.MainGUI;
//O2Ref:O2_External_IE.dll
 
namespace O2.XRules.Database.Utils
{
    public class ascx_CaptchaQuestion : ContainerControl
    {       
		public string CaptchaQuestionUrl { get;set;}
		public string CaptchaAnswer { get;set;}		
		public AutoResetEvent HaveAnswer { get;set;}
        public TextBox AnswerTextBox { get; set;}
        public ascx_CaptchaQuestion()
    	{
    		HaveAnswer = new AutoResetEvent(false);
    		CaptchaAnswer = "";    		
    	}
 
    	public void buildGui(string captchaQuestionUrl)
    	{
    		CaptchaQuestionUrl =  captchaQuestionUrl;    		   							
 
			var browser = this.add_WebBrowser_Control(); 
 
			var panelAbove = browser.insert_Above<Panel>(25);
			var label = panelAbove.add_Label("What does the CAPTCHA  say?").top(6);
			var button = panelAbove.add_Button("Submit");
			button.left(panelAbove.width()-button.width())
				  .anchor_TopRight();
 
			AnswerTextBox = label.append_TextBox("");
			AnswerTextBox.onTextChange((text)=>CaptchaAnswer = text)
						 .onEnter((text) => answerAvailable())
					     .width(panelAbove.width() -button.width() - label.width() - 10)
						 .anchor_TopLeftRight()												
						 .top(2);
 
 
			button.onClick(() => answerAvailable());
 
			browser.open(CaptchaQuestionUrl);
 
			this.parentForm().Closed += (sender,e) => answerAvailable();			
    	}
 
    	public string getAnswer()
    	{
    		HaveAnswer.WaitOne();    		
    		return AnswerTextBox.get_Text();    		
    	}
 
    	public void answerAvailable()
    	{	
    		"answerAvailable".debug();
    		HaveAnswer.Set();
    	}
 
    	public void close()
    	{
    		if (this.parentForm() != null)
				this.parentForm().Close();     		
    	}
 
 
    	public static string askQuestion(string captchaQuestionUrl)
    	{
    		var captchaQuestion = O2Gui.open<ascx_CaptchaQuestion>("Answer CAPTCHA Question", 400,200);
    		captchaQuestion.buildGui(captchaQuestionUrl);    		
 
    		var captchaAnswer = captchaQuestion.getAnswer();
    		captchaQuestion.close();
 
    		return captchaAnswer;
    	}    	   	   	   	   
     }
 
}
