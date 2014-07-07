using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FluentSharp.CoreLib;
using FluentSharp.REPL;
using FluentSharp.REPL.Controls;
using FluentSharp.WinForms;

namespace FluentSharp.CSharpAST.Utils
{
    public static class MethodMappings_ExtensionMethods_GUI
    {
        public static TreeView CreateTreeViewFor_MethodMappingsView(this Control targetControl, ascx_SourceCodeViewer sourceCodeViewer)
        {
            targetControl.clear();
            var treeView = targetControl.add_TreeView()
                .sort();

            Action<MethodMapping> showMethodMapping = 
                (methodMapping)=>{
                                     if (methodMapping.File.valid()) 
                                     {
                                         sourceCodeViewer.open(methodMapping.File);
                                         sourceCodeViewer.editor().clearMarkers();
                                         sourceCodeViewer.editor().caret(methodMapping.Start_Line,methodMapping.Start_Column); 
                                         sourceCodeViewer.editor().selectTextWithColor(methodMapping.Start_Line,
                                             methodMapping.Start_Column, 
                                             methodMapping.End_Line, 
                                             methodMapping.End_Column);
                                     }
                };
								
            treeView.afterSelect<MethodMapping>(showMethodMapping);
            treeView.afterSelect<List<MethodMapping>>(
                (methodMappings)=>showMethodMapping(methodMappings[0]));
				
            return treeView;										 
        }
		
		
        public static void showInTreeView(this MethodMappings methodMappings, TreeView treeView, string filter)		
        {
            methodMappings.showInTreeView(treeView, filter,false, false);
        }
		
        public static void showInTreeView(this MethodMappings methodMappings, TreeView treeView, string filter, bool showSourceCodeSnippets, bool onlyShowSourceCodeLine)		
        {
            treeView.parent().backColor("LightPink");
            treeView.visible(false);
            treeView.clear();
            var indexedMappings = methodMappings.indexedByKey(filter);
            if (onlyShowSourceCodeLine)
            {			
                //do this so that we don't add more than one item per line
                var indexedByFileAndLine = new Dictionary<string, MethodMapping>();
				
                foreach(var item in indexedMappings)
                    foreach(var methodMapping in item.Value)
                        if (methodMapping.File.valid())
                        {
                            var key = "{0}_{1}".format(methodMapping.File, methodMapping.Start_Line);
                            indexedByFileAndLine.add(key, methodMapping);														
                        }
                // now group then by the same text in the SourceCodeLine		
                var indexedBySourceCodeLine =  new Dictionary<string, List<MethodMapping>>();				
                foreach(var methodMapping in indexedByFileAndLine.Values)								
                    indexedBySourceCodeLine.add(methodMapping.sourceCodeLine(), methodMapping);				
				
                //Finally show then
                foreach(var item  in indexedBySourceCodeLine)
                {
                    var uniqueTextNode = treeView.add_Node(item.Key, item.Value,true);
                }
            }
            else
            {
                foreach(var item in indexedMappings)
                {
                    var keyNodeText = "{0}                       ({1})".format(item.Key, item.Value.size());
                    var keyNode= treeView.add_Node(keyNodeText, item.Value,true); 																
                }
                treeView.afterSelect<List<MethodMapping>>(
                    (mappings)=>{
                                    var keyNode = treeView.selected();
                                    keyNode.clear();
                                    foreach(var methodMapping in mappings)
                                    {					
                                        var nodeText = (showSourceCodeSnippets) 
                                            ? methodMapping.sourceCodeLine()								
                                            : "{0} - {1}".format(methodMapping.INodeType,methodMapping.SourceCode);
                                        keyNode.add_Node(nodeText, methodMapping);
                                    }
                    });									
            }
            treeView.parent().backColor("Control");
            treeView.visible(true);
        }

    }
}