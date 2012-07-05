using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using O2.DotNetWrappers.DotNet;
using O2.Kernel;
using O2.Views.ASCX.ExtensionMethods;
using O2.DotNetWrappers.ExtensionMethods;

namespace O2.Views.ASCX.classes.MainGUI
{
    public class O2Gui : Form
    {
		public AutoResetEvent formClosed = new AutoResetEvent(false);
        public AutoResetEvent formLoaded = new AutoResetEvent(false);
        
        public O2Gui() : this(-1,-1, false)
        {            	
        }

        public O2Gui(bool isMdiContainer) : this(-1,-1, isMdiContainer)
        {            
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (base.IsDisposed.isFalse())
                    base.Dispose(disposing);               
            }
            catch (Exception ex)
            {
                ex.log("in O2Gui dispose");
            }
        }

        public O2Gui(int width, int height) : this(width, height, false)
        {
        }

        public O2Gui(int width, int height, bool isMdiContainer)
        {
            if (PublicDI.log.LogRedirectionTarget == null)
                PublicDI.log.LogRedirectionTarget = new WinFormsUILog();
            if (width > -1)
        	    Width = width;
            if (height > -1)
        	    Height = height;
            
            Top = 1;
            Left = 1;

            IsMdiContainer = isMdiContainer;

            Closed += (sender, e) => formClosed.Set();
            Load += (sender, e) => formLoaded.Set();
            Closed += (sender,e) => this.Dispose();

            this.set_O2Icon();
            
        }
        		
        public void showDialog()
        {
            showDialog(true);
        }
        public void showDialog(bool useNewStaThread)
        {
            if (useNewStaThread)
        	    O2Thread.staThread(()=>ShowDialog());
            else
                ShowDialog();
            //formLoaded.WaitOne();
        }

        public void show()
        {           
            O2Thread.staThread(Show);
            formLoaded.WaitOne();
        }

        public void waitForFormClose()
        {
            formClosed.WaitOne();
        }        
        
        public void waitForFormLoad()
        {
        	formLoaded.WaitOne();
        }

        public static T open<T>() where T : Control
        {
            return load<T>();
        }

        public static T open<T>(string title) where T : Control
        {
            return load<T>(title);
        }

        public static T open<T>(string title, int width, int height) where T : Control
        {
            return load<T>(title,width,height);
        }        

        public static T load<T>() where T : Control
        {                
            return (T)typeof(T).showAsForm();
        }

        public static T load<T>(string title) where T : Control
        {
            return (T)WinForms.showAscxInForm(typeof(T), title);
        }

        public static T load<T>(string title, int width, int height) where T : Control
        {
            return (T)WinForms.showAscxInForm(typeof(T), title, width, height);
        }        

        public static T showAsForm<T>() where T : Control
        {
            return (T) typeof(T).showAsForm();            
        }

        public static T showAsForm<T>(string title) where T : Control
        {
            return (T)WinForms.showAscxInForm(typeof(T),title);
        }

        public static T showAsForm<T>(string title, int width, int height) where T : Control
        {
            return (T)WinForms.showAscxInForm(typeof(T), title,width, height);
        }
        
            

    }
}
