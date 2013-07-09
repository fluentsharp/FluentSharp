using System.Windows.Controls;
using FluentSharp.CoreLib;

namespace FluentSharp.WPF
{
    public static class ComboBox_ExtensionMethods
    {
	
        #region ComboBox

        public static ComboBox add_Item(this ComboBox comboBox, string itemText)
        {
            return comboBox.add_Items(itemText);
        }

        public static ComboBox add_Items(this ComboBox comboBox, params string[] itemTexts)
        {
            return (ComboBox)comboBox.wpfInvoke(
                () =>
                    {
                        foreach (var itemText in itemTexts)
                        {
                            var comboBoxItem = new ComboBoxItem();
                            comboBoxItem.Content = itemText;
                            comboBox.Items.Add(comboBoxItem);
                        }
                        return comboBox;
                    });
        }

        public static ComboBox selectFirst(this ComboBox comboBox)
        {
            return (ComboBox)comboBox.wpfInvoke(
                () =>
                    {
                        if (comboBox.Items.size() > 0)
                            comboBox.SelectedIndex = 0;
                        return comboBox;
                    });
        }                

        public static string get_Text_Wpf(this ComboBox comboBox)
        {
            return (string)comboBox.wpfInvoke(
                () =>
                    {
                        if (comboBox.SelectedItem.notNull() && comboBox.SelectedItem is ComboBoxItem)
                            return (comboBox.SelectedItem as ComboBoxItem).Content;
                        return "";
                    });
        }

        #endregion


    }
}