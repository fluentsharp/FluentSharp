// Guids.cs
// MUST match guids.h
using System;

namespace O2.FluentSharp.VSIX
{
    static class GuidList
    {
        public const string guidO2_FluentSharp_VSIXPkgString = "b9f846b6-99cc-43f3-9ec5-5b5ad8f73e1f";
        public const string guidO2_FluentSharp_VSIXCmdSetString = "7546e1b7-92e4-4e9d-83b7-021547507e41";
        public const string guidToolWindowPersistanceString = "bbe3bf58-bd64-4e05-ac03-d00f1dedc3e5";

        public static readonly Guid guidO2_FluentSharp_VSIXCmdSet = new Guid(guidO2_FluentSharp_VSIXCmdSetString);
    };
}