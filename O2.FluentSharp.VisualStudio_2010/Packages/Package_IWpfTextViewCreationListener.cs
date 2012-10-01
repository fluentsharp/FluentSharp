using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.Text.Editor;
using O2.DotNetWrappers.ExtensionMethods;

namespace O2.FluentSharp.VisualStudio.Packages
{
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    public class Package_IWpfTextViewCreationListener : IWpfTextViewCreationListener
    {
        [Export(typeof(AdornmentLayerDefinition))]
        [Name("TextAdornment")]
        [Order(After = PredefinedAdornmentLayers.Selection, Before = PredefinedAdornmentLayers.Text)]
        [TextViewRole(PredefinedTextViewRoles.Document)]

        public static List<Action<IWpfTextView>> On_TextViewCreated { get; set; }
        public static IWpfTextView               Last_IWpfTextView  { get;set; }
        //public AdornmentLayerDefinition editorAdornmentLayer = null;

        static Package_IWpfTextViewCreationListener()
        {
            "In WpfTextViewMarginProvider static ctor".info();
            On_TextViewCreated = new List<Action<IWpfTextView>>();
        }
        
        public void TextViewCreated(IWpfTextView textView)
        {
            "In WpfTextViewMarginProvider TextViewCreated".info();
            Last_IWpfTextView = textView;
            On_TextViewCreated.invoke(textView);            
        }
    }
}
