using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace FluentSharp.WPF
{
    public static class Animation_ExtensionMethods
    {
        public static T rotate<T>(this T uiElement)
            where T : UIElement
        {
            return uiElement.rotate(0, 360, 3, false);
        }

        public static T rotate<T>(this T uiElement, bool loopAnimation)
            where T : UIElement
        {
            return uiElement.rotate(0, 360, 3, loopAnimation);
        }

        public static T rotate<T>(this T uiElement, double fromValue, double toValue, int durationInSeconds, bool loopAnimation)
            where T : UIElement
        {
            return (T)uiElement.wpfInvoke(
                () =>
                    {
                        DoubleAnimation doubleAnimation = new DoubleAnimation(fromValue, toValue, new Duration(TimeSpan.FromSeconds(durationInSeconds)));
                        RotateTransform rotateTransform = new RotateTransform();
                        uiElement.RenderTransform = rotateTransform;
                        uiElement.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);
                        if (loopAnimation)
                            doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
                        rotateTransform.BeginAnimation(RotateTransform.AngleProperty, doubleAnimation);
                        return uiElement;
                    });
        }

        public static T fadeIn<T>(this T uiElement, int durationInSeconds)
            where T : UIElement
        {
            return uiElement.fadeFromTo(0, 1, durationInSeconds, false);
        }

        public static T fadeOut<T>(this T uiElement, int durationInSeconds)
            where T : UIElement
        {
            return uiElement.fadeFromTo(1, 0, durationInSeconds, false);
        }

        public static T fadeFromTo<T>(this T uiElement, double fromOpacity, double toOpacity, int durationInSeconds, bool loopAnimation)
            where T : UIElement
        {
            return (T)uiElement.wpfInvoke(() =>
                {
                    var doubleAnimation = new DoubleAnimation(fromOpacity, toOpacity, new Duration(TimeSpan.FromSeconds(durationInSeconds)));
                    if (loopAnimation)
                        doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
                    uiElement.BeginAnimation(UIElement.OpacityProperty, doubleAnimation);
                    return uiElement;
                });
        }     
    }
}