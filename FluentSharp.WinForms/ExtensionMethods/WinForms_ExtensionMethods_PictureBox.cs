using System;
using System.Drawing;
using System.Windows.Forms;
using FluentSharp.CoreLib;
using FluentSharp.Web35;

namespace FluentSharp.WinForms
{
    public static class WinForms_ExtensionMethods_PictureBox
    {             
        public static PictureBox add_Image(this Control control)
        {
            return control.add_PictureBox();
        }
        public static PictureBox add_Image(this Control control, string pathToImage)
        {
            return control.add_PictureBox(pathToImage);
        }
        public static PictureBox add_PictureBox(this Control control)
        {
            return control.add_PictureBox(-1, -1);
        }
        public static PictureBox add_PictureBox(this Control control, int top, int left)
        {
            return (PictureBox)control.invokeOnThread(
                ()=>{
                        var pictureBox = new PictureBox();
                        pictureBox.BackgroundImageLayout = ImageLayout.Stretch;
                        if (top == -1 && left == -1)
                            pictureBox.fill();
                        else
                        {
                            if (top > -1)
                                pictureBox.Top = top;
                            if (left > -1)
                                pictureBox.Left = left;
                        }
                        control.Controls.Add(pictureBox);
                        return pictureBox;
                });
        }
        public static PictureBox add_PictureBox(this Control hostControl, ref PictureBox pictureBox)
        {
            return pictureBox = hostControl.add_PictureBox();
        }		        
        public static PictureBox add_PictureBox(this Control control, string pathToImage)
        {
            var pictureBox = control.add_PictureBox();
            return pictureBox.load(pathToImage);
        }        
        public static PictureBox append_PictureBox(this Control control, ref PictureBox pictureBox)
        {
            pictureBox = control.append_Control<PictureBox>();
            pictureBox.height(control.height());
            pictureBox.width(control.width());			
            return pictureBox;
        }
        public static PictureBox open(this PictureBox pictureBox, string pathToImage)
        {
            return pictureBox.load(pathToImage);
        }
        public static PictureBox open(this PictureBox pictureBox, Bitmap bitmap)
        {
            return pictureBox.load(bitmap);
        }
        public static PictureBox load(this PictureBox pictureBox, Image image)
        {
            if (pictureBox.notNull())
                pictureBox.BackgroundImage = image;
            return pictureBox;
        }
        public static PictureBox load(this PictureBox pictureBox, string pathToImage)
        {
            if (pathToImage.fileExists())
            {
                var image = Image.FromFile(pathToImage);
                pictureBox.load(image);
                return pictureBox;
            }
            return null;
        }
        public static PictureBox show(this PictureBox pictureBox, string pathToImage)
        {
            return pictureBox.load(pathToImage);
        }
        public static PictureBox show(this PictureBox pictureBox, Bitmap bitmap)
        {
            return pictureBox.load(bitmap);
        }
        public static PictureBox loadFromUri(this PictureBox pictureBox, Uri uri)
        {
            "loading image from Uri into PictureBox".debug();
            pictureBox.Image = uri.getImageAsBitmap();
            return pictureBox;
        }
        public static PictureBox onClick(this PictureBox pictureBox, Action callback)
        {
            pictureBox.Click += (sender, e) => callback();
            return pictureBox;
        }
        public static PictureBox onDoubleClick(this PictureBox pictureBox, Action callback)
        {
            if (pictureBox.notNull())
                pictureBox.DoubleClick += (sender, e) => callback();
            return pictureBox;
        }
        public static PictureBox layout(this PictureBox pictureBox, ImageLayout imageLayout)
        {
            return pictureBox.invokeOnThread(
                () =>
                    {
                        pictureBox.BackgroundImageLayout = imageLayout;
                        return pictureBox;
                    });
        }
        public static PictureBox layout_Center(this PictureBox pictureBox)
        {
            return pictureBox.layout(ImageLayout.Center);
        }
        public static PictureBox layout_None(this PictureBox pictureBox)
        {
            return pictureBox.layout(ImageLayout.None);
        }
        public static PictureBox layout_Stretch(this PictureBox pictureBox)
        {
            return pictureBox.layout(ImageLayout.Stretch);
        }
        public static PictureBox layout_Tile(this PictureBox pictureBox)
        {
            return pictureBox.layout(ImageLayout.Tile);
        }
        public static PictureBox layout_Zoom(this PictureBox pictureBox)
        {
            return pictureBox.layout(ImageLayout.Zoom);
        }
    }
}