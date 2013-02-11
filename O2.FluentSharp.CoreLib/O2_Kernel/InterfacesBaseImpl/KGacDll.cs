// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using O2.Interfaces.CIR;
using O2.Interfaces.DotNet;

namespace O2.Kernel.InterfacesBaseImpl
{
    public class KGacDll : IGacDll
    {
        public string               Name { get; set; }
        public string               Version { get; set; }
        public string               FullPath { get; set; }
        public ICirData             CirData { get; set; }
        public PostSharpHookStatus  PostSharpHooks { get; set; }

        public KGacDll(string name, string version, string fullPath)
        {
            Name = name;
            Version = version;
            FullPath = fullPath;
            CirData = null;
            PostSharpHooks = PostSharpHookStatus.NotCalculated;
        }

        public override string ToString()
        {
            return Name;
        }

    }
}
