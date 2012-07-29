// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Drawing;
using System.Windows.Forms;
using O2.DotNetWrappers.DotNet;
using O2.DotNetWrappers.ExtensionMethods;
using O2.DotNetWrappers.Network;
using O2.DotNetWrappers.Windows;
using O2.External.WinFormsUI.O2Environment;
using O2.Interfaces.Messages;
using O2.Interfaces.Views;
using O2.Kernel;
using O2.Kernel.CodeUtils;
using O2.Views.ASCX.Ascx.MainGUI;
using O2.Views.ASCX.classes.MainGUI;
using WeifenLuo.WinFormsUI.Docking;
using System.IO;
using O2.Views.ASCX;

namespace O2.External.WinFormsUI.Forms
{
    public partial class O2GuiWithDockPanel
    {
                
        public DockPanel getDockPanel()
        {
            return dockPanel;
        }

        private void o2MessageQueue_onMessages(IO2Message o2Message)
        {
            //first thing to do is to make sure we are on the correct thread
            if (InvokeRequired)
                Invoke(new EventHandler(delegate { o2MessageQueue_onMessages(o2Message); }));
            else
            {
                if (o2Message is IM_GUIAction)
                {
                    var mGuiAction = (IM_GUIAction) o2Message;
                    PublicDI.log.info("O2GuiWithDockPanel received IM_GUIAction of action: {0}", mGuiAction.GuiAction);
                    switch (mGuiAction.GuiAction)
                    {
                            
                        case (IM_GUIActions.openControlInGui):
                            O2DockPanel.loadControl(mGuiAction.controlType, mGuiAction.o2DockState,
                                                    mGuiAction.controlName);
                            break;                            
                        case (IM_GUIActions.setAscxDockStateAndOpenIfNotAvailable):                            
                            // if setDockState fails is because the control is not loaded
                            if (false == O2DockUtils.setDockState(mGuiAction.controlName,mGuiAction.o2DockState))
                                O2AscxGUI.openAscxASync(mGuiAction.controlTypeString, mGuiAction.o2DockState,mGuiAction.controlName);                                 
                            break;

                    }
                    return;
                }
            }
        }

        public void sendEmailToO2SupportUsingTextInMenuBar()
        {
            sendEmailToO2Support("Main GUI", tbTextToemailSupport.Text);
            tbTextToemailSupport.Text = "";
        }

        public void sendEmailToO2Support(string subject, string text)
        {
            sendEmailToO2Support(subject, text, true);
        }

        public void sendEmailToO2Support(string subject, string text, bool sendSync)
        {
            sendMessageToolStripMenuItem1.Text = "Sending email....";
            try
            {
                var screenShotToSend = new PictureBox
                {
                    BackgroundImage =
                        Screenshots.getScreenshotOfFormObjectAndItsControls(this)
                };
                PublicDI.log.debug("Sending email to O2 Support with: " + text);
                Mail.sendMail(PublicDI.sEmailHost, "O2User@ouncelabs.com",
                              PublicDI.sEmailToSendBugReportsTo, "",
                              "Email from O2 User - " + subject,
                              Mail.getUserDetailsAsEmailFormat() + Environment.NewLine + text,
                              DebugMsg.createAttachmentsForRemoteSupport(DebugMsg.getFirstRtbObject(), screenShotToSend),
                              sendSync, emailMessageSent);

                //
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex, "in sendEmailToO2Support");
                PublicDI.log.error(
                    "Could not send support email, please ensure that this o2 module is connected to the Internet");
            }
        }

        public MenuStrip getMainMenu()
        {
            return menuStripForO2GuiWithDocPanel;
        }

        private void emailMessageSent(bool result)
        {
            if (result)
            {
                toolStripTextBoxForMailServer.Visible = false;
                PublicDI.log.debug("Email sucessfull sent. Thanks");
                setDefaultEmailO2SupportMessage();
                sendMessageToolStripMenuItem1.Text =
                    "Mail Sent OK: You can type another message and Click here to send it (or press entrer)";
            }
            else
            {
                toolStripTextBoxForMailServer.Text = PublicDI.sEmailHost;
                toolStripTextBoxForMailServer.ForeColor = Color.Black;
                toolStripTextBoxForMailServer.BackColor = Color.LightPink;
                toolStripTextBoxForMailServer.Visible = true;
                sendMessageToolStripMenuItem1.Text =
                    "Could not send support email, please ensure that this o2 module is connected to the Internet";
            }
        }


        private void tryToLoadFileInMainDocumentArea(DragEventArgs e) // todo:to implement tryToLoadFileInMainDocumentArea
        {
            string file = Dnd.tryToGetFileOrDirectoryFromDroppedObject(e);
            if (File.Exists(file))
                O2Messages.fileOrFolderSelected(file);                
        }

        public static void CloseThisForm()
        {
            O2AscxGUI.o2GuiWithDockPanel.Close();
        }
        

        public static void openLogViewerControl()
        {
            O2Messages.openControlInGUI(typeof(ascx_LogViewer), O2DockState.Float, "O2 Temp Directory");            
        }

        //isAscxGuiAvailable        

        public Control getAscx(string ascxControlName)
        {
            if (O2AscxGUI.dO2LoadedO2DockContent.ContainsKey(ascxControlName))
                return O2AscxGUI.dO2LoadedO2DockContent[ascxControlName].control;           
            return null;
        }

        public void addControlToLoadedO2ModulesMenu(O2DockContent controlToLoad)
        {
            O2AscxGUI.o2GuiWithDockPanel.invokeOnThread(
                () =>
                    {
                        // Make sure there isn't alread an item with this type 
                        foreach (ToolStripItem currentToolStripItem in loadedO2ModuleToolStripMenuItem.DropDownItems)
                            if (currentToolStripItem.Tag != null && currentToolStripItem.Tag is O2DockContent &&
                                ((O2DockContent) currentToolStripItem.Tag).type == controlToLoad.type)
                                return;
                        var toolStripItem = new ToolStripMenuItem(controlToLoad.name, null,
                                                                  loadedO2ModuleToolStripMenuItem_Click)
                                                {Tag = controlToLoad};
                        loadedO2ModuleToolStripMenuItem.DropDownItems.Add(toolStripItem);
                    });
        }

        public void addToLoadedO2ModulesMenu(string menuItemName, Action onMenuItemClick)
        {
            O2AscxGUI.o2GuiWithDockPanel.invokeOnThread(
                () =>
                    {                        
                        var toolStripItem = new ToolStripMenuItem(
                            menuItemName, null,new EventHandler((_object, _EventArgs) =>O2Thread.mtaThread(onMenuItemClick)));
                                                                                          
                        ;
                        loadedO2ModuleToolStripMenuItem.DropDownItems.Add(toolStripItem);
                    });
        }
    

        private void loadedO2ModuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem)
            {
                var toolStripItem = (ToolStripMenuItem) sender;
                if (toolStripItem.Tag is O2DockContent)
                {
                    var o2DockPanel = (O2DockContent) toolStripItem.Tag;
                    var uniqueControlName = o2DockPanel.name;
                    if (O2AscxGUI.isAscxLoaded(o2DockPanel.name))
                        uniqueControlName += "  " + Guid.NewGuid();
                    O2AscxGUI.openAscxASync(o2DockPanel.type,O2DockState.Float,uniqueControlName);
                }
            }
        }
    }
}
