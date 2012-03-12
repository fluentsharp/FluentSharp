// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using O2.DotNetWrappers.ExtensionMethods;
using O2.DotNetWrappers.Network;
using O2.DotNetWrappers.Windows;
using O2.Kernel;
using O2.Views.ASCX.classes.MainGUI;

namespace O2.Views.ASCX.Forms
{
    public partial class ReportBug : Form
    {
        public static String sFromEmail = "";
        public bool bOnPingSuccessHideControls = true;
        public Form fParentForm;

        public ReportBug()
        {
            InitializeComponent();
        }

        public void setRichTextBoxContents(RichTextBox rtbSourceRichTextBox)
        {
            if (rtbSourceRichTextBox != null)
            {
                rtbSourceRichTextBox.invokeOnThread(
                    () =>
                        {
                            var rtfText = rtbSourceRichTextBox.Rtf;
                            Threads_ExtensionMethods.invokeOnThread(rtbLogViewContentsToSend, () =>
                                    {
                                        rtbLogViewContentsToSend.Rtf = rtfText;
                                    });
                        });
            }
        }

        public void setScreenShoot(Image iSourceImage)
        {
            pbScreenShotToSend.BackgroundImage = iSourceImage;
        }

        public void setFromEmail(String sFrom)
        {
            sFromEmail = sFrom;
            tBoxFromEmail.Text = sFromEmail;
            //tBoxFromEmail.BackColor = Color.Coral;
        }

        public void setSubject(String sSubject)
        {
            tbSubject.Text = sSubject;
            tbSubject.BackColor = Color.Coral;
        }


        public void setMessage(String sMessage)
        {
            tbMessage.Text = sMessage;
            tbMessage.BackColor = Color.Coral;
        }


        private void btSendMessage_Click(object sender, EventArgs e)
        {
            btSendMessage.Enabled = false;
            lbMailServerConnectErrorMessage.Visible = false;
            //var oAttachments = new List<object> {rtbLogViewContentsToSend.Rtf};
            //oAttachments.Add(Bitmap. pbScreenShotToSend.I


            List<String> lsAttachements = DebugMsg.createAttachmentsForRemoteSupport(rtbLogViewContentsToSend, pbScreenShotToSend); 
            //Mail.createAttachmentsForRemoteSupport(rtbLogViewContentsToSend, pbScreenShotToSend, sFile_LogViews, sFile_LogViewsTxt, sFile_ScreenShot);

            Mail.sendMail(tbMailServer.Text, tBoxFromEmail.Text, tBoxToEmail.Text, "", tbSubject.Text,
                          tbMessage.Text, lsAttachements, true, emailSentCallback);
        }

        private void showEmailCouldNotBeSendError()
        {
            lbMailServerConnectErrorMessage.Text = "Could not send email, refreshing LogView information below";
            lbMailServerConnectErrorMessage.Visible = true;
            updateRichTextBoxWithLogViewer();
        }

        public void closeForm()
        {
            Close();
        }


        private void tBoxFromEmail_TextChanged(object sender, EventArgs e)
        {
            if (cbOunceLabsEmail.Checked && tBoxFromEmail.Text.IndexOf("@ouncelabs.com") == -1)
                tBoxFromEmail.Text += "@ouncelabs.com";
            tBoxFromEmail.BackColor = Color.LightGreen;
        }

        private void ReportBug_Load(object sender, EventArgs e)
        {
            if (false == DesignMode)
            {
                if (sFromEmail != "" && tBoxFromEmail.Text.IndexOf(sFromEmail) == -1)
                    tBoxFromEmail.Text = sFromEmail;
                tBoxToEmail.Text = PublicDI.sEmailToSendBugReportsTo;
                tbMailServer.Text = PublicDI.sEmailHost;
                updateRichTextBoxWithLogViewer();
                updateImageWithScreenShotOfParentForm();
                checkIfMailServerIsOnline();
            }
        }

        private void checkIfMailServerIsOnline()
        {
            if (Mail.isMailServerOnline(tbMailServer.Text))
                setHostControlsVisibleStatus(false);
            else
                setHostControlsVisibleStatus(true);
        }

        private void emailSentCallback(bool success)
        {
            if (success)
            {
                closeForm();
            }
            else
            {
                btSendMessage.Enabled = true;
                showEmailCouldNotBeSendError();
            }
        }

        /*private void network_ePingCompleted(object oObject, PingCompletedEventArgs e)
        {
            try
            {
                if (e.Reply == null)
                    lbPingStatus.Text = "null Reply";
                else
                {
                    lbFirstCheckToMailServer.Visible = false;
                    if (e.Reply.Status == IPStatus.Success && bOnPingSuccessHideControls)
                        setHostControlsVisibleStatus(false);
                    else
                        setHostControlsVisibleStatus(true);
                    lbPingStatus.Text = e.Reply.Status.ToString();
                    lbPingStatus.ForeColor = (e.Reply.Status == IPStatus.Success) ? Color.Green : Color.Red;
                    lbMailServerConnectErrorMessage.Visible = (e.Reply.Status != IPStatus.Success);
                }
            }
            catch (Exception ex)
            {
                DI.log.error("in network_ePingCompleted: {0}", ex.Message);
            }
            btPingMailServer.Enabled = true;
        }*/

        public void setHostControlsVisibleStatus(bool bVisibleStatus)
        {
            tbMailServer.Visible = bVisibleStatus;
            lbMailServerConnectErrorMessage.Visible = bVisibleStatus;
            lbMailServerLabel.Visible = bVisibleStatus;
            lbFirstCheckToMailServer.Visible = false;
        }

        /*private void btPingMailServer_Click(object sender, EventArgs e)
        {
            bOnPingSuccessHideControls = false;
            btPingMailServer.Enabled = false;
            lbPingStatus.Text = "";
            Ping.ping_Async(tbMailServer.Text);
        }*/

        public void updateImageWithScreenShotOfParentForm()
        {
            setScreenShoot(Screenshots.getScreenshotOfFormObjectAndItsControls(fParentForm));
            //setScreenShoot(Screenshots.getScreenshotOfFormObjectAndItsControls(btRefresh_LogViewerData));
        }

        public void updateRichTextBoxWithLogViewer()
        {
            setRichTextBoxContents(DebugMsg.getFirstRtbObject());
        }

        private void btRefresh_LogViewerData_Click(object sender, EventArgs e)
        {
            updateRichTextBoxWithLogViewer();
        }

        private void btRefresh_ScreenShot_Click(object sender, EventArgs e)
        {
            updateImageWithScreenShotOfParentForm();
        }

        private void tbMessage_TextChanged(object sender, EventArgs e)
        {
            tbMessage.BackColor = Color.LightGreen;
        }

        private void tbMailServer_TextChanged(object sender, EventArgs e)
        {
            checkIfMailServerIsOnline();
        }

        private void tbSubject_TextChanged(object sender, EventArgs e)
        {
            tbSubject.BackColor = Color.LightGreen;
        }
    }
}