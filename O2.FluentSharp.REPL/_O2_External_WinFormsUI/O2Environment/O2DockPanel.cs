// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using O2.DotNetWrappers.DotNet;
using O2.DotNetWrappers.ExtensionMethods;
using O2.External.WinFormsUI.Forms;
using O2.Interfaces.Views;
using O2.Kernel;
using O2.Views.ASCX.Ascx.MainGUI;
using O2.Views.ASCX.classes.MainGUI;
using WeifenLuo.WinFormsUI.Docking;
using O2.Views.ASCX;

namespace O2.External.WinFormsUI.O2Environment
{
    public class O2DockPanel
    {
        public static AutoResetEvent guiLoaded = new AutoResetEvent(false); 

        public O2DockPanel()
        {
            openO2DockContentInNewDockPanel(new List<O2DockContent>());
        }

        public O2DockPanel(Type controlToLoad)
        {
            openO2DockContentInNewDockPanel(controlToLoad);
        }

        public O2DockPanel(O2DockContent controlToLoad)
        {
            openO2DockContentInNewDockPanel(controlToLoad);
        }

        public O2DockPanel(List<O2DockContent> controlsToLoad)
        {
            openO2DockContentInNewDockPanel(controlsToLoad);
        }

        public string formName { get; set; }
        public int formWidth { get; set; }
        public int formHeight { get; set; }
        public List<O2DockContent> o2DockContentObjects { get; set; }

        /*public O2DockPanel(List<Type> controlsToLoad)
        {
        }*/


        public void openO2DockContentInNewDockPanel(string controlToLoad)
        {
            Type typeToLoad = Type.GetType(controlToLoad);
            if (typeToLoad == null)
                PublicDI.log.error("in openO2DockContentInNewDockPanel, could not resolve type: {0}", controlToLoad);
            else
                openO2DockContentInNewDockPanel(typeToLoad);
        }

        public void openO2DockContentInNewDockPanel(Type controlToLoad)
        {            
            openO2DockContentInNewDockPanel(new O2DockContent(controlToLoad));
        }

        public void openO2DockContentInNewDockPanel(O2DockContent o2DockContent)
        {
            openO2DockContentInNewDockPanel(new List<O2DockContent> {o2DockContent});
        }

        public void openO2DockContentInNewDockPanel(List<O2DockContent> controlsToLoad)
        {
            if (O2AscxGUI.autoAddLogViewerToGui)
                addLogViewerToControlsToLoad(controlsToLoad);
                        
            
            //O2Thread.staThread(() =>
              //                     {
                                       //var mainO2Form = new O2GuiWithDockPanel();
                                       addControlsToFormAndStartIt(controlsToLoad);
                //                       O2AscxGUI.guiClosed.WaitOne();
                  //                 });
            
            
        }
        
        private O2GuiWithDockPanel getO2GuiWithDockPanel()
        {
            if (O2AscxGUI.o2GuiWithDockPanel == null)
            {
                O2AscxGUI.o2GuiWithDockPanel = new O2GuiWithDockPanel();
                O2AscxGUI.o2GuiWithDockPanel.Text =
                    ClickOnceDeployment.getFormTitle_forClickOnce(formName ?? O2AscxGUI.o2GuiWithDockPanel.Text);
                O2AscxGUI.o2GuiWithDockPanel.Width = (formWidth > 0) ? formWidth : O2AscxGUI.o2GuiWithDockPanel.Width;
                O2AscxGUI.o2GuiWithDockPanel.Height = (formHeight > 0) ? formHeight : O2AscxGUI.o2GuiWithDockPanel.Height;
            }
            return O2AscxGUI.o2GuiWithDockPanel;                            
        }

        private void addControlToO2GuiWithDockPanelSync(O2DockContent controlToAdd)
        {
            var controlAdded = new AutoResetEvent(false);
            var o2GuiWithDockPanel = getO2GuiWithDockPanel();
            //if (O2AscxGUI.o2GuiWithDockPanel.Handle != null)
            if (O2AscxGUI.o2GuiWithDockPanel.InvokeRequired)
                O2AscxGUI.o2GuiWithDockPanel.Invoke(new EventHandler(delegate { addControlToO2GuiWithDockPanelSync(o2GuiWithDockPanel,controlToAdd, controlAdded); }));
            else
               addControlToO2GuiWithDockPanelSync(o2GuiWithDockPanel,controlToAdd, controlAdded);

            controlAdded.WaitOne();            
        }

        private static void addControlToO2GuiWithDockPanelSync(O2GuiWithDockPanel o2GuiWithDockPanel, O2DockContent controlToAdd, EventWaitHandle controlAdded)
        {
            //if (controlToAdd.dockContent != null && controlToAdd.dockContent.okThread(
            //    delegate { addControlToO2GuiWithDockPanelSync(controlToAdd, controlAdded); }))
            //{

            //var sync = new AutoResetEvent(false);

            // add the control on the o2GuiThread             
            //O2AscxGUI.o2GuiWithDockPanel.Invoke(new EventHandler(delegate {
            try
            {
                if (controlToAdd.createControlFromType())
                {
                    controlToAdd.dockContent.Show(o2GuiWithDockPanel.getDockPanel(), controlToAdd.dockState);

                    if (controlToAdd.dockState == DockState.Float && controlToAdd.dockContent.TopLevelControl != null)
                    {
                        controlToAdd.dockContent.TopLevelControl.Width = controlToAdd.desiredWidth;
                        controlToAdd.dockContent.TopLevelControl.Height = controlToAdd.desiredHeight;
                    }
                    if (controlToAdd.dockState == DockState.Document)
                    {
                        if (O2AscxGUI.o2GuiWithDockPanel.Width < controlToAdd.desiredWidth)
                            O2AscxGUI.o2GuiWithDockPanel.Width = controlToAdd.desiredWidth + 10;
                        if (O2AscxGUI.o2GuiWithDockPanel.Height < controlToAdd.desiredHeight + 100)
                            O2AscxGUI.o2GuiWithDockPanel.Height = controlToAdd.desiredHeight + 100;
                    }
                    O2DockUtils.addO2DockContentToDIGlobalVar(controlToAdd);
                }
            }
            catch(Exception ex)
            {
                PublicDI.log.ex(ex, "in addControlToO2GuiWithDockPanelSync");
            }

            controlAdded.Set();
            //                                                 }));
            //}
            //controlAdded.WaitOne();
        }

        private void addControlsToFormAndStartIt(IEnumerable<O2DockContent> controlsToAdd)
        {
//            if (mainO2Form.getDockPanel().okThread(delegate { addControlsToFormAndStartIt(mainO2Form, controlsToLoad); }))
            {
                // check if the O2AscxGUI.o2GuiWithDockPanel exists, and if it doesn't create it (we need to do do this here because of multi=thread conflics 
                // that occour sometimes if the O2AscxGUI.o2GuiWithDockPanel is created on a separate thread
         
                foreach (O2DockContent controlToAdd in controlsToAdd)                
                    addControlToO2GuiWithDockPanelSync(controlToAdd);                                   

                try
                {

                    //ClickOnceDeployment.startThreadFor_checkForClickOnceUpdatesAndInstall();  // removed 
                                                    
                    guiLoaded.Set();                    
                    Application.Run(O2AscxGUI.o2GuiWithDockPanel);                    
                }
                catch (Exception ex)
                {
                    //PublicDI.log.reportCriticalErrorToO2Developers(this, ex, "Inside Application.Run(mainO2Form);");
					PublicDI.log.error("Inside Application.Run(mainO2Form);: " + ex.Message);
                }
            }
        }
        

        private static void addLogViewerToControlsToLoad(ICollection<O2DockContent> controlsToLoad)
        {
            // first make sure the ascx_LogViewer is not already in the list of controls to add
            foreach(var controlToLoad in controlsToLoad)
                if (controlToLoad.type == typeof(ascx_LogViewer))
                    return;

            var logViewer = new O2DockContent(typeof(ascx_LogViewer), DockState.DockBottomAutoHide, PublicDI.LogViewerControlName);
            controlsToLoad.Add(logViewer);
        }

        public void openO2DockContentInNewDockPanel()
        {
            openO2DockContentInNewDockPanel(o2DockContentObjects);
        }

        public static Control loadControl(Type ascxControlToLoad, DockState dockState, String name)
        {
            return addAscxControlToO2GuiWithDockPanelWithDockState(ascxControlToLoad, dockState, name).control;
        }

        public static Control loadControl(Type ascxControlToLoad, O2DockState dockState, String name)
        {
            var o2DockContent = addAscxControlToO2GuiWithDockPanelWithDockState(
                ascxControlToLoad, O2DockUtils.getDockStateFromO2DockState(dockState), name);
            if (o2DockContent != null)
                return o2DockContent.control;
            
            return null;            
        }

        public static Control loadControl(Type ascxControlToLoad, bool showAsFloat, String name)
        {
            return addAscxControlToO2GuiWithDockPanel(ascxControlToLoad, showAsFloat, name).control;
        }

        public static O2DockContent addAscxControlToO2GuiWithDockPanel(Type ascxControlToLoad, bool showAsFloat, String name)
        {
            return addAscxControlToO2GuiWithDockPanelWithDockState(
                ascxControlToLoad,
                (showAsFloat) ? DockState.Float : DockState.Document,
                name);
        }

        public static O2DockContent addAscxControlToO2GuiWithDockPanelWithDockState(Type ascxControlToLoad,
                                                                                            DockState dockState,
                                                                                            String name)
        {
            return addAscxControlToO2GuiWithDockPanelWithDockState(ascxControlToLoad, dockState, name,true);
        }

        public static O2DockContent addAscxControlToO2GuiWithDockPanelWithDockState(Type ascxControlToLoad,
                                                                                    DockState dockState,
                                                                                    String name, bool showControl)
        {
            if (O2AscxGUI.o2GuiWithDockPanel == null)
            {
                PublicDI.log.error(" in addAscxControlToO2GuiWithDockPanelWithDockState o2GuiWithDockPanel was null, so aborting load of {0}", ascxControlToLoad.FullName);
                return null;
            }
            var o2DocContent = new O2DockContent(ascxControlToLoad, dockState, name);

            // add it as soon as possible to this list (in case there is a request for it from another thread)
            O2DockUtils.addO2DockContentToDIGlobalVar(o2DocContent);

            if (showControl)
            {
                //add content to DockPanel (in a thread safe way)
                showO2DockContentInDockPanel(o2DocContent);


                // wait for control to be loaded      
                if (false == DebugMsg.IsDebuggerAttached())
                    o2DocContent.controlLoadedIntoGui.WaitOne();
                else if (false == o2DocContent.controlLoadedIntoGui.WaitOne(10000))
                    PublicDI.log.error("Failed to load control {0} in 10 sec", o2DocContent.name);
            }
            return o2DocContent;
        }
        
        public static void showO2DockContentInDockPanel(O2DockContent o2DockContent)
        {
            try
            {
                PublicDI.log.info("on O2DockPanel.showO2DockContentInDockPanel: {0} [{1}]", o2DockContent.name, o2DockContent.type);
                if (O2AscxGUI.o2GuiWithDockPanel.okThread(delegate { showO2DockContentInDockPanel(o2DockContent); }))
                {
                    //  if (o2DocContent.dockContent.okThread(delegate { showO2DockContentInDockPanel(o2DocContent); }))
                    //    if (O2AscxGUI.o2GuiWithDockPanel.getDockPanel().okThread(
                    //        delegate { showO2DockContentInDockPanel(o2DocContent); }))

                    // now that we are on the correct thread the control can be created
                    if (o2DockContent.createControlFromType())
                    {
                        if (o2DockContent.dockContent.TopLevelControl != null)
                        {
                            o2DockContent.dockContent.Show(O2AscxGUI.o2GuiWithDockPanel.getDockPanel(), o2DockContent.dockState);
                            
                            if (o2DockContent.dockState == DockState.Float &&
                                o2DockContent.dockContent.TopLevelControl != null)
                            {
                                o2DockContent.dockContent.TopLevelControl.Width = o2DockContent.desiredWidth;
                                o2DockContent.dockContent.TopLevelControl.Height = o2DockContent.desiredHeight;
                            }
                        }
                    }
                    else
                        PublicDI.log.error("in showO2DockContentInDockPanel, could not create instance of controlToLoad: {0}",
                                                           o2DockContent.type.ToString());
                }
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex, "on O2DockPanel.showO2DockContentInDockPanel");
            }
            o2DockContent.controlLoadedIntoGui.Set();
        }


        /*
        public static void openO2FormWithDockPanel(List<Type> controlsToLoad)
        {
            var mainO2Form = new O2GuiWithDockPanel();

            foreach (var controlToLoad in controlsToLoad)
            {
                var formObject = (DockContent)new GenericDockContent().loadTypeAsMainControl(controlToLoad);                
                formObject.Show(mainO2Form.getDockPanel(),DockState.DockLeftAutoHide);                
            }
                        
            Application.Run(mainO2Form);
        }
  */        
    }
}
