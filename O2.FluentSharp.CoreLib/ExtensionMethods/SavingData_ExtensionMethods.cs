using System;
using System.Drawing;
using System.Drawing.Imaging;

using O2.Kernel;
using System.Drawing.Drawing2D;

namespace FluentSharp.ExtensionMethods
{
    public static class SavingData_ExtensionMethods
    {
        #region bitmap

        public static string save(this Bitmap bitmap)
        {
            return bitmap.save(PublicDI.config.getTempFileInTempDirectory("jpeg"),ImageFormat.Jpeg);
        }

        public static string save(this Bitmap bitmap,string targetFile)
        {
            return bitmap.save(targetFile, ImageFormat.Jpeg);
        }

        public static string save(this Bitmap bitmap, string targetFile,  ImageFormat imageFormat)
        {
            try
            {
                bitmap.Save(targetFile, imageFormat);
                return targetFile;
            }
            catch (Exception ex)
            {
                ex.log("in Bitmap.save");
                return null;
            }        
        }

        public static string gif(this Bitmap bitmap)
        {
            var tempGif = PublicDI.config.getTempFileInTempDirectory(".gif");
            return bitmap.save(tempGif, ImageFormat.Gif);
        }

        public static string jpg(this Bitmap bitmap)
        {
            return bitmap.jpeg();
        }

        public static string jpeg(this Bitmap bitmap)
        {
            var tempGif = PublicDI.config.getTempFileInTempDirectory(".jpeg");
            return bitmap.save(tempGif, ImageFormat.Jpeg);
        }

        public static string png(this Bitmap bitmap)
        {
            var tempGif = PublicDI.config.getTempFileInTempDirectory(".png");
            return bitmap.save(tempGif, ImageFormat.Png);
        }

        public static string icon(this Bitmap bitmap)
        {
            var tempGif = PublicDI.config.getTempFileInTempDirectory(".icon");
            return bitmap.save(tempGif, ImageFormat.Icon);
        }


        public static Bitmap thumbnail(this Bitmap bitmap)
        {
            return bitmap.resize(50, 50);
        }

        public static Bitmap resize(this Bitmap bitmap, int newWidth, int newHeight)
        {
            try
            {
                var bitmapRect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                var thumbnail = new Bitmap(newWidth, newHeight);
                using (Graphics gfx = Graphics.FromImage(thumbnail))
                {
                    // high quality image sizing
                    gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;                                                                       // make it look pretty 
                    gfx.DrawImage(bitmap, new Rectangle(0, 0, newWidth, newHeight), bitmapRect, GraphicsUnit.Pixel);
                }
                bitmap.Dispose();
                return thumbnail;

            }
            catch (Exception ex)
            {
                ex.log("in Bitmap.resize");
                return null;
            }        
        }
        
        /*public static void show(this Bitmap bitmap)
        {
            O2.Kernel.show.image(bitmap);
        }*/
        
        #endregion 
    }
}
