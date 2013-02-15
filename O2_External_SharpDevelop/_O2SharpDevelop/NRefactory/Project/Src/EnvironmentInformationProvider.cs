// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Daniel Grunwald" email="daniel@danielgrunwald.de"/>
//     <version>$Revision: 4482 $</version>
// </file>

using System;

namespace ICSharpCode.NRefactory
{
	public interface IEnvironmentInformationProvider
	{
		bool HasField(string fullTypeName, int typeParameterCount, string fieldName);
	}

	//DC had to make this class public so that it could be consumed from O2's version of CodeDOMOutputVisitor
	public class DummyEnvironmentInformationProvider : IEnvironmentInformationProvider
	{
		public static readonly IEnvironmentInformationProvider Instance = new DummyEnvironmentInformationProvider();
		
		public bool HasField(string fullTypeName, int typeParameterCount, string fieldName)
		{
			return false;
		}
	}
}
