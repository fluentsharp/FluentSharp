using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using FluentSharp.CoreLib;
using FluentSharp.WinForms.Controls;

namespace FluentSharp.WinForms
{
    public static class ComObjects_TreeView_ExtensionMethods
    {
        //note:the results of this are not consistent
        public static TreeView showInfo_ComObject(this  object _rootObject)
        {
            var treeView = O2Gui.open<Panel>("showInfo_ComObject",400,400).add_TreeView();
            var propertyGrid = treeView.insert_Below<Panel>().add_PropertyGrid();
			
            Action<TreeNode, object> add_Object =
                (treeNode,_object)=>{
                                        treeNode.clear();									
                                        //treeNode.add_Node(_object.str(), _object, true);
                                        WinForms_ExtensionMethods_TreeView.add_Node(treeNode,_object.str(), _object, true);
                };
            Action<TreeNode, IEnumerable> add_Objects = 
                (treeNode,items)=>{
                                      treeNode.clear();
                                      foreach(var item in items)
                                          //treeNode.add_Node(item.str(), item, true);
                                          WinForms_ExtensionMethods_TreeView.add_Node(treeNode, item.str(), item, true);
                };
			
			 
            treeView.beforeExpand<object>(
                (treeNode, _object)=>{		
                                         if (_object is String)
                                             treeNode.add_Node(_object); 
                                         else
                                         {
                                             if (_object is IEnumerable)
                                                 add_Objects(treeNode, _object as IEnumerable);
                                             else
                                                 foreach(PropertyDescriptor property in TypeDescriptor.GetProperties(_object))
                                                 {
                                                     try
                                                     {
                                                         var value =  property.GetValue(_object);
                                                         treeNode.add_Node(property.Name.str(),value,true);
                                                     }
                                                     catch(Exception ex)
                                                     {
                                                         treeNode.add_Node(property.Name.str(),"O2 ERROR:".format(ex.Message) ,false);
                                                     }
                                                 }
                                         }
                });
			
            treeView.afterSelect<object>(
                (_object)=> propertyGrid.show(_object));
				
            if(_rootObject is IEnumerable)
                add_Objects(treeView.rootNode(), _rootObject as IEnumerable);  
            else
                add_Object(treeView.rootNode(), _rootObject);  
            return treeView;
        }
    }
}