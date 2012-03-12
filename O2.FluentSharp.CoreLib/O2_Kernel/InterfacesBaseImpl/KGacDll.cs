// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using O2.Interfaces.CIR;
using O2.Interfaces.DotNet;

namespace O2.Kernel.InterfacesBaseImpl
{
    public class KGacDll : IGacDll
    {
        public string name { get; set; }
        public string version { get; set; }
        public string fullPath { get; set; }
        public ICirData cirData { get; set; }
        public PostSharpHookStatus PostSharpHooks { get; set; }

        public KGacDll(string _name, string _version, string _fullPath)
        {
            name = _name;
            version = _version;
            fullPath = _fullPath;
            cirData = null;
            PostSharpHooks = PostSharpHookStatus.NotCalculated;
        }

        public override string ToString()
        {
            return name;
        }

    }
}
