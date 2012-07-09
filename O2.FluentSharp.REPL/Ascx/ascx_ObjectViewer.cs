// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Drawing;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Text;
using O2.Kernel;

using O2.DotNetWrappers.DotNet;
using O2.DotNetWrappers.ExtensionMethods;
using O2.Views.ASCX.ExtensionMethods;
using O2.Views.ASCX.classes.MainGUI;
using O2.External.SharpDevelop.Ascx;
using O2.External.SharpDevelop.ExtensionMethods;

namespace O2.XRules.Database.Utils
{
    public class test_ascx_ObjectViewer : ContainerControl
    {
        public static void launchGui()
        {
            //var _object = new List<string>().add("aaa").add("bbb").add("ccc");
            var _object = new List<object>().add(PublicDI.config)
                                            .add("aaa")
                                            .add(AppDomain.CurrentDomain);
            _object.Add("789".wrapOnList().add("456").add("123"));

            _object.showObject();

        }
    }

    public class ascx_ObjectViewer : ContainerControl
    {
        public Object RootObject { get; set; }
        public bool ShowMethods { get; set; }
        public bool ShowPropertyAndFieldInfo { get; set; }
        public bool Sorted { get; set; }
        public bool ShowSerializedString { get; set; }
        public bool SimpleView { get; set; }
        public bool CreateObjectWhenNull { get; set; }

        public PropertyGrid propertyGrid;
        public TreeView treeView;
        public ascx_SourceCodeViewer serializedString;
        public CheckBox showSerializedString_CheckBox;
        public CheckBox createObjectWhenNull_CheckBox;
        public CheckBox simpleView_CheckBox;

        public bool guiReady;
        public ascx_ObjectViewer()
        {
            //showMethods = true;
            //showPropertyAndFieldInfo = true;
            this.Width = 400;
            this.Height = 300;
            buildGui();
        }

        public void buildGui()
        {
            try
            {
                var topPanel = this.add_Panel();
                serializedString = topPanel.insert_Right(200).add_GroupBox("Serialized Object").add_SourceCodeViewer();
                var serializedStringPanel = serializedString.splitContainer().panel2Collapsed(true);
                propertyGrid = topPanel.add_GroupBox("").add_PropertyGrid().helpVisible(false);
                treeView = propertyGrid.parent().insert_Left<Panel>().add_TreeView().sort(); ;
                //treeView.splitterDistance(300);
                var toStringValue = propertyGrid.parent().insert_Below<Panel>(100).add_GroupBox("ToString Value (not editable):").add_TextArea();
                var optionsPanel = treeView.insert_Below<Panel>(70);
                var objectName = toStringValue.parent().insert_Above<Panel>(20).add_TextBox("name", "");
                propertyGrid.onValueChange(updateSerializedString);

                //setSerializedStringSyncWithPropertyGrid();

                serializedString.insert_Above(20).add_Link("Sync Serialized String With PropertyGrid", 0, 0, () => updateSerializedStringSyncWithPropertyGrid());

                LinkLabel invokeLink = null;
                invokeLink = optionsPanel.add_CheckBox("Show Methods", 0, 0,
                                (value) =>
                                {
                                    ShowMethods = value;
                                    invokeLink.enabled(value);
                                    refresh();
                                })
                                         .append_Link("invoke", invokeSelectedMethod)
                                            .leftAdd(-16).topAdd(5).bringToFront()
                                            .enabled(false);

                optionsPanel.add_CheckBox("Show Property and Field info", 22, 0,
                    (value) =>
                    {
                        ShowPropertyAndFieldInfo = value;
                        refresh();
                    })
                            .autoSize()
                            .append_Link("refresh", () => refresh())
                            .left(200);
                optionsPanel.add_CheckBox("Sort Values", 0, 135,
                    (value) =>
                    {
                        Sorted = value;
                        try
                        {
                            treeView.sort(Sorted);  // throwing "Unable to cast object of type 'System.Boolean' to type 'System.Windows.Forms.TreeView'"
                        }
                        catch//(Exception ex)
                        {
                            //ex.log();
                        }
                    }).@checked(true);

                simpleView_CheckBox = optionsPanel.add_CheckBox("Simple View", 0, 220,
                    (value) =>
                    {
                        SimpleView = value;
                        //propertyGrid.splitContainer().panel1Collapsed(value);
                        refresh();
                    })
                            .bringToFront();

                showSerializedString_CheckBox = optionsPanel.add_CheckBox("Show serialized string", 44, 0,
                    (value) =>
                    {
                        ShowSerializedString = value;
                        serializedStringPanel.panel2Collapsed(!value);
                        refresh();
                    })
                             .autoSize();

                createObjectWhenNull_CheckBox = optionsPanel.add_CheckBox("Create Object When Null", 44, 150,
                    (value) =>
                    {
                        CreateObjectWhenNull = value;
                    })
                            .bringToFront()
                            .autoSize();

                treeView.afterSelect<object>(
                    (selectedObject) =>
                    {
                        var treeNode = treeView.selected();
                        objectName.set_Text(treeNode.get_Text());
                        var tag = WinForms_ExtensionMethods_TreeView.get_Tag(treeNode);
                        if (tag.notNull())// && tag.str() != treeNode.get_Text())
                        {
                            propertyGrid.show(selectedObject);
                            var toString = selectedObject.str();
                            if (toString == "System.__ComObject")
                                toString += " : {0}".format(selectedObject.comTypeName());
                            toStringValue.set_Text(toString);

                            propertyGrid.parent().set_Text(selectedObject.typeFullName());
                            if (treeNode.nodes().size() == 0)
                            {
                                addObjectPropertyAndFields(treeNode, selectedObject);
                            }
                        }
                        else if (treeNode.nodes().size() == 0)
                        {
                            propertyGrid.show(null);
                            propertyGrid.parent().set_Text("[null value]");
                            toStringValue.set_Text("[null value]");
                            objectName.set_Text("");
                            treeNode.color(Color.Red);
                        }

                    });
                treeView.add_ContextMenu().add_MenuItem("Invoke Method", () => invokeSelectedMethod());
            }
            catch (Exception ex)
            {
                ex.log("in buildGui", true);
            }
            guiReady = true;
        }

        public void addObjectPropertyAndFields(TreeNode targetNode, object targetObject)
        {
            if (targetObject is String)  // skip strings
                return;

            if (targetObject is IDictionary)
            {
                var dictionary = (IDictionary)targetObject;
                var index = 0;
                foreach (var key in dictionary.Keys)
                {
                    if (key is String)
                        targetNode.add_Node(key.str(), dictionary[key]);
                    else
                    {
                        index++;
                        var value = dictionary[key];
                        targetNode.add_Node("key_{0}: {1}".format(index, key.str()), key);
                        targetNode.add_Node("value_{0}: {1}".format(index, value.str()), value);
                    }
                }
                targetNode.expand();
            }
            else
                if (targetObject is IEnumerable)
                {
                    try
                    {
                        foreach (var item in (targetObject as IEnumerable))
                            targetNode.add_Node(item);
                        targetNode.expand();
                    }
                    catch (Exception ex)
                    {
                        ex.log("in addObjectPropertyAndFields IEnumerable loop");
                    }
                }
                else
                {
                    if (SimpleView)
                    {
                        foreach (var property in targetObject.type().properties())
                        {
                            var propertyValue = PublicDI.reflection.getProperty(property, targetObject);

                            var newNode = targetNode.add_Node(property.Name, propertyValue, false);
                            switch (property.PropertyType.FullName)
                            {
                                case "System.String":
                                case "System.String[]":
                                case "System.Boolean":
                                case "System.DateTime":
                                case "System.Int32":
                                case "System.Int32[]":
                                case "System.Byte":
                                case "System.Int64":
                                    newNode.color(Color.Gray);
                                    break;
                                default:
                                    if (propertyValue.isNull())
                                    {
                                        if (CreateObjectWhenNull)
                                        {
                                            propertyValue = property.PropertyType.ctor();
                                            if (propertyValue.notNull())
                                            {
                                                "CREATE object for type: {0}".debug(propertyValue.type());
                                                PublicDI.reflection.setProperty(property, targetObject, propertyValue);
                                                WinForms_ExtensionMethods_TreeView.set_Tag(newNode, propertyValue);
                                            }
                                            else
                                                "Could not create instance of type: {0}".error(propertyValue.type());
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                    else
                    {
                        //var objectNode = targetNode.add_Node(targetObject.str(), targetObject);				
                        targetNode.add_Node("properties", null).add_Nodes(targetObject.type().properties(),
                                                                    (item) => item.Name,
                                                                    (item) => PublicDI.reflection.getProperty(item, targetObject),
                                                                    (item) => false);

                        targetNode.add_Node("fields", null).add_Nodes(targetObject.type().fields(),
                                                                    (item) => item.Name,
                                                                    (item) => targetObject.field(item.Name), //PublicDI.reflection.getField(item,_object),   
                                                                    (item) => false);
                        targetNode.set_Text("{0}             ({1} properties {2} fields)"
                            .format(targetNode.get_Text(),
                                    targetNode.nodes()[1].nodes().size(),
                                    targetNode.nodes()[0].nodes().size()));
                    }
                }
            if (ShowPropertyAndFieldInfo)
            {
                targetNode.add_Node("_PropertyInfo(s)", null).add_Nodes(targetObject.type().properties(),
                                                        (item) => item.Name);
                targetNode.add_Node("_FieldInfo(s)", null).add_Nodes(targetObject.type().fields(),
                                                        (item) => item.Name);
            }
            if (ShowMethods)
            {
                targetNode.add_Node("MethodInfo(s)", null).add_Nodes(targetObject.type().methods(),
                                                        (item) => item.Name);
            }

        }

        public void addFirstObject(object targetObject)
        {
            if (targetObject.notNull())
            {
                var objectNode = treeView.rootNode().add_Node(targetObject.str(), targetObject);
                addObjectPropertyAndFields(objectNode, targetObject);
                objectNode.expand();
                treeView.selectFirst();
                updateSerializedString();
            }
        }

        public ascx_ObjectViewer simpleView()
        {
            simpleView_CheckBox.@checked(true);
            return this;
        }

        public ascx_ObjectViewer createObjectWhenNull()
        {
            createObjectWhenNull_CheckBox.@checked(true);
            return this;
        }

        public ascx_ObjectViewer showSerializedString()
        {
            return showSerializedString(true);
        }
        public ascx_ObjectViewer showSerializedString(bool value)
        {
            showSerializedString_CheckBox.@checked(value);
            return this;
        }

        public void updateSerializedString()
        {
            if (guiReady && ShowSerializedString)
            {
                var serializedText = this.RootObject.serialize(false);
                if (serializedText.valid())
                {
                    this.serializedString.enabled(true);
                    if (this.serializedString.get_Text() != serializedText)
                        this.serializedString.set_Text(serializedText, "a.xml");
                }
                else
                    this.serializedString.enabled(false);
            }
        }

        public void updateSerializedStringSyncWithPropertyGrid()
        {
            //this.serializedString.onTextChange(
            //	(text)=>{ 	
            if (guiReady && ShowSerializedString)
            {
                try
                {
                    var text = serializedString.get_Text();
                    var newObject = Serialize.getDeSerializedObjectFromXmlFile(text.save(), RootObject.type());
                    if (newObject.notNull())
                    {
                        RootObject = newObject;
                        refresh();
                    }
                }
                catch (Exception ex)
                {
                    ex.log();
                }
            }
            //	});

        }

        public void show(object _object)
        {
            viewObject(_object);
        }

        public void viewObject(object _object)
        {
            if (_object.isNull())
                "in ascx_ObjectViewer provided object was null".error();
            else
            {
                RootObject = _object;
                //addFirstObject(_object);
                refresh();
            }
        }

        public void refresh()
        {
            treeView.clear();
            propertyGrid.show(null);
            addFirstObject(RootObject);
        }

        public void invokeSelectedMethod()
        {
            var selectedNodeTag = WinForms_ExtensionMethods_TreeView.get_Tag(treeView.selected());
            if (selectedNodeTag is MethodInfo)
            {
                var methodToInvoke = (MethodInfo)selectedNodeTag;
                "invoking Method: {0}".info(methodToInvoke);
                "using root object: {0}".info(this.RootObject);

                //PublicDI.reflection.invoke(targetObject,methodToInvoke);
                var result = RootObject.invoke(methodToInvoke.Name);
                "invocation result: {0}".debug(result);
                if (result.notNull())
                    Kernel.show.info(result);
            }
            else
                "Selected Node was not a Method, it was {0}".error(selectedNodeTag.typeName());
        }
    }

    public static class ascx_ObjectViewer_ExtensionMethods
    {

        public static T showDetails<T>(this T _object)
        {
            return _object.showObject();
        }

        public static T objectDetails<T>(this T _object)
        {
            return _object.showObject();
        }

        public static T viewObject<T>(this T _object)
        {
            return _object.showObject();
        }

        public static T showObject<T>(this T _object)
        {
            if (_object.isNull())
                "in showObject object provided was null".error();
            else
            {
                var formTitle = "Object Viewer = {0}".format(_object.type());
                var objectViewer = O2Gui.open<ascx_ObjectViewer>(formTitle, 800, 400);
                objectViewer.show(_object);
            }
            return _object;
        }
    }


    public static class _Extra_ObjectDetails_ExtensionMethods
    {        
        public static void details<T>(this T _object)
        {
            O2Thread.mtaThread(() => _object.showObject());
        }
    }
}
