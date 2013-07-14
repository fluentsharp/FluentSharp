using System;
using System.Windows.Forms;
using FluentSharp.WinForms.Controls;

namespace FluentSharp.WinForms
{
    public static class WinForms_ExtensionMethods_PropertyGrid
    {
        public static T showInfo<T>(this T _object)
        {
            "Property Grid".popupWindow(300, 300)
                           .add_Control<ctrl_ShowInfo>().show(_object);            
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
            propertyGrid.invokeOnThread(() => propertyGrid.SelectedObject = _object);
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