// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FluentSharp.CoreLib.API;


namespace FluentSharp.WinForms.Utils
{
    public class O2Forms_ThreadSafe_ToolStrip
    {        
        public static bool hasToolStripControl(object hostControl)
        {            
            if (hostControl is Control)                            
            {
                foreach (var field in PublicDI.reflection.getFields(hostControl.GetType()))                
                    if (field.FieldType != null && field.FieldType == typeof (ToolStrip))
                        return true;                                    
                return false;
            }
            PublicDI.log.error("In hasToolStripControl, control object provide is NOT Windows Control");
            return false;
        }

        public static ToolStrip getToolStripControl(object hostControl)
        {
            foreach (var field in PublicDI.reflection.getFields(hostControl.GetType()))
                if (field.FieldType != null && field.FieldType == typeof(ToolStrip))
                    return (ToolStrip)PublicDI.reflection.getFieldValue(field, hostControl);
            return null;
        }

        public static List<ToolStripItem> getItems(object hostControl)
        {
            var items = new List<ToolStripItem>();
            var toolStrip = getToolStripControl(hostControl);
            if (toolStrip != null)
            {
                foreach (ToolStripItem item in toolStrip.Items)
                    items.Add(item);
            }
            return items;
        }

        public static ToolStripItem getItem(object hostControl, string itemToGet)
        {
            var items = getItems(hostControl);
            foreach(var item in items)
                if (item.Name == itemToGet)
                    return item;
            return null;
        }

        public static ToolStripItem getItem(object control, Type typeToGet)
        {
            var items = getItems(control);
            foreach (var item in items)
                if (item.GetType() == typeToGet)
                    return item;
            return null;
        }

        public static ToolStripItem addTextBox(object control, String controlName, string defaultValue)
        {
            return addTextBox(control, controlName, defaultValue,null);
        }


        public static ToolStripItem addTextBox(object control, String controlName, string defaultValue, KeyEventHandler onKeyUp)
        {
            var toolStrip = getToolStripControl(control);
            if (toolStrip != null)
            {
                return (ToolStripItem)toolStrip.invokeOnThread(
                    () =>
                    {
                        var newTextBox = new ToolStripTextBox(controlName);
                        newTextBox.Text = defaultValue;
                        toolStrip.Items.Add(newTextBox);
                        if (onKeyUp != null)
                            newTextBox.KeyUp += onKeyUp;
                        return newTextBox;
                    });
            }
            return null;
        }   
                       
        public static ToolStripItem addLabel(object control, String labelName, string labelText)
        {
            var toolStrip = getToolStripControl(control);
            if (toolStrip != null)
            {
                return (ToolStripItem)toolStrip.invokeOnThread(
                    () =>
                    {
                        var newLabel = new ToolStripLabel(labelName);
						newLabel.Text = labelText;
						newLabel.Name = labelName;
                        toolStrip.Items.Add(newLabel);                        
                        return newLabel;
                    });
            }
            return null;
        }   

        public static bool removeControl(object control, String controlName)
        {
            var toolStrip = getToolStripControl(control);
            if (toolStrip != null)
            {
                return (bool) toolStrip.invokeOnThread(
                                  () =>
                                      {
                                          var itemToRemove = getItem(control, controlName);
                                          if (itemToRemove != null)
                                          {
                                              toolStrip.Items.Remove(itemToRemove);
                                              return true;
                                          }
                                          return false;
                                      });
            }
            return false;
        }
    }
}
