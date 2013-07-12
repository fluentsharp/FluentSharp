using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;

namespace FluentSharp.WPF
{
    public static class Image_ExtensionMethods
    {
	
        #region Image

        public static Image open(this Image image, string imageLocation)
        {
            return image.open(imageLocation.uri(), -1, -1);
        }

        public static Image open(this Image image, string imageLocation, int width, int height)
        {
            return image.open(imageLocation.uri(), width, height);
        }

        public static Image open(this Image image, Uri imageLocation)
        {
            return image.open(imageLocation, -1, -1);
        }

        public static Image open(this Image image, Uri imageLocation, int width, int height)
        {
            return (Image)image.wpfInvoke(
                () =>
                    {
                        if (imageLocation.notNull())
                        {
                            var bitmap = new BitmapImage(imageLocation);
                            image.Source = bitmap;
                            if (width > -1)
                            {
                                image.width_Wpf<Image>((double)width);
                            }
                            if (height > -1)
                            {
                                image.height_Wpf<Image>((double)height);
                            }
                        }
                        return image;
                    });
        }


        public static Image add_Image_Wpf<T>(this  T uiElement)
            where T : UIElement
        {
            return (Image)uiElement.wpfInvoke(
                () =>
                    {
                        return uiElement.add_Control_Wpf<Image>();
                    });
        }

        public static Image add_Image_Wpf<T>(this  T uiElement, string pathToImage)
            where T : UIElement
        {
            return uiElement.add_Image_Wpf(pathToImage, -1, -1);
        }

        public static Image add_Image_Wpf<T>(this  T uiElement, string pathToImage, int width, int height)
            where T : UIElement
        {
            return (Image)uiElement.wpfInvoke(
                () =>
                    {
                        var image = pathToImage.image_Wpf(width, height);
                        if (image.notNull())
                            uiElement.add_Control_Wpf(image);
                        return image;
                    });
        }

        public static Image image_Wpf(this string pathToImage)
        {
            return pathToImage.image_Wpf(-1, -1);
        }

        public static Image image_Wpf(this string pathToImage, int width, int height)
        {
            try
            {
                var image = new Image().open(pathToImage);
                if (width > -1)
                    image.width_Wpf(width);
                if (height > -1)
                    image.height_Wpf(height);
                return image;
            }
            catch (Exception ex)
            {
                ex.log("in pathToImage image_Wpf");
                return null;
            }
        }
        public static List<Image> images_Wpf(this List<string> pathToImages)
        {
            return pathToImages.images_Wpf(-1, -1);
        }

        public static List<Image> images_Wpf(this List<string> pathToImages, int width, int height)
        {
            var images = new List<Image>();
            foreach (var pathToImage in pathToImages)
            {
                var image = pathToImage.image_Wpf(width, height);
                if (image.notNull())
                    images.Add(image);
            }
            return images;
        }

        public static Image show(this Image targetImage, Image sourceImage)
        {
            return (Image)targetImage.wpfInvoke(
                () =>
                    {
                        targetImage.Source = sourceImage.Source;
                        return targetImage;
                    });
        }

        public static List<string> saveAs_Gifs(this List<Image> images)
        {
            var files = new List<string>();
            foreach (var image in images)
            {
                var file = image.saveAs_Gif();
                if (file.valid())
                    files.Add(file);
            }
            return files;
        }

        public static string saveAs_Gif(this Image image)
        {
            return image.saveAs_Gif(PublicDI.config.getTempFileInTempDirectory(".gif"));
        }
        
        public static string saveAs_Gif(this Image image, string pathToSaveImage)
        {
            return (string)image.wpfInvoke(
                () =>
                    {
                        try
                        {
                            using (FileStream outStream = new FileStream(pathToSaveImage, FileMode.Create))
                            {
                                var gifBitmapEncoder = new GifBitmapEncoder();
                                gifBitmapEncoder.Frames.Add(BitmapFrame.Create((BitmapImage)image.Source));//BitmapFrame.Create(image));

                                gifBitmapEncoder.Save(outStream);
                            }
                        }
                        catch (Exception ex)
                        {
                            ex.log("in WPF Image saveas_Gif");
                        }
                        if (pathToSaveImage.fileExists())
                            return pathToSaveImage;
                        return "";
                    });


        }

        #endregion

    }
}