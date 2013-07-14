using System;
using System.ComponentModel;

namespace FluentSharp.WinForms
{
    public static class WinForms_ExtensionMethods_Component
    {
        public static T onDisposed<T>(this T component, Action onDisposed)  
            where T : Component
        {
            component.Disposed += (sender, e) => onDisposed();
            return component;
        }
    }
}