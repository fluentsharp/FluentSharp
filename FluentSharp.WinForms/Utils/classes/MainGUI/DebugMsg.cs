// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;

namespace FluentSharp.WinForms.Utils
{
    /// <summary>
    /// Debug and Logging functions
    /// </summary>    
    public class DebugMsg
    {
        public static int DEBUG_MSG_WAIT_FOR_MESSAGES_DELAY = 20;
		public static int DEBUG_MSG_TIMEOUT_THREAD_WRITE_   = 3 * 1000;        
        public static bool bLogCache; // use when we want to keep a log of particular errors
        public static bool bShowDebug = true;
        public static bool bShowError = true;
        public static bool bShowInfo = true;
        public static bool bShowMessages = true;
        public static bool bShowTimeStamp = true;
        public static bool bUseShortTimeFormat = false;
        public static bool showMessages = true;
        public static bool criticalErrorHasBeenHandled;
        public static StringBuilder sbLogCache = new StringBuilder();        // use for errors thrown before the richtextbox is loaded
        public static Stream sOutputStream;
        public static bool sShowStreamWithHtmlFormating;
        public static List<RichTextBox> targetRichTextBoxes = new List<RichTextBox>();
        public static Queue<MessageWithColor> messagesToWrite = new Queue<MessageWithColor>();
        public static Thread activeShowThread; 
        public static void setRtbObject(RichTextBox rtrObject)
        {
            try
            {
                targetRichTextBoxes.Add(rtrObject);
                if (sbLogCache.Length > 0)
                {
                    setRichTextBoxesText(sbLogCache.ToString());
                    sbLogCache = new StringBuilder();
                }

                /*rtrObject.onHandleCreated(() =>
                    {
                        rtrObject.onClosed(() =>
                        {
                            
                        });
                    });
                rtrObject.ParentChanged += (sender, e) =>
                        {
                        };
                rtrObject.Disposed += (sender, e) =>
                {
                };*/

                rtrObject.HandleDestroyed += (sender, e) => removeRtbObject(rtrObject);
                startShowThread();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[DebugMsg] error in setRtbObject: {0}".format(ex.Message));
            }
        }        
        public static void startShowThread()
        {
            if (targetRichTextBoxes.notEmpty() && activeShowThread.isNull())
            {
                showMessages = true;
                activeShowThread = O2Thread.mtaThread(showLoop); 
            }
        }
        public static void showLoop()
        {
            //"[DebugMsg] Starting DebugMsg ShowLoop".info();
            while (showMessages)
            {
				
				if (messagesToWrite.empty())
					Thread.Sleep(DEBUG_MSG_WAIT_FOR_MESSAGES_DELAY);
				else
				{
					var nextMessage = messagesToWrite.next();
					if (bShowDebug || bShowError || bShowDebug)
						writeMessageToRichTextBoxes(nextMessage);
				}
				                
            }
            activeShowThread = null;
            //"[DebugMsg] Ended DebugMsg ShowLoop".info();
        }
        public static void writeMessageToRichTextBoxes(MessageWithColor messageWithColor)
        {
            if(messageWithColor.notNull())
                writeMessageToRichTextBoxes(messageWithColor.message, messageWithColor.color);
        }
        public static void writeMessageToRichTextBoxes(string sText, Color cColour)
        {
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
                                sText = "<span style=\"color: {0}\">{1}</text><br/>".format(cColour.Name, sText);
                            var bAsciiString = Encoding.ASCII.GetBytes(sText);
                            sOutputStream.Write(bAsciiString, 0, bAsciiString.Length);
                        }
                        return;
                    }                    
                    if (bShowTimeStamp)
                        if (bUseShortTimeFormat)
                            sText = "[" + DateTime.Now.ToShortTimeString() + "] " + sText;
                        else
                            sText = "[" + DateTime.Now.ToLongTimeString() + "] " + sText;                    
                    foreach (RichTextBox richTextBoxToUpdate in targetRichTextBoxes.toList())
                    {
                     //   if (richTextBoxToUpdate.noHandle())
                     //       continue;                        
                        var richTextBox = richTextBoxToUpdate;                        
                        try
                        {
							richTextBox.invokeOnThread(DEBUG_MSG_TIMEOUT_THREAD_WRITE_, () =>
                                {
									lock (richTextBox)
									{
										if (richTextBox.IsDisposed)
										{
											removeRtbObject(richTextBox);
											return "...";
										}
										try
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
												// PublicDI.log.error(sText);                              
												//if (bAlsoSendMessageToDebugView)
												//    Debug.WriteLine(sText);
											}
										}
										catch (Exception ex)
										{
											Debug.WriteLine("ERROR Inside DebugMsg" + ex.Message);
										}
										return "done"; //make this sync
									}
                                });
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("ERROR Inside DebugMsg" + ex.Message);
                        }
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
        }    
        public static void stopShowThread()
        {
            //"[DebugMsg] stopShowThread received".error();			
            showMessages= false;
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
        public static void _Info(String sFormat, params Object[] oArgs)
        {
            if (bShowInfo)
                    insertText("INFO : " + sFormat.format(oArgs), Color.Black);
        }
        public static void _Debug(String sFormat, params Object[] oArgs)
        {
            if (bShowDebug)
                insertText("DEBUG: " + sFormat.format(oArgs), Color.Green);
        }
        public static void _Error(String sFormat, params Object[] oArgs)
        {
            try
            {
                if (bShowError)
                    insertText("ERROR: " + sFormat.format(oArgs), Color.Red);
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
            messagesToWrite.add(new MessageWithColor { message = sText, color = cColour });
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
        public static DialogResult showMessageBox(string message, string messageBoxTitle, MessageBoxButtons messageBoxButtons)
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
                LogException(ex.InnerException, "     InnerException: {0}", showStackTrace);
            }
            //_Error("     InnerException: {0}", ex.InnerException.Message);            
        }      
        public static void removeRtbObject(RichTextBox rtbToRemove)
        {
            if (targetRichTextBoxes.Contains(rtbToRemove))
                targetRichTextBoxes.Remove(rtbToRemove);
            if (targetRichTextBoxes.empty())
                stopShowThread();
        }      
        public static List<String> createAttachmentsForRemoteSupport(RichTextBox logViewContentsToSend, PictureBox screenShotToSend)
        {
            String sFile_LogViews = PublicDI.config.TempFileNameInTempDirectory + "_" + PublicDI.sDefaultFileName_ReportBug_LogView;
            String sFile_LogViewsTxt = PublicDI.config.TempFileNameInTempDirectory + "_" + PublicDI.sDefaultFileName_ReportBug_LogView + ".txt";
            String sFile_ScreenShot = PublicDI.config.TempFileNameInTempDirectory + "_" + PublicDI.sDefaultFileName_ReportBug_ScreenShotImage + ".Jpeg";
            return Main_WinForms.createAttachmentsForRemoteSupport(logViewContentsToSend, screenShotToSend, sFile_LogViews, sFile_LogViewsTxt, sFile_ScreenShot);
        }
        public static bool IsDebuggerAttached()
        {
            return Debugger.IsAttached;
        }

        public class MessageWithColor
        {
            public string message;
            public Color  color;
        }
    }
}