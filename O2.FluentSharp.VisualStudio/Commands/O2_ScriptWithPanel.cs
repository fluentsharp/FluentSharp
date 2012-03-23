using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using O2.Kernel					 .ExtensionMethods;
using O2.Views.ASCX				 .ExtensionMethods;
using O2.DotNetWrappers			 .ExtensionMethods;
using O2.DotNetWrappers.DotNet;
using O2.FluentSharp.VisualStudio.ExtensionMethods;

namespace O2.FluentSharp.VisualStudio.Commands
{
	public class O2_ScriptWithPanel : CommandBase
	{

		public O2_ScriptWithPanel(O2_VS_AddIn o2AddIn) : base(o2AddIn)
		{
			this.create();			
/*			var assemblyResolver = new AssemblyResolver();
			var assemblyPath = CompileEngine.resolveCompilationReferencePath("O2_API_Ast.dll");
			
			var assembly = assemblyResolver.loadAssembly("O2_API_Ast.dll");
				assembly = assemblyResolver.loadAssembly("O2_Platform_BCL.dll");
			    assembly = assemblyResolver.loadAssembly("O2_External_SharpDevelop.dll");
				assembly = assemblyResolver.loadAssembly("O2_Misc_Microsoft_MPL_Libs.dll");
				assembly = assemblyResolver.loadAssembly("O2_External_WinFormsUI.dll");
				assembly = assemblyResolver.loadAssembly("O2SharpDevelop.dll");
				assembly = assemblyResolver.loadAssembly("O2_External_IE.dll");
				assembly = assemblyResolver.loadAssembly("O2_Platform_Launcher.exe");

				assembly = assemblyResolver.loadAssembly("O2SharpDevelop");
				assembly = assemblyResolver.loadAssembly("O2SharpDevelop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
				
			
			//var assembly = assemblyResolver.loadAssembly("O2SharpDevelop.dll");
			
			//var assembly = assemblyResolver.loadAssembly("O2_API_Ast.dll");
 */ 
		}

		public override void create()
		{
			this.ButtonText = "O2 Script - with Panel";			
			this.ToolTip = "Opens the O2 Script Guid (with a top panel)";
			this.TargetMenu = "O2 Platform";
			base.create();
			this.Execute = () =>
						{
							var script = "ascx_Quick_Development_GUI.cs.o2".local();
							var assembly = new CompileEngine().compileSourceFile(script);
							assembly.methods()[0].invoke(new object[] { });
							//assembly.executeFirstMethod();
						};
		}
		
	}
}
