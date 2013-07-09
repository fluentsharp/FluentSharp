using System.Drawing;
using FluentSharp.BCL;
using NUnit.Framework;
using FluentSharp.CoreLib;

namespace UnitTests.FluentSharp_BCL
{
    [TestFixture]
    public class Test_Ex_Forms
    {
        [Test]
        public void AsIconAsBitmap()
        {
            var image   = "folder".formImage();
            var bitmap = image.asBitmap();
            var iconFromImage = image.asIcon();
            var iconFromBitmap = bitmap.asIcon();

            Assert.NotNull(image);
            Assert.NotNull(bitmap);
            Assert.NotNull(iconFromImage);
            Assert.NotNull(iconFromBitmap);

            Assert.IsInstanceOf<Image >(image);
            Assert.IsInstanceOf<Bitmap>(bitmap);
            Assert.IsInstanceOf<Icon>  (iconFromImage);
            Assert.IsInstanceOf<Icon>  (iconFromBitmap);
        }
    }
}
