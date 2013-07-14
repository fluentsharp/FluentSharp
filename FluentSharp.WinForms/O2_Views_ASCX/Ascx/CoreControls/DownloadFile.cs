// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.WinForms.Utils;

// with code inspired from the sample in : http://www.geekpedia.com/tutorial196_Creating-an-advanced-download-manager-in-Csharp.html

namespace FluentSharp.WinForms.Controls
{
    public partial class DownloadFile : UserControl
    {
        // The thread inside which the download happens
        // The stream of data that we write to the harddrive
        private static int PercentProgress;
        private Callbacks.dMethod_String dCallbackWhenCompleted;
        private bool goPause;
        private Stream strLocal;
        private Stream strResponse;
        private Thread thrDownload;
        // The request to the web server for file information
        private HttpWebRequest webRequest;
        // The response from the web server containing information about the file
        private HttpWebResponse webResponse;
        // The progress of the download in percentage

        public DownloadFile()
        {
            InitializeComponent();
        }

        public void setDownloadDetails(String sSourceUrl, String TargetFile)
        {
            this.invokeOnThread(() =>
                                    {
                                        txtUrl.Text = sSourceUrl;
                                        txtPath.Text = TargetFile;
                                    });
        }

        public void setAutoCloseOnDownload(bool bCloseOnDownloadCompletion)
        {
            this.invokeOnThread(() =>
                                    {
                                        cbCloseWindowWhenDownloadComplete.Checked = bCloseOnDownloadCompletion;
                                    });
        }

        public void setCallBackWhenCompleted(Callbacks.dMethod_String _callbackWhenCompleted)
        {
            dCallbackWhenCompleted = _callbackWhenCompleted;
        }

        public void downloadFile()
        {
            this.invokeOnThread(() => btnDownload_Click(null, null));
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (thrDownload != null && thrDownload.ThreadState == ThreadState.Running)
            {
                MessageBox.Show(
                    "A download is already running. Please either the stop the current download or await for its completion before starting a new one.",
                    "Download in progress", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // Let the user know we are connecting to the server
                lblProgress.Text = "Download Starting";
                // Create a new thread that calls the Download() method
                thrDownload = new Thread(Download);
                // Start the thread, and thus call Download(); start downloading from the beginning (0)
                thrDownload.Start(0);
                // Enable the Pause/Resume button
                btnPauseResume.Enabled = true;
                // Set the button's caption to Pause because a download is in progress
                btnPauseResume.Text = "Pause";
            }
        }

        private void UpdateProgress(Int64 BytesRead, Int64 TotalBytes)
        {
            // Calculate the download progress in percentages
            PercentProgress = Convert.ToInt32((BytesRead*100)/TotalBytes);
            if (PercentProgress > 100)
                PercentProgress = 100;
            // Make progress on the progress bar
            prgDownload.Value = PercentProgress;
            // Display the current progress on the O2Form
            lblProgress.Text = "Downloaded " + BytesRead + " out of " + TotalBytes + " (" + PercentProgress + "%)";
        }

        private void Download(object startPoint)
        {
            try
            {
                // Put the object argument into an int variable
                int startPointInt = Convert.ToInt32(startPoint);
                // Create a request to the file we are downloading
                webRequest = (HttpWebRequest) WebRequest.Create((string) txtUrl.Text);
                // Set the starting point of the request
                webRequest.AddRange(startPointInt);

                // Set default authentication for retrieving the file
                webRequest.Credentials = CredentialCache.DefaultCredentials;
                // Retrieve the response from the server
                webResponse = (HttpWebResponse) webRequest.GetResponse();
                // Ask the server for the file size and store it
                Int64 fileSize = webResponse.ContentLength;

                // Open the URL for download 
                strResponse = webResponse.GetResponseStream();

                // calculate the TargetFile
                String sTargetFile = txtPath.Text;
                if (Directory.Exists(txtPath.Text))
                {
                    String sFileName = Path.GetFileName(txtUrl.Text);
                    sTargetFile = Path.Combine(sTargetFile, sFileName);
                }
                if (File.Exists(sTargetFile))
                {
                    try
                    {
                        File.Delete(sTargetFile);
                    }
                    catch (Exception)
                    {
                        string alternativeFileName = PublicDI.config.TempFileNameInTempDirectory + "_" +
                                                     Path.GetFileName(sTargetFile);
                        PublicDI.log.info("In Donwnload could not create file :{0}, so downloding into {1} instead",
                                    sTargetFile, alternativeFileName);
                        sTargetFile = alternativeFileName;
                    }
                }

                // Create a new file stream where we will be saving the data (local drive)
                strLocal = startPointInt == 0
                               ? new FileStream(sTargetFile, FileMode.Create, FileAccess.Write, FileShare.None)
                               : new FileStream(sTargetFile, FileMode.Append, FileAccess.Write, FileShare.None);

                // It will store the current number of bytes we retrieved from the server
                int bytesSize;
                // A buffer for storing and writing the data retrieved from the server
                var downBuffer = new byte[2048];

                // Loop through the buffer until the buffer is empty
                while ((bytesSize = strResponse.Read(downBuffer, 0, downBuffer.Length)) > 0)
                {
                    // Write the data from the buffer to the local hard drive
                    strLocal.Write(downBuffer, 0, bytesSize);
                    // Invoke the method that updates the O2Form's label and progress bar
                    Invoke(new UpdateProgessCallback(UpdateProgress),
                           new object[] {strLocal.Length, fileSize + startPointInt});

                    if (goPause)
                    {
                        break;
                    }
                }
                // invoke completed callback
                if (dCallbackWhenCompleted != null)
                {
                    dCallbackWhenCompleted.Invoke(sTargetFile);
                }
            }
            catch (Exception ex)
            {
                string message = "in Download: {0} (for request:{1})".format(ex.Message, txtUrl.Text);
                lblProgress.invokeOnThread(() =>
                                               {
                                                   lblProgress.Text = message;
                                                   lblProgress.ForeColor = Color.Red;
                                               });                                
                PublicDI.log.error(message);
            }
            finally
            {
                // When the above code has ended, close the streams
                if (strResponse != null)
                    strResponse.Close();
                if (strLocal != null)
                    strLocal.Close();
            }

            // close O2Form if required
            if (cbCloseWindowWhenDownloadComplete.Checked)
            {                
                if (Parent != null && (Parent.GetType().Name == "Form" || Parent.GetType().Name == "GenericDockContent"))
                {
                    O2Forms.executeMethodThreadSafe(Parent, "Close");
                }
                //().Close();
                //this.Parent
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                // Abort the thread that's downloading
                thrDownload.Abort();
                // Close the web response and the streams
                webResponse.Close();
                strResponse.Close();
                strLocal.Close();
                // Set the progress bar back to 0 and the label
                prgDownload.Value = 0;
                lblProgress.Text = "Download Stopped";
                // Disable the Pause/Resume button because the download has ended
            }
            catch (Exception ex)
            {
                PublicDI.log.error(" in btnStop_Click: {0}", ex.Message);
            }
            btnPauseResume.Enabled = false;
        }

        private void btnPauseResume_Click(object sender, EventArgs e)
        {
            // If the thread exists
            if (thrDownload != null)
            {
                if (btnPauseResume.Text == "Pause")
                {
                    // The Pause/Resume button was set to Pause, thus pause the download
                    goPause = true;

                    // Now that the download was paused, turn the button into a resume button
                    btnPauseResume.Text = "Resume";

                    // Close the web response and the streams
                    webResponse.Close();
                    strResponse.Close();
                    strLocal.Close();
                    // Abort the thread that's downloading
                    thrDownload.Abort();
                }
                else
                {
                    // The Pause/Resume button was set to Resume, thus resume the download
                    goPause = false;

                    // Now that the download was resumed, turn the button into a pause button
                    btnPauseResume.Text = "Pause";

                    long startPoint = 0;

                    if (File.Exists(txtPath.Text))
                    {
                        startPoint = Files_WinForms.getFileSize(txtPath.Text);
                    }
                    else
                    {
                        MessageBox.Show("The file you choosed to resume doesn't exist.", "Could not resume",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    // Let the user know we are connecting to the server
                    lblProgress.Text = "Download Resuming";
                    // Create a new thread that calls the Download() method
                    thrDownload = new Thread(Download);
                    // Start the thread, and thus call Download()
                    thrDownload.Start(startPoint);
                    // Enable the Pause/Resume button
                    btnPauseResume.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("A download does not appear to be in progress.", "Could not pause", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        #region Nested type: UpdateProgessCallback

        private delegate void UpdateProgessCallback(Int64 BytesRead, Int64 TotalBytes);

        #endregion
    }
}
