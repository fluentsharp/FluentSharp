// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Windows.Forms;
using System.Diagnostics;
using O2.Kernel;
using O2.Kernel.ExtensionMethods;
using O2.DotNetWrappers.ExtensionMethods;
using O2.Views.ASCX.classes.MainGUI;

namespace O2.Views.ASCX.Ascx.MainGUI
{
    public partial class ascx_LogViewer : UserControl
    {
        public bool loadConfigExecuted;

        public ascx_LogViewer()
        {
            InitializeComponent();
            if (false == DesignMode)
            {
				if (PublicDI.log.LogRedirectionTarget == null)
					PublicDI.log.LogRedirectionTarget = new WinFormsUILog();

                DebugMsg.setRtbObject(rtbDebugMessages);

                cbErrorMessages.Checked = true;
                cbDebugMessages.Checked = true;
                cbInfoMessages.Checked = true;
                //DI.log.error("Testing logging: Error Message");
                DI.log.debug("Testing logging: Debug Message");
                DI.log.info("Testing logging: Info Message");                  
            }
            //  this.Location = new Point(20,500);            
        }


        private void btAddNewLineToDebugWindiw_Click(object sender, EventArgs e)
        {
            DI.log.info("");
        }



        private void btClearDebugView_Click(object sender, EventArgs e)
        {
            DebugMsg.clearText();
        }

        private void rtbDebugMessages_TextChanged(object sender, EventArgs e)
        {
        }

        private void cbErrorMessages_CheckedChanged(object sender, EventArgs e)
        {
            DebugMsg.bShowError = cbErrorMessages.Checked;
        }

        private void cbDebugMessages_CheckedChanged(object sender, EventArgs e)
        {
            DebugMsg.bShowDebug = cbDebugMessages.Checked;
        }

        private void cbInfoMessages_CheckedChanged(object sender, EventArgs e)
        {
            DebugMsg.bShowInfo = cbInfoMessages.Checked;
        }

        private void llGCCollect_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GC.Collect();
        }

        private void ascx_LogViewer_Load(object sender, EventArgs e)
        {
            if (false == DesignMode)
            {
                if (false == loadConfigExecuted)
                {
                    if (ParentForm != null)
                        ParentForm.Closing += delegate
                                                  {
                                                      DebugMsg.removeRtbObject(rtbDebugMessages);
                                                  };
                    else
                        DI.log.error("in ascx_LogViewer_Load, there was not parent so .Closing event could not be set");
                    loadConfigExecuted = true;
					updateMemoryUsageLabel();
                }
            }
        }

		public void updateMemoryUsageLabel()
		{
			lbMemoryUsed.setText("{0:#,###} kb |  {1:#,###} kb".format(Process.GetCurrentProcess().WorkingSet64 / 1024, Process.GetCurrentProcess().PrivateMemorySize64 / 1024));
		}

		private void lbMemoryUsed_Click(object sender, EventArgs e)
		{
			updateMemoryUsageLabel();
		}
    }
}