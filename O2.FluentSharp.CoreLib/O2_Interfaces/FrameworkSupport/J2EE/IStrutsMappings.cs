// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;

namespace FluentSharp.CoreLib.Interfaces.J2EE
{
    public interface IStrutsMappings
    {
        List<IStrutsMappings_ActionServlet> actionServlets { get; set; }
        List<string> otherServlets { get; set; }
    }

    public interface IStrutsMappings_ActionServlet
    {
        string name { get; set; }
        string loadOnStartUp { get; set; }
        string urlPattern { get; set; }        
        List<string> configFiles { get; set; }
        List<string> chainConfigFiles { get; set; } 
        string debug { get; set; }
        string detail { get; set; }
        string validate { get; set; }
        string ruleSets { get; set; }
        string application { get; set; }
        List<IStrutsMappings_Filter> filters { get; set; }
        Dictionary<string, IStrutsMappings_Controller> controllers { get; set; }
        Dictionary<string, IStrutsConfig_FormBean> formsBeans { get; set; }             
    }

    public interface IStrutsMappings_Filter
    {
        string name { get; set; }        
        string urlPattern { get; set; }
        string @class { get; set; }
        string dispatcher { get; set; }
    }

    public interface IStrutsMappings_Controller
    {
        string name { get; set; }
        string type { get; set; }
        IStrutsConfig_FormBean formBean { get; set; }
        List<IStrutsMappings_Controller_Path> paths { get; set; }        
    }

    public interface IStrutsMappings_Controller_Path
    {
        string path { get; set; }
        string validate { get; set; }
        string input { get; set; }
        List<string> resolvedViews { get; set; }
        List<string> notResolvedViews { get; set; }
        List<IStrutsConfig_Action_Forward> forwards { get; set; }
    }

    [Serializable]
    public class KStrutsMappings_Controller_Path : IStrutsMappings_Controller_Path
    {
        public string path { get; set; }
        public string validate { get; set; }
        public string input { get; set; }
        public List<IStrutsConfig_Action_Forward> forwards { get; set; }
        public List<string> resolvedViews { get; set; }
        public List<string> notResolvedViews { get; set; }
        
        public KStrutsMappings_Controller_Path()
        {
            forwards = new List<IStrutsConfig_Action_Forward>();
            resolvedViews = new List<string>();
            notResolvedViews = new List<string>();
        }
    }

    [Serializable]
    public class KStrutsMappings_Controller : IStrutsMappings_Controller
    {
        public string name { get; set; }
        public string type { get; set; }
        public IStrutsConfig_FormBean formBean { get; set; }
        public List<IStrutsMappings_Controller_Path> paths { get; set; }

        public KStrutsMappings_Controller()
        {
            paths = new List<IStrutsMappings_Controller_Path>();
        }
    }

    [Serializable]
    public class KStrutsMappings : IStrutsMappings
    {
        public List<IStrutsMappings_ActionServlet> actionServlets { get; set; }
        public List<string> otherServlets { get; set; }

        public KStrutsMappings()
        {
            actionServlets = new List<IStrutsMappings_ActionServlet>();
            otherServlets = new List<string>();
        }
    }

    [Serializable]
    public class KStrutsMappings_ActionServlet : IStrutsMappings_ActionServlet
    {
        public string name { get; set; }
        public string loadOnStartUp { get; set; }
        public string urlPattern { get; set; }
        public List<string> configFiles { get; set; }
        public List<string> chainConfigFiles { get; set; }        
        public string debug { get; set; }
        public string detail { get; set; }
        public string validate { get; set; }
        public string ruleSets { get; set; }
        public string application { get; set; }
        public List<IStrutsMappings_Filter> filters { get; set; }
        public Dictionary<string,IStrutsMappings_Controller> controllers { get; set; }
        public Dictionary<string, string> resolvedViews { get; set; }
        public Dictionary<string, IStrutsConfig_FormBean> formsBeans { get; set; }    

        public KStrutsMappings_ActionServlet()
        {
            configFiles = new List<string>();
            chainConfigFiles = new List<string>();
            filters = new List<IStrutsMappings_Filter>();
            controllers = new Dictionary<string,IStrutsMappings_Controller>();
            resolvedViews = new Dictionary<string, string>();
            formsBeans = new Dictionary<string, IStrutsConfig_FormBean>();
        }

        public override string ToString()
        {
            return string.Format("Struts Action: name={0} , urlPattern={1}", name, urlPattern);
        }
    }

    [Serializable]
    public class KStrutsMappings_Filter : IStrutsMappings_Filter
    {
        public string name { get; set; }
        public string urlPattern { get; set; }
        public string @class { get; set; }
        public string dispatcher { get; set; }
    }
}