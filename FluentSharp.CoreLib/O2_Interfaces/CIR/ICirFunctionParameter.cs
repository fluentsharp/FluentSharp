// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
namespace FluentSharp.CoreLib.Interfaces
{
    public interface ICirFunctionParameter
    {
        string ParameterName { get; set; }
        string ParameterType { get; set; }
        string Constant { get; set; }
        bool HasConstant { get; set; }
        bool HasDefault { get; set; }
        string Method { get; set; }
        bool IsTainted { get; set; }
    }
}