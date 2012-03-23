using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using O2.Kernel					.ExtensionMethods;
using O2.FluentSharp.VisualStudio.ExtensionMethods;
using O2.DotNetWrappers			.ExtensionMethods;
using O2.Views.ASCX				.ExtensionMethods;

namespace O2.FluentSharp.VisualStudio.Commands
{
	public class O2_LogViewer : CommandBase
	{
		
		public O2_LogViewer(O2_VS_AddIn o2AddIn) : base(o2AddIn)
		{
			this.create();
		}

		public override void create()
		{
			this.ButtonText = "O2 LogViewer";			
			this.ToolTip = "Opens the LogViewer";
			this.TargetMenu = "O2 Platform";
			base.create();			
			this.Execute = () =>
					{
						
						var panel = O2AddIn.VS_Dte.createWindowWithPanel(O2AddIn.VS_AddIn, "test window");
						panel.add_LogViewer();

						//"Util - LogViewer.h2".local().executeH2Script();
					};
		}		
	}
}
