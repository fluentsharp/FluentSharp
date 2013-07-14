// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Drawing;
using System.Windows.Forms;
using FluentSharp.CoreLib.API;

namespace FluentSharp.WinForms.Utils
{
    public class Screenshots
    {
        public static void fillPictureBoxWithThumbnailOfObject(PictureBox pbTargetPictureBox, Object oTargetObject)
        {
            pbTargetPictureBox.Image = getThumbnailofObject(oTargetObject, pbTargetPictureBox.Width,
                                                            pbTargetPictureBox.Height);
        }

        public static Image getThumbnailofObject(Object oObjectToTakeScreenShotOf, int iWidth, int iHeight)
        {
            return getScreenshotOfObject(oObjectToTakeScreenShotOf).GetThumbnailImage(iWidth, iHeight, null, IntPtr.Zero);
        }


        // I tried to do this but still can't get the data from objects like webbrowser and richtext box
        // Make sure the caller is on the correct thread
        public static Image getScreenshotOfFormObjectAndItsControls(Object oObjectToTakeScreenShotOf)
        {
            try
            {
                Object oControls = PublicDI.reflection.getProperty("Controls", oObjectToTakeScreenShotOf);
                if (oControls != null)
                {
                    var cccControls = (Control.ControlCollection) oControls;
                    Object oWidth = PublicDI.reflection.getProperty("Width", oObjectToTakeScreenShotOf);
                    Object oHeight = PublicDI.reflection.getProperty("Height", oObjectToTakeScreenShotOf);
                    if (null != oWidth && null != oHeight && (int) oWidth > 0 && (int) oHeight > 0)
                    {
                        var bWorkBitmap = new Bitmap((int) oWidth, (int) oHeight);
                        Graphics gWorkGraphics = Graphics.FromImage(bWorkBitmap);
                        gWorkGraphics.DrawImage(getScreenshotOfObject(oObjectToTakeScreenShotOf), new Point(0, 0));
                        foreach (Control cControl in cccControls)
                        {
                            if (cControl.Left > 0 && cControl.Top > 0)
                            {
                                Image iControlScreenShot = getScreenshotOfFormObjectAndItsControls(cControl);
                                if (iControlScreenShot == null)
                                {
                                    gWorkGraphics.DrawImage(getScreenshotOfObject(cControl),
                                                            new Point(cControl.Left, cControl.Top));
                                }
                                else
                                {
                                    gWorkGraphics.DrawImage(iControlScreenShot, new Point(cControl.Left, cControl.Top));
                                }
                            }
                        }
                        return bWorkBitmap;
                    }
                }
                else
                    PublicDI.log.error(
                        "In getScreenshotOfFormObjectAndItsControls, class doesn't have a Controls property {0}",
                        oObjectToTakeScreenShotOf.GetType());
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex, "getScreenshotOfFormObjectAndItsControls");
            }
            return null;
        }

        public static Image getScreenshotOfObject(Object oObjectToTakeScreenShotOf)
        {
            Object oWidth = PublicDI.reflection.getProperty("Width", oObjectToTakeScreenShotOf);
            Object oHeight = PublicDI.reflection.getProperty("Height", oObjectToTakeScreenShotOf);
            if (null != oWidth && null != oHeight && (int) oWidth > 0 && (int) oHeight > 0)
            {
                var bBitmap = new Bitmap((Int32) oWidth, (Int32) oHeight);
                var oParams = new object[] {bBitmap, new Rectangle(0, 0, (Int32) oWidth, (Int32) oHeight)};
                object oResult = PublicDI.reflection.invokeMethod_InstanceStaticPublicNonPublic(oObjectToTakeScreenShotOf,
                                                                                          "DrawToBitmap", oParams);
                return bBitmap;
            }
            return null;
        }

		public static Image getScreenshotOfDesktop()
		{
			Bitmap WorkingImage = null;
        	Graphics WorkingGraphics = null;
        	Rectangle TargetArea = Screen.PrimaryScreen.WorkingArea;
        	//Image ReturnImage = null;
        	WorkingImage = new Bitmap(TargetArea.Width,TargetArea.Height);

            WorkingGraphics = Graphics.FromImage(WorkingImage);
            WorkingGraphics.CopyFromScreen(TargetArea.X, TargetArea.X, 0, 0, TargetArea.Size);
			return (Image)WorkingImage.Clone();
		}

        /*  public static void takeScreenShotOfObject(String sObjectToTakeScreenShot)
        {
            Messages.sendMessage("open Image,test2");
            Messages.sendMessage("test2.ascx_Image.loadImageIntoPictureBox forward");
            Object oScreenShot =
                Messages.sendMessage(String.Format("screenshots.getScreenshotOfFormObjectAndItsControls !{0}",
                                                   sObjectToTakeScreenShot));
            if (oScreenShot == null)
                PublicDI.log.debug("oScreenShot was NULL");
            else
                Messages.sendMessage("test2.ascx_Image.updateImage", new List<Object>(new[] {oScreenShot}));
        }*/
    }
}
