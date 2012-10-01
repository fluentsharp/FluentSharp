using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.Text.Editor;
using O2.DotNetWrappers.ExtensionMethods;
using System.Windows.Controls;

namespace O2.FluentSharp.VisualStudio.Packages
{
    [Export(typeof(IWpfTextViewMarginProvider))]
    [Name(EditorMargin.MarginName)]
    [Order(After = PredefinedMarginNames.HorizontalScrollBar)] //Ensure that the margin occurs below the horizontal scrollbar
    [MarginContainer(PredefinedMarginNames.Bottom)] //Set the container to the bottom of the editor window
    [ContentType("text")] //Show this margin for all text-based types
    [TextViewRole(PredefinedTextViewRoles.Interactive)]

    public sealed class Package_IWpfTextViewMarginProvider : IWpfTextViewMarginProvider
    {
        public static IWpfTextViewHost   IWpfTextViewHost { get; set; }        
        public static IWpfTextViewMargin IWpfTextViewMargin_Container { get; set; }
        public static EditorMargin       IWpfTextViewMargin_New { get; set; }
        public static List<Action>       On_IWpfTextViewMarginProvider { get; set; }
        
        public Package_IWpfTextViewMarginProvider()
        {
            "In WpfTextViewMarginProvider ctor".info();            
        }
        
        public IWpfTextViewMargin CreateMargin(IWpfTextViewHost textViewHost, IWpfTextViewMargin containerMargin)
        {
            "In WpfTextViewMarginProvider CreateMargin".info();
            IWpfTextViewHost = textViewHost;
            IWpfTextViewMargin_Container = containerMargin;
            

            var IWpfTextViewMargin_New  = new EditorMargin(textViewHost.TextView);

            return IWpfTextViewMargin_New; 
        }
    }

    public class EditorMargin : Canvas, IWpfTextViewMargin
    {
        public const string MarginName = "EditorMargin";
        public IWpfTextView _textView;
        private bool _isDisposed = false;

        public EditorMargin(IWpfTextView textView)
        {
            _textView = textView;
        }

        #region IWpfTextViewMargin Members

        private void ThrowIfDisposed()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(MarginName);
        }


        /// <summary>
        /// The <see cref="Sytem.Windows.FrameworkElement"/> that implements the visual representation
        /// of the margin.
        /// </summary>
        public System.Windows.FrameworkElement VisualElement
        {
            // Since this margin implements Canvas, this is the object which renders
            // the margin.
            get
            {
                ThrowIfDisposed();
                return this;
            }
        }

        #endregion

        #region ITextViewMargin Members

        public double MarginSize
        {
            // Since this is a horizontal margin, its width will be bound to the width of the text view.
            // Therefore, its size is its height.
            get
            {
                ThrowIfDisposed();
                return this.ActualHeight;
            }
        }

        public bool Enabled
        {
            // The margin should always be enabled
            get
            {
                ThrowIfDisposed();
                return true;
            }
        }

        /// <summary>
        /// Returns an instance of the margin if this is the margin that has been requested.
        /// </summary>
        /// <param name="marginName">The name of the margin requested</param>
        /// <returns>An instance of EditorMargin2 or null</returns>
        public ITextViewMargin GetTextViewMargin(string marginName)
        {
            return (marginName == EditorMargin.MarginName) ? (IWpfTextViewMargin)this : null;
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                GC.SuppressFinalize(this);
                _isDisposed = true;
            }
        }
        #endregion
    }


}
