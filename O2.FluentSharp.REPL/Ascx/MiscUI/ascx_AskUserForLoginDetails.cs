// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Text;
using O2.Kernel;
using O2.Kernel.ExtensionMethods;
using O2.DotNetWrappers.ExtensionMethods;
using O2.Views.ASCX.ExtensionMethods;
using O2.Views.ASCX.classes.MainGUI;
using O2.XRules.Database.Utils.O2;
//O2File:ISecretData.cs
//O2Ref:O2_External_IE.dll

namespace O2.XRules.Database.Utils
{
    public class ascx_AskUserForLoginDetails : ContainerControl
    {       
		public string UserName { get;set;}
		public string Password { get;set;}
 
		public AutoResetEvent HaveAnswer { get;set;}
        public TextBox UserNameTextBox { get; set;}
        public TextBox PasswordTextBox { get; set;}
        public Button OKButton { get; set;}
 
		public static ICredential ask()
    	{
    		var loginDetailsGui = O2Gui.open<ascx_AskUserForLoginDetails>("Enter Login Details", 250,115);    	
    		loginDetailsGui.buildGui();    		   	
    		var credential = loginDetailsGui.getAnswer();    		
    		loginDetailsGui.close();
 
    		return credential;
    	}    	   	   	   	   
 
        public ascx_AskUserForLoginDetails()
    	{
    		HaveAnswer = new AutoResetEvent(false);
    		UserName = "";
    		Password = "";
    	}
 
    	public void buildGui()
    	{    								
			UserNameTextBox = this.add_Label("Username:",10,0)
							      .append_TextBox("");
 
			UserNameTextBox.onTextChange((text)=> UserName = text)
						   .align_Right(this);
 
			PasswordTextBox = this.add_Label("Password: ",35,0)
								  .append_TextBox("")
								  .isPasswordField();
			PasswordTextBox.onTextChange((text)=> Password = text)
						   .align_Right(this);
 
			var OKButton = this.add_Button("OK", 60,0);							  
			OKButton.onClick(answerAvailable)
					.left(this.width() - OKButton.width() - 1)
					.anchor_BottomRight();
 
			this.parentForm().Closed += (sender,e) => answerAvailable();			
    	}
 
    	public ICredential getAnswer()
    	{    		
    		HaveAnswer.WaitOne();      	
    		
    		var credential = new Credential();
    		credential.UserName = UserName;
    		credential.Password = Password;
    		//"u:{0}".debug(credential.UserName);
    		//"p:{0}".debug(credential.Password);
    		return credential;
    	}
 
    	public void answerAvailable()
    	{	   	    		
    		HaveAnswer.Set();
    	}
 
    	public void close()
    	{
    		if (this.parentForm() != null)
				this.parentForm().close();     		
    	}
 
     }
 
}
