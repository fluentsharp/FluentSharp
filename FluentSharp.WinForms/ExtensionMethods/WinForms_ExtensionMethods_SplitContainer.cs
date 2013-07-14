using System.Windows.Forms;
using FluentSharp.CoreLib;

namespace FluentSharp.WinForms
{
    public static class WinForms_ExtensionMethods_SplitContainer
    { 
        public static SplitContainer add_SplitContainer(this Control control)
        {
            return add_SplitContainer(control, false, false, false);
        }
        public static SplitContainer add_SplitContainer(this Control control, bool setOrientationToVertical, bool setDockStyleoFill, bool setBorderStyleTo3D)        
        {
            return add_SplitContainer(
                control,
                (setOrientationToVertical) ? Orientation.Vertical : Orientation.Horizontal,
                setDockStyleoFill,
                setBorderStyleTo3D);
        }       
        public static SplitContainer add_SplitContainer(this Control control, Orientation orientation, bool setDockStyleToFill, bool setBorderStyleTo3D)
        {
            return (SplitContainer) control.invokeOnThread(
                () =>
                    {
                        var splitContainer = new SplitContainer();
                        splitContainer.Orientation = orientation;
                        splitContainer.minimumSize(1);                                                
                        if (setDockStyleToFill)
                            splitContainer.Dock = DockStyle.Fill;
                        if (setBorderStyleTo3D)
                            splitContainer.BorderStyle = BorderStyle.Fixed3D;
                        control.Controls.Add(splitContainer);
                        return splitContainer;
                    });
        }
        public static SplitContainer border3D(this SplitContainer control)
        {
            return (SplitContainer)control.invokeOnThread(
                () =>
                    {
                        control.BorderStyle = BorderStyle.Fixed3D;
                        return control;
                    });
        }
        public static SplitContainer borderNone(this SplitContainer control)
        {
            return (SplitContainer)control.invokeOnThread(
                () =>
                    {
                        control.BorderStyle = BorderStyle.None;
                        return control;
                    });
        }        
        public static SplitContainer fixedPanel1(this SplitContainer control)
        {
            return (SplitContainer)control.invokeOnThread(
                () =>
                    {
                        control.FixedPanel = FixedPanel.Panel1;
                        return control;
                    });
        }
        public static SplitContainer fixedPanel2(this SplitContainer control)
        {
            return (SplitContainer)control.invokeOnThread(
                () =>
                    {
                        control.FixedPanel = FixedPanel.Panel2;
                        return control;
                    });
        }
        public static SplitContainer panel2Collapsed(this SplitContainer control, bool value)
        {
            return (SplitContainer)control.invokeOnThread(
                () =>
                    {
                        control.Panel2Collapsed = value;
                        return control;
                    });
        }
        public static SplitContainer panel1Collapsed(this SplitContainer control, bool value)
        {
            return (SplitContainer)control.invokeOnThread(
                () =>
                    {
                        control.Panel1Collapsed = value;
                        return control;
                    });
        }
        public static SplitContainer distance(this SplitContainer control, int value)
        {
            return control.splitterDistance(value);
        }        
        public static SplitContainer splitterDistance(this SplitContainer control, int value)
        {
            return (SplitContainer)control.invokeOnThread(
                () =>
                    {   
                        if (value > 0)
                            control.SplitterDistance = value;
                        return control;
                    });
        }
        public static T              splitterDistance<T>(this T control, int distance)			where T : Control
        {
            var splitContainer = control.splitContainer();
            if (splitContainer.notNull())
                splitterDistance(splitContainer,distance);
            return control;
        }
        public static SplitContainer verticalOrientation(this SplitContainer splitContainer)
        {
            splitContainer.invokeOnThread(() => splitContainer.Orientation = Orientation.Vertical);
            return splitContainer;
        }
        public static SplitContainer horizontalOrientation(this SplitContainer splitContainer)
        {
            splitContainer.invokeOnThread(() => splitContainer.Orientation = Orientation.Horizontal);
            return splitContainer;
        }
        public static SplitContainer minimumSize(this SplitContainer splitContainer, int size)
        {
            splitContainer.invokeOnThread(
                () =>
                    {
                        splitContainer.Panel1MinSize = size;
                        splitContainer.Panel2MinSize = size;
                    });
            return splitContainer;
        }       
        public static SplitContainer splitContainer(this Control control)
        {
            return control.parent<SplitContainer>();
        }		
        public static SplitContainer splitterWidth(this SplitContainer splitContainer, int value)
        {
            if (splitContainer.notNull())
                splitContainer.invokeOnThread(()=> splitContainer.SplitterWidth = value);
            return splitContainer;
        }
        public static T splitterWidth<T>(this T control, int value) where T : Control
        {
            var splitContainer = control.splitContainer();
            WinForms_ExtensionMethods_SplitContainer.splitterWidth(splitContainer, value);
            return control;
        }
        public static T splitContainerFixed<T>(this T control) where T : Control
        {
            control.splitContainer().isFixed(true);
            return control;
        }		
        public static SplitContainer @fixed(this SplitContainer splitContainer, bool value)
        {
            return 	splitContainer.isFixed(value);
        }		
        public static SplitContainer isFixed(this SplitContainer splitContainer, bool value)
        {
            splitContainer.invokeOnThread(
                ()=>{
                        splitContainer.IsSplitterFixed = value;
                        splitContainer.SplitterWidth = 1;
                });
            return splitContainer;
        }

    }
}