// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using FluentSharp.CoreLib.API;
using FluentSharp.CoreLib.Interfaces;
using FluentSharp.WinForms.O2Findings;
using FluentSharp.WinForms.Utils;

namespace FluentSharp.WinForms.Controls
{
    public partial class ascx_FindingEditor
    {
        private bool runOnLoad = true;
        private bool o2FindingLoadedViaSerializedData;
        private IO2Finding currentO2Finding;
        private IO2Trace currentO2Trace;


        private void onLoad()
        {
            if (runOnLoad && DesignMode == false)
            {
                updateVisibilityOfSaveButton();
                if (dgvFindingsDetails.Columns.Count == 0) // only do this once
                {
                    O2Forms.addToDataGridView_Column(dgvFindingsDetails, "Name", 100);
                    O2Forms.addToDataGridView_Column(dgvFindingsDetails, "Value", -1);
                    dgvFindingsDetails.Columns[0].ReadOnly = true;

                    O2Forms.addToDataGridView_Column(dgvTraceDetails, "Name", 100);
                    O2Forms.addToDataGridView_Column(dgvTraceDetails, "Value", -1);
                    dgvTraceDetails.Columns[0].ReadOnly = true;

                    OzasmtMappedToWindowsForms.loadIntoToolStripCombox_O2TraceTypes(cbCurrentO2TraceType);

                    //newO2Finding();
                    runOnLoad = false;

                    ascxTraceTreeView.onTreeNodeAfterSelect += ascx_TraceTreeView_onTreeNodeAfterSelect;
                    ascxTraceTreeView.onTreeNodeAfterLabelEdit += ascxTraceView_onTreeNodeAfterLabelEdit;
                }
            }
        }

        void ascxTraceView_onTreeNodeAfterLabelEdit(string newValue)
        {
            if (newValue != null)
            {
                getCellWithCurrentO2TraceText("signature").Value = newValue;
                getCellWithCurrentO2TraceText("clazz").Value = newValue;
            }
        }

        void ascx_TraceTreeView_onTreeNodeAfterSelect(object selectedNode)
        {
            if (selectedNode != null && selectedNode is O2Trace)
                showO2Trace((O2Trace)selectedNode);
        }

        public void loadO2Finding(object o2Finding)
        {
            if (o2Finding.GetType().Name != "O2Finding")
                PublicDI.log.error("in loadO2Finding type of value provided was not correct: {0}",
                             o2Finding.GetType().FullName);
            else
                loadO2Finding((O2Finding)o2Finding);
        }

        public void loadSerializedO2Finding(string serializedO2Finding)
        {
            var deserializedObject = (IO2Finding)Serialize.getDeSerializedObjectFromString(serializedO2Finding, typeof(IO2Finding));
            if (deserializedObject != null)
                loadO2Finding(deserializedObject, true);  //fromSerializedData = true, since we can't save O2Finding objects that were loaded using serizalized strings
        }

        private void loadO2Finding(IO2Finding o2Finding, bool fromSerializedData)
        {
            o2FindingLoadedViaSerializedData = fromSerializedData;
            loadO2Finding(o2Finding);

        }
        public void loadO2Finding(IO2Finding o2Finding)
        {
            currentO2Finding = o2Finding;
            // show O2Finding In DataGrid View
            showO2FindingDetails();
            updateVisibilityOfSaveButton();
        }

        public void showO2FindingDetails()
        {
            this.invokeOnThread(() =>
                                    {
                                        {
                                            if (currentO2Finding != null)
                                            {
                                                dgvFindingsDetails.Visible = false;
                                                dgvFindingsDetails.Rows.Clear();
                                                foreach (
                                                    PropertyInfo property in currentO2Finding.GetType().GetProperties())
                                                {
                                                    if (property.Name != "o2Trace" && property.Name != "getSource" &&
                                                        property.Name != "getSink" &&
                                                        property.Name != "getLostSink" &&
                                                        property.Name != "getKnownSink")
                                                    {
                                                        var newRow = new DataGridViewRow();
                                                        var cellName = new DataGridViewTextBoxCell();
                                                        cellName.Value = property.Name;
                                                        var cellValue = new DataGridViewTextBoxCell();
														cellValue.Value = PublicDI.reflection.getProperty(property.Name, currentO2Finding);
														cellValue.ValueType = property.PropertyType;                                                        
                                                        newRow.Cells.AddRange(new[] {cellName, cellValue});

                                                        dgvFindingsDetails.Rows.Add(newRow);
                                                    }
                                                }

                                                dgvFindingsDetails.Visible = true;

                                                ascxTraceTreeView.loadO2Finding(currentO2Finding);
                                            }
                                        }
                                    });
        }

        public void showO2Trace(IO2Trace o2Trace)
        {
            this.invokeOnThread(()=>{
                                        currentO2Trace = o2Trace;
                                        btSaveChangesToTrace.Visible = false;
                                        if (o2Trace != null)
                                        {
                                            dgvTraceDetails.Visible = false;
                                            dgvTraceDetails.Rows.Clear();

                                            foreach (PropertyInfo property in o2Trace.GetType().GetProperties())
                                            {
                                                if (property.Name != "childTraces")
                                                {
                                                    var newRow = new DataGridViewRow();
                                                    var cellName = new DataGridViewTextBoxCell();
                                                    cellName.Value = property.Name;
													var cellValue = new DataGridViewTextBoxCell();
													cellValue.Value = PublicDI.reflection.getProperty(property.Name, o2Trace);
													cellValue.ValueType = property.PropertyType;

                                                    newRow.Cells.AddRange(new[] {cellName, cellValue});
                                                    dgvTraceDetails.Rows.Add(newRow);
                                                }
                                            }
                                            cbCurrentO2TraceType.Text = getCellWithCurrentO2TraceText("traceType").Value.ToString();
                                            dgvTraceDetails.Visible = true;
                                        }
                                    });
        }

       


        public object getValueFromCell(DataGridViewCell cell)
        {
            object value = null;

            switch (cell.ValueType.Name)
                //need to do this because row.Cells["Value"].ValueType returns a string value for all cells
            {
                case "Boolean":
                    value = bool.Parse(cell.EditedFormattedValue.ToString());
                    break;
                case "Byte":
                    value = byte.Parse(cell.EditedFormattedValue.ToString());
                    break;
                case "List`1":
                    value = new List<String>(new[] { cell.EditedFormattedValue.ToString() });
                    break;
                case "UInt32":
                    value = UInt32.Parse(cell.EditedFormattedValue.ToString());
                    break;
                case "String":
                    value = cell.EditedFormattedValue.ToString();
                    break;
                case "TraceType":
                    value = Enum.Parse(typeof(TraceType), cell.Value.ToString());
                    break;
                default:
                    break;
            }
            return value;
        }

        public void saveCurrentO2Finding()
        {
            PublicDI.log.info("Saving changes made to CurrentO2Finding");
            foreach (DataGridViewRow row in dgvFindingsDetails.Rows)
            {
                object value = getValueFromCell(row.Cells["Value"]);
                if (value != null)
                    PublicDI.reflection.setProperty(row.Cells["Name"].Value.ToString(), currentO2Finding, value);
                //currentO2Finding.setField(row.Cells["Name"].Value.ToString(), value);
            }
        }

        public void saveCurrentO2Trace()
        {
            PublicDI.log.info("Saving changes made to CurrentO2Trace");
            foreach (DataGridViewRow row in dgvTraceDetails.Rows)
            {
                object value = getValueFromCell(row.Cells["Value"]);
                if (value != null)
                    PublicDI.reflection.setProperty(row.Cells["Name"].Value.ToString(), currentO2Trace, value);
                //currentO2Trace.setField(row.Cells["Name"].Value.ToString(), value);
            }
            if (getCellWithCurrentO2TraceText("signature").Value.ToString() != "")
                ascxTraceTreeView.selectedNode.Text = getCellWithCurrentO2TraceText("signature").Value.ToString();
            ascxTraceTreeView.selectedNode.ForeColor = OzasmtUtils.getTraceColorBasedOnTraceType(currentO2Trace);
            //showO2TraceTree();
            //currentO2Finding
        }


        public DataGridViewCell getCellWithCurrentO2TraceText(string name)
        {
            foreach (DataGridViewRow row in dgvTraceDetails.Rows)
                if (row.Cells[0].Value.ToString() == name)
                    return row.Cells[1];
            return null;
        }




        public static void openInFloatWindow(IO2Finding o2Finding)
        {
            O2Thread.mtaThread(
                () =>
                    {
                        var windowName = "Finding Editor for: " + o2Finding;
                        O2Messages.openControlInGUISync(typeof (ascx_FindingEditor), O2DockState.Float,
                                                    windowName);
                        O2Messages.getAscx(windowName, guiControl =>
                                                           {
                                                               if (guiControl != null &&
                                                                   guiControl is ascx_FindingEditor)
                                                               {
                                                                   var findingEditor =
                                                                       (ascx_FindingEditor) guiControl;
                                                                   findingEditor.loadO2Finding(
                                                                       o2Finding, false);
                                                               }
                                                           });
                    });

        }

        private void updateVisibilityOfSaveButton()
        {
            this.invokeOnThread(() =>
                                    {
                                        btSaveChanges.Visible = !o2FindingLoadedViaSerializedData &&
                                                                !cbAutoSaveChanges.Checked;

                                        btSaveChangesToTrace.Visible = false;

                                        llSaveAndDrag.Text = (!o2FindingLoadedViaSerializedData)
                                                                 ? "Drag"
                                                                 : "Save and Drag";
                                        cbAutoSaveChanges.Visible = !o2FindingLoadedViaSerializedData;

                                    });
        }

        private void handleDragDrop(DragEventArgs e)
        {
            
            var o2Finding = (O2Finding)Dnd.tryToGetObjectFromDroppedObject(e, typeof(O2Finding));
            if (o2Finding != null)            
                loadO2Finding(o2Finding,false);
            else
            {
                var treeNode = (TreeNode)Dnd.tryToGetObjectFromDroppedObject(e, typeof(TreeNode));
                if (treeNode != null)
                    if (treeNode.Tag != null && treeNode.Tag.GetType().Name == "O2Finding")
                        loadO2Finding((O2Finding)treeNode.Tag,false);
            }
        }

        

        public void openFileViewer(string file, int lineToSelect)
        {
        }

        public void newO2Finding()
        {
            currentO2Finding= new O2Finding();
            //ascxTraceTreeView.o2Finding = new O2Finding();
            //ascxTraceTreeView.appendNewO2Trace();
            //currentO2Finding.o2Trace = new O2Trace();
            showO2FindingDetails();
        }
    }
}
