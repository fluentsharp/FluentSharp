// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Mike Krüger" email="mike@icsharpcode.net"/>
//     <version>$Revision: 1965 $</version>
// </file>

using System;
using FluentSharp.CoreLib;

namespace ICSharpCode.TextEditor.Document
{
	public class HighlightingStrategyFactory
	{
		public static IHighlightingStrategy CreateHighlightingStrategy()
		{
			return (IHighlightingStrategy)HighlightingManager.Manager.HighlightingDefinitions["Default"];
		}
		
		public static IHighlightingStrategy CreateHighlightingStrategy(string name)
		{
			IHighlightingStrategy highlightingStrategy  = HighlightingManager.Manager.FindHighlighter(name);
			
			if (highlightingStrategy == null) {
				return CreateHighlightingStrategy();
			}
			return highlightingStrategy;
		}
		
		public static IHighlightingStrategy CreateHighlightingStrategyForFile(string fileName)
		{
		    try
		    {
                var highlightingStrategy  = HighlightingManager.Manager.FindHighlighterForFile(fileName);
			    return highlightingStrategy ?? CreateHighlightingStrategy();
		    }
		    catch (Exception ex)
		    {
		        ex.log("in CreateHighlightingStrategyForFile for file: {0}", fileName);
		        return null;
		    }			
		}
	}
}
