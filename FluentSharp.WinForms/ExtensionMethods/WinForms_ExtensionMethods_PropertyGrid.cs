using System;
using System.Windows.Forms;
using FluentSharp.WinForms.Controls;

namespace FluentSharp.WinForms
{
    public static class WinForms_ExtensionMethods_PropertyGrid
    {
        /// <summary>
        /// Shows the provided object in a popupwindow using the default .NET property grid
        /// 
        /// Note: if you want to explore the object you should use <code>_object.details()</code> since that will give you
        ///       recursive access to public and private: fields, properties and methods
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_object"></param>
        /// <returns></returns>
        public static T showInfo<T>(this T _object)
        {
            "Property Grid".popupWindow(300, 300)
                           .add_Control<ctrl_ShowInfo>().show(_object);            
            return _object;
        }
        /// <summary>
        /// Same as <code>showInfo</code> but will wait the current execution unit the popupWindow is closed
        /// This is usefully when debugging code or unit-tests
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_object"></param>
        /// <returns></returns>
        public static T showInfo_WaitForClose<T>(this T _object)
        {
            var popupWindow = "Property Grid".popupWindow(300, 300);
            popupWindow.add_Control<ctrl_ShowInfo>().show(_object);            
            popupWindow.waitForClose();
            return _object;
        }
        public static PropertyGrid add_PropertyGrid(this Control control, bool helpVisible)
        {
            return control.add_PropertyGrid().helpVisible(helpVisible);
        }
        public static PropertyGrid add_PropertyGrid(this Control control)
        {
            return control.add_Control<PropertyGrid>();
        }        
        public static T show<T>(this PropertyGrid propertyGrid, T _object)
        {
            var maxInvokeWait = 2000;  // 2 secs (more than this means a thread deadlock)
            propertyGrid.invokeOnThread(maxInvokeWait, () => propertyGrid.SelectedObject = _object);
            return _object;
        }
        public static PropertyGrid loadInPropertyGrid(this object objectToLoad)
        {
            var propertyGrid = new PropertyGrid();
            propertyGrid.show(objectToLoad);
            return propertyGrid;
        }
        public static PropertyGrid toolBarVisible(this PropertyGrid propertyGrid, bool value)
        {
            propertyGrid.invokeOnThread(() => propertyGrid.ToolbarVisible = value);

            return propertyGrid;
        }
        public static PropertyGrid helpVisible(this PropertyGrid propertyGrid, bool value)
        {
            propertyGrid.invokeOnThread(() => propertyGrid.HelpVisible = value);
            return propertyGrid;
        }
        public static PropertyGrid sort_Alphabetical(this PropertyGrid propertyGrid)
        {
            propertyGrid.invokeOnThread(() => propertyGrid.PropertySort = PropertySort.Alphabetical);
            return propertyGrid;
        }
        public static PropertyGrid sort_Categorized(this PropertyGrid propertyGrid)
        {
            propertyGrid.invokeOnThread(() => propertyGrid.PropertySort = PropertySort.Categorized);
            return propertyGrid;
        }
        public static PropertyGrid sort_CategorizedAlphabetical(this PropertyGrid propertyGrid)
        {
            propertyGrid.invokeOnThread(() => propertyGrid.PropertySort = PropertySort.CategorizedAlphabetical);
            return propertyGrid;
        }
        public static PropertyGrid onValueChange(this PropertyGrid propertyGrid, Action callback)
        {
            propertyGrid.invokeOnThread(()=>propertyGrid.PropertyValueChanged+=(sender,e)=>callback() );
            return propertyGrid;
        }

    }
}