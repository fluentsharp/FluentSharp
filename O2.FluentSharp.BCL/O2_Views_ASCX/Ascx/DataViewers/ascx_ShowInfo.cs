// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Reflection;
using System.Text;
using O2.Interfaces.O2Core;
using O2.Kernel;
using O2.Kernel.ExtensionMethods;
using O2.DotNetWrappers.ExtensionMethods;
using O2.DotNetWrappers.DotNet;
using O2.DotNetWrappers.Windows;
using O2.Views.ASCX;
using O2.Views.ASCX.classes.MainGUI;

namespace O2.Views.ASCX.DataViewers
{
    public class ascx_ShowInfo : UserControl
    {      
    	public PropertyGrid propertyGrid;    	    	
 
 		/*public void test() 		
 		{
 			var info = O2Gui.open<ascx_ShowInfo>("methods");
 			info.show(info.type().methods_public());
 		}*/
        public ascx_ShowInfo()
    	{    	
    		this.Width = 400;
    		this.Height = 300;
    		buildGui();
    	}
    	
    	public PropertyGrid buildGui()
    	{
            return (PropertyGrid)this.invokeOnThread(
    			()=>{
    		            this.clear();
    		            propertyGrid = this.add_Control<PropertyGrid>();    		
                        return propertyGrid;
                });
    	}

        public void show(object _object)
        {
            buildGui();
            if (propertyGrid == null)
            {
                "in show, propertyGrid was null)".error();
                return;
            }
            if (_object is IEnumerable)
            {
                this.invokeOnThread(
                () =>
                {
                    var treeView = this.add_TreeView();
                    var sliderDistance = propertyGrid.Width /2;
                    if (sliderDistance > 200)
                        sliderDistance = 200;
                    propertyGrid.insert_Left(treeView, sliderDistance);
                    var textBox = this.add_TextBox(true);
                    treeView.insert_Above(textBox, 25);
                    //config
                    textBox.ScrollBars = ScrollBars.None;
                    textBox.Multiline = false;

                    ((SplitContainer)textBox.Parent.Parent).Panel1MinSize = 20;
                    ((SplitContainer)textBox.Parent.Parent).SplitterDistance = 20;

                    //events
                    treeView.afterSelect<object>((tag) => showInPropertyGrid(tag));
                    textBox.onTextChange(
                        (text) => treeView.add_Nodes((IEnumerable)_object, true, text).Sort());
                    // populate treeview
                    var contextMenu = treeView.add_ContextMenu();
                    contextMenu.add_MenuItem("Copy To Clipboard: Selected Node Text", (item) => { treeView.SelectedNode.Text.toClipboard(); });
                    var items = (_object as IEnumerable).toList();
                    treeView.add_Nodes(items).sort();
                });
            }
            else
            {
                showInPropertyGrid(_object);
            }
        }
    	    	    	
    	public void showInPropertyGrid(object _object)
    	{
    	//	if (_object is Control)
    	//		((Control)_object).invokeOnThread(()=> propertyGrid.show(_object));
    	//	else                
    			propertyGrid.show(_object);
    	}
    	
    	
    	    	    	    	    
    }
}
