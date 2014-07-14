using System.Collections.Generic;
using System.Windows.Forms;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.WinForms;

namespace FluentSharp.REPL
{
    public static class CSharp_FastCompiler_ExtensionMethods_WinForms_Helpers
    {

        //add links (which execute o2 scripts		
        public static LinkLabel add_Link_To_H2Script(this Control control, string label, string script)
        {
            return control.add_Link(label, ()=> script.executeH2Script());
        }		
        public static LinkLabel append_Link_To_H2Script(this Control control, string label, string script)
        {
            return control.append_Link(label, ()=> script.executeH2Script());
        }		
        public static LinkLabel append_Below_Link_To_H2Script(this Control control, string label, string script)
        {
            return control.append_Below_Link(label, ()=> script.executeH2Script());
        }		


        //ComboBox execution
        public static ComboBox add_ExecutionComboBox(this Control control, string labelText, int top, int left, Items scriptMappings)
        {
            return control.add_ExecutionComboBox(labelText, top, left, scriptMappings, scriptMappings.keys());
        }		
        public static ComboBox add_ExecutionComboBox(this Control control, string labelText, int top, int left, Items scriptMappings, List<string> comboBoxItems)
        {						
            // ReSharper disable CoVariantArrayConversion
            return control.add_Label(labelText, top, left)
                .append_Control<ComboBox>().top(top-3).dropDownList() // .width(100)		 				  
                .add_Items(comboBoxItems.insert("... select one...").ToArray())
                .executeScriptOnSelection(scriptMappings)		 							
                .selectFirst(); 
            // ReSharper restore CoVariantArrayConversion
        }		
        public static ComboBox executeScriptOnSelection(this ComboBox comboBox, Items mappings)
        {			
            comboBox.onSelection<string>(
                (key)=>{
                           if (mappings.hasKey(key))
                           {
                               comboBox.parent().focus();// do this in order to prevent a nasty user experience that happens if the user uses the up and down arrows to navigate the comboBox	
                               "executing script mapped to '{0}: {1}".info(key, mappings[key]);
                               var itemToExecute = mappings[key];
                               if (itemToExecute.isUri())
                                   Processes.startProcess(itemToExecute);
                                   //itemToExecute.startProcess();
                               else
                               {
                                   if(itemToExecute.fileExists().isFalse() && itemToExecute.local().fileExists().isFalse())																							
                                       CompileEngine.clearLocalScriptFileMappings();											
                                   "itemToExecute: {0}".debug(itemToExecute);	
                                   //"itemToExecuteextension: {0}".debug(itemToExecute.extension(".o2"));
                                   if (itemToExecute.extension(".h2") || itemToExecute.extension(".o2"))											
                                       if (Control.ModifierKeys == Keys.Shift)
                                       {
                                           "Shift Key pressed, so launching in new process: {0}".info(itemToExecute);
                                           itemToExecute.executeH2_or_O2_in_new_Process();
                                           return;
                                       }
												
/*												else
												{
													"Shift Key was pressed, so running script in current process".debug(itemToExecute);													
													O2Thread.mtaThread(()=>itemToExecute.executeFirstMethod());
												}
											else*/

                                   O2Thread.mtaThread(()=>itemToExecute.executeFirstMethod());
											
                               }                                        
                           }
                });		
            return comboBox;			
        }                
    }
}