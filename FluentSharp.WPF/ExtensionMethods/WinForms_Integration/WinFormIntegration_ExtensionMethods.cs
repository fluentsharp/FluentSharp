// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using FluentSharp.WinForms;
using FluentSharp.CoreLib;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using FluentSharp.WPF.Controls;

namespace FluentSharp.WPF
{
    public static class WinFormIntegration_ExtensionMethods
    {   
    	//#region generic 
    	
        public static ElementHost add_Wpf(this System.Windows.Forms.Control winFormsControl)
        {
            return winFormsControl.add_WPF_Host();
        }

        public static ElementHost add_WpfHost(this System.Windows.Forms.Control winFormsControl)
        {
            return winFormsControl.add_WPF_Host();
        }

        public static ElementHost add_WPF_Host(this System.Windows.Forms.Control winFormsControl)
        {
            var xamlHost = winFormsControl.add_Control<ascx_Xaml_Host>();
            return xamlHost.element();
        }

    	public static T add_WPF_Control<T>(this System.Windows.Forms.Control winFormsControl) where T : UIElement
    	{
    		var xamlHost = winFormsControl.add_Control<ascx_Xaml_Host>();
    		return (T)xamlHost.add_Control<T>();    		
    	}
    	
    	public static T add_Control<T>(this ascx_Xaml_Host xamlHost) where T : UIElement
    	{    		
    		return (T)xamlHost.invokeOnThread(
    			()=>{
    					try
			            {
	    					var wpfControl = typeof(T).ctor();					    				
	    					if (wpfControl is UIElement)
	    					{
	    						xamlHost.element().Child = (UIElement)wpfControl;
	    						return (T)wpfControl;			    								    					
	    					}
						}			
			            catch (Exception ex)
			            {
			                ex.log("in add_Control");
			            }
			            return null;			    				
					});
    	}
    	
    	public static ElementHost clear(this ElementHost elementHost)
    	{
    		return (ElementHost)elementHost.invokeOnThread(()=>{  elementHost.Child = null; return elementHost;} );
    	}


    	//#region Add WPF Controls to ascx_Xml_Host or Controls

    	public static Label add_WPF_Label(this System.Windows.Forms.Control winFormsControl, string text)
    	{    		
    		var label = winFormsControl.add_WPF_Control<Label>();
    		label.set_Text_Wpf(text);
    	
    		return label;
    	}    	
    	    
		
    	public static Label add_Label(this ascx_Xaml_Host xamlHost, string text)
    	{
    		var label = add_Control<Label>(xamlHost);
            label.set_Text_Wpf(text);
    		//"in add_Label".debug();
    		return label;
    	} 
    	
    	public static Grid add_Grid(this ascx_Xaml_Host xamlHost)
    	{
    		return add_Control<Grid>(xamlHost);            
    	}
    	
		public static StackPanel add_StackPanel(this ascx_Xaml_Host xamlHost)
    	{
    		return add_Control<StackPanel>(xamlHost);            
    	}
    	
    	//#region WinForms inside WPF 
    				
        public static System.Windows.Forms.Panel add_WinForms_Panel(this UIElement uiElement)
        {
            return uiElement.add_WinFormToWPF<System.Windows.Forms.Panel>();
        }

        public static T add_WinFormToWPF<T>(this UIElement uiElement)
            where T : System.Windows.Forms.Control
        {
            return (T)uiElement.wpfInvoke(
            () =>
            {
                try
                {
                    var controlHost = uiElement.add_Control_Wpf<WindowsFormsHost>();
                    var winFormsControl = (System.Windows.Forms.Control)typeof(T).ctor();
                    if (winFormsControl.isNull())
                        "in UIElement add_WinFormToWPF, new control of Type {0} could not be created".error(typeof(T).typeFullName());
                    else
                    {
                        winFormsControl.width(400) 		// give the panel a decent size to that it doesn't cause problems during the new T Control dynamic Gui creation
                                       .height(400);
                        //"in UIElement add_WinFormToWPF, new control created ok: {0}".info(typeof(T).typeFullName());
                        winFormsControl.Dock = System.Windows.Forms.DockStyle.Fill;
                        controlHost.Child = winFormsControl;
                        return (T)winFormsControl;
                    }
                }
                catch (Exception ex)
                {
                    ex.log("in UIElement add_WinFormToWPF");

                }
                return null;
            });
        }


        //removed to remove depencies to FluentSharp.WinForms
        /*
        // WPF Winforms TreeView
        public static System.Windows.Forms.TreeView showStructureInTreeView(this UIElement uiElement, System.Windows.Forms.TreeView treeView)
        {
            treeView.clear();
            uiElement.showStructureInTreeNode(treeView.rootNode());
            return treeView;
        }

        public static System.Windows.Forms.TreeNode showStructureInTreeNode(this UIElement uiElement, System.Windows.Forms.TreeNode treeNode)
        {
            var uiElementNode = treeNode.add_Node(uiElement.typeName());
            uiElement.wpfInvoke(
                () =>
                {
                    if (uiElement is ContentControl)
                    {
                        var content = (uiElement as ContentControl).Content;
                        if (content is UIElement)
                            (content as UIElement).showStructureInTreeNode(uiElementNode);
                    }
                    if (uiElement is Panel)
                    {
                        var children = (uiElement as Panel).Children;
                        foreach (var child in children)
                            if (child is UIElement)
                                (child as UIElement).showStructureInTreeNode(uiElementNode);

                        //if (content is UIElement)
                        //	(content as UIElement).showStructureInTreeNode(uiElementNode);
                        //uiElementNode.add_Node("{0} children".format(children.size()));
                    }
                });
            return uiElementNode;
        }
        */
		//#endregion

    }
}
