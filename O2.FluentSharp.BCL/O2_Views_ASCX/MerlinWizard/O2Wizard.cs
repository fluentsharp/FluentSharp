using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using O2.Kernel;
using O2.Views.ASCX.CoreControls;
//O2Ref:Merlin.dll
using Merlin;
using O2.DotNetWrappers.DotNet;
using O2.DotNetWrappers.Windows;
using System.Threading;

namespace O2.Views.ASCX.MerlinWizard
{
    public class O2WizardAttribute : Attribute
    {    
    }
    public class StartWizardAttribute : Attribute
    {
    }

    public class O2Wizard
    {
        public string Title {get;set;}
        public int Width {get;set;}
        public int Height {get;set;}
        public List<IStep> Steps { get; set; }
        public Object Model;                // this is starting to be redundant since just about no Wizard script uses it
        public Type ModelType;

        public O2Wizard()
        {
            Title = "Default O2 Wizard";
            Width = -1;
            Height = -1;
            Steps = new List<IStep>();
        }
        public O2Wizard(string title) : this()
        {
            Title = title;
        }
        
        public O2Wizard(string title, int width, int height) : this(title)
        {
            Width = width;
            Height = height;
        }
        
        public O2Wizard(string title, object model) : this(title)
        {
        	Model = model;
        }

        public void setModel(object model)
        {
            if (model != null)
            {
                Model = model;
                ModelType = model.GetType();
            }
        }

        public Thread run()
        {
            return start();
        }
        public Thread start()
        {
        	// make sure all steps have access to the Model
        	foreach(var step in Steps)
        		step.Model = Model;
            return MerlinUtils.runWizardWithSteps(Steps, Title, Width, Height);
        }

        



    }
}
