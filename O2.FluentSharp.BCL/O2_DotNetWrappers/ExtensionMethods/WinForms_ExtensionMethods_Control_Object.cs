using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using O2.DotNetWrappers.DotNet;
using O2.Kernel;
using O2.DotNetWrappers.ExtensionMethods;
using System.Drawing;
using O2.DotNetWrappers.Windows;

namespace O2.DotNetWrappers.ExtensionMethods
{
    public static class WinForms_ExtensionMethods_Control_Object
    {

        //add
        public static T add<T>(this Control hostControl, T childControl) where T : Control
        {
            return (T)hostControl.invokeOnThread(
                    () =>
                    {
                        hostControl.Controls.Add(childControl);
                        return childControl;
                    });
        }
        public static T add<T>(this Control hostControl, params int[] position) where T : Control
        {
            return hostControl.add_Control<T>(position);
        }
        public static T add_Control<T>(this Control hostControl, T childControl) where T : Control
        {
            return (T)hostControl.add(childControl);
        }
        public static Control add_Control(this Control control, Type childControlType)            
        {
            return (Control)control.invokeOnThread(
                                 () =>
                                 {
                                     try
                                     {
                                         var childControl = (Control)PublicDI.reflection.createObjectUsingDefaultConstructor(childControlType);
                                         if (control != null)
                                         {
                                             childControl.Dock = DockStyle.Fill;
                                             control.Controls.Add(childControl);
                                             return childControl;
                                         }
                                     }
                                     catch (Exception ex)
                                     {
                                         ex.log("in Control.add_Control");
                                     }
                                     return null;
                                 });
        }          
        public static T add_Control<T>(this Control hostControl, params int[] position) where T : Control
        {
            var values = new[] { -1, -1, -1, -1 };
            for (int i = 0; i < position.Length; i++)
                values[i] = position[i];
            return hostControl.add_Control<T>(values[0], values[1], values[2], values[3]);
        }
        public static T add_Control<T>(this Control hostControl, int top, int left, int width, int height) where T : Control
        {
            return (T)hostControl.invokeOnThread(
                () =>
                {
                    try
                    {
                        var newControl = (Control)typeof(T).ctor();
                        if (newControl != null)
                        {
                            if (top == -1 && left == -1 && width == -1 && height == -1)
                                newControl.fill();
                            else
                            {
                                if (top > -1)
                                    newControl.Top = top;
                                if (left > -1)
                                    newControl.Left = left;
                                if (width > -1)
                                    newControl.Width = width;
                                if (height > -1)
                                    newControl.Height = height;
                            }
                            hostControl.Controls.Add(newControl);
                            return newControl;
                        }
                    }
                    catch (Exception ex)
                    {
                       // ex.log("in add_Control<T>");  // can't realy log this since it will introduce an race-condition
                        System.Diagnostics.Debug.WriteLine("in add_Control<T>: " + ex.Message);
                    }
                    return null;
                });
        }
        public static List<T> add_Controls<T>(this Control control, List<T> controlsToAdd) where T : Control
        {
            foreach (var controlToAdd in controlsToAdd)
                control.add_Control(controlToAdd);
            return controlsToAdd;
        }
        public static T append_Control<T>(this Control control) where T : Control
        {
            return control.Parent.add_Control<T>(control.Top, control.Left + control.Width + 5);
        }


        //Add nXn
        public static List<Control> add_1x1(this Control control)
        {
            return control.add_1x1("", "");
        }
        public static List<Control> add_1x1(this Control control, bool verticalSplit)
        {
            var splitterDistance = (verticalSplit)                                        
                                        ? control.Width / 2
                                        : control.Height / 2;

            return control.add_1x1("", "", verticalSplit, splitterDistance);
        }
        public static List<Control> add_1x1(this Control control, string title1, string title2)
        {
            return control.add_1x1(title1, title2, true, control.width() / 2);
        }
        public static List<Control> add_1x1(this Control control, string title1, string title2, bool verticalSplit, int distance1)
        {
            return control.add_SplitContainer_1x1(title1, title2, verticalSplit, distance1);
        }
        public static List<Control> add_1x1(this Control hostControl, string title_1, string title_2, Control control_1, Control control_2)
        {
            var groupBoxes = hostControl.add_1x1(title_1, title_2);
            groupBoxes[0].Controls.Add(control_1);
            groupBoxes[1].Controls.Add(control_2);
            groupBoxes.Add(control_1);		// also add these controls to the list to controls returned
            groupBoxes.Add(control_2);
            return groupBoxes;
        }        
        public static List<Control> add_1x1(this Control control, Action<Control> buildPanel1,  Action<Control> buildPanel2)
		{
			var controls = control.add_1x1();
			buildPanel1(controls[0].add_Panel());
			buildPanel2(controls[1].add_Panel());
			return controls;
		}
        public static List<GroupBox> add_1x1x1(this Control control, string title1, string title2, string title3)
        {
            return control.add_1x1x1(title1, title2, title3, true);
        }
        public static List<GroupBox> add_1x1x1(this Control control, string title1, string title2, string title3, bool verticalSplit)
        {
            var panels = control.add_1x1x1(verticalSplit);
            var groupBox1 = panels[0].add_GroupBox(title1);
            var groupBox2 = panels[1].add_GroupBox(title2);
            var groupBox3 = panels[2].add_GroupBox(title3);
            return groupBox1.wrapOnList().add(groupBox2).add(groupBox3);
        }
        public static List<Panel> add_1x1x1(this Control control)
        {
            return control.add_1x1x1(true);
        }
        public static List<Panel> add_1x1x1(this Control control, bool verticalSplit)
        {

            var spliterDistance = ((verticalSplit) ? control.width() : control.height()) / 3;
            var panel3 = control.add_Panel();
            var panel2 = (verticalSplit) ? panel3.insert_Left<Panel>(spliterDistance)
                                         : panel3.insert_Above<Panel>(spliterDistance);
            var panel1 = (verticalSplit) ? panel3.insert_Left<Panel>(spliterDistance)
                                         : panel3.insert_Above<Panel>(spliterDistance);
            panel2.parent<SplitContainer>().splitterDistance(spliterDistance);
            panel1.parent<SplitContainer>().splitterDistance(spliterDistance);
            return panel2.wrapOnList().add(panel1).add(panel3);
        }
        public static List<Control> add_1x2(this Control control, string title1, string title2, string title3)
        {
            return control.add_1x2(title1, title2, title3, true, control.Height / 2, control.Width / 2);
        }
        public static List<Control> add_1x2(this Control control, string title1, string title2, string title3, bool align, int distance1, int distance2)
        {
            return control.add_SplitContainer_1x2(title1, title2, title3, align, distance1, distance2);
        }
        public static List<Control> add_1x2(this Control control, string title1, string title2, string title3, bool verticalSplit)
        {
            return control.add_1x2(title1, title2, title3, verticalSplit, control.Height / 2, control.Width / 2);
        }
        public static List<Control> add_1x2<T1, T2, T3>(this Control control, bool verticalSplit)            where T1 : Control            where T2 : Control            where T3 : Control
        {
            return control.add_1x2<T1, T2, T3>(typeof(T1).Name, typeof(T2).Name, typeof(T3).Name, verticalSplit);
        }
        public static List<Control> add_1x2<T1, T2, T3>(this Control control, string title1, string title2, string title3)            where T1 : Control            where T2 : Control            where T3 : Control
        {
            return control.add_1x2<T1, T2, T3>(title1, title2, title3, true);
        }
        public static List<Control> add_1x2<T1, T2, T3>(this Control control, string title1, string title2, string title3, bool verticalSplit)            where T1 : Control            where T2 : Control            where T3 : Control
        {
            var controls = control.add_1x2(title1, title2, title3, verticalSplit);
            controls[0] = controls[0].add_Control<T1>();
            controls[1] = controls[1].add_Control<T2>();
            controls[2] = controls[2].add_Control<T3>();
            return controls;
        }
        public static List<Control> add_1x1<T1, T2>(this Control control, string title1, string title2)            where T1 : Control            where T2 : Control
        {
            var controls = control.add_1x1(title1, title2);
            controls[0] = controls[0].add_Control<T1>();
            controls[1] = controls[1].add_Control<T2>();
            return controls;
        }
        public static List<Control> add_1x1(this Control control, string title1, string title2, Action<Control> host1Callback, Action<Control> host2Callback)
        {
            var controls = control.add_1x1(title1, title2);
            host1Callback(controls[0]);
            host2Callback(controls[1]);
            return controls;
        }
        public static List<Control> add_1x1(this Control control, string title1, string title2, bool verticalSplit, int splitterDistance, Action<Control> host1Callback, Action<Control> host2Callback)
        {
            var controls = control.add_1x1(title1, title2, verticalSplit, splitterDistance);
            host1Callback(controls[0]);
            host2Callback(controls[1]);
            return controls;
        }
        public static List<Control> add_1x1(this Control control, string title1, string title2, bool verticalSplit)
        {
            var controls = control.add_1x1(title1, title2);
            var splitContainer = controls.parent<SplitContainer>();
            if (verticalSplit)                            
                splitContainer.verticalOrientation();
            else
                splitContainer.horizontalOrientation();

            var splitterDistance = (verticalSplit)
                                        ? control.Width / 2
                                        : control.Height / 2;
            splitContainer.splitterDistance(splitterDistance);

            controls.Add(splitContainer);
            return controls;
        }        
        public static List<Control> add_SplitContainer_1x1(this Control control, string title_1, string title_2, bool verticalSplit, int spliterDistance)
        {
            SplitContainer splitControl_1 = control.add_SplitContainer(
                verticalSplit, //setOrientationToHorizontal
                true, // setDockStyleoFill
                true); // setBorderStyleTo3D
            return (List<Control>)splitControl_1.invokeOnThread(
                () =>
                {
                    if (spliterDistance > splitControl_1.Panel1MinSize && spliterDistance < splitControl_1.Width - splitControl_1.Panel2MinSize)
                        splitControl_1.SplitterDistance = spliterDistance;
                    else
                        "In add_SplitContainer_1x1, could not set the spliterDistance value of: {0}".format(spliterDistance).error();

                    GroupBox groupBox_1 = splitControl_1.Panel1.add_GroupBox(title_1);
                    GroupBox groupBox_2 = splitControl_1.Panel2.add_GroupBox(title_2);
                    return new List<Control> { groupBox_1, groupBox_2 };
                });
        }
        public static GroupBox add_SplitContainer_1x1(this Control control, Control childControl_1, string title_2, bool verticalSplit, int spliterDistance)
        {
            SplitContainer splitControl_1 = control.add_SplitContainer(
                verticalSplit, //setOrientationToHorizontal
                true, // setDockStyleoFill
                true); // setBorderStyleTo3D
            splitControl_1.SplitterDistance = spliterDistance;
            splitControl_1.Panel1.Controls.Add(childControl_1);
            GroupBox groupBox_2 = splitControl_1.Panel2.add_GroupBox(title_2);
            return groupBox_2;
        }
        public static GroupBox add_SplitContainer_1x1(this Control control, string title_1, Control childControl_2, bool verticalSplit, int spliterDistance)
        {
            SplitContainer splitControl_1 = control.add_SplitContainer(
                verticalSplit, //setOrientationToHorizontal
                true, // setDockStyleoFill
                true); // setBorderStyleTo3D
            return (GroupBox)splitControl_1.invokeOnThread(
                () =>
                {
                    splitControl_1.SplitterDistance = spliterDistance;
                    GroupBox groupBox_1 = splitControl_1.Panel1.add_GroupBox(title_1);
                    splitControl_1.Panel2.Controls.Add(childControl_2);
                    return groupBox_1;
                });
        }
        public static SplitContainer add_SplitContainer_1x1(this Control control, Control childControl_1, Control childControl_2, bool verticalSplit, int spliterDistance)
        {
            SplitContainer splitControl_1 = control.add_SplitContainer(
                verticalSplit, //setOrientationToHorizontal
                true, // setDockStyleoFill
                true); // setBorderStyleTo3D
            splitControl_1.SplitterDistance = spliterDistance;
            splitControl_1.Panel1.Controls.Add(childControl_1);
            splitControl_1.Panel2.Controls.Add(childControl_2);
            return splitControl_1;
        }
        public static List<Control> add_SplitContainer_1x2(this Control control, string title_1, string title_2, string title_3, bool verticalSplit, int spliterDistance_1, int spliterDistance_2)
        {
            return (List<Control> )control.invokeOnThread(
                () =>
                {
                    var tempPanel = new Panel();
                    tempPanel.Width = control.Width;
                    tempPanel.Height = control.Height;
                    SplitContainer splitControl_2 = tempPanel.add_SplitContainer(
                        !verticalSplit, //setOrientationToHorizontal
                        true, // setDockStyleoFill
                        true); // setBorderStyleTo3D			
                    splitControl_2.SplitterDistance = spliterDistance_2;
                    GroupBox groupBox_2 = splitControl_2.Panel1.add_GroupBox(title_2);
                    GroupBox groupBox_3 = splitControl_2.Panel2.add_GroupBox(title_3);

                    Control groupBox_1 = control.add_SplitContainer_1x1(title_1, splitControl_2, verticalSplit,
                                                                        spliterDistance_1);

                    var controls = new List<Control> { groupBox_1, groupBox_2, groupBox_3 };
                    return controls;
                });
        }
        public static List<Control> add_SplitContainer_2x2(this Control control, string title_1, string title_2, string title_3, string title_4, bool verticalSplit, int spliterDistance_1, int spliterDistance_2,  int spliterDistance_3)
        {
            var tempPanel = new Panel();
            SplitContainer splitControl_2 = tempPanel.add_SplitContainer(
                !verticalSplit, //setOrientationToHorizontal
                true, // setDockStyleoFill
                true); // setBorderStyleTo3D			

            SplitContainer splitControl_3 = tempPanel.add_SplitContainer(
                !verticalSplit, //setOrientationToHorizontal
                true, // setDockStyleoFill
                true); // setBorderStyleTo3D			

            splitControl_2.SplitterDistance = spliterDistance_2;
            splitControl_3.SplitterDistance = spliterDistance_3;

            GroupBox groupBox_1 = splitControl_2.Panel1.add_GroupBox(title_1);
            GroupBox groupBox_2 = splitControl_2.Panel2.add_GroupBox(title_2);
            GroupBox groupBox_3 = splitControl_3.Panel1.add_GroupBox(title_3);
            GroupBox groupBox_4 = splitControl_3.Panel2.add_GroupBox(title_4);


            control.add_SplitContainer_1x1(splitControl_2, splitControl_3, verticalSplit, spliterDistance_1);

            var controls = new List<Control> {groupBox_1, groupBox_2, groupBox_3, groupBox_4};
            return controls;
        }

        //get
        public static T get<T>(this Control controlToSearch) where T : Control
        {
            foreach (var control in controlToSearch.controls())
                if (control is T)
                    return (T)control;
                else
                {
                    var childMatch = control.get<T>();
                    if (childMatch != null)
                        return childMatch;
                }
            return null;
        }
        public static T get<T>(this List<Control> controls) where T : Control
        {
            foreach (Control control in controls)
                if (control.type() == typeof(T))
                    return (T)control;
            return null;
        }
        public static T castOrCreate<T>(this Control control, object objectToCheck, Func<T> createFunction)
        {
            if (objectToCheck != null && objectToCheck is T)
                return (T)objectToCheck;
            control.clear();
            return createFunction();
        }
        public static Control parent(this Control control)
        {
            return (Control)control.invokeOnThread(
                () =>
                {
                    if (control.notNull())
                        return control.Parent;
                    return null;
                });
        }
        public static T parent<T>(this List<Control> controls) where T : Control
        {
            foreach (var control in controls)
            {
                var match = control.parent<T>();
                if (match != null)
                    return (T)match;
            }
            return null;
        }
        public static T parent<T>(this Control control) where T : Control
        {
            if (control != null && control.Parent != null)
            {
                var parent = control.Parent;
                if (parent is T)
                    return (T)parent;
                var match = parent.parent<T>();
                if (match != null)
                    return (T)match;
            }
            return null;
        }
        public static string get_Text(this Control control)
        {
            return (string)control.invokeOnThread(() => { return control.Text; });
        }
        public static T text<T>(this List<T> controls, string textToFind) where T : Control
        {
            foreach (var control in controls)
                if (control.get_Text() == textToFind)
                    return control;
            return null;
        }
        public static List<string> texts<T>(this List<T> controls)  where T : Control
        {
            return (from control in controls
                    select control.get_Text()).toList();
        }

        //Misc
        public static Form parentForm(this Control control)
        {
            return control.parent<Form>();
        }
        public static T set_Text<T>(this T control, string text) where T : Control
        {
            return (T)control.invokeOnThread(
                () =>
                {

                    control.Text = text;
                    return (T)control;
                });
        }
        public static T createInThread<T>(this Control control) where T : Control
        {
            return control.newInThread<T>();
        }
        public static T newInThread<T>(this Control control) where T : Control
        {
            return (T)control.invokeOnThread(
                () =>
                {
                    try
                    {
                        return (T)typeof(T).ctor();
                    }
                    catch (Exception ex)
                    {
                        ex.log("in Control.invokeOnThread");
                        return null;
                    }
                });
        }
        public static Form newFormInThread(this Control control)
        {
            return control.newFormInThread("New Form in thread of: {0}".format(control.get_Text()));
        }
        public static Form newFormInThread(this Control control, string text)
        {
            return (Form)control.invokeOnThread(
                () =>
                {
                    var newForm = new Form();
                    newForm.set_H2Icon();
                    newForm.Text = text;
                    newForm.Show();
                    return newForm;
                });
        }
        public static T fill<T>(this T control) where T : Control
        {
            return control.fill(true);
        }
        public static T fill<T>(this T control, bool status) where T : Control
        {
            return (T)control.invokeOnThread(
                () =>
                {
                    control.Dock = (status) ? DockStyle.Fill : DockStyle.None;
                    return (T)control;
                });

        }
        public static T enabled<T>(this T control) where T : Control
        {
            return control.enabled(true);
        }
        public static T enabled<T>(this T control, bool state) where T : Control
        {
            return (T)control.invokeOnThread(
                () =>
                {
                    control.Enabled = state;
                    return (T)control;
                });
        }
        public static T visible<T>(this T control) where T : Control
        {
            return control.visible(true);
        }
        public static T visible<T>(this T control, bool state) where T : Control
        {
            if (control.isNull())
                return null;
            return (T)control.invokeOnThread(
                () =>
                {
                    control.Visible = state;
                    return (T)control;
                });
        }
        public static T mapToWidth<T>(this Control hostControl, T control, bool alignToTop) where T : Control
        {
            if (alignToTop)
                control.anchor_TopLeftRight();
            else
                control.anchor_BottomLeftRight();

            const int pad = 5;
            control.Left = hostControl.Left + pad;
            control.Width = hostControl.Width - pad - pad;
            return control;
        }
        public static void close(this Control control)
        {
            control.invokeOnThread(
                () =>
                {
                    if (control is UserControl)
                    {
                        var userControl = (UserControl)control;
                        if (userControl.ParentForm != null)
                            userControl.ParentForm.Close();
                    }
                });
        }
        public static T foreColor<T>(this T control, Color color) where T : Control
        {
            return (T)control.invokeOnThread(
                () =>
                {
                    control.ForeColor = color;
                    return control;
                });
        }
        public static T backColor<T>(this T control, Color color) where T : Control
        {
            return (T)control.invokeOnThread(
                () =>
                {
                    control.BackColor = color;
                    return control;
                });
        }
        public static T backColor<T>(this T control, string colorName) where T : Control
        {
            return control.backColor(Color.FromName(colorName));
        }
        public static List<Control> controls(this Control control)
        {
            return control.controls(false);
        }
        public static List<T> controls<T>(this Control control, bool recursiveSearch) where T : Control
        {
            return (from childControl in control.controls(recursiveSearch)
                    where childControl is T
                    select (T)childControl).toList();
        }
        public static List<Control> controls(this Control control, bool recursiveSearch)
        {
            if (recursiveSearch)
            {    
                var allControls = control.controls();
                foreach(var childControl in control.controls())
                    allControls.AddRange(childControl.controls(true));
                return allControls;
            }
            else
                return control.Controls.list();
        }
        public static T controls<T>(this Control rootControl, int depthToReturn) where T : Control
        {
            var controls = rootControl.controls(depthToReturn);
            return controls.control<T>();
        }
        public static List<Control> controls(this Control control, int depthToReturn)
        {
            var allControls = new List<Control>();
            allControls.add(control);
            for (int i = 0; i < depthToReturn; i++)
                allControls = allControls.controls(false);
            return allControls;
        }
        public static List<T> controls<T>(this List<Control> controls) where T : Control
        {
            return (from control in controls
                    where control is T
                    select (T)control).toList();            
        }
        public static List<Control> controls(this List<Control> controls)
        {
            return controls.controls(false);
        }        
        public static List<Control> controls(this List<Control> controls, bool includeRoot)
        {
            var allControls = new List<Control>();
            foreach (var control in controls)
            {
                if (includeRoot)
                    allControls.add(control);
                allControls.add(control.controls());
            }
            return allControls;
        }
        public static T controls<T>(this Control control) where T : Control
        {
            foreach (var childControl in control.controls())
                if (childControl is T)
                    return (T)childControl;
            return null;
        }
        public static T control<T>(this List<Control> controls) where T : Control
        {
            foreach (var control in controls)
                if (control is T)
                    return (T)control;
            return null;
        }
        public static T control<T>(this Control control) where T : Control
        {
            return control.control<T>(true);
        }
        public static Control control<T>(this T hostControl, string controlName, bool recursive) where T : Control
        {
            var controls = hostControl.controls(recursive);
            foreach (var control in controls)
                if (control.typeName() == controlName)
                    return control;
            return null;
        }
        public static T control<T>(this Control control, bool searchRecursively) where T : Control
        {
            if (control.notNull())
                foreach (var childControl in control.controls(searchRecursively))
                    if (childControl is T)
                        return (T)childControl;
            return null;
        }
        public static List<Control> list(this Control.ControlCollection controlCollection)
        {
            var controls = new List<Control>();
            foreach (Control control in controlCollection)
                controls.Add(control);
            return controls;
        }

        public static T focus<T>(this T control) where T : Control
        {
            return (T)control.invokeOnThread(
                () =>
                {
                    control.Focus();
                    return control;
                });
        }
        public static T top<T>(this T control, int top) where T : Control
        {
            return (T)control.invokeOnThread(
                    () =>
                    {
                        if (top > -1)
                            control.Top = top;
                        return control;
                    });
        }
        public static T left<T>(this T control, int left) where T : Control
        {
            return (T)control.invokeOnThread(
                    () =>
                    {
                        if (left > -1)
                            control.Left = left;
                        return control;
                    });
        }
        public static T width<T>(this T control, int value) where T : Control
        {
            return (T)control.invokeOnThread(
                () =>
                {
                    if (value > -1)
                        control.Width = value;
                    return (T)control;
                });
        }
        public static T height<T>(this T control, int value) where T : Control
        {
            return (T)control.invokeOnThread(
                () =>
                {
                    if (value > -1)
                        control.Height = value;
                    return (T)control;
                });
        }
        public static int width(this Control control)
        {
            return (int)control.invokeOnThread(
                () =>
                {
                    return control.Width;
                });
        }
        public static int height(this Control control)
        {
            return (int)control.invokeOnThread(
                () =>
                {
                    return control.Height;
                });
        }
        public static T widthAdd<T>(this T control, int value) where T : Control
        {
            return (T)control.invokeOnThread(
                () =>
                {
                    control.width(control.Width + value);
                    return control;
                });

        }
        public static T heightAdd<T>(this T control, int value) where T : Control
        {
            return (T)control.invokeOnThread(
                () =>
                {
                    control.height(control.Height + value);
                    return control;
                });

        }
        public static T topAdd<T>(this T control, int value) where T : Control
        {
            return (T)control.invokeOnThread(
                () =>
                {
                    control.top(control.Top + value);
                    return control;
                });

        }
        public static T leftAdd<T>(this T control, int value) where T : Control
        {
            return (T)control.invokeOnThread(
                () =>
                {
                    control.left(control.Left + value);
                    return control;
                });

        }
        public static T putBitmapOnClipboard<T>(this T control, Bitmap bitmap) where T : Control
        {
            control.invokeOnThread(() => Clipboard.SetImage(bitmap));
            return control;
        }
        public static Bitmap fromClipboardGetImage(this Control control)
        {
            return (Bitmap)control.invokeOnThread(
                () =>
                {
                    if (Clipboard.ContainsImage())
                    {
                        return Clipboard.GetImage();
                    }
                    "in fromClipboardGetImage, the Clipboard doesn't contain an image".debug();
                    return null;
                });
        }
        public static T front<T>(this T control) where T : Control
        {
            return control.bringToFront();
        }
        public static T bringToFront<T>(this T control) where T : Control
        {
            control.invokeOnThread(() => control.BringToFront());
            return control;
        }
        public static T back<T>(this T control) where T : Control
        {
            return control.sendToBack();
        }
        public static T sendToBack<T>(this T control) where T : Control
        {
            control.invokeOnThread(() => control.SendToBack());
            return control;
        }

        //events 
        public static T onDrop<T>(this T control, Action<string> onDropFileOrFolder) where T : Control
        {
            return (T)control.invokeOnThread(() =>
                {

                    control.AllowDrop = true;
                    control.DragEnter += (sender, e) => Dnd.setEffect(e);
                    control.DragDrop += (sender, e)
                         =>
                    {
                        var fileOrFolder = Dnd.tryToGetFileOrDirectoryFromDroppedObject(e);
                        if (fileOrFolder.valid())
                            onDropFileOrFolder(fileOrFolder);
                    };
                    return control;
                });
            //
        }

        public static T onHandleCreated<T>(this T control, Action onHandleCreated) where T : Control
        {
            control.HandleCreated += (sender, e) =>
            {
                onHandleCreated();
            };
            return control;
        }

        
        public static T onControlAdded<T>(this T control, Action onControlAdded) where T : Control
        {
            control.ControlAdded += (sender, e) =>
            {
                onControlAdded();
            };
            return control;
        }

        public static T onClosed<T>(this T control, MethodInvoker onClosed) where T : Control
        {
            var parentForm = control.parentForm();
            if (parentForm == null)
            {
                "in Control.onClosed, provided form value was null".error();
                return null;
            }
            parentForm.Closed += (sender, e) => onClosed();
            return control;
        }

        //remove
        public static void remove_ContextMenu(this Control control)
        {
            control.ContextMenuStrip = null;
        }
        public static T clear<T>(this T control) where T : Control
        {
            return (T)control.invokeOnThread(
                () =>
                {
                    if (control.Controls.IsReadOnly)
                        "cannot clear control list for type {0} since it is marked as read only".format(control.typeName()).error();
                        control.Controls.Clear();                    
                    return control;
                });
        }
        public static T removeOtherControls<T>(this Control hostControl, T controlToKeep) where T : Control
        {
            return (T)hostControl.invokeOnThread(
                () =>
                {
                    foreach (var control in hostControl.controls())
                        if (control != controlToKeep)
                            hostControl.Controls.Remove(control);
                    return controlToKeep;
                });
        }             
        public static T remove<T>(this T hostControl, Control controlToRemove) where T : Control
        {
            return (T)hostControl.invokeOnThread(
                () =>
                {
                    foreach (var control in hostControl.controls(true))
                        if (control != controlToRemove)
                            hostControl.Controls.Remove(controlToRemove);
                    return hostControl;
                });
        }        

        //location
        public static T location<T>(this T control, int top) where T : Control
        {
            return control.location(top, -1, -1, -1);
        }
        public static T location<T>(this T control, int top, int left) where T : Control
        {
            return control.location(top, left, -1, -1);
        }
        public static T location<T>(this T control, int top, int left, int width) where T : Control
        {
            return control.location(top, left, width, -1);
        }
        public static T location<T>(this T control, int top, int left, int width, int height) where T : Control
        {
            return (T)control.invokeOnThread(
                () =>
                {
                    control.Dock = DockStyle.None;
                    if (top > -1)
                        control.Top = top;
                    if (left > -1)
                        control.Left = left;
                    if (top > -1)
                        control.Top = top;
                    if (width > -1)
                        control.Width = width;
                    if (height > -1)
                        control.Height = height;
                    return control;
                });
        }

        // align
        public static T align_Left<T>(this T control, Control controlToAlignWith) where T : Control
        {
            return control.align_Left(controlToAlignWith, 0);
        }
        public static T align_Left<T>(this T control, Control controlToAlignWith, int border) where T : Control
        {
            return (T)control.invokeOnThread(
                () =>
                {
                    control.Dock = DockStyle.None;
                    control.Left = controlToAlignWith.Left + border;
                    control.anchor_Left();
                    return control;
                });
        }
        public static T align_Right<T>(this T control, Control controlToAlignWith) where T : Control
        {
            return control.align_Right(controlToAlignWith, 0);
        }
        public static T align_Right<T>(this T control, Control controlToAlignWith, int border)           where T : Control
        {
            return (T)control.invokeOnThread(
                        () =>
                        {
                            control.Dock = DockStyle.None;
                            control.Width = controlToAlignWith.Width - control.Left - border;
                            control.anchor_Right();
                            return control;
                        });
        }
        public static T align_Top<T>(this T control, Control controlToAlignWith) where T : Control
        {
            return control.align_Top(controlToAlignWith, 0);
        }
        public static T align_Top<T>(this T control, Control controlToAlignWith, int border) where T : Control
        {
            return (T)control.invokeOnThread(
                        () =>
                        {
                            control.Dock = DockStyle.None;
                            control.Top = controlToAlignWith.Top + border;
                            control.anchor_Top();
                            return control;
                        });
        }
        public static T align_Bottom<T>(this T control, Control controlToAlignWith) where T : Control
        {
            return control.align_Bottom(controlToAlignWith, 0);
        }
        public static T align_Bottom<T>(this T control, Control controlToAlignWith, int border) where T : Control
        {
            return (T)control.invokeOnThread(
                        () =>
                        {
                            control.Dock = DockStyle.None;
                            PublicDI.log.debug("controlToAlignWith.Height: {0}", controlToAlignWith.Height);
                            PublicDI.log.debug("control.Top: {0}", control.Top);
                            control.Height = controlToAlignWith.Height - control.Top - border;
                            control.anchor_Bottom();
                            return control;
                        });
        }
        
        // anchor
        public static T anchor<T>(this T control) where T : Control
        {
            control.Anchor = AnchorStyles.None;
            return control;
        }
        public static T anchor_Top<T>(this T control) where T : Control
        {
            control.Anchor = control.Anchor | AnchorStyles.Top;
            return control;
        }
        public static T anchor_Bottom<T>(this T control) where T : Control
        {
            control.Anchor = control.Anchor | AnchorStyles.Bottom;
            return control;
        }
        public static T anchor_Left<T>(this T control) where T : Control
        {
            control.Anchor = control.Anchor | AnchorStyles.Left;
            return control;
        }
        public static T anchor_Right<T>(this T control) where T : Control
        {
            control.Anchor = control.Anchor | AnchorStyles.Right;
            return control;
        }        
        public static T anchor_TopLeft<T>(this T control) where T : Control
        {
            control.anchor().anchor_Top().anchor_Left();
            return control;
        }
        public static T anchor_BottomLeft<T>(this T control) where T : Control
        {
            control.anchor().anchor_Bottom().anchor_Left();
            return control;
        }
        public static T anchor_TopRight<T>(this T control) where T : Control
        {
            control.anchor().anchor_Top().anchor_Right();
            return control;
        }
        public static T anchor_BottomRight<T>(this T control) where T : Control
        {
            control.anchor().anchor_Bottom().anchor_Right();
            return control;
        }
        public static T anchor_TopLeftRight<T>(this T control) where T : Control
        {
            control.anchor().anchor_Top().anchor_Left().anchor_Right();
            return control;
        }
        public static T anchor_BottomLeftRight<T>(this T control) where T : Control
        {
            control.anchor().anchor_Bottom().anchor_Left().anchor_Right();
            return control;
        }
        public static T anchor_All<T>(this T control) where T : Control
        {
            control.anchor().anchor_Top().anchor_Right().anchor_Bottom().anchor_Left();
            return control;
        }


        // insert control
        public static List<Control>     injectControl(this Control controlToWrap, Control controlToInject, AnchorStyles location)
        {
            return controlToWrap.injectControl(controlToInject, location, -1, false, controlToWrap.Text, controlToInject.Text);
        }
        public static List<Control>     injectControl(this Control controlToWrap, Control controlToInject, AnchorStyles location, int splitterDistance, bool border3D)
        {
            return controlToWrap.injectControl(controlToInject, location, splitterDistance, border3D, controlToWrap.Text, controlToInject.Text);
        }
        public static List<Control>     injectControl(this Control controlToWrap, Control controlToInject, AnchorStyles location, int splitterDistance, bool border3D, string title_1, string title_2)
        {
            try
            {
                if (controlToWrap == null || controlToInject == null)
                    return new List<Control>();
                return (List<Control>)controlToWrap.invokeOnThread(
                    () =>
                    {
                        var parentControl = controlToWrap.Parent;
                        parentControl.clear();
                        var controls = new List<Control>();
                        SplitContainer splitContainer = parentControl.add_SplitContainer();
                        splitContainer.fill();
                        if (border3D)
                            splitContainer.border3D();
                        switch (location)
                        {
                            case AnchorStyles.Top:
                            case AnchorStyles.Left:
                                splitContainer.Panel1.add(controlToInject);
                                splitContainer.Panel2.add(controlToWrap);
                                splitContainer.FixedPanel = FixedPanel.Panel1;
                                try
                                {
                                    //if (splitterDistance > -1 && splitterDistance > splitContainer.Panel1MinSize && 
                                    //                             splitterDistance < ( splitContainer.Width -  splitContainer.Panel2MinSize))                                    
                                    if (splitterDistance > 0)
                                    {                                        
                                        splitContainer.SplitterDistance = splitterDistance;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    "Could not set Splitter Distance to value {0} : {1}".format(splitterDistance,ex.Message).error();
                                }
                                splitContainer.Orientation = (location == AnchorStyles.Top) ? Orientation.Horizontal : Orientation.Vertical;
                                break;

                            case AnchorStyles.Bottom:
                            case AnchorStyles.Right:
                                splitContainer.Panel1.add(controlToWrap);
                                splitContainer.Panel2.add(controlToInject);
                                splitContainer.Orientation = (location == AnchorStyles.Bottom) ? Orientation.Horizontal : Orientation.Vertical;
                                splitContainer.FixedPanel = FixedPanel.Panel2;

                                if (splitterDistance > -1)
                                {

                                    var newSplitterDistance = (location == AnchorStyles.Bottom)
                                                                        ? splitContainer.Height - splitterDistance
                                                                        : splitContainer.Width - splitterDistance;
                                    if (newSplitterDistance > 0)
                                    {                                     
                                        splitContainer.SplitterDistance = newSplitterDistance;
                                    }
                                    else
                                        "Could not set Splitter Distance since it was a negative value: {0}".format(newSplitterDistance).error();
                                }                                
                                break;
                            case AnchorStyles.None:
                                PublicDI.log.error("in injectControl the location provided was AnchorStyles.None");
                                break;
                        }
                        controls.add(splitContainer)
                                .add(controlToWrap)
                                .add(controlToInject);

                        return controls;
                    });
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex, "in injectControl");
                return null;
            }
        }

        public static List<Control>     insert_Right(this Control controlToWrap, Control controlToInject, int splitterDistance)
        {
            return controlToWrap.insert_Right(controlToInject, splitterDistance, false);
        }
        public static List<Control>     insert_Right(this Control controlToWrap, Control controlToInject)
        {
            return controlToWrap.injectControl(controlToInject, AnchorStyles.Right);
        }
        public static List<Control>     insert_Right(this Control controlToWrap, Control controlToInject, int splitterDistance, bool border3D)
        {
            return controlToWrap.injectControl(controlToInject, AnchorStyles.Right, splitterDistance, border3D);
        }
        public static T                 insert_Right<T>(this Control control) where T : Control
        {
            return control.insert_Right<T>(-1);
        }
        public static T                 insert_Right<T>(this Control control, int distance) where T : Control
        {
            return (T)control.invokeOnThread(
                () =>
                {
                    var newControl = control.add_Control<T>();
                    newControl.fill();
                    control.insert_Right(newControl, distance);
                    return newControl;
                });
        }
        public static Panel             insert_Right(this Control control)
		{
			return control.insert_Right<Panel>(control.width()/2);
		}		
        public static Panel             insert_Right(this Control control, int width)
		{
			return control.insert_Right<Panel>(width);
		}		
		public static Panel             insert_Right(this Control control, string title)
		{
			return control.insert_Right(control.width()/2, title);
		}		
        public static Panel             insert_Right(this Control control, int width, string title)
		{
			return control.insert_Right<Panel>(width).add_GroupBox(title).add_Panel();
		}		

        public static List<Control>     insert_Left(this Control controlToWrap, Control controlToInject, int splitterDistance)
        {
            return controlToWrap.insert_Left(controlToInject, splitterDistance, false);
        }
        public static List<Control>     insert_Left(this Control controlToWrap, Control controlToInject)
        {
            return controlToWrap.injectControl(controlToInject, AnchorStyles.Left);
        }
        public static List<Control>     insert_Left(this Control controlToWrap, Control controlToInject, int splitterDistance, bool border3D)
        {
            return controlToWrap.injectControl(controlToInject, AnchorStyles.Left, splitterDistance, border3D);
        }
        public static T                 insert_Left<T>(this Control control) where T : Control
        {
            return control.insert_Left<T>(-1);
        }
        /*public static Panel             insert_Left(this Control control, int width)
		{			
			var panel = control.insert_Left<Panel>(width); 
			// to deal with bug in insert_Left<Panel>
			//replace with (when merging extension methods): panel.splitterDistance(width);
			{
				var splitContainer = control.parent<SplitContainer>();
				    WinForms_ExtensionMethods_SplitContainer.splitterDistance(splitContainer,width);
			}
			
			return panel;
		}		*/
        public static Panel             insert_Left(this Control control, int width)
        {
            return control.insert_Left<Panel>(width); 
        }
        public static T                 insert_Left<T>(this Control control, int distance) where T : Control
        {
            return (T)control.invokeOnThread(
                () =>
                {
                    var newControl = control.add_Control<T>();
                    newControl.fill();
                    control.insert_Left(newControl, distance);
                    var splitContainer = control.parent<SplitContainer>();
				    WinForms_ExtensionMethods_SplitContainer.splitterDistance(splitContainer,distance);
                    return newControl;
                });
        }
        public static Panel             insert_Left(this Control control)
		{
			return control.insert_Left(control.width()/2);			
		}		        		
        public static Panel             insert_Left(this Control control, string title)
		{
			return control.insert_Left(control.width()/2, title);
		}		
        public static Panel             insert_Left(this Control control, int width, string title)
		{
			return control.insert_Left<Panel>(width).add_GroupBox(title).add_Panel();
		}		

        public static List<Control>     insert_Above(this Control controlToWrap, Control controlToInject, int splitterDistance)
        {
            return controlToWrap.insert_Above(controlToInject, splitterDistance, false);
        }        
        public static List<Control>     insert_Above(this Control controlToWrap, Control controlToInject)
        {
            return controlToWrap.injectControl(controlToInject, AnchorStyles.Top);
        }
        public static List<Control>     insert_Above(this Control controlToWrap, Control controlToInject, int splitterDistance, bool border3D)
        {
            return controlToWrap.injectControl(controlToInject, AnchorStyles.Top, splitterDistance, border3D);
        }        
        public static T                 insert_Above<T>(this Control control) where T : Control
        {
            return control.insert_Above<T>(-1);
        }
        public static T                 insert_Above<T>(this Control control, int distance) where T : Control
        {
            return (T)control.invokeOnThread(
                () =>
                {
                    //var newControl = control.add_Control<T>();
                    var newControl = control.newInThread<T>();
                    newControl.fill();
                    control.insert_Above(newControl, distance);
                    return newControl;
                });
        }
        public static Panel             insert_Above(this Control control)
		{			
			return control.insert_Above<Panel>(control.height()/2);
		}		
        public static Panel             insert_Above(this Control control, int width)
		{
			return control.insert_Above<Panel>(width);
		}		
		public static Panel             insert_Above(this Control control, string title)
		{
			return control.insert_Above(control.height()/2, title);
		}		
		public static Panel             insert_Above(this Control control, int width, string title)
		{
			return control.insert_Above<Panel>(width).add_GroupBox(title).add_Panel();
		}		

        public static List<Control>     insert_Below(this Control controlToWrap, Control controlToInject, int splitterDistance)
        {
            return controlToWrap.insert_Below(controlToInject, splitterDistance, false);
        }
        public static List<Control>     insert_Below(this Control controlToWrap, Control controlToInject)
        {
            return controlToWrap.injectControl(controlToInject, AnchorStyles.Bottom);
        }
        public static List<Control>     insert_Below(this Control controlToWrap, Control controlToInject, int splitterDistance, bool border3D)
        {
            return controlToWrap.injectControl(controlToInject, AnchorStyles.Bottom, splitterDistance, border3D);
        }
        public static T                 insert_Below<T>(this Control control) where T : Control
        {
            return control.insert_Below<T>(-1);
        }
        public static T                 insert_Below<T>(this Control control, int distance) where T : Control
        {
            return (T)control.invokeOnThread(
                () =>
                {
                    var newControl = control.add_Control<T>();
                    if (newControl == null)
                        return null;
                    newControl.fill();
                    control.insert_Below(newControl, distance);
                    return newControl;
                });
        }
        public static Panel             insert_Below(this Control control)
		{
			return control.insert_Below<Panel>(control.height()/2);
		}				
        public static Panel             insert_Below(this Control control, int width)
		{
			return control.insert_Below<Panel>(width);
		}		
        public static Panel             insert_Below(this Control control, string title)
		{
			return control.insert_Below(control.height()/2, title);
		}		
		public static Panel             insert_Below(this Control control, int width, string title)
		{
			return control.insert_Below<Panel>(width).add_GroupBox(title).add_Panel();
		}

        // Key Events
        public static T onKeyPress<T>(this T control, Func<Keys, bool> callback) where T : Control
        {
            control.KeyDown += (sender, e) => e.Handled = callback(e.KeyData);
            return control;
        }  
        public static T onKeyPress<T>(this T control, Action<Keys> callback) where T : Control
        {
            control.KeyUp += (sender, e) => callback(e.KeyData);
            return control;
        }
        public static T onKeyPress<T>(this T control, Action<Keys, String> callback) where T : Control
        {
            control.KeyUp += (sender, e) => callback(e.KeyData, control.Text);
            return control;
        }
        public static T onKeyPress<T>(this T control, Keys onlyFireOnKey, MethodInvoker callback) where T : Control
        {
            control.KeyUp += (sender, e) =>
            {
                if (e.KeyData == onlyFireOnKey)
                    callback();
            };
            return control;
        }
        public static T onKeyPress<T>(this T control, Keys onlyFireOnKey, Action<String> callback) where T : Control
        {
            control.KeyDown += (sender, e) =>
            {
                if (e.KeyData == onlyFireOnKey)
                {
                    callback(control.Text);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            };
            return control;
        }
        public static T onKeyPress_getChar<T>(this T control, Func<char, bool> callback)            where T : Control
        {
            control.KeyPress += (sender, e) => e.Handled = callback(e.KeyChar);
            return control;
        }        
		public static T onKeyPress_getChar<T>(this T control, Action<char> callback)            where T : Control
        {
            control.KeyPress += (sender, e) => callback(e.KeyChar);
            return control;
        }
        public static T onEnter<T>(this T control, Action<String> callback) where T : Control
        {
            return control.onKeyPress(Keys.Enter, callback);
        }

        // Control - Sync
        public static T syncTextBoxWithControl<T>(this T control, Action<string> onTextChanged) where T : Control
        {
            return control.syncTextBoxWithControl(true, onTextChanged);
        }
        public static T syncTextBoxWithControl<T>(this T control, bool onlyFireOnEnter, Action<string> onTextChanged) where T : Control
        {
            var textBox = control.insert_Above<TextBox>(20);
            textBox.KeyUp += (sender, e) =>
            {
                if (onlyFireOnEnter.isFalse() || e.KeyCode == Keys.Enter)
                    onTextChanged(textBox.get_Text());
            };
            return control;
        }
        public static T syncComboBoxWithControl<T>(this T control, Action<string> onTextChanged) where T : Control
        {
            return control.syncComboBoxWithControl(true, true, onTextChanged);
        }
        public static T syncComboBoxWithControl<T>(this T control, bool onlyFireOnEnter, bool addToHistory, Action<string> onTextChanged) where T : Control
        {
            var comboBox = control.insert_Above<ComboBox>(20);
            comboBox.KeyUp += (sender, e) =>
            {
                //	comboBox.invokeOnThread(
                //		()=>{
                var text = comboBox.Text;
                if (onlyFireOnEnter.isFalse() || e.KeyCode == Keys.Enter)
                {
                    onTextChanged(text);
                    if (addToHistory)
                    {
                        comboBox.Items.Insert(0, text);
                        comboBox.Text = "";
                    }
                }
                //			});					
            };
            return control;
        }
       
        //SendKeys
        public static T sendKeys<T>(this T control, string textToSend) where T : Control
        {
            return (T)control.invokeOnThread(
                () =>
                {
                    control.focus();
                    SendKeys.Send(textToSend);
                    return control;
                });
        }
        public static T sendEnter<T>(this T control) where T : Control
        {
            return control.sendKeys("".line());
        }

        
        //askUserFor...
        public static string askUserForFileToOpen(this Control control)
        {
            return (string)control.invokeOnThread(
                () => O2Forms.askUserForFileToOpen());
        }
        public static string askUserForFileToOpen(this Control control,string baseDirectory)
        {
            return (string)control.invokeOnThread(
                () => O2Forms.askUserForFileToOpen(baseDirectory));
        }
        public static string askUserForFileToOpen(this Control control, string baseDirectory, string filter)
        {
            return (string)control.invokeOnThread(
                () => O2Forms.askUserForFileToOpen(baseDirectory, filter));
        }
        public static string askUserForFileToSave(this Control control, string baseDirectory)
        {
            return (string)control.invokeOnThread(
                () => O2Forms.askUserForFileToSave(baseDirectory));
        }
        public static string askUserForDirectory(this Control control, string baseDirectory)
        {
            return (string)control.invokeOnThread(
                () => O2Forms.askUserForDirectory(baseDirectory));
        }        

        public static T size<T>(this T control, int value)			where T : Control
		{
			return control.textSize(value);
		}		
		public static T size<T>(this T control, string value)			where T : Control
		{
			return control.textSize(value.toInt());
		}		
		public static T font<T>(this T control, string fontFamily, string size)			where T : Control
		{
			return control.font(fontFamily, size.toInt());
		}		
		public static T font<T>(this T control, string fontFamily, int size)			where T : Control
		{
			return control.font(new FontFamily(fontFamily), size);
		}		
		public static T font<T>(this T control, FontFamily fontFamily, string size)			where T : Control
		{
			return control.font(fontFamily, size.toInt());
		}		
		public static T font<T>(this T control, FontFamily fontFamily, int size)			where T : Control
		{
			if (control.isNull())
				return null;
			control.invokeOnThread(
				()=>{
						if (fontFamily.isNull())
							fontFamily = control.Font.FontFamily;
						control.Font = new Font(fontFamily, size);
					});
			return control;
		}		
		public static T font<T>(this T control, string fontFamily)			where T : Control
		{
			return control.fontFamily(fontFamily);
		}		
		public static T fontFamily<T>(this T control, string fontFamily)			where T : Control
		{
			control.invokeOnThread(
				()=> control.Font = new Font(new FontFamily(fontFamily), control.Font.Size));			
			return control;
		}		
		public static T textSize<T>(this T control, int value)			where T : Control
		{
			control.invokeOnThread(
				()=> control.Font = new Font(control.Font.FontFamily, value));			
			return control;
		}		
		public static T font_bold<T>(this T control)					where T : Control
		{
            // just bold() conficts with WPF version
			control.invokeOnThread(
				()=> control.Font = new Font( control.Font, control.Font.Style | FontStyle.Bold ));
			return control;
		}		
		public static T font_italic<T>(this T control)			where T : Control
		{
			control.invokeOnThread(
				()=> control.Font = new Font( control.Font, control.Font.Style | FontStyle.Italic ));
			return control;
		}		


        //misc to place in correct location 
        public static T white<T>(this T control)			where T : Control
		{
			return control.backColor(Color.White);
		}		
		public static T pink<T>(this T control)			where T : Control
		{
			return control.backColor(Color.LightPink);
		}		
		public static T green<T>(this T control)			where T : Control
		{
			return control.backColor(Color.LightGreen);
		}			
		public static T azure<T>(this T control)			where T : Control
		{
			return control.backColor(Color.Azure);
		}			
		public static int top<T>(this T control)			where T : Control
		{
			return (int)control.invokeOnThread(()=>  control.Top);
		}
		public static int left<T>(this T control)			where T : Control
		{
			return (int)control.invokeOnThread(()=>  control.Left);
		}		
		public static int width<T>(this T control)			where T : Control
		{
			return (int)control.invokeOnThread(()=>  control.Width);
		}		
		public static int height<T>(this T control)			where T : Control
		{
			return (int)control.invokeOnThread(()=>  control.Height);
		}		
		public static T align_Right<T>(this T control)			where T : Control
		{
			return control.align_Right(control.parent());
		}


        //Add and Append controls

        public static LinkLabel add_Link(this Control control, string label, Action onClickCallback)
		{
			return control.add_Link(label, 0,0, ()=> onClickCallback());
		}		
		public static LinkLabel append_Link_Below(this Control control, string label,int left, Action onClickCallback)
		{
			return control.append_Link_Below(label,onClickCallback).left(left);
		}		
		public static LinkLabel append_Link_Below(this Control control, string label, Action onClickCallback)
		{
			return control.append_Below_Link(label,onClickCallback);
		}		
		public static LinkLabel append_Below_Link(this Control control, string label, Action onClickCallback)
		{
			return control.parent().add_Link(label, control.top() + 20 , control.left(), ()=> onClickCallback());
		}		
		public static Label append_Below_Label(this Control control, string label)
		{
			return control.parent().add_Label(label, control.top() + 22 , control.left());
		}						
    }
}