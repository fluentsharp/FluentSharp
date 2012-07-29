using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using O2.Kernel					.ExtensionMethods;
using O2.FluentSharp.VisualStudio.ExtensionMethods;
using O2.DotNetWrappers			.ExtensionMethods;
using O2.Views.ASCX				.ExtensionMethods;

using O2.DotNetWrappers.DotNet;

namespace O2.FluentSharp.VisualStudio.Commands
{
	public class O2_ScriptGui : CommandBase
	{

		public O2_ScriptGui(O2_VS_AddIn o2AddIn) : base(o2AddIn)
		{
			this.create();
		}

		public override void create()
		{
			this.ButtonText = "O2 Script Gui";			
			this.ToolTip = "Opens the O2 Script GUI";
			this.TargetMenu = "O2 Platform";
			base.create();

			this.Execute = () =>
					{
						var title	 = "O2 Simple Script Editor";
						var o2Script = "ascx_Simple_Script_Editor.cs.o2";
						var type	 = "ascx_Simple_Script_Editor";
						O2AddIn.add_WinForm_Control_from_O2Script(title, o2Script, type, 500,300);
					};

		}
		
	}
}
