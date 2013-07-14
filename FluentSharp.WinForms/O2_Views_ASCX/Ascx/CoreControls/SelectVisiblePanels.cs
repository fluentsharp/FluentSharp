// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Drawing;
using System.Windows.Forms;
using FluentSharp.WinForms.Utils;

namespace FluentSharp.WinForms.Controls
{
    public partial class SelectVisiblePanels : UserControl
    {
        private Color cSelectedColor = Color.LightGray;
        private Color cUnSelectedColor = Color.DimGray;
        private SplitContainer scTarget_Bottom;

        private SplitContainer scTarget_Host;
        private SplitContainer scTarget_Top;

        public SelectVisiblePanels()
        {
            InitializeComponent();
        }

        public void setTitle(String sTitle)
        {
            gbVisibleControls.Text = sTitle;
        }

        public void setVisibleControlsColapseState_4Panels_TopRight(String sTitle, SplitContainer scTarget_Host,
                                                                    SplitContainer scTarget_Top,
                                                                    SplitContainer scTarget_Bottom,
                                                                    String sTopLeft, String sTopRight,
                                                                    String sBottomLeft, String sBottomRight)
        {
            gbVisibleControls.Text = sTitle;
            setVisibleControlsColapseState_4Panels_TopRight(scTarget_Host, scTarget_Top, scTarget_Bottom, sTopLeft,
                                                            sTopRight, sBottomLeft, sBottomRight);
        }

        public void setVisibleControlsColapseState_4Panels_TopRight(SplitContainer scTarget_Host,
                                                                    SplitContainer scTarget_Top,
                                                                    SplitContainer scTarget_Bottom,
                                                                    String sTopLeft, String sTopRight,
                                                                    String sBottomLeft, String sBottomRight)
        {
            this.scTarget_Host = scTarget_Host;
            this.scTarget_Top = scTarget_Top;
            this.scTarget_Bottom = scTarget_Bottom;

            setCheckBox_Text(1, sTopLeft);
            setCheckBox_Text(2, sTopRight);
            setCheckBox_Text(3, sBottomLeft);
            setCheckBox_Text(4, sBottomRight);

            updateVisibleControlsColapseState();
        }


        public CheckBox getCheckBox(int iCheckBoxId)
        {
            switch (iCheckBoxId)
            {
                case 1:
                    return cb_View_1;
                case 2:
                    return cb_View_2;
                case 3:
                    return cb_View_3;
                case 4:
                    return cb_View_4;
                default:
                    return null;
            }
        }

        public void setCheckBox_Visible(int iCheckBoxId, bool bVisibleState)
        {
            getCheckBox(iCheckBoxId).Visible = bVisibleState;
        }

        public void setCheckBox_Checked(int iCheckBoxId, bool bCheckedState)
        {
            //     getCheckBox(iCheckBoxId).Checked = bCheckedState;
            CheckBox cTargetCheckBox = getCheckBox(iCheckBoxId);
            O2Forms.executeMethodThreadSafe(cTargetCheckBox, cTargetCheckBox, "set_Checked",
                                            new object[] {bCheckedState});
        }

        public void setCheckBox_Left(int iCheckBoxId, int iLeftPos)
        {
            getCheckBox(iCheckBoxId).Left = iLeftPos;
        }

        public void setCheckBox_Text(int iCheckBoxId, String sCheckBoxText)
        {
            if (sCheckBoxText != null)
                getCheckBox(iCheckBoxId).Text = sCheckBoxText;
            else
                getCheckBox(iCheckBoxId).Visible = false;
        }

        public void setGroupBoxWidth(int iWidth)
        {
            gbVisibleControls.Width = iWidth;
        }

        public void updateVisibleControlsColapseState()
        {
            updateVisibleControlsColapseState_4Panels_TopRight(scTarget_Host, scTarget_Top, scTarget_Bottom,
                                                               cb_View_1.Checked, cb_View_2.Checked, cb_View_3.Checked,
                                                               cb_View_4.Checked);
        }

        public void updateVisibleControlsColapseState_4Panels_TopRight(SplitContainer scTarget_Host,
                                                                       SplitContainer scTarget_Top,
                                                                       SplitContainer scTarget_Bottom,
                                                                       bool bTopLeft, bool bTopRight, bool bBottomLeft,
                                                                       bool bBottomRight)
        {
            if ((bBottomRight == false && bBottomLeft == false) && (bTopLeft == false && bTopRight == false))
            {
                scTarget_Host.Visible = false;
            }
            else
            {
                scTarget_Host.Visible = true;

                scTarget_Host.Panel1Collapsed = (bTopLeft == false && bTopRight == false);
                scTarget_Host.Panel2Collapsed = (bBottomRight == false && bBottomLeft == false);

                scTarget_Top.Panel1Collapsed = !bTopLeft;
                scTarget_Top.Panel2Collapsed = !bTopRight;
                scTarget_Bottom.Panel1Collapsed = !bBottomLeft;
                scTarget_Bottom.Panel2Collapsed = !bBottomRight;
            }
        }


        private void gbVisibleControls_Enter(object sender, EventArgs e)
        {
        }


        private void ascx_SelectVisiblePanels_Click(object sender, EventArgs e)
        {
            /*    if (gbVisibleControls.ForeColor == cSelectedColor)            
                gbVisibleControls.ForeColor = cUnSelectedColor;            
            else
                gbVisibleControls.ForeColor = cSelectedColor;*/
        }

        private void ascx_SelectVisiblePanels_Load(object sender, EventArgs e)
        {
            gbVisibleControls.Click += ascx_SelectVisiblePanels_Click;
        }

        private void cb_View_1_CheckedChanged(object sender, EventArgs e)
        {
            updateVisibleControlsColapseState();
        }

        private void cb_View_2_CheckedChanged(object sender, EventArgs e)
        {
            updateVisibleControlsColapseState();
        }

        private void cb_View_3_CheckedChanged(object sender, EventArgs e)
        {
            updateVisibleControlsColapseState();
        }

        private void cb_View_4_CheckedChanged(object sender, EventArgs e)
        {
            updateVisibleControlsColapseState();
        }
    }
}
