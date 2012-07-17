// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using O2.DotNetWrappers.ExtensionMethods;
using O2.DotNetWrappers.Network;
using O2.DotNetWrappers.Windows;
using O2.Kernel;
using O2.Views.ASCX.Forms;
using O2.DotNetWrappers.DotNet;

namespace O2.Views.ASCX.classes.MainGUI
{
    /// <summary>
    /// Debug and Logging functions
    /// </summary>    
    public class DebugMsg
    {
        #region Delegates

        public delegate void dDelegateForInsertText(String sText, Color cColour);

        #endregion

        //internal static bool bAlsoSendMessageToDebugView;

        public static bool bLogCache; // use when we want to keep a log of particular errors

        public static bool bShowDebug = true;
        public static bool bShowError = true;
        public static bool bShowInfo = true;
        public static bool bShowMessages = true;
        public static bool bShowTimeStamp = true;
        public static bool bUseShortTimeFormat = false;
        public static bool criticalErrorHasBeenHandled;

        public static StringBuilder sbLogCache = new StringBuilder();
        // use for errors thrown before the richtextbox is loaded

        public static Stream sOutputStream;
        public static bool sShowStreamWithHtmlFormating;
        public static List<RichTextBox> targetRichTextBoxes = new List<RichTextBox>();

        /// <summary>
        /// use to set the RichTextObject to use when displaying log messages
        /// </summary>
        /// <see cref="http://www.o--2.org"/>
        /// <param name="rtrObject"></param>
        public static void setRtbObject(RichTextBox rtrObject)
        {
            targetRichTextBoxes.Add(rtrObject);
            if (sbLogCache.Length > 0)
            {
                setRichTextBoxesText(sbLogCache.ToString());
                sbLogCache = new StringBuilder();
            }
            rtrObject.onHandleCreated(()=>
                {
                    rtrObject.onClosed(() =>
                    {
                        DebugMsg.removeRtbObject(rtrObject);
                    });                                        
                });            
        }

        private static void setRichTextBoxesText(string text)
        {
            foreach (RichTextBox richTextBox in targetRichTextBoxes)
                richTextBox.set_Text(text);
        }

        public static List<RichTextBox> getRtbObject()
        {
            return targetRichTextBoxes;
        }

        public static void _Info(String sInfoMessage)
        {
            if (bShowInfo)
                insertText("INFO: " + sInfoMessage, Color.Black);
        }

        public static void _Info(String sFormat, params Object[] oArgs)
        {
            if (bShowInfo)
                    insertText("INFO: " + String.Format(sFormat, oArgs), Color.Black);
        }

        public static void _Debug(String sInfoMessage)
        {
            if (bShowDebug)
                insertText("DEBUG: " + sInfoMessage, Color.Green);
        }

        public static void _Debug(String sFormat, params Object[] oArgs)
        {
            if (bShowDebug)
                insertText("DEBUG: " + String.Format(sFormat, oArgs), Color.Green);
        }

        public static void _Error(String sInfoMessage)
        {
            if (bShowError)
                insertText("ERROR: " + sInfoMessage, Color.Red);
        }

        public static void _Error(String sFormat, params Object[] oArgs)
        {
            try
            {
                if (bShowError)
                    insertText("ERROR: " + String.Format(sFormat, oArgs), Color.Red);
            }
            catch 
            {
                if (sFormat!= null)
                    insertText("ERROR: " + sFormat, Color.Red);
                else
                    insertText("ERROR: in _Error function", Color.Red);
            }
        }

        public static void insertText(String sText, Color cColour)
        {
            //can't do this or the whole thing start to fall apart (too many threads :) )
            //O2Thread.mtaThread(() =>
            //       {
                       try
                       {
                           if (PublicDI.log.alsoShowInConsole)         // for the cases when the GUI is started from the command line
                               Console.WriteLine(sText);

                           if (PublicDI.log.LogRedirectionTarget == null)   // this means that we have already removed this logging mode and most of the RichTextBoxes below have been disposed
                               PublicDI.log.debug("Item in DebugMsg:", sText);
                           else
                           {
                               if (targetRichTextBoxes.Count == 0)
                               {
                                   // if there are no RichTextBox defined, then just sent it to System.Diagnostics.Debug
                                   Debug.WriteLine(sText);
                                   if (sOutputStream != null)
                                   {
                                       if (sShowStreamWithHtmlFormating)
                                           sText = String.Format("<span style=\"color: {0}\">{1}</text><br/>", cColour.Name, sText);
                                       byte[] bASCIIString = Encoding.ASCII.GetBytes(sText);
                                       sOutputStream.Write(bASCIIString, 0, bASCIIString.Length);
                                   }
                                   return;
                               }
                               string sText1 = sText;
                               if (bShowTimeStamp)
                                   if (bUseShortTimeFormat)
                                       sText = "[" + DateTime.Now.ToShortTimeString() + "] " + sText;
                                   else
                                       sText = "[" + DateTime.Now.ToLongTimeString() + "] " + sText;
                               //if (targetRichTextBoxes[0].okThread(delegate { insertText(sText1, cColour); }))
                               //{
                               foreach (RichTextBox richTextBoxToUpdate in targetRichTextBoxes.toList())
                               {
                                   var richTextBox = richTextBoxToUpdate;
                                   richTextBox.invokeOnThread(
                                       () =>
                                       {
                                           if (bLogCache)
                                           {
                                               sbLogCache.Insert(0, sText + Environment.NewLine);
                                           }

                                           if (bShowMessages)
                                           {
                                               richTextBox.SelectionStart = 0;
                                               richTextBox.SelectedText = sText + Environment.NewLine +
                                                                          richTextBox.SelectedText;
                                               richTextBox.SelectionStart = 0;
                                               richTextBox.SelectionLength = sText.Length;
                                               richTextBox.SelectionColor = cColour;
                                               //Application.DoEvents();
                                               // System.Diagnostics.Debug.WriteLine(sText);                              
                                               //if (bAlsoSendMessageToDebugView)
                                               //    Debug.WriteLine(sText);
                                           }
                                           return "done"; //make this sync
                                       });
                               }
                               //}

                           }
                       }
                       catch (Exception ex)
                       {
                           Debug.WriteLine("ERROR IN DebugMsg (1/3): " + ex.Message + "\n\n");
                           Debug.WriteLine("ERROR IN DebugMsg (2/3): original message: " + sText);
                           Debug.WriteLine("ERROR IN DebugMsg (2/3): Stack Trace" + ex.StackTrace);
                       }
             //      });
        }

        public static void showNow(String sMessage)
        {
            _Info("{0} {1}", sMessage, DateTime.Now.ToLongTimeString());
            Application.DoEvents();
        }

        public static void clearText()
        {
            setRichTextBoxesText("");
        }

        public static RichTextBox getFirstRtbObject()
        {
            return (targetRichTextBoxes.Count == 0) ? null : targetRichTextBoxes[0];
        }

        /*public static String getText()
        {
            return rtbDebugMsg.Text;
        }*/


        public static void setLogCacheStatus(bool bNewLogCacheStatus)
        {
            bLogCache = bNewLogCacheStatus;
        }

        public static String getLogCacheContents(bool bClearIt)
        {
            String sLogCacheContents = sbLogCache.ToString();
            if (bClearIt)
                sbLogCache = new StringBuilder();
            return sLogCacheContents;
        }

        public static void saveLogIntoFile(String sFileToSave)
        {
            if (sbLogCache.Length == 0)
                _Error("in saveLogIntoFile: Log Cache is empty");
            else
            {
                Files.WriteFileContent(sFileToSave, sbLogCache.ToString());
                _Info("Log Cache file of size {0} saved to {1}", sbLogCache.Length, sFileToSave);
                sbLogCache = new StringBuilder();
            }
        }

        public static DialogResult showMessageBox(string message)
        {
            return showMessageBox(message, "O2 Message", MessageBoxButtons.OK);
        }

        public static DialogResult showMessageBox(string message, string messageBoxTitle,
                                                  MessageBoxButtons messageBoxButtons)
        {
            return MessageBox.Show(message, messageBoxTitle, messageBoxButtons);
        }

        public static void debugBreak()
        {
            Debugger.Break();
        }

        public static void LogException(Exception ex)
        {
            LogException(ex, false);
        }

        public static void LogException(Exception ex, bool showStackTrace)
        {
            LogException(ex, "Exception:", showStackTrace);
        }

        public static void LogException(Exception ex, String text)
        {
            LogException(ex, text, false);
        }

        public static void LogException(Exception ex, String text, bool showStackTrace)
        {
            if (ex != null)
            {
                _Error(text + " {0}", ex.Message);
                if (showStackTrace)
                    _Error(text + "  StackTrace:\n\n{0}\n\n", ex.StackTrace);
                LogException(ex.InnerException, "     InnerException: {0}", true);
            }
            //_Error("     InnerException: {0}", ex.InnerException.Message);            
        }      

        public static void removeRtbObject(RichTextBox rtbToRemove)
        {
            if (targetRichTextBoxes.Contains(rtbToRemove))
                targetRichTextBoxes.Remove(rtbToRemove);
        }

/*        public static void reportCriticalErrorToO2Developers(object currentObject, Exception ex, String sourceMessage)
        {
            if (criticalErrorHasBeenHandled) // since we only want to this for the first critical error that we get
                return;
            criticalErrorHasBeenHandled = true;
            showMessageBox(
                "We sorry but a critical error has occured and O2 will have to close now :( . " + Environment.NewLine +
                Environment.NewLine +
                " Please use the next form to report this bug and explain as much as possible what youdid that (may have) caused this problem. \n\nThanks");
            String errorMessage =
                String.Format(
                    "Critial error in O2 Main Form : " + Environment.NewLine + Environment.NewLine + " Message: {0} " +
                    Environment.NewLine + Environment.NewLine + " Exception: {1}" + Environment.NewLine +
                    Environment.NewLine + " StackTrace: {2} ", sourceMessage, ex.Message,
                    ex.StackTrace);

            // DI.log.error(errorMessage);
            reportCriticalErrorToO2Developers(PublicDI.CurrentGUIHost, "O2 GUI Crash", errorMessage);
            //DebugMsg.showMessageBox(errorMessage + "\n\n" + ex.StackTrace+ "\n\n");
            Application.Exit();
        }


        public static DialogResult reportCriticalErrorToO2Developers(object currentObject, string subject,
                                                                     string message)
        {
            var rpReportBug = new ReportBug();
            rpReportBug.setFromEmail("o2User@ouncelabs.com");
            rpReportBug.setSubject("[CRITICAL O2 ERROR] " + currentObject);
            rpReportBug.setMessage(Mail.getUserDetailsAsEmailFormat() + " says:" + Environment.NewLine + subject +
                                   Environment.NewLine + Environment.NewLine + message);
            return rpReportBug.ShowDialog();         
        }*/
        

        public static List<String> createAttachmentsForRemoteSupport(RichTextBox logViewContentsToSend, PictureBox screenShotToSend)
        {
            String sFile_LogViews = DI.config.TempFileNameInTempDirectory + "_" + PublicDI.sDefaultFileName_ReportBug_LogView;
            String sFile_LogViewsTxt = DI.config.TempFileNameInTempDirectory + "_" + PublicDI.sDefaultFileName_ReportBug_LogView + ".txt";
            String sFile_ScreenShot = DI.config.TempFileNameInTempDirectory + "_" + PublicDI.sDefaultFileName_ReportBug_ScreenShotImage + ".Jpeg";
            return Main_WinForms.createAttachmentsForRemoteSupport(logViewContentsToSend, screenShotToSend, sFile_LogViews, sFile_LogViewsTxt, sFile_ScreenShot);
        }



        #region Nested type: O2Timer

        #endregion

        public static bool IsDebuggerAttached()
        {
            return Debugger.IsAttached;
        }
    }
}