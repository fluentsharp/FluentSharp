// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Threading;
using System.Windows.Forms;
using O2.DotNetWrappers.DotNet;
using O2.External.WinFormsUI.Forms;
using O2.Interfaces.Views;
using WeifenLuo.WinFormsUI.Docking;
using O2.Kernel;

namespace O2.External.WinFormsUI.O2Environment
{
    public class O2DockContent
    {        
        public Control control;
        public int desiredHeight;
        public int desiredWidth;
        public GenericDockContent dockContent;
        public DockState dockState;
        public string name;
        public Type type;
        public AutoResetEvent controlLoadedIntoGui = new AutoResetEvent(false);

        public O2DockContent(Type typeOfControlToLoad)
            : this(typeOfControlToLoad, DockState.Document)
        {
        }


        public O2DockContent(Type typeOfControlToLoad, DockState controlDockState)
            : this(typeOfControlToLoad, controlDockState, typeOfControlToLoad.Name)
        {
        }

        /*public O2DockContent(Type controlToLoad, DockState controlDockState, string controlName)
            : this((Control) Activator.CreateInstance(controlToLoad), controlDockState, controlName)
        {
        }*/

        public O2DockContent(Type typeOfControlToLoad, O2DockState controlDockState, string controlName)
            : this(typeOfControlToLoad, (DockState)Enum.Parse(typeof(DockState), controlDockState.ToString()), controlName)
        {

        }

        /*public O2DockContent(Control controlToLoad)
            : this(controlToLoad, DockState.Document)
        {
        }


        public O2DockContent(Control controlToLoad, DockState controlDockState)
            : this(controlToLoad, controlDockState, controlToLoad.Name)
        {
        }*/

        /// <summary>
        /// _note this will not create the Control, it expects a type and the control creation should be done by the form host 
        /// (this way we avoid the multi thread problems of this control being created on a diferent thread from the main hosting Form
        /// </summary>
        /// <param name="typeOfControlToLoad"></param>
        /// <param name="controlDockState"></param>
        /// <param name="controlName"></param>
        public O2DockContent(Type typeOfControlToLoad, DockState controlDockState, string controlName)
        //public O2DockContent(Control controlToLoad, DockState controlDockState, string controlName)
        {

            type = typeOfControlToLoad;
            dockContent = new GenericDockContent {Text = controlName};
            dockState = controlDockState;
            name = controlName;
        }

        public bool createControlFromType()
        {
            try
            {
                control = (Control)Activator.CreateInstance(type); //  dockContent.Controls[0]; 
                if (control != null)
                {
                    desiredWidth = control.Width;
                    desiredHeight = control.Height;
                    control.Text = name;
                    dockContent.Text = control.Text;                    
                    dockContent.loadControlAsMainControl(control, name);
                    return true;
                }
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex, "in createControlFromType");
            }
            return false;            

        }

        public static void launchO2DockContentAsStandAloneForm(Type typeOfControlToLoad, string controlName)
        {
            
            if (typeOfControlToLoad == null)
                PublicDI.log.error("in launchO2DockContentAsStandAloneForm typeOfControlToLoad was null");
            else
                try
                {
                    var sync = new AutoResetEvent(false);
                    O2Thread.staThread(() =>
                                           {
                                               try
                                               {
                                                   O2AscxGUI.o2GuiStandAloneFormMode = true;
                                                   //var controlToLoad = (Control) Activator.CreateInstance(typeOfControlToLoad);
                                                  // if (typeOfControlToLoad != null)
                                                  // {
                                                    var o2DockContent = new O2DockContent(typeOfControlToLoad, DockState.Float, controlName);
                                                    o2DockContent.dockContent.HandleCreated += (sender, e) => sync.Set();
                                                    // as soons as the control HandleCreated is created, we can let this function (launchO2DockContentAsStandAloneForm end)
                                                    if (o2DockContent.createControlFromType())
                                                    {
                                                        o2DockContent.dockContent.Width = o2DockContent.desiredWidth;
                                                        o2DockContent.dockContent.Height = o2DockContent.desiredHeight;
                                                        O2DockUtils.addO2DockContentToDIGlobalVar(o2DockContent);
                                                        o2DockContent.dockContent.Closed += (sender, e) =>
                                                                                                {
                                                                                                    if (O2AscxGUI.dO2LoadedO2DockContent.Count == 0) // if there are no more controls trigger the end of the GUI session
                                                                                                        O2AscxGUI.guiClosed.Set();
                                                                                                };

                                                        o2DockContent.dockContent.ShowDialog();
                                                    }
                                                    else
                                                    {
                                                        PublicDI.log.error(
                                                            "in launchO2DockContentAsStandAloneForm, could not create instance of controlToLoad: {0}",
                                                            typeOfControlToLoad.ToString());
                                                    }
                                               }
                                               catch (Exception ex)
                                               {
                                                   PublicDI.log.ex(ex, "in launchO2DockContentAsStandAloneForm");
                                               }
                                               sync.Set();
                                           });
                    sync.WaitOne();
                }
                catch (Exception ex)
                {
                    PublicDI.log.ex(ex);
                }
            
        }
    }
}
