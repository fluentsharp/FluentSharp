// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Windows.Forms;

namespace FluentSharp.WinForms.Utils
{
    public static class Controls
    {
        public static ListBox add_ListBox(Control cTargetControl, String sTitle, int iLeft, int iTop, int iWidth,
                                         int iHeight)
        {
            var lbListBox = new ListBox();
			lbListBox.Sorted = true;
			lbListBox.HorizontalScrollbar = true;
			lbListBox.FormattingEnabled = true;
			lbListBox.Height = iHeight;
			lbListBox.Width = iWidth;
			lbListBox.Top = iTop;
			lbListBox.Left = iLeft;
            cTargetControl.Controls.Add(lbListBox);
            // if sTitle != "" add a title on the top left
            if (sTitle != "")
                add_Label(cTargetControl, sTitle, iLeft, iTop - 20);
            return lbListBox;
        }

        public static Label add_Label(Control cTargetControl, String sLabelText, int iLeft, int iTop)
        {
            var lLabel = new Label();
			lLabel.Top = iTop;
			lLabel.Left = iLeft;
			lLabel.Text = sLabelText;
			lLabel.AutoSize = true;
            cTargetControl.Controls.Add(lLabel);
            return lLabel;
        }

        public static TextBox add_TextBox(Control cTargetControl, String sLabelText, int iLeft, int iTop)
        {
            var tbTextBox = new TextBox();
			tbTextBox.Top = iTop;
			tbTextBox.Left = iLeft;
			tbTextBox.Text = sLabelText;
			tbTextBox.AutoSize = true;
            cTargetControl.Controls.Add(tbTextBox);
            return tbTextBox;
        }

        public static Button add_Button(Control cTargetControl, String sLabelText, int iLeft, int iTop)
        {
            var btButton = new Button();
			btButton.Top = iTop;
			btButton.Left = iLeft;
			btButton.Text = sLabelText;
			btButton.AutoSize = true;
            cTargetControl.Controls.Add(btButton);
            return btButton;
        }

        public static DataGridView add_DataGridView(Control cTargetControl,
                                                   int iLeft, int iTop, int iWidth, int iHeight,
                                                   AnchorStyles asAnchorStyles)
        {
			var dgvDataGridView = new DataGridView();
			dgvDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			dgvDataGridView.Anchor = asAnchorStyles;
			dgvDataGridView.Height = iHeight;
			dgvDataGridView.Width = iWidth;
			dgvDataGridView.Left = iLeft;
			dgvDataGridView.Top = iTop;
            //	dgvDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            cTargetControl.Controls.Add(dgvDataGridView);
            return dgvDataGridView;
        }

        /*public static SplitContainer add_SplitContainer(Control cTargetControl, Control cPanel1, Control cPanel2,
                                                       Orientation oOrientation, int iLeft, int iTop, int iWidth,
                                                       int iHeight, AnchorStyles asAnchorStyles)
        {
            var scSplitContainer = new SplitContainer
                                       {
                                           BorderStyle = BorderStyle.Fixed3D,
                                           Anchor = asAnchorStyles,
                                           Orientation = oOrientation
                                       };            
            scSplitContainer.Panel1.Controls.Add(cPanel1);
            scSplitContainer.Panel2.Controls.Add(cPanel2);
            cPanel1.Dock = DockStyle.Fill;
            cPanel2.Dock = DockStyle.Fill;
            //Orientation = Orientation.Horizontal;
            //Dock = 
            scSplitContainer.Left = iLeft;
            scSplitContainer.Top = iTop;
            scSplitContainer.Width = iWidth;
            scSplitContainer.Height = iHeight;
            cTargetControl.Controls.Add(scSplitContainer);
            return scSplitContainer;
        }*/
    }
}
