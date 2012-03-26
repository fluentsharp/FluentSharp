using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using O2.Views.ASCX.ExtensionMethods;
using O2.Views.ASCX.Ascx.MainGUI;

namespace O2.DotNetWrappers.ExtensionMethods
{
    public static class WinForms_ExtensionMethods_O2_Controls
    {
        public static ascx_LogViewer add_LogViewer<T>(this T control)
            where T : Control
        {
            return control.add_Control<ascx_LogViewer>();
        }

        public static T insert_LogViewer<T>(this T control)
			where T : Control
		{
			return control.insert_LogViewer(false);
		}
			
		public static T insert_LogViewer<T>(this T control, bool make_Panel1_Fixed)
			where T : Control	
		{
			control.insert_Below(100)
				   .add_LogViewer();
			if (make_Panel1_Fixed)
				control.parent<SplitContainer>()
					   .fixedPanel1();
			return control;
		}
    }
}
