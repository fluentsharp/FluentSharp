using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;

namespace FluentSharp.WinForms
{
    public static class WinForms_ExtensionMethods_LinkLabel
    { 
        public static LinkLabel add_Link(this Control control, string text, int top, int left, MethodInvoker onClick)
        {
            return (LinkLabel)control.invokeOnThread(
                ()=>{
                        var link = new LinkLabel();
                        link.Left = left;
                        link.Top = top;
                        link.Text = text;
                        link.AutoSize = true;
                        link.LinkClicked += 
                            (sender, e)=> { 
                                              if (onClick != null) 
                                                  O2Thread.mtaThread(()=> onClick()); 
                            };
                        control.Controls.Add(link);
                        return link;
                });

        }
        public static LinkLabel append_Link(this Control control, string text, MethodInvoker onClick)
        {
            return control.Parent.add_Link(text, control.Top, control.Left + control.Width + 5, onClick);
        }
        public static void click(this LinkLabel linkLabel)
        {
            var e = new LinkLabelLinkClickedEventArgs((LinkLabel.Link)(linkLabel.prop("FocusLink")));
            linkLabel.invoke("OnLinkClicked", e);
        }
        public static List<LinkLabel> links(this Control control)
        {
            return control.controls<LinkLabel>(true);
        }		
        public static LinkLabel link(this Control control, string text)
        {
            foreach(var link in control.links())
                if (link.get_Text() == text)
                    return link;
            return null;
        }		
        public static LinkLabel onClick(this LinkLabel link, Action callback)
        {
            link.invokeOnThread(	
                ()=>{
                        link.Click += (sender,e) => callback();
                });
            return link;
        }
    }
}