// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Reflection;
using System.Windows.Forms;
using FluentSharp.CoreLib.Interfaces;

namespace FluentSharp.WinForms.O2Findings
{
    public class OzasmtMappedToWindowsForms
    {
        public static void showO2FindingInDataGridView(O2Finding o2Finding, DataGridView dataGridView)
        {
        }

        public static void showO2TraceInTreeView(O2Trace o2Trace, DataGridView dataGridView, String showItem)
        {
        }

        public static void showO2TraceInDataGridView(O2Trace o2Trace, DataGridView dataGridView)
        {
        }

        public static void loadIntoToolStripCombox_O2FindingFieldsNames(ToolStripComboBox comboBox, string defaultValue)
        {
            comboBox.Items.Clear();
            foreach (PropertyInfo property in typeof (O2Finding).GetProperties())
                comboBox.Items.Add(property.Name);
            comboBox.Text = defaultValue;
        }

        public static void loadIntoToolStripCombox_O2TraceTypes(ToolStripComboBox comboBox)
        {
            comboBox.Items.Clear();

            foreach (object value in Enum.GetValues(typeof (TraceType)))
                comboBox.Items.Add(value);
        }
    }
}
