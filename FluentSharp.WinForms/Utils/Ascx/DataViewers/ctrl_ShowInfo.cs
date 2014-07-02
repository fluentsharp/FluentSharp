// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;

namespace FluentSharp.WinForms.Controls
{
    public class ctrl_ShowInfo : UserControl
    {      
    	public PropertyGrid propertyGrid;    	    	
 
 		/*public void test() 		
 		{
 			var info = O2Gui.open<ascx_ShowInfo>("methods");
 			info.show(info.type().methods_public());
 		}*/
        public ctrl_ShowInfo()
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
                //do this here due to threading issues that can occur when running the .str in the ctrl_ShowInfo thread
                var mappedItems = new List<NameValuePair<string,object>>();
                var items = (_object as IEnumerable).toList();
                foreach(var item in items)
                    mappedItems.add(item.str(), item);
               
                this.invokeOnThread(() =>
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

                    //show mappedItems
                    if (mappedItems.size() > 1000)
                    { 
                        mappedItems = mappedItems.take(1000);
                        treeView.add_Node("__NOTE__: since there where {0} items, only the first 1000 are shown here".format(items.size()));
                    }
                    treeView.beginUpdate();
                    foreach(var mappedItem in mappedItems)                                            
                        treeView.add_Node(mappedItem.Key,mappedItem.Value);                    
                    treeView.endUpdate();
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
