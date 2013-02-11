// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using O2.Interfaces.CIR;

namespace O2.Interfaces.DotNet
{
    public interface IGacDll
    {
        string Name         { get;set;}
        string Version      { get; set; }
        string FullPath     { get; set; }
        ICirData CirData    { get; set; }
        PostSharpHookStatus PostSharpHooks { get; set; }
    }
}