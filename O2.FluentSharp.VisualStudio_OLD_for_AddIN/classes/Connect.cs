using System;
using Extensibility;
using EnvDTE;
using EnvDTE80;
using O2.DotNetWrappers.ExtensionMethods; 
using O2.FluentSharp.VisualStudio.ExtensionMethods;
using System.Diagnostics;
using System.Windows.Forms;

//O2File:O2_VS_AddIn.cs

//O2Ref:EnvDTE.dll
//O2Ref:EnvDTE80.dll
//O2Ref:Extensibility.dll
//O2Ref:Microsoft.VisualStudio.CommandBars.dll

namespace O2.FluentSharp.VisualStudio
{
	/// <summary>The object for implementing an Add-in.</summary>
	/// <seealso class='IDTExtensibility2' />
	public class Connect : IDTExtensibility2, IDTCommandTarget
	{

		public static O2_VS_AddIn VS_AddIn	{ get; set; }
		
		/// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
		/// <param term='application'>Root object of the host application.</param>
		/// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
		/// <param term='addInInst'>Object representing this Add-in.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
		{			
            //Debug.WriteLine("This is a test!!!!!!!!!!!");  
            //MessageBox.Show("This was dynamically changed....!!!AAasd");

            //MessageBox.Show("connectMode...:"  + connectMode.str());
            //if (connectMode.uiSetUp())			
            if (VS_AddIn == null)
			{
                MessageBox.Show("in uiSetUp*__");
                var type = "O2.VisualStudio.Connect"; //this.typeFullName()
				VS_AddIn = new O2_VS_AddIn().setup((DTE2)application, (AddIn)addInInst, type);
				if (VS_AddIn.isNull())
					MessageBox.Show("VS_AddIn was null, something is wrong");
			}
		}

        public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
        {
        }

		#region not_implemented_methods	
		/// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of Add-ins has changed.</summary>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />		
		public void OnAddInsUpdate(ref Array custom)
		{
		}

		/// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnStartupComplete(ref Array custom)
		{
		}

		/// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnBeginShutdown(ref Array custom)
		{
		}
		#endregion 

		/// <summary>Implements the QueryStatus method of the IDTCommandTarget interface. This is called when the command's availability is updated</summary>
		/// <param term='commandName'>The name of the command to determine state for.</param>
		/// <param term='neededText'>Text that is needed for the command.</param>
		/// <param term='status'>The state of the command in the user interface.</param>
		/// <param term='commandText'>Text requested by the neededText parameter.</param>
		/// <seealso class='Exec' />
		public void QueryStatus(string commandName, vsCommandStatusTextWanted neededText, ref vsCommandStatus status, ref object commandText)
		{			
			if (VS_AddIn.notNull())
				if(neededText == vsCommandStatusTextWanted.vsCommandStatusTextWantedNone)
				{				
					if (VS_AddIn.showCommand(commandName))
					{
						status = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported|vsCommandStatus.vsCommandStatusEnabled;
						return;
					}
				}
		}

		/// <summary>Implements the Exec method of the IDTCommandTarget interface. This is called when the command is invoked.</summary>
		/// <param term='commandName'>The name of the command to execute.</param>
		/// <param term='executeOption'>Describes how the command should be run.</param>
		/// <param term='varIn'>Parameters passed from the caller to the command handler.</param>
		/// <param term='varOut'>Parameters passed from the command handler to the caller.</param>
		/// <param term='handled'>Informs the caller if the command was handled or not.</param>
		/// <seealso class='Exec' />
		public void Exec(string commandName, vsCommandExecOption executeOption, ref object varIn, ref object varOut, ref bool handled)
		{			
			handled = false;
			if (VS_AddIn.notNull())
				if(executeOption == vsCommandExecOption.vsCommandExecOptionDoDefault)
				{
					handled = VS_AddIn.executeCommand(commandName);
				}
		}
		
	}
}