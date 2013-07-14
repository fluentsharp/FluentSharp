using System;
using System.Windows.Forms;
using FluentSharp.CoreLib;

namespace FluentSharp.WinForms
{
    public static class WinForms_ExtensionMethods_Mini_Gui_Controls
    { 
        public static T     add_LabelAndTextAndButton<T>(this T control, string labelText, string textBoxText, string buttonText, Action<string> onButtonClick)            where T : Control
        {
            //create controls
            var label = control.add_Label(labelText);
            var textBox = label.append_Control<TextBox>();
            var button = textBox.append_Control<System.Windows.Forms.Button>();

            //set text (the label needs to set on the ctor so that the append_Control puts the textbox on its right
            textBox.set_Text(textBoxText);
            button.set_Text(buttonText);

            //position controls
            button.anchor_TopRight();
            button.left(control.width() - button.width());
            textBox.align_Right(control);
            textBox.width(textBox.width() - button.width());

            //final tweaks
            label.topAdd(3);
            textBox.widthAdd(-5);
            button.widthAdd(-2);
            button.heightAdd(-2);

            //events
            button.onClick(() => onButtonClick(textBox.get_Text()));
            textBox.onEnter((text) => onButtonClick(text));
            return control;
        }
        public static T     add_LabelAndComboBoxAndButton<T>(this T control, string labelText, string comboBoxText, string buttonText, Action<string> onButtonClick)            where T : Control
        {
            //create controls
            var label = control.add_Label(labelText);
            var comboBox = label.append_Control<ComboBox>();
            var button = comboBox.append_Control<System.Windows.Forms.Button>();

            //set text (the label needs to set on the ctor so that the append_Control puts the textbox on its right
            comboBox.set_Text(comboBoxText);
            button.set_Text(buttonText);

            //position controls
            button.anchor_TopRight();
            button.left(control.width() - button.width());
            comboBox.align_Right(control);
            comboBox.width(comboBox.width() - button.width());

            //final tweaks
            label.topAdd(3);
            comboBox.widthAdd(-5);
            button.widthAdd(-2);
            button.heightAdd(-2);

            Action<String> onNewItem =
                (newItem) =>
                {
                    if (comboBox.items().Contains(newItem).isFalse())
                        comboBox.insert_Item(newItem);
                    onButtonClick(newItem);
                };


            //events
            button.onClick(() => onNewItem(comboBox.get_Text()));
            comboBox.onEnter((text) => onNewItem(text));
            comboBox.onSelection(() => onNewItem(comboBox.get_Text()));
            return control;
        }
    }
}
;