// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Windows.Forms;
using System.Diagnostics;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.WinForms.Utils;

namespace FluentSharp.WinForms.Controls
{
    public partial class ascx_LogViewer : UserControl
    {
        public bool loadConfigExecuted;

        public ascx_LogViewer()
        {
            try
            {
                InitializeComponent();
                if (false == DesignMode)
                {
					if (PublicDI.log.LogRedirectionTarget == null || (PublicDI.log.LogRedirectionTarget is WinFormsUILog).isFalse())
                        PublicDI.log.LogRedirectionTarget = new WinFormsUILog();

                    cbErrorMessages.Checked = true;
                    cbDebugMessages.Checked = true;
                    cbInfoMessages.Checked = true;

                    Load+=(sender,e)=>
                        {
                            try
                            {                                
                                DebugMsg.setRtbObject(rtbDebugMessages);

                                "Testing logging: Debug Message".debug();
                                "Testing logging: Info Message".info();
                            }
                            catch (Exception ex)
                            {
                                ex.log("[in ascx_LogViewer onLoad]");
                            }
                        };
                }
            }
            catch (Exception ex)
            {
                ex.log("[in ascx_LogViewer]");
            }
            //  this.Location = new Point(20,500);            
        }


        private void btAddNewLineToDebugWindiw_Click(object sender, EventArgs e)
        {
            PublicDI.log.info("");
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
                    loadConfigExecuted = true;
                    updateMemoryUsageLabel();
                }
            }
        }

        public void updateMemoryUsageLabel()
        {
            lbMemoryUsed.set_Text("{0:#,###} kb |  {1:#,###} kb".format(Process.GetCurrentProcess().WorkingSet64 / 1024, Process.GetCurrentProcess().PrivateMemorySize64 / 1024));
        }

        private void lbMemoryUsed_Click(object sender, EventArgs e)
        {
            updateMemoryUsageLabel();
        }
    }
}