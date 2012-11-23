using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.PlatformUI; 
using Microsoft.VisualStudio.Platform.WindowManagement; 
using Microsoft.VisualStudio.Platform.WindowManagement.DTE;
using System.Windows.Forms.Integration;
using WinForms = System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using VSLangProj;
using O2.DotNetWrappers.DotNet;
using O2.DotNetWrappers.ExtensionMethods;
using O2.FluentSharp.VisualStudio;


namespace O2.FluentSharp.VisualStudio.ExtensionMethods
{
	public static class VisualStudio_2010_ExtensionMethods_VSProjects
	{
		public static VSProject vsProject(this VisualStudio_2010 visualStudio, string nameorUniqueName)
		{
			return visualStudio.project(nameorUniqueName).vsProject();
		}

		public static VSProject vsProject(this Project project)
		{
			if (project.notNull() && project.Object is VSProject)
				return (VSProject)project.Object;
			return null;
		}
		public static List<Reference> references(this VSProject vsProject)
		{
			return vsProject.References.toList<Reference>();
		}
		public static Reference reference(this VSProject vsProject, string name)
		{
			return vsProject.references().Where((r) => r.Name == name).first();
		}
		public static List<string> names(this List<Reference> references)
		{
			return references.Select((r) => r.Name).toList();
		}
		public static VSProject add_Reference(this VSProject vsProject, string pathToReference)
		{
			vsProject.References.Add(pathToReference);
			return vsProject;
		}
		public static VSProject remove_Reference(this VSProject vsProject, string name)
		{
			var reference = vsProject.reference(name);
			if (reference.notNull())
				reference.Remove();
			return vsProject;
		}
		public static VSProject add_Compiled_O2Script_as_Reference(this VSProject vsProject, string o2Script)
		{
			return vsProject.add_Compiled_O2Script_as_Reference(o2Script, "_O2_Dlls");
		}
		public static VSProject add_Compiled_O2Script_as_Reference(this VSProject vsProject, string o2Script, string targetVirtualProjectDir)
		{
			var targetDir = vsProject.Project.folder().pathCombine(targetVirtualProjectDir).createDir();
			"Compiling O2 Script into Project folder: {0}".info(targetDir);
			var dllFile = o2Script.local().compileToDll(targetDir);
			if (dllFile.fileExists())
			{
				"Dll File compiled into: {0}".info(dllFile);
				vsProject.add_Reference(dllFile);
			}
			return vsProject;
		}
	}

	public static class VisualStudio_2010_ExtensionMethods_Projects
	{
		public static Project project(this VisualStudio_2010 visualStudio, string nameorUniqueName)
		{
			return visualStudio.projects()
							   .Where((project) => project.Name == nameorUniqueName || project.UniqueName == nameorUniqueName)
							   .first();
		}
		public static List<Project> projects(this VisualStudio_2010 visualStudio)
		{
			return VisualStudio_2010.DTE2.Solution.Projects.toList<Project>();
		}

		public static string folder(this Project project)
		{
			return project.FullName.parentFolder();
		}

		public static Configuration configuration_Active(this Project project)
		{
			return project.ConfigurationManager.ActiveConfiguration;
		}

		public static Configuration configuration(this Project project, string name)
		{
			return project.configurations().Where((c) => c.ConfigurationName == name).first();
		}

		public static List<Configuration> configurations(this Project project)
		{
			return project.ConfigurationManager.toList<Configuration>();
		}

		public static T property<T>(this Configuration configuration, string name)
		{
			return (T)configuration.properties()
									.Where((p) => p.Key == name && p.Value is T).first().value();
		}

		public static object property(this Configuration configuration, string name)
		{
			return configuration.properties().Where((p) => p.Key == name).first().value();
		}

		public static List<NameValuePair<string, object>> properties(this Configuration configuration)
		{
			return configuration.properties<object>();
		}

		public static List<NameValuePair<string, T>> properties<T>(this Configuration configuration)
		{
			var properties = new List<NameValuePair<string, T>>();
			foreach (Property property in configuration.Properties)
			{
				try
				{
					if (property.Value is T)
						properties.Add(new NameValuePair<string, T>(property.Name, (T)property.Value));
				}
				catch //(Exception ex)
				{
					//ex.log();
				}
			}
			return properties;
		}
	}
}