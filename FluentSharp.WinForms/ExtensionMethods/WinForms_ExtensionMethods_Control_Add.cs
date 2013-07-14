using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;

namespace FluentSharp.WinForms
{
    public static class WinForms_ExtensionMethods_Control_Add
    {       
        // ReSharper disable RedundantAssignment
        //add
        public static T add<T>(this Control hostControl, T childControl) where T : Control
        {
            return hostControl.invokeOnThread(
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
            return hostControl.add(childControl);
        }
        public static Control add_Control(this Control control, Type childControlType)            
        {
            return control.invokeOnThread(
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
            if (hostControl.notNull())
            {
                var values = new[] { -1, -1, -1, -1 };
                for (int i = 0; i < position.Length; i++)
                    values[i] = position[i];
                return hostControl.add_Control<T>(values[0], values[1], values[2], values[3]);
            }
            return default(T);
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
                        PublicDI.log.error("in add_Control<T>: " + ex.Message);
                    }
                    return null;
                });
        }

		public static T add_Control<T>(this Control hostControl, ref T controlAdded) where T : Control
		{
			return controlAdded = hostControl.add_Control<T>();
		}
        public static List<T> add_Controls<T>(this Control control, List<T> controlsToAdd) where T : Control
        {
            foreach (var controlToAdd in controlsToAdd)
                control.add_Control(controlToAdd);
            return controlsToAdd;
        }
        public static T append_Control<T>(this Control control) where T : Control
        {
            if(control.notNull())
                return control.parent().add_Control<T>(control.Top, control.Left + control.Width + 5);
            return null;
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
                    var controls = new List<Control>();
                    controls.add(groupBox_1);
                    controls.add(groupBox_2 );
                    return controls;
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

                    var controls = new List<Control>();
					controls.Add(groupBox_3);
					controls.Add(groupBox_2);
					controls.Add(groupBox_1);
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

            var controls = new List<Control>();
			controls.Add(groupBox_4);
			controls.Add(groupBox_3);
			controls.Add(groupBox_2);
			controls.Add(groupBox_1);
            return controls;
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
				return (List<Control>)controlToWrap.invokeOnThread(() =>
                    {
						var childControls = new List<Control>();
                        var parentControl = controlToWrap.parent();
						if (parentControl.isNull())	//means we will need to add to the current one (or the parent control is a a Form)
						{
							childControls.add(controlToWrap.controls());
							parentControl = controlToWrap;
						}
						else
						{
							childControls.add(controlToWrap);							
						}
						foreach (var childControl in childControls)
							parentControl.remove(childControl);

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
								splitContainer.Panel2.Controls.AddRange(childControls.ToArray());
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
								splitContainer.Panel1.Controls.AddRange(childControls.ToArray());
								splitContainer.Panel2.add(controlToInject);
								
                                //splitContainer.Panel1.add(controlToWrap);
                                //splitContainer.Panel2.add(controlToInject);
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
                    //var newControl = control.add_Control<T>();
					var newControl = control.newInThread<T>();
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
                    //var newControl = control.add_Control<T>();
					var newControl = control.newInThread<T>();
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
		public static T					insert_Above<T>(this Control hostControl, int topPanelSize, ref T controlAdded) where T : Control
		{
			return controlAdded = hostControl.insert_Above<T>(topPanelSize);
		}
		public static T					insert_Above<T>(this Control hostControl, ref T controlAdded) where T : Control
		{
			return controlAdded = hostControl.insert_Above<T>();
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
                    //var newControl = control.add_Control<T>();
					var newControl = control.newInThread<T>();
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

        public static Panel             insert_ActionPanel(this Control control)
        {
            return control.insert_Above(40, "Actions");
        }

        // ReSharper restore RedundantAssignment
    }
}
