using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;

namespace FluentSharp.WinForms
{
    public static class BitMap_ExtensionMethods
    {
        public static Bitmap    bitmap(this string file)
        {
            if (file.fileExists())
                return new Bitmap(file);
            return null;
        }
        public static Bitmap    asBitmap(this Image image)
        {
            return image as Bitmap;				
        }
        public static string md5Hash(this Bitmap bitmap)
        {
            try
            {
                if (bitmap.isNull())
                    return null;
                //based on code snippets from http://dotnet.itags.org/dotnet-c-sharp/85838/
                using (var strm = new MemoryStream())
                {
                    var image = new Bitmap(bitmap);
                    bitmap.Save(strm, System.Drawing.Imaging.ImageFormat.Bmp);
                    strm.Seek(0, 0);
                    byte[] bytes = strm.ToArray();
                    var md5 = new MD5CryptoServiceProvider();
                    byte[] hashed = md5.TransformFinalBlock(bytes, 0, bytes.Length);
                    string hash = BitConverter.ToString(hashed).ToLower();
                    md5.Clear();
                    image.Dispose();
                    return hash;
                }
            }
            catch (Exception ex)
            {
                ex.log("in bitmap.md5Hash");
                return "";
            }
        }
        public static bool isNotEqualTo(this Bitmap bitmap1, Bitmap bitmap2)
        {
            return bitmap1.isEqualTo(bitmap2).isFalse();
        }
        public static bool isEqualTo(this Bitmap bitmap1, Bitmap bitmap2)
        {
            var md5Hash1 = bitmap1.md5Hash();
            var md5Hash2 = bitmap2.md5Hash();
            if (md5Hash1.valid() && md5Hash2.valid())
                return md5Hash1 == md5Hash2;
            
            "in Bitmap.isEqualTo at least one of the calculated MD5 Hashes was not valid".error();
            return false;
        }	
	
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
                    gfx.SmoothingMode = SmoothingMode.HighQuality;
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
    }
}