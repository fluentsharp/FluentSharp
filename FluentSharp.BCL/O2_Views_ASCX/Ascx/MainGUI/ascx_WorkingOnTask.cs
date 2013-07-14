// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Windows.Forms;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;

namespace FluentSharp.WinForms.Controls
{
    public partial class ascx_WorkingOnTask : UserControl
    {
        public bool runOnLoad = true;
        public bool runAnnimation = true;
        public int timeElapsed = 0;
        public ascx_WorkingOnTask()
        {
            InitializeComponent();            
        }


        public void startProgressBarAnimation()
        {
            if (ParentForm != null)
                ParentForm.Closed += (a, b) => runAnnimation = false;
            progressBar.Maximum = 100;
            progressBar.Value = 0;
            runAnnimation = true;
            O2Thread.mtaThread(
                () => {
                          while (runAnnimation)                        
                          {
                              timeElapsed += 1;                              
                              Thread_Invoke_ExtensionMethods.invokeOnThread(progressBar, () =>
                                                                               {
                                                                                   lbTimeElapsed.Text = "{0} Sec".format(timeElapsed);
                                                                                   progressBar.Value++;                                                                                    
                                                                                   if (progressBar.Value >= progressBar.Maximum)
                                                                                       progressBar.Value = 0;
                                                                               });
                              System.Threading.Thread.Sleep(1000);                                 
                          }
                });
        }

        public void setWorkingTaskText(string text)
        {
            this.invokeOnThread(
                () =>
                    {
                        lbTaskCurrentlyWorkingOn.Text = text;
                        startProgressBarAnimation();
                    });
        }

        public void stopAnimation()
        {
            runAnnimation = false;            
        }

        public void close()
        {
            if (ParentForm != null)
            {
                stopAnimation(); 
                ParentForm.Close();
            }
        }

        private void ascx_WorkingOnTask_Load(object sender, EventArgs e)
        {
            onLoad();
        }

        private void onLoad()
        {
            if (runOnLoad)
            {
                startProgressBarAnimation();
                setWorkingTaskText(Text);
                runOnLoad = true;
            }            
        }
    }
}