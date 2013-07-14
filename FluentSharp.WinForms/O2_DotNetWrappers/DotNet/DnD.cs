// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using FluentSharp.CoreLib.API;

namespace FluentSharp.WinForms.Utils
{
    public class Dnd
    {
        #region Delegates

        public delegate void dDnDActionCallBack(DnDAction ddActionObject);

        #endregion

        #region Action enum

        public enum Action
        {
            SourceCodeAction,
            SelectedFileAction,
            ObjectData
        }

        #endregion

        public static DragAndDropAction getSelectedFileAction(String sSelectedFile)
        {
            var dActionData = new Dictionary<String, object>();
			dActionData.Add("Path", sSelectedFile);
            var dddaSourceCode = new DragAndDropAction(Action.SelectedFileAction, dActionData);
            return dddaSourceCode;
        }

        public static DragAndDropAction getSourceCodeAction(String sPathToSourceCodeFile)
        {
            var dActionData = new Dictionary<String, object>();
			dActionData.Add("Path", sPathToSourceCodeFile);
			dActionData.Add("Contents", Files.getFileContents(sPathToSourceCodeFile));
            var dddaSourceCode = new DragAndDropAction(Action.SourceCodeAction, dActionData);
            return dddaSourceCode;
        }

        public static DragAndDropAction getObjectDataAction(Object oObjectToSend)
        {
            var dActionData = new Dictionary<String, object>();
			dActionData.Add("Object", oObjectToSend);
            var dddaSourceCode = new DragAndDropAction(Action.ObjectData, dActionData);
            return dddaSourceCode;
        }

        public static Object getGetObjectFromDroppedData(DragEventArgs deaDragEventArgs, String sTypeToFind)
        {
            var oDataReceived = new List<object>();
            String[] sFormats = deaDragEventArgs.Data.GetFormats();
            foreach (string sFormat in sFormats)
                oDataReceived.Add(deaDragEventArgs.Data.GetData(sFormat));
            foreach (Object oObject in oDataReceived)
                if (oObject != null)
                    if (oObject.GetType().Name == sTypeToFind)
                        return oObject;
            return null;
        }

        public static object tryToGetObjectFromDroppedObject(DragEventArgs dragEventArgs)
        {
            var dataReceived = new List<object>();
            string[] sFormats = dragEventArgs.Data.GetFormats();
            foreach (string sFormat in sFormats)
                dataReceived.Add(dragEventArgs.Data.GetData(sFormat));
            return (dataReceived.Count > 0) ? dataReceived[0] : null;
        }

        public static object tryToGetObjectFromDroppedObject(DragEventArgs dragEventArgs, Type typeToFind)
        {
            var dataReceived = new List<object>();
            string[] sFormats = dragEventArgs.Data.GetFormats();
            foreach (string sFormat in sFormats)
                dataReceived.Add(dragEventArgs.Data.GetData(sFormat));

            foreach (object item in dataReceived)
                if (item!= null && item.GetType() == typeToFind)
                    return item;
            return null;
        }

        public static String tryToGetFileOrDirectoryFromDroppedObject(DragEventArgs dragEventArgs)
        {
            return tryToGetFileOrDirectoryFromDroppedObject(dragEventArgs, true);
        }

        public static String tryToGetFileOrDirectoryFromDroppedObject(DragEventArgs dragEventArgs, bool downloadIfHttp)
        {
            var dataReceived = new List<object>();
            String[] sFormats = dragEventArgs.Data.GetFormats();
            foreach (string sFormat in sFormats)
                dataReceived.Add(dragEventArgs.Data.GetData(sFormat));

            foreach (object item in dataReceived)
            {
                if (item != null)
                    switch (item.GetType().Name)
                    {
                        case "String":
                            if (File.Exists(item.ToString()) || Directory.Exists(item.ToString()))
                                return item.ToString();                            
                            if ( item.ToString().ToLower().StartsWith("http"))
                            {
                                if (downloadIfHttp)
                                {
                                    var savedUrlContents = new Web().saveUrlContents(item.ToString());
                                    if (savedUrlContents != "" && File.Exists(savedUrlContents))
                                        return savedUrlContents;
                                }
                                return item.ToString();
                            }
                            break;
                        case "String[]":
                            foreach (string subItem in (string[]) item)
                                if (File.Exists(subItem) || Directory.Exists(subItem))
                                    return subItem;
                            break;
                    }
            }
            return "";
        }

        public static void setEffect(DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        #region Nested type: DnDAction

        public class DnDAction
        {
            public Action aAction;
            public dDnDActionCallBack dCallback;
            public DateTime dtReceived;
            public DateTime dtSent;
            public Object oReceiver;
            public Object oSender;
        }

        #endregion

        #region Nested type: DnDActionObjectData

        public class DnDActionObjectData : DnDAction
        {
            public Object oPayload;

            public DnDActionObjectData(Object oPayload)
            {
                aAction = Action.ObjectData;
                this.oPayload = oPayload;
            }

            public Object getPayload()
            {
                return oPayload;
            }

            public void setPayload(Object _payload)
            {
                oPayload = _payload;
            }
        }

        #endregion

        #region Nested type: DragAndDropAction

        public class DragAndDropAction
        {
            public Action aAction;
            public Dictionary<String, Object> dActionData;

            public DragAndDropAction(Action aAction, Dictionary<String, Object> dActionData)
            {
                this.aAction = aAction;
                this.dActionData = dActionData;
            }
        }

        #endregion
    }
}
