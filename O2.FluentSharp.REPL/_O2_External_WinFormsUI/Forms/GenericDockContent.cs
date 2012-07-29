// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Windows.Forms;
using O2.External.WinFormsUI.O2Environment;
using WeifenLuo.WinFormsUI.Docking;

namespace O2.External.WinFormsUI.Forms
{
    public partial class GenericDockContent : DockContent
    {
        public GenericDockContent()
        {
            InitializeComponent();
            if (DesignMode!= false)
                new O2MessagesHandler(); // make sure the Messages Handler is setup
        }

        public Form loadTypeAsMainControl(Type controlToLoad)
        {
            return loadTypeAsMainControl(controlToLoad, controlToLoad.Name);
        }

        public Form loadTypeAsMainControl(Type controlToLoad, String tabText)
        {
            return loadControlAsMainControl((Control) Activator.CreateInstance(controlToLoad), tabText);
        }

        public Form loadControlAsMainControl(Control control, String tabText)
        {
            control.Dock = DockStyle.Fill;
            Controls.Add(control);
            TabText = tabText;
            Text = control.Text;
            return this;
        }

        private void GenericDockContent_FormClosing(object sender, FormClosingEventArgs e)
        {
            O2DockUtils.removeO2DockContentFromDIGlobalVar(Text);  // remove the control hosted by by this     
        }
    }
}
