// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using FluentSharp.CoreLib.API;
using FluentSharp.WinForms.Utils;

namespace FluentSharp.WinForms.Controls
{
    public partial class DropObject : UserControl
    {
        public bool bAcceptNonO2ObjectsAndFireThemAsObjects = true;

        public Color cDragOutColor = Color.Maroon;
        public Color cDragOverColor = Color.Red;
        public Form cHostForm;
        public Object oTag; // could be used to store an object value

        public DropObject()
        {
            InitializeComponent();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [Bindable(true)]
        public override string Text
        {
            get { return lbDropObjectText.Text; }

            set { lbDropObjectText.Text = value; }
        }

        public event Callbacks.dMethod_Object eStartExecution_Event;
        public event Callbacks.dMethod_Object eDnDAction_ObjectDataReceived_Event;

        private void DropObject_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
            BackColor = cDragOverColor;
        }

        private void DropObject_DragLeave(object sender, EventArgs e)
        {
            BackColor = cDragOutColor;
        }

        private void DropObject_DragDrop(object sender, DragEventArgs e)
        {
            BackColor = cDragOutColor;
            //    o2Messages.sendMessage_DnDQueue_SendInfoMessage("A drop item was received");
            var oDataReceived = new List<object>();
            String[] sFormats = e.Data.GetFormats();
            foreach (string sFormat in sFormats)
                oDataReceived.Add(e.Data.GetData(sFormat));
            foreach (Object oObject in oDataReceived)
            {
                Type tObjectBaseType = oObject.GetType().BaseType;
                if (tObjectBaseType != null && tObjectBaseType == typeof (Dnd.DnDAction))
                {
                    processReceivedDnDActionObject((Dnd.DnDAction) oObject);
                    return;
                }
            }
            if (bAcceptNonO2ObjectsAndFireThemAsObjects)
            {
                // check if it there is a file name in there:


                if (oDataReceived.GetType().Name == "List`1")
                {
                    List<Object> lObject = oDataReceived;
                    foreach (Object oItem in lObject)
                    {
                        String sdata = oItem.ToString();
                        if (oItem.GetType().Name == "String")
                        {
                            if (File.Exists(sdata))
                            {
                                //        Messages.sendMessage_DnDQueue_SendInfoMessage("Data drop was a file");
                                eDnDAction_ObjectDataReceived_Event(sdata);
                                return;
                            }
                        }
                        if (oItem.GetType().Name == "String[]")
                        {
                            var asStrings = (String[]) oItem;
                            foreach (String sString in asStrings)
                            {
                                if (File.Exists(sString))
                                {
                                    //           Messages.sendMessage_DnDQueue_SendInfoMessage("Data drop was a file");
                                    eDnDAction_ObjectDataReceived_Event(sString);
                                    return;
                                }
                                if (Directory.Exists(sString))
                                {
                                    //               Messages.sendMessage_DnDQueue_SendInfoMessage("Data drop was a Directory");
                                    eDnDAction_ObjectDataReceived_Event(sString);
                                    return;
                                }
                            }
                        }
                    }
                }
                // if not sent it as an object
                //     Messages.sendMessage_DnDQueue_SendInfoMessage(
                //         "Unrecognized DnD object was droped, but since bAcceptNonO2ObjectsAndFireThemAsObjects is set, we will invoke the events using the received object");

                invokeObjectDataReceivedEventWithDataReceived(oDataReceived);
            }
            //  else
            //      Messages.sendMessage_DnDQueue_SendInfoMessage("Error: wrong type was dropped on the current host");
        }

        public void invokeObjectDataReceivedEventWithDataReceived(List<Object> oDataReceived)
        {
            if (eDnDAction_ObjectDataReceived_Event != null)
            {
                if (oDataReceived.Count == 1)
                    eDnDAction_ObjectDataReceived_Event(oDataReceived[0]); // send the first object
                else
                    eDnDAction_ObjectDataReceived_Event(oDataReceived); // send the data received
            }
        }

        public void processReceivedDnDActionObject(Dnd.DnDAction ddActionObject)
        {
            if (cHostForm == null)
                ddActionObject.oReceiver = this;
            else
                ddActionObject.oReceiver = cHostForm;
            ddActionObject.dtReceived = DateTime.Now;
            ddActionObject.dCallback = ddActionStartExecution;

            // if the ddAction is also fire this event (since this will be what is needed for most cases            
            if (ddActionObject.GetType().Name == "DnDActionObjectData")
            {
                var fdndAction_ObjectData = (Dnd.DnDActionObjectData) ddActionObject;
                if (eDnDAction_ObjectDataReceived_Event != null)
                    eDnDAction_ObjectDataReceived_Event(fdndAction_ObjectData.oPayload); // just send the payload
            }
            else
                //  NEED TO DOUBLE CHECK IMPACT OF THIS (before this would always fire and was before the o2DnDAction_ObjectData above
            {
                if (eStartExecution_Event != null)
                    eStartExecution_Event(ddActionObject);
            }


            //o2Messages.sendMessage_DnDQueue_AddAction(ddActionObject);            // for now just invoke the event callback
        }

        public void ddActionStartExecution(Dnd.DnDAction ddActionObject)
        {
            /*if (ddActionObject.aAction == Dnd.Action.ObjectData)
            {
                var ddaObjectData = (Dnd.DnDActionObjectData) ddActionObject;
            }
            else*/
            PublicDI.log.error("DnDAction Not Suported by this host: {0}", ddActionObject.aAction.ToString());
        }

        public void setText(String sText)
        {
            lbDropObjectText.Text = sText;
        }

        public void setDragOverColor(Color cColor)
        {
            cDragOverColor = cColor;
        }

        public void setDragOutColor(Color cColor)
        {
            cDragOutColor = cColor;
        }
    }
}
